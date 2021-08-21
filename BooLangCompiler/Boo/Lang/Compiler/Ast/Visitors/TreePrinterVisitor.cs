using System.IO;

namespace Boo.Lang.Compiler.Ast.Visitors
{
	public class TreePrinterVisitor : TextEmitter
	{
		public TreePrinterVisitor(TextWriter writer)
			: base(writer)
		{
		}

		public void Print(Node ast)
		{
			ast.Accept(this);
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			WriteIndented("MethodInvocationExpression(");
			BeginNode();
			WriteIndented("Target: ");
			Visit(node.Target);
			if (node.Arguments.Count > 0)
			{
				Write(", ");
				WriteLine();
				WriteIndented("Arguments: ");
				WriteArray(node.Arguments);
			}
			if (node.NamedArguments.Count > 0)
			{
				Write(", ");
				WriteLine();
				WriteIndented("NamedArguments: ");
				WriteArray(node.NamedArguments);
			}
			EndNode();
		}

		private void EndNode()
		{
			Dedent();
			Write(")");
		}

		private void BeginNode()
		{
			WriteLine();
			Indent();
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			Write("ReferenceExpression(");
			WriteString(node.Name);
			Write(")");
		}

		private void WriteString(string value)
		{
			BooPrinterVisitor.WriteStringLiteral(value, _writer);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			Write("MemberReferenceExpression(");
			BeginNode();
			WriteIndented("Target: ");
			Visit(node.Target);
			Write(", ");
			WriteLine();
			WriteIndented("Name: ");
			WriteString(node.Name);
			EndNode();
		}

		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			Write("StringLiteralExpression(");
			BooPrinterVisitor.WriteStringLiteral(node.Value, _writer);
			Write(")");
		}
	}
}
