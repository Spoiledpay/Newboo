using antlr.collections.impl;

namespace antlr
{
	public class TokenStreamHiddenTokenFilter : TokenStreamBasicFilter, TokenStream
	{
		protected internal BitSet hideMask;

		private IHiddenStreamToken nextMonitoredToken;

		protected internal IHiddenStreamToken lastHiddenToken;

		protected internal IHiddenStreamToken firstHidden = null;

		public TokenStreamHiddenTokenFilter(TokenStream input)
			: base(input)
		{
			hideMask = new BitSet();
		}

		protected internal virtual void consume()
		{
			nextMonitoredToken = (IHiddenStreamToken)input.nextToken();
		}

		private void consumeFirst()
		{
			consume();
			IHiddenStreamToken hiddenStreamToken = null;
			while (hideMask.member(LA(1).Type) || discardMask.member(LA(1).Type))
			{
				if (hideMask.member(LA(1).Type))
				{
					if (hiddenStreamToken == null)
					{
						hiddenStreamToken = LA(1);
					}
					else
					{
						hiddenStreamToken.setHiddenAfter(LA(1));
						LA(1).setHiddenBefore(hiddenStreamToken);
						hiddenStreamToken = LA(1);
					}
					lastHiddenToken = hiddenStreamToken;
					if (firstHidden == null)
					{
						firstHidden = hiddenStreamToken;
					}
				}
				consume();
			}
		}

		public virtual BitSet getDiscardMask()
		{
			return discardMask;
		}

		public virtual IHiddenStreamToken getHiddenAfter(IHiddenStreamToken t)
		{
			return t.getHiddenAfter();
		}

		public virtual IHiddenStreamToken getHiddenBefore(IHiddenStreamToken t)
		{
			return t.getHiddenBefore();
		}

		public virtual BitSet getHideMask()
		{
			return hideMask;
		}

		public virtual IHiddenStreamToken getInitialHiddenToken()
		{
			return firstHidden;
		}

		public virtual void hide(int m)
		{
			hideMask.add(m);
		}

		public virtual void hide(BitSet mask)
		{
			hideMask = mask;
		}

		protected internal virtual IHiddenStreamToken LA(int i)
		{
			return nextMonitoredToken;
		}

		public override IToken nextToken()
		{
			if (LA(1) == null)
			{
				consumeFirst();
			}
			IHiddenStreamToken hiddenStreamToken = LA(1);
			hiddenStreamToken.setHiddenBefore(lastHiddenToken);
			lastHiddenToken = null;
			consume();
			IHiddenStreamToken hiddenStreamToken2 = hiddenStreamToken;
			while (hideMask.member(LA(1).Type) || discardMask.member(LA(1).Type))
			{
				if (hideMask.member(LA(1).Type))
				{
					hiddenStreamToken2.setHiddenAfter(LA(1));
					if (hiddenStreamToken2 != hiddenStreamToken)
					{
						LA(1).setHiddenBefore(hiddenStreamToken2);
					}
					hiddenStreamToken2 = (lastHiddenToken = LA(1));
				}
				consume();
			}
			return hiddenStreamToken;
		}

		public virtual void resetState()
		{
			firstHidden = null;
			lastHiddenToken = null;
			nextMonitoredToken = null;
		}
	}
}
