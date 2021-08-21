namespace antlr
{
	public class CommonToken : Token
	{
		public class CommonTokenCreator : TokenCreator
		{
			public override string TokenTypeName => typeof(CommonToken).FullName;

			public override IToken Create()
			{
				return new CommonToken();
			}
		}

		public static readonly CommonTokenCreator Creator = new CommonTokenCreator();

		protected internal int line;

		protected internal string text = null;

		protected internal int col;

		public CommonToken()
		{
		}

		public CommonToken(int t, string txt)
		{
			type_ = t;
			setText(txt);
		}

		public CommonToken(string s)
		{
			text = s;
		}

		public override int getLine()
		{
			return line;
		}

		public override string getText()
		{
			return text;
		}

		public override void setLine(int l)
		{
			line = l;
		}

		public override void setText(string s)
		{
			text = s;
		}

		public override string ToString()
		{
			return "[\"" + getText() + "\",<" + type_ + ">,line=" + line + ",col=" + col + "]";
		}

		public override int getColumn()
		{
			return col;
		}

		public override void setColumn(int c)
		{
			col = c;
		}
	}
}
