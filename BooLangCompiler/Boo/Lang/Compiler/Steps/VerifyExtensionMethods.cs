using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class VerifyExtensionMethods : AbstractFastVisitorCompilerStep
	{
		public override void OnConstructor(Constructor node)
		{
			CheckExtensionSemantics(node);
		}

		public override void OnDestructor(Destructor node)
		{
			CheckExtensionSemantics(node);
		}

		public override void OnProperty(Property node)
		{
		}

		public override void OnField(Field node)
		{
		}

		public override void OnMethod(Method node)
		{
			CheckExtensionSemantics(node);
		}

		private void CheckExtensionSemantics(Method node)
		{
			if (((IMethod)node.Entity).IsExtension && (NodeType.Method != node.NodeType || (!node.IsStatic && !(node.DeclaringType is Module)) || node.Parameters.Count == 0))
			{
				base.Errors.Add(CompilerErrorFactory.InvalidExtensionDefinition(node));
			}
		}
	}
}
