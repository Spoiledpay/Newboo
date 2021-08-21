namespace antlr.debug
{
	public class TokenEventArgs : ANTLREventArgs
	{
		private int value_;

		private int amount;

		public static int LA = 0;

		public static int CONSUME = 1;

		public virtual int Amount
		{
			get
			{
				return amount;
			}
			set
			{
				amount = value;
			}
		}

		public virtual int Value
		{
			get
			{
				return value_;
			}
			set
			{
				value_ = value;
			}
		}

		public TokenEventArgs()
		{
		}

		public TokenEventArgs(int type, int amount, int val)
		{
			setValues(type, amount, val);
		}

		internal void setValues(int type, int amount, int val)
		{
			setValues(type);
			Amount = amount;
			Value = val;
		}

		public override string ToString()
		{
			if (Type == LA)
			{
				return "ParserTokenEvent [LA," + Amount + "," + Value + "]";
			}
			return "ParserTokenEvent [consume,1," + Value + "]";
		}
	}
}
