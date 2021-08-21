using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class CompileUnitNamespace : AbstractNamespace
	{
		private readonly CompileUnit _compileUnit;

		private readonly NameResolutionService _nameResolutionService;

		private readonly InternalTypeSystemProvider _internalTypeSystemProvider;

		public override INamespace ParentNamespace => _nameResolutionService.GlobalNamespace;

		public CompileUnitNamespace(CompileUnit unit)
		{
			_nameResolutionService = My<NameResolutionService>.Instance;
			_internalTypeSystemProvider = My<InternalTypeSystemProvider>.Instance;
			_compileUnit = unit;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool result = false;
			foreach (InternalModule item in InternalModules())
			{
				if (string.IsNullOrEmpty(item.Namespace))
				{
					if (item.Resolve(resultingSet, name, typesToConsider))
					{
						result = true;
					}
				}
				else if (HasNamespacePrefix(item, name))
				{
					if (item.Namespace.Length == name.Length)
					{
						resultingSet.Add(item.ModuleMembersNamespace);
						result = true;
					}
					else if (item.Namespace[name.Length] == '.')
					{
						resultingSet.Add(new PartialModuleNamespace(name, item));
						result = true;
					}
				}
			}
			return result;
		}

		private static bool HasNamespacePrefix(InternalModule module, string @namespace)
		{
			string namespace2 = module.Namespace;
			return namespace2.StartsWith(@namespace);
		}

		private IEnumerable<InternalModule> InternalModules()
		{
			foreach (Module i in _compileUnit.Modules)
			{
				if (i.Entity != null)
				{
					yield return (InternalModule)_internalTypeSystemProvider.EntityFor(i);
				}
			}
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			yield break;
		}
	}
}
