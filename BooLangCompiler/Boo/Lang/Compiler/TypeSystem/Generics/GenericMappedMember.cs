namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public abstract class GenericMappedMember<T> : IGenericMappedMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes where T : IMember
	{
		protected readonly TypeSystemServices _tss;

		private readonly T _sourceMember;

		private readonly GenericMapping _genericMapping;

		private string _fullName = null;

		public T SourceMember => _sourceMember;

		IMember IGenericMappedMember.SourceMember => SourceMember;

		public GenericMapping GenericMapping => _genericMapping;

		public bool IsDuckTyped => SourceMember.IsDuckTyped;

		public IType DeclaringType => GenericMapping.MapType(SourceMember.DeclaringType);

		public bool IsStatic => SourceMember.IsStatic;

		public IType Type => GenericMapping.MapType(SourceMember.Type);

		public EntityType EntityType => SourceMember.EntityType;

		public string Name => SourceMember.Name;

		public string FullName => _fullName ?? (_fullName = BuildFullName());

		public bool IsPublic => SourceMember.IsPublic;

		protected GenericMappedMember(TypeSystemServices tss, T sourceMember, GenericMapping genericMapping)
		{
			_tss = tss;
			_sourceMember = sourceMember;
			_genericMapping = genericMapping;
		}

		protected virtual string BuildFullName()
		{
			return DeclaringType.FullName + "." + Name;
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool IsDefined(IType attributeType)
		{
			T sourceMember = _sourceMember;
			return sourceMember.IsDefined(attributeType);
		}
	}
}
