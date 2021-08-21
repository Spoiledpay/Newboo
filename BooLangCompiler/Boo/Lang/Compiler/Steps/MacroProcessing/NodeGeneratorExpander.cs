using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	internal sealed class NodeGeneratorExpander
	{
		private readonly MacroStatement _node;

		private readonly StatementTypeMember _typeMemberPrototype;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService = default(EnvironmentProvision<NameResolutionService>);

		private NameResolutionService NameResolutionService => _nameResolutionService;

		public NodeGeneratorExpander(MacroStatement node)
		{
			_node = node;
			_typeMemberPrototype = node.ParentNode as StatementTypeMember;
		}

		public Statement Expand(IEnumerable<Node> generator)
		{
			Block block = new Block();
			foreach (Node item in generator)
			{
				Node node = item ?? _node.Body;
				if (null == node)
				{
					continue;
				}
				TypeMember typeMember = node as TypeMember;
				if (null != typeMember)
				{
					ExpandTypeMember(typeMember, block);
					continue;
				}
				Block block2 = node as Block;
				if (null != block2)
				{
					block.Add(block2);
					continue;
				}
				Statement statement = node as Statement;
				if (null != statement)
				{
					block.Add(statement);
					continue;
				}
				Expression expression = node as Expression;
				if (null != expression)
				{
					block.Add(expression);
					continue;
				}
				Import import = node as Import;
				if (null != import)
				{
					ExpandImport(import);
					continue;
				}
				throw new CompilerError(_node, "Unsupported expansion: " + node.ToCodeString());
			}
			return block.IsEmpty ? null : block.Simplify();
		}

		private void ExpandTypeMember(TypeMember member, Block resultingBlock)
		{
			ApplyPrototypeModifiersAndAttributesTo(member);
			resultingBlock.Add(new TypeMemberStatement(member)
			{
				InsertionPoint = _typeMemberPrototype
			});
		}

		private void ApplyPrototypeModifiersAndAttributesTo(TypeMember member)
		{
			if (null != _typeMemberPrototype)
			{
				member.Attributes.ExtendWithClones(_typeMemberPrototype.Attributes);
				if (member.IsVisibilitySet)
				{
					member.Modifiers |= _typeMemberPrototype.Modifiers & ~TypeMemberModifiers.VisibilityMask;
				}
				else
				{
					member.Modifiers |= _typeMemberPrototype.Modifiers;
				}
			}
		}

		private void ExpandImport(Import import)
		{
			ImportCollection imports = _node.GetAncestor<Module>().Imports;
			if (!imports.Contains(import.Matches))
			{
				imports.Add(import);
				BindImport(import);
			}
		}

		private void BindImport(Import import)
		{
			INamespace currentNamespace = NameResolutionService.CurrentNamespace;
			try
			{
				NameResolutionService.Reset();
				ResolveImports resolveImports = new ResolveImports();
				resolveImports.Initialize(CompilerContext.Current);
				import.Accept(resolveImports);
			}
			catch (Exception cause)
			{
				throw new CompilerError(_node, "Error expanding " + import.ToCodeString(), cause);
			}
			finally
			{
				NameResolutionService.EnterNamespace(currentNamespace);
			}
		}
	}
}
