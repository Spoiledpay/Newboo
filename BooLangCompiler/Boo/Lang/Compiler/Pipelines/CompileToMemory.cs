using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class CompileToMemory : Compile
	{
		public CompileToMemory()
		{
			Add(new EmitAssembly());
		}
	}
}
