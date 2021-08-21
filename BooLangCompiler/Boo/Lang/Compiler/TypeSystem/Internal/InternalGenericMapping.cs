using Boo.Lang.Compiler.TypeSystem.Generics;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalGenericMapping : GenericMapping
	{
		public InternalGenericMapping(IType constructedType, IType[] arguments)
			: base(constructedType, arguments)
		{
		}

		public InternalGenericMapping(IMethod constructedMethod, IType[] arguments)
			: base(constructedMethod, arguments)
		{
		}

		protected override IMember CreateMappedMember(IMember source)
		{
			switch (source.EntityType)
			{
			case EntityType.Method:
			{
				IMethod source6 = (IMethod)source;
				return new GenericMappedMethod(base.TypeSystemServices, source6, this);
			}
			case EntityType.Constructor:
			{
				IConstructor source5 = (IConstructor)source;
				return new GenericMappedConstructor(base.TypeSystemServices, source5, this);
			}
			case EntityType.Field:
			{
				IField source4 = (IField)source;
				return new GenericMappedField(base.TypeSystemServices, source4, this);
			}
			case EntityType.Property:
			{
				IProperty source3 = (IProperty)source;
				return new GenericMappedProperty(base.TypeSystemServices, source3, this);
			}
			case EntityType.Event:
			{
				IEvent source2 = (IEvent)source;
				return new GenericMappedEvent(base.TypeSystemServices, source2, this);
			}
			default:
				return source;
			}
		}

		public override IMember UnMap(IMember mapped)
		{
			return (mapped as IGenericMappedMember)?.SourceMember;
		}
	}
}
