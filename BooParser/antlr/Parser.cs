using System;
using System.ComponentModel;
using antlr.collections;
using antlr.collections.impl;
using antlr.debug;

namespace antlr
{
	public abstract class Parser : IParserDebugSubject, IDebugSubject
	{
		private EventHandlerList events_;

		internal static readonly object EnterRuleEventKey = new object();

		internal static readonly object ExitRuleEventKey = new object();

		internal static readonly object DoneEventKey = new object();

		internal static readonly object ReportErrorEventKey = new object();

		internal static readonly object ReportWarningEventKey = new object();

		internal static readonly object NewLineEventKey = new object();

		internal static readonly object MatchEventKey = new object();

		internal static readonly object MatchNotEventKey = new object();

		internal static readonly object MisMatchEventKey = new object();

		internal static readonly object MisMatchNotEventKey = new object();

		internal static readonly object ConsumeEventKey = new object();

		internal static readonly object LAEventKey = new object();

		internal static readonly object SemPredEvaluatedEventKey = new object();

		internal static readonly object SynPredStartedEventKey = new object();

		internal static readonly object SynPredFailedEventKey = new object();

		internal static readonly object SynPredSucceededEventKey = new object();

		protected internal ParserSharedInputState inputState;

		protected internal string[] tokenNames;

		protected internal AST returnAST;

		protected internal ASTFactory _astFactory;

		private bool ignoreInvalidDebugCalls = false;

		protected internal int traceDepth = 0;

		protected internal EventHandlerList Events
		{
			get
			{
				if (null == events_)
				{
					events_ = new EventHandlerList();
				}
				return events_;
			}
		}

		public virtual ASTFactory astFactory
		{
			get
			{
				if (null == _astFactory)
				{
					_astFactory = new ASTFactory();
				}
				return _astFactory;
			}
			set
			{
				_astFactory = value;
			}
		}

		public event TraceEventHandler EnterRule
		{
			add
			{
				Events.AddHandler(EnterRuleEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(EnterRuleEventKey, value);
			}
		}

		public event TraceEventHandler ExitRule
		{
			add
			{
				Events.AddHandler(ExitRuleEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(ExitRuleEventKey, value);
			}
		}

		public event TraceEventHandler Done
		{
			add
			{
				Events.AddHandler(DoneEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(DoneEventKey, value);
			}
		}

		public event MessageEventHandler ErrorReported
		{
			add
			{
				Events.AddHandler(ReportErrorEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(ReportErrorEventKey, value);
			}
		}

		public event MessageEventHandler WarningReported
		{
			add
			{
				Events.AddHandler(ReportWarningEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(ReportWarningEventKey, value);
			}
		}

		public event MatchEventHandler MatchedToken
		{
			add
			{
				Events.AddHandler(MatchEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(MatchEventKey, value);
			}
		}

		public event MatchEventHandler MatchedNotToken
		{
			add
			{
				Events.AddHandler(MatchNotEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(MatchNotEventKey, value);
			}
		}

		public event MatchEventHandler MisMatchedToken
		{
			add
			{
				Events.AddHandler(MisMatchEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(MisMatchEventKey, value);
			}
		}

		public event MatchEventHandler MisMatchedNotToken
		{
			add
			{
				Events.AddHandler(MisMatchNotEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(MisMatchNotEventKey, value);
			}
		}

		public event TokenEventHandler ConsumedToken
		{
			add
			{
				Events.AddHandler(ConsumeEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(ConsumeEventKey, value);
			}
		}

		public event TokenEventHandler TokenLA
		{
			add
			{
				Events.AddHandler(LAEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(LAEventKey, value);
			}
		}

		public event SemanticPredicateEventHandler SemPredEvaluated
		{
			add
			{
				Events.AddHandler(SemPredEvaluatedEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(SemPredEvaluatedEventKey, value);
			}
		}

		public event SyntacticPredicateEventHandler SynPredStarted
		{
			add
			{
				Events.AddHandler(SynPredStartedEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(SynPredStartedEventKey, value);
			}
		}

		public event SyntacticPredicateEventHandler SynPredFailed
		{
			add
			{
				Events.AddHandler(SynPredFailedEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(SynPredFailedEventKey, value);
			}
		}

		public event SyntacticPredicateEventHandler SynPredSucceeded
		{
			add
			{
				Events.AddHandler(SynPredSucceededEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(SynPredSucceededEventKey, value);
			}
		}

		public Parser()
		{
			inputState = new ParserSharedInputState();
		}

		public Parser(ParserSharedInputState state)
		{
			inputState = state;
		}

		public virtual void addMessageListener(MessageListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addMessageListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addParserListener(ParserListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addParserListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addParserMatchListener(ParserMatchListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addParserMatchListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addParserTokenListener(ParserTokenListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addParserTokenListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addSemanticPredicateListener(SemanticPredicateListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addSemanticPredicateListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addSyntacticPredicateListener() is only valid if parser built for debugging");
			}
		}

		public virtual void addTraceListener(TraceListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("addTraceListener() is only valid if parser built for debugging");
			}
		}

		public abstract void consume();

		public virtual void consumeUntil(int tokenType)
		{
			while (LA(1) != 1 && LA(1) != tokenType)
			{
				consume();
			}
		}

		public virtual void consumeUntil(BitSet bset)
		{
			while (LA(1) != 1 && !bset.member(LA(1)))
			{
				consume();
			}
		}

		protected internal virtual void defaultDebuggingSetup(TokenStream lexer, TokenBuffer tokBuf)
		{
		}

		public virtual AST getAST()
		{
			return returnAST;
		}

		public virtual ASTFactory getASTFactory()
		{
			return astFactory;
		}

		public virtual string getFilename()
		{
			return inputState.filename;
		}

		public virtual ParserSharedInputState getInputState()
		{
			return inputState;
		}

		public virtual void setInputState(ParserSharedInputState state)
		{
			inputState = state;
		}

		public virtual void resetState()
		{
			traceDepth = 0;
			inputState.reset();
		}

		public virtual string getTokenName(int num)
		{
			return tokenNames[num];
		}

		public virtual string[] getTokenNames()
		{
			return tokenNames;
		}

		public virtual bool isDebugMode()
		{
			return false;
		}

		public abstract int LA(int i);

		public abstract IToken LT(int i);

		public virtual int mark()
		{
			return inputState.input.mark();
		}

		public virtual void match(int t)
		{
			if (LA(1) != t)
			{
				throw new MismatchedTokenException(tokenNames, LT(1), t, matchNot: false, getFilename());
			}
			consume();
		}

		public virtual void match(BitSet b)
		{
			if (!b.member(LA(1)))
			{
				throw new MismatchedTokenException(tokenNames, LT(1), b, matchNot: false, getFilename());
			}
			consume();
		}

		public virtual void matchNot(int t)
		{
			if (LA(1) == t)
			{
				throw new MismatchedTokenException(tokenNames, LT(1), t, matchNot: true, getFilename());
			}
			consume();
		}

		[Obsolete("De-activated since version 2.7.2.6 as it cannot be overidden.", true)]
		public static void panic()
		{
			Console.Error.WriteLine("Parser: panic");
			Environment.Exit(1);
		}

		public virtual void removeMessageListener(MessageListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("removeMessageListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeParserListener(ParserListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("removeParserListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeParserMatchListener(ParserMatchListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("removeParserMatchListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeParserTokenListener(ParserTokenListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("removeParserTokenListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeSemanticPredicateListener(SemanticPredicateListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("removeSemanticPredicateListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new ArgumentException("removeSyntacticPredicateListener() is only valid if parser built for debugging");
			}
		}

		public virtual void removeTraceListener(TraceListener l)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("removeTraceListener() is only valid if parser built for debugging");
			}
		}

		public virtual void reportError(RecognitionException ex)
		{
			Console.Error.WriteLine(ex);
		}

		public virtual void reportError(string s)
		{
			if (getFilename() == null)
			{
				Console.Error.WriteLine("error: " + s);
			}
			else
			{
				Console.Error.WriteLine(getFilename() + ": error: " + s);
			}
		}

		public virtual void reportWarning(string s)
		{
			if (getFilename() == null)
			{
				Console.Error.WriteLine("warning: " + s);
			}
			else
			{
				Console.Error.WriteLine(getFilename() + ": warning: " + s);
			}
		}

		public virtual void recover(RecognitionException ex, BitSet tokenSet)
		{
			consume();
			consumeUntil(tokenSet);
		}

		public virtual void rewind(int pos)
		{
			inputState.input.rewind(pos);
		}

		public virtual void setASTFactory(ASTFactory f)
		{
			astFactory = f;
		}

		public virtual void setASTNodeClass(string cl)
		{
			astFactory.setASTNodeType(cl);
		}

		[Obsolete("Replaced by setASTNodeClass(string) since version 2.7.1", true)]
		public virtual void setASTNodeType(string nodeType)
		{
			setASTNodeClass(nodeType);
		}

		public virtual void setDebugMode(bool debugMode)
		{
			if (!ignoreInvalidDebugCalls)
			{
				throw new SystemException("setDebugMode() only valid if parser built for debugging");
			}
		}

		public virtual void setFilename(string f)
		{
			inputState.filename = f;
		}

		public virtual void setIgnoreInvalidDebugCalls(bool Value)
		{
			ignoreInvalidDebugCalls = Value;
		}

		public virtual void setTokenBuffer(TokenBuffer t)
		{
			inputState.input = t;
		}

		public virtual void traceIndent()
		{
			for (int i = 0; i < traceDepth; i++)
			{
				Console.Out.Write(" ");
			}
		}

		public virtual void traceIn(string rname)
		{
			traceDepth++;
			traceIndent();
			Console.Out.WriteLine("> " + rname + "; LA(1)==" + LT(1).getText() + ((inputState.guessing > 0) ? " [guessing]" : ""));
		}

		public virtual void traceOut(string rname)
		{
			traceIndent();
			Console.Out.WriteLine("< " + rname + "; LA(1)==" + LT(1).getText() + ((inputState.guessing > 0) ? " [guessing]" : ""));
			traceDepth--;
		}
	}
}
