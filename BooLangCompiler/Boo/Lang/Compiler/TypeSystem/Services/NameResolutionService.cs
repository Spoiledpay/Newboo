using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class NameResolutionService
	{
		public static readonly char[] DotArray = new char[1] { '.' };

		protected INamespace _global;

		private EntityNameMatcher _entityNameMatcher = Matches;

		private readonly CurrentScope _current = My<CurrentScope>.Instance;

		private readonly MemoizedFunction<string, IType, IEntity> _resolveExtensionFor;

		private readonly MemoizedFunction<string, EntityType, IEntity> _resolveName;

		public EntityNameMatcher EntityNameMatcher
		{
			get
			{
				return _entityNameMatcher;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException();
				}
				_entityNameMatcher = value;
			}
		}

		public INamespace GlobalNamespace
		{
			get
			{
				return _global;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("GlobalNamespace");
				}
				_global = value;
			}
		}

		public INamespace CurrentNamespace
		{
			get
			{
				return _current.Value;
			}
			private set
			{
				_current.Value = value;
			}
		}

		public NameResolutionService()
		{
			_resolveExtensionFor = new MemoizedFunction<string, IType, IEntity>(StringComparer.Ordinal, ResolveExtensionFor);
			_resolveName = new MemoizedFunction<string, EntityType, IEntity>(StringComparer.Ordinal, ResolveImpl);
			CurrentScope current = _current;
			EventHandler value = delegate
			{
				ClearResolutionCache();
			};
			current.Changed += value;
			_global = My<Boo.Lang.Compiler.TypeSystem.Core.GlobalNamespace>.Instance;
			Reset();
		}

		public void EnterNamespace(INamespace ns)
		{
			if (null == ns)
			{
				throw new ArgumentNullException("ns");
			}
			CurrentNamespace = ns;
		}

		public void Reset()
		{
			EnterNamespace(_global);
		}

		public void LeaveNamespace()
		{
			CurrentNamespace = CurrentNamespace.ParentNamespace;
		}

		public IEntity Resolve(string name)
		{
			return Resolve(name, EntityType.Any);
		}

		public IEntity Resolve(string name, EntityType flags)
		{
			return _resolveName.Invoke(name, flags);
		}

		private IEntity ResolveImpl(string name, EntityType flags)
		{
			Set<IEntity> set = new Set<IEntity>();
			Resolve(set, name, flags);
			return Entities.EntityFromList(set);
		}

		public void ClearResolutionCacheFor(string name)
		{
			_resolveName.Clear(name);
		}

		private void ClearResolutionCache()
		{
			_resolveName.Clear();
			_resolveExtensionFor.Clear();
		}

		public IEnumerable<TEntityOut> Select<TEntityOut>(IEnumerable<IEntity> candidates, string name, EntityType typesToConsider)
		{
			foreach (IEntity candidate in candidates)
			{
				if (Matches(candidate, name, typesToConsider))
				{
					yield return (TEntityOut)candidate;
				}
			}
		}

		public bool Resolve(string name, IEnumerable<IEntity> candidates, EntityType typesToConsider, ICollection<IEntity> resolvedSet)
		{
			bool result = false;
			foreach (IEntity item in Select<IEntity>(candidates, name, typesToConsider))
			{
				resolvedSet.Add(item);
				result = true;
			}
			return result;
		}

		private bool Matches(IEntity entity, string name, EntityType typesToConsider)
		{
			return Entities.IsFlagSet(typesToConsider, entity.EntityType) && _entityNameMatcher(entity, name);
		}

		private static bool Matches(IEntity entity, string name)
		{
			return entity.Name == name;
		}

		private void Resolve(ICollection<IEntity> targetList, string name, EntityType flags)
		{
			IEntity entity = My<TypeSystemServices>.Instance.ResolvePrimitive(name);
			if (entity != null)
			{
				targetList.Add(entity);
				return;
			}
			AssertInNamespace();
			INamespace @namespace = CurrentNamespace;
			while (!Namespaces.ResolveCoalescingNamespaces(@namespace.ParentNamespace, @namespace, name, flags, targetList))
			{
				@namespace = @namespace.ParentNamespace;
				if (@namespace == null)
				{
					break;
				}
			}
		}

		public IEntity ResolveExtension(INamespace ns, string name)
		{
			IType type = ns as IType;
			if (null == type)
			{
				return null;
			}
			return _resolveExtensionFor.Invoke(name, type);
		}

		private IEntity ResolveExtensionFor(string name, IType type)
		{
			INamespace @namespace = CurrentNamespace;
			while (null != @namespace)
			{
				IEntity entity = ResolveExtensionForType(@namespace, type, name);
				if (null != entity)
				{
					return entity;
				}
				@namespace = @namespace.ParentNamespace;
			}
			return null;
		}

		private IEntity ResolveExtensionForType(INamespace ns, IType type, string name)
		{
			Set<IEntity> set = new Set<IEntity>();
			if (!ns.Resolve(set, name, EntityType.Method | EntityType.Property))
			{
				return null;
			}
			Predicate<IEntity> predicate = (IEntity item) => !IsExtensionOf(type, item as IExtensionEnabled);
			set.RemoveAll(predicate);
			return Entities.EntityFromList(set);
		}

		private bool IsExtensionOf(IType type, IExtensionEnabled entity)
		{
			if (!(entity?.IsExtension ?? false))
			{
				return false;
			}
			IParameter[] parameters = entity.GetParameters();
			if (parameters.Length == 0)
			{
				return false;
			}
			IType type2 = parameters[0].Type;
			return IsValidExtensionType(type, type2, entity);
		}

		private bool IsValidExtensionType(IType actualType, IType extensionType, IExtensionEnabled extension)
		{
			if (TypeCompatibilityRules.IsAssignableFrom(extensionType, actualType))
			{
				return true;
			}
			IMethod method = extension as IMethod;
			if (method == null || method.GenericInfo == null)
			{
				return false;
			}
			System.Collections.Generic.List<IGenericParameter> list = new System.Collections.Generic.List<IGenericParameter>(GenericsServices.FindGenericParameters(extensionType));
			if (list.Count == 0)
			{
				return false;
			}
			TypeInferrer typeInferrer = new TypeInferrer(list);
			typeInferrer.Infer(extensionType, actualType);
			return typeInferrer.FinalizeInference();
		}

		public IEntity ResolveQualifiedName(string name)
		{
			return ResolveQualifiedName(name, EntityType.Any);
		}

		private IEntity ResolveQualifiedName(string name, EntityType flags)
		{
			if (!IsQualifiedName(name))
			{
				return Resolve(name, flags);
			}
			Set<IEntity> set = new Set<IEntity>();
			ResolveQualifiedName(set, name, flags);
			return Entities.EntityFromList(set);
		}

		private bool ResolveQualifiedName(ICollection<IEntity> targetList, string name, EntityType flags)
		{
			AssertInNamespace();
			INamespace @namespace = CurrentNamespace;
			do
			{
				if (ResolveQualifiedNameAgainst(@namespace, name, flags, targetList))
				{
					return true;
				}
				@namespace = @namespace.ParentNamespace;
			}
			while (@namespace != null);
			return false;
		}

		private bool ResolveQualifiedNameAgainst(INamespace current, string name, EntityType flags, ICollection<IEntity> resultingSet)
		{
			string[] array = name.Split(DotArray);
			for (int i = 0; i < array.Length - 1; i++)
			{
				current = Resolve(current, array[i], EntityType.Type | EntityType.Namespace) as INamespace;
				if (current == null)
				{
					return false;
				}
			}
			return ResolveCoalescingNamespaces(current, array[array.Length - 1], flags, resultingSet);
		}

		private void AssertInNamespace()
		{
			if (_current == null)
			{
				throw new InvalidOperationException("No namespace.");
			}
		}

		public void ResolveTypeReference(TypeReference node)
		{
			if (null == node.Entity)
			{
				switch (node.NodeType)
				{
				case NodeType.ArrayTypeReference:
					ResolveArrayTypeReference((ArrayTypeReference)node);
					break;
				case NodeType.CallableTypeReference:
					break;
				default:
					ResolveSimpleTypeReference((SimpleTypeReference)node);
					break;
				}
			}
		}

		public void ResolveArrayTypeReference(ArrayTypeReference node)
		{
			if (null == node.Entity)
			{
				ResolveTypeReference(node.ElementType);
				IType type = TypeSystemServices.GetType(node.ElementType);
				if (TypeSystemServices.IsError(type))
				{
					node.Entity = TypeSystemServices.ErrorEntity;
					return;
				}
				int rank = (int)((node.Rank == null) ? 1 : node.Rank.Value);
				node.Entity = type.MakeArrayType(rank);
			}
		}

		private void ResolveTypeReferenceCollection(TypeReferenceCollection collection)
		{
			foreach (TypeReference item in collection)
			{
				ResolveTypeReference(item);
			}
		}

		public void ResolveSimpleTypeReference(SimpleTypeReference node)
		{
			if (null != node.Entity)
			{
				return;
			}
			IEntity entity = ResolveQualifiedName(node.Name, EntityType.Type);
			if (entity == null)
			{
				node.Entity = NameNotType(node, null);
				return;
			}
			IEntity entity2 = null;
			if (entity.IsAmbiguous())
			{
				Set<IEntity> set = new Set<IEntity>(((Ambiguous)entity).Entities);
				entity2 = set.First();
				FilterGenericTypes(set, node);
				entity = Entities.EntityFromList(set);
			}
			if (NodeType.SimpleTypeReference == node.NodeType)
			{
				if (IsGenericType(entity))
				{
					entity2 = entity;
					entity = null;
				}
				if (entity == null)
				{
					GenericArgumentsCountMismatch(node, entity2 as IType);
					node.Entity = TypeSystemServices.ErrorEntity;
					return;
				}
			}
			if (NodeType.GenericTypeReference == node.NodeType)
			{
				GenericTypeReference gtr = node as GenericTypeReference;
				entity = ResolveGenericTypeReference(gtr, entity);
			}
			if (NodeType.GenericTypeDefinitionReference == node.NodeType)
			{
				GenericTypeDefinitionReference genericTypeDefinitionReference = node as GenericTypeDefinitionReference;
				IType type = (IType)entity;
				if (genericTypeDefinitionReference.GenericPlaceholders != type.GenericInfo.GenericParameters.Length)
				{
					GenericArgumentsCountMismatch(genericTypeDefinitionReference, type);
					node.Entity = TypeSystemServices.ErrorEntity;
					return;
				}
			}
			entity = Entities.PreferInternalEntitiesOverExternalOnes(entity);
			if (EntityType.Type != entity.EntityType)
			{
				if (entity.IsAmbiguous())
				{
					entity = AmbiguousReference(node, (Ambiguous)entity);
				}
				else if (EntityType.Error != entity.EntityType)
				{
					entity = NameNotType(node, entity);
				}
			}
			else
			{
				node.Name = entity.FullName;
			}
			if (node.IsPointer && EntityType.Type == entity.EntityType)
			{
				entity = ((IType)entity).MakePointerType();
			}
			node.Entity = entity;
		}

		internal IEntity ResolveTypeName(SimpleTypeReference node)
		{
			IEntity entity = ResolveQualifiedName(node.Name, EntityType.Type);
			if (entity == null)
			{
				return null;
			}
			if (entity.IsAmbiguous())
			{
				Set<IEntity> set = new Set<IEntity>(((Ambiguous)entity).Entities);
				FilterGenericTypes(set, node);
				return Entities.EntityFromList(set);
			}
			return entity;
		}

		public IEntity ResolveGenericTypeReference(GenericTypeReference gtr, IEntity definition)
		{
			ResolveTypeReferenceCollection(gtr.GenericArguments);
			IType[] typeArguments = gtr.GenericArguments.ToArray((TypeReference t) => TypeSystemServices.GetType(t));
			return My<GenericsServices>.Instance.ConstructEntity(gtr, definition, typeArguments);
		}

		public IEntity ResolveGenericReferenceExpression(GenericReferenceExpression gre, IEntity definition)
		{
			ResolveTypeReferenceCollection(gre.GenericArguments);
			IType[] typeArguments = gre.GenericArguments.ToArray((TypeReference t) => TypeSystemServices.GetType(t));
			return My<GenericsServices>.Instance.ConstructEntity(gre, definition, typeArguments);
		}

		private void FilterGenericTypes(Set<IEntity> types, SimpleTypeReference node)
		{
			if (node is GenericTypeReference || node is GenericTypeDefinitionReference)
			{
				types.RemoveAll(IsNotGenericType);
			}
			else
			{
				types.RemoveAll(IsGenericType);
			}
		}

		private bool IsGenericType(IEntity entity)
		{
			IType type = entity as IType;
			return type != null && type.GenericInfo != null;
		}

		private static bool IsNotGenericType(IEntity entity)
		{
			IType type = entity as IType;
			return type != null && type.GenericInfo == null;
		}

		private IEntity NameNotType(SimpleTypeReference node, IEntity actual)
		{
			string mostSimilarTypeName = GetMostSimilarTypeName(node.Name);
			CompilerErrors().Add(CompilerErrorFactory.NameNotType(node, node.Name, actual, mostSimilarTypeName));
			return TypeSystemServices.ErrorEntity;
		}

		private CompilerErrorCollection CompilerErrors()
		{
			return My<CompilerErrorCollection>.Instance;
		}

		private IEntity AmbiguousReference(SimpleTypeReference node, Ambiguous entity)
		{
			CompilerErrors().Add(CompilerErrorFactory.AmbiguousReference(node, node.Name, entity.Entities));
			return TypeSystemServices.ErrorEntity;
		}

		private void GenericArgumentsCountMismatch(TypeReference node, IType type)
		{
			CompilerErrorEmitter().GenericArgumentsCountMismatch(node, type);
		}

		private CompilerErrorEmitter CompilerErrorEmitter()
		{
			return My<Boo.Lang.Compiler.TypeSystem.Services.CompilerErrorEmitter>.Instance;
		}

		public IField ResolveField(IType type, string name)
		{
			return (IField)ResolveMember(type, name, EntityType.Field);
		}

		public IMethod ResolveMethod(IType type, string name)
		{
			return (IMethod)ResolveMember(type, name, EntityType.Method);
		}

		public IProperty ResolveProperty(IType type, string name)
		{
			return (IProperty)ResolveMember(type, name, EntityType.Property);
		}

		public IEntity ResolveMember(IType type, string name, EntityType elementType)
		{
			foreach (IEntity member in type.GetMembers())
			{
				if (elementType == member.EntityType && _entityNameMatcher(member, name))
				{
					return member;
				}
			}
			return null;
		}

		public IEntity Resolve(INamespace @namespace, string name, EntityType elementType)
		{
			if (@namespace == null)
			{
				throw new ArgumentNullException("namespace");
			}
			Set<IEntity> set = new Set<IEntity>();
			ResolveCoalescingNamespaces(@namespace, name, elementType, set);
			return Entities.EntityFromList(set);
		}

		private bool ResolveCoalescingNamespaces(INamespace ns, string name, EntityType elementType, ICollection<IEntity> resultingSet)
		{
			return Namespaces.ResolveCoalescingNamespaces(ns.ParentNamespace, ns, name, elementType, resultingSet);
		}

		public IEntity Resolve(INamespace ns, string name)
		{
			return Resolve(ns, name, EntityType.Any);
		}

		private static bool IsQualifiedName(string name)
		{
			return name.IndexOf('.') > 0;
		}

		private GlobalNamespace GetGlobalNamespace()
		{
			INamespace @namespace = _global;
			GlobalNamespace globalNamespace = @namespace as GlobalNamespace;
			while (globalNamespace == null && @namespace != null)
			{
				@namespace = @namespace.ParentNamespace;
				globalNamespace = @namespace as GlobalNamespace;
			}
			return globalNamespace;
		}

		private static void FlattenChildNamespaces(ICollection<INamespace> resultingList, INamespace ns)
		{
			foreach (IEntity member in ns.GetMembers())
			{
				if (EntityType.Namespace == member.EntityType)
				{
					INamespace @namespace = (INamespace)member;
					resultingList.Add(@namespace);
					FlattenChildNamespaces(resultingList, @namespace);
				}
			}
		}

		public string GetMostSimilarTypeName(string name)
		{
			string[] array = name.Split('.');
			int num = array.Length;
			string text = null;
			if (num > 1)
			{
				INamespace @namespace = null;
				INamespace ns = null;
				for (int i = 1; i < num; i++)
				{
					string name2 = string.Join(".", array, 0, i);
					@namespace = ResolveQualifiedName(name2) as INamespace;
					if (null == @namespace)
					{
						array[i - 1] = GetMostSimilarMemberName(ns, array[i - 1], EntityType.Namespace);
						if (null == array[i - 1])
						{
							break;
						}
						i--;
					}
					else
					{
						ns = @namespace;
					}
				}
				text = GetMostSimilarMemberName(@namespace, array[num - 1], EntityType.Type);
				if (null != text)
				{
					array[num - 1] = text;
					return string.Join(".", array);
				}
			}
			System.Collections.Generic.List<INamespace> list = new System.Collections.Generic.List<INamespace>();
			FlattenChildNamespaces(list, GetGlobalNamespace());
			list.Reverse();
			foreach (INamespace item in list)
			{
				text = GetMostSimilarMemberName(item, array[num - 1], EntityType.Type);
				if (null != text)
				{
					return item.ToString() + "." + text;
				}
			}
			return GetMostSimilarMemberName(GetGlobalNamespace(), array[num - 1], EntityType.Type);
		}

		public string GetMostSimilarMemberName(INamespace ns, string name, EntityType elementType)
		{
			if (null == ns)
			{
				return null;
			}
			string soundex = StringUtilities.GetSoundex(name);
			string text = null;
			foreach (IEntity member in ns.GetMembers())
			{
				if ((EntityType.Any != elementType && elementType != member.EntityType) || text == member.Name)
				{
					continue;
				}
				if (soundex == StringUtilities.GetSoundex(member.Name))
				{
					IMethod method = member as IMethod;
					if (method != null && method.IsSpecialName)
					{
						return member.Name.Substring(4);
					}
					return member.Name;
				}
				text = member.Name;
			}
			return null;
		}

		public IEntity ResolveQualifiedName(INamespace namespaceToResolveAgainst, string name)
		{
			Set<IEntity> set = new Set<IEntity>();
			ResolveQualifiedNameAgainst(namespaceToResolveAgainst, name, EntityType.Any, set);
			return Entities.EntityFromList(set);
		}
	}
}
