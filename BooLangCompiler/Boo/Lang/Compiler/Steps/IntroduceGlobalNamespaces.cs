using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.Steps
{
	public class IntroduceGlobalNamespaces : AbstractCompilerStep
	{
		public override void Run()
		{
			base.NameResolutionService.Reset();
			base.NameResolutionService.GlobalNamespace = new NamespaceDelegator(base.NameResolutionService.GlobalNamespace, SafeGetNamespace("Boo.Lang"), SafeGetNamespace("Boo.Lang.Extensions"), base.TypeSystemServices.BuiltinsType);
		}

		protected INamespace SafeGetNamespace(string qname)
		{
			INamespace @namespace = (INamespace)base.NameResolutionService.ResolveQualifiedName(qname);
			return (@namespace == null) ? NullNamespace.Default : @namespace;
		}
	}
}
