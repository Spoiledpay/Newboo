using System.Collections;
using antlr.collections.impl;

namespace antlr.debug
{
	public class ParseTreeDebugParser : LLkParser
	{
		protected Stack currentParseTreeRoot = new Stack();

		protected ParseTreeRule mostRecentParseTreeRoot = null;

		protected int numberOfDerivationSteps = 1;

		public ParseTreeDebugParser(int k_)
			: base(k_)
		{
		}

		public ParseTreeDebugParser(ParserSharedInputState state, int k_)
			: base(state, k_)
		{
		}

		public ParseTreeDebugParser(TokenBuffer tokenBuf, int k_)
			: base(tokenBuf, k_)
		{
		}

		public ParseTreeDebugParser(TokenStream lexer, int k_)
			: base(lexer, k_)
		{
		}

		public ParseTree getParseTree()
		{
			return mostRecentParseTreeRoot;
		}

		public int getNumberOfDerivationSteps()
		{
			return numberOfDerivationSteps;
		}

		public override void match(int i)
		{
			addCurrentTokenToParseTree();
			base.match(i);
		}

		public override void match(BitSet bitSet)
		{
			addCurrentTokenToParseTree();
			base.match(bitSet);
		}

		public override void matchNot(int i)
		{
			addCurrentTokenToParseTree();
			base.matchNot(i);
		}

		protected void addCurrentTokenToParseTree()
		{
			if (inputState.guessing <= 0)
			{
				ParseTreeRule parseTreeRule = (ParseTreeRule)currentParseTreeRoot.Peek();
				ParseTreeToken parseTreeToken = null;
				parseTreeToken = ((LA(1) != 1) ? new ParseTreeToken(LT(1)) : new ParseTreeToken(new CommonToken("EOF")));
				parseTreeRule.addChild(parseTreeToken);
			}
		}

		public override void traceIn(string s)
		{
			if (inputState.guessing <= 0)
			{
				ParseTreeRule parseTreeRule = new ParseTreeRule(s);
				if (currentParseTreeRoot.Count > 0)
				{
					ParseTreeRule parseTreeRule2 = (ParseTreeRule)currentParseTreeRoot.Peek();
					parseTreeRule2.addChild(parseTreeRule);
				}
				currentParseTreeRoot.Push(parseTreeRule);
				numberOfDerivationSteps++;
			}
		}

		public override void traceOut(string s)
		{
			if (inputState.guessing <= 0)
			{
				mostRecentParseTreeRoot = (ParseTreeRule)currentParseTreeRoot.Pop();
			}
		}
	}
}
