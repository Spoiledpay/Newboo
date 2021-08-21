using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class Error : AbstractType
	{
		public static Error Default = new Error();

		public override string Name => "error";

		public override EntityType EntityType => EntityType.Error;

		private Error()
		{
		}
	}
}
