using System;

namespace antlr
{
	public class LLkParser : Parser
	{
		internal int k;

		public LLkParser(int k_)
		{
			k = k_;
		}

		public LLkParser(ParserSharedInputState state, int k_)
		{
			k = k_;
			inputState = state;
		}

		public LLkParser(TokenBuffer tokenBuf, int k_)
		{
			k = k_;
			setTokenBuffer(tokenBuf);
		}

		public LLkParser(TokenStream lexer, int k_)
		{
			k = k_;
			TokenBuffer tokenBuffer = new TokenBuffer(lexer);
			setTokenBuffer(tokenBuffer);
		}

		public override void consume()
		{
			inputState.input.consume();
		}

		public override int LA(int i)
		{
			return inputState.input.LA(i);
		}

		public override IToken LT(int i)
		{
			return inputState.input.LT(i);
		}

		private void trace(string ee, string rname)
		{
			traceIndent();
			Console.Out.Write(ee + rname + ((inputState.guessing > 0) ? "; [guessing]" : "; "));
			for (int i = 1; i <= k; i++)
			{
				if (i != 1)
				{
					Console.Out.Write(", ");
				}
				if (LT(i) != null)
				{
					Console.Out.Write("LA(" + i + ")==" + LT(i).getText());
				}
				else
				{
					Console.Out.Write("LA(" + i + ")==ull");
				}
			}
			Console.Out.WriteLine("");
		}

		public override void traceIn(string rname)
		{
			traceDepth++;
			trace("> ", rname);
		}

		public override void traceOut(string rname)
		{
			trace("< ", rname);
			traceDepth--;
		}
	}
}
