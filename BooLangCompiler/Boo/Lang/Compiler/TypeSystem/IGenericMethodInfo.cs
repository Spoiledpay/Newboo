namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IGenericMethodInfo : IGenericParametersProvider
	{
		IMethod ConstructMethod(params IType[] arguments);
	}
}
