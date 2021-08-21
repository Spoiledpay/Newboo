using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class ModuleMembersNamespace : AbstractNamespace
	{
		private readonly InternalModule _module;

		public override INamespace ParentNamespace => _module.ParentNamespace;

		public ModuleMembersNamespace(InternalModule module)
		{
			_module = module;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return _module.ResolveMember(resultingSet, name, typesToConsider);
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			return _module.GetMembers();
		}
	}
}
