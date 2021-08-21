using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public abstract class ExternalEntity<T> : IExternalEntity, IEntity, IEntityWithAttributes where T : MemberInfo
	{
		protected readonly T _memberInfo;

		private string _cachedFullName;

		protected readonly IReflectionTypeSystemProvider _provider;

		private bool? _isDuckTyped;

		private bool? _isExtension;

		public MemberInfo MemberInfo => _memberInfo;

		public virtual string Name
		{
			get
			{
				T memberInfo = _memberInfo;
				return memberInfo.Name;
			}
		}

		public string FullName
		{
			get
			{
				if (_cachedFullName != null)
				{
					return _cachedFullName;
				}
				return _cachedFullName = BuildFullName();
			}
		}

		public abstract EntityType EntityType { get; }

		protected abstract Type MemberType { get; }

		public bool IsDuckTyped
		{
			get
			{
				if (!_isDuckTyped.HasValue)
				{
					_isDuckTyped = !MemberType.IsValueType && MetadataUtil.IsAttributeDefined(_memberInfo, Types.DuckTypedAttribute);
				}
				return _isDuckTyped.Value;
			}
		}

		public bool IsExtension
		{
			get
			{
				if (!_isExtension.HasValue)
				{
					_isExtension = IsClrExtension;
				}
				return _isExtension.Value;
			}
		}

		private bool IsClrExtension => MetadataUtil.IsAttributeDefined(_memberInfo, Types.ClrExtensionAttribute);

		public ExternalEntity(IReflectionTypeSystemProvider typeSystemServices, T memberInfo)
		{
			_provider = typeSystemServices;
			_memberInfo = memberInfo;
		}

		protected virtual string BuildFullName()
		{
			T memberInfo = _memberInfo;
			return Map(memberInfo.DeclaringType).FullName + "." + Name;
		}

		protected IType Map(Type type)
		{
			return _provider.Map(type);
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool IsDefined(IType attributeType)
		{
			ExternalType externalType = attributeType as ExternalType;
			if (null == externalType)
			{
				return false;
			}
			return MetadataUtil.IsAttributeDefined(_memberInfo, externalType.ActualType);
		}
	}
}
