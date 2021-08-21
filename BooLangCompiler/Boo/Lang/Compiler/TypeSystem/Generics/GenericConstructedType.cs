using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericConstructedType : IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes, IConstructedTypeInfo, IGenericArgumentsProvider
	{
		protected readonly IType _definition;

		private IType[] _arguments;

		private GenericMapping _genericMapping;

		private bool _fullyConstructed;

		private string _fullName = null;

		private ArrayTypeCache _arrayTypes;

		protected GenericMapping GenericMapping => _genericMapping;

		public IEntity DeclaringEntity => _definition.DeclaringEntity;

		public bool IsClass => _definition.IsClass;

		public bool IsAbstract => _definition.IsAbstract;

		public bool IsInterface => _definition.IsInterface;

		public bool IsEnum => _definition.IsEnum;

		public bool IsByRef => _definition.IsByRef;

		public bool IsValueType => _definition.IsValueType;

		public bool IsFinal => _definition.IsFinal;

		public bool IsArray => _definition.IsArray;

		public bool IsPointer => _definition.IsPointer;

		public IType ElementType => GenericMapping.MapType(_definition.ElementType);

		public IType BaseType => GenericMapping.MapType(_definition.BaseType);

		public IGenericTypeInfo GenericInfo => _definition.GenericInfo;

		public IConstructedTypeInfo ConstructedInfo => this;

		public IType Type => this;

		public INamespace ParentNamespace => GenericMapping.Map(_definition.ParentNamespace) as INamespace;

		public string Name => _definition.Name;

		public string FullName => _fullName ?? (_fullName = BuildFullName());

		public EntityType EntityType => EntityType.Type;

		IType[] IGenericArgumentsProvider.GenericArguments => _arguments;

		IType IConstructedTypeInfo.GenericDefinition => _definition;

		bool IConstructedTypeInfo.FullyConstructed => _fullyConstructed;

		public GenericConstructedType(IType definition, IType[] arguments)
		{
			_definition = definition;
			_arguments = arguments;
			_genericMapping = new InternalGenericMapping(this, arguments);
			_fullyConstructed = IsFullyConstructed();
		}

		protected bool IsFullyConstructed()
		{
			return GenericsServices.GetTypeGenerity(this) == 0;
		}

		protected string BuildFullName()
		{
			return _definition.FullName;
		}

		public int GetTypeDepth()
		{
			return _definition.GetTypeDepth();
		}

		public IEntity GetDefaultMember()
		{
			IEntity defaultMember = _definition.GetDefaultMember();
			if (defaultMember != null)
			{
				return GenericMapping.Map(defaultMember);
			}
			return null;
		}

		public IType[] GetInterfaces()
		{
			return Array.ConvertAll(_definition.GetInterfaces(), GenericMapping.MapType);
		}

		public bool IsSubclassOf(IType other)
		{
			if (null == other)
			{
				return false;
			}
			if (BaseType != null && (BaseType == other || BaseType.IsSubclassOf(other)))
			{
				return true;
			}
			if (other.IsInterface && Array.Exists(GetInterfaces(), (IType i) => TypeCompatibilityRules.IsAssignableFrom(other, i)))
			{
				return true;
			}
			if (other.ConstructedInfo != null && ConstructedInfo.GenericDefinition == other.ConstructedInfo.GenericDefinition)
			{
				for (int j = 0; j < ConstructedInfo.GenericArguments.Length; j++)
				{
					if (!ConstructedInfo.GenericArguments[j].IsSubclassOf(other.ConstructedInfo.GenericArguments[j]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public virtual bool IsAssignableFrom(IType other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this || other.IsSubclassOf(this) || (other.IsNull() && !IsValueType))
			{
				return true;
			}
			return false;
		}

		public bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			Set<IEntity> set = new Set<IEntity>();
			if (!_definition.Resolve(set, name, typesToConsider))
			{
				return false;
			}
			foreach (IEntity item in set)
			{
				resultingSet.Add(GenericMapping.Map(item));
			}
			return true;
		}

		public IEnumerable<IEntity> GetMembers()
		{
			return _definition.GetMembers().Select(GenericMapping.Map);
		}

		IType IConstructedTypeInfo.Map(IType type)
		{
			return GenericMapping.MapType(type);
		}

		IMember IConstructedTypeInfo.Map(IMember member)
		{
			return (IMember)GenericMapping.Map(member);
		}

		IMember IConstructedTypeInfo.UnMap(IMember mapped)
		{
			return GenericMapping.UnMap(mapped);
		}

		public bool IsDefined(IType attributeType)
		{
			return _definition.IsDefined(GenericMapping.MapType(attributeType));
		}

		public override string ToString()
		{
			return FullName;
		}

		public IArrayType MakeArrayType(int rank)
		{
			if (null == _arrayTypes)
			{
				_arrayTypes = new ArrayTypeCache(this);
			}
			return _arrayTypes.MakeArrayType(rank);
		}

		public IType MakePointerType()
		{
			return null;
		}
	}
}
