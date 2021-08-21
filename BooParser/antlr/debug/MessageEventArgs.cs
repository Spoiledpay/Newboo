namespace antlr.debug
{
	public class MessageEventArgs : ANTLREventArgs
	{
		private string text_;

		public static int WARNING = 0;

		public static int ERROR = 1;

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

		public MessageEventArgs()
		{
		}

		public MessageEventArgs(int type, string text)
		{
			setValues(type, text);
		}

		internal void setValues(int type, string text)
		{
			setValues(type);
			Text = text;
		}

		public override string ToString()
		{
			return "ParserMessageEvent [" + ((Type == WARNING) ? "warning," : "error,") + Text + "]";
		}
	}
}
