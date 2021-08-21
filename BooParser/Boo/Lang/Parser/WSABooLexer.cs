using System.Collections;
using System.Globalization;
using System.IO;
using antlr;
using antlr.collections.impl;
using Boo.Lang.Parser.Util;

namespace Boo.Lang.Parser
{
	public class WSABooLexer : CharScanner, TokenStream
	{
		public const int EOF = 1;

		public const int NULL_TREE_LOOKAHEAD = 3;

		public const int ELIST = 4;

		public const int DLIST = 5;

		public const int ESEPARATOR = 6;

		public const int ASSEMBLY_ATTRIBUTE_BEGIN = 7;

		public const int MODULE_ATTRIBUTE_BEGIN = 8;

		public const int ABSTRACT = 9;

		public const int AND = 10;

		public const int AS = 11;

		public const int BREAK = 12;

		public const int CONTINUE = 13;

		public const int CALLABLE = 14;

		public const int CAST = 15;

		public const int CHAR = 16;

		public const int CLASS = 17;

		public const int CONSTRUCTOR = 18;

		public const int DEF = 19;

		public const int DESTRUCTOR = 20;

		public const int DO = 21;

		public const int ELIF = 22;

		public const int ELSE = 23;

		public const int END = 24;

		public const int ENSURE = 25;

		public const int ENUM = 26;

		public const int EVENT = 27;

		public const int EXCEPT = 28;

		public const int FAILURE = 29;

		public const int FINAL = 30;

		public const int FROM = 31;

		public const int FOR = 32;

		public const int FALSE = 33;

		public const int GET = 34;

		public const int GOTO = 35;

		public const int IMPORT = 36;

		public const int INTERFACE = 37;

		public const int INTERNAL = 38;

		public const int IS = 39;

		public const int ISA = 40;

		public const int IF = 41;

		public const int IN = 42;

		public const int NAMESPACE = 43;

		public const int NEW = 44;

		public const int NOT = 45;

		public const int NULL = 46;

		public const int OF = 47;

		public const int OR = 48;

		public const int OVERRIDE = 49;

		public const int PUBLIC = 50;

		public const int PROTECTED = 51;

		public const int PRIVATE = 52;

		public const int RAISE = 53;

		public const int REF = 54;

		public const int RETURN = 55;

		public const int SET = 56;

		public const int SELF = 57;

		public const int SUPER = 58;

		public const int STATIC = 59;

		public const int STRUCT = 60;

		public const int THEN = 61;

		public const int TRY = 62;

		public const int TRANSIENT = 63;

		public const int TRUE = 64;

		public const int TYPEOF = 65;

		public const int UNLESS = 66;

		public const int VIRTUAL = 67;

		public const int PARTIAL = 68;

		public const int WHILE = 69;

		public const int YIELD = 70;

		public const int ID = 71;

		public const int TRIPLE_QUOTED_STRING = 72;

		public const int EOS = 73;

		public const int NEWLINE = 74;

		public const int LPAREN = 75;

		public const int RPAREN = 76;

		public const int DOUBLE_QUOTED_STRING = 77;

		public const int SINGLE_QUOTED_STRING = 78;

		public const int MULTIPLY = 79;

		public const int LBRACK = 80;

		public const int RBRACK = 81;

		public const int ASSIGN = 82;

		public const int SUBTRACT = 83;

		public const int COMMA = 84;

		public const int SPLICE_BEGIN = 85;

		public const int DOT = 86;

		public const int COLON = 87;

		public const int NULLABLE_SUFFIX = 88;

		public const int EXPONENTIATION = 89;

		public const int BITWISE_OR = 90;

		public const int LBRACE = 91;

		public const int RBRACE = 92;

		public const int QQ_BEGIN = 93;

		public const int QQ_END = 94;

		public const int INPLACE_BITWISE_OR = 95;

		public const int INPLACE_EXCLUSIVE_OR = 96;

		public const int INPLACE_BITWISE_AND = 97;

		public const int INPLACE_SHIFT_LEFT = 98;

		public const int INPLACE_SHIFT_RIGHT = 99;

		public const int CMP_OPERATOR = 100;

		public const int GREATER_THAN = 101;

		public const int LESS_THAN = 102;

		public const int ADD = 103;

		public const int EXCLUSIVE_OR = 104;

		public const int DIVISION = 105;

		public const int MODULUS = 106;

		public const int BITWISE_AND = 107;

		public const int SHIFT_LEFT = 108;

		public const int SHIFT_RIGHT = 109;

		public const int LONG = 110;

		public const int INCREMENT = 111;

		public const int DECREMENT = 112;

		public const int ONES_COMPLEMENT = 113;

		public const int INT = 114;

		public const int BACKTICK_QUOTED_STRING = 115;

		public const int RE_LITERAL = 116;

		public const int DOUBLE = 117;

		public const int FLOAT = 118;

		public const int TIMESPAN = 119;

		public const int ID_SUFFIX = 120;

		public const int LINE_CONTINUATION = 121;

		public const int INTERPOLATED_EXPRESSION = 122;

		public const int INTERPOLATED_REFERENCE = 123;

		public const int SL_COMMENT = 124;

		public const int ML_COMMENT = 125;

		public const int WS = 126;

		public const int X_RE_LITERAL = 127;

		public const int DQS_ESC = 128;

		public const int SQS_ESC = 129;

		public const int SESC = 130;

		public const int RE_CHAR = 131;

		public const int X_RE_CHAR = 132;

		public const int RE_ESC = 133;

		public const int DIGIT_GROUP = 134;

		public const int REVERSE_DIGIT_GROUP = 135;

		public const int AT_SYMBOL = 136;

		public const int ID_LETTER = 137;

		public const int DIGIT = 138;

		public const int HEXDIGIT = 139;

		protected int _skipWhitespaceRegion = 0;

		private TokenStreamRecorder _erecorder;

		private TokenStreamSelector _selector;

		private bool _preserveComments;

		public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());

		public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());

		public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());

		public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());

		public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());

		public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());

		public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());

		public static readonly BitSet tokenSet_7_ = new BitSet(mk_tokenSet_7_());

		public static readonly BitSet tokenSet_8_ = new BitSet(mk_tokenSet_8_());

		public static readonly BitSet tokenSet_9_ = new BitSet(mk_tokenSet_9_());

		public static readonly BitSet tokenSet_10_ = new BitSet(mk_tokenSet_10_());

		public static readonly BitSet tokenSet_11_ = new BitSet(mk_tokenSet_11_());

		private bool SkipWhitespace => _skipWhitespaceRegion > 0;

		public bool PreserveComments
		{
			get
			{
				return _preserveComments;
			}
			set
			{
				_preserveComments = value;
			}
		}

		internal void Initialize(TokenStreamSelector selector, int tabSize, TokenCreator tokenCreator)
		{
			setTabSize(tabSize);
			setTokenCreator(tokenCreator);
			_selector = selector;
			_erecorder = new TokenStreamRecorder(selector);
		}

		internal TokenStream CreateExpressionLexer()
		{
			WSABooExpressionLexer wSABooExpressionLexer = new WSABooExpressionLexer(getInputState());
			wSABooExpressionLexer.setTabSize(getTabSize());
			wSABooExpressionLexer.setTokenCreator(tokenCreator);
			return wSABooExpressionLexer;
		}

		internal static bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		private void ParseInterpolatedExpression(int tokenClose, int tokenOpen)
		{
			EnqueueESEPARATOR();
			if (0 == _erecorder.RecordUntil(CreateExpressionLexer(), tokenClose, tokenOpen))
			{
				_erecorder.Dequeue();
			}
			else
			{
				EnqueueESEPARATOR();
			}
			refresh();
		}

		private void EnqueueESEPARATOR()
		{
			_erecorder.Enqueue(makeESEPARATOR());
		}

		private void Enqueue(IToken token, string text)
		{
			token.setText(text);
			EnqueueInterpolatedToken(token);
		}

		private void EnqueueInterpolatedToken(IToken token)
		{
			EnqueueESEPARATOR();
			_erecorder.Enqueue(token);
			EnqueueESEPARATOR();
		}

		private IToken makeESEPARATOR()
		{
			return makeToken(6);
		}

		internal void EnterSkipWhitespaceRegion()
		{
			_skipWhitespaceRegion++;
		}

		internal void LeaveSkipWhitespaceRegion()
		{
			_skipWhitespaceRegion--;
		}

		public WSABooLexer(Stream ins)
			: this(new ByteBuffer(ins))
		{
		}

		public WSABooLexer(TextReader r)
			: this(new CharBuffer(r))
		{
		}

		public WSABooLexer(InputBuffer ib)
			: this(new LexerSharedInputState(ib))
		{
		}

		public WSABooLexer(LexerSharedInputState state)
			: base(state)
		{
			initialize();
		}

		private void initialize()
		{
			caseSensitiveLiterals = true;
			setCaseSensitive(t: true);
			literals = new Hashtable(100, 0.4f, null, Comparer.Default);
			literals.Add("public", 50);
			literals.Add("namespace", 43);
			literals.Add("break", 12);
			literals.Add("while", 69);
			literals.Add("new", 44);
			literals.Add("end", 24);
			literals.Add("then", 61);
			literals.Add("raise", 53);
			literals.Add("typeof", 65);
			literals.Add("and", 10);
			literals.Add("failure", 29);
			literals.Add("not", 45);
			literals.Add("return", 55);
			literals.Add("from", 31);
			literals.Add("null", 46);
			literals.Add("def", 19);
			literals.Add("protected", 51);
			literals.Add("ref", 54);
			literals.Add("class", 17);
			literals.Add("do", 21);
			literals.Add("except", 28);
			literals.Add("event", 27);
			literals.Add("unless", 66);
			literals.Add("super", 58);
			literals.Add("set", 56);
			literals.Add("transient", 63);
			literals.Add("constructor", 18);
			literals.Add("interface", 37);
			literals.Add("of", 47);
			literals.Add("is", 39);
			literals.Add("internal", 38);
			literals.Add("final", 30);
			literals.Add("yield", 70);
			literals.Add("or", 48);
			literals.Add("destructor", 20);
			literals.Add("if", 41);
			literals.Add("override", 49);
			literals.Add("as", 11);
			literals.Add("try", 62);
			literals.Add("goto", 35);
			literals.Add("enum", 26);
			literals.Add("isa", 40);
			literals.Add("for", 32);
			literals.Add("char", 16);
			literals.Add("private", 52);
			literals.Add("false", 33);
			literals.Add("static", 59);
			literals.Add("abstract", 9);
			literals.Add("partial", 68);
			literals.Add("callable", 14);
			literals.Add("get", 34);
			literals.Add("continue", 13);
			literals.Add("cast", 15);
			literals.Add("struct", 60);
			literals.Add("else", 23);
			literals.Add("import", 36);
			literals.Add("elif", 22);
			literals.Add("in", 42);
			literals.Add("self", 57);
			literals.Add("ensure", 25);
			literals.Add("true", 64);
			literals.Add("virtual", 67);
		}

		public override IToken nextToken()
		{
			IToken token = null;
			while (true)
			{
				bool flag = true;
				IToken token2 = null;
				int num = 0;
				resetText();
				try
				{
					try
					{
						switch (cached_LA1)
						{
						case '\\':
							mLINE_CONTINUATION(_createToken: true);
							token = returnToken_;
							break;
						case '\n':
						case '\r':
							mNEWLINE(_createToken: true);
							token = returnToken_;
							break;
						case '#':
							mSL_COMMENT(_createToken: true);
							token = returnToken_;
							break;
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							mINT(_createToken: true);
							token = returnToken_;
							break;
						case '.':
							mDOT(_createToken: true);
							token = returnToken_;
							break;
						case ':':
							mCOLON(_createToken: true);
							token = returnToken_;
							break;
						case '&':
							mBITWISE_AND(_createToken: true);
							token = returnToken_;
							break;
						case '^':
							mEXCLUSIVE_OR(_createToken: true);
							token = returnToken_;
							break;
						case '(':
							mLPAREN(_createToken: true);
							token = returnToken_;
							break;
						case ')':
							mRPAREN(_createToken: true);
							token = returnToken_;
							break;
						case ']':
							mRBRACK(_createToken: true);
							token = returnToken_;
							break;
						case '{':
							mLBRACE(_createToken: true);
							token = returnToken_;
							break;
						case '}':
							mRBRACE(_createToken: true);
							token = returnToken_;
							break;
						case '$':
							mSPLICE_BEGIN(_createToken: true);
							token = returnToken_;
							break;
						case '%':
							mMODULUS(_createToken: true);
							token = returnToken_;
							break;
						case '/':
							mDIVISION(_createToken: true);
							token = returnToken_;
							break;
						case '~':
							mONES_COMPLEMENT(_createToken: true);
							token = returnToken_;
							break;
						case '=':
							mASSIGN(_createToken: true);
							token = returnToken_;
							break;
						case ',':
							mCOMMA(_createToken: true);
							token = returnToken_;
							break;
						case '"':
							mDOUBLE_QUOTED_STRING(_createToken: true);
							token = returnToken_;
							break;
						case '\'':
							mSINGLE_QUOTED_STRING(_createToken: true);
							token = returnToken_;
							break;
						case '`':
							mBACKTICK_QUOTED_STRING(_createToken: true);
							token = returnToken_;
							break;
						case '\t':
						case '\f':
						case ' ':
							mWS(_createToken: true);
							token = returnToken_;
							break;
						case ';':
							mEOS(_createToken: true);
							token = returnToken_;
							break;
						case '?':
							mNULLABLE_SUFFIX(_createToken: true);
							token = returnToken_;
							break;
						default:
							if (cached_LA1 == '<' && cached_LA2 == '<' && LA(3) == '=')
							{
								mINPLACE_SHIFT_LEFT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '>' && cached_LA2 == '>' && LA(3) == '=')
							{
								mINPLACE_SHIFT_RIGHT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '[' && cached_LA2 == '|')
							{
								mQQ_BEGIN(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '|' && cached_LA2 == ']')
							{
								mQQ_END(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '+' && cached_LA2 == '+')
							{
								mINCREMENT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '-' && cached_LA2 == '-')
							{
								mDECREMENT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '*' && cached_LA2 == '*')
							{
								mEXPONENTIATION(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '<' && cached_LA2 == '<')
							{
								mSHIFT_LEFT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '>' && cached_LA2 == '>')
							{
								mSHIFT_RIGHT(_createToken: true);
								token = returnToken_;
								break;
							}
							if ((cached_LA1 == '!' || cached_LA1 == '<' || cached_LA1 == '>') && (cached_LA2 == '=' || cached_LA2 == '~'))
							{
								mCMP_OPERATOR(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '@' && cached_LA2 == '/')
							{
								mX_RE_LITERAL(_createToken: true);
								token = returnToken_;
								break;
							}
							if (tokenSet_0_.member(cached_LA1))
							{
								mID(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '|')
							{
								mBITWISE_OR(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '[')
							{
								mLBRACK(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '+')
							{
								mADD(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '-')
							{
								mSUBTRACT(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '*')
							{
								mMULTIPLY(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '<')
							{
								mLESS_THAN(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == '>')
							{
								mGREATER_THAN(_createToken: true);
								token = returnToken_;
								break;
							}
							if (cached_LA1 == CharScanner.EOF_CHAR)
							{
								uponEOF();
								returnToken_ = makeToken(1);
								break;
							}
							throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
						}
						if (null == returnToken_)
						{
							continue;
						}
						num = returnToken_.Type;
						returnToken_.Type = num;
						return returnToken_;
					}
					catch (RecognitionException re)
					{
						throw new TokenStreamRecognitionException(re);
					}
				}
				catch (CharStreamException ex)
				{
					if (ex is CharStreamIOException)
					{
						throw new TokenStreamIOException(((CharStreamIOException)ex).io);
					}
					throw new TokenStreamException(ex.Message);
				}
			}
		}

		public void mID(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int ttype = 71;
			bool flag = false;
			if (cached_LA1 == '@' && tokenSet_1_.member(cached_LA2))
			{
				int pos = mark();
				flag = true;
				inputState.guessing++;
				try
				{
					mAT_SYMBOL(_createToken: false);
					mID_LETTER(_createToken: false);
				}
				catch (RecognitionException)
				{
					flag = false;
				}
				rewind(pos);
				inputState.guessing--;
			}
			if (flag)
			{
				mAT_SYMBOL(_createToken: false);
				mID_SUFFIX(_createToken: false);
			}
			else if (cached_LA1 == '@')
			{
				mAT_SYMBOL(_createToken: false);
			}
			else
			{
				if (!tokenSet_1_.member(cached_LA1))
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				mID_SUFFIX(_createToken: false);
			}
			ttype = testLiteralsTable(ttype);
			if (_createToken && token == null && ttype != Token.SKIP)
			{
				token = makeToken(ttype);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mAT_SYMBOL(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 136;
			match('@');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mID_LETTER(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 137;
			switch (cached_LA1)
			{
			case '_':
				match('_');
				break;
			case 'a':
			case 'b':
			case 'c':
			case 'd':
			case 'e':
			case 'f':
			case 'g':
			case 'h':
			case 'i':
			case 'j':
			case 'k':
			case 'l':
			case 'm':
			case 'n':
			case 'o':
			case 'p':
			case 'q':
			case 'r':
			case 's':
			case 't':
			case 'u':
			case 'v':
			case 'w':
			case 'x':
			case 'y':
			case 'z':
				matchRange('a', 'z');
				break;
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
			case 'G':
			case 'H':
			case 'I':
			case 'J':
			case 'K':
			case 'L':
			case 'M':
			case 'N':
			case 'O':
			case 'P':
			case 'Q':
			case 'R':
			case 'S':
			case 'T':
			case 'U':
			case 'V':
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
				matchRange('A', 'Z');
				break;
			default:
				if (cached_LA1 >= '\u0080' && cached_LA1 <= '\ufffe' && char.IsLetter(LA(1)))
				{
					matchRange('\u0080', '\ufffe');
					break;
				}
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mID_SUFFIX(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 120;
			mID_LETTER(_createToken: false);
			while (true)
			{
				bool flag = true;
				if (tokenSet_1_.member(cached_LA1))
				{
					mID_LETTER(_createToken: false);
					continue;
				}
				if (cached_LA1 >= '0' && cached_LA1 <= '9')
				{
					mDIGIT(_createToken: false);
					continue;
				}
				break;
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mDIGIT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 138;
			matchRange('0', '9');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mLINE_CONTINUATION(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 121;
			match('\\');
			int num2 = 0;
			while (true)
			{
				bool flag = true;
				switch (cached_LA1)
				{
				case '\n':
				case '\r':
					mNEWLINE(_createToken: false);
					break;
				case '\t':
				case ' ':
				{
					int num3 = 0;
					while (true)
					{
						flag = true;
						if (cached_LA1 == ' ')
						{
							match(' ');
						}
						else
						{
							if (cached_LA1 != '\t')
							{
								break;
							}
							match('\t');
						}
						num3++;
					}
					if (num3 >= 1)
					{
						break;
					}
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				case '#':
					mSL_COMMENT(_createToken: false);
					break;
				case '/':
					mML_COMMENT(_createToken: false);
					break;
				default:
					if (num2 < 1)
					{
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = Token.SKIP;
					}
					if (_createToken && token == null && num != Token.SKIP)
					{
						token = makeToken(num);
						token.setText(text.ToString(length, text.Length - length));
					}
					returnToken_ = token;
					return;
				}
				num2++;
			}
		}

		public void mNEWLINE(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 74;
			int num2 = 0;
			while (true)
			{
				bool flag = true;
				if (cached_LA1 != '\n' && cached_LA1 != '\r')
				{
					break;
				}
				switch (cached_LA1)
				{
				case '\n':
					match('\n');
					break;
				case '\r':
					match('\r');
					if (cached_LA1 == '\n')
					{
						match('\n');
					}
					break;
				default:
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				if (0 == inputState.guessing)
				{
					newline();
				}
				num2++;
			}
			if (num2 < 1)
			{
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (0 == inputState.guessing && SkipWhitespace)
			{
				num = Token.SKIP;
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSL_COMMENT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 124;
			match("#");
			while (true)
			{
				bool flag = true;
				if (tokenSet_2_.member(cached_LA1))
				{
					match(tokenSet_2_);
					continue;
				}
				break;
			}
			if (0 == inputState.guessing && !_preserveComments)
			{
				num = Token.SKIP;
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mML_COMMENT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 125;
			match("/*");
			while (true)
			{
				bool flag = true;
				if (cached_LA1 == '*' && cached_LA2 >= '\u0003' && cached_LA2 <= '\ufffe' && LA(3) >= '\u0003' && LA(3) <= '\ufffe' && LA(2) != '/')
				{
					match('*');
					continue;
				}
				bool flag2 = false;
				if (cached_LA1 == '/' && cached_LA2 == '*' && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					int pos = mark();
					flag2 = true;
					inputState.guessing++;
					try
					{
						match("/*");
					}
					catch (RecognitionException)
					{
						flag2 = false;
					}
					rewind(pos);
					inputState.guessing--;
				}
				if (flag2)
				{
					mML_COMMENT(_createToken: false);
					continue;
				}
				if (tokenSet_3_.member(cached_LA1) && cached_LA2 >= '\u0003' && cached_LA2 <= '\ufffe' && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					match(tokenSet_3_);
					continue;
				}
				if (cached_LA1 == '\n' || cached_LA1 == '\r')
				{
					mNEWLINE(_createToken: false);
					continue;
				}
				break;
			}
			match("*/");
			if (0 == inputState.guessing && !_preserveComments)
			{
				num = Token.SKIP;
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mINT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 114;
			if (cached_LA1 == '0' && cached_LA2 == 'x')
			{
				match("0x");
				int num2 = 0;
				while (true)
				{
					bool flag = true;
					if (!tokenSet_4_.member(cached_LA1))
					{
						break;
					}
					mHEXDIGIT(_createToken: false);
					num2++;
				}
				if (num2 < 1)
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				if (cached_LA1 == 'L' || cached_LA1 == 'l')
				{
					switch (cached_LA1)
					{
					case 'l':
						match('l');
						break;
					case 'L':
						match('L');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 110;
					}
				}
			}
			else
			{
				if (cached_LA1 < '0' || cached_LA1 > '9')
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				mDIGIT_GROUP(_createToken: false);
				if (cached_LA1 == 'E' || cached_LA1 == 'e')
				{
					switch (cached_LA1)
					{
					case 'e':
						match('e');
						break;
					case 'E':
						match('E');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					switch (cached_LA1)
					{
					case '+':
						match('+');
						break;
					case '-':
						match('-');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						break;
					}
					mDIGIT_GROUP(_createToken: false);
				}
				switch (cached_LA1)
				{
				case 'L':
				case 'l':
					switch (cached_LA1)
					{
					case 'l':
						match('l');
						break;
					case 'L':
						match('L');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 110;
					}
					break;
				case 'F':
				case 'f':
					switch (cached_LA1)
					{
					case 'f':
						match('f');
						break;
					case 'F':
						match('F');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 118;
					}
					break;
				default:
					if (cached_LA1 == '.' && IsDigit(LA(2)))
					{
						match('.');
						mREVERSE_DIGIT_GROUP(_createToken: false);
						if (cached_LA1 == 'E' || cached_LA1 == 'e')
						{
							switch (cached_LA1)
							{
							case 'e':
								match('e');
								break;
							case 'E':
								match('E');
								break;
							default:
								throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
							}
							switch (cached_LA1)
							{
							case '+':
								match('+');
								break;
							case '-':
								match('-');
								break;
							default:
								throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
								break;
							}
							mDIGIT_GROUP(_createToken: false);
						}
						if (cached_LA1 == 'F' || cached_LA1 == 'f')
						{
							switch (cached_LA1)
							{
							case 'f':
								match('f');
								break;
							case 'F':
								match('F');
								break;
							default:
								throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
							}
							if (0 == inputState.guessing)
							{
								num = 118;
							}
						}
						else if (0 == inputState.guessing)
						{
							num = 117;
						}
					}
					if (cached_LA1 != 'd' && cached_LA1 != 'h' && cached_LA1 != 'm' && cached_LA1 != 's')
					{
						break;
					}
					switch (cached_LA1)
					{
					case 's':
						match('s');
						break;
					case 'h':
						match('h');
						break;
					case 'd':
						match('d');
						break;
					default:
						if (cached_LA1 == 'm' && cached_LA2 == 's')
						{
							match("ms");
							break;
						}
						if (cached_LA1 == 'm')
						{
							match('m');
							break;
						}
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 119;
					}
					break;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mHEXDIGIT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 139;
			switch (cached_LA1)
			{
			case 'a':
			case 'b':
			case 'c':
			case 'd':
			case 'e':
			case 'f':
				matchRange('a', 'f');
				break;
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
				matchRange('A', 'F');
				break;
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				matchRange('0', '9');
				break;
			default:
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mDIGIT_GROUP(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 134;
			mDIGIT(_createToken: false);
			while (true)
			{
				bool flag = true;
				switch (cached_LA1)
				{
				case '_':
				{
					int num2 = 0;
					num2 = text.Length;
					match('_');
					text.Length = num2;
					mDIGIT(_createToken: false);
					mDIGIT(_createToken: false);
					mDIGIT(_createToken: false);
					break;
				}
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					mDIGIT(_createToken: false);
					break;
				default:
					if (_createToken && token == null && num != Token.SKIP)
					{
						token = makeToken(num);
						token.setText(text.ToString(length, text.Length - length));
					}
					returnToken_ = token;
					return;
				}
			}
		}

		protected void mREVERSE_DIGIT_GROUP(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 135;
			int num2 = 0;
			while (true)
			{
				bool flag = true;
				if (cached_LA1 >= '0' && cached_LA1 <= '9' && cached_LA2 >= '0' && cached_LA2 <= '9' && LA(3) >= '0' && LA(3) <= '9')
				{
					mDIGIT(_createToken: false);
					mDIGIT(_createToken: false);
					mDIGIT(_createToken: false);
					if (cached_LA1 == '_' && IsDigit(LA(2)))
					{
						int num3 = 0;
						num3 = text.Length;
						match('_');
						text.Length = num3;
					}
				}
				else
				{
					if (cached_LA1 < '0' || cached_LA1 > '9')
					{
						break;
					}
					mDIGIT(_createToken: false);
				}
				num2++;
			}
			if (num2 < 1)
			{
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mDOT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 86;
			match('.');
			if (cached_LA1 >= '0' && cached_LA1 <= '9')
			{
				mREVERSE_DIGIT_GROUP(_createToken: false);
				if (cached_LA1 == 'E' || cached_LA1 == 'e')
				{
					switch (cached_LA1)
					{
					case 'e':
						match('e');
						break;
					case 'E':
						match('E');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					switch (cached_LA1)
					{
					case '+':
						match('+');
						break;
					case '-':
						match('-');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						break;
					}
					mDIGIT_GROUP(_createToken: false);
				}
				switch (cached_LA1)
				{
				case 'F':
				case 'f':
					switch (cached_LA1)
					{
					case 'f':
						match('f');
						break;
					case 'F':
						match('F');
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 118;
					}
					break;
				case 'd':
				case 'h':
				case 'm':
				case 's':
					switch (cached_LA1)
					{
					case 's':
						match('s');
						break;
					case 'h':
						match('h');
						break;
					case 'd':
						match('d');
						break;
					default:
						if (cached_LA1 == 'm' && cached_LA2 == 's')
						{
							match("ms");
							break;
						}
						if (cached_LA1 == 'm')
						{
							match('m');
							break;
						}
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = 119;
					}
					break;
				default:
					if (0 == inputState.guessing)
					{
						num = 117;
					}
					break;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mCOLON(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 87;
			match(':');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mBITWISE_OR(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 90;
			match('|');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 95;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mBITWISE_AND(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 107;
			match('&');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 97;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mEXCLUSIVE_OR(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 104;
			match('^');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 96;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mLPAREN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 75;
			match('(');
			if (0 == inputState.guessing)
			{
				EnterSkipWhitespaceRegion();
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mRPAREN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 76;
			match(')');
			if (0 == inputState.guessing)
			{
				LeaveSkipWhitespaceRegion();
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mLBRACK(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 80;
			match('[');
			if (0 == inputState.guessing)
			{
				EnterSkipWhitespaceRegion();
			}
			bool flag = false;
			if (cached_LA1 == 'a' || cached_LA1 == 'm')
			{
				int pos = mark();
				flag = true;
				inputState.guessing++;
				try
				{
					switch (cached_LA1)
					{
					case 'm':
						match("module:");
						break;
					case 'a':
						match("assembly:");
						break;
					default:
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
				}
				catch (RecognitionException)
				{
					flag = false;
				}
				rewind(pos);
				inputState.guessing--;
			}
			if (flag)
			{
				switch (cached_LA1)
				{
				case 'm':
					match("module:");
					if (0 == inputState.guessing)
					{
						num = 8;
					}
					break;
				case 'a':
					match("assembly:");
					if (0 == inputState.guessing)
					{
						num = 7;
					}
					break;
				default:
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mRBRACK(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 81;
			match(']');
			if (0 == inputState.guessing)
			{
				LeaveSkipWhitespaceRegion();
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mLBRACE(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 91;
			match('{');
			if (0 == inputState.guessing)
			{
				EnterSkipWhitespaceRegion();
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mRBRACE(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 92;
			match('}');
			if (0 == inputState.guessing)
			{
				LeaveSkipWhitespaceRegion();
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSPLICE_BEGIN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 85;
			match("$");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mQQ_BEGIN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 93;
			match("[|");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mQQ_END(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 94;
			match("|]");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mINCREMENT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 111;
			match("++");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mDECREMENT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 112;
			match("--");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mADD(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 103;
			match('+');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 82;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSUBTRACT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 83;
			match('-');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 82;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mMODULUS(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 106;
			match('%');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 82;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mMULTIPLY(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 79;
			match('*');
			if (cached_LA1 == '=')
			{
				match('=');
				if (0 == inputState.guessing)
				{
					num = 82;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mEXPONENTIATION(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 89;
			match("**");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mDIVISION(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 105;
			bool flag = false;
			if (cached_LA1 == '/' && cached_LA2 == '*' && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
			{
				int pos = mark();
				flag = true;
				inputState.guessing++;
				try
				{
					match("/*");
				}
				catch (RecognitionException)
				{
					flag = false;
				}
				rewind(pos);
				inputState.guessing--;
			}
			if (flag)
			{
				mML_COMMENT(_createToken: false);
				if (0 == inputState.guessing && !_preserveComments)
				{
					num = Token.SKIP;
				}
			}
			else
			{
				bool flag2 = false;
				if (cached_LA1 == '/' && tokenSet_5_.member(cached_LA2) && tokenSet_6_.member(LA(3)))
				{
					int pos2 = mark();
					flag2 = true;
					inputState.guessing++;
					try
					{
						mRE_LITERAL(_createToken: false);
					}
					catch (RecognitionException)
					{
						flag2 = false;
					}
					rewind(pos2);
					inputState.guessing--;
				}
				if (flag2)
				{
					mRE_LITERAL(_createToken: false);
					if (0 == inputState.guessing)
					{
						num = 116;
					}
				}
				else
				{
					if (cached_LA1 != '/')
					{
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					match('/');
					switch (cached_LA1)
					{
					case '/':
						match('/');
						while (true)
						{
							bool flag3 = true;
							if (tokenSet_2_.member(cached_LA1))
							{
								match(tokenSet_2_);
								continue;
							}
							break;
						}
						if (0 == inputState.guessing)
						{
							num = ((!_preserveComments) ? Token.SKIP : 124);
						}
						break;
					case '=':
						match('=');
						if (0 == inputState.guessing)
						{
							num = 82;
						}
						break;
					}
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mRE_LITERAL(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 116;
			match('/');
			int num2 = 0;
			while (true)
			{
				bool flag = true;
				if (!tokenSet_5_.member(cached_LA1))
				{
					break;
				}
				mRE_CHAR(_createToken: false);
				num2++;
			}
			if (num2 < 1)
			{
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			match('/');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mLESS_THAN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 102;
			match('<');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSHIFT_LEFT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 108;
			match("<<");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mINPLACE_SHIFT_LEFT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 98;
			match("<<=");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mGREATER_THAN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 101;
			match('>');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSHIFT_RIGHT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 109;
			match(">>");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mINPLACE_SHIFT_RIGHT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 99;
			match(">>=");
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mONES_COMPLEMENT(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 113;
			match('~');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mCMP_OPERATOR(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 100;
			switch (cached_LA1)
			{
			case '<':
				match("<=");
				break;
			case '>':
				match(">=");
				break;
			default:
				if (cached_LA1 == '!' && cached_LA2 == '~')
				{
					match("!~");
					break;
				}
				if (cached_LA1 == '!' && cached_LA2 == '=')
				{
					match("!=");
					break;
				}
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mASSIGN(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 82;
			match('=');
			if (cached_LA1 == '=' || cached_LA1 == '~')
			{
				switch (cached_LA1)
				{
				case '=':
					match('=');
					break;
				case '~':
					match('~');
					break;
				default:
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				if (0 == inputState.guessing)
				{
					num = 100;
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mCOMMA(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 84;
			match(',');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mTRIPLE_QUOTED_STRING(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 72;
			int num2 = 0;
			num2 = text.Length;
			match("\"\"");
			text.Length = num2;
			while (true)
			{
				bool flag = true;
				if (cached_LA1 == '"' && cached_LA2 == '"' && LA(3) == '"')
				{
					break;
				}
				bool flag2 = false;
				if (cached_LA1 == '$' && (cached_LA2 == '(' || cached_LA2 == '{') && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					int pos = mark();
					flag2 = true;
					inputState.guessing++;
					try
					{
						if (cached_LA1 == '$' && cached_LA2 == '{')
						{
							match("${");
						}
						else
						{
							if (cached_LA1 != '$' || cached_LA2 != '(')
							{
								throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
							}
							match("$(");
						}
					}
					catch (RecognitionException)
					{
						flag2 = false;
					}
					rewind(pos);
					inputState.guessing--;
				}
				if (flag2)
				{
					if (0 == inputState.guessing)
					{
						Enqueue(makeToken(72), text.ToString(length, text.Length - length));
						text.Length = length;
						text.Append("");
					}
					mINTERPOLATED_EXPRESSION(_createToken: false);
					continue;
				}
				bool flag3 = false;
				if (cached_LA1 == '$' && tokenSet_0_.member(cached_LA2) && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					int pos2 = mark();
					flag3 = true;
					inputState.guessing++;
					try
					{
						match('$');
						mID(_createToken: false);
					}
					catch (RecognitionException)
					{
						flag3 = false;
					}
					rewind(pos2);
					inputState.guessing--;
				}
				if (flag3)
				{
					if (0 == inputState.guessing)
					{
						Enqueue(makeToken(72), text.ToString(length, text.Length - length));
						text.Length = length;
						text.Append("");
					}
					mINTERPOLATED_REFERENCE(_createToken: false);
					continue;
				}
				bool flag4 = false;
				if (cached_LA1 == '\\' && cached_LA2 == '$' && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					int pos3 = mark();
					flag4 = true;
					inputState.guessing++;
					try
					{
						match("\\$");
					}
					catch (RecognitionException)
					{
						flag4 = false;
					}
					rewind(pos3);
					inputState.guessing--;
				}
				if (flag4)
				{
					num2 = text.Length;
					match('\\');
					text.Length = num2;
					match('$');
				}
				else if (tokenSet_2_.member(cached_LA1) && cached_LA2 >= '\u0003' && cached_LA2 <= '\ufffe' && LA(3) >= '\u0003' && LA(3) <= '\ufffe')
				{
					match(tokenSet_2_);
				}
				else
				{
					if (cached_LA1 != '\n' && cached_LA1 != '\r')
					{
						break;
					}
					mNEWLINE(_createToken: false);
				}
			}
			num2 = text.Length;
			match("\"\"\"");
			text.Length = num2;
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mINTERPOLATED_EXPRESSION(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 122;
			if (cached_LA1 == '$' && cached_LA2 == '{')
			{
				int num2 = 0;
				num2 = text.Length;
				match("${");
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					ParseInterpolatedExpression(92, 91);
				}
			}
			else
			{
				if (cached_LA1 != '$' || cached_LA2 != '(')
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				int num2 = 0;
				num2 = text.Length;
				match("$(");
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					ParseInterpolatedExpression(76, 75);
				}
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mINTERPOLATED_REFERENCE(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 123;
			IToken token2 = null;
			int num2 = 0;
			num2 = text.Length;
			match("$");
			text.Length = num2;
			num2 = text.Length;
			mID(_createToken: true);
			text.Length = num2;
			token2 = returnToken_;
			if (0 == inputState.guessing)
			{
				EnqueueInterpolatedToken(token2);
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mDOUBLE_QUOTED_STRING(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 77;
			int num2 = 0;
			num2 = text.Length;
			match('"');
			text.Length = num2;
			if (cached_LA1 == '"' && cached_LA2 == '"' && LA(3) >= '\u0003' && LA(3) <= '\ufffe' && LA(1) == '"' && LA(2) == '"')
			{
				mTRIPLE_QUOTED_STRING(_createToken: false);
				if (0 == inputState.guessing)
				{
					num = 72;
				}
			}
			else
			{
				if (!tokenSet_2_.member(cached_LA1))
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				while (true)
				{
					bool flag = true;
					bool flag2 = false;
					if (cached_LA1 == '$' && (cached_LA2 == '(' || cached_LA2 == '{') && tokenSet_2_.member(LA(3)))
					{
						int pos = mark();
						flag2 = true;
						inputState.guessing++;
						try
						{
							if (cached_LA1 == '$' && cached_LA2 == '{')
							{
								match("${");
							}
							else
							{
								if (cached_LA1 != '$' || cached_LA2 != '(')
								{
									throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
								}
								match("$(");
							}
						}
						catch (RecognitionException)
						{
							flag2 = false;
						}
						rewind(pos);
						inputState.guessing--;
					}
					if (flag2)
					{
						if (0 == inputState.guessing)
						{
							Enqueue(makeToken(77), text.ToString(length, text.Length - length));
							text.Length = length;
							text.Append("");
						}
						mINTERPOLATED_EXPRESSION(_createToken: false);
						continue;
					}
					bool flag3 = false;
					if (cached_LA1 == '$' && tokenSet_0_.member(cached_LA2) && tokenSet_2_.member(LA(3)))
					{
						int pos2 = mark();
						flag3 = true;
						inputState.guessing++;
						try
						{
							match('$');
							mID(_createToken: false);
						}
						catch (RecognitionException)
						{
							flag3 = false;
						}
						rewind(pos2);
						inputState.guessing--;
					}
					if (flag3)
					{
						if (0 == inputState.guessing)
						{
							Enqueue(makeToken(77), text.ToString(length, text.Length - length));
							text.Length = length;
							text.Append("");
						}
						mINTERPOLATED_REFERENCE(_createToken: false);
					}
					else if (tokenSet_7_.member(cached_LA1) && tokenSet_2_.member(cached_LA2))
					{
						match(tokenSet_7_);
					}
					else
					{
						if (cached_LA1 != '\\')
						{
							break;
						}
						mDQS_ESC(_createToken: false);
					}
				}
				num2 = text.Length;
				match('"');
				text.Length = num2;
			}
			if (0 == inputState.guessing && _erecorder.Count > 0)
			{
				Enqueue(makeToken(77), text.ToString(length, text.Length - length));
				num = 6;
				text.Length = length;
				text.Append("");
				_selector.push(_erecorder);
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mDQS_ESC(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 128;
			int num2 = 0;
			num2 = text.Length;
			match('\\');
			text.Length = num2;
			switch (cached_LA1)
			{
			case '0':
			case '\\':
			case 'a':
			case 'b':
			case 'f':
			case 'n':
			case 'r':
			case 't':
			case 'u':
				mSESC(_createToken: false);
				break;
			case '"':
				match('"');
				break;
			case '$':
				match('$');
				break;
			default:
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mSINGLE_QUOTED_STRING(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 78;
			int num2 = 0;
			num2 = text.Length;
			match('\'');
			text.Length = num2;
			while (true)
			{
				bool flag = true;
				if (cached_LA1 == '\\')
				{
					mSQS_ESC(_createToken: false);
					continue;
				}
				if (tokenSet_8_.member(cached_LA1))
				{
					match(tokenSet_8_);
					continue;
				}
				break;
			}
			num2 = text.Length;
			match('\'');
			text.Length = num2;
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mSQS_ESC(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 129;
			int num2 = 0;
			num2 = text.Length;
			match('\\');
			text.Length = num2;
			switch (cached_LA1)
			{
			case '0':
			case '\\':
			case 'a':
			case 'b':
			case 'f':
			case 'n':
			case 'r':
			case 't':
			case 'u':
				mSESC(_createToken: false);
				break;
			case '\'':
				match('\'');
				break;
			default:
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mBACKTICK_QUOTED_STRING(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 115;
			int num2 = 0;
			num2 = text.Length;
			match('`');
			text.Length = num2;
			while (true)
			{
				bool flag = true;
				if (tokenSet_9_.member(cached_LA1))
				{
					match(tokenSet_9_);
					continue;
				}
				if (cached_LA1 == '\n' || cached_LA1 == '\r')
				{
					mNEWLINE(_createToken: false);
					continue;
				}
				break;
			}
			num2 = text.Length;
			match('`');
			text.Length = num2;
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mWS(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 126;
			int num2 = 0;
			while (true)
			{
				bool flag = true;
				switch (cached_LA1)
				{
				case ' ':
					match(' ');
					break;
				case '\t':
					match('\t');
					if (0 == inputState.guessing)
					{
						tab();
					}
					break;
				case '\f':
					match('\f');
					break;
				default:
					if (num2 < 1)
					{
						throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
					}
					if (0 == inputState.guessing)
					{
						num = Token.SKIP;
					}
					if (_createToken && token == null && num != Token.SKIP)
					{
						token = makeToken(num);
						token.setText(text.ToString(length, text.Length - length));
					}
					returnToken_ = token;
					return;
				}
				num2++;
			}
		}

		public void mEOS(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 73;
			match(';');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mX_RE_LITERAL(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 127;
			int num2 = 0;
			num2 = text.Length;
			match('@');
			text.Length = num2;
			match('/');
			int num3 = 0;
			while (true)
			{
				bool flag = true;
				if (!tokenSet_10_.member(cached_LA1))
				{
					break;
				}
				mX_RE_CHAR(_createToken: false);
				num3++;
			}
			if (num3 < 1)
			{
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			match('/');
			if (0 == inputState.guessing)
			{
				num = 116;
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mX_RE_CHAR(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 132;
			switch (cached_LA1)
			{
			case ' ':
				match(' ');
				break;
			case '\t':
				match('\t');
				break;
			default:
				if (tokenSet_5_.member(cached_LA1))
				{
					mRE_CHAR(_createToken: false);
					break;
				}
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mSESC(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 130;
			switch (cached_LA1)
			{
			case 'r':
			{
				int num2 = 0;
				num2 = text.Length;
				match('r');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\r");
				}
				break;
			}
			case 'n':
			{
				int num2 = 0;
				num2 = text.Length;
				match('n');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\n");
				}
				break;
			}
			case 't':
			{
				int num2 = 0;
				num2 = text.Length;
				match('t');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\t");
				}
				break;
			}
			case 'a':
			{
				int num2 = 0;
				num2 = text.Length;
				match('a');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\a");
				}
				break;
			}
			case 'b':
			{
				int num2 = 0;
				num2 = text.Length;
				match('b');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\b");
				}
				break;
			}
			case 'f':
			{
				int num2 = 0;
				num2 = text.Length;
				match('f');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\f");
				}
				break;
			}
			case '0':
			{
				int num2 = 0;
				num2 = text.Length;
				match('0');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\0");
				}
				break;
			}
			case 'u':
			{
				int num2 = 0;
				num2 = text.Length;
				match('u');
				text.Length = num2;
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				if (0 == inputState.guessing)
				{
					char value = (char)int.Parse(text.ToString(length, 4), NumberStyles.HexNumber);
					text.Length = length;
					text.Append(value);
				}
				break;
			}
			case '\\':
			{
				int num2 = 0;
				num2 = text.Length;
				match('\\');
				text.Length = num2;
				if (0 == inputState.guessing)
				{
					text.Length = length;
					text.Append("\\");
				}
				break;
			}
			default:
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mRE_CHAR(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 131;
			if (cached_LA1 == '\\')
			{
				mRE_ESC(_createToken: false);
			}
			else
			{
				if (!tokenSet_11_.member(cached_LA1))
				{
					throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
				}
				match(tokenSet_11_);
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		protected void mRE_ESC(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 133;
			match('\\');
			switch (cached_LA1)
			{
			case '+':
				match('+');
				break;
			case '/':
				match('/');
				break;
			case '(':
				match('(');
				break;
			case ')':
				match(')');
				break;
			case '|':
				match('|');
				break;
			case '.':
				match('.');
				break;
			case '*':
				match('*');
				break;
			case '?':
				match('?');
				break;
			case '$':
				match('$');
				break;
			case '^':
				match('^');
				break;
			case '[':
				match('[');
				break;
			case ']':
				match(']');
				break;
			case '{':
				match('{');
				break;
			case '}':
				match('}');
				break;
			case 'a':
				match('a');
				break;
			case 'b':
				match('b');
				break;
			case 'c':
				match('c');
				matchRange('A', 'Z');
				break;
			case 't':
				match('t');
				break;
			case 'r':
				match('r');
				break;
			case 'v':
				match('v');
				break;
			case 'f':
				match('f');
				break;
			case 'n':
				match('n');
				break;
			case 'e':
				match('e');
				break;
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
			{
				int num2 = 0;
				while (true)
				{
					bool flag = true;
					if (cached_LA1 < '0' || cached_LA1 > '9' || !tokenSet_2_.member(cached_LA2))
					{
						break;
					}
					mDIGIT(_createToken: false);
					num2++;
				}
				if (num2 >= 1)
				{
					break;
				}
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			case 'x':
				match('x');
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				break;
			case 'u':
				match('u');
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				mHEXDIGIT(_createToken: false);
				break;
			case '\\':
				match('\\');
				break;
			case 'w':
				match('w');
				break;
			case 'W':
				match('W');
				break;
			case 's':
				match('s');
				break;
			case 'S':
				match('S');
				break;
			case 'd':
				match('d');
				break;
			case 'D':
				match('D');
				break;
			case 'p':
				match('p');
				break;
			case 'P':
				match('P');
				break;
			case 'A':
				match('A');
				break;
			case 'z':
				match('z');
				break;
			case 'Z':
				match('Z');
				break;
			case 'g':
				match('g');
				break;
			case 'B':
				match('B');
				break;
			case 'k':
				match('k');
				break;
			default:
				throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
			}
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		public void mNULLABLE_SUFFIX(bool _createToken)
		{
			IToken token = null;
			int length = text.Length;
			int num = 88;
			match('?');
			if (_createToken && token == null && num != Token.SKIP)
			{
				token = makeToken(num);
				token.setText(text.ToString(length, text.Length - length));
			}
			returnToken_ = token;
		}

		private static long[] mk_tokenSet_0_()
		{
			long[] array = new long[3072];
			array[0] = 0L;
			array[1] = 576460745995190271L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 3071; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_1_()
		{
			long[] array = new long[3072];
			array[0] = 0L;
			array[1] = 576460745995190270L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 3071; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_2_()
		{
			long[] array = new long[2048];
			array[0] = -9224L;
			for (int i = 1; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_3_()
		{
			long[] array = new long[2048];
			array[0] = -4398046520328L;
			for (int i = 1; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_4_()
		{
			long[] array = new long[1025];
			array[0] = 287948901175001088L;
			array[1] = 541165879422L;
			for (int i = 2; i <= 1024; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_5_()
		{
			long[] array = new long[2048];
			array[0] = -140741783332360L;
			for (int i = 1; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_6_()
		{
			long[] array = new long[2048];
			array[0] = -4294977032L;
			for (int i = 1; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_7_()
		{
			long[] array = new long[2048];
			array[0] = -17179878408L;
			array[1] = -268435457L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_8_()
		{
			long[] array = new long[2048];
			array[0] = -549755823112L;
			array[1] = -268435457L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_9_()
		{
			long[] array = new long[2048];
			array[0] = -9224L;
			array[1] = -4294967297L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_10_()
		{
			long[] array = new long[2048];
			array[0] = -140737488364552L;
			for (int i = 1; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}

		private static long[] mk_tokenSet_11_()
		{
			long[] array = new long[2048];
			array[0] = -140741783332360L;
			array[1] = -268435457L;
			for (int i = 2; i <= 1022; i++)
			{
				array[i] = -1L;
			}
			array[1023] = long.MaxValue;
			for (int i = 1024; i <= 2047; i++)
			{
				array[i] = 0L;
			}
			return array;
		}
	}
}
