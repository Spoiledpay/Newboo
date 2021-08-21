using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Generics;

namespace Boo.Lang.Compiler.Steps
{
	public class AccessibilityChecker : IAccessibilityChecker
	{
		public class GlobalAccessibilityChecker : IAccessibilityChecker
		{
			public bool IsAccessible(IAccessibleMember member)
			{
				if (member.IsPublic)
				{
					return true;
				}
				return member.IsInternal && member is IInternalEntity;
			}
		}

		public static readonly IAccessibilityChecker Global = new GlobalAccessibilityChecker();

		private readonly TypeDefinition _scope;

		public AccessibilityChecker(TypeDefinition scope)
		{
			_scope = scope;
		}

		public bool IsAccessible(IAccessibleMember member)
		{
			if (member.IsPublic)
			{
				return true;
			}
			IInternalEntity internalEntity = GetInternalEntity(member);
			if (null != internalEntity)
			{
				internalEntity.Node.RemoveAnnotation("PrivateMemberNeverUsed");
				if (member.IsInternal)
				{
					return true;
				}
			}
			IType declaringType = member.DeclaringType;
			if (declaringType == CurrentType())
			{
				return true;
			}
			if (member.IsProtected && CurrentType().IsSubclassOf(declaringType))
			{
				return true;
			}
			return IsDeclaredInside(declaringType);
		}

		private static IInternalEntity GetInternalEntity(IAccessibleMember member)
		{
			if (member is IInternalEntity)
			{
				return (IInternalEntity)member;
			}
			IGenericMappedMember genericMappedMember = member as IGenericMappedMember;
			if (genericMappedMember != null && genericMappedMember.SourceMember is IInternalEntity)
			{
				return (IInternalEntity)genericMappedMember.SourceMember;
			}
			return null;
		}

		private IType CurrentType()
		{
			return (IType)_scope.Entity;
		}

		private bool IsDeclaredInside(IType candidate)
		{
			IInternalEntity internalEntity = candidate as IInternalEntity;
			if (null == internalEntity)
			{
				return false;
			}
			for (TypeDefinition declaringType = _scope.DeclaringType; declaringType != null; declaringType = declaringType.DeclaringType)
			{
				if (declaringType == internalEntity.Node)
				{
					return true;
				}
			}
			return false;
		}
	}
}
