using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class GlobalNamespace : AbstractNamespace
	{
		private readonly IEnumerable<ICompileUnit> _references;

		private readonly ICompileUnit _compileUnit;

		public GlobalNamespace()
		{
			InternalTypeSystemProvider instance = My<InternalTypeSystemProvider>.Instance;
			_compileUnit = instance.EntityFor(My<CompileUnit>.Instance);
			_references = My<CompilerParameters>.Instance.References;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return Namespaces.ResolveCoalescingNamespaces(this, RootNamespaces(), name, typesToConsider, resultingSet);
		}

		private IEnumerable<INamespace> RootNamespaces()
		{
			foreach (ICompileUnit reference in _references)
			{
				yield return reference.RootNamespace;
			}
			yield return _compileUnit.RootNamespace;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			foreach (INamespace root in RootNamespaces())
			{
				foreach (IEntity member in root.GetMembers())
				{
					yield return member;
				}
			}
		}
	}
}
