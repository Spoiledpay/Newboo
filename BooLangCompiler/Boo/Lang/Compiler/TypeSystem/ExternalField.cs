using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalField : ExternalEntity<FieldInfo>, IField, IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		private static readonly Type IsVolatileType = typeof(IsVolatile);

		private bool? _isVolatile;

		public virtual IType DeclaringType => _provider.Map(_memberInfo.DeclaringType);

		public bool IsPublic => _memberInfo.IsPublic;

		public bool IsProtected => _memberInfo.IsFamily || _memberInfo.IsFamilyOrAssembly;

		public bool IsPrivate => _memberInfo.IsPrivate;

		public bool IsInternal => _memberInfo.IsAssembly;

		public bool IsStatic => _memberInfo.IsStatic;

		public bool IsLiteral => _memberInfo.IsLiteral;

		public bool IsInitOnly => _memberInfo.IsInitOnly;

		public override EntityType EntityType => EntityType.Field;

		public virtual IType Type => _provider.Map(_memberInfo.FieldType);

		public object StaticValue => _memberInfo.GetValue(null);

		public bool IsVolatile
		{
			get
			{
				if (!_isVolatile.HasValue)
				{
					Type[] requiredCustomModifiers = _memberInfo.GetRequiredCustomModifiers();
					_isVolatile = false;
					Type[] array = requiredCustomModifiers;
					foreach (Type type in array)
					{
						if (type == IsVolatileType)
						{
							_isVolatile = true;
							break;
						}
					}
				}
				return _isVolatile.Value;
			}
		}

		public FieldInfo FieldInfo => _memberInfo;

		protected override Type MemberType => _memberInfo.FieldType;

		public ExternalField(IReflectionTypeSystemProvider provider, FieldInfo field)
			: base(provider, field)
		{
		}
	}
}
