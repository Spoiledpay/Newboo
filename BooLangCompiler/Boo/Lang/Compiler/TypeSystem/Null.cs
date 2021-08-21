using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class Null : AbstractType
	{
		public static readonly Null Default = new Null();

		public override string Name => "null";

		public override EntityType EntityType => EntityType.Null;

		private Null()
		{
		}

		public override IArrayType MakeArrayType(int rank)
		{
			return My<TypeSystemServices>.Instance.ObjectType.MakeArrayType(rank);
		}
	}
}
