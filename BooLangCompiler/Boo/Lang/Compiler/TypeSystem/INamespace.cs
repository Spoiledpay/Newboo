using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem
{
	public interface INamespace : IEntity
	{
		INamespace ParentNamespace { get; }

		bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider);

		IEnumerable<IEntity> GetMembers();
	}
}
