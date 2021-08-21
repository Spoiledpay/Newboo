#define TRACE
using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	internal class GenericParameterInferrer : TypeInferrer
	{
		public delegate void ResolveClosureEventHandler(GenericParameterInferrer inferrer, BlockExpression closure, ICallableType formalType);

		private struct TypedArgument
		{
			public Expression Expression;

			public IType FormalType;

			public TypedArgument(Expression expression, IType formalType)
			{
				Expression = expression;
				FormalType = formalType;
			}
		}

		private IMethod _genericMethod;

		private TypedArgument[] _typedArguments;

		private IDictionary<BlockExpression, List<InferredType>> _closureDependencies = new Dictionary<BlockExpression, List<InferredType>>();

		private BlockExpression _currentClosure = null;

		public IMethod GenericMethod => _genericMethod;

		private TypedArgument[] Arguments => _typedArguments;

		public event ResolveClosureEventHandler ResolveClosure;

		public GenericParameterInferrer(CompilerContext context, IMethod genericMethod, ExpressionCollection arguments)
		{
			_genericMethod = genericMethod;
			Initialize(context);
			InitializeArguments(arguments);
			InitializeTypeParameters(GenericMethod.GenericInfo.GenericParameters);
			InitializeDependencies(GenericMethod.GenericInfo.GenericParameters, GenericMethod.CallableType.GetSignature());
			InitializeClosureDependencies();
		}

		private void InitializeClosureDependencies()
		{
			TypedArgument[] arguments = Arguments;
			for (int i = 0; i < arguments.Length; i++)
			{
				TypedArgument typedArgument = arguments[i];
				BlockExpression blockExpression = typedArgument.Expression as BlockExpression;
				if (blockExpression == null)
				{
					continue;
				}
				ICallableType callableType = typedArgument.FormalType as ICallableType;
				if (callableType == null)
				{
					continue;
				}
				TypeCollector typeCollector = new TypeCollector(delegate(IType t)
				{
					IGenericParameter genericParameter2 = t as IGenericParameter;
					return genericParameter2 != null && base.InferredTypes.ContainsKey(genericParameter2);
				});
				IType[] parameterTypes = GetParameterTypes(callableType.GetSignature());
				foreach (IType type in parameterTypes)
				{
					typeCollector.Visit(type);
				}
				foreach (IGenericParameter match in typeCollector.Matches)
				{
					RecordClosureDependency(blockExpression, match);
				}
			}
		}

		private void InitializeArguments(ExpressionCollection arguments)
		{
			_typedArguments = new TypedArgument[arguments.Count];
			CallableSignature signature = GenericMethod.CallableType.GetSignature();
			int num = Math.Min(arguments.Count, signature.Parameters.Length);
			IType type = null;
			for (int i = 0; i < num; i++)
			{
				type = signature.Parameters[i].Type;
				if (GenericMethod.AcceptVarArgs && i == num - 1)
				{
					type = type.ElementType;
				}
				ref TypedArgument reference = ref _typedArguments[i];
				reference = new TypedArgument(arguments[i], type);
			}
			for (int i = num; i < arguments.Count; i++)
			{
				ref TypedArgument reference2 = ref _typedArguments[i];
				reference2 = new TypedArgument(arguments[i], GenericMethod.AcceptVarArgs ? type : null);
			}
		}

		public bool Run()
		{
			InferenceStart();
			if (GenericMethod.AcceptVarArgs)
			{
				if (Arguments.Length < GenericMethod.GetParameters().Length)
				{
					return InferenceComplete(successfully: false);
				}
			}
			else if (Arguments.Length != GenericMethod.GetParameters().Length)
			{
				return InferenceComplete(successfully: false);
			}
			InferExplicits();
			while (HasUnfixedTypes())
			{
				if (!FixAll(HasNoDependencies) && !FixAll(HasDependantsAndBounds))
				{
					return InferenceComplete(successfully: false);
				}
				InferCallables();
			}
			return InferenceComplete(successfully: true);
		}

		private void InferExplicits()
		{
			TypedArgument[] arguments = Arguments;
			for (int i = 0; i < arguments.Length; i++)
			{
				TypedArgument typedArgument = arguments[i];
				_currentClosure = typedArgument.Expression as BlockExpression;
				Infer(typedArgument.FormalType, typedArgument.Expression.ExpressionType, TypeInference.AllowCovariance);
			}
		}

		protected override bool InferGenericParameter(IGenericParameter formalType, IType actualType, TypeInference inference)
		{
			if (_currentClosure != null && actualType == formalType)
			{
				return true;
			}
			return base.InferGenericParameter(formalType, actualType, inference);
		}

		private void RecordClosureDependency(BlockExpression closure, IGenericParameter genericParameter)
		{
			if (!_closureDependencies.ContainsKey(closure))
			{
				_closureDependencies.Add(closure, new List<InferredType>());
			}
			_closureDependencies[closure].AddUnique(base.InferredTypes[genericParameter]);
		}

		private void InferCallables()
		{
			TypedArgument[] arguments = Arguments;
			for (int i = 0; i < arguments.Length; i++)
			{
				TypedArgument typedArgument = arguments[i];
				BlockExpression blockExpression = typedArgument.Expression as BlockExpression;
				if (blockExpression != null && CanResolveClosure(blockExpression))
				{
					ICallableType callableType = (ICallableType)typedArgument.FormalType;
					if (blockExpression.Parameters.Count == callableType.GetSignature().Parameters.Length)
					{
						this.ResolveClosure(this, blockExpression, callableType);
						_closureDependencies.Remove(blockExpression);
						Infer(typedArgument.FormalType, blockExpression.ExpressionType);
					}
				}
			}
		}

		private bool CanResolveClosure(BlockExpression closure)
		{
			if (this.ResolveClosure == null)
			{
				return false;
			}
			if (!_closureDependencies.ContainsKey(closure))
			{
				return false;
			}
			foreach (InferredType item in _closureDependencies[closure])
			{
				if (!item.Fixed)
				{
					return false;
				}
			}
			return true;
		}

		private bool FixAll(Predicate<InferredType> predicate)
		{
			bool flag = false;
			foreach (KeyValuePair<IGenericParameter, InferredType> inferredType in base.InferredTypes)
			{
				IGenericParameter key = inferredType.Key;
				InferredType value = inferredType.Value;
				if (!value.Fixed && predicate(value))
				{
					flag |= Fix(key, value);
				}
			}
			return flag;
		}

		private bool Fix(IGenericParameter genericParameter, InferredType inferredType)
		{
			if (inferredType.Fix())
			{
				base.Context.TraceVerbose("Generic parameter {0} fixed to {1}.", genericParameter.Name, inferredType.ResultingType);
				return true;
			}
			return false;
		}

		private bool HasUnfixedTypes()
		{
			foreach (InferredType value in base.InferredTypes.Values)
			{
				if (!value.Fixed)
				{
					return true;
				}
			}
			return false;
		}

		private bool HasNoDependencies(InferredType inferredType)
		{
			return !inferredType.HasDependencies;
		}

		private bool HasDependantsAndBounds(InferredType inferredType)
		{
			return inferredType.HasDependants && inferredType.HasBounds;
		}

		public IType[] GetInferredTypes()
		{
			return Array.ConvertAll(GenericMethod.GenericInfo.GenericParameters, base.GetInferredType);
		}

		private void InitializeDependencies(IGenericParameter[] genericParameters, CallableSignature signature)
		{
			IType[] parameterTypes = GetParameterTypes(signature);
			IType[] array = parameterTypes;
			foreach (IType type in array)
			{
				ICallableType callableType = type as ICallableType;
				if (callableType != null)
				{
					CalculateDependencies(callableType.GetSignature());
				}
			}
		}

		private void CalculateDependencies(CallableSignature signature)
		{
			IType[] parameterTypes = GetParameterTypes(signature);
			foreach (IType inputType in parameterTypes)
			{
				CalculateDependencies(inputType, signature.ReturnType);
			}
		}

		private void CalculateDependencies(IType inputType, IType outputType)
		{
			foreach (IGenericParameter item in FindGenericParameters(outputType))
			{
				foreach (IGenericParameter item2 in FindGenericParameters(inputType))
				{
					SetDependency(item, item2);
				}
			}
		}

		private IEnumerable<IGenericParameter> FindGenericParameters(IType type)
		{
			foreach (IGenericParameter gp in GenericsServices.FindGenericParameters(type))
			{
				if (base.InferredTypes.ContainsKey(gp))
				{
					yield return gp;
				}
			}
		}

		private void SetDependency(IGenericParameter dependant, IGenericParameter dependee)
		{
			base.InferredTypes[dependant].SetDependencyOn(base.InferredTypes[dependee]);
		}

		private IType[] GetParameterTypes(CallableSignature signature)
		{
			return Array.ConvertAll(signature.Parameters, (IParameter p) => p.Type);
		}

		private void InferenceStart()
		{
			string param = string.Join(", ", Array.ConvertAll(Arguments, (TypedArgument arg) => arg.Expression.ToString()));
			base.Context.TraceVerbose("Attempting to infer generic type arguments for {0} from argument list ({1}).", _genericMethod, param);
		}

		private bool InferenceComplete(bool successfully)
		{
			base.Context.TraceVerbose("Generic type inference for {0} {1}.", _genericMethod, successfully ? "succeeded" : "failed");
			return successfully;
		}
	}
}
