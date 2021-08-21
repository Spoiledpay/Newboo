namespace antlr
{
	public class CommonHiddenStreamToken : CommonToken, IHiddenStreamToken, IToken
	{
		public class CommonHiddenStreamTokenCreator : TokenCreator
		{
			public override string TokenTypeName => typeof(CommonHiddenStreamToken).FullName;

			public override IToken Create()
			{
				return new CommonHiddenStreamToken();
			}
		}

		public new static readonly CommonHiddenStreamTokenCreator Creator = new CommonHiddenStreamTokenCreator();

		protected internal IHiddenStreamToken hiddenBefore;

		protected internal IHiddenStreamToken hiddenAfter;

		public CommonHiddenStreamToken()
		{
		}

		public CommonHiddenStreamToken(int t, string txt)
			: base(t, txt)
		{
		}

		public CommonHiddenStreamToken(string s)
			: base(s)
		{
		}

		public virtual IHiddenStreamToken getHiddenAfter()
		{
			return hiddenAfter;
		}

		public virtual IHiddenStreamToken getHiddenBefore()
		{
			return hiddenBefore;
		}

		public virtual void setHiddenAfter(IHiddenStreamToken t)
		{
			hiddenAfter = t;
		}

		public virtual void setHiddenBefore(IHiddenStreamToken t)
		{
			hiddenBefore = t;
		}
	}
}
