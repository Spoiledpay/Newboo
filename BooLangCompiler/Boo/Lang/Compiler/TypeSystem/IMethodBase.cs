namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IMethodBase : IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IEntityWithParameters, IEntity
	{
		ICallableType CallableType { get; }

		IType ReturnType { get; }
	}
}
