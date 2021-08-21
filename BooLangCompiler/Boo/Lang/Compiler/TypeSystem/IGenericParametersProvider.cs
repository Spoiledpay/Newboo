namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IGenericParametersProvider
	{
		IGenericParameter[] GenericParameters { get; }
	}
}
