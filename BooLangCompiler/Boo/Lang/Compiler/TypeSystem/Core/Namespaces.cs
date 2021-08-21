using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public static class Namespaces
	{
		public static bool ResolveCoalescingNamespaces(INamespace parent, IEnumerable<INamespace> namespacesToResolveAgainst, string name, EntityType typesToConsider, ICollection<IEntity> resultingSet)
		{
			bool flag = false;
			Set<IEntity> set = new Set<IEntity>();
			foreach (INamespace item in namespacesToResolveAgainst)
			{
				if (item.Resolve(set, name, typesToConsider))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			return CoalesceResolved(set, parent, name, resultingSet);
		}

		public static bool ResolveCoalescingNamespaces(INamespace parent, INamespace namespaceToResolveAgainst, string name, EntityType typesToConsider, ICollection<IEntity> resultingSet)
		{
			Set<IEntity> set = new Set<IEntity>();
			if (!namespaceToResolveAgainst.Resolve(set, name, typesToConsider))
			{
				return false;
			}
			return CoalesceResolved(set, parent, name, resultingSet);
		}

		private static bool CoalesceResolved(IEnumerable<IEntity> resolved, INamespace parent, string name, ICollection<IEntity> resultingSet)
		{
			List<INamespace> list = new List<INamespace>();
			foreach (IEntity item in resolved)
			{
				if (item.EntityType == EntityType.Namespace)
				{
					list.Add((INamespace)item);
				}
				else
				{
					resultingSet.Add(item);
				}
			}
			INamespace @namespace = CoalescedNamespaceFor(parent, name, list);
			if (@namespace != null)
			{
				resultingSet.Add(@namespace);
			}
			return true;
		}

		public static INamespace CoalescedNamespaceFor(INamespace parent, string name, List<INamespace> namespaces)
		{
			return namespaces.Count switch
			{
				0 => null, 
				1 => namespaces.First(), 
				_ => new ResolvedNamespaces(name, parent, namespaces.ToArray()), 
			};
		}
	}
}
