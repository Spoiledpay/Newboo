using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class CompileToFile : CompileToMemory
	{
		public CompileToFile()
		{
			Add(new SaveAssembly());
		}
	}
}
