namespace antlr.debug
{
	public class InputBufferEventArgs : ANTLREventArgs
	{
		public const int CONSUME = 0;

		public const int LA = 1;

		public const int MARK = 2;

		public const int REWIND = 3;

		internal char c_;

		internal int lookaheadAmount_;

		public virtual char Char
		{
			get
			{
				return c_;
			}
			set
			{
				c_ = value;
			}
		}

		public virtual int LookaheadAmount
		{
			get
			{
				return lookaheadAmount_;
			}
			set
			{
				lookaheadAmount_ = value;
			}
		}

		public InputBufferEventArgs()
		{
		}

		public InputBufferEventArgs(int type, char c, int lookaheadAmount)
		{
			setValues(type, c, lookaheadAmount);
		}

		internal void setValues(int type, char c, int la)
		{
			setValues(type);
			Char = c;
			LookaheadAmount = la;
		}

		public override string ToString()
		{
			return "CharBufferEvent [" + ((Type == 0) ? "CONSUME, " : "LA, ") + Char + "," + LookaheadAmount + "]";
		}
	}
}
