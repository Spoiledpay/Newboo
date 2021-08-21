namespace antlr.debug
{
	public abstract class SyntacticPredicateListenerBase : SyntacticPredicateListener, Listener
	{
		public virtual void doneParsing(object source, TraceEventArgs e)
		{
		}

		public virtual void refresh()
		{
		}

		public virtual void syntacticPredicateFailed(object source, SyntacticPredicateEventArgs e)
		{
		}

		public virtual void syntacticPredicateStarted(object source, SyntacticPredicateEventArgs e)
		{
		}

		public virtual void syntacticPredicateSucceeded(object source, SyntacticPredicateEventArgs e)
		{
		}
	}
}
