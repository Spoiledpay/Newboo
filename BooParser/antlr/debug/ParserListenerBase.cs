namespace antlr.debug
{
	public class ParserListenerBase : ParserListener, SemanticPredicateListener, ParserMatchListener, MessageListener, ParserTokenListener, TraceListener, SyntacticPredicateListener, Listener
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

		public virtual void parserConsume(object source, TokenEventArgs e)
		{
		}

		public virtual void parserLA(object source, TokenEventArgs e)
		{
		}

		public virtual void parserMatch(object source, MatchEventArgs e)
		{
		}

		public virtual void parserMatchNot(object source, MatchEventArgs e)
		{
		}

		public virtual void parserMismatch(object source, MatchEventArgs e)
		{
		}

		public virtual void parserMismatchNot(object source, MatchEventArgs e)
		{
		}

		public virtual void reportError(object source, MessageEventArgs e)
		{
		}

		public virtual void reportWarning(object source, MessageEventArgs e)
		{
		}

		public virtual void semanticPredicateEvaluated(object source, SemanticPredicateEventArgs e)
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

		public virtual void refresh()
		{
		}
	}
}
