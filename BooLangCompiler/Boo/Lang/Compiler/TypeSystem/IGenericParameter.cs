namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IGenericParameter : IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		int GenericParameterPosition { get; }

		bool MustHaveDefaultConstructor { get; }

		Variance Variance { get; }

		IType[] GetTypeConstraints();
	}
}
