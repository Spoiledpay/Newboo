using Boo.Lang.Compiler.Ast.Visitors;

namespace Boo.Lang.Compiler.Steps
{
	public class PrintAst : AbstractCompilerStep
	{
		public override void Run()
		{
			TreePrinterVisitor treePrinterVisitor = new TreePrinterVisitor(base.OutputWriter);
			treePrinterVisitor.Print(base.CompileUnit);
		}
	}
}
