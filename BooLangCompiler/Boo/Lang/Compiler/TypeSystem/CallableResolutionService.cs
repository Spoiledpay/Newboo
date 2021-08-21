using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class CallableResolutionService : AbstractCompilerComponent
	{
		public class Candidate : IEquatable<Candidate>
		{
			public IMethod Method;

			private CallableResolutionService _crs;

			private int[] _scores;

			private bool _expanded;

			public IParameter[] Parameters => Method.GetParameters();

			public int[] ArgumentScores => _scores;

			public bool Expanded
			{
				get
				{
					return _expanded;
				}
				set
				{
					_expanded = value;
				}
			}

			public Candidate(CallableResolutionService crs, IMethod entity)
			{
				_crs = crs;
				Method = entity;
				_scores = new int[crs._arguments.Count];
			}

			public int Score(int argumentIndex)
			{
				int num = _crs.CalculateArgumentScore(Parameters[argumentIndex], Parameters[argumentIndex].Type, _crs.GetArgument(argumentIndex));
				_scores[argumentIndex] = num;
				return num;
			}

			public int ScoreVarArgs(int argumentIndex)
			{
				IParameter parameter = Parameters[Parameters.Length - 1];
				_scores[argumentIndex] = _crs.CalculateArgumentScore(parameter, parameter.Type.ElementType, _crs.GetArgument(argumentIndex));
				return _scores[argumentIndex];
			}

			public override int GetHashCode()
			{
				return Method.GetHashCode();
			}

			public override bool Equals(object other)
			{
				return Equals(other as Candidate);
			}

			public bool Equals(Candidate other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				return Method == other.Method;
			}

			public override string ToString()
			{
				return Method.ToString();
			}
		}

		protected const int CallableExactMatchScore = 10;

		protected const int CallableUpCastScore = 9;

		protected const int CallableImplicitConversionScore = 8;

		protected const int ExactMatchScore = 8;

		protected const int UpCastScore = 7;

		protected const int WideningPromotion = 6;

		protected const int ImplicitConversionScore = 5;

		protected const int NarrowingPromotion = 4;

		protected const int DowncastScore = 3;

		protected List<Candidate> _candidates = new List<Candidate>();

		protected ExpressionCollection _arguments;

		private DowncastPermissions _downcastPermissions;

		private readonly MemoizedFunction<IType, IType, int> _calculateArgumentScore;

		public IList<Candidate> ValidCandidates => _candidates;

		public CallableResolutionService()
			: base(CompilerContext.Current)
		{
			_calculateArgumentScore = new MemoizedFunction<IType, IType, int>(CalculateArgumentScore);
			CurrentScope instance = My<CurrentScope>.Instance;
			EventHandler value = delegate
			{
				_calculateArgumentScore.Clear();
			};
			instance.Changed += value;
		}

		protected Expression GetArgument(int index)
		{
			return _arguments[index];
		}

		public int GetLogicalTypeDepth(IType type)
		{
			int typeDepth = type.GetTypeDepth();
			if (type.IsValueType)
			{
				return typeDepth - 1;
			}
			return typeDepth;
		}

		protected IType ArgumentType(Expression e)
		{
			return TypeSystemServices.GetExpressionType(e);
		}

		public bool IsValidByRefArg(IParameter param, IType parameterType, IType argType, Node arg)
		{
			if ((parameterType.IsByRef && argType == parameterType.ElementType) || (param.IsByRef && argType == parameterType))
			{
				return CanLoadAddress(arg);
			}
			return false;
		}

		private static bool CanLoadAddress(Node node)
		{
			IEntity entity = node.Entity;
			if (entity == null || node is SelfLiteralExpression)
			{
				return true;
			}
			return entity.EntityType switch
			{
				EntityType.Local => !((InternalLocal)entity).IsPrivateScope, 
				EntityType.Parameter => true, 
				EntityType.Field => !TypeSystemServices.IsReadOnlyField((IField)entity), 
				_ => false, 
			};
		}

		public IEntity ResolveCallableReference(ExpressionCollection args, IEntity[] candidates)
		{
			Reset(args, candidates);
			InferGenericMethods();
			FindApplicableCandidates();
			if (ValidCandidates.Count == 0)
			{
				return null;
			}
			if (ValidCandidates.Count == 1)
			{
				return ValidCandidates[0].Method;
			}
			List<Candidate> list = FindDataPreservingCandidates();
			if (list.Count > 0)
			{
				FindBestMethod(list);
				if (list.Count == 1)
				{
					return list[0].Method;
				}
			}
			FindBestMethod(_candidates);
			if (ValidCandidates.Count > 1)
			{
				PreferInternalEntitiesOverNonInternal();
			}
			if (ValidCandidates.Count == 1)
			{
				return ValidCandidates[0].Method;
			}
			return null;
		}

		private void PreferInternalEntitiesOverNonInternal()
		{
			if (!HasInternalCandidate() || !HasNonInternalCandidate())
			{
				return;
			}
			foreach (Candidate nonInternalCandidate in GetNonInternalCandidates())
			{
				ValidCandidates.Remove(nonInternalCandidate);
			}
		}

		private bool HasNonInternalCandidate()
		{
			return ValidCandidates.Any(IsNonInternalCandidate);
		}

		private bool HasInternalCandidate()
		{
			return ValidCandidates.Any(IsInternalCandidate);
		}

		private static bool IsInternalCandidate(Candidate c)
		{
			return EntityPredicates.IsInternalEntity(c.Method);
		}

		private static bool IsNonInternalCandidate(Candidate c)
		{
			return EntityPredicates.IsNonInternalEntity(c.Method);
		}

		private IEnumerable<Candidate> GetNonInternalCandidates()
		{
			return ValidCandidates.Where(IsNonInternalCandidate).ToList();
		}

		private List<Candidate> FindDataPreservingCandidates()
		{
			return _candidates.FindAll(DoesNotRequireConversions);
		}

		private static bool DoesNotRequireConversions(Candidate candidate)
		{
			return !Array.Exists(candidate.ArgumentScores, RequiresConversion);
		}

		private static bool RequiresConversion(int score)
		{
			return score < 6;
		}

		private void FindBestMethod(List<Candidate> candidates)
		{
			candidates.Sort(BetterCandidate);
			Candidate pivot = candidates[candidates.Count - 1];
			candidates.RemoveAll((Candidate candidate) => 0 != BetterCandidate(candidate, pivot));
		}

		private bool ApplicableCandidate(Candidate candidate)
		{
			bool flag = ShouldExpandVarArgs(candidate);
			int num = (flag ? (candidate.Parameters.Length - 1) : candidate.Parameters.Length);
			if (_arguments.Count < num)
			{
				return false;
			}
			if (_arguments.Count > num && !flag)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (candidate.Score(i) < 0)
				{
					return false;
				}
			}
			if (flag)
			{
				candidate.Expanded = true;
				for (int i = num; i < _arguments.Count; i++)
				{
					if (candidate.ScoreVarArgs(i) < 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool ShouldExpandVarArgs(Candidate candidate)
		{
			IMethod method = candidate.Method;
			if (!method.AcceptVarArgs)
			{
				return false;
			}
			if (_arguments.Count == 0)
			{
				return true;
			}
			return ShouldExpandArgs(method, _arguments);
		}

		protected virtual bool ShouldExpandArgs(IMethod method, ExpressionCollection args)
		{
			return args.Count > 0 && !AstUtil.IsExplodeExpression(args[-1]);
		}

		private int TotalScore(Candidate c1)
		{
			int num = 0;
			int[] argumentScores = c1.ArgumentScores;
			foreach (int num2 in argumentScores)
			{
				num += num2;
			}
			return num;
		}

		private int BetterCandidate(Candidate c1, Candidate c2)
		{
			if (c1 == c2)
			{
				return 0;
			}
			int num = Math.Sign(TotalScore(c1) - TotalScore(c2));
			if (num != 0)
			{
				return num;
			}
			num = c1.Method.DeclaringType.GetTypeDepth() - c2.Method.DeclaringType.GetTypeDepth();
			if (num != 0)
			{
				return num;
			}
			num = GenericsServices.GetMethodGenerity(c2.Method) - GenericsServices.GetMethodGenerity(c1.Method);
			if (num != 0)
			{
				return num;
			}
			if (!c1.Expanded && c2.Expanded)
			{
				return 1;
			}
			if (c1.Expanded && !c2.Expanded)
			{
				return -1;
			}
			num = c1.Parameters.Length - c2.Parameters.Length;
			if (num != 0)
			{
				return num;
			}
			return MoreSpecific(c1, c2);
		}

		private int MoreSpecific(Candidate c1, Candidate c2)
		{
			int num = 0;
			for (int i = 0; i < _arguments.Count && i < c1.Parameters.Length; i++)
			{
				if (c1.ArgumentScores[i] <= 3)
				{
					continue;
				}
				if (_arguments[i] is NullLiteralExpression)
				{
					return 0;
				}
				int num2 = MoreSpecific(GetParameterTypeTemplate(c1, i), GetParameterTypeTemplate(c2, i));
				if (num2 != 0)
				{
					if (num == 0)
					{
						num = num2;
					}
					else if (num != num2)
					{
						return 0;
					}
				}
			}
			return num;
		}

		private IType GetParameterTypeTemplate(Candidate candidate, int position)
		{
			IMethod method = candidate.Method;
			if (candidate.Method.DeclaringType.ConstructedInfo != null)
			{
				method = (IMethod)candidate.Method.DeclaringType.ConstructedInfo.UnMap(method);
			}
			if (candidate.Method.ConstructedInfo != null)
			{
				method = candidate.Method.ConstructedInfo.GenericDefinition;
			}
			IParameter[] parameters = method.GetParameters();
			if (candidate.Expanded && position >= parameters.Length - 1)
			{
				return parameters[parameters.Length - 1].Type.ElementType;
			}
			return parameters[position].Type;
		}

		private int MoreSpecific(IType t1, IType t2)
		{
			if ((t1.IsArray && t2.IsArray) || (t1.IsByRef && t2.IsByRef))
			{
				return MoreSpecific(t1.ElementType, t2.ElementType);
			}
			int num = GenericsServices.GetTypeGenerity(t2) - GenericsServices.GetTypeGenerity(t1);
			if (num != 0)
			{
				return num;
			}
			return GetLogicalTypeDepth(t1) - GetLogicalTypeDepth(t2);
		}

		private void InferGenericMethods()
		{
			GenericsServices instance = My<GenericsServices>.Instance;
			foreach (Candidate candidate in _candidates)
			{
				if (candidate.Method.GenericInfo != null)
				{
					IType[] array = instance.InferMethodGenericArguments(candidate.Method, _arguments);
					if (array != null && instance.CheckGenericConstruction(candidate.Method, array))
					{
						candidate.Method = candidate.Method.GenericInfo.ConstructMethod(array);
					}
				}
			}
		}

		private void FindApplicableCandidates()
		{
			_candidates = _candidates.FindAll(ApplicableCandidate);
		}

		private void Reset(ExpressionCollection arguments, IEnumerable<IEntity> candidateEntities)
		{
			_arguments = arguments;
			InitializeCandidates(candidateEntities);
		}

		private void InitializeCandidates(IEnumerable<IEntity> candidateEntities)
		{
			_candidates.Clear();
			foreach (IEntity candidateEntity in candidateEntities)
			{
				IMethod method = candidateEntity as IMethod;
				if (null != method)
				{
					Candidate item = new Candidate(this, method);
					_candidates.Add(item);
				}
			}
		}

		public bool IsValidVargsInvocation(IParameter[] parameters, ExpressionCollection args)
		{
			int num = parameters.Length - 1;
			if (args.Count < num)
			{
				return false;
			}
			if (!parameters[num].Type.IsArray)
			{
				return false;
			}
			if (!IsValidInvocation(parameters, args, num))
			{
				return false;
			}
			if (args.Count > 0)
			{
				return CheckVarArgsParameter(parameters, args);
			}
			return true;
		}

		protected virtual bool CheckVarArgsParameter(IParameter[] parameters, ExpressionCollection args)
		{
			int num = parameters.Length - 1;
			Expression expression = args[-1];
			if (AstUtil.IsExplodeExpression(expression))
			{
				return CalculateArgumentScore(parameters[num], parameters[num].Type, expression) > 0;
			}
			IType elementType = parameters[num].Type.ElementType;
			for (int i = num; i < args.Count; i++)
			{
				int num2 = CalculateArgumentScore(parameters[num], elementType, args[i]);
				if (num2 < 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsValidInvocation(IParameter[] parameters, ExpressionCollection args, int count)
		{
			for (int i = 0; i < count; i++)
			{
				IParameter parameter = parameters[i];
				IType type = parameter.Type;
				int num = CalculateArgumentScore(parameter, type, args[i]);
				if (num < 0)
				{
					return false;
				}
			}
			return true;
		}

		protected int CalculateArgumentScore(IParameter param, IType parameterType, Expression arg)
		{
			IType type = ArgumentType(arg);
			if (param.IsByRef)
			{
				return CalculateByRefArgumentScore(arg, param, parameterType, type);
			}
			return _calculateArgumentScore.Invoke(parameterType, type);
		}

		private int CalculateByRefArgumentScore(Node arg, IParameter param, IType parameterType, IType argumentType)
		{
			return IsValidByRefArg(param, parameterType, argumentType, arg) ? 8 : (-1);
		}

		private int CalculateArgumentScore(IType parameterType, IType argumentType)
		{
			if (parameterType == argumentType || (base.TypeSystemServices.IsSystemObject(argumentType) && base.TypeSystemServices.IsSystemObject(parameterType)))
			{
				return (parameterType is ICallableType) ? 10 : 8;
			}
			if (TypeCompatibilityRules.IsAssignableFrom(parameterType, argumentType))
			{
				ICallableType callableType = parameterType as ICallableType;
				ICallableType callableType2 = argumentType as ICallableType;
				if (callableType != null && callableType2 != null)
				{
					return CalculateCallableScore(callableType, callableType2);
				}
				return 7;
			}
			if (base.TypeSystemServices.FindImplicitConversionOperator(argumentType, parameterType) != null)
			{
				return 5;
			}
			if (base.TypeSystemServices.CanBeReachedByPromotion(parameterType, argumentType))
			{
				return IsWideningPromotion(parameterType, argumentType) ? 6 : 4;
			}
			if (MyDowncastPermissions().CanBeReachedByDowncast(parameterType, argumentType))
			{
				return 3;
			}
			return -1;
		}

		private DowncastPermissions MyDowncastPermissions()
		{
			return _downcastPermissions ?? (_downcastPermissions = My<DowncastPermissions>.Instance);
		}

		private bool IsWideningPromotion(IType paramType, IType argumentType)
		{
			ExternalType externalType = paramType as ExternalType;
			if (null == externalType)
			{
				return false;
			}
			ExternalType externalType2 = argumentType as ExternalType;
			if (null == externalType2)
			{
				return false;
			}
			return NumericTypes.IsWideningPromotion(externalType.ActualType, externalType2.ActualType);
		}

		private static int CalculateCallableScore(ICallableType parameterType, ICallableType argType)
		{
			CallableSignature signature = parameterType.GetSignature();
			CallableSignature signature2 = argType.GetSignature();
			if (signature.Parameters.Length != signature2.Parameters.Length)
			{
				return 9;
			}
			for (int i = 0; i < signature.Parameters.Length; i++)
			{
				if (signature.Parameters[i].Type != signature2.Parameters[i].Type)
				{
					return 8;
				}
			}
			return (signature.ReturnType == signature2.ReturnType) ? 10 : 9;
		}
	}
}
