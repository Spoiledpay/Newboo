using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public abstract class LexicalInfoPreservingGeneratorMacro : AbstractAstGeneratorMacro
	{
		protected LexicalInfoPreservingGeneratorMacro()
		{
		}

		protected LexicalInfoPreservingGeneratorMacro(CompilerContext context)
			: base(context)
		{
		}

		public override Statement Expand(MacroStatement macro)
		{
			return ExpandImpl(macro);
		}

		protected virtual Statement ExpandImpl(MacroStatement macro)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Node> ExpandGenerator(MacroStatement macro)
		{
			IEnumerable<Node> nodes = ExpandGeneratorImpl(macro);
			if (null == nodes)
			{
				yield break;
			}
			foreach (Node i in nodes)
			{
				if (!IsEmptyBlock(i))
				{
					if (null != i)
					{
						i.LexicalInfo = macro.LexicalInfo;
					}
					yield return i;
				}
			}
		}

		private bool IsEmptyBlock(Node node)
		{
			Block block = node as Block;
			if (null == block)
			{
				return false;
			}
			return block.IsEmpty;
		}

		protected abstract IEnumerable<Node> ExpandGeneratorImpl(MacroStatement macro);
	}
}
