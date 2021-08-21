using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using antlr.collections.impl;
using antlr.debug;

namespace antlr
{
	public abstract class CharScanner : TokenStream, ICharScannerDebugSubject, IDebugSubject
	{
		internal const char NO_CHAR = '\0';

		public static readonly char EOF_CHAR = '\uffff';

		private EventHandlerList events_ = new EventHandlerList();

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

		protected internal StringBuilder text;

		protected bool saveConsumedInput = true;

		protected TokenCreator tokenCreator;

		protected char cached_LA1;

		protected char cached_LA2;

		protected bool caseSensitive = true;

		protected bool caseSensitiveLiterals = true;

		protected Hashtable literals;

		protected internal int tabsize = 8;

		protected internal IToken returnToken_ = null;

		protected internal LexerSharedInputState inputState;

		protected internal bool commitToPath = false;

		protected internal int traceDepth = 0;

		protected internal EventHandlerList Events => events_;

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

		public event NewLineEventHandler HitNewLine
		{
			add
			{
				Events.AddHandler(NewLineEventKey, value);
			}
			remove
			{
				Events.RemoveHandler(NewLineEventKey, value);
			}
		}

		public event MatchEventHandler MatchedChar
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

		public event MatchEventHandler MatchedNotChar
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

		public event MatchEventHandler MisMatchedChar
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

		public event MatchEventHandler MisMatchedNotChar
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

		public event TokenEventHandler ConsumedChar
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

		public event TokenEventHandler CharLA
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

		public CharScanner()
		{
			text = new StringBuilder();
			setTokenCreator(new CommonToken.CommonTokenCreator());
		}

		public CharScanner(InputBuffer cb)
			: this()
		{
			inputState = new LexerSharedInputState(cb);
			cached_LA2 = inputState.input.LA(2);
			cached_LA1 = inputState.input.LA(1);
		}

		public CharScanner(LexerSharedInputState sharedState)
			: this()
		{
			inputState = sharedState;
			if (inputState != null)
			{
				cached_LA2 = inputState.input.LA(2);
				cached_LA1 = inputState.input.LA(1);
			}
		}

		public virtual IToken nextToken()
		{
			return null;
		}

		public virtual void append(char c)
		{
			if (saveConsumedInput)
			{
				text.Append(c);
			}
		}

		public virtual void append(string s)
		{
			if (saveConsumedInput)
			{
				text.Append(s);
			}
		}

		public virtual void commit()
		{
			inputState.input.commit();
		}

		public virtual void consume()
		{
			if (inputState.guessing == 0)
			{
				if (caseSensitive)
				{
					append(cached_LA1);
				}
				else
				{
					append(inputState.input.LA(1));
				}
				if (cached_LA1 == '\t')
				{
					tab();
				}
				else
				{
					inputState.column++;
				}
			}
			if (caseSensitive)
			{
				cached_LA1 = inputState.input.consume();
				cached_LA2 = inputState.input.LA(2);
			}
			else
			{
				cached_LA1 = toLower(inputState.input.consume());
				cached_LA2 = toLower(inputState.input.LA(2));
			}
		}

		public virtual void consumeUntil(int c)
		{
			while (EOF_CHAR != cached_LA1 && c != cached_LA1)
			{
				consume();
			}
		}

		public virtual void consumeUntil(BitSet bset)
		{
			while (cached_LA1 != EOF_CHAR && !bset.member(cached_LA1))
			{
				consume();
			}
		}

		public virtual bool getCaseSensitive()
		{
			return caseSensitive;
		}

		public bool getCaseSensitiveLiterals()
		{
			return caseSensitiveLiterals;
		}

		public virtual int getColumn()
		{
			return inputState.column;
		}

		public virtual void setColumn(int c)
		{
			inputState.column = c;
		}

		public virtual bool getCommitToPath()
		{
			return commitToPath;
		}

		public virtual string getFilename()
		{
			return inputState.filename;
		}

		public virtual InputBuffer getInputBuffer()
		{
			return inputState.input;
		}

		public virtual LexerSharedInputState getInputState()
		{
			return inputState;
		}

		public virtual void setInputState(LexerSharedInputState state)
		{
			inputState = state;
		}

		public virtual int getLine()
		{
			return inputState.line;
		}

		public virtual string getText()
		{
			return text.ToString();
		}

		public virtual IToken getTokenObject()
		{
			return returnToken_;
		}

		public virtual char LA(int i)
		{
			switch (i)
			{
			case 1:
				return cached_LA1;
			case 2:
				return cached_LA2;
			default:
				if (caseSensitive)
				{
					return inputState.input.LA(i);
				}
				return toLower(inputState.input.LA(i));
			}
		}

		protected internal virtual IToken makeToken(int t)
		{
			IToken token = null;
			bool flag;
			try
			{
				token = tokenCreator.Create();
				if (token != null)
				{
					token.Type = t;
					token.setColumn(inputState.tokenStartColumn);
					token.setLine(inputState.tokenStartLine);
					token.setFilename(inputState.filename);
				}
				flag = true;
			}
			catch
			{
				flag = false;
			}
			if (!flag)
			{
				panic("Can't create Token object '" + tokenCreator.TokenTypeName + "'");
				token = Token.badToken;
			}
			return token;
		}

		public virtual int mark()
		{
			return inputState.input.mark();
		}

		public virtual void match(char c)
		{
			match((int)c);
		}

		public virtual void match(int c)
		{
			if (cached_LA1 != c)
			{
				throw new MismatchedCharException(cached_LA1, Convert.ToChar(c), matchNot: false, this);
			}
			consume();
		}

		public virtual void match(BitSet b)
		{
			if (!b.member(cached_LA1))
			{
				throw new MismatchedCharException(cached_LA1, b, matchNot: false, this);
			}
			consume();
		}

		public virtual void match(string s)
		{
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				if (cached_LA1 != s[i])
				{
					throw new MismatchedCharException(cached_LA1, s[i], matchNot: false, this);
				}
				consume();
			}
		}

		public virtual void matchNot(char c)
		{
			matchNot((int)c);
		}

		public virtual void matchNot(int c)
		{
			if (cached_LA1 == c)
			{
				throw new MismatchedCharException(cached_LA1, Convert.ToChar(c), matchNot: true, this);
			}
			consume();
		}

		public virtual void matchRange(int c1, int c2)
		{
			if (cached_LA1 < c1 || cached_LA1 > c2)
			{
				throw new MismatchedCharException(cached_LA1, Convert.ToChar(c1), Convert.ToChar(c2), matchNot: false, this);
			}
			consume();
		}

		public virtual void matchRange(char c1, char c2)
		{
			matchRange((int)c1, (int)c2);
		}

		public virtual void newline()
		{
			inputState.line++;
			inputState.column = 1;
		}

		public virtual void tab()
		{
			int column = getColumn();
			int column2 = ((column - 1) / tabsize + 1) * tabsize + 1;
			setColumn(column2);
		}

		public virtual void setTabSize(int size)
		{
			tabsize = size;
		}

		public virtual int getTabSize()
		{
			return tabsize;
		}

		public virtual void panic()
		{
			panic("");
		}

		public virtual void panic(string s)
		{
			throw new ANTLRPanicException("CharScanner::panic: " + s);
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

		public virtual void refresh()
		{
			if (caseSensitive)
			{
				cached_LA2 = inputState.input.LA(2);
				cached_LA1 = inputState.input.LA(1);
			}
			else
			{
				cached_LA2 = toLower(inputState.input.LA(2));
				cached_LA1 = toLower(inputState.input.LA(1));
			}
		}

		public virtual void resetState(InputBuffer ib)
		{
			text.Length = 0;
			traceDepth = 0;
			inputState.resetInput(ib);
			refresh();
		}

		public void resetState(Stream s)
		{
			resetState(new ByteBuffer(s));
		}

		public void resetState(TextReader tr)
		{
			resetState(new CharBuffer(tr));
		}

		public virtual void resetText()
		{
			text.Length = 0;
			inputState.tokenStartColumn = inputState.column;
			inputState.tokenStartLine = inputState.line;
		}

		public virtual void rewind(int pos)
		{
			inputState.input.rewind(pos);
			cached_LA2 = inputState.input.LA(2);
			cached_LA1 = inputState.input.LA(1);
		}

		public virtual void setCaseSensitive(bool t)
		{
			caseSensitive = t;
			if (caseSensitive)
			{
				cached_LA2 = inputState.input.LA(2);
				cached_LA1 = inputState.input.LA(1);
			}
			else
			{
				cached_LA2 = toLower(inputState.input.LA(2));
				cached_LA1 = toLower(inputState.input.LA(1));
			}
		}

		public virtual void setCommitToPath(bool commit)
		{
			commitToPath = commit;
		}

		public virtual void setFilename(string f)
		{
			inputState.filename = f;
		}

		public virtual void setLine(int line)
		{
			inputState.line = line;
		}

		public virtual void setText(string s)
		{
			resetText();
			text.Append(s);
		}

		public virtual void setTokenCreator(TokenCreator tokenCreator)
		{
			this.tokenCreator = tokenCreator;
		}

		public virtual int testLiteralsTable(int ttype)
		{
			string text = this.text.ToString();
			if (text == null || text == string.Empty)
			{
				return ttype;
			}
			object obj = literals[text];
			return (obj == null) ? ttype : ((int)obj);
		}

		public virtual int testLiteralsTable(string someText, int ttype)
		{
			if (someText == null || someText == string.Empty)
			{
				return ttype;
			}
			object obj = literals[someText];
			return (obj == null) ? ttype : ((int)obj);
		}

		public virtual char toLower(int c)
		{
			return char.ToLower(Convert.ToChar(c), CultureInfo.InvariantCulture);
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
			Console.Out.WriteLine("> lexer " + rname + "; c==" + LA(1));
		}

		public virtual void traceOut(string rname)
		{
			traceIndent();
			Console.Out.WriteLine("< lexer " + rname + "; c==" + LA(1));
			traceDepth--;
		}

		public virtual void uponEOF()
		{
		}
	}
}
