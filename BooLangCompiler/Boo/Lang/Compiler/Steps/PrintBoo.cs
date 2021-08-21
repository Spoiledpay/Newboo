using Boo.Lang.Compiler.Ast.Visitors;

namespace Boo.Lang.Compiler.Steps
{
	public class PrintBoo : AbstractCompilerStep
	{
		public override void Run()
		{
			BooPrinterVisitor booPrinterVisitor = new BooPrinterVisitor(base.OutputWriter);
			if (base.Parameters.WhiteSpaceAgnostic)
			{
				booPrinterVisitor.Options |= BooPrinterVisitor.PrintOptions.WSA;
			}
			booPrinterVisitor.Print(base.CompileUnit);
		}
	}
}
