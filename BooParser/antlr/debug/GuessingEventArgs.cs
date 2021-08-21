namespace antlr.debug
{
	public abstract class GuessingEventArgs : ANTLREventArgs
	{
		private int guessing_;

		public virtual int Guessing
		{
			get
			{
				return guessing_;
			}
			set
			{
				guessing_ = value;
			}
		}

		public GuessingEventArgs()
		{
		}

		public GuessingEventArgs(int type)
			: base(type)
		{
		}

		public virtual void setValues(int type, int guessing)
		{
			setValues(type);
			Guessing = guessing;
		}
	}
}
