using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public abstract class AbstractType : IType, ITypedEntity, IEntityWithAttributes, INamespace, IEntity
	{
		private ArrayTypeCache _arrayTypes;

		public abstract string Name { get; }

		public abstract EntityType EntityType { get; }

		public virtual string FullName => Name;

		public virtual IEntity DeclaringEntity => null;

		public virtual IType Type => this;

		public virtual bool IsByRef => false;

		public virtual bool IsClass => false;

		public virtual bool IsAbstract => false;

		public virtual bool IsInterface => false;

		public virtual bool IsFinal => true;

		public virtual bool IsEnum => false;

		public virtual bool IsValueType => false;

		public virtual bool IsArray => false;

		public virtual bool IsPointer => false;

		public virtual IType BaseType => null;

		public virtual IType ElementType => null;

		public virtual INamespace ParentNamespace => null;

		IGenericTypeInfo IType.GenericInfo => null;

		IConstructedTypeInfo IType.ConstructedInfo => null;

		public virtual IEntity GetDefaultMember()
		{
			return null;
		}

		public virtual int GetTypeDepth()
		{
			return 0;
		}

		public virtual bool IsSubclassOf(IType other)
		{
			return false;
		}

		public virtual bool IsAssignableFrom(IType other)
		{
			return false;
		}

		public virtual IType[] GetInterfaces()
		{
			return new IType[0];
		}

		public virtual IEnumerable<IEntity> GetMembers()
		{
			return new IEntity[0];
		}

		public virtual bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return false;
		}

		public sealed override string ToString()
		{
			return this.DisplayName();
		}

		public bool IsDefined(IType attributeType)
		{
			return false;
		}

		public virtual IArrayType MakeArrayType(int rank)
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
