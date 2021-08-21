namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericMappedEvent : GenericMappedMember<IEvent>, IEvent, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		public bool IsAbstract => base.SourceMember.IsAbstract;

		public bool IsVirtual => base.SourceMember.IsVirtual;

		public GenericMappedEvent(TypeSystemServices tss, IEvent source, GenericMapping genericMapping)
			: base(tss, source, genericMapping)
		{
		}

		public IMethod GetAddMethod()
		{
			return base.GenericMapping.Map(base.SourceMember.GetAddMethod());
		}

		public IMethod GetRemoveMethod()
		{
			return base.GenericMapping.Map(base.SourceMember.GetRemoveMethod());
		}

		public IMethod GetRaiseMethod()
		{
			return base.GenericMapping.Map(base.SourceMember.GetRemoveMethod());
		}
	}
}
