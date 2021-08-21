namespace antlr.debug
{
	public class SemanticPredicateListenerBase : SemanticPredicateListener, Listener
	{
		public virtual void doneParsing(object source, TraceEventArgs e)
		{
		}

		public virtual void refresh()
		{
		}

		public virtual void semanticPredicateEvaluated(object source, SemanticPredicateEventArgs e)
		{
		}
	}
}
