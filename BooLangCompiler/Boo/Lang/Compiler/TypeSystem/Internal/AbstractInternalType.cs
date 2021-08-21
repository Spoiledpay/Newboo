using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public abstract class AbstractInternalType : InternalEntity<TypeDefinition>, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes, IGenericTypeInfo, IGenericParametersProvider
	{
		protected readonly InternalTypeSystemProvider _provider;

		private readonly Dictionary<IType[], IType> _constructedTypes = new Dictionary<IType[], IType>(ArrayEqualityComparer<IType>.Default);

		private readonly System.Collections.Generic.List<IEntity> _memberEntitiesCache = new System.Collections.Generic.List<IEntity>();

		protected IType _elementType;

		private ArrayTypeCache _arrayTypes;

		public override string FullName => _node.FullName;

		public IEntity DeclaringEntity => _node.ParentNode.Entity as IType;

		public virtual INamespace ParentNamespace => (INamespace)TypeSystemServices.GetEntity(_node.ParentNode);

		public virtual IType BaseType => null;

		public TypeDefinition TypeDefinition => _node;

		public IType Type => this;

		public bool IsByRef { get; protected set; }

		public IType ElementType => _elementType ?? (_elementType = CreateElementType());

		public bool IsClass => NodeType.ClassDefinition == _node.NodeType;

		public bool IsAbstract => _node.IsAbstract;

		public virtual bool IsFinal => _node.IsFinal || IsValueType;

		public bool IsInterface => NodeType.InterfaceDefinition == _node.NodeType;

		public bool IsEnum => NodeType.EnumDefinition == _node.NodeType;

		public virtual bool IsValueType => false;

		public bool IsArray => false;

		public virtual bool IsPointer => false;

		public override EntityType EntityType => EntityType.Type;

		public IGenericTypeInfo GenericInfo => (TypeDefinition.GenericParameters.Count != 0) ? this : null;

		public IConstructedTypeInfo ConstructedInfo => null;

		IGenericParameter[] IGenericParametersProvider.GenericParameters => Array.ConvertAll(_node.GenericParameters.ToArray(), (GenericParameterDeclaration gpd) => (IGenericParameter)gpd.Entity);

		protected AbstractInternalType(InternalTypeSystemProvider provider, TypeDefinition typeDefinition)
			: base(typeDefinition)
		{
			_provider = provider;
			TypeMemberCollection members = typeDefinition.Members;
			EventHandler value = delegate
			{
				ClearMemberEntitiesCache();
			};
			members.Changed += value;
		}

		public virtual bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (ResolveGenericParameter(resultingSet, name, typesToConsider))
			{
				return true;
			}
			return ResolveMember(resultingSet, name, typesToConsider);
		}

		protected bool ResolveMember(ICollection<IEntity> resolvedSet, string name, EntityType typesToConsider)
		{
			return My<NameResolutionService>.Instance.Resolve(name, GetMembers(), typesToConsider, resolvedSet);
		}

		protected bool ResolveGenericParameter(ICollection<IEntity> targetList, string name, EntityType flags)
		{
			if (!Entities.IsFlagSet(flags, EntityType.Type))
			{
				return false;
			}
			using (IEnumerator<GenericParameterDeclaration> enumerator = _node.GenericParameters.Where((GenericParameterDeclaration gpd) => gpd.Name == name).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					GenericParameterDeclaration current = enumerator.Current;
					targetList.Add(current.Entity);
					return true;
				}
			}
			return false;
		}

		protected virtual IType CreateElementType()
		{
			return null;
		}

		public virtual int GetTypeDepth()
		{
			return 1;
		}

		public IEntity GetDefaultMember()
		{
			IType type = My<TypeSystemServices>.Instance.Map(Types.DefaultMemberAttribute);
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in _node.Attributes)
			{
				IConstructor constructor = TypeSystemServices.GetEntity(attribute) as IConstructor;
				if (constructor != null && type == constructor.DeclaringType)
				{
					StringLiteralExpression stringLiteralExpression = attribute.Arguments[0] as StringLiteralExpression;
					if (null != stringLiteralExpression)
					{
						System.Collections.Generic.List<IEntity> list = new System.Collections.Generic.List<IEntity>();
						Resolve(list, stringLiteralExpression.Value, EntityType.Any);
						return Entities.EntityFromList(list);
					}
				}
			}
			return null;
		}

		public virtual bool IsSubclassOf(IType other)
		{
			return false;
		}

		public virtual bool IsAssignableFrom(IType other)
		{
			return this == other || (!IsValueType && other.IsNull()) || other.IsSubclassOf(this);
		}

		public IType[] GetInterfaces()
		{
			List<IType> list = new List<IType>(_node.BaseTypes.Count);
			foreach (TypeReference baseType in _node.BaseTypes)
			{
				IType type = TypeSystemServices.GetType(baseType);
				if (type.IsInterface)
				{
					list.AddUnique(type);
				}
			}
			return list.ToArray();
		}

		public virtual IEnumerable<IEntity> GetMembers()
		{
			if (_memberEntitiesCache.Count > 0)
			{
				return _memberEntitiesCache;
			}
			GetMemberEntities(_node.Members);
			return _memberEntitiesCache;
		}

		private void ClearMemberEntitiesCache()
		{
			_memberEntitiesCache.Clear();
		}

		private void GetMemberEntities(TypeMemberCollection members)
		{
			foreach (TypeMember item in members.Except<StatementTypeMember, Destructor>())
			{
				_memberEntitiesCache.Add(_provider.EntityFor(item));
			}
		}

		public sealed override string ToString()
		{
			return this.DisplayName();
		}

		IType IGenericTypeInfo.ConstructType(IType[] arguments)
		{
			if (!_constructedTypes.TryGetValue(arguments, out var value))
			{
				value = CreateConstructedType(arguments);
				_constructedTypes.Add(arguments, value);
			}
			return value;
		}

		protected virtual IType CreateConstructedType(IType[] arguments)
		{
			return new GenericConstructedType(this, arguments);
		}

		public IArrayType MakeArrayType(int rank)
		{
			if (null == _arrayTypes)
			{
				_arrayTypes = new ArrayTypeCache(this);
			}
			return _arrayTypes.MakeArrayType(rank);
		}

		public virtual IType MakePointerType()
		{
			return null;
		}
	}
}
