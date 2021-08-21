namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public abstract class AbstractLocalEntity
	{
		public bool IsUsed { get; set; }

		public bool IsShared { get; set; }
	}
}
