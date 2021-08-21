using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalConstructor : InternalMethod, IConstructor, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IEntityWithParameters, IEntity
	{
		public bool HasSuperCall { get; set; }

		public bool HasSelfCall { get; set; }

		public override IType ReturnType => _provider.VoidType;

		public override EntityType EntityType => EntityType.Constructor;

		public InternalConstructor(InternalTypeSystemProvider provider, Constructor constructor)
			: base(provider, constructor)
		{
		}
	}
}
