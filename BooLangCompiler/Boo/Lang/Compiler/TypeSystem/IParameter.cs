namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IParameter : ITypedEntity, IEntity
	{
		bool IsByRef { get; }
	}
}
