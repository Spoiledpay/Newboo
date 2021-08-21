using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class Unknown : AbstractType
	{
		public static Unknown Default = new Unknown();

		public override string Name => "unknown";

		public override EntityType EntityType => EntityType.Unknown;

		private Unknown()
		{
		}
	}
}
