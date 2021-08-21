namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IConstructedMethodInfo : IGenericArgumentsProvider
	{
		IMethod GenericDefinition { get; }

		bool FullyConstructed { get; }
	}
}
