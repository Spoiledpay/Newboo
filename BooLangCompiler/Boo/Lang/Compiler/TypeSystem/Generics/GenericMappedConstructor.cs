namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericMappedConstructor : GenericMappedMethod, IConstructor, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IEntityWithParameters, IEntity
	{
		public GenericMappedConstructor(TypeSystemServices tss, IConstructor source, GenericMapping genericMapping)
			: base(tss, (IMethod)source, genericMapping)
		{
		}
	}
}
