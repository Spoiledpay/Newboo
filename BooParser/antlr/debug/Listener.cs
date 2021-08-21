namespace antlr.debug
{
	public interface Listener
	{
		void doneParsing(object source, TraceEventArgs e);

		void refresh();
	}
}
