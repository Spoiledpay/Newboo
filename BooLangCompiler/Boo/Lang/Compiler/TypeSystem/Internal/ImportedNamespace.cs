using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class ImportedNamespace : AbstractNamespace
	{
		private readonly Import _import;

		private readonly INamespace _namespace;

		public override string FullName => _import.Namespace;

		public ImportedNamespace(Import import, INamespace @namespace)
		{
			_import = import;
			_namespace = @namespace;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (_namespace.Resolve(resultingSet, name, typesToConsider))
			{
				ImportAnnotations.MarkAsUsed(_import);
				return true;
			}
			return false;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			return _namespace.GetMembers();
		}
	}
}
