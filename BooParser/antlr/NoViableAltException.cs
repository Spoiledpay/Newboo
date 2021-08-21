using System;
using antlr.collections;

namespace antlr
{
	[Serializable]
	public class NoViableAltException : RecognitionException
	{
		public IToken token;

		public AST node;

		public override string Message
		{
			get
			{
				if (token != null)
				{
					return "unexpected token: " + token.ToString();
				}
				if (node == null || node == TreeParser.ASTNULL)
				{
					return "unexpected end of subtree";
				}
				return "unexpected AST node: " + node.ToString();
			}
		}

		public NoViableAltException(AST t)
			: base("NoViableAlt", "<AST>", -1, -1)
		{
			node = t;
		}

		public NoViableAltException(IToken t, string fileName_)
			: base("NoViableAlt", fileName_, t.getLine(), t.getColumn())
		{
			token = t;
		}
	}
}
