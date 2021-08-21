using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class ResolveExpressions : ExpandMacros
	{
		public ResolveExpressions()
		{
			Add(new ExpandAstLiterals());
			Add(new IntroduceModuleClasses());
			Add(new NormalizeStatementModifiers());
			Add(new NormalizeTypeAndMemberDefinitions());
			Add(new NormalizeExpressions());
			Add(new BindTypeDefinitions());
			Add(new BindGenericParameters());
			Add(new BindEnumMembers());
			Add(new BindBaseTypes());
			Add(new CheckMemberTypes());
			Add(new BindMethods());
			Add(new ResolveTypeReferences());
			Add(new BindTypeMembers());
			Add(new CheckGenericConstraints());
			Add(new ProcessInheritedAbstractMembers());
			Add(new CheckMemberNames());
			Add(new ProcessMethodBodiesWithDuckTyping());
			Add(new ReifyTypes());
			Add(new TypeInference());
		}
	}
}
