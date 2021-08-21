namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericMappedField : GenericMappedAccessibleMember<IField>, IField, IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		public bool IsInitOnly => base.SourceMember.IsInitOnly;

		public bool IsLiteral => base.SourceMember.IsLiteral;

		public object StaticValue => base.SourceMember.StaticValue;

		public bool IsVolatile => base.SourceMember.IsVolatile;

		public GenericMappedField(TypeSystemServices tss, IField source, GenericMapping genericMapping)
			: base(tss, source, genericMapping)
		{
		}
	}
}
