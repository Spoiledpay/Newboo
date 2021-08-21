namespace Boo.Lang.Compiler
{
	public class CompilerStepEventArgs : CompilerPipelineEventArgs
	{
		public readonly ICompilerStep Step;

		public CompilerStepEventArgs(CompilerContext context, ICompilerStep step)
			: base(context)
		{
			Step = step;
		}
	}
}
