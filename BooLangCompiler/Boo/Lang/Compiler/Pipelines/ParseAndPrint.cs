using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class ParseAndPrint : Parse
	{
		public ParseAndPrint()
		{
			base.BreakOnErrors = false;
			Add(new PrintBoo());
		}
	}
}
