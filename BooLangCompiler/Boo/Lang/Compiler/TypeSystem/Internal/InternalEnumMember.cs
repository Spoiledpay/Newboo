using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalEnumMember : InternalEntity<EnumMember>, IField, IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		public override bool IsStatic => true;

		public override bool IsPublic => true;

		public override bool IsProtected => false;

		public bool IsLiteral => true;

		public override bool IsInternal => false;

		public override bool IsPrivate => false;

		public bool IsInitOnly => false;

		public override EntityType EntityType => EntityType.Field;

		public IType Type => base.DeclaringType;

		public object StaticValue
		{
			get
			{
				if (_node.Initializer.NodeType == NodeType.IntegerLiteralExpression)
				{
					return Convert.ChangeType(((IntegerLiteralExpression)_node.Initializer).Value, ((InternalEnum)base.DeclaringType).UnderlyingType);
				}
				return Error.Default;
			}
		}

		public bool IsVolatile => false;

		public bool IsDuckTyped => false;

		public InternalEnumMember(EnumMember member)
			: base(member)
		{
		}
	}
}
