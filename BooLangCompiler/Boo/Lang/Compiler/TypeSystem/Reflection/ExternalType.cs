using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	public class ExternalType : IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		protected IReflectionTypeSystemProvider _provider;

		private readonly Type _type;

		private IType[] _interfaces;

		private IEntity[] _members;

		private int _typeDepth = -1;

		private string _primitiveName;

		private string _fullName;

		private string _name;

		private ExternalGenericTypeInfo _genericTypeDefinitionInfo = null;

		private ExternalConstructedTypeInfo _genericTypeInfo = null;

		private ArrayTypeCache _arrayTypes;

		public virtual string FullName
		{
			get
			{
				if (null != _fullName)
				{
					return _fullName;
				}
				return _fullName = BuildFullName();
			}
		}

		internal string PrimitiveName
		{
			get
			{
				return _primitiveName;
			}
			set
			{
				_primitiveName = value;
			}
		}

		public virtual string Name
		{
			get
			{
				if (null != _name)
				{
					return _name;
				}
				return _name = TypeUtilities.TypeName(_type);
			}
		}

		public EntityType EntityType => EntityType.Type;

		public IType Type => this;

		public virtual bool IsFinal => _type.IsSealed;

		public bool IsByRef => _type.IsByRef;

		public virtual IEntity DeclaringEntity => DeclaringType;

		public IType DeclaringType
		{
			get
			{
				Type declaringType = _type.DeclaringType;
				return (null != declaringType) ? _provider.Map(declaringType) : null;
			}
		}

		public virtual IType ElementType => _provider.Map(_type.GetElementType() ?? _type);

		public virtual bool IsClass => _type.IsClass;

		public bool IsAbstract => _type.IsAbstract;

		public bool IsInterface => _type.IsInterface;

		public bool IsEnum => _type.IsEnum;

		public virtual bool IsValueType => _type.IsValueType;

		public bool IsArray => false;

		public bool IsPointer => _type.IsPointer;

		public virtual IType BaseType
		{
			get
			{
				Type baseType = _type.BaseType;
				return (null == baseType) ? null : _provider.Map(baseType);
			}
		}

		public Type ActualType => _type;

		public virtual INamespace ParentNamespace => null;

		public virtual IGenericTypeInfo GenericInfo
		{
			get
			{
				if (ActualType.IsGenericTypeDefinition)
				{
					return _genericTypeDefinitionInfo ?? (_genericTypeDefinitionInfo = new ExternalGenericTypeInfo(_provider, this));
				}
				return null;
			}
		}

		public virtual IConstructedTypeInfo ConstructedInfo
		{
			get
			{
				if (ActualType.IsGenericType && !ActualType.IsGenericTypeDefinition)
				{
					return _genericTypeInfo ?? (_genericTypeInfo = new ExternalConstructedTypeInfo(_provider, this));
				}
				return null;
			}
		}

		public ExternalType(IReflectionTypeSystemProvider tss, Type type)
		{
			if (null == type)
			{
				throw new ArgumentException("type");
			}
			_provider = tss;
			_type = type;
		}

		public bool IsDefined(IType attributeType)
		{
			ExternalType externalType = attributeType as ExternalType;
			if (null == externalType)
			{
				return false;
			}
			return MetadataUtil.IsAttributeDefined(_type, externalType.ActualType);
		}

		protected virtual MemberInfo[] GetDefaultMembers()
		{
			return ActualType.GetDefaultMembers();
		}

		public IEntity GetDefaultMember()
		{
			return _provider.Map(GetDefaultMembers());
		}

		public virtual bool IsSubclassOf(IType other)
		{
			ExternalType externalType = other as ExternalType;
			if (externalType == null)
			{
				return false;
			}
			return _type.IsSubclassOf(externalType._type) || (externalType.IsInterface && externalType._type.IsAssignableFrom(_type));
		}

		public virtual bool IsAssignableFrom(IType other)
		{
			ExternalType externalType = other as ExternalType;
			if (null == externalType)
			{
				if (EntityType.Null == other.EntityType)
				{
					return !IsValueType;
				}
				return other.IsSubclassOf(this);
			}
			if (other == _provider.Map(Types.Void))
			{
				return false;
			}
			return _type.IsAssignableFrom(externalType._type);
		}

		public virtual IType[] GetInterfaces()
		{
			if (null == _interfaces)
			{
				Type[] interfaces = _type.GetInterfaces();
				_interfaces = new IType[interfaces.Length];
				for (int i = 0; i < _interfaces.Length; i++)
				{
					_interfaces[i] = _provider.Map(interfaces[i]);
				}
			}
			return _interfaces;
		}

		public virtual IEnumerable<IEntity> GetMembers()
		{
			if (null == _members)
			{
				IEntity[] array = (_members = CreateMembers());
			}
			return _members;
		}

		protected virtual IEntity[] CreateMembers()
		{
			List<IEntity> list = new List<IEntity>();
			MemberInfo[] array = DeclaredMembers();
			foreach (MemberInfo member in array)
			{
				list.Add(_provider.Map(member));
			}
			return list.ToArray();
		}

		private MemberInfo[] DeclaredMembers()
		{
			return _type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		public int GetTypeDepth()
		{
			if (-1 == _typeDepth)
			{
				_typeDepth = GetTypeDepth(_type);
			}
			return _typeDepth;
		}

		public virtual bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool flag = My<NameResolutionService>.Instance.Resolve(name, GetMembers(), typesToConsider, resultingSet);
			if (IsInterface)
			{
				if (_provider.Map(typeof(object)).Resolve(resultingSet, name, typesToConsider))
				{
					flag = true;
				}
				IType[] interfaces = GetInterfaces();
				foreach (IType type in interfaces)
				{
					flag |= type.Resolve(resultingSet, name, typesToConsider);
				}
			}
			else if (!flag || TypeSystemServices.ContainsMethodsOnly(resultingSet))
			{
				IType baseType = BaseType;
				if (null != baseType)
				{
					flag |= baseType.Resolve(resultingSet, name, typesToConsider);
				}
			}
			return flag;
		}

		public override string ToString()
		{
			return this.DisplayName();
		}

		private static int GetTypeDepth(Type type)
		{
			if (type.IsByRef)
			{
				return GetTypeDepth(type.GetElementType());
			}
			if (type.IsInterface)
			{
				return GetInterfaceDepth(type);
			}
			return GetClassDepth(type);
		}

		private static int GetClassDepth(Type type)
		{
			int num = 0;
			Type @object = Types.Object;
			while (type != null && type != @object)
			{
				type = type.BaseType;
				num++;
			}
			return num;
		}

		private static int GetInterfaceDepth(Type type)
		{
			Type[] interfaces = type.GetInterfaces();
			if (interfaces.Length > 0)
			{
				int num = 0;
				Type[] array = interfaces;
				foreach (Type type2 in array)
				{
					int interfaceDepth = GetInterfaceDepth(type2);
					if (interfaceDepth > num)
					{
						num = interfaceDepth;
					}
				}
				return 1 + num;
			}
			return 1;
		}

		protected virtual string BuildFullName()
		{
			if (_primitiveName != null)
			{
				return _primitiveName;
			}
			if (_type.IsByRef)
			{
				return "ref " + ElementType.FullName;
			}
			return TypeUtilities.GetFullName(_type);
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
			return _provider.Map(_type.MakePointerType());
		}
	}
}
