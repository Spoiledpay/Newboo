using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class ParseAndPrintAst : Parse
	{
		public ParseAndPrintAst()
		{
			base.BreakOnErrors = false;
			Add(new PrintAst());
		}
	}
}
