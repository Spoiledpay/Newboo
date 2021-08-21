namespace antlr.debug
{
	public interface NewLineListener : Listener
	{
		void hitNewLine(object source, NewLineEventArgs e);
	}
}
