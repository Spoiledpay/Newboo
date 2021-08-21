using System;
using System.Reflection;
using System.Threading;
using antlr.collections.impl;

namespace antlr.debug
{
	public class LLkDebuggingParser : LLkParser, DebuggingParser
	{
		protected internal ParserEventSupport parserEventSupport;

		private bool _notDebugMode = false;

		protected internal string[] ruleNames;

		protected internal string[] semPredNames;

		private void InitBlock()
		{
			parserEventSupport = new ParserEventSupport(this);
		}

		public override void setDebugMode(bool mode)
		{
			_notDebugMode = !mode;
		}

		public LLkDebuggingParser(int k_)
			: base(k_)
		{
			InitBlock();
		}

		public LLkDebuggingParser(ParserSharedInputState state, int k_)
			: base(state, k_)
		{
			InitBlock();
		}

		public LLkDebuggingParser(TokenBuffer tokenBuf, int k_)
			: base(tokenBuf, k_)
		{
			InitBlock();
		}

		public LLkDebuggingParser(TokenStream lexer, int k_)
			: base(lexer, k_)
		{
			InitBlock();
		}

		public override void addMessageListener(MessageListener l)
		{
			parserEventSupport.addMessageListener(l);
		}

		public override void addParserListener(ParserListener l)
		{
			parserEventSupport.addParserListener(l);
		}

		public override void addParserMatchListener(ParserMatchListener l)
		{
			parserEventSupport.addParserMatchListener(l);
		}

		public override void addParserTokenListener(ParserTokenListener l)
		{
			parserEventSupport.addParserTokenListener(l);
		}

		public override void addSemanticPredicateListener(SemanticPredicateListener l)
		{
			parserEventSupport.addSemanticPredicateListener(l);
		}

		public override void addSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			parserEventSupport.addSyntacticPredicateListener(l);
		}

		public override void addTraceListener(TraceListener l)
		{
			parserEventSupport.addTraceListener(l);
		}

		public override void consume()
		{
			int num = -99;
			num = LA(1);
			base.consume();
			parserEventSupport.fireConsume(num);
		}

		protected internal virtual void fireEnterRule(int num, int data)
		{
			if (isDebugMode())
			{
				parserEventSupport.fireEnterRule(num, inputState.guessing, data);
			}
		}

		protected internal virtual void fireExitRule(int num, int data)
		{
			if (isDebugMode())
			{
				parserEventSupport.fireExitRule(num, inputState.guessing, data);
			}
		}

		protected internal virtual bool fireSemanticPredicateEvaluated(int type, int num, bool condition)
		{
			if (isDebugMode())
			{
				return parserEventSupport.fireSemanticPredicateEvaluated(type, num, condition, inputState.guessing);
			}
			return condition;
		}

		protected internal virtual void fireSyntacticPredicateFailed()
		{
			if (isDebugMode())
			{
				parserEventSupport.fireSyntacticPredicateFailed(inputState.guessing);
			}
		}

		protected internal virtual void fireSyntacticPredicateStarted()
		{
			if (isDebugMode())
			{
				parserEventSupport.fireSyntacticPredicateStarted(inputState.guessing);
			}
		}

		protected internal virtual void fireSyntacticPredicateSucceeded()
		{
			if (isDebugMode())
			{
				parserEventSupport.fireSyntacticPredicateSucceeded(inputState.guessing);
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

		public override bool isDebugMode()
		{
			return !_notDebugMode;
		}

		public virtual bool isGuessing()
		{
			return inputState.guessing > 0;
		}

		public override int LA(int i)
		{
			int num = base.LA(i);
			parserEventSupport.fireLA(i, num);
			return num;
		}

		public override void match(int t)
		{
			string text = LT(1).getText();
			int i = LA(1);
			try
			{
				base.match(t);
				parserEventSupport.fireMatch(t, text, inputState.guessing);
			}
			catch (MismatchedTokenException ex)
			{
				if (inputState.guessing == 0)
				{
					parserEventSupport.fireMismatch(i, t, text, inputState.guessing);
				}
				throw ex;
			}
		}

		public override void match(BitSet b)
		{
			string text = LT(1).getText();
			int num = LA(1);
			try
			{
				base.match(b);
				parserEventSupport.fireMatch(num, b, text, inputState.guessing);
			}
			catch (MismatchedTokenException ex)
			{
				if (inputState.guessing == 0)
				{
					parserEventSupport.fireMismatch(num, b, text, inputState.guessing);
				}
				throw ex;
			}
		}

		public override void matchNot(int t)
		{
			string text = LT(1).getText();
			int num = LA(1);
			try
			{
				base.matchNot(t);
				parserEventSupport.fireMatchNot(num, t, text, inputState.guessing);
			}
			catch (MismatchedTokenException ex)
			{
				if (inputState.guessing == 0)
				{
					parserEventSupport.fireMismatchNot(num, t, text, inputState.guessing);
				}
				throw ex;
			}
		}

		public override void removeMessageListener(MessageListener l)
		{
			parserEventSupport.removeMessageListener(l);
		}

		public override void removeParserListener(ParserListener l)
		{
			parserEventSupport.removeParserListener(l);
		}

		public override void removeParserMatchListener(ParserMatchListener l)
		{
			parserEventSupport.removeParserMatchListener(l);
		}

		public override void removeParserTokenListener(ParserTokenListener l)
		{
			parserEventSupport.removeParserTokenListener(l);
		}

		public override void removeSemanticPredicateListener(SemanticPredicateListener l)
		{
			parserEventSupport.removeSemanticPredicateListener(l);
		}

		public override void removeSyntacticPredicateListener(SyntacticPredicateListener l)
		{
			parserEventSupport.removeSyntacticPredicateListener(l);
		}

		public override void removeTraceListener(TraceListener l)
		{
			parserEventSupport.removeTraceListener(l);
		}

		public override void reportError(RecognitionException ex)
		{
			parserEventSupport.fireReportError(ex);
			base.reportError(ex);
		}

		public override void reportError(string s)
		{
			parserEventSupport.fireReportError(s);
			base.reportError(s);
		}

		public override void reportWarning(string s)
		{
			parserEventSupport.fireReportWarning(s);
			base.reportWarning(s);
		}

		public virtual void setupDebugging(TokenBuffer tokenBuf)
		{
			setupDebugging(null, tokenBuf);
		}

		public virtual void setupDebugging(TokenStream lexer)
		{
			setupDebugging(lexer, null);
		}

		protected internal virtual void setupDebugging(TokenStream lexer, TokenBuffer tokenBuf)
		{
			setDebugMode(mode: true);
			try
			{
				Type type = Type.GetType("antlr.parseview.ParseView");
				ConstructorInfo constructor = type.GetConstructor(new Type[3]
				{
					typeof(LLkDebuggingParser),
					typeof(TokenStream),
					typeof(TokenBuffer)
				});
				constructor.Invoke(new object[3] { this, lexer, tokenBuf });
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("Error initializing ParseView: " + ex);
				Console.Error.WriteLine("Please report this to Scott Stanchfield, thetick@magelang.com");
				Environment.Exit(1);
			}
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
