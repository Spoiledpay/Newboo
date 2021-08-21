using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public abstract class AbstractAstMacro : AbstractCompilerComponent, IAstMacro, ICompilerComponent
	{
		protected AbstractAstMacro()
		{
		}

		protected AbstractAstMacro(CompilerContext context)
			: base(context)
		{
		}

		public abstract Statement Expand(MacroStatement macro);
	}
}
