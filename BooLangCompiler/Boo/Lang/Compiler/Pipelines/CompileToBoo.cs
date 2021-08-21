using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class CompileToBoo : Compile
	{
		public CompileToBoo()
		{
			base.BreakOnErrors = false;
			Add(new PrintBoo());
		}
	}
}
