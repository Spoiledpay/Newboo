namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IProperty : IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity
	{
		IMethod GetGetMethod();

		IMethod GetSetMethod();
	}
}
