using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public abstract class AbstractAstGeneratorMacro : AbstractCompilerComponent, IAstMacro, IAstGeneratorMacro, ICompilerComponent
	{
		protected AbstractAstGeneratorMacro()
		{
		}

		protected AbstractAstGeneratorMacro(CompilerContext context)
			: base(context)
		{
		}

		public abstract Statement Expand(MacroStatement macro);

		public abstract IEnumerable<Node> ExpandGenerator(MacroStatement macro);
	}
}
