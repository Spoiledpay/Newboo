using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckMembersProtectionLevel : AbstractFastVisitorCompilerStep
	{
		private IAccessibilityChecker _checker = AccessibilityChecker.Global;

		public override void OnClassDefinition(ClassDefinition node)
		{
			IAccessibilityChecker checker = _checker;
			_checker = new AccessibilityChecker(node);
			base.OnClassDefinition(node);
			_checker = checker;
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.OnMemberReferenceExpression(node);
			OnReferenceExpression(node);
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			IAccessibleMember accessibleMember = node.Entity as IAccessibleMember;
			if (null == accessibleMember)
			{
				return;
			}
			if (!IsAccessible(accessibleMember))
			{
				Error(CompilerErrorFactory.UnaccessibleMember(node, accessibleMember));
				return;
			}
			IProperty property = accessibleMember as IProperty;
			if (null != property)
			{
				accessibleMember = (node.IsTargetOfAssignment() ? property.GetSetMethod() : property.GetGetMethod());
				if (!IsAccessible(accessibleMember))
				{
					Error(CompilerErrorFactory.UnaccessibleMember(node, accessibleMember));
				}
			}
		}

		private bool IsAccessible(IAccessibleMember member)
		{
			return _checker.IsAccessible(member);
		}
	}
}
