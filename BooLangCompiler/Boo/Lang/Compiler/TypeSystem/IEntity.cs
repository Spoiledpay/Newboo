namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IEntity
	{
		string Name { get; }

		string FullName { get; }

		EntityType EntityType { get; }
	}
}
