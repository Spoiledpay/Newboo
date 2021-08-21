using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.Steps
{
	public class ExpandDuckTypedExpressions : AbstractTransformerCompilerStep
	{
		protected IMethod RuntimeServices_Invoke;

		protected IMethod RuntimeServices_InvokeCallable;

		protected IMethod RuntimeServices_InvokeBinaryOperator;

		protected IMethod RuntimeServices_InvokeUnaryOperator;

		protected IMethod RuntimeServices_SetProperty;

		protected IMethod RuntimeServices_GetProperty;

		protected IMethod RuntimeServices_SetSlice;

		protected IMethod RuntimeServices_GetSlice;

		protected IType _duckTypingServicesType;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			InitializeDuckTypingServices();
		}

		protected virtual void InitializeDuckTypingServices()
		{
			_duckTypingServicesType = GetDuckTypingServicesType();
			RuntimeServices_Invoke = ResolveInvokeMethod();
			RuntimeServices_InvokeCallable = ResolveMethod(_duckTypingServicesType, "InvokeCallable");
			RuntimeServices_InvokeBinaryOperator = ResolveMethod(_duckTypingServicesType, "InvokeBinaryOperator");
			RuntimeServices_InvokeUnaryOperator = ResolveMethod(_duckTypingServicesType, "InvokeUnaryOperator");
			RuntimeServices_SetProperty = ResolveSetPropertyMethod();
			RuntimeServices_GetProperty = ResolveGetPropertyMethod();
			RuntimeServices_SetSlice = ResolveMethod(_duckTypingServicesType, "SetSlice");
			RuntimeServices_GetSlice = ResolveMethod(_duckTypingServicesType, "GetSlice");
		}

		protected virtual IMethod ResolveInvokeMethod()
		{
			return ResolveMethod(_duckTypingServicesType, "Invoke");
		}

		protected virtual IMethod ResolveGetPropertyMethod()
		{
			return ResolveMethod(_duckTypingServicesType, "GetProperty");
		}

		protected virtual IMethod ResolveSetPropertyMethod()
		{
			return ResolveMethod(_duckTypingServicesType, "SetProperty");
		}

		protected virtual IType GetDuckTypingServicesType()
		{
			return base.TypeSystemServices.Map(typeof(RuntimeServices));
		}

		protected virtual IMethod GetGetPropertyMethod()
		{
			return RuntimeServices_GetProperty;
		}

		protected virtual IMethod GetSetPropertyMethod()
		{
			return RuntimeServices_SetProperty;
		}

		protected IMethod ResolveMethod(IType type, string name)
		{
			IMethod method = base.NameResolutionService.ResolveMethod(type, name);
			if (null == method)
			{
				throw new ArgumentException($"Method '{type}' not found in type '{name}'");
			}
			return method;
		}

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (!base.TypeSystemServices.IsDuckTyped(node.Target))
			{
				base.OnMethodInvocationExpression(node);
				return;
			}
			if (base.TypeSystemServices.IsQuackBuiltin(node.Target))
			{
				ExpandQuackInvocation(node);
				return;
			}
			base.OnMethodInvocationExpression(node);
			if (node.GetAncestor(NodeType.Constructor) == null || (node.Target.NodeType != NodeType.SelfLiteralExpression && node.Target.NodeType != NodeType.SuperLiteralExpression) || !(node.Target.Entity is IConstructor))
			{
				ExpandCallableInvocation(node);
			}
		}

		private void ExpandCallableInvocation(MethodInvocationExpression node)
		{
			MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, RuntimeServices_InvokeCallable, node.Target, base.CodeBuilder.CreateObjectArray(node.Arguments));
			Replace(node2);
		}

		public override void LeaveSlicingExpression(SlicingExpression node)
		{
			if (IsDuckTyped(node.Target) && !node.IsTargetOfAssignment())
			{
				MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, RuntimeServices_GetSlice, GetSlicingTarget(node), base.CodeBuilder.CreateStringLiteral(GetSlicingMemberName(node)), GetArrayForIndices(node));
				Replace(node2);
			}
		}

		private bool IsDuckTyped(Expression e)
		{
			return base.TypeSystemServices.IsDuckTyped(e);
		}

		private static string GetSlicingMemberName(SlicingExpression node)
		{
			if (NodeType.MemberReferenceExpression == node.Target.NodeType)
			{
				MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)node.Target;
				return memberReferenceExpression.Name;
			}
			return "";
		}

		private static Expression GetSlicingTarget(SlicingExpression node)
		{
			Expression target = node.Target;
			if (NodeType.MemberReferenceExpression == target.NodeType)
			{
				MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)target;
				return memberReferenceExpression.Target;
			}
			return target;
		}

		private ArrayLiteralExpression GetArrayForIndices(SlicingExpression node)
		{
			ArrayLiteralExpression arrayLiteralExpression = new ArrayLiteralExpression();
			foreach (Slice index in node.Indices)
			{
				arrayLiteralExpression.Items.Add(index.Begin);
			}
			BindExpressionType(arrayLiteralExpression, base.TypeSystemServices.ObjectArrayType);
			return arrayLiteralExpression;
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			if (IsDuckTyped(node.Operand) && node.Operator == UnaryOperatorType.UnaryNegation)
			{
				MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, RuntimeServices_InvokeUnaryOperator, base.CodeBuilder.CreateStringLiteral(AstUtil.GetMethodNameForOperator(node.Operator)), node.Operand);
				Replace(node2);
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (BinaryOperatorType.Assign == node.Operator)
			{
				ProcessAssignment(node);
			}
			else if (AstUtil.IsOverloadableOperator(node.Operator) && (IsDuckTyped(node.Left) || IsDuckTyped(node.Right)))
			{
				MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, RuntimeServices_InvokeBinaryOperator, base.CodeBuilder.CreateStringLiteral(AstUtil.GetMethodNameForOperator(node.Operator)), node.Left, node.Right);
				Replace(node2);
			}
		}

		private void ProcessAssignment(BinaryExpression node)
		{
			SlicingExpression slicingExpression = node.Left as SlicingExpression;
			if (slicingExpression != null)
			{
				if (IsDuckTyped(slicingExpression.Target))
				{
					ProcessDuckSlicingPropertySet(node);
				}
			}
			else if (base.TypeSystemServices.IsQuackBuiltin(node.Left))
			{
				ProcessQuackPropertySet(node);
			}
		}

		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			if (base.TypeSystemServices.IsQuackBuiltin(node) && !node.IsTargetOfAssignment() && !AstUtil.IsTargetOfSlicing(node))
			{
				MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, GetGetPropertyMethod(), node.Target, base.CodeBuilder.CreateStringLiteral(node.Name));
				Replace(node2);
			}
		}

		private void ProcessDuckSlicingPropertySet(BinaryExpression node)
		{
			SlicingExpression node2 = (SlicingExpression)node.Left;
			if (node2.IsComplexSlicing())
			{
				throw CompilerErrorFactory.NotImplemented(node2, "complex slicing for duck");
			}
			ArrayLiteralExpression arrayForIndices = GetArrayForIndices(node2);
			arrayForIndices.Items.Add(node.Right);
			MethodInvocationExpression node3 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, RuntimeServices_SetSlice, GetSlicingTarget(node2), base.CodeBuilder.CreateStringLiteral(GetSlicingMemberName(node2)), arrayForIndices);
			Replace(node3);
		}

		private void ProcessQuackPropertySet(BinaryExpression node)
		{
			MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)node.Left;
			MethodInvocationExpression node2 = base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, GetSetPropertyMethod(), memberReferenceExpression.Target, base.CodeBuilder.CreateStringLiteral(memberReferenceExpression.Name), node.Right);
			Replace(node2);
		}

		protected virtual void ExpandQuackInvocation(MethodInvocationExpression node)
		{
			ExpandQuackInvocation(node, RuntimeServices_Invoke);
		}

		protected virtual void ExpandQuackInvocation(MethodInvocationExpression node, IMethod runtimeInvoke)
		{
			Visit(node.Arguments);
			Visit(node.NamedArguments);
			MemberReferenceExpression memberReferenceExpression = node.Target as MemberReferenceExpression;
			if (memberReferenceExpression != null)
			{
				ExpandMemberInvocation(node, memberReferenceExpression, runtimeInvoke);
			}
		}

		private void ExpandMemberInvocation(MethodInvocationExpression node, MemberReferenceExpression target, IMethod runtimeInvoke)
		{
			target.Target = (Expression)VisitNode(target.Target);
			node.Target = base.CodeBuilder.CreateMemberReference(runtimeInvoke);
			Expression item = base.CodeBuilder.CreateObjectArray(node.Arguments);
			node.Arguments.Clear();
			node.Arguments.Add(target.Target);
			node.Arguments.Add(base.CodeBuilder.CreateStringLiteral(target.Name));
			node.Arguments.Add(item);
		}

		private void BindDuck(Expression node)
		{
			BindExpressionType(node, base.TypeSystemServices.DuckType);
		}

		private void Replace(Expression node)
		{
			BindDuck(node);
			ReplaceCurrentNode(node);
		}
	}
}
