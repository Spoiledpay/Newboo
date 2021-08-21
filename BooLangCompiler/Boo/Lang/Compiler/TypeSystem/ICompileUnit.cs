namespace Boo.Lang.Compiler.TypeSystem
{
	public interface ICompileUnit : IEntity
	{
		INamespace RootNamespace { get; }
	}
}
