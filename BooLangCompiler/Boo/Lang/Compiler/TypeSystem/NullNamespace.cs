using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class NullNamespace : AbstractNamespace
	{
		public static readonly INamespace Default = new NullNamespace();

		public static readonly IEntity[] EmptyEntityArray = new IEntity[0];

		private NullNamespace()
		{
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType flags)
		{
			return false;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			return EmptyEntityArray;
		}
	}
}
