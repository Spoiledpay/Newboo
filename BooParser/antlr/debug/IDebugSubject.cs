namespace antlr.debug
{
	public interface IDebugSubject
	{
		event TraceEventHandler EnterRule;

		event TraceEventHandler ExitRule;

		event TraceEventHandler Done;

		event MessageEventHandler ErrorReported;

		event MessageEventHandler WarningReported;

		event SemanticPredicateEventHandler SemPredEvaluated;

		event SyntacticPredicateEventHandler SynPredStarted;

		event SyntacticPredicateEventHandler SynPredFailed;

		event SyntacticPredicateEventHandler SynPredSucceeded;
	}
}
