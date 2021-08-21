namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IMethod : IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity, IOverridableMember
	{
		bool IsAbstract { get; }

		bool IsVirtual { get; }

		bool IsSpecialName { get; }

		bool IsPInvoke { get; }

		IConstructedMethodInfo ConstructedInfo { get; }

		IGenericMethodInfo GenericInfo { get; }
	}
}
