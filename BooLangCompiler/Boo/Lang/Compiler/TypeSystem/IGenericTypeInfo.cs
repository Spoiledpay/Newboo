namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IGenericTypeInfo : IGenericParametersProvider
	{
		IType ConstructType(params IType[] arguments);
	}
}
