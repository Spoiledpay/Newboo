using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public interface IAstGeneratorMacro : ICompilerComponent
	{
		IEnumerable<Node> ExpandGenerator(MacroStatement statement);
	}
}
