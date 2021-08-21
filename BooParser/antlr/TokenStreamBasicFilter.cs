using antlr.collections.impl;

namespace antlr
{
	public class TokenStreamBasicFilter : TokenStream
	{
		protected internal BitSet discardMask;

		protected internal TokenStream input;

		public TokenStreamBasicFilter(TokenStream input)
		{
			this.input = input;
			discardMask = new BitSet();
		}

		public virtual void discard(int ttype)
		{
			discardMask.add(ttype);
		}

		public virtual void discard(BitSet mask)
		{
			discardMask = mask;
		}

		public virtual IToken nextToken()
		{
			IToken token = input.nextToken();
			while (token != null && discardMask.member(token.Type))
			{
				token = input.nextToken();
			}
			return token;
		}
	}
}
