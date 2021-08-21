using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class CompileToFileAndVerify : CompileToFile
	{
		public CompileToFileAndVerify()
		{
			Add(new PEVerify());
		}
	}
}
