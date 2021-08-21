using System;

namespace Boo.Lang.Compiler.Ast
{
	public class PreservingLexicalInfoMacro : AbstractAstMacro
	{
		public override Statement Expand(MacroStatement macro)
		{
			if (macro.Arguments.Count > 0)
			{
				throw new ArgumentException("preservingLexicalInfo doesn't take any arguments");
			}
			macro.Body.Annotate(QuasiquoteExpander.PreserveLexicalInfoAnnotation);
			return macro.Body;
		}
	}
}
