using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class Compile : ResolveExpressions
	{
		public Compile()
		{
			Add(new InjectImplicitBooleanConversions());
			Add(new ConstantFolding());
			Add(new CheckLiteralValues());
			Add(new OptimizeIterationStatements());
			Add(new BranchChecking());
			Add(new VerifyExtensionMethods());
			Add(new CheckIdentifiers());
			Add(new CheckSlicingExpressions());
			Add(new StricterErrorChecking());
			Add(new DetectNotImplementedFeatureUsage());
			Add(new CheckAttributesUsage());
			Add(new ExpandDuckTypedExpressions());
			Add(new ExpandComplexSlicingExpressions());
			Add(new ProcessAssignmentsToValueTypeMembers());
			Add(new ExpandPropertiesAndEvents());
			Add(new CheckMembersProtectionLevel());
			Add(new NormalizeIterationStatements());
			Add(new ProcessSharedLocals());
			Add(new ProcessClosures());
			Add(new ProcessGenerators());
			Add(new ExpandVarArgsMethodInvocations());
			Add(new InjectCallableConversions());
			Add(new ImplementICallableOnCallableDefinitions());
			Add(new RemoveDeadCode());
			Add(new CheckNeverUsedMembers());
			Add(new CacheRegularExpressionsInStaticFields());
		}
	}
}
