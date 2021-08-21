using System;
using System.Collections;
using antlr.collections.impl;

namespace antlr.debug
{
	public class ParserEventSupport
	{
		private object source;

		private Hashtable listeners;

		private MatchEventArgs matchEvent;

		private MessageEventArgs messageEvent;

		private TokenEventArgs tokenEvent;

		private SemanticPredicateEventArgs semPredEvent;

		private SyntacticPredicateEventArgs synPredEvent;

		private TraceEventArgs traceEvent;

		private NewLineEventArgs newLineEvent;

		private ParserController controller;

		private int ruleDepth = 0;

		public ParserEventSupport(object source)
		{
			matchEvent = new MatchEventArgs();
			messageEvent = new MessageEventArgs();
			tokenEvent = new TokenEventArgs();
			traceEvent = new TraceEventArgs();
			semPredEvent = new SemanticPredicateEventArgs();
			synPredEvent = new SyntacticPredicateEventArgs();
			newLineEvent = new NewLineEventArgs();
			listeners = new Hashtable();
			this.source = source;
		}

		public virtual void checkController()
		{
			if (controller != null)
			{
				controller.checkBreak();
			}
		}

		public virtual void addDoneListener(Listener l)
		{
			((Parser)source).Done += l.doneParsing;
			listeners[l] = l;
		}

		public virtual void addMessageListener(MessageListener l)
		{
			((Parser)source).ErrorReported += l.reportError;
			((Parser)source).WarningReported += l.reportWarning;
			addDoneListener(l);
		}

		public virtual void addParserListener(ParserListener l)
		{
			if (l is ParserController)
			{
				((ParserController)l).ParserEventSupport = this;
				controller = (ParserController)l;
			}
			addParserMatchListener(l);
			addParserTokenListener(l);
			addMessageListener(l);
			addTraceListener(l);
			addSemanticPredicateListener(l);
			addSyntacticPredicateListener(l);
		}

		public virtual void addParserMatchListener(ParserMatchListener l)
		{
			((Parser)source).MatchedToken += l.parserMatch;
			((Parser)source).MatchedNotToken += l.parserMatchNot;
			((Parser)source).MisMatchedToken += l.parserMismatch;
			((Parser)source).MisMatchedNotToken += l.parserMismatchNot;
			addDoneListener(l);
		}

		public virtual void addParserTokenListener(ParserTokenListener l)
		{
			((Parser)source).ConsumedToken += l.parserConsume;
			((Parser)source).TokenLA += l.parserLA;
			addDoneListener(l);
		}

		public virtual void addSemanticPredicateListener(SemanticPredicateListener l)
		{
			((Parser)source).SemPredEvaluated += l.semanticPredicateEvaluated;
			addDoneListener(l);
		}

		public virtual void addSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			((Parser)source).SynPredStarted += l.syntacticPredicateStarted;
			((Parser)source).SynPredFailed += l.syntacticPredicateFailed;
			((Parser)source).SynPredSucceeded += l.syntacticPredicateSucceeded;
			addDoneListener(l);
		}

		public virtual void addTraceListener(TraceListener l)
		{
			((Parser)source).EnterRule += l.enterRule;
			((Parser)source).ExitRule += l.exitRule;
			addDoneListener(l);
		}

		public virtual void fireConsume(int c)
		{
			TokenEventHandler tokenEventHandler = (TokenEventHandler)((Parser)source).Events[Parser.LAEventKey];
			if (tokenEventHandler != null)
			{
				tokenEvent.setValues(TokenEventArgs.CONSUME, 1, c);
				tokenEventHandler(source, tokenEvent);
			}
			checkController();
		}

		public virtual void fireDoneParsing()
		{
			TraceEventHandler traceEventHandler = (TraceEventHandler)((Parser)source).Events[Parser.DoneEventKey];
			if (traceEventHandler != null)
			{
				traceEvent.setValues(TraceEventArgs.DONE_PARSING, 0, 0, 0);
				traceEventHandler(source, traceEvent);
			}
			checkController();
		}

		public virtual void fireEnterRule(int ruleNum, int guessing, int data)
		{
			ruleDepth++;
			TraceEventHandler traceEventHandler = (TraceEventHandler)((Parser)source).Events[Parser.EnterRuleEventKey];
			if (traceEventHandler != null)
			{
				traceEvent.setValues(TraceEventArgs.ENTER, ruleNum, guessing, data);
				traceEventHandler(source, traceEvent);
			}
			checkController();
		}

		public virtual void fireExitRule(int ruleNum, int guessing, int data)
		{
			TraceEventHandler traceEventHandler = (TraceEventHandler)((Parser)source).Events[Parser.ExitRuleEventKey];
			if (traceEventHandler != null)
			{
				traceEvent.setValues(TraceEventArgs.EXIT, ruleNum, guessing, data);
				traceEventHandler(source, traceEvent);
			}
			checkController();
			ruleDepth--;
			if (ruleDepth == 0)
			{
				fireDoneParsing();
			}
		}

		public virtual void fireLA(int k, int la)
		{
			TokenEventHandler tokenEventHandler = (TokenEventHandler)((Parser)source).Events[Parser.LAEventKey];
			if (tokenEventHandler != null)
			{
				tokenEvent.setValues(TokenEventArgs.LA, k, la);
				tokenEventHandler(source, tokenEvent);
			}
			checkController();
		}

		public virtual void fireMatch(char c, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR, c, c, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatch(char c, BitSet b, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR_BITSET, c, b, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatch(char c, string target, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR_RANGE, c, target, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatch(int c, BitSet b, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.BITSET, c, b, text, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatch(int n, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.TOKEN, n, n, text, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatch(string s, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.STRING, 0, s, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatchNot(char c, char n, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchNotEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR, c, n, null, guessing, inverse: true, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMatchNot(int c, int n, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MatchNotEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.TOKEN, c, n, text, guessing, inverse: true, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(char c, char n, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR, c, n, null, guessing, inverse: false, matched: false);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(char c, BitSet b, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR_BITSET, c, b, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(char c, string target, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR_RANGE, c, target, null, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(int i, int n, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.TOKEN, i, n, text, guessing, inverse: false, matched: false);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(int i, BitSet b, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.BITSET, i, b, text, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatch(string s, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.STRING, 0, text, s, guessing, inverse: false, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatchNot(char v, char c, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchNotEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.CHAR, v, c, null, guessing, inverse: true, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireMismatchNot(int i, int n, string text, int guessing)
		{
			MatchEventHandler matchEventHandler = (MatchEventHandler)((Parser)source).Events[Parser.MisMatchNotEventKey];
			if (matchEventHandler != null)
			{
				matchEvent.setValues(MatchEventArgs.TOKEN, i, n, text, guessing, inverse: true, matched: true);
				matchEventHandler(source, matchEvent);
			}
			checkController();
		}

		public virtual void fireReportError(Exception e)
		{
			MessageEventHandler messageEventHandler = (MessageEventHandler)((Parser)source).Events[Parser.ReportErrorEventKey];
			if (messageEventHandler != null)
			{
				messageEvent.setValues(MessageEventArgs.ERROR, e.ToString());
				messageEventHandler(source, messageEvent);
			}
			checkController();
		}

		public virtual void fireReportError(string s)
		{
			MessageEventHandler messageEventHandler = (MessageEventHandler)((Parser)source).Events[Parser.ReportErrorEventKey];
			if (messageEventHandler != null)
			{
				messageEvent.setValues(MessageEventArgs.ERROR, s);
				messageEventHandler(source, messageEvent);
			}
			checkController();
		}

		public virtual void fireReportWarning(string s)
		{
			MessageEventHandler messageEventHandler = (MessageEventHandler)((Parser)source).Events[Parser.ReportWarningEventKey];
			if (messageEventHandler != null)
			{
				messageEvent.setValues(MessageEventArgs.WARNING, s);
				messageEventHandler(source, messageEvent);
			}
			checkController();
		}

		public virtual bool fireSemanticPredicateEvaluated(int type, int condition, bool result, int guessing)
		{
			SemanticPredicateEventHandler semanticPredicateEventHandler = (SemanticPredicateEventHandler)((Parser)source).Events[Parser.SemPredEvaluatedEventKey];
			if (semanticPredicateEventHandler != null)
			{
				semPredEvent.setValues(type, condition, result, guessing);
				semanticPredicateEventHandler(source, semPredEvent);
			}
			checkController();
			return result;
		}

		public virtual void fireSyntacticPredicateFailed(int guessing)
		{
			SyntacticPredicateEventHandler syntacticPredicateEventHandler = (SyntacticPredicateEventHandler)((Parser)source).Events[Parser.SynPredFailedEventKey];
			if (syntacticPredicateEventHandler != null)
			{
				synPredEvent.setValues(0, guessing);
				syntacticPredicateEventHandler(source, synPredEvent);
			}
			checkController();
		}

		public virtual void fireSyntacticPredicateStarted(int guessing)
		{
			SyntacticPredicateEventHandler syntacticPredicateEventHandler = (SyntacticPredicateEventHandler)((Parser)source).Events[Parser.SynPredStartedEventKey];
			if (syntacticPredicateEventHandler != null)
			{
				synPredEvent.setValues(0, guessing);
				syntacticPredicateEventHandler(source, synPredEvent);
			}
			checkController();
		}

		public virtual void fireSyntacticPredicateSucceeded(int guessing)
		{
			SyntacticPredicateEventHandler syntacticPredicateEventHandler = (SyntacticPredicateEventHandler)((Parser)source).Events[Parser.SynPredSucceededEventKey];
			if (syntacticPredicateEventHandler != null)
			{
				synPredEvent.setValues(0, guessing);
				syntacticPredicateEventHandler(source, synPredEvent);
			}
			checkController();
		}

		public virtual void refreshListeners()
		{
			Hashtable hashtable;
			lock (listeners.SyncRoot)
			{
				hashtable = (Hashtable)listeners.Clone();
			}
			foreach (DictionaryEntry item in hashtable)
			{
				if (item.Value != null)
				{
					((Listener)item.Value).refresh();
				}
			}
		}

		public virtual void removeDoneListener(Listener l)
		{
			((Parser)source).Done -= l.doneParsing;
			listeners.Remove(l);
		}

		public virtual void removeMessageListener(MessageListener l)
		{
			((Parser)source).ErrorReported -= l.reportError;
			((Parser)source).WarningReported -= l.reportWarning;
			removeDoneListener(l);
		}

		public virtual void removeParserListener(ParserListener l)
		{
			removeParserMatchListener(l);
			removeMessageListener(l);
			removeParserTokenListener(l);
			removeTraceListener(l);
			removeSemanticPredicateListener(l);
			removeSyntacticPredicateListener(l);
		}

		public virtual void removeParserMatchListener(ParserMatchListener l)
		{
			((Parser)source).MatchedToken -= l.parserMatch;
			((Parser)source).MatchedNotToken -= l.parserMatchNot;
			((Parser)source).MisMatchedToken -= l.parserMismatch;
			((Parser)source).MisMatchedNotToken -= l.parserMismatchNot;
			removeDoneListener(l);
		}

		public virtual void removeParserTokenListener(ParserTokenListener l)
		{
			((Parser)source).ConsumedToken -= l.parserConsume;
			((Parser)source).TokenLA -= l.parserLA;
			removeDoneListener(l);
		}

		public virtual void removeSemanticPredicateListener(SemanticPredicateListener l)
		{
			((Parser)source).SemPredEvaluated -= l.semanticPredicateEvaluated;
			removeDoneListener(l);
		}

		public virtual void removeSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			((Parser)source).SynPredStarted -= l.syntacticPredicateStarted;
			((Parser)source).SynPredFailed -= l.syntacticPredicateFailed;
			((Parser)source).SynPredSucceeded -= l.syntacticPredicateSucceeded;
			removeDoneListener(l);
		}

		public virtual void removeTraceListener(TraceListener l)
		{
			((Parser)source).EnterRule -= l.enterRule;
			((Parser)source).ExitRule -= l.exitRule;
			removeDoneListener(l);
		}
	}
}
