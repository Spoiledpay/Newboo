using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang
{
	public class StrictModeAttribute : AbstractAstAttribute
	{
		public override void Apply(Node targetNode)
		{
			if (!(targetNode is CompileUnit))
			{
				base.Context.Warnings.Add(CompilerWarningFactory.CustomWarning(base.LexicalInfo, "Use [assembly: StrictMode]"));
			}
			base.Parameters.Strict = true;
		}
	}
}
