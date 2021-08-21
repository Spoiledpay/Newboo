#define DEBUG
#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Ast.Visitors;
using Boo.Lang.Compiler.Steps.Generators;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessMethodBodies : AbstractNamespaceSensitiveVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		private sealed class ReturnExpressionFinder : FastDepthFirstVisitor
		{
			private bool _hasReturnStatements;

			private bool _hasYieldStatements;

			public bool HasReturnStatements => _hasReturnStatements;

			public bool HasYieldStatements => _hasYieldStatements;

			public ReturnExpressionFinder(Method node)
			{
				Visit(node.Body);
			}

			public override void OnReturnStatement(ReturnStatement node)
			{
				_hasReturnStatements |= null != node.Expression;
			}

			public override void OnYieldStatement(YieldStatement node)
			{
				_hasYieldStatements = true;
			}
		}

		private const string TempInitializerName = "$___temp_initializer";

		private static readonly ExpressionCollection EmptyExpressionCollection = new ExpressionCollection();

		private static readonly object OptionalReturnStatementAnnotation = new object();

		private static readonly object ResolvedAsExtensionAnnotation = new object();

		private Stack<InternalMethod> _methodStack;

		private Stack _memberStack;

		private Boo.Lang.Compiler.Ast.Module _currentModule;

		private InternalMethod _currentMethod;

		private bool _optimizeNullComparisons = true;

		private EnvironmentProvision<CallableResolutionService> _callableResolutionService;

		private static readonly object PreProcessedKey = new object();

		private EnvironmentProvision<RuntimeMethodCache> _methodCache;

		private EnvironmentProvision<InvocationTypeInferenceRules> _invocationTypeReferenceRules;

		private EnvironmentProvision<GenericsServices> _genericServices;

		private EnvironmentProvision<TypeChecker> _typeChecker;

		protected CallableResolutionService CallableResolutionService => _callableResolutionService;

		protected Method CurrentMethod => _currentMethod.Method;

		protected RuntimeMethodCache MethodCache => _methodCache;

		private TypeDefinition CurrentTypeDefinition => _currentMethod.Method.DeclaringType;

		private TypeChecker TypeChecker => _typeChecker.Instance;

		protected IType CurrentType => _currentMethod.DeclaringType;

		private TypeMember CurrentMember => (TypeMember)_memberStack.Peek();

		public bool OptimizeNullComparisons
		{
			get
			{
				return _optimizeNullComparisons;
			}
			set
			{
				_optimizeNullComparisons = value;
			}
		}

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_currentModule = null;
			_currentMethod = null;
			_methodStack = new Stack<InternalMethod>();
			_memberStack = new Stack();
			_callableResolutionService = default(EnvironmentProvision<CallableResolutionService>);
			_invocationTypeReferenceRules = default(EnvironmentProvision<InvocationTypeInferenceRules>);
			_typeChecker = default(EnvironmentProvision<TypeChecker>);
			_methodCache = default(EnvironmentProvision<RuntimeMethodCache>);
		}

		public override void Run()
		{
			base.NameResolutionService.Reset();
			Visit(base.CompileUnit);
		}

		public override void Dispose()
		{
			base.Dispose();
			_currentModule = null;
			_currentMethod = null;
			_methodStack = null;
			_memberStack = null;
		}

		protected IMethod ResolveMethod(IType type, string name)
		{
			return base.NameResolutionService.ResolveMethod(type, name);
		}

		protected IProperty ResolveProperty(IType type, string name)
		{
			return base.NameResolutionService.ResolveProperty(type, name);
		}

		public override void OnModule(Boo.Lang.Compiler.Ast.Module module)
		{
			if (!WasVisited(module))
			{
				MarkVisited(module);
				_currentModule = module;
				EnterNamespace(InternalModule.ScopeFor(module));
				Visit(module.Members);
				Visit(module.AssemblyAttributes);
				LeaveNamespace();
			}
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			if (!WasVisited(node))
			{
				MarkVisited(node);
				VisitTypeDefinition(node);
			}
		}

		private void VisitBaseTypes(TypeDefinition node)
		{
			foreach (TypeReference baseType in node.BaseTypes)
			{
				EnsureRelatedNodeWasVisited(baseType, baseType.Entity);
			}
		}

		private void VisitTypeDefinition(TypeDefinition node)
		{
			INamespace ns = (INamespace)GetEntity(node);
			EnterNamespace(ns);
			VisitBaseTypes(node);
			Visit(node.Attributes);
			Visit(node.Members);
			LeaveNamespace();
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			if (!WasVisited(node))
			{
				MarkVisited(node);
				VisitTypeDefinition(node);
				FlushFieldInitializers(node);
			}
		}

		private void FlushFieldInitializers(ClassDefinition node)
		{
			TypeMember[] array = node.Members.ToArray();
			foreach (TypeMember typeMember in array)
			{
				switch (typeMember.NodeType)
				{
				case NodeType.Field:
					ProcessFieldInitializer((Field)typeMember);
					break;
				case NodeType.StatementTypeMember:
					ProcessStatementTypeMemberInitializer(node, (StatementTypeMember)typeMember);
					break;
				}
			}
			Method method = (Method)node["$initializer$"];
			if (null != method)
			{
				AddInitializerToInstanceConstructors(node, method);
				node.Members.Remove(method);
			}
		}

		private void ProcessStatementTypeMemberInitializer(ClassDefinition node, StatementTypeMember statementTypeMember)
		{
			Statement statement = statementTypeMember.Statement;
			Method initializerFor = GetInitializerFor(node, node.IsStatic);
			initializerFor.Body.Add(statement);
			InternalMethod internalMethod = (InternalMethod)GetEntity(initializerFor);
			ProcessNodeInMethodContext(internalMethod, internalMethod, statement);
			node.Members.Remove(statementTypeMember);
		}

		public override void OnAttribute(Boo.Lang.Compiler.Ast.Attribute node)
		{
			IType type = node.Entity as IType;
			if (type != null && !IsError(type))
			{
				Visit(node.Arguments);
				ResolveNamedArguments(type, node.NamedArguments);
				IConstructor correctConstructor = GetCorrectConstructor(node, type, node.Arguments);
				if (null != correctConstructor)
				{
					Bind(node, correctConstructor);
				}
			}
		}

		private static bool IsError(IEntity entity)
		{
			return TypeSystemServices.IsError(entity);
		}

		public override void OnProperty(Property node)
		{
			if (!WasVisited(node))
			{
				MarkVisited(node);
				Visit(node.Attributes);
				Visit(node.Type);
				Visit(node.Parameters);
				ResolvePropertyOverride(node);
				ProcessGetter(node);
				if (node.Type == null)
				{
					node.Type = base.CodeBuilder.CreateTypeReference(node.LexicalInfo, InferTypeOfProperty(node));
				}
				if (node.Getter != null)
				{
					node.Getter.ReturnType = node.Type.CloneNode();
				}
				ProcessSetter(node);
			}
		}

		private void ProcessSetter(Property node)
		{
			if (node.Setter != null)
			{
				NormalizeSetterOf(node);
				Visit(node.Setter);
			}
		}

		private void ProcessGetter(Property node)
		{
			if (node.Getter != null)
			{
				NormalizeGetterOf(node);
				Visit(node.Getter);
			}
		}

		private static void NormalizeGetterOf(Property node)
		{
			node.Getter.Parameters.ExtendWithClones(node.Parameters);
			if (node.Getter.ReturnType == null && node.Type != null)
			{
				node.Getter.ReturnType = node.Type.CloneNode();
			}
		}

		private IType InferTypeOfProperty(Property node)
		{
			if (node.Getter == null)
			{
				return base.TypeSystemServices.ObjectType;
			}
			IType returnType = GetEntity(node.Getter).ReturnType;
			if (returnType != base.TypeSystemServices.VoidType)
			{
				return returnType;
			}
			return base.TypeSystemServices.ObjectType;
		}

		private void NormalizeSetterOf(Property node)
		{
			Method setter = node.Setter;
			setter.Name = "set_" + node.Name;
			ParameterDeclarationCollection parameters = setter.Parameters;
			parameters.ExtendWithClones(node.Parameters);
			parameters.Add(base.CodeBuilder.CreateParameterDeclaration(base.CodeBuilder.GetFirstParameterIndex(setter) + parameters.Count, "value", GetType(node.Type)));
		}

		public override void OnStatementTypeMember(StatementTypeMember node)
		{
		}

		public override void OnField(Field node)
		{
			if (WasVisited(node))
			{
				return;
			}
			MarkVisited(node);
			InternalField internalField = (InternalField)GetEntity(node);
			Visit(node.Attributes);
			Visit(node.Type);
			if (node.Initializer != null)
			{
				IType type = ((node.Type != null) ? GetType(node.Type) : null);
				if (type != null && TypeSystemServices.IsNullable(type))
				{
					BindNullableInitializer(node, node.Initializer, type);
				}
				if (internalField.DeclaringType.IsValueType && !node.IsStatic)
				{
					Error(CompilerErrorFactory.ValueTypeFieldsCannotHaveInitializers(node.Initializer));
				}
				try
				{
					PushMember(node);
					PreProcessFieldInitializer(node);
				}
				finally
				{
					PopMember();
				}
			}
			else if (null == node.Type)
			{
				node.Type = CreateTypeReference(node.LexicalInfo, base.TypeSystemServices.ObjectType);
			}
			CheckFieldType(node.Type);
		}

		private static bool IsValidLiteralInitializer(Expression e)
		{
			switch (e.NodeType)
			{
			case NodeType.StringLiteralExpression:
			case NodeType.IntegerLiteralExpression:
			case NodeType.DoubleLiteralExpression:
			case NodeType.NullLiteralExpression:
			case NodeType.BoolLiteralExpression:
				return true;
			default:
				return false;
			}
		}

		private void ProcessLiteralField(Field node)
		{
			Visit(node.Initializer);
			ProcessFieldInitializerType(node, node.Initializer.ExpressionType);
			((InternalField)node.Entity).StaticValue = node.Initializer;
			node.Initializer = null;
		}

		private void ProcessFieldInitializerType(Field node, IType initializerType)
		{
			if (null == node.Type)
			{
				node.Type = CreateTypeReference(node.LexicalInfo, MapWildcardType(initializerType));
			}
			else
			{
				AssertTypeCompatibility(node.Initializer, GetType(node.Type), initializerType);
			}
		}

		private TypeReference CreateTypeReference(LexicalInfo info, IType type)
		{
			TypeReference typeReference = base.CodeBuilder.CreateTypeReference(type);
			typeReference.LexicalInfo = info;
			return typeReference;
		}

		private void PreProcessFieldInitializer(Field node)
		{
			Expression initializer = node.Initializer;
			if (node.IsFinal && node.IsStatic && IsValidLiteralInitializer(initializer))
			{
				ProcessLiteralField(node);
				return;
			}
			BlockExpression blockExpression = node.Initializer as BlockExpression;
			if (blockExpression != null)
			{
				InferClosureSignature(blockExpression);
			}
			Method initializerMethod = GetInitializerMethod(node);
			InternalMethod internalMethod = (InternalMethod)initializerMethod.Entity;
			ReferenceExpression referenceExpression = new ReferenceExpression("$___temp_initializer");
			BinaryExpression binaryExpression = new BinaryExpression(node.LexicalInfo, BinaryOperatorType.Assign, referenceExpression, initializer);
			ProcessNodeInMethodContext(internalMethod, internalMethod, binaryExpression);
			initializerMethod.Locals.RemoveByEntity(referenceExpression.Entity);
			IType expressionType = GetExpressionType(binaryExpression.Right);
			ProcessFieldInitializerType(node, expressionType);
			node.Initializer = binaryExpression.Right;
		}

		private void ProcessFieldInitializer(Field node)
		{
			Expression initializer = node.Initializer;
			if (null == initializer)
			{
				return;
			}
			switch (initializer.NodeType)
			{
			case NodeType.NullLiteralExpression:
				node.Initializer = null;
				return;
			case NodeType.IntegerLiteralExpression:
				if (0 == ((IntegerLiteralExpression)initializer).Value)
				{
					node.Initializer = null;
					return;
				}
				break;
			case NodeType.BoolLiteralExpression:
				if (!((BoolLiteralExpression)initializer).Value)
				{
					node.Initializer = null;
					return;
				}
				break;
			case NodeType.DoubleLiteralExpression:
				if (0.0 == ((DoubleLiteralExpression)initializer).Value)
				{
					node.Initializer = null;
					return;
				}
				break;
			}
			Method initializerMethod = GetInitializerMethod(node);
			initializerMethod.Body.Add(base.CodeBuilder.CreateAssignment(initializer.LexicalInfo, base.CodeBuilder.CreateReference(node), initializer));
			node.Initializer = null;
		}

		private Method CreateInitializerMethod(TypeDefinition type, string name, TypeMemberModifiers modifiers)
		{
			Method method = base.CodeBuilder.CreateMethod(name, base.TypeSystemServices.VoidType, modifiers);
			type.Members.Add(method);
			MarkVisited(method);
			return method;
		}

		private Field GetFieldsInitializerInitializedField(TypeDefinition type)
		{
			string name = AstUtil.BuildUniqueTypeMemberName(type, "initialized");
			Field field = (Field)type.Members[name];
			if (null == field)
			{
				field = base.CodeBuilder.CreateField(name, base.TypeSystemServices.BoolType);
				field.Visibility = TypeMemberModifiers.Private;
				type.Members.Add(field);
				MarkVisited(field);
			}
			return field;
		}

		private Method GetInitializerMethod(Field node)
		{
			return GetInitializerFor(node.DeclaringType, node.IsStatic);
		}

		private Method GetInitializerFor(TypeDefinition type, bool isStatic)
		{
			string text = (isStatic ? "$static_initializer$" : "$initializer$");
			Method method = (Method)type[text];
			if (null == method)
			{
				if (isStatic)
				{
					if (!type.HasStaticConstructor)
					{
						method = base.CodeBuilder.CreateStaticConstructor(type);
					}
					else
					{
						method = CreateInitializerMethod(type, text, TypeMemberModifiers.Static);
						AddInitializerToStaticConstructor(type, (InternalMethod)method.Entity);
					}
				}
				else
				{
					method = CreateInitializerMethod(type, text, TypeMemberModifiers.None);
				}
				type[text] = method;
			}
			return method;
		}

		private void AddInitializerToStaticConstructor(TypeDefinition type, InternalMethod initializer)
		{
			GetStaticConstructor(type).Body.Insert(0, base.CodeBuilder.CreateMethodInvocation(initializer));
		}

		private void AddInitializerToInstanceConstructors(TypeDefinition type, Method initializer)
		{
			int num = 0;
			foreach (TypeMember member in type.Members)
			{
				if (NodeType.Constructor == member.NodeType && !member.IsStatic)
				{
					num++;
				}
			}
			if (num > 1)
			{
				AddInitializedGuardToInitializer(type, initializer);
			}
			foreach (TypeMember member2 in type.Members)
			{
				if (NodeType.Constructor != member2.NodeType || member2.IsStatic)
				{
					continue;
				}
				Constructor constructor = (Constructor)member2;
				num = GetIndexAfterSuperInvocation(constructor.Body);
				foreach (Statement statement in initializer.Body.Statements)
				{
					constructor.Body.Insert(num, (Statement)statement.Clone());
					num++;
				}
				foreach (Local local in initializer.Locals)
				{
					constructor.Locals.Add(local);
				}
			}
		}

		private void AddInitializedGuardToInitializer(TypeDefinition type, Method initializer)
		{
			Field fieldsInitializerInitializedField = GetFieldsInitializerInitializedField(type);
			Block block = new Block();
			block.Add(new GotoStatement(LexicalInfo.Empty, new ReferenceExpression("___initialized___")));
			IfStatement stmt = new IfStatement(base.CodeBuilder.CreateReference(fieldsInitializerInitializedField), block, null);
			initializer.Body.Insert(0, stmt);
			initializer.Body.Add(base.CodeBuilder.CreateFieldAssignment(fieldsInitializerInitializedField, new BoolLiteralExpression(value: true)));
			initializer.Body.Add(new LabelStatement(LexicalInfo.Empty, "___initialized___"));
		}

		private int GetIndexAfterSuperInvocation(Block body)
		{
			int num = 0;
			foreach (Statement statement in body.Statements)
			{
				if (NodeType.ExpressionStatement == statement.NodeType)
				{
					Expression expression = ((ExpressionStatement)statement).Expression;
					if (NodeType.MethodInvocationExpression == expression.NodeType && NodeType.SuperLiteralExpression == ((MethodInvocationExpression)expression).Target.NodeType)
					{
						return num + 1;
					}
				}
				num++;
			}
			return 0;
		}

		private Constructor GetStaticConstructor(TypeDefinition type)
		{
			return base.CodeBuilder.GetOrCreateStaticConstructorFor(type);
		}

		private void CheckRuntimeMethod(Method method)
		{
			if (!method.Body.IsEmpty)
			{
				Error(CompilerErrorFactory.RuntimeMethodBodyMustBeEmpty(method, GetEntity(method)));
			}
		}

		private void CheckInstanceMethodInvocationsWithinConstructor(Constructor ctor)
		{
			if (ctor.Body.IsEmpty)
			{
				return;
			}
			foreach (Statement statement in ctor.Body.Statements)
			{
				ExpressionStatement expressionStatement = statement as ExpressionStatement;
				if (null == expressionStatement)
				{
					continue;
				}
				MethodInvocationExpression methodInvocationExpression = expressionStatement.Expression as MethodInvocationExpression;
				if (null == methodInvocationExpression)
				{
					continue;
				}
				if (methodInvocationExpression.Target is SelfLiteralExpression || methodInvocationExpression.Target is SuperLiteralExpression)
				{
					break;
				}
				if (methodInvocationExpression.Target is MemberReferenceExpression)
				{
					MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)methodInvocationExpression.Target;
					if (memberReferenceExpression.Target is SelfLiteralExpression || memberReferenceExpression.Target is SuperLiteralExpression)
					{
						Error(CompilerErrorFactory.InstanceMethodInvocationBeforeInitialization(ctor, memberReferenceExpression));
					}
				}
			}
		}

		public override void OnConstructor(Constructor node)
		{
			if (WasVisited(node))
			{
				return;
			}
			MarkVisited(node);
			Visit(node.Attributes);
			Visit(node.Parameters);
			InternalConstructor internalConstructor = (InternalConstructor)node.Entity;
			ProcessMethodBody(internalConstructor);
			if (node.IsRuntime)
			{
				CheckRuntimeMethod(node);
				return;
			}
			if (internalConstructor.DeclaringType.IsValueType)
			{
				if (node.Parameters.Count == 0 && !node.IsStatic && !node.IsSynthetic)
				{
					Error(CompilerErrorFactory.ValueTypesCannotDeclareParameterlessConstructors(node));
				}
			}
			else if (!internalConstructor.HasSelfCall && !internalConstructor.HasSuperCall && !internalConstructor.IsStatic)
			{
				IType baseType = internalConstructor.DeclaringType.BaseType;
				IConstructor correctConstructor = GetCorrectConstructor(node, baseType, EmptyExpressionCollection);
				if (null != correctConstructor)
				{
					node.Body.Statements.Insert(0, base.CodeBuilder.CreateSuperConstructorInvocation(correctConstructor));
				}
			}
			if (!internalConstructor.IsStatic)
			{
				CheckInstanceMethodInvocationsWithinConstructor(node);
			}
		}

		public override void LeaveParameterDeclaration(ParameterDeclaration node)
		{
			AssertIdentifierName(node, node.Name);
			CheckParameterType(node.Type);
		}

		private void CheckParameterType(TypeReference type)
		{
			if (type.Entity == VoidType())
			{
				Error(CompilerErrorFactory.InvalidParameterType(type, VoidType()));
			}
		}

		private IType VoidType()
		{
			return base.TypeSystemServices.VoidType;
		}

		private void CheckFieldType(TypeReference type)
		{
			if (type.Entity == VoidType())
			{
				Error(CompilerErrorFactory.InvalidFieldType(type, VoidType()));
			}
		}

		private bool CheckDeclarationType(TypeReference type)
		{
			if (type.Entity != VoidType())
			{
				return true;
			}
			Error(CompilerErrorFactory.InvalidDeclarationType(type, VoidType()));
			return false;
		}

		public override void OnBlockExpression(BlockExpression node)
		{
			if (!WasVisited(node) && !ShouldDeferClosureProcessing(node))
			{
				InferClosureSignature(node);
				ProcessClosureBody(node);
			}
		}

		private void InferClosureSignature(BlockExpression node)
		{
			ClosureSignatureInferrer closureSignatureInferrer = new ClosureSignatureInferrer(node);
			ICallableType callableType = closureSignatureInferrer.InferCallableType();
			BindExpressionType(node, callableType);
			AddInferredClosureParameterTypes(node, callableType);
		}

		private bool ShouldDeferClosureProcessing(BlockExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = node.ParentNode as MethodInvocationExpression;
			if (methodInvocationExpression == null)
			{
				return false;
			}
			if (!methodInvocationExpression.Arguments.Contains(node))
			{
				return false;
			}
			if (methodInvocationExpression.Target.Entity is Ambiguous)
			{
				return ((Ambiguous)methodInvocationExpression.Target.Entity).Any(GenericsServices.IsGenericMethod);
			}
			IMethod method = methodInvocationExpression.Target.Entity as IMethod;
			return method != null && GenericsServices.IsGenericMethod(method);
		}

		private void AddInferredClosureParameterTypes(BlockExpression node, ICallableType callableType)
		{
			IParameter[] array = callableType?.GetSignature().Parameters;
			for (int i = 0; i < node.Parameters.Count; i++)
			{
				ParameterDeclaration parameterDeclaration = node.Parameters[i];
				if (parameterDeclaration.Type == null)
				{
					IType type = ((array != null && i < array.Length) ? array[i].Type : ((!parameterDeclaration.IsParamArray) ? base.TypeSystemServices.ObjectType : base.TypeSystemServices.ObjectArrayType));
					parameterDeclaration.Type = base.CodeBuilder.CreateTypeReference(type);
				}
			}
		}

		private void ProcessClosureBody(BlockExpression node)
		{
			MarkVisited(node);
			if (node.ContainsAnnotation("inline"))
			{
				AddOptionalReturnStatement(node.Body);
			}
			string text = node[BlockExpression.ClosureNameAnnotation] as string;
			Method method = base.CodeBuilder.CreateMethod(ClosureName(text), node.ReturnType ?? base.CodeBuilder.CreateTypeReference(Unknown.Default), ClosureModifiers());
			MarkVisited(method);
			InternalMethod internalMethod = (InternalMethod)method.Entity;
			method.LexicalInfo = node.LexicalInfo;
			method.Parameters = node.Parameters;
			method.Body = node.Body;
			CurrentMethod.DeclaringType.Members.Add(method);
			base.CodeBuilder.BindParameterDeclarations(_currentMethod.IsStatic, method);
			Visit(method.Parameters);
			NamespaceDelegator namespaceDelegator = new NamespaceDelegator(base.CurrentNamespace, internalMethod);
			if (text != null)
			{
				namespaceDelegator.DelegateTo(new AliasedNamespace(text, internalMethod));
			}
			ProcessMethodBody(internalMethod, namespaceDelegator);
			if (internalMethod.ReturnType is Unknown)
			{
				TryToResolveReturnType(internalMethod);
			}
			node.ExpressionType = internalMethod.Type;
			node.Entity = internalMethod;
		}

		private void ProcessClosureInMethodInvocation(GenericParameterInferrer inferrer, BlockExpression closure, ICallableType formalType)
		{
			CallableSignature signature = formalType.GetSignature();
			TypeReplacer typeReplacer = new TypeReplacer();
			TypeCollector typeCollector = new TypeCollector(delegate(IType t)
			{
				IGenericParameter genericParameter = t as IGenericParameter;
				return genericParameter != null && genericParameter.DeclaringEntity == inferrer.GenericMethod;
			});
			typeCollector.Visit(formalType);
			foreach (IType match in typeCollector.Matches)
			{
				IType inferredType = inferrer.GetInferredType((IGenericParameter)match);
				if (inferredType != null)
				{
					typeReplacer.Replace(match, inferredType);
				}
			}
			for (int i = 0; i < signature.Parameters.Length; i++)
			{
				ParameterDeclaration parameterDeclaration = closure.Parameters[i];
				if (parameterDeclaration.Type == null)
				{
					parameterDeclaration.Type = base.CodeBuilder.CreateTypeReference(typeReplacer.MapType(signature.Parameters[i].Type));
				}
			}
			ProcessClosureBody(closure);
		}

		private TypeMemberModifiers ClosureModifiers()
		{
			TypeMemberModifiers typeMemberModifiers = TypeMemberModifiers.Internal;
			if (_currentMethod.IsStatic)
			{
				typeMemberModifiers |= TypeMemberModifiers.Static;
			}
			return typeMemberModifiers;
		}

		private string ClosureName(string explicitName)
		{
			string text = explicitName ?? "closure";
			return base.Context.GetUniqueName(_currentMethod.Name, text);
		}

		private static void AddOptionalReturnStatement(Block body)
		{
			if (body.Statements.Count == 1)
			{
				ExpressionStatement expressionStatement = body.FirstStatement as ExpressionStatement;
				if (null != expressionStatement)
				{
					ReturnStatement returnStatement = new ReturnStatement(expressionStatement.LexicalInfo, expressionStatement.Expression, null);
					returnStatement.Annotate(OptionalReturnStatementAnnotation);
					body.Replace(expressionStatement, returnStatement);
				}
			}
		}

		public override void OnMethod(Method method)
		{
			if (WasVisited(method))
			{
				return;
			}
			MarkVisited(method);
			Visit(method.Attributes);
			Visit(method.Parameters);
			Visit(method.ReturnType);
			Visit(method.ReturnTypeAttributes);
			bool isPInvoke = GetEntity(method).IsPInvoke;
			if (method.IsRuntime || isPInvoke)
			{
				CheckRuntimeMethod(method);
				if (isPInvoke)
				{
					method.Modifiers |= TypeMemberModifiers.Static;
				}
				return;
			}
			try
			{
				PushMember(method);
				ProcessRegularMethod(method);
			}
			finally
			{
				PopMember();
			}
		}

		private void CheckIfIsMethodOverride(InternalMethod method)
		{
			if (method.IsStatic || method.IsNew)
			{
				return;
			}
			IMethod method2 = FindMethodOverridenBy(method);
			if (method2 != null)
			{
				if (CanBeOverriden(method2))
				{
					ProcessMethodOverride(method, method2);
				}
				else if (InStrictMode())
				{
					CantOverrideNonVirtual(method.Method, method2);
				}
				else
				{
					MethodHidesInheritedNonVirtual(method, method2);
				}
			}
		}

		private bool InStrictMode()
		{
			return base.Parameters.Strict;
		}

		private void MethodHidesInheritedNonVirtual(InternalMethod hidingMethod, IMethod hiddenMethod)
		{
			base.Warnings.Add(CompilerWarningFactory.MethodHidesInheritedNonVirtual(hidingMethod.Method, hidingMethod, hiddenMethod));
		}

		private IMethod FindPropertyAccessorOverridenBy(Property property, Method accessor)
		{
			IProperty overriden = ((InternalProperty)property.Entity).Overriden;
			if (overriden == null)
			{
				return null;
			}
			IMethod method = ((property.Getter == accessor) ? overriden.GetGetMethod() : overriden.GetSetMethod());
			if (method != null && TypeSystemServices.CheckOverrideSignature((IMethod)accessor.Entity, method))
			{
				return method;
			}
			return null;
		}

		private IMethod FindMethodOverridenBy(InternalMethod entity)
		{
			Method method = entity.Method;
			if (method.ParentNode.NodeType == NodeType.Property)
			{
				return FindPropertyAccessorOverridenBy((Property)method.ParentNode, method);
			}
			IType baseType = entity.DeclaringType.BaseType;
			IEntity entity2 = base.NameResolutionService.Resolve(baseType, entity.Name, EntityType.Method);
			if (entity2 == null)
			{
				return null;
			}
			IMethod method2 = FindMethodOverridenBy(entity, entity2);
			if (method2 != null)
			{
				EnsureRelatedNodeWasVisited(method, method2);
			}
			return method2;
		}

		private static IMethod FindMethodOverridenBy(InternalMethod entity, IEntity candidates)
		{
			if (EntityType.Method == candidates.EntityType)
			{
				IMethod method = (IMethod)candidates;
				if (TypeSystemServices.CheckOverrideSignature(entity, method))
				{
					return method;
				}
			}
			if (candidates.IsAmbiguous())
			{
				IEntity[] entities = ((Ambiguous)candidates).Entities;
				for (int i = 0; i < entities.Length; i++)
				{
					IMethod method = (IMethod)entities[i];
					if (TypeSystemServices.CheckOverrideSignature(entity, method))
					{
						return method;
					}
				}
			}
			return null;
		}

		private void ResolveMethodOverride(InternalMethod entity)
		{
			IMethod method = FindMethodOverridenBy(entity);
			if (method == null)
			{
				string mostSimilarBaseMethodName = GetMostSimilarBaseMethodName(entity);
				if (mostSimilarBaseMethodName == entity.Name)
				{
					Error(CompilerErrorFactory.NoMethodToOverride(entity.Method, entity, incompatibleSignature: true));
				}
				else
				{
					Error(CompilerErrorFactory.NoMethodToOverride(entity.Method, entity, mostSimilarBaseMethodName));
				}
			}
			else
			{
				ValidateOverride(entity, method);
			}
		}

		private string GetMostSimilarBaseMethodName(InternalMethod entity)
		{
			return base.NameResolutionService.GetMostSimilarMemberName(entity.DeclaringType.BaseType, entity.Name, EntityType.Method);
		}

		private void ValidateOverride(InternalMethod entity, IMethod baseMethod)
		{
			if (CanBeOverriden(baseMethod))
			{
				ProcessMethodOverride(entity, baseMethod);
			}
			else
			{
				CantOverrideNonVirtual(entity.Method, baseMethod);
			}
		}

		private bool CanBeOverriden(IMethod baseMethod)
		{
			return baseMethod.IsVirtual && !baseMethod.IsFinal;
		}

		private void ProcessMethodOverride(InternalMethod entity, IMethod baseMethod)
		{
			CallableSignature overriddenSignature = TypeSystemServices.GetOverriddenSignature(baseMethod, entity);
			if (TypeSystemServices.IsUnknown(entity.ReturnType))
			{
				entity.Method.ReturnType = base.CodeBuilder.CreateTypeReference(entity.Method.LexicalInfo, overriddenSignature.ReturnType);
			}
			else if (overriddenSignature.ReturnType != entity.ReturnType)
			{
				Error(CompilerErrorFactory.InvalidOverrideReturnType(entity.Method.ReturnType, baseMethod, baseMethod.ReturnType, entity.ReturnType));
			}
			SetOverride(entity, baseMethod);
		}

		private void CantOverrideNonVirtual(Method method, IMethod baseMethod)
		{
			Error(CompilerErrorFactory.CantOverrideNonVirtual(method, baseMethod));
		}

		private static void SetPropertyAccessorOverride(Method accessor)
		{
			if (null != accessor)
			{
				accessor.Modifiers |= TypeMemberModifiers.Override;
			}
		}

		private IProperty ResolvePropertyOverride(IProperty p, IEntity[] candidates)
		{
			foreach (IEntity entity in candidates)
			{
				if (EntityType.Property == entity.EntityType)
				{
					IProperty property = (IProperty)entity;
					if (CheckOverrideSignature(p, property))
					{
						return property;
					}
				}
			}
			return null;
		}

		private static bool CheckOverrideSignature(IProperty p, IProperty candidate)
		{
			return TypeSystemServices.CheckOverrideSignature(p.GetParameters(), candidate.GetParameters());
		}

		private void ResolvePropertyOverride(Property property)
		{
			IProperty property2 = FindPropertyOverridenBy(property);
			if (property2 != null)
			{
				EntityFor(property).Overriden = property2;
				EnsureRelatedNodeWasVisited(property, property2);
				PropagateOverrideToAccessors(property);
				if (property.Type == null)
				{
					property.Type = base.CodeBuilder.CreateTypeReference(EntityFor(property).Overriden.Type);
				}
			}
		}

		private void PropagateOverrideToAccessors(Property property)
		{
			if (property.IsOverride)
			{
				SetPropertyAccessorOverride(property.Getter);
				SetPropertyAccessorOverride(property.Setter);
				return;
			}
			IProperty overriden = EntityFor(property).Overriden;
			if (overriden.GetGetMethod() != null)
			{
				SetPropertyAccessorOverride(property.Getter);
			}
			if (overriden.GetSetMethod() != null)
			{
				SetPropertyAccessorOverride(property.Setter);
			}
		}

		private InternalProperty EntityFor(Property property)
		{
			return (InternalProperty)property.Entity;
		}

		private IProperty FindPropertyOverridenBy(Property property)
		{
			IType baseType = EntityFor(property).DeclaringType.BaseType;
			IEntity entity = base.NameResolutionService.Resolve(baseType, property.Name, EntityType.Property);
			if (entity != null)
			{
				if (EntityType.Property == entity.EntityType)
				{
					IProperty property2 = (IProperty)entity;
					if (CheckOverrideSignature(EntityFor(property), property2))
					{
						return property2;
					}
				}
				else if (entity.IsAmbiguous())
				{
					return ResolvePropertyOverride(EntityFor(property), ((Ambiguous)entity).Entities);
				}
			}
			return null;
		}

		private void SetOverride(InternalMethod entity, IMethod baseMethod)
		{
			TraceOverride(entity.Method, baseMethod);
			entity.Overriden = baseMethod;
			entity.Method.Modifiers |= TypeMemberModifiers.Override;
		}

		private void TraceOverride(Method method, IMethod baseMethod)
		{
			_context.TraceInfo("{0}: Method '{1}' overrides '{2}'", method.LexicalInfo, method.Name, baseMethod);
		}

		protected bool HasNeitherReturnNorYield(Method node)
		{
			ReturnExpressionFinder returnExpressionFinder = new ReturnExpressionFinder(node);
			return !returnExpressionFinder.HasReturnStatements && !returnExpressionFinder.HasYieldStatements;
		}

		private void PreProcessMethod(Method node)
		{
			if (WasAlreadyPreProcessed(node))
			{
				return;
			}
			MarkPreProcessed(node);
			InternalMethod internalMethod = (InternalMethod)GetEntity(node);
			if (node.IsOverride)
			{
				ResolveMethodOverride(internalMethod);
				return;
			}
			CheckIfIsMethodOverride(internalMethod);
			if (TypeSystemServices.IsUnknown(internalMethod.ReturnType) && HasNeitherReturnNorYield(node))
			{
				node.ReturnType = base.CodeBuilder.CreateTypeReference(node.LexicalInfo, base.TypeSystemServices.VoidType);
			}
		}

		private static bool WasAlreadyPreProcessed(Method node)
		{
			return node.ContainsAnnotation(PreProcessedKey);
		}

		private static void MarkPreProcessed(Method node)
		{
			node[PreProcessedKey] = PreProcessedKey;
		}

		private void ProcessRegularMethod(Method node)
		{
			PreProcessMethod(node);
			InternalMethod entity = (InternalMethod)GetEntity(node);
			ProcessMethodBody(entity);
			PostProcessMethod(node);
		}

		private void PostProcessMethod(Method node)
		{
			if (node.DeclaringType.NodeType == NodeType.ClassDefinition)
			{
				InternalMethod internalMethod = (InternalMethod)GetEntity(node);
				if (TypeSystemServices.IsUnknown(internalMethod.ReturnType))
				{
					TryToResolveReturnType(internalMethod);
				}
				else if (internalMethod.IsGenerator)
				{
					CheckGeneratorReturnType(node, internalMethod.ReturnType);
					CheckGeneratorYieldType(internalMethod, internalMethod.ReturnType);
				}
				CheckGeneratorCantReturnValues(internalMethod);
			}
		}

		private void CheckGeneratorCantReturnValues(InternalMethod entity)
		{
			if (!entity.IsGenerator || null == entity.ReturnExpressions)
			{
				return;
			}
			foreach (Expression returnExpression in entity.ReturnExpressions)
			{
				Error(CompilerErrorFactory.GeneratorCantReturnValue(returnExpression));
			}
		}

		private void CheckGeneratorReturnType(Method method, IType returnType)
		{
			if (base.TypeSystemServices.IEnumerableType != returnType && base.TypeSystemServices.IEnumeratorType != returnType && !base.TypeSystemServices.IsSystemObject(returnType) && !base.TypeSystemServices.IsGenericGeneratorReturnType(returnType))
			{
				Error(CompilerErrorFactory.InvalidGeneratorReturnType(method.ReturnType, returnType));
			}
		}

		private void CheckGeneratorYieldType(InternalMethod method, IType returnType)
		{
			if (!base.TypeSystemServices.IsGenericGeneratorReturnType(returnType))
			{
				return;
			}
			IType type = returnType.ConstructedInfo.GenericArguments[0];
			foreach (Expression yieldExpression in method.YieldExpressions)
			{
				IType expressionType = yieldExpression.ExpressionType;
				if (!IsAssignableFrom(type, expressionType) && !base.TypeSystemServices.CanBeReachedByDownCastOrPromotion(type, expressionType))
				{
					Error(CompilerErrorFactory.YieldTypeDoesNotMatchReturnType(yieldExpression, expressionType, type));
				}
			}
		}

		private void ProcessMethodBody(InternalMethod entity)
		{
			ProcessMethodBody(entity, entity);
		}

		private void ProcessMethodBody(InternalMethod entity, INamespace ns)
		{
			ProcessNodeInMethodContext(entity, ns, entity.Method.Body);
		}

		private void ProcessNodeInMethodContext(InternalMethod entity, INamespace ns, Node node)
		{
			PushMethodInfo(entity);
			EnterNamespace(ns);
			try
			{
				Visit(node);
			}
			finally
			{
				LeaveNamespace();
				PopMethodInfo();
			}
		}

		private void ResolveGeneratorReturnType(InternalMethod entity)
		{
			IType generatorReturnType = GetGeneratorReturnType(entity);
			entity.Method.ReturnType = base.CodeBuilder.CreateTypeReference(generatorReturnType);
		}

		protected virtual IType GetGeneratorReturnType(InternalMethod generator)
		{
			return GeneratorTypeOf(GeneratorItemTypeFor(generator));
		}

		private IType GeneratorItemTypeFor(InternalMethod generator)
		{
			return My<GeneratorItemTypeInferrer>.Instance.GeneratorItemTypeFor(generator);
		}

		private void TryToResolveReturnType(InternalMethod entity)
		{
			if (entity.IsGenerator)
			{
				ResolveGeneratorReturnType(entity);
			}
			else if (CanResolveReturnType(entity))
			{
				ResolveReturnType(entity);
			}
		}

		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			if (!AstUtil.IsTargetOfMethodInvocation(node))
			{
				node.ExpressionType = _currentMethod.DeclaringType.BaseType;
			}
			else if (EntityType.Constructor == _currentMethod.EntityType)
			{
				node.Entity = _currentMethod;
			}
			else if (null == _currentMethod.Overriden)
			{
				Error(node, CompilerErrorFactory.MethodIsNotOverride(node, _currentMethod));
			}
			else
			{
				node.Entity = _currentMethod.Overriden;
			}
		}

		private static bool CanResolveReturnType(InternalMethod method)
		{
			ExpressionCollection returnExpressions = method.ReturnExpressions;
			if (null != returnExpressions)
			{
				foreach (Expression item in returnExpressions)
				{
					IType expressionType = item.ExpressionType;
					if (expressionType == null || TypeSystemServices.IsUnknown(expressionType))
					{
						return false;
					}
				}
			}
			return true;
		}

		private void ResolveReturnType(InternalMethod entity)
		{
			Method method = entity.Method;
			method.ReturnType = ((entity.ReturnExpressions == null) ? base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.VoidType) : GetMostGenericTypeReference(entity.ReturnExpressions));
			TraceReturnType(method, entity);
		}

		private TypeReference GetMostGenericTypeReference(ExpressionCollection expressions)
		{
			IType type = MapWildcardType(GetMostGenericType(expressions));
			return base.CodeBuilder.CreateTypeReference(type);
		}

		private IType MapWildcardType(IType type)
		{
			return base.TypeSystemServices.MapWildcardType(type);
		}

		private IType GetMostGenericType(IType lhs, IType rhs)
		{
			return base.TypeSystemServices.GetMostGenericType(lhs, rhs);
		}

		private IType GetMostGenericType(ExpressionCollection args)
		{
			return base.TypeSystemServices.GetMostGenericType(args);
		}

		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.BoolType);
		}

		public override void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.TimeSpanType);
		}

		public override void OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
			BindExpressionType(node, node.IsLong ? base.TypeSystemServices.LongType : base.TypeSystemServices.IntType);
		}

		public override void OnDoubleLiteralExpression(DoubleLiteralExpression node)
		{
			BindExpressionType(node, node.IsSingle ? base.TypeSystemServices.SingleType : base.TypeSystemServices.DoubleType);
		}

		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.StringType);
		}

		public override void OnCharLiteralExpression(CharLiteralExpression node)
		{
			CheckCharLiteralValue(node);
			BindExpressionType(node, base.TypeSystemServices.CharType);
		}

		private void CheckCharLiteralValue(CharLiteralExpression node)
		{
			string value = node.Value;
			if (value == null || value.Length != 1)
			{
				base.Errors.Add(CompilerErrorFactory.InvalidCharLiteral(node, value));
			}
		}

		private static IEntity[] GetSetMethods(IEntity[] entities)
		{
			return GetPropertyAccessors(entities, (IProperty p) => p.GetSetMethod());
		}

		private static IEntity[] GetGetMethods(IEntity[] entities)
		{
			return GetPropertyAccessors(entities, (IProperty p) => p.GetGetMethod());
		}

		private static IEntity[] GetPropertyAccessors(IEntity[] entities, Func<IProperty, IEntity> selector)
		{
			return entities.OfType<IProperty>().Select(selector).Distinct()
				.ToArray();
		}

		private void AssertIsNotComplexSlicing(SlicingExpression node)
		{
			if (node.IsComplexSlicing())
			{
				NotImplemented(node, "complex slicing");
			}
		}

		protected MethodInvocationExpression CreateEquals(BinaryExpression node)
		{
			return base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_EqualityOperator, node.Left, node.Right);
		}

		protected bool IsIndexedProperty(Expression expression)
		{
			return expression.Entity.IsIndexedProperty();
		}

		public override void LeaveSlicingExpression(SlicingExpression node)
		{
			if (node.Target.Entity.IsAmbiguous())
			{
				BindIndexedPropertySlicing(node);
				return;
			}
			IType expressionType = GetExpressionType(node.Target);
			if (IsError(expressionType))
			{
				Error(node);
				return;
			}
			if (node.IsComplexSlicing())
			{
				BindExpressionType(node, ResultingTypeForComplexSlicing(node));
				return;
			}
			if (IsIndexedProperty(node.Target))
			{
				BindIndexedPropertySlicing(node);
				return;
			}
			if (expressionType.IsArray)
			{
				BindExpressionType(node, expressionType.ElementType);
				return;
			}
			IEntity defaultMember = base.TypeSystemServices.GetDefaultMember(expressionType);
			if (defaultMember == null)
			{
				Error(node, CompilerErrorFactory.TypeDoesNotSupportSlicing(node.Target, expressionType));
				return;
			}
			node.Target = new MemberReferenceExpression(node.LexicalInfo, node.Target, defaultMember.Name)
			{
				Entity = defaultMember,
				ExpressionType = Null.Default
			};
			SliceMember(node, defaultMember);
		}

		private IType ResultingTypeForComplexSlicing(SlicingExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			return expressionType.IsArray ? ResultingTypeForArraySlicing(node) : expressionType;
		}

		private IType ResultingTypeForArraySlicing(SlicingExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			if (node.Indices.Count > 1)
			{
				int num = node.Indices.Count((Slice t) => t.End == null);
				return expressionType.ElementType.MakeArrayType(node.Indices.Count - num);
			}
			return expressionType;
		}

		private void BindIndexedPropertySlicing(SlicingExpression node)
		{
			AssertIsNotComplexSlicing(node);
			SliceMember(node, node.Target.Entity);
		}

		private void SliceMember(SlicingExpression node, IEntity member)
		{
			EnsureRelatedNodeWasVisited(node, member);
			if (node.IsTargetOfAssignment())
			{
				Bind(node, member);
				return;
			}
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo);
			foreach (Slice index in node.Indices)
			{
				methodInvocationExpression.Arguments.Add(index.Begin);
			}
			IMethod method = null;
			if (member.IsAmbiguous())
			{
				IEntity entity = ResolveAmbiguousPropertyReference((ReferenceExpression)node.Target, (Ambiguous)member, methodInvocationExpression.Arguments);
				IProperty property = entity as IProperty;
				if (null != property)
				{
					method = property.GetGetMethod();
				}
				else if (entity.IsAmbiguous())
				{
					Error(node);
					return;
				}
			}
			else if (EntityType.Property == member.EntityType)
			{
				method = ((IProperty)member).GetGetMethod();
			}
			if (null != method)
			{
				if (AssertParameters(node, method, methodInvocationExpression.Arguments))
				{
					Expression indexedPropertySlicingTarget = GetIndexedPropertySlicingTarget(node);
					methodInvocationExpression.Target = base.CodeBuilder.CreateMemberReference(indexedPropertySlicingTarget, method);
					BindExpressionType(methodInvocationExpression, method.ReturnType);
					node.ParentNode.Replace(node, methodInvocationExpression);
				}
				else
				{
					Error(node);
				}
			}
			else
			{
				NotImplemented(node, "slice for anything but arrays and default properties");
			}
		}

		private Expression GetIndexedPropertySlicingTarget(SlicingExpression node)
		{
			Expression target = node.Target;
			MemberReferenceExpression memberReferenceExpression = target as MemberReferenceExpression;
			if (null != memberReferenceExpression)
			{
				return memberReferenceExpression.Target;
			}
			return CreateSelfReference();
		}

		public override void LeaveExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.StringType);
		}

		public override void LeaveListLiteralExpression(ListLiteralExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.ListType);
			base.TypeSystemServices.MapToConcreteExpressionTypes(node.Items);
		}

		public override void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			BlockExpression blockExpression = new BlockExpression(node.LexicalInfo);
			Block block = blockExpression.Body;
			Expression expression = node.Items[0].Expression;
			foreach (GeneratorExpression item in node.Items)
			{
				ForStatement forStatement = new ForStatement(item.LexicalInfo);
				forStatement.Iterator = item.Iterator;
				forStatement.Declarations = item.Declarations;
				block.Add(forStatement);
				if (null == item.Filter)
				{
					block = forStatement.Block;
				}
				else
				{
					forStatement.Block.Add(NormalizeStatementModifiers.MapStatementModifier(item.Filter, out block));
				}
			}
			block.Add(new YieldStatement(expression.LexicalInfo, expression));
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo);
			methodInvocationExpression.Target = blockExpression;
			Node parentNode = node.ParentNode;
			bool flag = AstUtil.IsListMultiGenerator(parentNode);
			parentNode.Replace(node, methodInvocationExpression);
			methodInvocationExpression.Accept(this);
			if (flag)
			{
				parentNode.ParentNode.Replace(parentNode, base.CodeBuilder.CreateConstructorInvocation(base.TypeSystemServices.Map(ProcessGenerators.List_IEnumerableConstructor), methodInvocationExpression));
			}
		}

		public override void OnGeneratorExpression(GeneratorExpression node)
		{
			Visit(node.Iterator);
			node.Iterator = ProcessIterator(node.Iterator, node.Declarations);
			EnterNamespace(new DeclarationsNamespace(base.CurrentNamespace, node.Declarations));
			Visit(node.Filter);
			Visit(node.Expression);
			LeaveNamespace();
			IType concreteExpressionType = base.TypeSystemServices.GetConcreteExpressionType(node.Expression);
			BindExpressionType(node, GeneratorTypeOf(concreteExpressionType));
		}

		private IType GeneratorTypeOf(IType generatorItemType)
		{
			if (generatorItemType == base.TypeSystemServices.VoidType)
			{
				return TypeSystemServices.ErrorEntity;
			}
			return GetConstructedType(base.TypeSystemServices.IEnumerableGenericType, generatorItemType);
		}

		protected IType GetConstructedType(IType genericType, IType argType)
		{
			return genericType.GenericInfo.ConstructType(argType);
		}

		public override void LeaveHashLiteralExpression(HashLiteralExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.HashType);
			foreach (ExpressionPair item in node.Items)
			{
				GetConcreteExpressionType(item.First);
				GetConcreteExpressionType(item.Second);
			}
		}

		public override void LeaveArrayLiteralExpression(ArrayLiteralExpression node)
		{
			base.TypeSystemServices.MapToConcreteExpressionTypes(node.Items);
			IArrayType arrayType = InferArrayType(node);
			BindExpressionType(node, arrayType);
			if (node.Type == null)
			{
				node.Type = (ArrayTypeReference)base.CodeBuilder.CreateTypeReference(arrayType);
			}
			else
			{
				CheckItems(arrayType.ElementType, node.Items);
			}
		}

		private IArrayType InferArrayType(ArrayLiteralExpression node)
		{
			if (null != node.Type)
			{
				return (IArrayType)node.Type.Entity;
			}
			if (node.Items.Count == 0)
			{
				return EmptyArrayType.Default;
			}
			return GetMostGenericType(node.Items).MakeArrayType(1);
		}

		public override void LeaveDeclaration(Declaration node)
		{
			if (null != node.Type)
			{
				CheckDeclarationType(node.Type);
			}
		}

		public override void LeaveDeclarationStatement(DeclarationStatement node)
		{
			EnsureDeclarationType(node);
			AssertDeclarationName(node.Declaration);
			IType declarationType = GetDeclarationType(node);
			IEntity entity = DeclareLocal(node, node.Declaration.Name, declarationType);
			InternalLocal internalLocal = entity as InternalLocal;
			if (null != internalLocal)
			{
				internalLocal.OriginalDeclaration = node.Declaration;
			}
			if (node.Initializer != null)
			{
				IType expressionType = GetExpressionType(node.Initializer);
				if (CheckDeclarationType(node.Declaration.Type))
				{
					AssertTypeCompatibility(node.Initializer, declarationType, expressionType);
				}
				if (TypeSystemServices.IsNullable(declarationType) && !TypeSystemServices.IsNullable(expressionType))
				{
					BindNullableInitializer(node, node.Initializer, declarationType);
				}
				node.ReplaceBy(new ExpressionStatement(base.CodeBuilder.CreateAssignment(node.LexicalInfo, base.CodeBuilder.CreateReference(entity), node.Initializer)));
			}
			else
			{
				node.ReplaceBy(new ExpressionStatement(CreateDefaultLocalInitializer(node, entity)));
			}
		}

		private IType GetDeclarationType(DeclarationStatement node)
		{
			return GetType(node.Declaration.Type);
		}

		private void EnsureDeclarationType(DeclarationStatement node)
		{
			Declaration declaration = node.Declaration;
			if (declaration.Type == null)
			{
				declaration.Type = base.CodeBuilder.CreateTypeReference(declaration.LexicalInfo, InferDeclarationType(node));
			}
		}

		private IType InferDeclarationType(DeclarationStatement node)
		{
			if (null == node.Initializer)
			{
				return base.TypeSystemServices.ObjectType;
			}
			return MapWildcardType(GetConcreteExpressionType(node.Initializer));
		}

		protected virtual Expression CreateDefaultLocalInitializer(Node sourceNode, IEntity local)
		{
			return base.CodeBuilder.CreateDefaultInitializer(sourceNode.LexicalInfo, (InternalLocal)local);
		}

		public override void LeaveExpressionStatement(ExpressionStatement node)
		{
			AssertHasSideEffect(node.Expression);
		}

		public override void OnNullLiteralExpression(NullLiteralExpression node)
		{
			BindExpressionType(node, Null.Default);
		}

		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			if (null == _currentMethod)
			{
				Error(node, CompilerErrorFactory.SelfOutsideMethod(node));
				return;
			}
			if (_currentMethod.IsStatic && NodeType.MemberReferenceExpression != node.ParentNode.NodeType)
			{
				Error(CompilerErrorFactory.SelfIsNotValidInStaticMember(node));
			}
			node.Entity = _currentMethod;
			node.ExpressionType = _currentMethod.DeclaringType;
		}

		public override void LeaveTypeofExpression(TypeofExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.TypeType);
		}

		public override void LeaveCastExpression(CastExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			IType type = GetType(node.Type);
			BindExpressionType(node, type);
			if (IsError(expressionType) || IsError(type) || IsAssignableFrom(type, expressionType) || base.TypeSystemServices.CanBeReachedByPromotion(type, expressionType) || (base.TypeSystemServices.IsFloatingPointNumber(type) && expressionType.IsEnum))
			{
				return;
			}
			IMethod method = base.TypeSystemServices.FindExplicitConversionOperator(expressionType, type) ?? base.TypeSystemServices.FindImplicitConversionOperator(expressionType, type);
			if (null != method)
			{
				node.ParentNode.Replace(node, base.CodeBuilder.CreateMethodInvocation(method, node.Target));
				return;
			}
			if (type.IsValueType)
			{
				if (base.TypeSystemServices.IsSystemObject(expressionType))
				{
					return;
				}
			}
			else if (!expressionType.IsFinal)
			{
				return;
			}
			Error(CompilerErrorFactory.IncompatibleExpressionType(node, type, expressionType));
		}

		public override void LeaveTryCastExpression(TryCastExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			IType type = GetType(node.Type);
			if (expressionType.IsValueType)
			{
				Error(CompilerErrorFactory.CantCastToValueType(node.Target, expressionType));
			}
			else if (type.IsValueType)
			{
				Error(CompilerErrorFactory.CantCastToValueType(node.Type, type));
			}
			BindExpressionType(node, type);
		}

		protected Expression CreateMemberReferenceTarget(Node sourceNode, IMember member)
		{
			Expression expression = null;
			if (member.IsStatic)
			{
				expression = base.CodeBuilder.CreateReference(sourceNode.LexicalInfo, member.DeclaringType);
			}
			else
			{
				if (member.DeclaringType != CurrentType && !CurrentType.IsSubclassOf(member.DeclaringType))
				{
					Error(CompilerErrorFactory.InstanceRequired(sourceNode, member));
				}
				expression = new SelfLiteralExpression(sourceNode.LexicalInfo);
			}
			BindExpressionType(expression, member.DeclaringType);
			return expression;
		}

		protected MemberReferenceExpression MemberReferenceFromReference(ReferenceExpression node, IMember member)
		{
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(node.LexicalInfo);
			memberReferenceExpression.Name = node.Name;
			memberReferenceExpression.Target = CreateMemberReferenceTarget(node, member);
			return memberReferenceExpression;
		}

		private void ResolveMemberInfo(ReferenceExpression node, IMember member)
		{
			MemberReferenceExpression memberReferenceExpression = MemberReferenceFromReference(node, member);
			Bind(memberReferenceExpression, member);
			node.ParentNode.Replace(node, memberReferenceExpression);
			Visit(memberReferenceExpression);
		}

		public override void OnRELiteralExpression(RELiteralExpression node)
		{
			if (null == node.Entity)
			{
				IType regexType = base.TypeSystemServices.RegexType;
				BindExpressionType(node, regexType);
			}
		}

		public override void LeaveGenericReferenceExpression(GenericReferenceExpression node)
		{
			if (node.Target.Entity == null || IsError(node.Target.Entity))
			{
				BindExpressionType(node, TypeSystemServices.ErrorEntity);
				return;
			}
			IEntity entity = base.NameResolutionService.ResolveGenericReferenceExpression(node, node.Target.Entity);
			Bind(node, entity);
			if (entity.EntityType == EntityType.Type)
			{
				BindTypeReferenceExpressionType(node, (IType)entity);
				return;
			}
			if (entity.EntityType == EntityType.Method)
			{
				BindExpressionType(node, ((IMethod)entity).Type);
			}
			if (!(node.Target is MemberReferenceExpression))
			{
				node.Target = base.CodeBuilder.MemberReferenceForEntity(CreateSelfReference(), entity);
			}
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			if (AlreadyBound(node))
			{
				return;
			}
			IEntity entity = ResolveName(node, node.Name);
			if (null == entity)
			{
				Error(node);
				return;
			}
			if (AstUtil.IsTargetOfMethodInvocation(node) && !IsCallableEntity(entity))
			{
				IEntity entity2 = ResolveCallable(node);
				if (null != entity2)
				{
					entity = entity2;
				}
			}
			IMember member = entity as IMember;
			if (null != member)
			{
				if (IsExtensionMethod(member))
				{
					Bind(node, member);
				}
				else
				{
					ResolveMemberInfo(node, member);
				}
			}
			else
			{
				EnsureRelatedNodeWasVisited(node, entity);
				node.Entity = entity;
				PostProcessReferenceExpression(node);
			}
		}

		private static bool AlreadyBound(ReferenceExpression node)
		{
			return null != node.ExpressionType;
		}

		private IEntity ResolveCallable(ReferenceExpression node)
		{
			return base.NameResolutionService.Resolve(node.Name, EntityType.Type | EntityType.Method | EntityType.Event | EntityType.BuiltinFunction);
		}

		private bool IsCallableEntity(IEntity entity)
		{
			switch (entity.EntityType)
			{
			case EntityType.Type:
			case EntityType.Method:
			case EntityType.Constructor:
			case EntityType.Event:
			case EntityType.BuiltinFunction:
				return true;
			case EntityType.Ambiguous:
				return true;
			default:
			{
				ITypedEntity typedEntity = entity as ITypedEntity;
				return typedEntity != null && base.TypeSystemServices.IsCallable(typedEntity.Type);
			}
			}
		}

		private void PostProcessReferenceExpression(ReferenceExpression node)
		{
			IEntity entity = GetEntity(node);
			switch (entity.EntityType)
			{
			case EntityType.Type:
				BindNonGenericTypeReferenceExpressionType(node, (IType)entity);
				break;
			case EntityType.Method:
			{
				IMethod method = entity as IMethod;
				if (method != null && IsGenericMethod(method) && IsStandaloneReference(node) && !IsSubjectToGenericArgumentInference(node))
				{
					CannotInferGenericMethodArguments(node, method);
				}
				break;
			}
			case EntityType.Ambiguous:
			{
				Ambiguous ambiguous = (Ambiguous)entity;
				IEntity entity2 = ResolveAmbiguousReference(node, ambiguous);
				IMember member = entity2 as IMember;
				if (null != member)
				{
					ResolveMemberInfo(node, member);
				}
				else if (entity2 is IType)
				{
					BindNonGenericTypeReferenceExpressionType(node, (IType)entity2);
				}
				else if (!AstUtil.IsTargetOfMethodInvocation(node) && !AstUtil.IsTargetOfSlicing(node) && !node.IsTargetOfAssignment())
				{
					Error(node, CompilerErrorFactory.AmbiguousReference(node, node.Name, ambiguous.Entities));
				}
				break;
			}
			case EntityType.Namespace:
				if (IsStandaloneReference(node))
				{
					Error(node, CompilerErrorFactory.NamespaceIsNotAnExpression(node, entity.Name));
				}
				break;
			case EntityType.Local:
			case EntityType.Parameter:
			{
				ILocalEntity localEntity = (ILocalEntity)node.Entity;
				localEntity.IsUsed = true;
				BindExpressionType(node, localEntity.Type);
				break;
			}
			default:
				if (EntityType.BuiltinFunction == entity.EntityType)
				{
					CheckBuiltinUsage(node, entity);
				}
				else if (node.ExpressionType == null)
				{
					BindExpressionType(node, ((ITypedEntity)entity).Type);
				}
				break;
			}
		}

		private static bool IsGenericMethod(IMethod m)
		{
			return m.GenericInfo != null;
		}

		protected virtual void BindTypeReferenceExpressionType(Expression node, IType type)
		{
			if (IsStandaloneReference(node))
			{
				BindExpressionType(node, base.TypeSystemServices.TypeType);
			}
			else
			{
				BindExpressionType(node, type);
			}
		}

		protected virtual void BindNonGenericTypeReferenceExpressionType(Expression node, IType type)
		{
			if (type.GenericInfo != null && !IsSubjectToGenericArgumentInference(node))
			{
				My<CompilerErrorEmitter>.Instance.GenericArgumentsCountMismatch(node, type);
				Error(node);
			}
			else
			{
				BindTypeReferenceExpressionType(node, type);
			}
		}

		private bool IsSubjectToGenericArgumentInference(Expression node)
		{
			return AstUtil.IsTargetOfGenericReferenceExpression(node) || AstUtil.IsTargetOfMethodInvocation(node);
		}

		protected virtual void CheckBuiltinUsage(ReferenceExpression node, IEntity entity)
		{
			if (!AstUtil.IsTargetOfMethodInvocation(node))
			{
				Error(node, CompilerErrorFactory.BuiltinCannotBeUsedAsExpression(node, entity.Name));
			}
		}

		public override bool EnterMemberReferenceExpression(MemberReferenceExpression node)
		{
			return null == node.ExpressionType;
		}

		private INamespace GetReferenceNamespace(MemberReferenceExpression expression)
		{
			Expression target = expression.Target;
			INamespace expressionType = target.ExpressionType;
			if (null != expressionType)
			{
				return GetConcreteExpressionType(target);
			}
			return (INamespace)GetEntity(target);
		}

		protected virtual void LeaveExplodeExpression(UnaryExpression node)
		{
			IType concreteExpressionType = GetConcreteExpressionType(node.Operand);
			if (!concreteExpressionType.IsArray)
			{
				Error(node, CompilerErrorFactory.ExplodedExpressionMustBeArray(node));
			}
			else
			{
				BindExpressionType(node, concreteExpressionType);
			}
		}

		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			_context.TraceVerbose("LeaveMemberReferenceExpression: {0}", node);
			if (TypeSystemServices.IsError(node.Target))
			{
				Error(node);
			}
			else
			{
				ProcessMemberReferenceExpression(node);
			}
		}

		protected virtual void MemberNotFound(MemberReferenceExpression node, INamespace ns)
		{
			EntityType elementType = ((!AstUtil.IsTargetOfMethodInvocation(node)) ? EntityType.Any : EntityType.Method);
			Error(node, CompilerErrorFactory.MemberNotFound(node, ns, base.NameResolutionService.GetMostSimilarMemberName(ns, node.Name, elementType)));
		}

		protected virtual bool ShouldRebindMember(IEntity entity)
		{
			return entity == null;
		}

		private IEntity ResolveMember(MemberReferenceExpression node)
		{
			IEntity entity = node.Entity;
			if (!ShouldRebindMember(entity))
			{
				return entity;
			}
			INamespace referenceNamespace = GetReferenceNamespace(node);
			IEntity entity2 = base.NameResolutionService.Resolve(referenceNamespace, node.Name);
			if (entity2 == null || !IsAccessible(entity2) || !IsApplicable(entity2, node))
			{
				IEntity entity3 = TryToResolveMemberAsExtension(node);
				if (null != entity3)
				{
					return entity3;
				}
			}
			if (null != entity2)
			{
				return Disambiguate(node, entity2);
			}
			MemberNotFound(node, referenceNamespace);
			return null;
		}

		private bool IsApplicable(IEntity entity, MemberReferenceExpression node)
		{
			if (node == null || node.ParentNode == null)
			{
				return true;
			}
			if (AstUtil.IsTargetOfMethodInvocation(node) && !IsCallableEntity(entity))
			{
				return false;
			}
			return true;
		}

		private IEntity Disambiguate(ReferenceExpression node, IEntity member)
		{
			Ambiguous ambiguous = member as Ambiguous;
			if (ambiguous != null)
			{
				return ResolveAmbiguousReference(node, ambiguous);
			}
			return member;
		}

		private IEntity TryToResolveMemberAsExtension(MemberReferenceExpression node)
		{
			IEntity entity = base.NameResolutionService.ResolveExtension(GetReferenceNamespace(node), node.Name);
			if (null != entity)
			{
				node.Annotate(ResolvedAsExtensionAnnotation);
			}
			return entity;
		}

		protected virtual void ProcessMemberReferenceExpression(MemberReferenceExpression node)
		{
			IEntity entity = ResolveMember(node);
			if (null == entity)
			{
				return;
			}
			EnsureRelatedNodeWasVisited(node, entity);
			if (EntityType.Namespace == entity.EntityType)
			{
				MarkRelatedImportAsUsed(node);
			}
			IMember member = entity as IMember;
			if (member != null)
			{
				if (!AssertTargetContext(node, member))
				{
					Error(node);
					return;
				}
				if (EntityType.Method != member.EntityType)
				{
					BindExpressionType(node, GetInferredType(member));
				}
				else
				{
					BindExpressionType(node, member.Type);
				}
			}
			if (EntityType.Property == entity.EntityType)
			{
				IProperty property = (IProperty)entity;
				if (property.IsIndexedProperty() && !AstUtil.IsTargetOfSlicing(node) && (!property.IsExtension || property.GetParameters().Length > 1))
				{
					Error(node, CompilerErrorFactory.PropertyRequiresParameters(MemberAnchorFor(node), entity));
					return;
				}
				if (IsWriteOnlyProperty(property) && !IsBeingAssignedTo(node))
				{
					Error(node, CompilerErrorFactory.PropertyIsWriteOnly(MemberAnchorFor(node), entity));
				}
			}
			else if (EntityType.Event == entity.EntityType && !AstUtil.IsTargetOfMethodInvocation(node) && !AstUtil.IsLhsOfInPlaceAddSubtract(node))
			{
				if (CurrentType == member.DeclaringType)
				{
					InternalEvent internalEvent = (InternalEvent)entity;
					node.Name = internalEvent.BackingField.Name;
					node.Entity = internalEvent.BackingField;
					BindExpressionType(node, internalEvent.BackingField.Type);
					return;
				}
				if (!node.IsTargetOfAssignment() || !IsNull(((BinaryExpression)node.ParentNode).Right))
				{
					Error(node, CompilerErrorFactory.EventIsNotAnExpression(node, entity));
				}
				else
				{
					EnsureInternalEventInvocation((IEvent)entity, node);
				}
			}
			Bind(node, entity);
			PostProcessReferenceExpression(node);
		}

		private static Node MemberAnchorFor(Node node)
		{
			return AstUtil.GetMemberAnchor(node);
		}

		private void MarkRelatedImportAsUsed(MemberReferenceExpression node)
		{
			string text = null;
			foreach (Import import in _currentModule.Imports)
			{
				if (!ImportAnnotations.IsUsedImport(import))
				{
					if (null == text)
					{
						text = node.ToCodeString();
					}
					if (import.Namespace == text)
					{
						ImportAnnotations.MarkAsUsed(import);
						break;
					}
				}
			}
		}

		private bool IsBeingAssignedTo(MemberReferenceExpression node)
		{
			Node node2 = node;
			Node parentNode = node2.ParentNode;
			BinaryExpression binaryExpression = parentNode as BinaryExpression;
			while (null == binaryExpression)
			{
				node2 = parentNode;
				parentNode = parentNode.ParentNode;
				if (parentNode == null || !(parentNode is Expression))
				{
					return false;
				}
				binaryExpression = parentNode as BinaryExpression;
			}
			return binaryExpression.Left == node2;
		}

		private bool IsWriteOnlyProperty(IProperty property)
		{
			return null == property.GetGetMethod();
		}

		private IEntity ResolveAmbiguousLValue(Expression sourceNode, Ambiguous candidates, Expression rvalue)
		{
			if (!candidates.AllEntitiesAre(EntityType.Property))
			{
				return null;
			}
			IEntity[] entities = candidates.Entities;
			IEntity[] setMethods = GetSetMethods(entities);
			ExpressionCollection expressionCollection = new ExpressionCollection();
			expressionCollection.Add(rvalue);
			IEntity correctCallableReference = GetCorrectCallableReference(sourceNode, expressionCollection, setMethods);
			if (correctCallableReference != null && EntityType.Method == correctCallableReference.EntityType)
			{
				IProperty property = (IProperty)entities[GetIndex(setMethods, correctCallableReference)];
				BindProperty(sourceNode, property);
				return property;
			}
			return null;
		}

		private static void BindProperty(Expression expression, IProperty property)
		{
			expression.Entity = property;
			expression.ExpressionType = property.Type;
		}

		private IEntity ResolveAmbiguousReference(ReferenceExpression node, Ambiguous candidates)
		{
			IEntity entity = ResolveAmbiguousReferenceByAccessibility(candidates);
			Ambiguous ambiguous = entity as Ambiguous;
			if (ambiguous == null || AstUtil.IsTargetOfSlicing(node) || node.IsTargetOfAssignment())
			{
				return entity;
			}
			if (ambiguous.AllEntitiesAre(EntityType.Property))
			{
				return ResolveAmbiguousPropertyReference(node, ambiguous, EmptyExpressionCollection);
			}
			if (ambiguous.AllEntitiesAre(EntityType.Method))
			{
				return ResolveAmbiguousMethodReference(node, ambiguous, EmptyExpressionCollection);
			}
			if (ambiguous.AllEntitiesAre(EntityType.Type))
			{
				return ResolveAmbiguousTypeReference(node, ambiguous);
			}
			return entity;
		}

		private IEntity ResolveAmbiguousMethodReference(ReferenceExpression node, Ambiguous candidates, ExpressionCollection args)
		{
			if (!AstUtil.IsTargetOfMethodInvocation(node) && !AstUtil.IsTargetOfSlicing(node) && !node.IsTargetOfAssignment())
			{
				return candidates.Entities[0];
			}
			return candidates;
		}

		private IEntity ResolveAmbiguousPropertyReference(ReferenceExpression node, Ambiguous candidates, ExpressionCollection args)
		{
			IEntity[] entities = candidates.Entities;
			IEntity[] getMethods = GetGetMethods(entities);
			IEntity correctCallableReference = GetCorrectCallableReference(node, args, getMethods);
			if (correctCallableReference != null && EntityType.Method == correctCallableReference.EntityType)
			{
				IProperty property = (IProperty)entities[GetIndex(getMethods, correctCallableReference)];
				BindProperty(node, property);
				return property;
			}
			return candidates;
		}

		private IEntity ResolveAmbiguousTypeReference(ReferenceExpression node, Ambiguous candidates)
		{
			List<IEntity> matchesByGenericity = GetMatchesByGenericity(node, candidates);
			if (matchesByGenericity.Count > 1)
			{
				PreferInternalEntitiesOverNonInternal(matchesByGenericity);
			}
			if (matchesByGenericity.Count == 1)
			{
				Bind(node, matchesByGenericity[0]);
			}
			else
			{
				Bind(node, new Ambiguous(matchesByGenericity));
			}
			return node.Entity;
		}

		private static void PreferInternalEntitiesOverNonInternal(List<IEntity> matches)
		{
			if (matches.Contains(EntityPredicates.IsInternalEntity) && matches.Contains(EntityPredicates.IsNonInternalEntity))
			{
				matches.RemoveAll(EntityPredicates.IsNonInternalEntity);
			}
		}

		private List<IEntity> GetMatchesByGenericity(ReferenceExpression node, Ambiguous candidates)
		{
			bool flag = node.ParentNode is GenericReferenceExpression;
			List<IEntity> list = new List<IEntity>();
			IEntity[] entities = candidates.Entities;
			foreach (IEntity entity in entities)
			{
				IType type = entity as IType;
				bool flag2 = type != null && type.GenericInfo != null;
				if (flag2 == flag)
				{
					list.Add(entity);
				}
			}
			return list;
		}

		private IEntity ResolveAmbiguousReferenceByAccessibility(Ambiguous candidates)
		{
			List<IEntity> list = new List<IEntity>();
			IEntity[] entities = candidates.Entities;
			foreach (IEntity entity in entities)
			{
				if (!IsInaccessible(entity))
				{
					list.Add(entity);
				}
			}
			return Entities.EntityFromList(list);
		}

		private int GetIndex(IEntity[] entities, IEntity entity)
		{
			for (int i = 0; i < entities.Length; i++)
			{
				if (entities[i] == entity)
				{
					return i;
				}
			}
			throw new ArgumentException("entity");
		}

		public override void LeaveConditionalExpression(ConditionalExpression node)
		{
			IType expressionType = GetExpressionType(node.TrueValue);
			IType expressionType2 = GetExpressionType(node.FalseValue);
			BindExpressionType(node, GetMostGenericType(expressionType, expressionType2));
		}

		public override void LeaveYieldStatement(YieldStatement node)
		{
			if (EntityType.Constructor == _currentMethod.EntityType)
			{
				Error(CompilerErrorFactory.YieldInsideConstructor(node));
			}
			else
			{
				_currentMethod.AddYieldStatement(node);
			}
		}

		public override void LeaveReturnStatement(ReturnStatement node)
		{
			if (null == node.Expression)
			{
				return;
			}
			IType concreteExpressionType = GetConcreteExpressionType(node.Expression);
			if (base.TypeSystemServices.VoidType == concreteExpressionType && node.ContainsAnnotation(OptionalReturnStatementAnnotation))
			{
				node.ParentNode.Replace(node, new ExpressionStatement(node.Expression));
				return;
			}
			IType returnType = _currentMethod.ReturnType;
			if (TypeSystemServices.IsUnknown(returnType))
			{
				_currentMethod.AddReturnExpression(node.Expression);
			}
			else
			{
				AssertTypeCompatibility(node.Expression, returnType, concreteExpressionType);
			}
			if (TypeSystemServices.IsNullable(concreteExpressionType) && !TypeSystemServices.IsNullable(returnType))
			{
				MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(node.Expression.LexicalInfo, node.Expression, "Value");
				Visit(memberReferenceExpression);
				node.Replace(node.Expression, memberReferenceExpression);
			}
		}

		protected Expression GetCorrectIterator(Expression iterator)
		{
			IType expressionType = GetExpressionType(iterator);
			if (IsError(expressionType))
			{
				return iterator;
			}
			if (!IsAssignableFrom(base.TypeSystemServices.IEnumerableType, expressionType) && !IsAssignableFrom(base.TypeSystemServices.IEnumeratorType, expressionType) && IsRuntimeIterator(expressionType))
			{
				return IsTextReader(expressionType) ? base.CodeBuilder.CreateMethodInvocation(MethodCache.TextReaderEnumerator_lines, iterator) : base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_GetEnumerable, iterator);
			}
			return iterator;
		}

		protected Expression ProcessIterator(Expression iterator, DeclarationCollection declarations)
		{
			iterator = GetCorrectIterator(iterator);
			ProcessDeclarationsForIterator(declarations, GetExpressionType(iterator));
			return iterator;
		}

		public override void OnGotoStatement(GotoStatement node)
		{
		}

		public override void OnForStatement(ForStatement node)
		{
			Visit(node.Iterator);
			node.Iterator = ProcessIterator(node.Iterator, node.Declarations);
			VisitForStatementBlock(node);
		}

		protected void VisitForStatementBlock(ForStatement node)
		{
			EnterForNamespace(node);
			Visit(node.Block);
			Visit(node.OrBlock);
			Visit(node.ThenBlock);
			LeaveNamespace();
		}

		private void EnterForNamespace(ForStatement node)
		{
			EnterNamespace(new DeclarationsNamespace(base.CurrentNamespace, node.Declarations));
		}

		public override void OnUnpackStatement(UnpackStatement node)
		{
			Visit(node.Expression);
			node.Expression = GetCorrectIterator(node.Expression);
			IType enumeratorItemType = GetEnumeratorItemType(GetExpressionType(node.Expression));
			foreach (Declaration declaration in node.Declarations)
			{
				bool flag = declaration.Type != null;
				GetDeclarationType(enumeratorItemType, declaration);
				if (flag)
				{
					AssertUniqueLocal(declaration);
				}
				else
				{
					IEntity entity = TryToResolveName(declaration.Name);
					if (null != entity)
					{
						Bind(declaration, entity);
						AssertLValue(declaration, entity);
						continue;
					}
				}
				DeclareLocal(declaration, privateScope: false);
			}
		}

		public override void LeaveRaiseStatement(RaiseStatement node)
		{
			if (node.Exception == null)
			{
				return;
			}
			IType expressionType = GetExpressionType(node.Exception);
			if (!IsError(expressionType))
			{
				if (base.TypeSystemServices.StringType == expressionType)
				{
					node.Exception = base.CodeBuilder.CreateConstructorInvocation(node.Exception.LexicalInfo, MethodCache.Exception_StringConstructor, node.Exception);
				}
				else if (!base.TypeSystemServices.IsValidException(expressionType))
				{
					Error(CompilerErrorFactory.InvalidRaiseArgument(node.Exception, expressionType));
				}
			}
		}

		public override void OnExceptionHandler(ExceptionHandler node)
		{
			bool flag = (node.Flags & ExceptionHandlerFlags.Untyped) == ExceptionHandlerFlags.Untyped;
			bool flag2 = (node.Flags & ExceptionHandlerFlags.Anonymous) == ExceptionHandlerFlags.Anonymous;
			bool flag3 = (node.Flags & ExceptionHandlerFlags.Filter) == ExceptionHandlerFlags.Filter;
			if (flag)
			{
				node.Declaration.Type = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.ExceptionType);
			}
			else
			{
				Visit(node.Declaration.Type);
				if (!base.TypeSystemServices.IsValidException(GetType(node.Declaration.Type)))
				{
					base.Errors.Add(CompilerErrorFactory.InvalidExceptArgument(node.Declaration.Type, GetType(node.Declaration.Type)));
				}
			}
			if (!flag2)
			{
				DeclareLocal(node.Declaration, privateScope: true);
				EnterNamespace(new DeclarationsNamespace(base.CurrentNamespace, node.Declaration));
			}
			try
			{
				if (flag3)
				{
					Visit(node.FilterCondition);
				}
				Visit(node.Block);
			}
			finally
			{
				if (!flag2)
				{
					LeaveNamespace();
				}
			}
		}

		protected virtual bool IsValidIncrementDecrementOperand(Expression e)
		{
			IType type = GetExpressionType(e);
			if (type.IsPointer)
			{
				return true;
			}
			if (TypeSystemServices.IsNullable(type))
			{
				type = base.TypeSystemServices.GetNullableUnderlyingType(type);
			}
			return base.TypeSystemServices.IsNumber(type) || base.TypeSystemServices.IsDuckType(type);
		}

		private void LeaveIncrementDecrement(UnaryExpression node)
		{
			if (AssertLValue(node.Operand))
			{
				if (!IsValidIncrementDecrementOperand(node.Operand))
				{
					InvalidOperatorForType(node);
				}
				else
				{
					ExpandIncrementDecrement(node);
				}
			}
			else
			{
				Error(node);
			}
		}

		private void ExpandIncrementDecrement(UnaryExpression node)
		{
			Expression expression = (IsArraySlicing(node.Operand) ? ExpandIncrementDecrementArraySlicing(node) : ExpandSimpleIncrementDecrement(node));
			node.ParentNode.Replace(node, expression);
			Visit(expression);
		}

		private Expression ExpandIncrementDecrementArraySlicing(UnaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Operand;
			AssertIsNotComplexSlicing(slicingExpression);
			Visit(slicingExpression);
			return CreateSideEffectAwareSlicingOperation(node.LexicalInfo, GetEquivalentBinaryOperator(node.Operator), slicingExpression, base.CodeBuilder.CreateIntegerLiteral(1), DeclareOldValueTempIfNeeded(node));
		}

		private Expression CreateSideEffectAwareSlicingOperation(LexicalInfo lexicalInfo, BinaryOperatorType binaryOperator, SlicingExpression lvalue, Expression rvalue, InternalLocal returnValue)
		{
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(lexicalInfo);
			if (HasSideEffect(lvalue.Target))
			{
				InternalLocal local = AddInitializedTempLocal(methodInvocationExpression, lvalue.Target);
				lvalue.Target = base.CodeBuilder.CreateReference(local);
			}
			foreach (Slice index in lvalue.Indices)
			{
				Expression begin = index.Begin;
				if (HasSideEffect(begin))
				{
					InternalLocal local = AddInitializedTempLocal(methodInvocationExpression, begin);
					index.Begin = base.CodeBuilder.CreateReference(local);
				}
			}
			BinaryExpression binaryExpression = base.CodeBuilder.CreateBoundBinaryExpression(GetExpressionType(lvalue), binaryOperator, CloneOrAssignToTemp(returnValue, lvalue), rvalue);
			Expression expression = base.CodeBuilder.CreateAssignment(lvalue.CloneNode(), binaryExpression);
			BindArithmeticOperator(binaryExpression);
			if (methodInvocationExpression.Arguments.Count > 0 || null != returnValue)
			{
				methodInvocationExpression.Arguments.Add(expression);
				if (null != returnValue)
				{
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(returnValue));
				}
				BindExpressionType(methodInvocationExpression, GetExpressionType(lvalue));
				expression = methodInvocationExpression;
			}
			return expression;
		}

		private InternalLocal AddInitializedTempLocal(MethodInvocationExpression eval, Expression initializer)
		{
			InternalLocal internalLocal = DeclareTempLocal(GetExpressionType(initializer));
			eval.Arguments.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(internalLocal), initializer));
			return internalLocal;
		}

		private InternalLocal DeclareOldValueTempIfNeeded(UnaryExpression node)
		{
			return AstUtil.IsPostUnaryOperator(node.Operator) ? DeclareTempLocal(GetExpressionType(node.Operand)) : null;
		}

		private Expression ExpandSimpleIncrementDecrement(UnaryExpression node)
		{
			InternalLocal internalLocal = DeclareOldValueTempIfNeeded(node);
			IType expressionType = GetExpressionType(node.Operand);
			BinaryExpression binaryExpression = base.CodeBuilder.CreateBoundBinaryExpression(expressionType, GetEquivalentBinaryOperator(node.Operator), CloneOrAssignToTemp(internalLocal, node.Operand), base.CodeBuilder.CreateIntegerLiteral(1));
			BinaryExpression binaryExpression2 = base.CodeBuilder.CreateAssignment(node.LexicalInfo, node.Operand, binaryExpression);
			BindArithmeticOperator(binaryExpression);
			return (internalLocal == null) ? ((Expression)binaryExpression2) : ((Expression)base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo, binaryExpression2, base.CodeBuilder.CreateReference(internalLocal)));
		}

		private Expression CloneOrAssignToTemp(InternalLocal temp, Expression operand)
		{
			return (temp == null) ? operand.CloneNode() : base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(temp), operand.CloneNode());
		}

		private static BinaryOperatorType GetEquivalentBinaryOperator(UnaryOperatorType op)
		{
			switch (op)
			{
			case UnaryOperatorType.Increment:
			case UnaryOperatorType.PostIncrement:
				return BinaryOperatorType.Addition;
			case UnaryOperatorType.Decrement:
			case UnaryOperatorType.PostDecrement:
				return BinaryOperatorType.Subtraction;
			default:
				throw new ArgumentException("op");
			}
		}

		private static UnaryOperatorType GetRelatedPreOperator(UnaryOperatorType op)
		{
			return op switch
			{
				UnaryOperatorType.PostIncrement => UnaryOperatorType.Increment, 
				UnaryOperatorType.PostDecrement => UnaryOperatorType.Decrement, 
				_ => throw new ArgumentException("op"), 
			};
		}

		public override bool EnterUnaryExpression(UnaryExpression node)
		{
			if (AstUtil.IsPostUnaryOperator(node.Operator) && NodeType.ExpressionStatement == node.ParentNode.NodeType)
			{
				node.Operator = GetRelatedPreOperator(node.Operator);
			}
			return true;
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			switch (node.Operator)
			{
			case UnaryOperatorType.Explode:
				LeaveExplodeExpression(node);
				break;
			case UnaryOperatorType.LogicalNot:
				LeaveLogicalNot(node);
				break;
			case UnaryOperatorType.Increment:
			case UnaryOperatorType.Decrement:
			case UnaryOperatorType.PostIncrement:
			case UnaryOperatorType.PostDecrement:
				LeaveIncrementDecrement(node);
				break;
			case UnaryOperatorType.UnaryNegation:
				LeaveUnaryNegation(node);
				break;
			case UnaryOperatorType.OnesComplement:
				LeaveOnesComplement(node);
				break;
			case UnaryOperatorType.AddressOf:
				LeaveAddressOf(node);
				break;
			case UnaryOperatorType.Indirection:
				LeaveIndirection(node);
				break;
			default:
				NotImplemented(node, "unary operator not supported");
				break;
			}
		}

		private void LeaveOnesComplement(UnaryExpression node)
		{
			if (IsPrimitiveOnesComplementOperand(node.Operand))
			{
				BindExpressionType(node, GetExpressionType(node.Operand));
			}
			else
			{
				ProcessOperatorOverload(node);
			}
		}

		private bool IsPrimitiveOnesComplementOperand(Expression operand)
		{
			IType expressionType = GetExpressionType(operand);
			return base.TypeSystemServices.IsIntegerNumber(expressionType) || expressionType.IsEnum;
		}

		private void LeaveLogicalNot(UnaryExpression node)
		{
			BindExpressionType(node, base.TypeSystemServices.BoolType);
		}

		private void LeaveUnaryNegation(UnaryExpression node)
		{
			if (IsPrimitiveNumber(node.Operand))
			{
				BindExpressionType(node, GetExpressionType(node.Operand));
			}
			else
			{
				ProcessOperatorOverload(node);
			}
		}

		private void LeaveAddressOf(UnaryExpression node)
		{
			IType type = GetExpressionType(node.Operand);
			if (type.IsArray)
			{
				type = type.ElementType;
				node.Replace(node.Operand, new SlicingExpression(node.Operand, new IntegerLiteralExpression(0L)));
				BindExpressionType(node.Operand, type);
			}
			if (base.TypeSystemServices.IsPointerCompatible(type))
			{
				node.Entity = type.MakePointerType();
				BindExpressionType(node, type.MakePointerType());
			}
			else
			{
				BindExpressionType(node, TypeSystemServices.ErrorEntity);
				Error(CompilerErrorFactory.PointerIncompatibleType(node.Operand, type));
			}
		}

		private void LeaveIndirection(UnaryExpression node)
		{
			if (!TypeSystemServices.IsError(node.Operand))
			{
				IType elementType = GetExpressionType(node.Operand).ElementType;
				if (elementType != null && base.TypeSystemServices.IsPointerCompatible(elementType))
				{
					node.Entity = node.Operand.Entity;
					BindExpressionType(node, elementType);
				}
				else
				{
					BindExpressionType(node, TypeSystemServices.ErrorEntity);
					Error(CompilerErrorFactory.PointerIncompatibleType(node.Operand, elementType));
				}
			}
		}

		private void ProcessOperatorOverload(UnaryExpression node)
		{
			if (!ResolveOperator(node))
			{
				InvalidOperatorForType(node);
			}
		}

		public override bool EnterBinaryExpression(BinaryExpression node)
		{
			if (BinaryOperatorType.Assign != node.Operator)
			{
				return true;
			}
			if (NodeType.ReferenceExpression != node.Left.NodeType)
			{
				return true;
			}
			if (node.Left.Entity != null)
			{
				return true;
			}
			ReferenceExpression referenceExpression = (ReferenceExpression)node.Left;
			IEntity entity = TryToResolveName(referenceExpression.Name);
			if (entity == null || base.TypeSystemServices.IsBuiltin(entity) || IsInaccessible(entity))
			{
				ProcessAutoLocalDeclaration(node, referenceExpression);
				return false;
			}
			return true;
		}

		protected virtual void ProcessAutoLocalDeclaration(BinaryExpression node, ReferenceExpression reference)
		{
			Visit(node.Right);
			IType type = MapWildcardType(GetConcreteExpressionType(node.Right));
			IEntity entity2 = (reference.Entity = DeclareLocal(reference, reference.Name, type));
			BindExpressionType(reference, type);
			BindExpressionType(node, type);
		}

		private bool IsInaccessible(IEntity info)
		{
			IAccessibleMember accessibleMember = info as IAccessibleMember;
			if (accessibleMember != null && accessibleMember.IsPrivate && accessibleMember.DeclaringType != CurrentType)
			{
				return true;
			}
			return false;
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (TypeSystemServices.IsUnknown(node.Left) || TypeSystemServices.IsUnknown(node.Right))
			{
				BindExpressionType(node, Unknown.Default);
			}
			else if (TypeSystemServices.IsError(node.Left) || TypeSystemServices.IsError(node.Right))
			{
				Error(node);
			}
			else
			{
				BindBinaryExpression(node);
			}
		}

		protected virtual void BindBinaryExpression(BinaryExpression node)
		{
			if (IsEnumOperation(node))
			{
				BindEnumOperation(node);
				return;
			}
			switch (node.Operator)
			{
			case BinaryOperatorType.Assign:
				BindAssignment(node);
				break;
			case BinaryOperatorType.Addition:
				if (GetExpressionType(node.Left).IsArray && GetExpressionType(node.Right).IsArray)
				{
					BindArrayAddition(node);
				}
				else
				{
					BindArithmeticOperator(node);
				}
				break;
			case BinaryOperatorType.Subtraction:
			case BinaryOperatorType.Multiply:
			case BinaryOperatorType.Division:
			case BinaryOperatorType.Modulus:
			case BinaryOperatorType.Exponentiation:
				BindArithmeticOperator(node);
				break;
			case BinaryOperatorType.TypeTest:
				BindTypeTest(node);
				break;
			case BinaryOperatorType.ReferenceEquality:
				BindReferenceEquality(node);
				break;
			case BinaryOperatorType.ReferenceInequality:
				BindReferenceEquality(node, inequality: true);
				break;
			case BinaryOperatorType.Or:
			case BinaryOperatorType.And:
				BindExpressionType(node, GetMostGenericType(node));
				break;
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
			case BinaryOperatorType.ShiftLeft:
			case BinaryOperatorType.ShiftRight:
				BindBitwiseOperator(node);
				break;
			case BinaryOperatorType.InPlaceAddition:
			case BinaryOperatorType.InPlaceSubtraction:
				BindInPlaceAddSubtract(node);
				break;
			case BinaryOperatorType.InPlaceMultiply:
			case BinaryOperatorType.InPlaceDivision:
			case BinaryOperatorType.InPlaceModulus:
			case BinaryOperatorType.InPlaceBitwiseAnd:
			case BinaryOperatorType.InPlaceBitwiseOr:
			case BinaryOperatorType.InPlaceExclusiveOr:
			case BinaryOperatorType.InPlaceShiftLeft:
			case BinaryOperatorType.InPlaceShiftRight:
				BindInPlaceArithmeticOperator(node);
				break;
			case BinaryOperatorType.LessThan:
			case BinaryOperatorType.LessThanOrEqual:
			case BinaryOperatorType.GreaterThan:
			case BinaryOperatorType.GreaterThanOrEqual:
			case BinaryOperatorType.Equality:
			case BinaryOperatorType.Inequality:
				BindCmpOperator(node);
				break;
			default:
				if (!ResolveOperator(node))
				{
					InvalidOperatorForTypes(node);
				}
				break;
			}
		}

		private IType GetMostGenericType(BinaryExpression node)
		{
			return GetMostGenericType(GetExpressionType(node.Left), GetExpressionType(node.Right));
		}

		private bool IsNullableOperation(BinaryExpression node)
		{
			if (node.Left.ExpressionType == null || null == node.Right.ExpressionType)
			{
				return false;
			}
			return TypeSystemServices.IsNullable(GetExpressionType(node.Left)) || TypeSystemServices.IsNullable(GetExpressionType(node.Right));
		}

		private bool IsEnumOperation(BinaryExpression node)
		{
			switch (node.Operator)
			{
			case BinaryOperatorType.Addition:
			case BinaryOperatorType.Subtraction:
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
			{
				IType expressionType = GetExpressionType(node.Left);
				IType expressionType2 = GetExpressionType(node.Right);
				if (expressionType.IsEnum)
				{
					return IsValidEnumOperand(expressionType, expressionType2);
				}
				if (expressionType2.IsEnum)
				{
					return IsValidEnumOperand(expressionType2, expressionType);
				}
				break;
			}
			}
			return false;
		}

		private bool IsValidEnumOperand(IType expected, IType actual)
		{
			if (expected == actual)
			{
				return true;
			}
			if (actual.IsEnum)
			{
				return true;
			}
			return base.TypeSystemServices.IsIntegerNumber(actual);
		}

		private void BindEnumOperation(BinaryExpression node)
		{
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			switch (node.Operator)
			{
			case BinaryOperatorType.Addition:
				if (expressionType.IsEnum != expressionType2.IsEnum)
				{
					BindExpressionType(node, expressionType.IsEnum ? expressionType : expressionType2);
					return;
				}
				break;
			case BinaryOperatorType.Subtraction:
				if (expressionType == expressionType2)
				{
					BindExpressionType(node, base.TypeSystemServices.IntType);
					return;
				}
				if (expressionType.IsEnum && !expressionType2.IsEnum)
				{
					BindExpressionType(node, expressionType);
					return;
				}
				break;
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
				if (expressionType == expressionType2)
				{
					BindExpressionType(node, expressionType);
					return;
				}
				break;
			}
			if (!ResolveOperator(node))
			{
				InvalidOperatorForTypes(node);
			}
		}

		private void BindBitwiseOperator(BinaryExpression node)
		{
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			if (base.TypeSystemServices.IsIntegerOrBool(expressionType) && base.TypeSystemServices.IsIntegerOrBool(expressionType2))
			{
				IType type;
				switch (node.Operator)
				{
				case BinaryOperatorType.ShiftLeft:
				case BinaryOperatorType.ShiftRight:
					type = expressionType;
					break;
				default:
					type = base.TypeSystemServices.GetPromotedNumberType(expressionType, expressionType2);
					break;
				}
				BindExpressionType(node, type);
			}
			else if (!ResolveOperator(node))
			{
				InvalidOperatorForTypes(node);
			}
		}

		private bool IsChar(IType type)
		{
			return base.TypeSystemServices.CharType == type;
		}

		private void BindCmpOperator(BinaryExpression node)
		{
			if (BindNullableComparison(node))
			{
				return;
			}
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			if (IsPrimitiveComparison(expressionType, expressionType2))
			{
				BindExpressionType(node, base.TypeSystemServices.BoolType);
			}
			else if (expressionType.IsEnum || expressionType2.IsEnum)
			{
				if (expressionType == expressionType2 || base.TypeSystemServices.IsPrimitiveNumber(expressionType2) || base.TypeSystemServices.IsPrimitiveNumber(expressionType))
				{
					BindExpressionType(node, base.TypeSystemServices.BoolType);
				}
				else if (!ResolveOperator(node))
				{
					InvalidOperatorForTypes(node);
				}
			}
			else
			{
				if (ResolveOperator(node))
				{
					return;
				}
				switch (node.Operator)
				{
				case BinaryOperatorType.Equality:
					if (OptimizeNullComparisons && (IsNull(node.Left) || IsNull(node.Right)))
					{
						node.Operator = BinaryOperatorType.ReferenceEquality;
						BindReferenceEquality(node);
					}
					else
					{
						Expression node2 = CreateEquals(node);
						node.ParentNode.Replace(node, node2);
					}
					break;
				case BinaryOperatorType.Inequality:
					if (OptimizeNullComparisons && (IsNull(node.Left) || IsNull(node.Right)))
					{
						node.Operator = BinaryOperatorType.ReferenceInequality;
						BindReferenceEquality(node, inequality: true);
					}
					else
					{
						Expression node2 = CreateEquals(node);
						Node parentNode = node.ParentNode;
						parentNode.Replace(node, base.CodeBuilder.CreateNotExpression(node2));
					}
					break;
				default:
					InvalidOperatorForTypes(node);
					break;
				}
			}
		}

		private bool IsPrimitiveComparison(IType lhs, IType rhs)
		{
			if (IsPrimitiveNumberOrChar(lhs) && IsPrimitiveNumberOrChar(rhs))
			{
				return true;
			}
			if (IsBool(lhs) && IsBool(rhs))
			{
				return true;
			}
			return false;
		}

		private bool IsPrimitiveNumberOrChar(IType lhs)
		{
			return base.TypeSystemServices.IsPrimitiveNumber(lhs) || IsChar(lhs);
		}

		private bool IsBool(IType lhs)
		{
			return base.TypeSystemServices.BoolType == lhs;
		}

		private static bool IsNull(Expression node)
		{
			return NodeType.NullLiteralExpression == node.NodeType;
		}

		private void BindInPlaceAddSubtract(BinaryExpression node)
		{
			if (IsEventSubscription(node))
			{
				BindEventSubscription(node);
			}
			else
			{
				BindInPlaceArithmeticOperator(node);
			}
		}

		private static bool IsEventSubscription(BinaryExpression node)
		{
			IEntity entity = node.Left.Entity;
			return entity != null && (IsEvent(entity) || entity.IsAmbiguous());
		}

		private static bool IsEvent(IEntity entity)
		{
			return EntityType.Event == entity.EntityType;
		}

		private void BindEventSubscription(BinaryExpression node)
		{
			IEntity entity = GetEntity(node.Left);
			if (entity.IsAmbiguous())
			{
				IList list = ((Ambiguous)entity).Select(IsPublicEvent);
				if (list.Count != 1)
				{
					Error(node);
					return;
				}
				entity = (IEntity)list[0];
				Bind(node.Left, entity);
			}
			IEvent delegateMember = (IEvent)entity;
			IType expressionType = GetExpressionType(node.Right);
			if (!AssertDelegateArgument(node, delegateMember, expressionType))
			{
				Error(node);
			}
			else
			{
				BindExpressionType(node, base.TypeSystemServices.VoidType);
			}
		}

		protected virtual void ProcessBuiltinInvocation(MethodInvocationExpression node, BuiltinFunction function)
		{
			switch (function.FunctionType)
			{
			case BuiltinFunctionType.Len:
				ProcessLenInvocation(node);
				break;
			case BuiltinFunctionType.AddressOf:
				ProcessAddressOfInvocation(node);
				break;
			case BuiltinFunctionType.Eval:
				ProcessEvalInvocation(node);
				break;
			default:
				NotImplemented(node, "BuiltinFunction: " + function);
				break;
			}
		}

		private bool ProcessSwitchInvocation(MethodInvocationExpression node)
		{
			if (BuiltinFunction.Switch != node.Target.Entity)
			{
				return false;
			}
			BindSwitchLabelReferences(node);
			if (CheckSwitchArguments(node))
			{
				return true;
			}
			Error(node, CompilerErrorFactory.InvalidSwitch(node.Target));
			return true;
		}

		private static void BindSwitchLabelReferences(MethodInvocationExpression node)
		{
			for (int i = 1; i < node.Arguments.Count; i++)
			{
				ReferenceExpression referenceExpression = (ReferenceExpression)node.Arguments[i];
				referenceExpression.ExpressionType = Unknown.Default;
			}
		}

		private bool CheckSwitchArguments(MethodInvocationExpression node)
		{
			ExpressionCollection arguments = node.Arguments;
			if (arguments.Count > 1)
			{
				Visit(arguments[0]);
				if (base.TypeSystemServices.IsIntegerNumber(GetExpressionType(arguments[0])))
				{
					for (int i = 1; i < arguments.Count; i++)
					{
						if (NodeType.ReferenceExpression != arguments[i].NodeType)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		private void ProcessEvalInvocation(MethodInvocationExpression node)
		{
			if (node.Arguments.Count > 0)
			{
				int num = node.Arguments.Count - 1;
				for (int i = 0; i < num; i++)
				{
					AssertHasSideEffect(node.Arguments[i]);
				}
				BindExpressionType(node, GetConcreteExpressionType(node.Arguments.Last));
			}
			else
			{
				BindExpressionType(node, base.TypeSystemServices.VoidType);
			}
		}

		private void ProcessAddressOfInvocation(MethodInvocationExpression node)
		{
			if (node.Arguments.Count != 1)
			{
				Error(node, CompilerErrorFactory.MethodArgumentCount(node.Target, "__addressof__", 1));
				return;
			}
			Expression expression = node.Arguments[0];
			EntityType entityType = GetEntity(expression).EntityType;
			if (EntityType.Method != entityType)
			{
				ReferenceExpression referenceExpression = expression as ReferenceExpression;
				if (referenceExpression != null && entityType == EntityType.Ambiguous)
				{
					Error(node, CompilerErrorFactory.AmbiguousReference(expression, referenceExpression.Name, ((Ambiguous)expression.Entity).Entities));
				}
				else
				{
					Error(node, CompilerErrorFactory.MethodReferenceExpected(expression));
				}
			}
			else
			{
				BindExpressionType(node, base.TypeSystemServices.IntPtrType);
			}
		}

		private void ProcessLenInvocation(MethodInvocationExpression node)
		{
			if (node.Arguments.Count < 1 || node.Arguments.Count > 2)
			{
				Error(node, CompilerErrorFactory.MethodArgumentCount(node.Target, "len", node.Arguments.Count));
				return;
			}
			Expression expression = null;
			Expression expression2 = node.Arguments[0];
			IType expressionType = GetExpressionType(expression2);
			bool flag = IsAssignableFrom(base.TypeSystemServices.ArrayType, expressionType);
			if (!flag && node.Arguments.Count != 1)
			{
				Error(node, CompilerErrorFactory.MethodArgumentCount(node.Target, "len", node.Arguments.Count));
			}
			if (base.TypeSystemServices.IsSystemObject(expressionType))
			{
				expression = base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_Len, expression2);
			}
			else if (base.TypeSystemServices.StringType == expressionType)
			{
				expression = base.CodeBuilder.CreateMethodInvocation(expression2, MethodCache.String_get_Length);
			}
			else if (flag)
			{
				expression = ((node.Arguments.Count != 1) ? base.CodeBuilder.CreateMethodInvocation(expression2, MethodCache.Array_GetLength, node.Arguments[1]) : base.CodeBuilder.CreateMethodInvocation(expression2, MethodCache.Array_get_Length));
			}
			else if (IsAssignableFrom(base.TypeSystemServices.ICollectionType, expressionType))
			{
				expression = base.CodeBuilder.CreateMethodInvocation(expression2, MethodCache.ICollection_get_Count);
			}
			else if (GenericsServices.HasConstructedType(expressionType, base.TypeSystemServices.ICollectionGenericType))
			{
				expression = new MemberReferenceExpression(node.LexicalInfo, expression2, "Count");
				Visit(expression);
			}
			else
			{
				Error(CompilerErrorFactory.InvalidLen(expression2, expressionType));
			}
			if (null != expression)
			{
				node.ParentNode.Replace(node, expression);
			}
		}

		private void CheckItems(IType expectedElementType, ExpressionCollection items)
		{
			foreach (Expression item in items)
			{
				AssertTypeCompatibility(item, expectedElementType, GetExpressionType(item));
			}
		}

		private void ApplyBuiltinMethodTypeInference(MethodInvocationExpression expression, IMethod method)
		{
			IType type = _invocationTypeReferenceRules.Instance.ApplyTo(expression, method);
			if (type != null)
			{
				Node parentNode = expression.ParentNode;
				if (parentNode.NodeType != NodeType.ExpressionStatement)
				{
					parentNode.Replace(expression, base.CodeBuilder.CreateCast(type, expression));
				}
			}
		}

		protected virtual IEntity ResolveAmbiguousMethodInvocation(MethodInvocationExpression node, IEntity entity)
		{
			Ambiguous ambiguous = entity as Ambiguous;
			if (ambiguous == null)
			{
				return entity;
			}
			_context.TraceVerbose("{0}: resolving ambigous method invocation: {1}", node.LexicalInfo, entity);
			IEntity entity2 = ResolveCallableReference(node, ambiguous);
			if (null != entity2)
			{
				return entity2;
			}
			if (!ResolvedAsExtension(node) && TryToProcessAsExtensionInvocation(node))
			{
				return null;
			}
			return CantResolveAmbiguousMethodInvocation(node, ambiguous.Entities);
		}

		private IEntity ResolveCallableReference(MethodInvocationExpression node, Ambiguous entity)
		{
			GenericsServices genericService = My<GenericsServices>.Instance;
			IMethod[] candidates = (from m in entity.Entities.OfType<IMethod>().Select(delegate(IMethod m)
				{
					if (m.GenericInfo == null)
					{
						return m;
					}
					GenericParameterInferrer genericParameterInferrer = new GenericParameterInferrer(base.Context, m, node.Arguments);
					genericParameterInferrer.ResolveClosure += ProcessClosureInMethodInvocation;
					if (!genericParameterInferrer.Run())
					{
						return null;
					}
					IType[] inferredTypes = genericParameterInferrer.GetInferredTypes();
					return (inferredTypes == null || !genericService.CheckGenericConstruction(m, inferredTypes)) ? null : m.GenericInfo.ConstructMethod(inferredTypes);
				})
				where m != null
				select m).ToArray();
			IEntity entity2 = CallableResolutionService.ResolveCallableReference(node.Arguments, candidates);
			if (null == entity2)
			{
				return null;
			}
			IMember member = (IMember)entity2;
			if (NodeType.ReferenceExpression == node.Target.NodeType)
			{
				ResolveMemberInfo((ReferenceExpression)node.Target, member);
			}
			else
			{
				Bind(node.Target, member);
				BindExpressionType(node.Target, member.Type);
			}
			return entity2;
		}

		private bool TryToProcessAsExtensionInvocation(MethodInvocationExpression node)
		{
			IEntity entity = ResolveExtension(node);
			if (null == entity)
			{
				return false;
			}
			node.Annotate(ResolvedAsExtensionAnnotation);
			ProcessMethodInvocationExpression(node, entity);
			return true;
		}

		private IEntity ResolveExtension(MethodInvocationExpression node)
		{
			ReferenceExpression referenceExpression = node.Target as ReferenceExpression;
			if (referenceExpression == null)
			{
				return null;
			}
			MemberReferenceExpression memberReferenceExpression = referenceExpression as MemberReferenceExpression;
			INamespace ns = ((memberReferenceExpression != null) ? GetReferenceNamespace(memberReferenceExpression) : CurrentType);
			return base.NameResolutionService.ResolveExtension(ns, referenceExpression.Name);
		}

		private static bool ResolvedAsExtension(MethodInvocationExpression node)
		{
			if (node.ContainsAnnotation(ResolvedAsExtensionAnnotation) || node.Target.ContainsAnnotation(ResolvedAsExtensionAnnotation))
			{
				return true;
			}
			return (node.Target as GenericReferenceExpression)?.Target.ContainsAnnotation(ResolvedAsExtensionAnnotation) ?? false;
		}

		protected virtual IEntity CantResolveAmbiguousMethodInvocation(MethodInvocationExpression node, IEntity[] entities)
		{
			EmitCallableResolutionError(node, entities, node.Arguments);
			Error(node);
			return null;
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (null != node.ExpressionType)
			{
				_context.TraceVerbose("{0}: Method invocation already bound.", node.LexicalInfo);
				return;
			}
			Visit(node.Target);
			if (ProcessSwitchInvocation(node) || ProcessMetaMethodInvocation(node, resolvedArgs: false))
			{
				return;
			}
			Visit(node.Arguments);
			if (TypeSystemServices.IsError(node.Target) || TypeSystemServices.IsErrorAny(node.Arguments))
			{
				Error(node);
				return;
			}
			IEntity entity = node.Target.Entity;
			if (null == entity)
			{
				ProcessGenericMethodInvocation(node);
			}
			else
			{
				ProcessMethodInvocationExpression(node, entity);
			}
		}

		private bool ProcessMetaMethodInvocation(MethodInvocationExpression node, bool resolvedArgs)
		{
			IEntity entity = node.Target.Entity;
			if (entity == null)
			{
				return false;
			}
			if (!IsOrContainMetaMethod(entity))
			{
				return false;
			}
			object[] metaMethodInvocationArguments = GetMetaMethodInvocationArguments(node);
			Type[] argumentTypes = MethodResolver.GetArgumentTypes(metaMethodInvocationArguments);
			MethodResolver methodResolver = new MethodResolver(argumentTypes);
			CandidateMethod candidateMethod = methodResolver.ResolveMethod(EnumerateMetaMethods(entity));
			if (candidateMethod == null)
			{
				return false;
			}
			if (ShouldResolveArgsOf(candidateMethod))
			{
				Visit(node.Arguments);
				InvokeMetaMethod(node, candidateMethod, GetMetaMethodInvocationArguments(node));
				return true;
			}
			InvokeMetaMethod(node, candidateMethod, metaMethodInvocationArguments);
			return true;
		}

		private static bool ShouldResolveArgsOf(CandidateMethod method)
		{
			return MetaAttributeOf(method).ResolveArgs;
		}

		private static MetaAttribute MetaAttributeOf(CandidateMethod method)
		{
			return (MetaAttribute)method.Method.GetCustomAttributes(typeof(MetaAttribute), inherit: false).Single();
		}

		private void InvokeMetaMethod(MethodInvocationExpression node, CandidateMethod method, object[] arguments)
		{
			Node replacement = (Node)method.DynamicInvoke(null, arguments);
			ReplaceMetaMethodInvocationSite(node, replacement);
		}

		private static object[] GetMetaMethodInvocationArguments(MethodInvocationExpression node)
		{
			if (node.NamedArguments.Count == 0)
			{
				return node.Arguments.ToArray();
			}
			List list = new List();
			list.Add(node.NamedArguments.ToArray());
			list.Extend(node.Arguments);
			return list.ToArray();
		}

		private void ReplaceMetaMethodInvocationSite(MethodInvocationExpression node, Node replacement)
		{
			if (replacement == null || replacement is Statement)
			{
				if (node.ParentNode.NodeType != NodeType.ExpressionStatement)
				{
					NotImplemented(node, "Cant use an statement where an expression is expected.");
				}
				Node parentNode = node.ParentNode.ParentNode;
				parentNode.Replace(node.ParentNode, replacement);
				if (replacement != null)
				{
					replacement = My<CodeReifier>.Instance.Reify((Statement)replacement);
				}
			}
			else
			{
				node.ParentNode.Replace(node, replacement);
				replacement = My<CodeReifier>.Instance.Reify((Expression)replacement);
			}
			Visit(replacement);
		}

		private IEnumerable<MethodInfo> EnumerateMetaMethods(IEntity entity)
		{
			if (entity.EntityType == EntityType.Method)
			{
				yield return GetMethodInfo(entity);
				yield break;
			}
			try
			{
				IEntity[] entities = ((Ambiguous)entity).Entities;
				foreach (IEntity item in entities)
				{
					yield return GetMethodInfo(item);
				}
			}
			finally
			{
			}
		}

		private static MethodInfo GetMethodInfo(IEntity entity)
		{
			return (MethodInfo)((ExternalMethod)entity).MethodInfo;
		}

		private bool IsOrContainMetaMethod(IEntity entity)
		{
			return entity.EntityType switch
			{
				EntityType.Ambiguous => ((Ambiguous)entity).Any(IsMetaMethod), 
				EntityType.Method => IsMetaMethod(entity), 
				_ => false, 
			};
		}

		private static bool IsMetaMethod(IEntity entity)
		{
			return (entity as ExternalMethod)?.IsMeta ?? false;
		}

		private void ProcessMethodInvocationExpression(MethodInvocationExpression node, IEntity targetEntity)
		{
			if (ResolvedAsExtension(node) || IsExtensionMethod(targetEntity))
			{
				PreNormalizeExtensionInvocation(node, targetEntity as IEntityWithParameters);
			}
			targetEntity = ResolveAmbiguousMethodInvocation(node, targetEntity);
			if (targetEntity != null)
			{
				switch (targetEntity.EntityType)
				{
				case EntityType.BuiltinFunction:
					ProcessBuiltinInvocation(node, (BuiltinFunction)targetEntity);
					break;
				case EntityType.Event:
					ProcessEventInvocation(node, (IEvent)targetEntity);
					break;
				case EntityType.Method:
					ProcessMethodInvocation(node, (IMethod)targetEntity);
					break;
				case EntityType.Constructor:
					ProcessConstructorInvocation(node, targetEntity);
					break;
				case EntityType.Type:
					ProcessTypeInvocation(node);
					break;
				case EntityType.Error:
					Error(node);
					break;
				default:
					ProcessGenericMethodInvocation(node);
					break;
				}
			}
		}

		private void ProcessConstructorInvocation(MethodInvocationExpression node, IEntity targetEntity)
		{
			NamedArgumentsNotAllowed(node);
			InternalConstructor internalConstructor = targetEntity as InternalConstructor;
			if (null != internalConstructor)
			{
				IType type = null;
				if (NodeType.SuperLiteralExpression == node.Target.NodeType)
				{
					internalConstructor.HasSuperCall = true;
					type = internalConstructor.DeclaringType.BaseType;
				}
				else if (node.Target.NodeType == NodeType.SelfLiteralExpression)
				{
					internalConstructor.HasSelfCall = true;
					type = internalConstructor.DeclaringType;
				}
				IConstructor correctConstructor = GetCorrectConstructor(node, type, node.Arguments);
				if (null != correctConstructor)
				{
					Bind(node.Target, correctConstructor);
				}
			}
		}

		protected virtual bool ProcessMethodInvocationWithInvalidParameters(MethodInvocationExpression node, IMethod targetMethod)
		{
			return false;
		}

		protected virtual void ProcessMethodInvocation(MethodInvocationExpression node, IMethod method)
		{
			if (ResolvedAsExtension(node))
			{
				PostNormalizeExtensionInvocation(node, method);
			}
			IMethod method2 = InferGenericMethodInvocation(node, method);
			if (method2 == null)
			{
				return;
			}
			if (!CheckParameters(method2.CallableType, node.Arguments, reportErrors: false))
			{
				if ((!ResolvedAsExtension(node) && TryToProcessAsExtensionInvocation(node)) || ProcessMethodInvocationWithInvalidParameters(node, method2))
				{
					return;
				}
				AssertParameters(node, method2, node.Arguments);
			}
			AssertTargetContext(node.Target, method2);
			NamedArgumentsNotAllowed(node);
			EnsureRelatedNodeWasVisited(node.Target, method2);
			BindExpressionType(node, GetInferredType(method2));
			ApplyBuiltinMethodTypeInference(node, method2);
		}

		private IMethod InferGenericMethodInvocation(MethodInvocationExpression node, IMethod targetMethod)
		{
			if (targetMethod.GenericInfo == null)
			{
				return targetMethod;
			}
			GenericParameterInferrer genericParameterInferrer = new GenericParameterInferrer(base.Context, targetMethod, node.Arguments);
			genericParameterInferrer.ResolveClosure += ProcessClosureInMethodInvocation;
			if (!genericParameterInferrer.Run())
			{
				CannotInferGenericMethodArguments(node, targetMethod);
				return null;
			}
			IType[] inferredTypes = genericParameterInferrer.GetInferredTypes();
			if (!_genericServices.Instance.CheckGenericConstruction(node, targetMethod, inferredTypes, reportErrors: true))
			{
				Error(node);
				return null;
			}
			IMethod method = targetMethod.GenericInfo.ConstructMethod(inferredTypes);
			Bind(node.Target, method);
			BindExpressionType(node, GetInferredType(method));
			return method;
		}

		private void CannotInferGenericMethodArguments(Expression node, IMethod genericMethod)
		{
			Error(node, CompilerErrorFactory.CannotInferGenericMethodArguments(node, genericMethod));
		}

		private bool IsAccessible(IEntity member)
		{
			IAccessibleMember accessibleMember = member as IAccessibleMember;
			if (accessibleMember == null)
			{
				return true;
			}
			return IsAccessible(accessibleMember);
		}

		private bool IsAccessible(IAccessibleMember accessible)
		{
			return GetAccessibilityChecker().IsAccessible(accessible);
		}

		private IAccessibilityChecker GetAccessibilityChecker()
		{
			if (null == _currentMethod)
			{
				return AccessibilityChecker.Global;
			}
			return new AccessibilityChecker(CurrentTypeDefinition);
		}

		private void NamedArgumentsNotAllowed(MethodInvocationExpression node)
		{
			if (node.NamedArguments.Count != 0)
			{
				Error(CompilerErrorFactory.NamedArgumentsNotAllowed(node.NamedArguments[0]));
			}
		}

		private bool IsExtensionMethod(IEntity entity)
		{
			return (entity as IExtensionEnabled)?.IsExtension ?? false;
		}

		private void PostNormalizeExtensionInvocation(MethodInvocationExpression node, IMethod targetMethod)
		{
			node.Target = base.CodeBuilder.CreateMethodReference(node.Target.LexicalInfo, targetMethod);
		}

		private void PreNormalizeExtensionInvocation(MethodInvocationExpression node, IEntityWithParameters extension)
		{
			if (node.Arguments.Count == 0 || extension == null || node.Arguments.Count < extension.GetParameters().Length)
			{
				node.Arguments.Insert(0, EnsureMemberReferenceForExtension(node).Target);
			}
		}

		private MemberReferenceExpression EnsureMemberReferenceForExtension(MethodInvocationExpression node)
		{
			Expression target = node.Target;
			GenericReferenceExpression genericReferenceExpression = target as GenericReferenceExpression;
			if (null != genericReferenceExpression)
			{
				target = genericReferenceExpression.Target;
			}
			MemberReferenceExpression memberReferenceExpression = target as MemberReferenceExpression;
			if (null != memberReferenceExpression)
			{
				return memberReferenceExpression;
			}
			return (MemberReferenceExpression)(node.Target = base.CodeBuilder.MemberReferenceForEntity(CreateSelfReference(), GetEntity(node.Target)));
		}

		private SelfLiteralExpression CreateSelfReference()
		{
			return base.CodeBuilder.CreateSelfReference(CurrentType);
		}

		protected virtual bool IsDuckTyped(IMember entity)
		{
			return entity.IsDuckTyped;
		}

		private IType GetInferredType(IMethod entity)
		{
			return IsDuckTyped(entity) ? base.TypeSystemServices.DuckType : entity.ReturnType;
		}

		private IType GetInferredType(IMember entity)
		{
			Debug.Assert(EntityType.Method != entity.EntityType);
			return IsDuckTyped(entity) ? base.TypeSystemServices.DuckType : entity.Type;
		}

		private void ReplaceTypeInvocationByEval(IType type, MethodInvocationExpression node)
		{
			node.ParentNode.Replace(node, EvalForTypeInvocation(type, node));
		}

		private MethodInvocationExpression EvalForTypeInvocation(IType type, MethodInvocationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo);
			ReferenceExpression referenceExpression = CreateTempLocal(node.Target.LexicalInfo, type);
			methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateAssignment(referenceExpression.CloneNode(), node));
			AddResolvedNamedArgumentsToEval(methodInvocationExpression, node.NamedArguments, referenceExpression);
			node.NamedArguments.Clear();
			methodInvocationExpression.Arguments.Add(referenceExpression);
			BindExpressionType(methodInvocationExpression, type);
			return methodInvocationExpression;
		}

		private void AddResolvedNamedArgumentsToEval(MethodInvocationExpression eval, ExpressionPairCollection namedArguments, ReferenceExpression instance)
		{
			foreach (ExpressionPair namedArgument in namedArguments)
			{
				if (!TypeSystemServices.IsError(namedArgument.First))
				{
					AddResolvedNamedArgumentToEval(eval, namedArgument, instance);
				}
			}
		}

		protected virtual void AddResolvedNamedArgumentToEval(MethodInvocationExpression eval, ExpressionPair pair, ReferenceExpression instance)
		{
			IEntity entity = GetEntity(pair.First);
			switch (entity.EntityType)
			{
			case EntityType.Event:
			{
				IEvent @event = (IEvent)entity;
				eval.Arguments.Add(base.CodeBuilder.CreateMethodInvocation(pair.First.LexicalInfo, instance.CloneNode(), @event.GetAddMethod(), pair.Second));
				break;
			}
			case EntityType.Field:
				eval.Arguments.Add(base.CodeBuilder.CreateAssignment(pair.First.LexicalInfo, base.CodeBuilder.CreateMemberReference(instance.CloneNode(), (IMember)entity), pair.Second));
				break;
			case EntityType.Property:
			{
				IProperty property = (IProperty)entity;
				IMethod setMethod = property.GetSetMethod();
				if (null == setMethod)
				{
					Error(CompilerErrorFactory.PropertyIsReadOnly(pair.First, property));
				}
				else
				{
					eval.Arguments.Add(base.CodeBuilder.CreateAssignment(pair.First.LexicalInfo, base.CodeBuilder.CreateMemberReference(instance.CloneNode(), property), pair.Second));
				}
				break;
			}
			}
		}

		private void ProcessEventInvocation(MethodInvocationExpression node, IEvent ev)
		{
			NamedArgumentsNotAllowed(node);
			if (EnsureInternalEventInvocation(ev, node))
			{
				IMethod raiseMethod = ev.GetRaiseMethod();
				if (AssertParameters(node, raiseMethod, node.Arguments))
				{
					node.Target = base.CodeBuilder.CreateMemberReference(((MemberReferenceExpression)node.Target).Target, raiseMethod);
					BindExpressionType(node, raiseMethod.ReturnType);
				}
			}
		}

		public bool EnsureInternalEventInvocation(IEvent ev, Expression node)
		{
			if (ev.IsAbstract || ev.IsVirtual || ev.DeclaringType == CurrentType)
			{
				return true;
			}
			Error(CompilerErrorFactory.EventCanOnlyBeInvokedFromWithinDeclaringClass(node, ev));
			return false;
		}

		private void ProcessCallableTypeInvocation(MethodInvocationExpression node, ICallableType type)
		{
			NamedArgumentsNotAllowed(node);
			if (node.Arguments.Count == 1)
			{
				AssertTypeCompatibility(node.Arguments[0], type, GetExpressionType(node.Arguments[0]));
				node.ParentNode.Replace(node, base.CodeBuilder.CreateCast(type, node.Arguments[0]));
				return;
			}
			IConstructor correctConstructor = GetCorrectConstructor(node, type, node.Arguments);
			if (null != correctConstructor)
			{
				BindConstructorInvocation(node, correctConstructor);
			}
			else
			{
				Error(node);
			}
		}

		private void ProcessTypeInvocation(MethodInvocationExpression node)
		{
			IType type = (IType)node.Target.Entity;
			ICallableType callableType = type as ICallableType;
			if (callableType != null)
			{
				ProcessCallableTypeInvocation(node, callableType);
				return;
			}
			if (!AssertCanCreateInstance(node.Target, type))
			{
				Error(node);
				return;
			}
			ResolveNamedArguments(type, node.NamedArguments);
			if (type.IsValueType && node.Arguments.Count == 0)
			{
				ProcessValueTypeInstantiation(type, node);
				return;
			}
			IConstructor correctConstructor = GetCorrectConstructor(node, type, node.Arguments);
			if (correctConstructor != null)
			{
				BindConstructorInvocation(node, correctConstructor);
				if (node.NamedArguments.Count > 0)
				{
					ReplaceTypeInvocationByEval(type, node);
				}
			}
			else
			{
				Error(node);
			}
		}

		private void BindConstructorInvocation(MethodInvocationExpression node, IConstructor ctor)
		{
			Bind(node.Target, ctor);
			BindExpressionType(node.Target, ctor.Type);
			BindExpressionType(node, ctor.DeclaringType);
		}

		private void ProcessValueTypeInstantiation(IType type, MethodInvocationExpression node)
		{
			ReferenceExpression referenceExpression = base.CodeBuilder.CreateReference(DeclareTempLocal(type));
			Expression item = base.CodeBuilder.CreateDefaultInitializer(node.LexicalInfo, referenceExpression, type);
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo);
			BindExpressionType(methodInvocationExpression, type);
			methodInvocationExpression.Arguments.Add(item);
			AddResolvedNamedArgumentsToEval(methodInvocationExpression, node.NamedArguments, referenceExpression.CloneNode());
			methodInvocationExpression.Arguments.Add(referenceExpression.CloneNode());
			node.ParentNode.Replace(node, methodInvocationExpression);
		}

		private void ProcessGenericMethodInvocation(MethodInvocationExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			if (base.TypeSystemServices.IsCallable(expressionType))
			{
				ProcessMethodInvocationOnCallableExpression(node);
			}
			else
			{
				Error(node, CompilerErrorFactory.TypeIsNotCallable(node.Target, expressionType));
			}
		}

		private void ProcessMethodInvocationOnCallableExpression(MethodInvocationExpression node)
		{
			IType concreteExpressionType = GetConcreteExpressionType(node.Target);
			ICallableType callableType = concreteExpressionType as ICallableType;
			if (callableType != null)
			{
				ProcessDelegateInvocation(node, callableType);
			}
			else if (IsAssignableFrom(base.TypeSystemServices.ICallableType, concreteExpressionType))
			{
				ProcessICallableInvocation(node);
			}
			else if (base.TypeSystemServices.TypeType == concreteExpressionType)
			{
				ProcessSystemTypeInvocation(node);
			}
			else
			{
				ProcessInvocationOnUnknownCallableExpression(node);
			}
		}

		private void ProcessDelegateInvocation(MethodInvocationExpression node, ICallableType delegateType)
		{
			if (!AssertParameters(node.Target, delegateType, delegateType, node.Arguments))
			{
				Error(node);
				return;
			}
			IMethod method = ResolveMethod(delegateType, "Invoke");
			node.Target = base.CodeBuilder.CreateMemberReference(node.Target, method);
			BindExpressionType(node, method.ReturnType);
		}

		private void ProcessICallableInvocation(MethodInvocationExpression node)
		{
			node.Target = base.CodeBuilder.CreateMemberReference(node.Target, MethodCache.ICallable_Call);
			ArrayLiteralExpression item = base.CodeBuilder.CreateObjectArray(node.Arguments);
			node.Arguments.Clear();
			node.Arguments.Add(item);
			BindExpressionType(node, MethodCache.ICallable_Call.ReturnType);
		}

		private void ProcessSystemTypeInvocation(MethodInvocationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = CreateInstanceInvocationFor(node);
			if (methodInvocationExpression.NamedArguments.Count == 0)
			{
				node.ParentNode.Replace(node, methodInvocationExpression);
				return;
			}
			ProcessNamedArgumentsForTypeInvocation(methodInvocationExpression);
			node.ParentNode.Replace(node, EvalForTypeInvocation(base.TypeSystemServices.ObjectType, methodInvocationExpression));
		}

		private void ProcessNamedArgumentsForTypeInvocation(MethodInvocationExpression invocation)
		{
			foreach (ExpressionPair namedArgument in invocation.NamedArguments)
			{
				if (ProcessNamedArgument(namedArgument))
				{
					NamedArgumentNotFound(base.TypeSystemServices.ObjectType, (ReferenceExpression)namedArgument.First);
				}
			}
		}

		private MethodInvocationExpression CreateInstanceInvocationFor(MethodInvocationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(MethodCache.Activator_CreateInstance, node.Target);
			if (MethodCache.Activator_CreateInstance.AcceptVarArgs)
			{
				methodInvocationExpression.Arguments.AddRange(node.Arguments);
			}
			else
			{
				methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateObjectArray(node.Arguments));
			}
			methodInvocationExpression.NamedArguments = node.NamedArguments;
			return methodInvocationExpression;
		}

		protected virtual void ProcessInvocationOnUnknownCallableExpression(MethodInvocationExpression node)
		{
			NotImplemented(node, string.Concat("Method invocation on type '", node.Target.ExpressionType, "'."));
		}

		private bool AssertIdentifierName(Node node, string name)
		{
			if (base.TypeSystemServices.IsPrimitive(name))
			{
				Error(CompilerErrorFactory.CantRedefinePrimitive(node, name));
				return false;
			}
			return true;
		}

		private bool CheckIsNotValueType(BinaryExpression node, Expression expression)
		{
			IType expressionType = GetExpressionType(expression);
			if (!TypeSystemServices.IsReferenceType(expressionType) && !TypeSystemServices.IsAnyType(expressionType))
			{
				Error(CompilerErrorFactory.OperatorCantBeUsedWithValueType(expression, GetBinaryOperatorText(node.Operator), expressionType));
				return false;
			}
			return true;
		}

		private void BindAssignmentToSlice(BinaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Left;
			Expression target = slicingExpression.Target;
			if (!target.Entity.IsAmbiguous() && IsArray(target))
			{
				BindAssignmentToSliceArray(node);
			}
			else if (base.TypeSystemServices.IsDuckTyped(target))
			{
				BindExpressionType(node, base.TypeSystemServices.DuckType);
			}
			else
			{
				BindAssignmentToSliceProperty(node);
			}
		}

		private bool IsArray(Expression expression)
		{
			return GetExpressionType(expression).IsArray;
		}

		private void BindAssignmentToSliceArray(BinaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Left;
			if (slicingExpression.IsComplexSlicing())
			{
				BindAssignmentToComplexSliceArray(node);
				return;
			}
			IType elementType = GetExpressionType(slicingExpression.Target).ElementType;
			IType expressionType = GetExpressionType(node.Right);
			if (!AssertTypeCompatibility(node.Right, elementType, expressionType))
			{
				Error(node);
			}
			else
			{
				node.ExpressionType = elementType;
			}
		}

		private void BindAssignmentToComplexSliceArray(BinaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Left;
			ArrayLiteralExpression arrayLiteralExpression = new ArrayLiteralExpression();
			ArrayLiteralExpression arrayLiteralExpression2 = new ArrayLiteralExpression();
			ArrayLiteralExpression arrayLiteralExpression3 = new ArrayLiteralExpression();
			for (int i = 0; i < slicingExpression.Indices.Count; i++)
			{
				arrayLiteralExpression.Items.Add(slicingExpression.Indices[i].Begin);
				if (slicingExpression.Indices[i].End == null)
				{
					arrayLiteralExpression.Items.Add(new IntegerLiteralExpression(1 + (int)((IntegerLiteralExpression)slicingExpression.Indices[i].Begin).Value));
					arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: true));
					arrayLiteralExpression3.Items.Add(new BoolLiteralExpression(value: false));
				}
				else if (slicingExpression.Indices[i].End == OmittedExpression.Default)
				{
					arrayLiteralExpression.Items.Add(new IntegerLiteralExpression(0L));
					arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: false));
					arrayLiteralExpression3.Items.Add(new BoolLiteralExpression(value: true));
				}
				else
				{
					arrayLiteralExpression.Items.Add(slicingExpression.Indices[i].End);
					arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: false));
					arrayLiteralExpression3.Items.Add(new BoolLiteralExpression(value: false));
				}
			}
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_SetMultiDimensionalRange1, node.Right, slicingExpression.Target, arrayLiteralExpression);
			methodInvocationExpression.Arguments.Add(arrayLiteralExpression3);
			methodInvocationExpression.Arguments.Add(arrayLiteralExpression2);
			BindExpressionType(methodInvocationExpression, base.TypeSystemServices.VoidType);
			BindExpressionType(arrayLiteralExpression, base.TypeSystemServices.Map(typeof(int[])));
			BindExpressionType(arrayLiteralExpression3, base.TypeSystemServices.Map(typeof(bool[])));
			BindExpressionType(arrayLiteralExpression2, base.TypeSystemServices.Map(typeof(bool[])));
			node.ParentNode.Replace(node, methodInvocationExpression);
		}

		private void BindAssignmentToSliceProperty(BinaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Left;
			IEntity entity = GetEntity(node.Left);
			if (IsError(entity))
			{
				return;
			}
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.Left.LexicalInfo);
			foreach (Slice index in slicingExpression.Indices)
			{
				methodInvocationExpression.Arguments.Add(index.Begin);
			}
			methodInvocationExpression.Arguments.Add(node.Right);
			IMethod method = null;
			IProperty property = entity as IProperty;
			if (property != null)
			{
				IMethod setMethod = property.GetSetMethod();
				if (setMethod == null)
				{
					Error(node, CompilerErrorFactory.PropertyIsReadOnly(slicingExpression.Target, property));
					return;
				}
				if (AssertParameters(node.Left, setMethod, methodInvocationExpression.Arguments))
				{
					method = setMethod;
				}
			}
			else if (entity.IsAmbiguous())
			{
				method = (IMethod)GetCorrectCallableReference(node.Left, methodInvocationExpression.Arguments, GetSetMethods(entity));
				if (method == null)
				{
					Error(node.Left);
					return;
				}
			}
			if (null == method)
			{
				Error(node, CompilerErrorFactory.LValueExpected(node.Left));
				return;
			}
			methodInvocationExpression.Target = base.CodeBuilder.CreateMemberReference(GetIndexedPropertySlicingTarget(slicingExpression), method);
			BindExpressionType(methodInvocationExpression, method.ReturnType);
			node.ParentNode.Replace(node, methodInvocationExpression);
		}

		private IEntity[] GetSetMethods(IEntity candidates)
		{
			return GetSetMethods(((Ambiguous)candidates).Entities);
		}

		private void BindAssignment(BinaryExpression node)
		{
			BindNullableOperation(node);
			if (NodeType.SlicingExpression == node.Left.NodeType)
			{
				BindAssignmentToSlice(node);
			}
			else
			{
				ProcessAssignment(node);
			}
		}

		protected virtual void ProcessAssignment(BinaryExpression node)
		{
			TryToResolveAmbiguousAssignment(node);
			if (ValidateAssignment(node))
			{
				BindExpressionType(node, GetExpressionType(node.Right));
			}
			else
			{
				Error(node);
			}
		}

		protected virtual bool ValidateAssignment(BinaryExpression node)
		{
			if (!AssertLValue(node.Left))
			{
				return false;
			}
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			if (!AssertTypeCompatibility(node.Right, expressionType, expressionType2))
			{
				return false;
			}
			CheckAssignmentToIndexedProperty(node.Left, node.Left.Entity);
			return true;
		}

		protected virtual void TryToResolveAmbiguousAssignment(BinaryExpression node)
		{
			if (!node.Left.Entity.IsAmbiguous())
			{
				return;
			}
			Expression left = node.Left;
			IEntity entity = ResolveAmbiguousLValue(left, (Ambiguous)node.Left.Entity, node.Right);
			if (NodeType.ReferenceExpression == left.NodeType)
			{
				IMember member = entity as IMember;
				if (null != member)
				{
					ResolveMemberInfo((ReferenceExpression)left, member);
				}
			}
		}

		private void CheckAssignmentToIndexedProperty(Node node, IEntity lhs)
		{
			IProperty property = lhs as IProperty;
			if (property != null && property.IsIndexedProperty())
			{
				Error(CompilerErrorFactory.PropertyRequiresParameters(MemberAnchorFor(node), property));
			}
		}

		private bool CheckIsaArgument(Expression e)
		{
			if (base.TypeSystemServices.TypeType != GetExpressionType(e))
			{
				Error(CompilerErrorFactory.IsaArgument(e));
				return false;
			}
			return true;
		}

		private bool BindNullableOperation(BinaryExpression node)
		{
			if (!IsNullableOperation(node))
			{
				return false;
			}
			if (BinaryOperatorType.ReferenceEquality == node.Operator)
			{
				node.Operator = BinaryOperatorType.Equality;
				return BindNullableComparison(node);
			}
			if (BinaryOperatorType.ReferenceInequality == node.Operator)
			{
				node.Operator = BinaryOperatorType.Inequality;
				return BindNullableComparison(node);
			}
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			bool flag = TypeSystemServices.IsNullable(expressionType);
			bool flag2 = TypeSystemServices.IsNullable(expressionType2);
			if (BinaryOperatorType.Assign == node.Operator && flag)
			{
				if (flag2)
				{
					return false;
				}
				BindNullableInitializer(node, node.Right, expressionType);
				return false;
			}
			if (flag)
			{
				MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(node.Left, "Value");
				node.Replace(node.Left, memberReferenceExpression);
				Visit(memberReferenceExpression);
				memberReferenceExpression.Annotate("nullableTarget", true);
			}
			if (flag2)
			{
				MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(node.Right, "Value");
				node.Replace(node.Right, memberReferenceExpression);
				Visit(memberReferenceExpression);
				memberReferenceExpression.Annotate("nullableTarget", true);
			}
			return false;
		}

		private bool BindNullableComparison(BinaryExpression node)
		{
			if (!IsNullableOperation(node))
			{
				return false;
			}
			if (IsNull(node.Left) || IsNull(node.Right))
			{
				Expression target = (IsNull(node.Left) ? node.Right : node.Left);
				Expression expression = new MemberReferenceExpression(target, "HasValue");
				node.Replace(node.Left, expression);
				Visit(expression);
				Expression expression2 = new BoolLiteralExpression(value: false);
				node.Replace(node.Right, expression2);
				Visit(expression2);
				BindExpressionType(node, base.TypeSystemServices.BoolType);
				return true;
			}
			BinaryExpression binaryExpression = new BinaryExpression((node.Operator == BinaryOperatorType.Inequality) ? BinaryOperatorType.BitwiseOr : BinaryOperatorType.BitwiseAnd, new BinaryExpression(GetCorrespondingHasValueOperator(node.Operator), CreateNullableHasValueOrTrueExpression(node.Left), CreateNullableHasValueOrTrueExpression(node.Right)), new BinaryExpression(node.Operator, CreateNullableGetValueOrDefaultExpression(node.Left), CreateNullableGetValueOrDefaultExpression(node.Right)));
			node.ParentNode.Replace(node, binaryExpression);
			Visit(binaryExpression);
			return true;
		}

		private BinaryOperatorType GetCorrespondingHasValueOperator(BinaryOperatorType op)
		{
			if (BinaryOperatorType.Equality == op || BinaryOperatorType.Inequality == op)
			{
				return op;
			}
			return BinaryOperatorType.BitwiseAnd;
		}

		private IEnumerable<Expression> FindNullableExpressions(Expression exp)
		{
			if (exp.ContainsAnnotation("nullableTarget"))
			{
				yield return ((MemberReferenceExpression)exp).Target;
				yield break;
			}
			BinaryExpression bex = exp as BinaryExpression;
			if (null == bex)
			{
				yield break;
			}
			foreach (Expression item in FindNullableExpressions(bex.Left))
			{
				yield return item;
			}
			foreach (Expression item2 in FindNullableExpressions(bex.Right))
			{
				yield return item2;
			}
		}

		private Expression BuildNullableCoalescingConditional(Expression exp)
		{
			if (IsNull(exp))
			{
				return null;
			}
			IEnumerator<Expression> enumerator = FindNullableExpressions(exp).GetEnumerator();
			Expression result = null;
			BinaryExpression binaryExpression = null;
			Expression expression = null;
			while (enumerator.MoveNext())
			{
				Expression current = enumerator.Current;
				expression = (enumerator.MoveNext() ? enumerator.Current : null);
				if (null != binaryExpression)
				{
					binaryExpression.Right = new BinaryExpression(BinaryOperatorType.BitwiseAnd, binaryExpression.Right, new BinaryExpression(BinaryOperatorType.BitwiseAnd, CreateNullableHasValueOrTrueExpression(current), CreateNullableHasValueOrTrueExpression(expression)));
					continue;
				}
				if (null == expression)
				{
					return CreateNullableHasValueOrTrueExpression(current);
				}
				result = (binaryExpression = new BinaryExpression(BinaryOperatorType.BitwiseAnd, CreateNullableHasValueOrTrueExpression(current), CreateNullableHasValueOrTrueExpression(expression)));
			}
			return result;
		}

		private void BindNullableInitializer(Node node, Expression rhs, IType type)
		{
			Expression expression = CreateNullableInstantiation(rhs, type);
			node.Replace(rhs, expression);
			Visit(expression);
			Expression expression2 = BuildNullableCoalescingConditional(rhs);
			if (null != expression2)
			{
				ConditionalExpression conditionalExpression = new ConditionalExpression();
				conditionalExpression.Condition = expression2;
				conditionalExpression.TrueValue = expression;
				conditionalExpression.FalseValue = CreateNullableInstantiation(type);
				ConditionalExpression conditionalExpression2 = conditionalExpression;
				node.Replace(expression, conditionalExpression2);
				Visit(conditionalExpression2);
			}
		}

		private void BindNullableParameters(ExpressionCollection args, ICallableType target)
		{
			if (null == target)
			{
				return;
			}
			IParameter[] parameters = target.GetSignature().Parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (TypeSystemServices.IsNullable(parameters[i].Type) && !TypeSystemServices.IsNullable(GetExpressionType(args[i])))
				{
					args.Replace(args[i], CreateNullableInstantiation(args[i], parameters[i].Type));
					Visit(args[i]);
				}
			}
		}

		private Expression CreateNullableInstantiation(IType type)
		{
			return CreateNullableInstantiation(null, type);
		}

		private Expression CreateNullableInstantiation(Expression val, IType type)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression();
			genericReferenceExpression.Target = new MemberReferenceExpression(new ReferenceExpression("System"), "Nullable");
			genericReferenceExpression.GenericArguments.Add(TypeReference.Lift(Nullable.GetUnderlyingType(((ExternalType)type).ActualType)));
			methodInvocationExpression.Target = genericReferenceExpression;
			if (val != null && !IsNull(val))
			{
				methodInvocationExpression.Arguments.Add(val);
			}
			return methodInvocationExpression;
		}

		private Expression CreateNullableHasValueOrTrueExpression(Expression target)
		{
			if (target == null || !TypeSystemServices.IsNullable(GetExpressionType(target)))
			{
				return new BoolLiteralExpression(value: true);
			}
			return new MemberReferenceExpression(target, "HasValue");
		}

		private Expression CreateNullableGetValueOrDefaultExpression(Expression target)
		{
			if (target == null || !TypeSystemServices.IsNullable(GetExpressionType(target)))
			{
				return target;
			}
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression.Target = new MemberReferenceExpression(target, "GetValueOrDefault");
			return methodInvocationExpression;
		}

		private void BindTypeTest(BinaryExpression node)
		{
			if (CheckIsNotValueType(node, node.Left) && CheckIsaArgument(node.Right))
			{
				BindExpressionType(node, base.TypeSystemServices.BoolType);
			}
			else
			{
				Error(node);
			}
		}

		private void BindReferenceEquality(BinaryExpression node)
		{
			BindReferenceEquality(node, inequality: false);
		}

		private void BindReferenceEquality(BinaryExpression node, bool inequality)
		{
			if (!BindNullableOperation(node))
			{
				BoolLiteralExpression boolLiteralExpression = node.Right as BoolLiteralExpression;
				if (null != boolLiteralExpression && GetExpressionType(node.Left) == base.TypeSystemServices.BoolType)
				{
					Node node2 = ((boolLiteralExpression.Value ^ inequality) ? node.Left : new UnaryExpression(UnaryOperatorType.LogicalNot, node.Left));
					node.ParentNode.Replace(node, node2);
					Visit(node2);
				}
				else if (CheckIsNotValueType(node, node.Left) && CheckIsNotValueType(node, node.Right))
				{
					BindExpressionType(node, base.TypeSystemServices.BoolType);
				}
				else
				{
					Error(node);
				}
			}
		}

		private void BindInPlaceArithmeticOperator(BinaryExpression node)
		{
			if (IsArraySlicing(node.Left))
			{
				BindInPlaceArithmeticOperatorOnArraySlicing(node);
				return;
			}
			Node parentNode = node.ParentNode;
			Expression left = node.Left;
			if (left.Entity != null && EntityType.Property == left.Entity.EntityType)
			{
				left.ExpressionType = null;
			}
			BinaryExpression binaryExpression = ExpandInPlaceBinaryExpression(node);
			parentNode.Replace(node, binaryExpression);
			Visit(binaryExpression);
		}

		protected BinaryExpression ExpandInPlaceBinaryExpression(BinaryExpression node)
		{
			BinaryExpression binaryExpression = new BinaryExpression(node.LexicalInfo);
			binaryExpression.Operator = BinaryOperatorType.Assign;
			binaryExpression.Left = node.Left.CloneNode();
			binaryExpression.Right = node;
			node.Operator = GetRelatedBinaryOperatorForInPlaceOperator(node.Operator);
			return binaryExpression;
		}

		private void BindInPlaceArithmeticOperatorOnArraySlicing(BinaryExpression node)
		{
			Node parentNode = node.ParentNode;
			Expression newNode = CreateSideEffectAwareSlicingOperation(node.LexicalInfo, GetRelatedBinaryOperatorForInPlaceOperator(node.Operator), (SlicingExpression)node.Left, node.Right, null);
			parentNode.Replace(node, newNode);
		}

		private BinaryOperatorType GetRelatedBinaryOperatorForInPlaceOperator(BinaryOperatorType op)
		{
			return op switch
			{
				BinaryOperatorType.InPlaceAddition => BinaryOperatorType.Addition, 
				BinaryOperatorType.InPlaceSubtraction => BinaryOperatorType.Subtraction, 
				BinaryOperatorType.InPlaceMultiply => BinaryOperatorType.Multiply, 
				BinaryOperatorType.InPlaceDivision => BinaryOperatorType.Division, 
				BinaryOperatorType.InPlaceModulus => BinaryOperatorType.Modulus, 
				BinaryOperatorType.InPlaceBitwiseAnd => BinaryOperatorType.BitwiseAnd, 
				BinaryOperatorType.InPlaceBitwiseOr => BinaryOperatorType.BitwiseOr, 
				BinaryOperatorType.InPlaceExclusiveOr => BinaryOperatorType.ExclusiveOr, 
				BinaryOperatorType.InPlaceShiftLeft => BinaryOperatorType.ShiftLeft, 
				BinaryOperatorType.InPlaceShiftRight => BinaryOperatorType.ShiftRight, 
				_ => throw new ArgumentException("op"), 
			};
		}

		private void BindArrayAddition(BinaryExpression node)
		{
			IArrayType arrayType = (IArrayType)GetExpressionType(node.Left);
			IArrayType arrayType2 = (IArrayType)GetExpressionType(node.Right);
			if (arrayType.ElementType == arrayType2.ElementType)
			{
				node.ParentNode.Replace(node, base.CodeBuilder.CreateCast(arrayType, base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_AddArrays, base.CodeBuilder.CreateTypeofExpression(arrayType.ElementType), node.Left, node.Right)));
			}
			else
			{
				InvalidOperatorForTypes(node);
			}
		}

		private void BindArithmeticOperator(BinaryExpression node)
		{
			BindNullableOperation(node);
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			if (base.TypeSystemServices.IsPrimitiveNumber(expressionType) && base.TypeSystemServices.IsPrimitiveNumber(expressionType2))
			{
				BindExpressionType(node, base.TypeSystemServices.GetPromotedNumberType(expressionType, expressionType2));
			}
			else if (expressionType.IsPointer && !BindPointerArithmeticOperator(node, expressionType, expressionType2))
			{
				InvalidOperatorForTypes(node);
			}
			else if (!ResolveOperator(node))
			{
				InvalidOperatorForTypes(node);
			}
		}

		private bool BindPointerArithmeticOperator(BinaryExpression node, IType left, IType right)
		{
			if (!left.IsPointer || !base.TypeSystemServices.IsPrimitiveNumber(right))
			{
				return false;
			}
			switch (node.Operator)
			{
			case BinaryOperatorType.Addition:
			case BinaryOperatorType.Subtraction:
			{
				if (node.ContainsAnnotation("pointerSizeNormalized"))
				{
					return true;
				}
				BindExpressionType(node, left);
				int num = base.TypeSystemServices.SizeOf(left);
				if (num == 1)
				{
					return true;
				}
				IntegerLiteralExpression integerLiteralExpression = node.Right as IntegerLiteralExpression;
				Expression newNode = ((integerLiteralExpression != null) ? ((Expression)new IntegerLiteralExpression(integerLiteralExpression.Value * num)) : ((Expression)new BinaryExpression(BinaryOperatorType.Multiply, node.Right, new IntegerLiteralExpression(num))));
				node.Replace(node.Right, newNode);
				Visit(node.Right);
				node.Annotate("pointerSizeNormalized", num);
				return true;
			}
			default:
				return false;
			}
		}

		private static string GetBinaryOperatorText(BinaryOperatorType op)
		{
			return BooPrinterVisitor.GetBinaryOperatorText(op);
		}

		private static string GetUnaryOperatorText(UnaryOperatorType op)
		{
			return BooPrinterVisitor.GetUnaryOperatorText(op);
		}

		private IEntity ResolveName(Node node, string name)
		{
			IEntity entity = base.NameResolutionService.Resolve(name);
			CheckNameResolution(node, name, entity);
			return entity;
		}

		private IEntity TryToResolveName(string name)
		{
			return base.NameResolutionService.Resolve(name);
		}

		protected void ClearResolutionCacheFor(string name)
		{
			base.NameResolutionService.ClearResolutionCacheFor(name);
		}

		protected bool CheckNameResolution(Node node, string name, IEntity resolvedEntity)
		{
			if (null == resolvedEntity)
			{
				EmitUnknownIdentifierError(node, name);
				return false;
			}
			return true;
		}

		protected void EmitUnknownIdentifierError(Node node, string name)
		{
			Error(CompilerErrorFactory.UnknownIdentifier(node, name));
		}

		private static bool IsPublicEvent(IEntity tag)
		{
			return EntityType.Event == tag.EntityType && ((IMember)tag).IsPublic;
		}

		private static bool IsVisibleFieldPropertyOrEvent(IEntity entity)
		{
			switch (entity.EntityType)
			{
			case EntityType.Field:
			{
				IField field = (IField)entity;
				return !TypeSystemServices.IsReadOnlyField(field) && IsVisible(field);
			}
			case EntityType.Event:
			{
				IEvent @event = (IEvent)entity;
				return IsVisible(@event.GetAddMethod());
			}
			case EntityType.Property:
			{
				IProperty member = (IProperty)entity;
				return IsVisible(member);
			}
			default:
				return false;
			}
		}

		private static bool IsVisible(IAccessibleMember member)
		{
			return member.IsPublic || member.IsInternal;
		}

		private IMember ResolveVisibleFieldPropertyOrEvent(Expression sourceNode, IType type, string name)
		{
			IEntity entity = ResolveFieldPropertyEvent(type, name);
			if (null == entity)
			{
				return null;
			}
			if (IsVisibleFieldPropertyOrEvent(entity))
			{
				return (IMember)entity;
			}
			if (!entity.IsAmbiguous())
			{
				return null;
			}
			IList<IEntity> list = ((Ambiguous)entity).Select(IsVisibleFieldPropertyOrEvent);
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return (IMember)list[0];
			}
			Error(sourceNode, CompilerErrorFactory.AmbiguousReference(sourceNode, name, list));
			return null;
		}

		protected IEntity ResolveFieldPropertyEvent(IType type, string name)
		{
			return base.NameResolutionService.Resolve(type, name, EntityType.Field | EntityType.Property | EntityType.Event);
		}

		private void ResolveNamedArguments(IType type, ExpressionPairCollection arguments)
		{
			foreach (ExpressionPair argument in arguments)
			{
				if (ProcessNamedArgument(argument))
				{
					ResolveNamedArgument(type, (ReferenceExpression)argument.First, argument.Second);
				}
			}
		}

		private bool ProcessNamedArgument(ExpressionPair arg)
		{
			Visit(arg.Second);
			if (NodeType.ReferenceExpression != arg.First.NodeType)
			{
				Error(arg.First, CompilerErrorFactory.NamedParameterMustBeIdentifier(arg));
				return false;
			}
			return true;
		}

		private void ResolveNamedArgument(IType type, ReferenceExpression name, Expression value)
		{
			IMember member = ResolveVisibleFieldPropertyOrEvent(name, type, name.Name);
			if (null == member)
			{
				NamedArgumentNotFound(type, name);
				return;
			}
			EnsureRelatedNodeWasVisited(name, member);
			Bind(name, member);
			IType type2 = member.Type;
			if (member.EntityType == EntityType.Event)
			{
				AssertDelegateArgument(value, member, GetExpressionType(value));
			}
			else
			{
				AssertTypeCompatibility(value, type2, GetExpressionType(value));
			}
		}

		protected virtual void NamedArgumentNotFound(IType type, ReferenceExpression name)
		{
			Error(name, CompilerErrorFactory.NotAPublicFieldOrProperty(name, name.Name, type));
		}

		private bool AssertTypeCompatibility(Node sourceNode, IType expectedType, IType actualType)
		{
			return TypeChecker.AssertTypeCompatibility(sourceNode, expectedType, actualType);
		}

		private bool CanBeReachedFrom(Node anchor, IType expectedType, IType actualType)
		{
			return TypeChecker.CanBeReachedFrom(anchor, expectedType, actualType);
		}

		private bool AssertDelegateArgument(Node sourceNode, ITypedEntity delegateMember, ITypedEntity argumentInfo)
		{
			if (!IsAssignableFrom(delegateMember.Type, argumentInfo.Type))
			{
				Error(CompilerErrorFactory.EventArgumentMustBeAMethod(sourceNode, delegateMember));
				return false;
			}
			return true;
		}

		private bool CheckParameterTypesStrictly(IMethod method, ExpressionCollection args)
		{
			IParameter[] parameters = method.GetParameters();
			for (int i = 0; i < args.Count; i++)
			{
				IType expressionType = GetExpressionType(args[i]);
				IType type = parameters[i].Type;
				if (!IsAssignableFrom(type, expressionType) && (!base.TypeSystemServices.IsNumber(expressionType) || !base.TypeSystemServices.IsNumber(type)) && base.TypeSystemServices.FindImplicitConversionOperator(expressionType, type) == null)
				{
					return false;
				}
			}
			return true;
		}

		private bool AssertParameterTypes(ICallableType method, ExpressionCollection args, int count, bool reportErrors)
		{
			IParameter[] parameters = method.GetSignature().Parameters;
			for (int i = 0; i < count; i++)
			{
				IParameter parameter = parameters[i];
				IType type = parameter.Type;
				IType expressionType = GetExpressionType(args[i]);
				if (parameter.IsByRef)
				{
					if (!(args[i] is ReferenceExpression) && !(args[i] is SlicingExpression) && (!(args[i] is SelfLiteralExpression) || !expressionType.IsValueType))
					{
						if (reportErrors)
						{
							Error(CompilerErrorFactory.RefArgTakesLValue(args[i]));
						}
						return false;
					}
					if (!CallableResolutionService.IsValidByRefArg(parameter, type, expressionType, args[i]))
					{
						return false;
					}
				}
				else if (!CanBeReachedFrom(args[i], type, expressionType))
				{
					return false;
				}
			}
			return true;
		}

		private bool AssertParameters(Node sourceNode, IMethod method, ExpressionCollection args)
		{
			return AssertParameters(sourceNode, method, method.CallableType, args);
		}

		private bool AcceptVarArgs(ICallableType method)
		{
			return method.GetSignature().AcceptVarArgs;
		}

		private bool AssertParameters(Node sourceNode, IEntity sourceEntity, ICallableType method, ExpressionCollection args)
		{
			if (CheckParameters(method, args, reportErrors: true))
			{
				return true;
			}
			if (IsLikelyMacroExtensionMethodInvocation(sourceEntity))
			{
				Error(CompilerErrorFactory.MacroExpansionError(sourceNode));
			}
			else
			{
				Error(CompilerErrorFactory.MethodSignature(sourceNode, sourceEntity, GetSignature(args)));
			}
			return false;
		}

		private bool IsLikelyMacroExtensionMethodInvocation(IEntity entity)
		{
			IMethod method = entity as IMethod;
			return method != null && method.IsExtension && base.TypeSystemServices.IsMacro(method.ReturnType) && method.GetParameters().Length == 2 && base.TypeSystemServices.IsMacro(method.GetParameters()[0].Type);
		}

		protected virtual bool CheckParameters(ICallableType method, ExpressionCollection args, bool reportErrors)
		{
			BindNullableParameters(args, method);
			return AcceptVarArgs(method) ? CheckVarArgsParameters(method, args) : CheckExactArgsParameters(method, args, reportErrors);
		}

		protected bool CheckVarArgsParameters(ICallableType method, ExpressionCollection args)
		{
			return CallableResolutionService.IsValidVargsInvocation(method.GetSignature().Parameters, args);
		}

		protected bool CheckExactArgsParameters(ICallableType method, ExpressionCollection args, bool reportErrors)
		{
			if (method.GetSignature().Parameters.Length != args.Count)
			{
				return false;
			}
			return AssertParameterTypes(method, args, args.Count, reportErrors);
		}

		private bool IsRuntimeIterator(IType type)
		{
			return base.TypeSystemServices.IsSystemObject(type) || IsTextReader(type);
		}

		private bool IsTextReader(IType type)
		{
			return IsAssignableFrom(typeof(TextReader), type);
		}

		private bool AssertTargetContext(Expression targetContext, IMember member)
		{
			if (member.IsStatic)
			{
				return true;
			}
			if (NodeType.MemberReferenceExpression != targetContext.NodeType)
			{
				return true;
			}
			Expression target = ((MemberReferenceExpression)targetContext).Target;
			IEntity entity = target.Entity;
			if ((entity != null && EntityType.Type == entity.EntityType) || (NodeType.SelfLiteralExpression == target.NodeType && _currentMethod.IsStatic))
			{
				Error(CompilerErrorFactory.InstanceRequired(targetContext, member));
				return false;
			}
			return true;
		}

		private static bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		private bool IsAssignableFrom(Type expectedType, IType actualType)
		{
			return IsAssignableFrom(base.TypeSystemServices.Map(expectedType), actualType);
		}

		private bool IsPrimitiveNumber(Expression expression)
		{
			return base.TypeSystemServices.IsPrimitiveNumber(GetExpressionType(expression));
		}

		private IConstructor GetCorrectConstructor(Node sourceNode, IType type, ExpressionCollection arguments)
		{
			IConstructor[] array = type.GetConstructors().ToArray();
			if (array.Length > 0)
			{
				return (IConstructor)GetCorrectCallableReference(sourceNode, arguments, array);
			}
			if (!IsError(type))
			{
				if (type is IGenericParameter)
				{
					Error(CompilerErrorFactory.CannotCreateAnInstanceOfGenericParameterWithoutDefaultConstructorConstraint(sourceNode, type));
				}
				else
				{
					Error(CompilerErrorFactory.NoApropriateConstructorFound(sourceNode, type, GetSignature(arguments)));
				}
			}
			return null;
		}

		private IEntity GetCorrectCallableReference(Node sourceNode, ExpressionCollection args, IEntity[] candidates)
		{
			EnsureRelatedNodesWereVisited(sourceNode, candidates);
			IEntity entity = CallableResolutionService.ResolveCallableReference(args, candidates);
			if (entity == null)
			{
				EmitCallableResolutionError(sourceNode, candidates, args);
			}
			else
			{
				BindNullableParameters(args, ((IMethodBase)entity).CallableType);
			}
			return entity;
		}

		private void EnsureRelatedNodesWereVisited(Node sourceNode, IEntity[] candidates)
		{
			foreach (IEntity entity in candidates)
			{
				EnsureRelatedNodeWasVisited(sourceNode, entity);
			}
		}

		private void EmitCallableResolutionError(Node sourceNode, IEntity[] candidates, ExpressionCollection args)
		{
			IMethod method = candidates.OfType<IMethod>().FirstOrDefault((IMethod m) => m.GenericInfo != null && m.GetParameters().Length == 0);
			if (args.Count == 0 && method != null)
			{
				Error(CompilerErrorFactory.CannotInferGenericMethodArguments(sourceNode, method));
				return;
			}
			if (CallableResolutionService.ValidCandidates.Count > 1)
			{
				Error(CompilerErrorFactory.AmbiguousReference(sourceNode, candidates[0].Name, ((IEnumerable<CallableResolutionService.Candidate>)CallableResolutionService.ValidCandidates).Select((Func<CallableResolutionService.Candidate, IEntity>)((CallableResolutionService.Candidate c) => c.Method))));
				return;
			}
			IEntity entity = candidates[0];
			IConstructor constructor = entity as IConstructor;
			if (constructor != null)
			{
				Error(CompilerErrorFactory.NoApropriateConstructorFound(sourceNode, constructor.DeclaringType, GetSignature(args)));
			}
			else
			{
				Error(CompilerErrorFactory.NoApropriateOverloadFound(sourceNode, GetSignature(args), entity.FullName));
			}
		}

		protected override void EnsureRelatedNodeWasVisited(Node sourceNode, IEntity entity)
		{
			IInternalEntity internalEntity = AbstractVisitorCompilerStep.GetConstructedInternalEntity(entity);
			if (null == internalEntity)
			{
				ITypedEntity typedEntity = entity as ITypedEntity;
				if (null == typedEntity)
				{
					return;
				}
				internalEntity = typedEntity.Type as IInternalEntity;
				if (null == internalEntity)
				{
					return;
				}
			}
			Node node = internalEntity.Node;
			switch (node.NodeType)
			{
			case NodeType.Field:
			case NodeType.Property:
			{
				IMember member = (IMember)entity;
				if (EntityType.Property == entity.EntityType || TypeSystemServices.IsUnknown(member.Type))
				{
					EnsureMemberWasVisited((TypeMember)node);
					AssertTypeIsKnown(sourceNode, member, member.Type);
				}
				break;
			}
			case NodeType.Method:
			{
				IMethod method = (IMethod)entity;
				if (TypeSystemServices.IsUnknown(method.ReturnType))
				{
					Method node2 = (Method)node;
					PreProcessMethod(node2);
					if (TypeSystemServices.IsUnknown(method.ReturnType))
					{
						EnsureMemberWasVisited(node2);
						AssertTypeIsKnown(sourceNode, method, method.ReturnType);
					}
				}
				break;
			}
			case NodeType.ClassDefinition:
			case NodeType.StructDefinition:
			case NodeType.InterfaceDefinition:
				EnsureMemberWasVisited((TypeDefinition)node);
				break;
			case NodeType.EnumDefinition:
			case NodeType.EnumMember:
			case NodeType.Event:
			case NodeType.Local:
			case NodeType.BlockExpression:
				break;
			}
		}

		private void EnsureMemberWasVisited(TypeMember node)
		{
			if (!WasVisited(node))
			{
				_context.TraceVerbose("Info {0} needs resolving.", node.Entity.Name);
				VisitMemberPreservingContext(node);
			}
		}

		protected virtual void VisitMemberPreservingContext(TypeMember node)
		{
			INamespace currentNamespace = base.NameResolutionService.CurrentNamespace;
			try
			{
				base.NameResolutionService.EnterNamespace((INamespace)node.DeclaringType.Entity);
				Visit(node);
			}
			finally
			{
				base.NameResolutionService.EnterNamespace(currentNamespace);
			}
		}

		private void AssertTypeIsKnown(Node sourceNode, IEntity sourceEntity, IType type)
		{
			if (TypeSystemServices.IsUnknown(type))
			{
				Error(CompilerErrorFactory.UnresolvedDependency(sourceNode, GetEntity(CurrentMember), sourceEntity));
			}
		}

		private bool ResolveOperator(UnaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo);
			methodInvocationExpression.Arguments.Add(node.Operand.CloneNode());
			string methodNameForOperator = AstUtil.GetMethodNameForOperator(node.Operator);
			IType expressionType = GetExpressionType(node.Operand);
			if (ResolveOperator(node, expressionType, methodNameForOperator, methodInvocationExpression))
			{
				return true;
			}
			return ResolveOperator(node, base.TypeSystemServices.RuntimeServicesType, methodNameForOperator, methodInvocationExpression);
		}

		private bool ResolveOperator(BinaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo);
			methodInvocationExpression.Arguments.Add(node.Left.CloneNode());
			methodInvocationExpression.Arguments.Add(node.Right.CloneNode());
			string methodNameForOperator = AstUtil.GetMethodNameForOperator(node.Operator);
			IType expressionType = GetExpressionType(node.Left);
			if (ResolveOperator(node, expressionType, methodNameForOperator, methodInvocationExpression))
			{
				return true;
			}
			IType expressionType2 = GetExpressionType(node.Right);
			if (ResolveOperator(node, expressionType2, methodNameForOperator, methodInvocationExpression))
			{
				return true;
			}
			if (expressionType.IsPointer && base.TypeSystemServices.IsIntegerNumber(expressionType2))
			{
				switch (node.Operator)
				{
				case BinaryOperatorType.Addition:
				case BinaryOperatorType.Subtraction:
					return true;
				}
			}
			return ResolveRuntimeOperator(node, methodNameForOperator, methodInvocationExpression);
		}

		protected virtual bool ResolveRuntimeOperator(BinaryExpression node, string operatorName, MethodInvocationExpression mie)
		{
			return ResolveOperator(node, base.TypeSystemServices.RuntimeServicesType, operatorName, mie);
		}

		private IMethod ResolveAmbiguousOperator(IEntity[] entities, ExpressionCollection args)
		{
			foreach (IEntity entity in entities)
			{
				IMethod method = entity as IMethod;
				if (null != method && HasOperatorSignature(method, args))
				{
					return method;
				}
			}
			return null;
		}

		private bool HasOperatorSignature(IMethod method, ExpressionCollection args)
		{
			return method.IsStatic && args.Count == method.GetParameters().Length && CheckParameterTypesStrictly(method, args);
		}

		private IMethod FindOperator(IType type, string operatorName, ExpressionCollection args)
		{
			IEntity entity = base.NameResolutionService.Resolve(type, operatorName, EntityType.Method);
			if (entity != null)
			{
				IMethod method = ResolveOperatorEntity(entity, args);
				if (null != method)
				{
					return method;
				}
			}
			entity = base.NameResolutionService.ResolveExtension(type, operatorName);
			if (entity != null)
			{
				return ResolveOperatorEntity(entity, args);
			}
			return null;
		}

		private IMethod ResolveOperatorEntity(IEntity op, ExpressionCollection args)
		{
			if (op.IsAmbiguous())
			{
				return ResolveAmbiguousOperator(((Ambiguous)op).Entities, args);
			}
			if (EntityType.Method == op.EntityType)
			{
				IMethod method = (IMethod)op;
				if (HasOperatorSignature(method, args))
				{
					return method;
				}
			}
			return null;
		}

		private bool ResolveOperator(Expression node, IType type, string operatorName, MethodInvocationExpression mie)
		{
			IMethod method = FindOperator(type, operatorName, mie.Arguments);
			if (null == method)
			{
				return false;
			}
			EnsureRelatedNodeWasVisited(node, method);
			mie.Target = new ReferenceExpression(method.FullName);
			IMethod method2 = method;
			BindExpressionType(mie, method2.ReturnType);
			BindExpressionType(mie.Target, method2.Type);
			Bind(mie.Target, method);
			node.ParentNode.Replace(node, mie);
			return true;
		}

		private ReferenceExpression CreateTempLocal(LexicalInfo li, IType type)
		{
			InternalLocal internalLocal = DeclareTempLocal(type);
			ReferenceExpression referenceExpression = new ReferenceExpression(li, internalLocal.Name);
			referenceExpression.Entity = internalLocal;
			referenceExpression.ExpressionType = type;
			return referenceExpression;
		}

		protected InternalLocal DeclareTempLocal(IType localType)
		{
			return base.CodeBuilder.DeclareTempLocal(CurrentMethod, localType);
		}

		private IEntity DeclareLocal(Node sourceNode, string name, IType localType)
		{
			return DeclareLocal(sourceNode, name, localType, privateScope: false);
		}

		protected virtual IEntity DeclareLocal(Node sourceNode, string name, IType localType, bool privateScope)
		{
			ClearResolutionCacheFor(name);
			Local local = new Local(name, privateScope);
			local.LexicalInfo = sourceNode.LexicalInfo;
			InternalLocal result = (InternalLocal)(local.Entity = new InternalLocal(local, localType));
			CurrentMethod.Locals.Add(local);
			return result;
		}

		private void PushMember(TypeMember member)
		{
			_memberStack.Push(member);
		}

		private void PopMember()
		{
			_memberStack.Pop();
		}

		private void PushMethodInfo(InternalMethod entity)
		{
			_methodStack.Push(_currentMethod);
			_currentMethod = entity;
		}

		private void PopMethodInfo()
		{
			_currentMethod = _methodStack.Pop();
		}

		private void AssertHasSideEffect(Expression expression)
		{
			if (!HasSideEffect(expression) && !TypeSystemServices.IsError(expression))
			{
				Error(CompilerErrorFactory.ExpressionMustBeExecutedForItsSideEffects(expression));
			}
		}

		protected virtual bool HasSideEffect(Expression node)
		{
			return node.NodeType == NodeType.MethodInvocationExpression || AstUtil.IsAssignment(node) || AstUtil.IsIncDec(node);
		}

		private bool AssertCanCreateInstance(Node sourceNode, IType type)
		{
			if (type.IsInterface)
			{
				Error(CompilerErrorFactory.CantCreateInstanceOfInterface(sourceNode, type));
				return false;
			}
			if (type.IsEnum)
			{
				Error(CompilerErrorFactory.CantCreateInstanceOfEnum(sourceNode, type));
				return false;
			}
			if (type.IsAbstract)
			{
				Error(CompilerErrorFactory.CantCreateInstanceOfAbstractType(sourceNode, type));
				return false;
			}
			if (!(type is GenericConstructedType) && ((type.GenericInfo != null && type.GenericInfo.GenericParameters.Length > 0) || (type.ConstructedInfo != null && !type.ConstructedInfo.FullyConstructed)))
			{
				Error(CompilerErrorFactory.GenericTypesMustBeConstructedToBeInstantiated(sourceNode));
				return false;
			}
			return true;
		}

		protected bool AssertDeclarationName(Declaration d)
		{
			if (AssertIdentifierName(d, d.Name))
			{
				return AssertUniqueLocal(d);
			}
			return false;
		}

		protected bool AssertUniqueLocal(Declaration d)
		{
			if (_currentMethod.ResolveLocal(d.Name) == null && null == _currentMethod.ResolveParameter(d.Name))
			{
				return true;
			}
			Error(CompilerErrorFactory.LocalAlreadyExists(d, d.Name));
			return false;
		}

		private void GetDeclarationType(IType defaultDeclarationType, Declaration d)
		{
			if (null != d.Type)
			{
				Visit(d.Type);
				AssertTypeCompatibility(d, GetType(d.Type), defaultDeclarationType);
			}
			else
			{
				d.Type = base.CodeBuilder.CreateTypeReference(defaultDeclarationType);
			}
		}

		private void DeclareLocal(Declaration d, bool privateScope)
		{
			AssertIdentifierName(d, d.Name);
			IEntity entity2 = (d.Entity = DeclareLocal(d, d.Name, GetType(d.Type), privateScope));
			InternalLocal internalLocal = entity2 as InternalLocal;
			if (internalLocal != null)
			{
				internalLocal.OriginalDeclaration = d;
			}
		}

		protected IType GetEnumeratorItemType(IType iteratorType)
		{
			return base.TypeSystemServices.GetEnumeratorItemType(iteratorType);
		}

		protected void ProcessDeclarationsForIterator(DeclarationCollection declarations, IType iteratorType)
		{
			IType enumeratorItemType = GetEnumeratorItemType(iteratorType);
			if (declarations.Count > 1)
			{
				enumeratorItemType = GetEnumeratorItemType(enumeratorItemType);
			}
			foreach (Declaration declaration in declarations)
			{
				ProcessDeclarationForIterator(declaration, enumeratorItemType);
			}
		}

		protected virtual Local LocalToReuseFor(Declaration d)
		{
			return (d.Type != null) ? null : LocalByName(d.Name);
		}

		protected Local LocalByName(string name)
		{
			return AstUtil.GetLocalByName(CurrentMethod, name);
		}

		protected void ProcessDeclarationForIterator(Declaration d, IType defaultDeclType)
		{
			Local local = LocalToReuseFor(d);
			if (local != null)
			{
				IType type = ((InternalLocal)GetEntity(local)).Type;
				AssertTypeCompatibility(d, type, defaultDeclType);
				d.Type = base.CodeBuilder.CreateTypeReference(type);
				d.Entity = local.Entity;
			}
			else
			{
				GetDeclarationType(defaultDeclType, d);
				DeclareLocal(d, privateScope: true);
			}
		}

		private bool AssertLValue(Expression node)
		{
			if (IsError(GetExpressionType(node)))
			{
				return false;
			}
			IEntity entity = node.Entity;
			if (null != entity)
			{
				return AssertLValue(node, entity);
			}
			if (IsArraySlicing(node))
			{
				return true;
			}
			LValueExpected(node);
			return false;
		}

		private void LValueExpected(Node node)
		{
			IEntity entity = node.Entity;
			if (entity == null || !IsError(entity))
			{
				Error(CompilerErrorFactory.LValueExpected(node));
			}
		}

		protected virtual bool AssertLValue(Node node, IEntity entity)
		{
			if (null != entity)
			{
				switch (entity.EntityType)
				{
				case EntityType.Error:
					return false;
				case EntityType.Event:
				case EntityType.Local:
				case EntityType.Parameter:
					return true;
				case EntityType.Property:
				{
					IProperty property = (IProperty)entity;
					if (property.GetSetMethod() == null)
					{
						Error(CompilerErrorFactory.PropertyIsReadOnly(MemberAnchorFor(node), property));
						return false;
					}
					return true;
				}
				case EntityType.Field:
				{
					IField field = (IField)entity;
					if (TypeSystemServices.IsReadOnlyField(field))
					{
						if (EntityType.Constructor != _currentMethod.EntityType || _currentMethod.DeclaringType != field.DeclaringType || field.IsStatic != _currentMethod.IsStatic)
						{
							Error(CompilerErrorFactory.FieldIsReadonly(MemberAnchorFor(node), entity.FullName));
							return false;
						}
						InternalField internalField = entity as InternalField;
						if (internalField != null && internalField.IsStatic)
						{
							internalField.StaticValue = null;
						}
					}
					return true;
				}
				}
			}
			LValueExpected(node);
			return false;
		}

		public static bool IsArraySlicing(Node node)
		{
			if (node.NodeType != NodeType.SlicingExpression)
			{
				return false;
			}
			return ((SlicingExpression)node).Target.ExpressionType?.IsArray ?? false;
		}

		private static bool IsStandaloneReference(Node node)
		{
			return AstUtil.IsStandaloneReference(node);
		}

		private string GetSignature(IEnumerable args)
		{
			StringBuilder stringBuilder = new StringBuilder("(");
			foreach (Expression arg in args)
			{
				if (stringBuilder.Length > 1)
				{
					stringBuilder.Append(", ");
				}
				if (AstUtil.IsExplodeExpression(arg))
				{
					stringBuilder.Append('*');
				}
				stringBuilder.Append(GetExpressionType(arg).DisplayName());
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		private void InvalidOperatorForType(UnaryExpression node)
		{
			Error(node, CompilerErrorFactory.InvalidOperatorForType(node, GetUnaryOperatorText(node.Operator), GetExpressionType(node.Operand)));
		}

		private void InvalidOperatorForTypes(BinaryExpression node)
		{
			Error(node, CompilerErrorFactory.InvalidOperatorForTypes(node, GetBinaryOperatorText(node.Operator), GetExpressionType(node.Left), GetExpressionType(node.Right)));
		}

		private void TraceReturnType(Method method, IMethod tag)
		{
			_context.TraceInfo("{0}: return type for method {1} bound to {2}", method.LexicalInfo, method.Name, tag.ReturnType);
		}

		public TypeMember Reify(TypeMember member)
		{
			Visit(member);
			Field field = member as Field;
			if (field != null)
			{
				FlushFieldInitializers((ClassDefinition)field.DeclaringType);
			}
			return member;
		}
	}
}
