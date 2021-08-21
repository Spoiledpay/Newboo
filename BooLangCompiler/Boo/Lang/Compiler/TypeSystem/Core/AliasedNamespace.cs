using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class AliasedNamespace : INamespace, IEntity
	{
		private string _alias;

		private IEntity _subject;

		public string Name => _alias;

		public string FullName => _subject.FullName;

		public EntityType EntityType => EntityType.Namespace;

		public INamespace ParentNamespace => ((INamespace)_subject).ParentNamespace;

		public AliasedNamespace(string alias, IEntity subject)
		{
			_alias = alias;
			_subject = subject;
		}

		public bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (name == _alias && Entities.IsFlagSet(typesToConsider, _subject.EntityType))
			{
				resultingSet.Add(_subject);
				return true;
			}
			return false;
		}

		public IEnumerable<IEntity> GetMembers()
		{
			return ((INamespace)_subject).GetMembers();
		}
	}
}
