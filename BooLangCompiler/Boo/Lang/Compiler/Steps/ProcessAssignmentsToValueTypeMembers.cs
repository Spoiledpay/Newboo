using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessAssignmentsToValueTypeMembers : ProcessAssignmentsToSpecialMembers
	{
		protected override bool IsSpecialMemberTarget(Expression container)
		{
			if (container.NodeType == NodeType.UnaryExpression)
			{
				UnaryExpression unaryExpression = container as UnaryExpression;
				if (unaryExpression.Operator == UnaryOperatorType.Indirection)
				{
					return false;
				}
			}
			return container.ExpressionType != null && container.ExpressionType.IsValueType;
		}
	}
}
