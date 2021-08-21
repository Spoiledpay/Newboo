namespace Boo.Lang.Compiler.TypeSystem
{
	public interface ICallableType : IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		bool IsAnonymous { get; }

		CallableSignature GetSignature();
	}
}
