using antlr;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Parser
{
	public class SourceLocationFactory
	{
		public static LexicalInfo ToLexicalInfo(IToken token)
		{
			return new LexicalInfo(token.getFilename(), token.getLine(), token.getColumn());
		}

		public static SourceLocation ToSourceLocation(IToken token)
		{
			return new SourceLocation(token.getLine(), token.getColumn());
		}

		public static SourceLocation ToEndSourceLocation(IToken token)
		{
			string text = token.getText() ?? "";
			return new SourceLocation(token.getLine(), token.getColumn() + text.Length - 1);
		}
	}
}
