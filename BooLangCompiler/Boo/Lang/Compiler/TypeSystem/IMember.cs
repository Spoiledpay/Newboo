namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IMember : ITypedEntity, IEntity, IEntityWithAttributes
	{
		bool IsDuckTyped { get; }

		IType DeclaringType { get; }

		bool IsStatic { get; }

		bool IsPublic { get; }
	}
}
