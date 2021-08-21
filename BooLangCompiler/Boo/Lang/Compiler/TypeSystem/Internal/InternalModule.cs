using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalModule : INamespace, IEntity
	{
		private readonly InternalTypeSystemProvider _provider;

		private readonly Module _module;

		private ClassDefinition _moduleClass;

		private INamespace _moduleClassNamespace = NullNamespace.Default;

		private INamespace _moduleAsNamespace;

		private readonly string _namespace;

		public EntityType EntityType => EntityType.Namespace;

		public string Name => _module.Name;

		public string FullName => _module.FullName;

		public string Namespace => _namespace;

		public ClassDefinition ModuleClass => _moduleClass;

		public INamespace ModuleMembersNamespace
		{
			get
			{
				if (_moduleAsNamespace != null)
				{
					return _moduleAsNamespace;
				}
				return _moduleAsNamespace = new ModuleMembersNamespace(this);
			}
		}

		public INamespace ParentNamespace => _provider.EntityFor((CompileUnit)_module.ParentNode).RootNamespace.ParentNamespace;

		public IEnumerable<Import> Imports => _module.Imports;

		public InternalModule(InternalTypeSystemProvider provider, Module module)
		{
			_provider = provider;
			_module = module;
			_namespace = SafeNamespace(module);
		}

		public static string SafeNamespace(Module module)
		{
			return (module.Namespace == null) ? string.Empty : module.Namespace.Name;
		}

		public void InitializeModuleClass(ClassDefinition moduleClass)
		{
			_moduleClassNamespace = (INamespace)_provider.EntityFor(moduleClass);
			_moduleClass = moduleClass;
		}

		public bool ResolveMember(ICollection<IEntity> targetList, string name, EntityType flags)
		{
			if (ResolveModuleMember(targetList, name, flags))
			{
				return true;
			}
			return _moduleClassNamespace.Resolve(targetList, name, flags);
		}

		public bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (ResolveMember(resultingSet, name, typesToConsider))
			{
				return true;
			}
			bool result = false;
			foreach (INamespace item in ImportedNamespaces())
			{
				if (item.Resolve(resultingSet, name, typesToConsider))
				{
					result = true;
				}
			}
			return result;
		}

		private IEnumerable<INamespace> ImportedNamespaces()
		{
			return _module.Imports.Select((Import i) => i.Entity).OfType<INamespace>();
		}

		private bool ResolveModuleMember(ICollection<IEntity> targetList, string name, EntityType flags)
		{
			bool result = false;
			foreach (TypeMember member in _module.Members)
			{
				if (!(name != member.Name))
				{
					IEntity entity = _provider.EntityFor(member);
					if (Entities.IsFlagSet(flags, entity.EntityType))
					{
						targetList.Add(entity);
						result = true;
					}
				}
			}
			return result;
		}

		public IEnumerable<IEntity> GetMembers()
		{
			yield break;
		}

		public static INamespace ScopeFor(Module module)
		{
			return (InternalModule)TypeSystemServices.GetEntity(module);
		}
	}
}
