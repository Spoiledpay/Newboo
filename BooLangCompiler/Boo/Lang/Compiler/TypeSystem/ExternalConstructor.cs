using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalConstructor : ExternalMethod, IConstructor, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IEntityWithParameters, IEntity
	{
		public override string Name => "constructor";

		public override EntityType EntityType => EntityType.Constructor;

		public ConstructorInfo ConstructorInfo => (ConstructorInfo)base.MethodInfo;

		public override IType ReturnType => _provider.Map(Types.Void);

		public ExternalConstructor(IReflectionTypeSystemProvider provider, ConstructorInfo ci)
			: base(provider, ci)
		{
		}
	}
}
