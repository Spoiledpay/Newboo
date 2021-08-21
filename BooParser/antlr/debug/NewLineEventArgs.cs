namespace antlr.debug
{
	public class NewLineEventArgs : ANTLREventArgs
	{
		private int line_;

		public virtual int Line
		{
			get
			{
				return line_;
			}
			set
			{
				line_ = value;
			}
		}

		public NewLineEventArgs()
		{
		}

		public NewLineEventArgs(int line)
		{
			Line = line;
		}

		public override string ToString()
		{
			return "NewLineEvent [" + line_ + "]";
		}
	}
}
