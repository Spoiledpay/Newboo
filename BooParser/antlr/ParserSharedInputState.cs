namespace antlr
{
	public class ParserSharedInputState
	{
		protected internal TokenBuffer input;

		public int guessing = 0;

		protected internal string filename;

		public virtual void reset()
		{
			guessing = 0;
			filename = null;
			input.reset();
		}
	}
}
