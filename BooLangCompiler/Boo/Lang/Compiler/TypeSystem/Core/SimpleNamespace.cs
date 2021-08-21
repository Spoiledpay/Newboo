using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class SimpleNamespace : AbstractNamespace
	{
		protected INamespace _parent;

		protected IEnumerable<IEntity> _members;

		protected IDictionary<string, string> _aliases;

		public override INamespace ParentNamespace => _parent;

		public SimpleNamespace(INamespace parent, IEnumerable<IEntity> members)
		{
			_parent = parent;
			_members = members;
		}

		public SimpleNamespace(INamespace parent, IEnumerable<IEntity> members, IDictionary<string, string> aliases)
			: this(parent, members)
		{
			_aliases = aliases;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			return _members;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (_aliases != null && _aliases.Count > 0)
			{
				if (!_aliases.ContainsKey(name))
				{
					return false;
				}
				name = _aliases[name];
			}
			return base.Resolve(resultingSet, name, typesToConsider);
		}
	}
}
