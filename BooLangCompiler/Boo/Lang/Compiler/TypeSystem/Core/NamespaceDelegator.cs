using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class NamespaceDelegator : AbstractNamespace
	{
		private INamespace _parent;

		private List<INamespace> _namespaces = new List<INamespace>();

		public override INamespace ParentNamespace => _parent;

		protected IEnumerable<INamespace> Delegates => _namespaces;

		public NamespaceDelegator(INamespace parent, params INamespace[] namespaces)
		{
			_parent = parent;
			_namespaces.ExtendUnique(namespaces);
		}

		public void DelegateTo(INamespace ns)
		{
			_namespaces.AddUnique(ns);
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool result = false;
			foreach (INamespace @delegate in Delegates)
			{
				if (@delegate.Resolve(resultingSet, name, typesToConsider))
				{
					result = true;
				}
			}
			return result;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			foreach (INamespace ns in _namespaces)
			{
				foreach (IEntity member in ns.GetMembers())
				{
					yield return member;
				}
			}
		}
	}
}
