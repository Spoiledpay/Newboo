using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class CheckForErrors : ResolveExpressions
	{
		public CheckForErrors()
		{
			base.BreakOnErrors = false;
			Add(new CheckSlicingExpressions());
			Add(new StricterErrorChecking());
		}
	}
}
