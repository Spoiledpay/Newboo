using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class ExpandPropertiesAndEvents : AbstractTransformerCompilerStep
	{
		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			IProperty property = node.Entity as IProperty;
			if (property != null && !node.IsTargetOfAssignment())
			{
				MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreatePropertyGet(node.Target, property);
				if (property.IsDuckTyped)
				{
					ReplaceCurrentNode(base.CodeBuilder.CreateCast(base.TypeSystemServices.DuckType, methodInvocationExpression));
				}
				else
				{
					ReplaceCurrentNode(methodInvocationExpression);
				}
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			IEvent @event = node.Left.Entity as IEvent;
			if (@event != null)
			{
				IMethod method;
				if (node.Operator == BinaryOperatorType.InPlaceAddition)
				{
					method = @event.GetAddMethod();
				}
				else
				{
					method = @event.GetRemoveMethod();
					CheckEventUnsubscribe(node, @event);
				}
				ReplaceCurrentNode(MethodInvocationForEventSubscription(node, method));
			}
		}

		private MethodInvocationExpression MethodInvocationForEventSubscription(BinaryExpression node, IMethod method)
		{
			Expression target = ((MemberReferenceExpression)node.Left).Target;
			return base.CodeBuilder.CreateMethodInvocation(node.LexicalInfo, target, method, node.Right);
		}

		private void CheckEventUnsubscribe(BinaryExpression node, IEvent eventInfo)
		{
			CallableSignature signature = ((ICallableType)eventInfo.Type).GetSignature();
			CallableSignature callableSignature = GetCallableSignature(node.Right);
			if (signature != callableSignature)
			{
				base.Warnings.Add(CompilerWarningFactory.InvalidEventUnsubscribe(node, eventInfo, signature));
			}
		}

		private CallableSignature GetCallableSignature(Expression node)
		{
			return ((ICallableType)GetExpressionType(node)).GetSignature();
		}
	}
}
