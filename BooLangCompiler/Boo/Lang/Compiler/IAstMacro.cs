using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public interface IAstMacro : ICompilerComponent
	{
		Statement Expand(MacroStatement statement);
	}
}
