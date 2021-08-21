namespace antlr
{
	internal class TokenQueue
	{
		private IToken[] buffer;

		private int sizeLessOne;

		private int offset;

		protected internal int nbrEntries;

		public TokenQueue(int minSize)
		{
			if (minSize < 0)
			{
				init(16);
				return;
			}
			if (minSize >= 1073741823)
			{
				init(int.MaxValue);
				return;
			}
			int num;
			for (num = 2; num < minSize; num *= 2)
			{
			}
			init(num);
		}

		public void append(IToken tok)
		{
			if (nbrEntries == buffer.Length)
			{
				expand();
			}
			buffer[(offset + nbrEntries) & sizeLessOne] = tok;
			nbrEntries++;
		}

		public IToken elementAt(int idx)
		{
			return buffer[(offset + idx) & sizeLessOne];
		}

		private void expand()
		{
			IToken[] array = new IToken[buffer.Length * 2];
			for (int i = 0; i < buffer.Length; i++)
			{
				array[i] = elementAt(i);
			}
			buffer = array;
			sizeLessOne = buffer.Length - 1;
			offset = 0;
		}

		private void init(int size)
		{
			buffer = new IToken[size];
			sizeLessOne = size - 1;
			offset = 0;
			nbrEntries = 0;
		}

		public void reset()
		{
			offset = 0;
			nbrEntries = 0;
		}

		public void removeFirst()
		{
			offset = (offset + 1) & sizeLessOne;
			nbrEntries--;
		}
	}
}
