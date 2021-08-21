using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public abstract class AbstractNamespace : INamespace, IEntity
	{
		public virtual string Name => string.Empty;

		public virtual string FullName
		{
			get
			{
				INamespace parentNamespace = ParentNamespace;
				if (parentNamespace == null)
				{
					return Name;
				}
				string fullName = parentNamespace.FullName;
				return string.IsNullOrEmpty(fullName) ? Name : (fullName + "." + Name);
			}
		}

		public virtual EntityType EntityType => EntityType.Namespace;

		public virtual INamespace ParentNamespace => null;

		public virtual bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return My<NameResolutionService>.Instance.Resolve(name, GetMembers(), typesToConsider, resultingSet);
		}

		public abstract IEnumerable<IEntity> GetMembers();

		public override string ToString()
		{
			return FullName;
		}
	}
}
