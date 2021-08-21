namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IExtensionEnabled : IEntityWithParameters, IEntity
	{
		bool IsExtension { get; }
	}
}
