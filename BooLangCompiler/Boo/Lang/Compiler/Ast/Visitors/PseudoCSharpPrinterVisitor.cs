using System.IO;

namespace Boo.Lang.Compiler.Ast.Visitors
{
	public class PseudoCSharpPrinterVisitor : TextEmitter
	{
		public PseudoCSharpPrinterVisitor(TextWriter writer)
			: base(writer)
		{
		}

		public void Print(Node node)
		{
			node.Accept(this);
		}
	}
}
