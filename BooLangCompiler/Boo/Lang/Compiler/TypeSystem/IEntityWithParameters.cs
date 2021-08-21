namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IEntityWithParameters : IEntity
	{
		bool AcceptVarArgs { get; }

		IParameter[] GetParameters();
	}
}
