using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalProperty : ExternalEntity<PropertyInfo>, IProperty, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity
	{
		private IParameter[] _parameters;

		private MethodInfo _accessor = null;

		private CachedMethod _getter = null;

		private CachedMethod _setter = null;

		public virtual IType DeclaringType => _provider.Map(_memberInfo.DeclaringType);

		public bool IsStatic => GetAccessor().IsStatic;

		public bool IsPublic => GetAccessor().IsPublic;

		public bool IsProtected
		{
			get
			{
				MethodInfo accessor = GetAccessor();
				return accessor.IsFamily || accessor.IsFamilyOrAssembly;
			}
		}

		public bool IsInternal => GetAccessor().IsAssembly;

		public bool IsPrivate => GetAccessor().IsPrivate;

		public override EntityType EntityType => EntityType.Property;

		public virtual IType Type => _provider.Map(_memberInfo.PropertyType);

		public PropertyInfo PropertyInfo => _memberInfo;

		public bool AcceptVarArgs => false;

		protected override Type MemberType => _memberInfo.PropertyType;

		public ExternalProperty(IReflectionTypeSystemProvider typeSystemServices, PropertyInfo property)
			: base(typeSystemServices, property)
		{
		}

		public virtual IParameter[] GetParameters()
		{
			if (null != _parameters)
			{
				return _parameters;
			}
			return _parameters = _provider.Map(_memberInfo.GetIndexParameters());
		}

		public virtual IMethod GetGetMethod()
		{
			if (null != _getter)
			{
				return _getter.Value;
			}
			return (_getter = new CachedMethod(FindGetMethod())).Value;
		}

		private IMethod FindGetMethod()
		{
			MethodInfo getMethod = _memberInfo.GetGetMethod(nonPublic: true);
			if (null == getMethod)
			{
				PropertyInfo propertyInfo = FindBaseProperty();
				if (null == propertyInfo)
				{
					return null;
				}
				getMethod = propertyInfo.GetGetMethod(nonPublic: true);
				if (null == getMethod)
				{
					return null;
				}
			}
			return _provider.Map(getMethod);
		}

		private PropertyInfo FindBaseProperty()
		{
			if (null == _memberInfo.DeclaringType.BaseType)
			{
				return null;
			}
			return _memberInfo.DeclaringType.BaseType.GetProperty(_memberInfo.Name, _memberInfo.PropertyType, GetParameterTypes(_memberInfo.GetIndexParameters()));
		}

		private static Type[] GetParameterTypes(ParameterInfo[] parameters)
		{
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return array;
		}

		public virtual IMethod GetSetMethod()
		{
			if (null != _setter)
			{
				return _setter.Value;
			}
			return (_setter = new CachedMethod(FindSetMethod())).Value;
		}

		private IMethod FindSetMethod()
		{
			MethodInfo setMethod = _memberInfo.GetSetMethod(nonPublic: true);
			if (null == setMethod)
			{
				return null;
			}
			return _provider.Map(setMethod);
		}

		private MethodInfo GetAccessor()
		{
			if (null != _accessor)
			{
				return _accessor;
			}
			return _accessor = FindAccessor();
		}

		private MethodInfo FindAccessor()
		{
			MethodInfo getMethod = _memberInfo.GetGetMethod(nonPublic: true);
			if (null != getMethod)
			{
				return getMethod;
			}
			return _memberInfo.GetSetMethod(nonPublic: true);
		}
	}
}
