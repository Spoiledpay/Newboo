using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractNamespaceSensitiveTransformerCompilerStep : AbstractTransformerCompilerStep
	{
		protected INamespace CurrentNamespace => base.NameResolutionService.CurrentNamespace;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			base.NameResolutionService.Reset();
		}

		protected void EnterNamespace(INamespace ns)
		{
			base.NameResolutionService.EnterNamespace(ns);
		}

		protected void LeaveNamespace()
		{
			base.NameResolutionService.LeaveNamespace();
		}
	}
}
