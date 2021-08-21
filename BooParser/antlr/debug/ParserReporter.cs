using System;

namespace antlr.debug
{
	public class ParserReporter : Tracer, ParserListener, SemanticPredicateListener, ParserMatchListener, MessageListener, ParserTokenListener, TraceListener, SyntacticPredicateListener, Listener
	{
		public virtual void parserConsume(object source, TokenEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void parserLA(object source, TokenEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void parserMatch(object source, MatchEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void parserMatchNot(object source, MatchEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void parserMismatch(object source, MatchEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void parserMismatchNot(object source, MatchEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void reportError(object source, MessageEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void reportWarning(object source, MessageEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void semanticPredicateEvaluated(object source, SemanticPredicateEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void syntacticPredicateFailed(object source, SyntacticPredicateEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void syntacticPredicateStarted(object source, SyntacticPredicateEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}

		public virtual void syntacticPredicateSucceeded(object source, SyntacticPredicateEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
		}
	}
}
