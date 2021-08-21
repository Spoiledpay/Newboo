namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public interface IGenericMappedMember : IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		IMember SourceMember { get; }
	}
}
