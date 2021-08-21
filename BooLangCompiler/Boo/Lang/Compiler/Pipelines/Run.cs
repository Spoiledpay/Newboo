using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class Run : CompileToMemory
	{
		public Run()
		{
			Add(new RunAssembly());
		}

		protected override void Prepare(CompilerContext context)
		{
			base.Prepare(context);
			context.Parameters.GenerateInMemory = true;
		}
	}
}
