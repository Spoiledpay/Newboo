using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalField : InternalEntity<Field>, IField, IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		public override EntityType EntityType => EntityType.Field;

		public IType Type => (_node.Type != null) ? TypeSystemServices.GetType(_node.Type) : Unknown.Default;

		public bool IsLiteral => StaticValue != null;

		public bool IsInitOnly => _node.IsFinal;

		public object StaticValue { get; set; }

		public bool IsVolatile => _node.IsVolatile;

		public Field Field => _node;

		public bool IsDuckTyped => false;

		public InternalField(Field field)
			: base(field)
		{
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
