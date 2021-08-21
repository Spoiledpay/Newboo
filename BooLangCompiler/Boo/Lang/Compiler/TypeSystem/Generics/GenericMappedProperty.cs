namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericMappedProperty : GenericMappedAccessibleMember<IProperty>, IProperty, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity
	{
		private IParameter[] _parameters;

		public bool AcceptVarArgs => base.SourceMember.AcceptVarArgs;

		public bool IsExtension => base.SourceMember.IsExtension;

		public GenericMappedProperty(TypeSystemServices tss, IProperty source, GenericMapping genericMapping)
			: base(tss, source, genericMapping)
		{
		}

		public IParameter[] GetParameters()
		{
			return _parameters ?? (_parameters = base.GenericMapping.MapParameters(base.SourceMember.GetParameters()));
		}

		public IMethod GetGetMethod()
		{
			return base.GenericMapping.Map(base.SourceMember.GetGetMethod());
		}

		public IMethod GetSetMethod()
		{
			return base.GenericMapping.Map(base.SourceMember.GetSetMethod());
		}

		public override string ToString()
		{
			return $"{base.Name} as {base.Type}";
		}
	}
}
