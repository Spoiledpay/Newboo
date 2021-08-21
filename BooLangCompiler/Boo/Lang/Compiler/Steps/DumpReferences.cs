using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class DumpReferences : AbstractFastVisitorCompilerStep
	{
		public override void OnReferenceExpression(ReferenceExpression node)
		{
			Console.WriteLine("{0}: '{1}': {2}", node.LexicalInfo, node.Name, node.Entity);
			Console.WriteLine("{0}: '{1}': {2}", node.LexicalInfo, node.Name, node.ExpressionType);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.OnMemberReferenceExpression(node);
			OnReferenceExpression(node);
		}
	}
}
