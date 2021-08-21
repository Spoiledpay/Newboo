using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class Parse : CompilerPipeline
	{
		public Parse()
		{
			Add(new Parsing());
		}
	}
}
