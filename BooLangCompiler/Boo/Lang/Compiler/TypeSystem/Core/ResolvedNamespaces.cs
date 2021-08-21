using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class ResolvedNamespaces : NamespaceDelegator
	{
		private readonly string _name;

		public override string Name => _name;

		public ResolvedNamespaces(string name, INamespace parent, params INamespace[] namespaces)
			: base(parent, namespaces)
		{
			_name = name;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return Namespaces.ResolveCoalescingNamespaces(this, base.Delegates, name, typesToConsider, resultingSet);
		}
	}
}
