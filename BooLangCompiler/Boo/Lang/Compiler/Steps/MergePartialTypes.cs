using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class MergePartialTypes : AbstractTransformerCompilerStep
	{
		private Dictionary<string, TypeDefinition> _partials = new Dictionary<string, TypeDefinition>();

		public override void Run()
		{
			Visit(base.CompileUnit.Modules);
		}

		public override void Dispose()
		{
			base.Dispose();
			_partials.Clear();
		}

		public override void OnModule(Module node)
		{
			Visit(node.Members);
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			OnCandidatePartialDefinition(node);
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			OnCandidatePartialDefinition(node);
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			OnCandidatePartialDefinition(node);
		}

		private void OnCandidatePartialDefinition(TypeDefinition node)
		{
			if (!node.IsPartial)
			{
				return;
			}
			string fullName = node.FullName;
			if (_partials.TryGetValue(fullName, out var value))
			{
				if (node != value)
				{
					if (value.NodeType != node.NodeType)
					{
						base.Errors.Add(CompilerErrorFactory.IncompatiblePartialDefinition(node, fullName, AstUtil.TypeKeywordFor(value), AstUtil.TypeKeywordFor(node)));
						return;
					}
					MergeImports(node, value);
					value.Merge(node);
					RemoveCurrentNode();
				}
			}
			else
			{
				_partials[fullName] = node;
			}
		}

		private static void MergeImports(TypeDefinition from, TypeDefinition to)
		{
			Module enclosingModule = from.EnclosingModule;
			Module enclosingModule2 = to.EnclosingModule;
			if (enclosingModule != enclosingModule2 && !enclosingModule2.ContainsAnnotation(enclosingModule))
			{
				enclosingModule2.Imports.ExtendWithClones(enclosingModule.Imports);
				enclosingModule2.Annotate(enclosingModule);
				if (!enclosingModule.ContainsAnnotation("merged-module"))
				{
					enclosingModule.Annotate("merged-module");
				}
				if (!enclosingModule2.ContainsAnnotation("merged-module"))
				{
					enclosingModule2.Annotate("merged-module");
				}
			}
		}
	}
}
