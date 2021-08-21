using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public abstract class AbstractGenericParameter : IGenericParameter, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private TypeSystemServices _tss;

		private ArrayTypeCache _arrayTypes;

		public abstract int GenericParameterPosition { get; }

		public abstract bool MustHaveDefaultConstructor { get; }

		public abstract Variance Variance { get; }

		public abstract bool IsClass { get; }

		public abstract bool IsValueType { get; }

		public abstract IEntity DeclaringEntity { get; }

		protected IType DeclaringType
		{
			get
			{
				if (DeclaringEntity is IType)
				{
					return (IType)DeclaringEntity;
				}
				if (DeclaringEntity is IMethod)
				{
					return ((IMethod)DeclaringEntity).DeclaringType;
				}
				return null;
			}
		}

		protected IMethod DeclaringMethod => DeclaringEntity as IMethod;

		bool IType.IsAbstract => false;

		bool IType.IsInterface => false;

		bool IType.IsEnum => false;

		public bool IsByRef => false;

		bool IType.IsFinal => true;

		bool IType.IsArray => false;

		bool IType.IsPointer => false;

		IType IType.ElementType => null;

		public IType BaseType => FindBaseType();

		IGenericTypeInfo IType.GenericInfo => null;

		IConstructedTypeInfo IType.ConstructedInfo => null;

		public abstract string Name { get; }

		public string FullName => $"{DeclaringEntity.FullName}.{Name}";

		public EntityType EntityType => EntityType.Type;

		public IType Type => this;

		INamespace INamespace.ParentNamespace => DeclaringType;

		protected AbstractGenericParameter(TypeSystemServices tss)
		{
			_tss = tss;
		}

		public abstract IType[] GetTypeConstraints();

		public int GetTypeDepth()
		{
			return DeclaringType.GetTypeDepth() + 1;
		}

		public IEntity GetDefaultMember()
		{
			return null;
		}

		public IType[] GetInterfaces()
		{
			return Array.FindAll(GetTypeConstraints(), (IType type) => type.IsInterface);
		}

		public bool IsSubclassOf(IType other)
		{
			return other == BaseType || BaseType.IsSubclassOf(other);
		}

		public bool IsAssignableFrom(IType other)
		{
			if (other == this)
			{
				return true;
			}
			if (other.IsNull())
			{
				return IsClass;
			}
			IGenericParameter genericParameter = other as IGenericParameter;
			if (genericParameter != null && Array.Exists(genericParameter.GetTypeConstraints(), (IType constraint) => TypeCompatibilityRules.IsAssignableFrom(this, constraint)))
			{
				return true;
			}
			return false;
		}

		IEnumerable<IEntity> INamespace.GetMembers()
		{
			return NullNamespace.EmptyEntityArray;
		}

		bool INamespace.Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool flag = false;
			IType[] typeConstraints = GetTypeConstraints();
			foreach (IType type in typeConstraints)
			{
				flag |= type.Resolve(resultingSet, name, typesToConsider);
			}
			return flag | _tss.ObjectType.Resolve(resultingSet, name, typesToConsider);
		}

		public override string ToString()
		{
			return Name;
		}

		bool IEntityWithAttributes.IsDefined(IType attributeType)
		{
			throw new NotImplementedException();
		}

		protected IType FindBaseType()
		{
			IType[] typeConstraints = GetTypeConstraints();
			foreach (IType type in typeConstraints)
			{
				if (!type.IsInterface)
				{
					return type;
				}
			}
			return _tss.ObjectType;
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
