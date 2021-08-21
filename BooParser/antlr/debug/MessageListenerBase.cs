namespace antlr.debug
{
	public class MessageListenerBase : MessageListener, Listener
	{
		public virtual void doneParsing(object source, TraceEventArgs e)
		{
		}

		public virtual void refresh()
		{
		}

		public virtual void reportError(object source, MessageEventArgs e)
		{
		}

		public virtual void reportWarning(object source, MessageEventArgs e)
		{
		}
	}
}
