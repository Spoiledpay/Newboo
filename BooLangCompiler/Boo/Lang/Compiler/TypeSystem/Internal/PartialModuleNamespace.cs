using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class PartialModuleNamespace : AbstractNamespace
	{
		private InternalModule _module;

		private string _name;

		public override INamespace ParentNamespace => _module.ParentNamespace;

		public override string Name => _name;

		public PartialModuleNamespace(string name, InternalModule m)
		{
			_name = name;
			_module = m;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			string @namespace = _module.Namespace;
			string text = _name + "." + name;
			if (@namespace.StartsWith(text))
			{
				if (@namespace.Length == text.Length)
				{
					resultingSet.Add(new NamespaceDelegator(this, _module.ModuleMembersNamespace));
					return true;
				}
				if (@namespace[text.Length] == '.')
				{
					resultingSet.Add(new PartialModuleNamespaceMember(this, text, name, _module));
					return true;
				}
			}
			return false;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			yield break;
		}
	}
}
