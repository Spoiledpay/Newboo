namespace antlr.debug
{
	public abstract class TraceListenerBase : TraceListener, Listener
	{
		public virtual void doneParsing(object source, TraceEventArgs e)
		{
		}

		public virtual void enterRule(object source, TraceEventArgs e)
		{
		}

		public virtual void exitRule(object source, TraceEventArgs e)
		{
		}

		public virtual void refresh()
		{
		}
	}
}
