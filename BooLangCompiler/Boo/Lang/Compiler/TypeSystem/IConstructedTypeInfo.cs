namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IConstructedTypeInfo : IGenericArgumentsProvider
	{
		IType GenericDefinition { get; }

		bool FullyConstructed { get; }

		IType Map(IType type);

		IMember Map(IMember source);

		IMember UnMap(IMember mapped);
	}
}
