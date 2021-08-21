namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IEvent : IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		bool IsAbstract { get; }

		bool IsVirtual { get; }

		IMethod GetAddMethod();

		IMethod GetRemoveMethod();

		IMethod GetRaiseMethod();
	}
}
