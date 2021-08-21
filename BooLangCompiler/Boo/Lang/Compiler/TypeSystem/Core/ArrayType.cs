using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class ArrayType : IArrayType, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private readonly IType _elementType;

		private readonly int _rank;

		private ArrayTypeCache _arrayTypes;

		public IEntity DeclaringEntity => null;

		public string Name => (_rank > 1) ? string.Concat("(", _elementType, ", ", _rank, ")") : string.Concat("(", _elementType, ")");

		public EntityType EntityType => EntityType.Array;

		public string FullName => Name;

		public IType Type => this;

		public bool IsFinal => true;

		public bool IsByRef => false;

		public bool IsClass => false;

		public bool IsInterface => false;

		public bool IsAbstract => false;

		public bool IsEnum => false;

		public bool IsValueType => false;

		public bool IsArray => true;

		public bool IsPointer => false;

		public int Rank => _rank;

		public IType ElementType => _elementType;

		public IType BaseType => My<TypeSystemServices>.Instance.ArrayType;

		public INamespace ParentNamespace => ElementType.ParentNamespace;

		IGenericTypeInfo IType.GenericInfo => null;

		IConstructedTypeInfo IType.ConstructedInfo => null;

		public ArrayType(IType elementType, int rank)
		{
			_elementType = elementType;
			_rank = rank;
		}

		public int GetTypeDepth()
		{
			return 2;
		}

		public IEntity GetDefaultMember()
		{
			return null;
		}

		public virtual bool IsSubclassOf(IType other)
		{
			TypeSystemServices instance = My<TypeSystemServices>.Instance;
			if (other == instance.ArrayType || instance.ArrayType.IsSubclassOf(other))
			{
				return true;
			}
			if (other.ConstructedInfo != null && other.ConstructedInfo.GenericDefinition == instance.IEnumerableGenericType && IsSubclassOfGenericEnumerable(other))
			{
				return true;
			}
			return false;
		}

		protected virtual bool IsSubclassOfGenericEnumerable(IType enumerableType)
		{
			return IsAssignableFrom(enumerableType.ConstructedInfo.GenericArguments[0], _elementType);
		}

		public virtual bool IsAssignableFrom(IType other)
		{
			if (other == this || other.IsNull())
			{
				return true;
			}
			if (!other.IsArray)
			{
				return false;
			}
			IArrayType arrayType = (IArrayType)other;
			if (arrayType.Rank != _rank)
			{
				return false;
			}
			if (arrayType == EmptyArrayType.Default)
			{
				return true;
			}
			IType elementType = arrayType.ElementType;
			return IsAssignableFrom(_elementType, elementType);
		}

		private bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		public IType[] GetInterfaces()
		{
			TypeSystemServices instance = My<TypeSystemServices>.Instance;
			return new IType[6]
			{
				instance.IEnumerableType,
				instance.ICollectionType,
				instance.IListType,
				instance.IEnumerableGenericType.GenericInfo.ConstructType(_elementType),
				instance.ICollectionGenericType.GenericInfo.ConstructType(_elementType),
				instance.IListGenericType.GenericInfo.ConstructType(_elementType)
			};
		}

		public IEnumerable<IEntity> GetMembers()
		{
			return BaseType.GetMembers();
		}

		public bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return BaseType.Resolve(resultingSet, name, typesToConsider);
		}

		public override string ToString()
		{
			return this.DisplayName();
		}

		public bool IsDefined(IType attributeType)
		{
			return false;
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
