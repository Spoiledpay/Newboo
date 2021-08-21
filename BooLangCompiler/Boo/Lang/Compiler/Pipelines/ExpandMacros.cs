using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class ExpandMacros : Parse
	{
		public ExpandMacros()
		{
			Add(new PreErrorChecking());
			MergePartialTypes step = new MergePartialTypes();
			Add(step);
			Add(new InitializeNameResolutionService());
			Add(new IntroduceGlobalNamespaces());
			Add(new TransformCallableDefinitions());
			Add(new BindTypeDefinitions());
			Add(new BindGenericParameters());
			Add(new ResolveImports());
			Add(new BindBaseTypes());
			Add(new MacroAndAttributeExpansion());
			Add(new RemoveEmptyBlocks());
			Add(step);
		}
	}
}
