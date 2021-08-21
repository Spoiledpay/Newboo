using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class ExpandAstLiterals : AbstractCompilerStep
	{
		public override void Run()
		{
			base.CompileUnit.Accept(new QuasiquoteExpander());
		}
	}
}
