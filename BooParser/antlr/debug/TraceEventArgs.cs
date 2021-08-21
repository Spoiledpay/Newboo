namespace antlr.debug
{
	public class TraceEventArgs : GuessingEventArgs
	{
		private int ruleNum_;

		private int data_;

		public static int ENTER = 0;

		public static int EXIT = 1;

		public static int DONE_PARSING = 2;

		public virtual int Data
		{
			get
			{
				return data_;
			}
			set
			{
				data_ = value;
			}
		}

		public virtual int RuleNum
		{
			get
			{
				return ruleNum_;
			}
			set
			{
				ruleNum_ = value;
			}
		}

		public TraceEventArgs()
		{
		}

		public TraceEventArgs(int type, int ruleNum, int guessing, int data)
		{
			setValues(type, ruleNum, guessing, data);
		}

		internal void setValues(int type, int ruleNum, int guessing, int data)
		{
			base.setValues(type, guessing);
			RuleNum = ruleNum;
			Data = data;
		}

		public override string ToString()
		{
			return "ParserTraceEvent [" + ((Type == ENTER) ? "enter," : "exit,") + RuleNum + "," + Guessing + "]";
		}
	}
}
