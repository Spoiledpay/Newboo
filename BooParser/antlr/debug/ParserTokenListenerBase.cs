namespace antlr.debug
{
	public abstract class ParserTokenListenerBase : ParserTokenListener, Listener
	{
		public virtual void doneParsing(object source, TraceEventArgs e)
		{
		}

		public virtual void refresh()
		{
		}

		public virtual void parserConsume(object source, TokenEventArgs e)
		{
		}

		public virtual void parserLA(object source, TokenEventArgs e)
		{
		}
	}
}
