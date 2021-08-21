using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	[Serializable]
	public class CheckGenericConstraints : AbstractFastVisitorCompilerStep
	{
		public override void OnClassDefinition(ClassDefinition node)
		{
			CheckConstraints(node);
		}

		private void CheckConstraints(TypeDefinition node)
		{
			new GenericConstraintsValidator(base.Context, node, node.GenericParameters).Validate();
		}
	}
}
