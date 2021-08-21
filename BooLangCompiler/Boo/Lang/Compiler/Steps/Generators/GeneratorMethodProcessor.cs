using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.Generators
{
	internal class GeneratorMethodProcessor : AbstractTransformerCompilerStep
	{
		private sealed class TryStatementInfo
		{
			internal TryStatement _statement;

			internal TryStatementInfo _parent;

			internal bool _containsYield;

			internal int _stateNumber = -1;

			internal Block _replacement;

			internal IMethod _ensureMethod;
		}

		private readonly InternalMethod _generator;

		private InternalMethod _moveNext;

		private readonly BooClassBuilder _enumerable;

		private BooClassBuilder _enumerator;

		private BooMethodBuilder _enumeratorConstructor;

		private BooMethodBuilder _enumerableConstructor;

		private IField _state;

		private IMethod _yield;

		private IMethod _yieldDefault;

		private Field _externalEnumeratorSelf;

		private readonly List<LabelStatement> _labels;

		private readonly System.Collections.Generic.List<TryStatementInfo> _tryStatementInfoForLabels = new System.Collections.Generic.List<TryStatementInfo>();

		private readonly Hashtable _mapping;

		private readonly IType _generatorItemType;

		private readonly BooMethodBuilder _getEnumeratorBuilder;

		private System.Collections.Generic.List<TryStatementInfo> _convertedTryStatements = new System.Collections.Generic.List<TryStatementInfo>();

		private Stack<TryStatementInfo> _tryStatementStack = new Stack<TryStatementInfo>();

		private int _finishedStateNumber;

		private LexicalInfo LexicalInfo => _generator.Method.LexicalInfo;

		protected TypeDefinition DeclaringTypeDefinition => _generator.Method.DeclaringType;

		public GeneratorMethodProcessor(CompilerContext context, InternalMethod method)
		{
			_labels = new List<LabelStatement>();
			_mapping = new Hashtable();
			_generator = method;
			GeneratorSkeleton generatorSkeleton = My<GeneratorSkeletonBuilder>.Instance.SkeletonFor(method);
			_generatorItemType = generatorSkeleton.GeneratorItemType;
			_enumerable = generatorSkeleton.GeneratorClassBuilder;
			_getEnumeratorBuilder = generatorSkeleton.GetEnumeratorBuilder;
			Initialize(context);
		}

		public override void Run()
		{
			CreateEnumerableConstructor();
			CreateEnumerator();
			MethodInvocationExpression enumerableConstructorInvocation = base.CodeBuilder.CreateConstructorInvocation(_enumerable.ClassDefinition);
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateConstructorInvocation(_enumerator.ClassDefinition);
			PropagateReferences(enumerableConstructorInvocation, methodInvocationExpression);
			CreateGetEnumeratorBody(methodInvocationExpression);
			FixGeneratorMethodBody(enumerableConstructorInvocation);
		}

		private void FixGeneratorMethodBody(MethodInvocationExpression enumerableConstructorInvocation)
		{
			Block body = _generator.Method.Body;
			body.Clear();
			body.Add(new ReturnStatement(_generator.Method.LexicalInfo, GeneratorReturnsIEnumerator() ? CreateGetEnumeratorInvocation(enumerableConstructorInvocation) : enumerableConstructorInvocation));
		}

		private void PropagateReferences(MethodInvocationExpression enumerableConstructorInvocation, MethodInvocationExpression enumeratorConstructorInvocation)
		{
			foreach (ParameterDeclaration parameter in _generator.Method.Parameters)
			{
				InternalParameter internalParameter = (InternalParameter)parameter.Entity;
				if (internalParameter.IsUsed)
				{
					enumerableConstructorInvocation.Arguments.Add(base.CodeBuilder.CreateReference(parameter));
					PropagateFromEnumerableToEnumerator(enumeratorConstructorInvocation, internalParameter.Name, internalParameter.Type);
				}
			}
			if (null != _externalEnumeratorSelf)
			{
				IType type = (IType)_externalEnumeratorSelf.Type.Entity;
				enumerableConstructorInvocation.Arguments.Add(base.CodeBuilder.CreateSelfReference(type));
				PropagateFromEnumerableToEnumerator(enumeratorConstructorInvocation, "self_", type);
			}
		}

		private MethodInvocationExpression CreateGetEnumeratorInvocation(MethodInvocationExpression enumerableConstructorInvocation)
		{
			return base.CodeBuilder.CreateMethodInvocation(enumerableConstructorInvocation, GetGetEnumeratorEntity());
		}

		private InternalMethod GetGetEnumeratorEntity()
		{
			return _getEnumeratorBuilder.Entity;
		}

		private bool GeneratorReturnsIEnumerator()
		{
			bool flag = _generator.ReturnType == base.TypeSystemServices.IEnumeratorType;
			return flag | (_generator.ReturnType.ConstructedInfo != null && _generator.ReturnType.ConstructedInfo.GenericDefinition == base.TypeSystemServices.IEnumeratorGenericType);
		}

		private void CreateGetEnumeratorBody(Expression enumeratorExpression)
		{
			_getEnumeratorBuilder.Body.Add(new ReturnStatement(enumeratorExpression));
		}

		private void CreateEnumerableConstructor()
		{
			_enumerableConstructor = CreateConstructor(_enumerable);
		}

		private void CreateEnumeratorConstructor()
		{
			_enumeratorConstructor = CreateConstructor(_enumerator);
		}

		private void CreateEnumerator()
		{
			IType type = base.TypeSystemServices.Map(typeof(GenericGeneratorEnumerator<>)).GenericInfo.ConstructType(_generatorItemType);
			_state = base.NameResolutionService.ResolveField(type, "_state");
			_yield = base.NameResolutionService.ResolveMethod(type, "Yield");
			_yieldDefault = base.NameResolutionService.ResolveMethod(type, "YieldDefault");
			_enumerator = base.CodeBuilder.CreateClass("$");
			_enumerator.AddAttribute(base.CodeBuilder.CreateAttribute(typeof(CompilerGeneratedAttribute)));
			_enumerator.Modifiers |= _enumerable.Modifiers;
			_enumerator.LexicalInfo = LexicalInfo;
			_enumerator.AddBaseType(type);
			_enumerator.AddBaseType(base.TypeSystemServices.IEnumeratorType);
			CreateEnumeratorConstructor();
			CreateMoveNext();
			_enumerable.ClassDefinition.Members.Add(_enumerator.ClassDefinition);
		}

		private void CreateMoveNext()
		{
			Method method = _generator.Method;
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("MoveNext", base.TypeSystemServices.BoolType);
			booMethodBuilder.Method.LexicalInfo = method.LexicalInfo;
			_moveNext = booMethodBuilder.Entity;
			TransformLocalsIntoFields(method);
			TransformParametersIntoFieldsInitializedByConstructor(method);
			booMethodBuilder.Body.Add(CreateLabel(method));
			_finishedStateNumber = _labels.Count;
			LabelStatement stmt = CreateLabel(method);
			booMethodBuilder.Body.Add(method.Body);
			method.Body.Clear();
			Visit(booMethodBuilder.Body);
			booMethodBuilder.Body.Add(CreateYieldInvocation(LexicalInfo.Empty, _finishedStateNumber, null));
			booMethodBuilder.Body.Add(stmt);
			booMethodBuilder.Body.Insert(0, base.CodeBuilder.CreateSwitch(LexicalInfo, base.CodeBuilder.CreateMemberReference(_state), _labels));
			if (_convertedTryStatements.Count > 0)
			{
				IMethod method2 = CreateDisposeMethod();
				TryStatement tryStatement = new TryStatement();
				tryStatement.ProtectedBlock.Add(booMethodBuilder.Body);
				tryStatement.FailureBlock = new Block();
				tryStatement.FailureBlock.Add(CallMethodOnSelf(method2));
				booMethodBuilder.Body.Clear();
				booMethodBuilder.Body.Add(tryStatement);
			}
		}

		private void TransformParametersIntoFieldsInitializedByConstructor(Method generator)
		{
			foreach (ParameterDeclaration parameter in generator.Parameters)
			{
				InternalParameter internalParameter = (InternalParameter)parameter.Entity;
				if (internalParameter.IsUsed)
				{
					Field field = DeclareFieldInitializedFromConstructorParameter(_enumerator, _enumeratorConstructor, internalParameter.Name, internalParameter.Type);
					_mapping[internalParameter] = field.Entity;
				}
			}
		}

		private void TransformLocalsIntoFields(Method generator)
		{
			foreach (Local local in generator.Locals)
			{
				InternalLocal internalLocal = (InternalLocal)local.Entity;
				if (IsExceptionHandlerVariable(internalLocal))
				{
					AddToMoveNextMethod(local);
				}
				else
				{
					AddInternalFieldFor(internalLocal);
				}
			}
			generator.Locals.Clear();
		}

		private void AddToMoveNextMethod(Local local)
		{
			_moveNext.Method.Locals.Add(local);
		}

		private void AddInternalFieldFor(InternalLocal entity)
		{
			Field field = _enumerator.AddInternalField(UniqueName(entity.Name), entity.Type);
			_mapping[entity] = field.Entity;
		}

		private bool IsExceptionHandlerVariable(InternalLocal local)
		{
			Declaration originalDeclaration = local.OriginalDeclaration;
			if (originalDeclaration == null)
			{
				return false;
			}
			return originalDeclaration.ParentNode is ExceptionHandler;
		}

		private MethodInvocationExpression CallMethodOnSelf(IMethod method)
		{
			return base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateSelfReference(_enumerator.Entity), method);
		}

		private IMethod CreateDisposeMethod()
		{
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("Dispose", base.TypeSystemServices.VoidType);
			booMethodBuilder.Method.LexicalInfo = LexicalInfo;
			LabelStatement labelStatement = base.CodeBuilder.CreateLabel(_generator.Method, "noEnsure").LabelStatement;
			booMethodBuilder.Body.Add(labelStatement);
			booMethodBuilder.Body.Add(SetStateTo(_finishedStateNumber));
			booMethodBuilder.Body.Add(new ReturnStatement());
			LabelStatement[] array = new LabelStatement[_labels.Count];
			for (int i = 0; i < _convertedTryStatements.Count; i++)
			{
				TryStatementInfo tryStatementInfo = _convertedTryStatements[i];
				array[tryStatementInfo._stateNumber] = base.CodeBuilder.CreateLabel(_generator.Method, "$ensure_" + tryStatementInfo._stateNumber).LabelStatement;
				booMethodBuilder.Body.Add(array[tryStatementInfo._stateNumber]);
				booMethodBuilder.Body.Add(SetStateTo(_finishedStateNumber));
				Block block = booMethodBuilder.Body;
				while (tryStatementInfo._parent != null)
				{
					TryStatement tryStatement = new TryStatement();
					block.Add(tryStatement);
					tryStatement.ProtectedBlock.Add(CallMethodOnSelf(tryStatementInfo._ensureMethod));
					Block block3 = (tryStatement.EnsureBlock = new Block());
					block = block3;
					tryStatementInfo = tryStatementInfo._parent;
				}
				block.Add(CallMethodOnSelf(tryStatementInfo._ensureMethod));
				booMethodBuilder.Body.Add(new ReturnStatement());
			}
			for (int i = 0; i < _labels.Count; i++)
			{
				if (_tryStatementInfoForLabels[i] == null)
				{
					array[i] = labelStatement;
				}
				else
				{
					array[i] = array[_tryStatementInfoForLabels[i]._stateNumber];
				}
			}
			booMethodBuilder.Body.Insert(0, base.CodeBuilder.CreateSwitch(LexicalInfo, base.CodeBuilder.CreateMemberReference(_state), array));
			return booMethodBuilder.Entity;
		}

		private void PropagateFromEnumerableToEnumerator(MethodInvocationExpression enumeratorConstructorInvocation, string parameterName, IType parameterType)
		{
			Field field = DeclareFieldInitializedFromConstructorParameter(_enumerable, _enumerableConstructor, parameterName, parameterType);
			enumeratorConstructorInvocation.Arguments.Add(base.CodeBuilder.CreateReference(field));
		}

		private Field DeclareFieldInitializedFromConstructorParameter(BooClassBuilder type, BooMethodBuilder constructor, string parameterName, IType parameterType)
		{
			Field field = type.AddInternalField(UniqueName(parameterName), parameterType);
			InitializeFieldFromConstructorParameter(constructor, field, parameterName, parameterType);
			return field;
		}

		private void InitializeFieldFromConstructorParameter(BooMethodBuilder constructor, Field field, string parameterName, IType parameterType)
		{
			ParameterDeclaration parameter = constructor.AddParameter(parameterName, parameterType);
			constructor.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(field), base.CodeBuilder.CreateReference(parameter)));
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			InternalField internalField = (InternalField)_mapping[node.Entity];
			if (null != internalField)
			{
				ReplaceCurrentNode(base.CodeBuilder.CreateMemberReference(node.LexicalInfo, base.CodeBuilder.CreateSelfReference(_enumerator.Entity), internalField));
			}
		}

		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			ReplaceCurrentNode(base.CodeBuilder.CreateReference(node.LexicalInfo, ExternalEnumeratorSelf()));
		}

		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			MemberReferenceExpression memberReferenceExpression = base.CodeBuilder.CreateReference(node.LexicalInfo, ExternalEnumeratorSelf());
			if (AstUtil.IsTargetOfMethodInvocation(node))
			{
				ReplaceCurrentNode(base.CodeBuilder.CreateMemberReference(memberReferenceExpression, (IMethod)GetEntity(node)));
			}
			else
			{
				ReplaceCurrentNode(memberReferenceExpression);
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			bool flag = IsInvocationOnSuperMethod(node);
			base.OnMethodInvocationExpression(node);
			if (flag)
			{
				IEntity tag = CreateAccessorForSuperMethod(node.Target);
				Bind(node.Target, tag);
			}
		}

		private IEntity CreateAccessorForSuperMethod(Expression target)
		{
			IMethod method = (IMethod)GetEntity(target);
			Method method2 = base.CodeBuilder.CreateMethodFromPrototype(target.LexicalInfo, method, TypeMemberModifiers.Internal, UniqueName(method.Name));
			IMethod method3 = (IMethod)GetEntity(method2);
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateSuperMethodInvocation(method);
			IParameter[] parameters = method3.GetParameters();
			foreach (IParameter entity in parameters)
			{
				methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(entity));
			}
			method2.Body.Add(new ReturnStatement(methodInvocationExpression));
			DeclaringTypeDefinition.Members.Add(method2);
			return GetEntity(method2);
		}

		private string UniqueName(string name)
		{
			return base.Context.GetUniqueName(name);
		}

		private bool IsInvocationOnSuperMethod(MethodInvocationExpression node)
		{
			if (node.Target is SuperLiteralExpression)
			{
				return true;
			}
			MemberReferenceExpression memberReferenceExpression = node.Target as MemberReferenceExpression;
			return memberReferenceExpression != null && memberReferenceExpression.Target is SuperLiteralExpression;
		}

		private Field ExternalEnumeratorSelf()
		{
			if (null == _externalEnumeratorSelf)
			{
				_externalEnumeratorSelf = DeclareFieldInitializedFromConstructorParameter(_enumerator, _enumeratorConstructor, "self_", _generator.DeclaringType);
			}
			return _externalEnumeratorSelf;
		}

		public override bool EnterTryStatement(TryStatement node)
		{
			TryStatementInfo tryStatementInfo = new TryStatementInfo();
			tryStatementInfo._statement = node;
			if (_tryStatementStack.Count > 0)
			{
				tryStatementInfo._parent = _tryStatementStack.Peek();
			}
			_tryStatementStack.Push(tryStatementInfo);
			return true;
		}

		private BinaryExpression SetStateTo(int num)
		{
			return base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateMemberReference(_state), base.CodeBuilder.CreateIntegerLiteral(num));
		}

		public override void LeaveTryStatement(TryStatement node)
		{
			TryStatementInfo tryStatementInfo = _tryStatementStack.Pop();
			if (tryStatementInfo._containsYield)
			{
				ReplaceCurrentNode(tryStatementInfo._replacement);
				TryStatementInfo tryStatementInfo2 = ((_tryStatementStack.Count > 0) ? _tryStatementStack.Peek() : null);
				tryStatementInfo._replacement.Add(node.ProtectedBlock);
				if (tryStatementInfo2 != null)
				{
					ConvertTryStatement(tryStatementInfo2);
					tryStatementInfo._replacement.Add(SetStateTo(tryStatementInfo2._stateNumber));
				}
				else
				{
					tryStatementInfo._replacement.Add(SetStateTo(_finishedStateNumber));
				}
				BooMethodBuilder booMethodBuilder = _enumerator.AddMethod("$ensure" + tryStatementInfo._stateNumber, base.TypeSystemServices.VoidType, TypeMemberModifiers.Private);
				booMethodBuilder.Body.Add(tryStatementInfo._statement.EnsureBlock);
				tryStatementInfo._ensureMethod = booMethodBuilder.Entity;
				tryStatementInfo._replacement.Add(CallMethodOnSelf(booMethodBuilder.Entity));
				_convertedTryStatements.Add(tryStatementInfo);
			}
		}

		private void ConvertTryStatement(TryStatementInfo currentTry)
		{
			if (!currentTry._containsYield)
			{
				currentTry._containsYield = true;
				currentTry._stateNumber = _labels.Count;
				Block block = new Block();
				_labels.Add(_labels[_finishedStateNumber]);
				_tryStatementInfoForLabels.Add(currentTry);
				block.Add(SetStateTo(currentTry._stateNumber));
				currentTry._replacement = block;
			}
		}

		public override void LeaveYieldStatement(YieldStatement node)
		{
			TryStatementInfo tryStatementInfo = ((_tryStatementStack.Count > 0) ? _tryStatementStack.Peek() : null);
			if (tryStatementInfo != null)
			{
				ConvertTryStatement(tryStatementInfo);
			}
			Block block = new Block();
			block.Add(new ReturnStatement(node.LexicalInfo, CreateYieldInvocation(node.LexicalInfo, _labels.Count, node.Expression), null));
			block.Add(CreateLabel(node));
			ReplaceCurrentNode(block);
		}

		private MethodInvocationExpression CreateYieldInvocation(LexicalInfo sourceLocation, int newState, Expression value)
		{
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateSelfReference(_enumerator.Entity), (value != null) ? _yield : _yieldDefault, base.CodeBuilder.CreateIntegerLiteral(newState));
			if (value != null)
			{
				methodInvocationExpression.Arguments.Add(value);
			}
			methodInvocationExpression.LexicalInfo = sourceLocation;
			return methodInvocationExpression;
		}

		private LabelStatement CreateLabel(Node sourceNode)
		{
			InternalLabel internalLabel = base.CodeBuilder.CreateLabel(sourceNode, "$state$" + _labels.Count);
			_labels.Add(internalLabel.LabelStatement);
			_tryStatementInfoForLabels.Add((_tryStatementStack.Count > 0) ? _tryStatementStack.Peek() : null);
			return internalLabel.LabelStatement;
		}

		private BooMethodBuilder CreateConstructor(BooClassBuilder builder)
		{
			BooMethodBuilder booMethodBuilder = builder.AddConstructor();
			booMethodBuilder.Body.Add(base.CodeBuilder.CreateSuperConstructorInvocation(builder.Entity.BaseType));
			return booMethodBuilder;
		}
	}
}
