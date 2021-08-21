namespace antlr
{
	public class TokenWithIndex : CommonToken
	{
		private int index;

		public TokenWithIndex()
		{
		}

		public TokenWithIndex(int i, string t)
			: base(i, t)
		{
		}

		public void setIndex(int i)
		{
			index = i;
		}

		public int getIndex()
		{
			return index;
		}

		public override string ToString()
		{
			return "[" + index + ":\"" + getText() + "\",<" + base.Type + ">,line=" + line + ",col=" + col + "]\n";
		}
	}
}
