using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class StricterErrorChecking : AbstractNamespaceSensitiveVisitorCompilerStep
	{
		private Dictionary<string, TypeDefinition> _types = new Dictionary<string, TypeDefinition>(StringComparer.Ordinal);

		private int _ensureBlock;

		private Set<string> _safeVars = new Set<string>();

		private bool InsideEnsure => _ensureBlock > 0;

		public override void Dispose()
		{
			base.Dispose();
			_types.Clear();
		}

		public override void LeaveCompileUnit(CompileUnit node)
		{
			CheckEntryPoint();
		}

		private void CheckEntryPoint()
		{
			Method entryPoint = ContextAnnotations.GetEntryPoint(base.Context);
			if (null != entryPoint)
			{
				IMethod method = (IMethod)TypeSystemServices.GetEntity(entryPoint);
				if (!IsValidEntryPointReturnType(method.ReturnType) || !IsValidEntryPointParameterList(method.GetParameters()))
				{
					base.Errors.Add(CompilerErrorFactory.InvalidEntryPoint(entryPoint));
				}
			}
		}

		private bool IsValidEntryPointParameterList(IParameter[] parameters)
		{
			if (parameters.Length == 0)
			{
				return true;
			}
			if (parameters.Length != 1)
			{
				return false;
			}
			return parameters[0].Type == base.TypeSystemServices.StringType.MakeArrayType(1);
		}

		private bool IsValidEntryPointReturnType(IType type)
		{
			return type == base.TypeSystemServices.VoidType || type == base.TypeSystemServices.IntType;
		}

		public override void LeaveTypeDefinition(TypeDefinition node)
		{
			_safeVars.Clear();
			if (node.NodeType != NodeType.Module)
			{
				string text = node.QualifiedName;
				if (node.HasGenericParameters)
				{
					text = text + "`" + node.GenericParameters.Count;
				}
				if (_types.ContainsKey(text))
				{
					base.Errors.Add(CompilerErrorFactory.NamespaceAlreadyContainsMember(node, GetNamespace(node), node.Name));
				}
				else
				{
					_types.Add(text, node);
				}
			}
		}

		private string GetNamespace(TypeDefinition node)
		{
			NamespaceDeclaration enclosingNamespace = node.EnclosingNamespace;
			return (enclosingNamespace == null) ? "" : enclosingNamespace.Name;
		}

		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			if (!AstUtil.IsTargetOfMethodInvocation(node) && !AstUtil.IsTargetOfMemberReference(node))
			{
				Error(CompilerErrorFactory.InvalidSuper(node));
			}
		}

		public override void OnTryStatement(TryStatement node)
		{
			Visit(node.ProtectedBlock);
			Visit(node.ExceptionHandlers);
			EnterEnsureBlock();
			Visit(node.FailureBlock);
			Visit(node.EnsureBlock);
			LeaveEnsureBlock();
		}

		private void EnterEnsureBlock()
		{
			_ensureBlock++;
		}

		private void LeaveEnsureBlock()
		{
			_ensureBlock--;
		}

		public override void OnCustomStatement(CustomStatement node)
		{
			Error(CompilerErrorFactory.InvalidNode(node));
		}

		public override void LeaveReturnStatement(ReturnStatement node)
		{
			if (InsideEnsure)
			{
				Error(CompilerErrorFactory.CantReturnFromEnsure(node));
			}
			if (null != node.Expression)
			{
				CheckExpressionType(node.Expression);
			}
		}

		public override void LeaveYieldStatement(YieldStatement node)
		{
			if (null != node.Expression)
			{
				CheckExpressionType(node.Expression);
			}
		}

		public override void LeaveExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			foreach (Expression expression in node.Expressions)
			{
				CheckExpressionType(expression);
			}
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			UnaryOperatorType @operator = node.Operator;
			if (@operator == UnaryOperatorType.Explode)
			{
				LeaveExplodeExpression(node);
			}
		}

		public override void LeaveHashLiteralExpression(HashLiteralExpression node)
		{
			foreach (ExpressionPair item in node.Items)
			{
				CheckExpressionType(item.First);
				CheckExpressionType(item.Second);
			}
		}

		public override void LeaveGeneratorExpression(GeneratorExpression node)
		{
			CheckExpressionType(node.Expression);
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (CheckExpressionType(node.Right))
			{
				CheckExpressionType(node.Left);
			}
			if (BinaryOperatorType.ReferenceEquality == node.Operator && IsTypeReference(node.Right))
			{
				base.Warnings.Add(CompilerWarningFactory.IsInsteadOfIsa(node));
			}
			if (BinaryOperatorType.Assign == node.Operator || AstUtil.GetBinaryOperatorKind(node) == BinaryOperatorKind.Comparison)
			{
				if (AreSameExpressions(node.Left, node.Right))
				{
					base.Warnings.Add((BinaryOperatorType.Assign == node.Operator) ? CompilerWarningFactory.AssignmentToSameVariable(node) : CompilerWarningFactory.ComparisonWithSameVariable(node));
				}
				else if (BinaryOperatorType.Assign != node.Operator && AreConstantExpressions(node.Left, node.Right))
				{
					WarnAboutConstantExpression(node);
				}
			}
		}

		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			if (node.ContainsAnnotation("foldedExpression"))
			{
				WarnAboutConstantExpression(node);
			}
		}

		public override void LeaveIfStatement(IfStatement node)
		{
			CheckNotConstant(node.Condition);
		}

		public override void LeaveUnlessStatement(UnlessStatement node)
		{
			CheckNotConstant(node.Condition);
		}

		private void CheckNotConstant(Expression node)
		{
			if (IsConstant(node))
			{
				WarnAboutConstantExpression(WarningAnchorFor(node));
			}
		}

		private static Expression WarningAnchorFor(Expression node)
		{
			GenericReferenceExpression genericReferenceExpression = node as GenericReferenceExpression;
			if (null != genericReferenceExpression)
			{
				return genericReferenceExpression.Target;
			}
			return node;
		}

		private void WarnAboutConstantExpression(Expression node)
		{
			base.Warnings.Add(CompilerWarningFactory.ConstantExpression(node));
		}

		protected virtual void LeaveExplodeExpression(UnaryExpression node)
		{
			if (!IsLastArgumentOfVarArgInvocation(node))
			{
				Error(CompilerErrorFactory.ExplodeExpressionMustMatchVarArgCall(node));
			}
		}

		private static bool AreSameExpressions(Expression a, Expression b)
		{
			if (a.NodeType != b.NodeType)
			{
				return false;
			}
			switch (a.NodeType)
			{
			case NodeType.ReferenceExpression:
			{
				ReferenceExpression referenceExpression = (ReferenceExpression)a;
				ReferenceExpression referenceExpression2 = (ReferenceExpression)b;
				if (referenceExpression.Name != referenceExpression2.Name)
				{
					return false;
				}
				return true;
			}
			case NodeType.MemberReferenceExpression:
			{
				MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)a;
				MemberReferenceExpression memberReferenceExpression2 = (MemberReferenceExpression)b;
				if (memberReferenceExpression.Name != memberReferenceExpression2.Name)
				{
					return false;
				}
				if (!AreSameExpressions(memberReferenceExpression.Target, memberReferenceExpression2.Target))
				{
					return false;
				}
				return true;
			}
			case NodeType.SelfLiteralExpression:
			case NodeType.SuperLiteralExpression:
				return true;
			default:
				return false;
			}
		}

		private static bool AreConstantExpressions(Expression a, Expression b)
		{
			return IsConstant(a) && IsConstant(b);
		}

		private static bool IsConstant(Expression e)
		{
			if (e.NodeType == NodeType.UnaryExpression)
			{
				return IsConstant(((UnaryExpression)e).Operand);
			}
			if (e.NodeType == NodeType.BinaryExpression)
			{
				BinaryExpression binaryExpression = (BinaryExpression)e;
				if (AstUtil.GetBinaryOperatorKind(binaryExpression) == BinaryOperatorKind.Logical)
				{
					return IsConstant(binaryExpression.Left) && IsConstant(binaryExpression.Right);
				}
			}
			if (e is LiteralExpression && !e.ContainsAnnotation("foldedExpression"))
			{
				return true;
			}
			if (IsImplicitCallable(e))
			{
				return true;
			}
			if (IsConstantInternalField(e.Entity as IField, e))
			{
				return true;
			}
			return false;
		}

		private static bool IsConstantInternalField(IField f, Expression e)
		{
			if (null == f)
			{
				return false;
			}
			InternalField internalField = e.Entity as InternalField;
			if (internalField != null && internalField.Field.IsStatic && internalField.IsLiteral)
			{
				return true;
			}
			return false;
		}

		private static bool IsImplicitCallable(Expression e)
		{
			return (e is ReferenceExpression || e is GenericReferenceExpression) && e.Entity is IMethod;
		}

		private static bool IsLastArgumentOfVarArgInvocation(UnaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = node.ParentNode as MethodInvocationExpression;
			if (null == methodInvocationExpression)
			{
				return false;
			}
			if (methodInvocationExpression.Arguments.Last != node)
			{
				return false;
			}
			ICallableType callableType = methodInvocationExpression.Target.ExpressionType as ICallableType;
			if (null != callableType)
			{
				return callableType.GetSignature().AcceptVarArgs;
			}
			return (methodInvocationExpression.Target.Entity as IMethod)?.AcceptVarArgs ?? false;
		}

		private static bool IsTypeReference(Expression node)
		{
			if (NodeType.TypeofExpression == node.NodeType)
			{
				return true;
			}
			return node.Entity is IType && (node is ReferenceExpression || node is GenericReferenceExpression);
		}

		public override void OnGotoStatement(GotoStatement node)
		{
			LabelStatement labelStatement = ((InternalLabel)node.Label.Entity).LabelStatement;
			int tryBlockDepth = AstAnnotations.GetTryBlockDepth(node);
			int tryBlockDepth2 = AstAnnotations.GetTryBlockDepth(labelStatement);
			if (tryBlockDepth < tryBlockDepth2)
			{
				BranchError(node, labelStatement);
			}
			else if (tryBlockDepth == tryBlockDepth2)
			{
				Node parentTryExceptEnsure = AstUtil.GetParentTryExceptEnsure(node);
				Node parentTryExceptEnsure2 = AstUtil.GetParentTryExceptEnsure(labelStatement);
				if (parentTryExceptEnsure != parentTryExceptEnsure2)
				{
					BranchError(node, labelStatement);
				}
			}
		}

		private void BranchError(GotoStatement node, LabelStatement target)
		{
			Node parentTryExceptEnsure = AstUtil.GetParentTryExceptEnsure(target);
			switch (parentTryExceptEnsure.NodeType)
			{
			case NodeType.TryStatement:
				Error(CompilerErrorFactory.CannotBranchIntoTry(node.Label));
				break;
			case NodeType.ExceptionHandler:
				Error(CompilerErrorFactory.CannotBranchIntoExcept(node.Label));
				break;
			case NodeType.Block:
				Error(CompilerErrorFactory.CannotBranchIntoEnsure(node.Label));
				break;
			case NodeType.DeclarationStatement:
			case NodeType.MacroStatement:
				break;
			}
		}

		public override void LeaveMethod(Method node)
		{
			InternalMethod internalMethod = (InternalMethod)node.Entity;
			IMethod overriden = internalMethod.Overriden;
			if (null != overriden)
			{
				TypeMemberModifiers access = TypeSystemServices.GetAccess(internalMethod);
				TypeMemberModifiers access2 = TypeSystemServices.GetAccess(overriden);
				if (access < access2)
				{
					Error(CompilerErrorFactory.DerivedMethodCannotReduceAccess(node, internalMethod, overriden, access, access2));
				}
			}
			CheckUnusedLocals(node);
			CheckAbstractMethodCantHaveBody(node);
			CheckValidExtension(node);
			CheckNotFinalizer(node);
			CheckImplicitReturn(node);
			CheckAmbiguousVariableNames(node);
		}

		private void CheckNotFinalizer(Method node)
		{
			if (node.Name == "Finalize" && !node.IsSynthetic && node.IsOverride && node.Parameters.Count == 0 && 0 == node.GenericParameters.Count)
			{
				base.Warnings.Add(CompilerWarningFactory.OverridingFinalizeIsBadPractice(node));
			}
		}

		private void CheckValidExtension(Method node)
		{
			IMethod entity = GetEntity(node);
			if (!entity.IsExtension)
			{
				return;
			}
			IType type = entity.GetParameters()[0].Type;
			IEntity entity2 = base.NameResolutionService.Resolve(type, entity.Name, EntityType.Method);
			if (entity2 != null)
			{
				IMethod method = FindConflictingMember(entity, entity2);
				if (method?.IsPublic ?? false)
				{
					Error(CompilerErrorFactory.MemberNameConflict(node, type, TypeSystemServices.GetSignature(method)));
				}
			}
		}

		private IMethod FindConflictingMember(IMethod extension, IEntity entity)
		{
			if (entity.IsAmbiguous())
			{
				return FindConflictingMember(extension, ((Ambiguous)entity).Entities);
			}
			IMethod method = (IMethod)entity;
			return IsConflictingMember(extension, method) ? method : null;
		}

		private IMethod FindConflictingMember(IMethod extension, IEntity[] methods)
		{
			for (int i = 0; i < methods.Length; i++)
			{
				IMethod method = (IMethod)methods[i];
				if (IsConflictingMember(extension, method))
				{
					return method;
				}
			}
			return null;
		}

		private static bool IsConflictingMember(IMethod extension, IMethod method)
		{
			IParameter[] parameters = extension.GetParameters();
			IParameter[] parameters2 = method.GetParameters();
			if (parameters2.Length != parameters.Length - 1)
			{
				return false;
			}
			for (int i = 0; i < parameters2.Length; i++)
			{
				if (parameters[i + 1].Type != parameters2[i].Type)
				{
					return false;
				}
			}
			return true;
		}

		private void CheckAbstractMethodCantHaveBody(Method node)
		{
			if (node.IsAbstract && !node.Body.IsEmpty)
			{
				Error(CompilerErrorFactory.AbstractMethodCantHaveBody(node, GetEntity(node)));
			}
		}

		private void CheckUnusedLocals(Method node)
		{
			foreach (Local local in node.Locals)
			{
				if (!(local.Name == "_"))
				{
					InternalLocal internalLocal = (InternalLocal)local.Entity;
					if (!internalLocal.IsPrivateScope && !internalLocal.IsUsed)
					{
						base.Warnings.Add(CompilerWarningFactory.UnusedLocalVariable(local, local.Name));
					}
				}
			}
		}

		private void CheckAmbiguousVariableNames(Method node)
		{
			if (node.DeclaringType == null || null == node.DeclaringType.Entity)
			{
				return;
			}
			InternalClass internalClass = node.DeclaringType.Entity as InternalClass;
			if (internalClass == null || null == internalClass.BaseType || base.Parameters.DisabledWarnings.Contains("BCW0025"))
			{
				return;
			}
			internalClass = internalClass.BaseType as InternalClass;
			foreach (Local local in node.Locals)
			{
				if (local.Entity == null || ((InternalLocal)local.Entity).IsExplicit)
				{
					continue;
				}
				if (_safeVars.Contains(local.Name))
				{
					break;
				}
				bool flag = true;
				while (null != internalClass)
				{
					Field field = internalClass.TypeDefinition.Members[local.Name] as Field;
					if (field != null && field.IsPrivate)
					{
						flag = false;
						base.Warnings.Add(CompilerWarningFactory.AmbiguousVariableName(local, local.Name, internalClass.Name));
						break;
					}
					internalClass = internalClass.BaseType as InternalClass;
				}
				if (flag)
				{
					_safeVars.Add(local.Name);
				}
			}
		}

		private void CheckImplicitReturn(Method node)
		{
			if (!base.Parameters.DisabledWarnings.Contains("BCW0023") && node.ReturnType != null && node.ReturnType.Entity != null && node.ReturnType.Entity != base.TypeSystemServices.VoidType && !node.Body.IsEmpty && !((InternalMethod)node.Entity).IsGenerator && !(node.Name == "ExpandImpl") && !AstUtil.AllCodePathsReturnOrRaise(node.Body))
			{
				base.Warnings.Add(CompilerWarningFactory.ImplicitReturn(node));
			}
		}

		public override void LeaveConstructor(Constructor node)
		{
			if (node.IsStatic)
			{
				if (!node.IsPrivate)
				{
					Error(CompilerErrorFactory.StaticConstructorMustBePrivate(node));
				}
				if (node.Parameters.Count != 0)
				{
					Error(CompilerErrorFactory.StaticConstructorCannotDeclareParameters(node));
				}
			}
			CheckUnusedLocals(node);
		}

		public override void LeaveMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (!IsEvalBuiltin(node.Target))
			{
				CheckArgumentTypes(node);
			}
			if (IsAddressOfBuiltin(node.Target) && !IsSecondArgumentOfDelegateConstructor(node))
			{
				Error(CompilerErrorFactory.AddressOfOutsideDelegateConstructor(node.Target));
			}
		}

		private bool IsEvalBuiltin(Expression expression)
		{
			return expression.Entity == BuiltinFunction.Eval;
		}

		private void CheckArgumentTypes(MethodInvocationExpression node)
		{
			foreach (Expression argument in node.Arguments)
			{
				CheckExpressionType(argument);
			}
		}

		public override void LeaveConditionalExpression(ConditionalExpression node)
		{
			CheckExpressionType(node.TrueValue);
			CheckExpressionType(node.FalseValue);
		}

		public override void LeaveExceptionHandler(ExceptionHandler node)
		{
			if (node.Declaration.Type.Entity != null && ((IType)node.Declaration.Type.Entity).FullName == "System.Exception" && !string.IsNullOrEmpty(node.Declaration.Name) && null != base.NameResolutionService.ResolveTypeName(new SimpleTypeReference(node.Declaration.Name)))
			{
				base.Warnings.Add(CompilerWarningFactory.AmbiguousExceptionName(node));
			}
		}

		public override void OnRELiteralExpression(RELiteralExpression node)
		{
			int regexOptions = (int)AstUtil.GetRegexOptions(node);
		}

		private static bool IsSecondArgumentOfDelegateConstructor(Expression node)
		{
			MethodInvocationExpression methodInvocationExpression = node.ParentNode as MethodInvocationExpression;
			if (null != methodInvocationExpression && IsDelegateConstructorInvocation(methodInvocationExpression))
			{
				return methodInvocationExpression.Arguments[1] == node;
			}
			return false;
		}

		private static bool IsDelegateConstructorInvocation(MethodInvocationExpression node)
		{
			IConstructor constructor = node.Target.Entity as IConstructor;
			if (null != constructor)
			{
				return constructor.DeclaringType is ICallableType;
			}
			return false;
		}

		private static bool IsAddressOfBuiltin(Expression node)
		{
			return BuiltinFunction.AddressOf == node.Entity;
		}

		private bool CheckExpressionType(Expression node)
		{
			IType expressionType = node.ExpressionType;
			if (expressionType != base.TypeSystemServices.VoidType)
			{
				return true;
			}
			Error(CompilerErrorFactory.InvalidExpressionType(node, expressionType));
			return false;
		}
	}
}
