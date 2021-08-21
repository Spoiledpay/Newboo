namespace Boo.Lang.Compiler.TypeSystem
{
	public interface ILocalEntity : ITypedEntity, IEntity
	{
		bool IsPrivateScope { get; }

		bool IsShared { get; set; }

		bool IsUsed { get; set; }
	}
}
