namespace antlr
{
	public class TreeParserSharedInputState
	{
		public int guessing = 0;

		public virtual void reset()
		{
			guessing = 0;
		}
	}
}
