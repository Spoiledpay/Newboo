using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class DeclarationsNamespace : AbstractNamespace
	{
		private readonly INamespace _parent;

		private readonly DeclarationCollection _declarations;

		public override INamespace ParentNamespace => _parent;

		public DeclarationsNamespace(INamespace parent, DeclarationCollection declarations)
		{
			_parent = parent;
			_declarations = declarations;
		}

		public DeclarationsNamespace(INamespace parent, Declaration declaration)
		{
			_parent = parent;
			_declarations = new DeclarationCollection { declaration };
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			return _declarations.Select((Declaration d) => TypeSystemServices.GetEntity(d));
		}
	}
}
