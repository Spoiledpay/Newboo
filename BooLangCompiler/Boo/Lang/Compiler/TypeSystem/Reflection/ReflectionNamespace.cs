using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	internal class ReflectionNamespace : AbstractNamespace
	{
		private sealed class ChildReflectionNamespace : ReflectionNamespace
		{
			private readonly ReflectionNamespace _parent;

			private string _name;

			public override string Name => _name;

			public override INamespace ParentNamespace => _parent;

			public ChildReflectionNamespace(ReflectionNamespace parent, string name)
				: base(parent._provider)
			{
				_parent = parent;
				_name = name;
			}

			public override string ToString()
			{
				return FullName;
			}
		}

		private readonly MemoizedFunction<string, ReflectionNamespace> _childNamespaces;

		private readonly MemoizedFunction<string, List<Type>> _typeLists;

		private List<INamespace> _modules;

		private readonly IReflectionTypeSystemProvider _provider;

		public ReflectionNamespace(IReflectionTypeSystemProvider provider)
		{
			_childNamespaces = new MemoizedFunction<string, ReflectionNamespace>(StringComparer.Ordinal, CreateChildNamespace);
			_typeLists = new MemoizedFunction<string, List<Type>>(StringComparer.Ordinal, NewTypeList);
			_provider = provider;
		}

		public ReflectionNamespace Produce(string name)
		{
			return _childNamespaces.Invoke(name);
		}

		private ReflectionNamespace CreateChildNamespace(string name)
		{
			return new ChildReflectionNamespace(this, name);
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (ResolveChildNamespace(resultingSet, name, typesToConsider))
			{
				return true;
			}
			if (ResolveType(resultingSet, name, typesToConsider))
			{
				return true;
			}
			return ResolveModules(resultingSet, name, typesToConsider);
		}

		private bool ResolveModules(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (_modules == null)
			{
				return false;
			}
			bool result = false;
			foreach (INamespace module in _modules)
			{
				if (module.Resolve(resultingSet, name, typesToConsider))
				{
					result = true;
				}
			}
			return result;
		}

		private bool ResolveType(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (Entities.IsFlagSet(typesToConsider, EntityType.Type) && _typeLists.TryGetValue(name, out var result))
			{
				foreach (IEntity item in EntitiesFor(result))
				{
					resultingSet.Add(item);
				}
				return true;
			}
			return false;
		}

		private bool ResolveChildNamespace(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (Entities.IsFlagSet(typesToConsider, EntityType.Namespace) && _childNamespaces.TryGetValue(name, out var result))
			{
				resultingSet.Add(result);
				return true;
			}
			return false;
		}

		private IEnumerable EntitiesFor(List<Type> types)
		{
			foreach (Type type in types)
			{
				yield return _provider.Map(type);
			}
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			foreach (ReflectionNamespace value in _childNamespaces.Values)
			{
				yield return value;
			}
			foreach (List<Type> typeList in _typeLists.Values)
			{
				foreach (Type type in typeList)
				{
					yield return _provider.Map(type);
				}
			}
			if (null == _modules)
			{
				yield break;
			}
			foreach (INamespace @namespace in _modules)
			{
				foreach (IEntity member in @namespace.GetMembers())
				{
					yield return member;
				}
			}
		}

		public void Add(Type type)
		{
			string name = TypeUtilities.TypeName(type);
			TypeListFor(name).Add(type);
			if (IsModule(type))
			{
				AddModule(type);
			}
		}

		private void AddModule(Type type)
		{
			if (_modules == null)
			{
				_modules = new List<INamespace>();
			}
			_modules.Add(_provider.Map(type));
		}

		private List<Type> TypeListFor(string name)
		{
			return _typeLists.Invoke(name);
		}

		private static bool IsModule(Type type)
		{
			return type.IsClass && type.IsSealed && !type.IsNestedPublic && HasModuleMarkerAttribute(type);
		}

		private static bool HasModuleMarkerAttribute(Type type)
		{
			return MetadataUtil.IsAttributeDefined(type, Types.ModuleAttribute) || MetadataUtil.IsAttributeDefined(type, Types.ClrExtensionAttribute);
		}

		private static List<Type> NewTypeList(string name)
		{
			return new List<Type>();
		}
	}
}
