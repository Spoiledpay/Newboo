namespace antlr.debug
{
	public class SemanticPredicateEventArgs : GuessingEventArgs
	{
		public const int VALIDATING = 0;

		public const int PREDICTING = 1;

		private int condition_;

		private bool result_;

		public virtual int Condition
		{
			get
			{
				return condition_;
			}
			set
			{
				condition_ = value;
			}
		}

		public virtual bool Result
		{
			get
			{
				return result_;
			}
			set
			{
				result_ = value;
			}
		}

		public SemanticPredicateEventArgs()
		{
		}

		public SemanticPredicateEventArgs(int type)
			: base(type)
		{
		}

		internal void setValues(int type, int condition, bool result, int guessing)
		{
			base.setValues(type, guessing);
			Condition = condition;
			Result = result;
		}

		public override string ToString()
		{
			return "SemanticPredicateEvent [" + Condition + "," + Result + "," + Guessing + "]";
		}
	}
}
