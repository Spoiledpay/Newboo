namespace antlr.debug
{
	public interface ParserController : ParserListener, SemanticPredicateListener, ParserMatchListener, MessageListener, ParserTokenListener, TraceListener, SyntacticPredicateListener, Listener
	{
		ParserEventSupport ParserEventSupport { set; }

		void checkBreak();
	}
}
