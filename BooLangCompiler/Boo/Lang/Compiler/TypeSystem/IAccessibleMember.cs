namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IAccessibleMember : IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		bool IsProtected { get; }

		bool IsInternal { get; }

		bool IsPrivate { get; }
	}
}
