namespace antlr.debug
{
	public class MatchEventArgs : GuessingEventArgs
	{
		public enum ParserMatchEnums
		{
			TOKEN,
			BITSET,
			CHAR,
			CHAR_BITSET,
			STRING,
			CHAR_RANGE
		}

		public static int TOKEN = 0;

		public static int BITSET = 1;

		public static int CHAR = 2;

		public static int CHAR_BITSET = 3;

		public static int STRING = 4;

		public static int CHAR_RANGE = 5;

		private bool inverse_;

		private bool matched_;

		private object target_;

		private int val_;

		private string text_;

		public virtual object Target
		{
			get
			{
				return target_;
			}
			set
			{
				target_ = value;
			}
		}

		public virtual string Text
		{
			get
			{
				return text_;
			}
			set
			{
				text_ = value;
			}
		}

		public virtual int Value
		{
			get
			{
				return val_;
			}
			set
			{
				val_ = value;
			}
		}

		internal bool Inverse
		{
			set
			{
				inverse_ = value;
			}
		}

		internal bool Matched
		{
			set
			{
				matched_ = value;
			}
		}

		public MatchEventArgs()
		{
		}

		public MatchEventArgs(int type, int val, object target, string text, int guessing, bool inverse, bool matched)
		{
			setValues(type, val, target, text, guessing, inverse, matched);
		}

		public virtual bool isInverse()
		{
			return inverse_;
		}

		public virtual bool isMatched()
		{
			return matched_;
		}

		internal void setValues(int type, int val, object target, string text, int guessing, bool inverse, bool matched)
		{
			base.setValues(type, guessing);
			Value = val;
			Target = target;
			Inverse = inverse;
			Matched = matched;
			Text = text;
		}

		public override string ToString()
		{
			return string.Concat("ParserMatchEvent [", isMatched() ? "ok," : "bad,", isInverse() ? "NOT " : "", (Type == TOKEN) ? "token," : "bitset,", Value, ",", Target, ",", Guessing, "]");
		}
	}
}
