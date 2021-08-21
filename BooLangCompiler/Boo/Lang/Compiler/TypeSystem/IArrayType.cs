namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IArrayType : IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		int Rank { get; }
	}
}
