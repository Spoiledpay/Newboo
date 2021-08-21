using System;
using System.Text;
using System.Threading;
using antlr.collections.impl;

namespace antlr.debug
{
	public abstract class DebuggingCharScanner : CharScanner, DebuggingParser
	{
		private ScannerEventSupport eventSupport;

		private bool _notDebugMode = false;

		protected internal string[] ruleNames;

		protected internal string[] semPredNames;

		private void InitBlock()
		{
			eventSupport = new ScannerEventSupport(this);
		}

		public virtual void setDebugMode(bool mode)
		{
			_notDebugMode = !mode;
		}

		public DebuggingCharScanner(InputBuffer cb)
			: base(cb)
		{
			InitBlock();
		}

		public DebuggingCharScanner(LexerSharedInputState state)
			: base(state)
		{
			InitBlock();
		}

		public virtual void addMessageListener(MessageListener l)
		{
			eventSupport.addMessageListener(l);
		}

		public virtual void addNewLineListener(NewLineListener l)
		{
			eventSupport.addNewLineListener(l);
		}

		public virtual void addParserListener(ParserListener l)
		{
			eventSupport.addParserListener(l);
		}

		public virtual void addParserMatchListener(ParserMatchListener l)
		{
			eventSupport.addParserMatchListener(l);
		}

		public virtual void addParserTokenListener(ParserTokenListener l)
		{
			eventSupport.addParserTokenListener(l);
		}

		public virtual void addSemanticPredicateListener(SemanticPredicateListener l)
		{
			eventSupport.addSemanticPredicateListener(l);
		}

		public virtual void addSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			eventSupport.addSyntacticPredicateListener(l);
		}

		public virtual void addTraceListener(TraceListener l)
		{
			eventSupport.addTraceListener(l);
		}

		public override void consume()
		{
			int c = -99;
			try
			{
				c = LA(1);
			}
			catch (CharStreamException)
			{
			}
			base.consume();
			eventSupport.fireConsume(c);
		}

		protected internal virtual void fireEnterRule(int num, int data)
		{
			if (isDebugMode())
			{
				eventSupport.fireEnterRule(num, inputState.guessing, data);
			}
		}

		protected internal virtual void fireExitRule(int num, int ttype)
		{
			if (isDebugMode())
			{
				eventSupport.fireExitRule(num, inputState.guessing, ttype);
			}
		}

		protected internal virtual bool fireSemanticPredicateEvaluated(int type, int num, bool condition)
		{
			if (isDebugMode())
			{
				return eventSupport.fireSemanticPredicateEvaluated(type, num, condition, inputState.guessing);
			}
			return condition;
		}

		protected internal virtual void fireSyntacticPredicateFailed()
		{
			if (isDebugMode())
			{
				eventSupport.fireSyntacticPredicateFailed(inputState.guessing);
			}
		}

		protected internal virtual void fireSyntacticPredicateStarted()
		{
			if (isDebugMode())
			{
				eventSupport.fireSyntacticPredicateStarted(inputState.guessing);
			}
		}

		protected internal virtual void fireSyntacticPredicateSucceeded()
		{
			if (isDebugMode())
			{
				eventSupport.fireSyntacticPredicateSucceeded(inputState.guessing);
			}
		}

		public virtual string getRuleName(int num)
		{
			return ruleNames[num];
		}

		public virtual string getSemPredName(int num)
		{
			return semPredNames[num];
		}

		public virtual void goToSleep()
		{
			lock (this)
			{
				try
				{
					Monitor.Wait(this);
				}
				catch (ThreadInterruptedException)
				{
				}
			}
		}

		public virtual bool isDebugMode()
		{
			return !_notDebugMode;
		}

		public override char LA(int i)
		{
			char c = base.LA(i);
			eventSupport.fireLA(i, c);
			return c;
		}

		protected internal override IToken makeToken(int t)
		{
			return base.makeToken(t);
		}

		public override void match(int c)
		{
			char c2 = LA(1);
			try
			{
				base.match(c);
				eventSupport.fireMatch(Convert.ToChar(c), inputState.guessing);
			}
			catch (MismatchedCharException ex)
			{
				if (inputState.guessing == 0)
				{
					eventSupport.fireMismatch(c2, Convert.ToChar(c), inputState.guessing);
				}
				throw ex;
			}
		}

		public override void match(BitSet b)
		{
			string text = base.text.ToString();
			char c = LA(1);
			try
			{
				base.match(b);
				eventSupport.fireMatch(c, b, text, inputState.guessing);
			}
			catch (MismatchedCharException ex)
			{
				if (inputState.guessing == 0)
				{
					eventSupport.fireMismatch(c, b, text, inputState.guessing);
				}
				throw ex;
			}
		}

		public override void match(string s)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			int length = s.Length;
			try
			{
				for (int i = 1; i <= length; i++)
				{
					stringBuilder.Append(base.LA(i));
				}
			}
			catch (Exception)
			{
			}
			try
			{
				base.match(s);
				eventSupport.fireMatch(s, inputState.guessing);
			}
			catch (MismatchedCharException ex2)
			{
				if (inputState.guessing == 0)
				{
					eventSupport.fireMismatch(stringBuilder.ToString(), s, inputState.guessing);
				}
				throw ex2;
			}
		}

		public override void matchNot(int c)
		{
			char c2 = LA(1);
			try
			{
				base.matchNot(c);
				eventSupport.fireMatchNot(c2, Convert.ToChar(c), inputState.guessing);
			}
			catch (MismatchedCharException ex)
			{
				if (inputState.guessing == 0)
				{
					eventSupport.fireMismatchNot(c2, Convert.ToChar(c), inputState.guessing);
				}
				throw ex;
			}
		}

		public override void matchRange(int c1, int c2)
		{
			char c3 = LA(1);
			try
			{
				base.matchRange(c1, c2);
				eventSupport.fireMatch(c3, string.Concat(c1, c2), inputState.guessing);
			}
			catch (MismatchedCharException ex)
			{
				if (inputState.guessing == 0)
				{
					eventSupport.fireMismatch(c3, string.Concat(c1, c2), inputState.guessing);
				}
				throw ex;
			}
		}

		public override void newline()
		{
			base.newline();
			eventSupport.fireNewLine(getLine());
		}

		public virtual void removeMessageListener(MessageListener l)
		{
			eventSupport.removeMessageListener(l);
		}

		public virtual void removeNewLineListener(NewLineListener l)
		{
			eventSupport.removeNewLineListener(l);
		}

		public virtual void removeParserListener(ParserListener l)
		{
			eventSupport.removeParserListener(l);
		}

		public virtual void removeParserMatchListener(ParserMatchListener l)
		{
			eventSupport.removeParserMatchListener(l);
		}

		public virtual void removeParserTokenListener(ParserTokenListener l)
		{
			eventSupport.removeParserTokenListener(l);
		}

		public virtual void removeSemanticPredicateListener(SemanticPredicateListener l)
		{
			eventSupport.removeSemanticPredicateListener(l);
		}

		public virtual void removeSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			eventSupport.removeSyntacticPredicateListener(l);
		}

		public virtual void removeTraceListener(TraceListener l)
		{
			eventSupport.removeTraceListener(l);
		}

		public virtual void reportError(MismatchedCharException e)
		{
			eventSupport.fireReportError(e);
			base.reportError(e);
		}

		public override void reportError(string s)
		{
			eventSupport.fireReportError(s);
			base.reportError(s);
		}

		public override void reportWarning(string s)
		{
			eventSupport.fireReportWarning(s);
			base.reportWarning(s);
		}

		public virtual void setupDebugging()
		{
		}

		public virtual void wakeUp()
		{
			lock (this)
			{
				Monitor.Pulse(this);
			}
		}
	}
}
