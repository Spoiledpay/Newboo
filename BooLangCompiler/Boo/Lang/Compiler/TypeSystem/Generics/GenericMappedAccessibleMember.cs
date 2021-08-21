namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public abstract class GenericMappedAccessibleMember<T> : GenericMappedMember<T>, IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes where T : IAccessibleMember
	{
		public bool IsProtected => base.SourceMember.IsProtected;

		public bool IsInternal => base.SourceMember.IsInternal;

		public bool IsPrivate => base.SourceMember.IsPrivate;

		protected GenericMappedAccessibleMember(TypeSystemServices tss, T source, GenericMapping genericMapping)
			: base(tss, source, genericMapping)
		{
		}
	}
}
