namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IGenericArgumentsProvider
	{
		IType[] GenericArguments { get; }
	}
}
