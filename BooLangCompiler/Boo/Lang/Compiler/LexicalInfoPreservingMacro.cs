using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public abstract class LexicalInfoPreservingMacro : AbstractAstMacro
	{
		protected LexicalInfoPreservingMacro()
		{
		}

		protected LexicalInfoPreservingMacro(CompilerContext context)
			: base(context)
		{
		}

		public override Statement Expand(MacroStatement macro)
		{
			Statement statement = ExpandImpl(macro);
			if (statement != null)
			{
				statement.LexicalInfo = macro.LexicalInfo;
			}
			return statement;
		}

		protected abstract Statement ExpandImpl(MacroStatement macro);
	}
}
