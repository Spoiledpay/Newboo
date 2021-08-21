using System.Text;
using antlr;
using antlr.collections.impl;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Parser
{
	public class WSABooParserBase : LLkParser
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

		protected StringBuilder _sbuilder = new StringBuilder();

		protected AttributeCollection _attributes = new AttributeCollection();

		protected TypeMemberModifiers _modifiers = TypeMemberModifiers.None;

		protected bool _inArray;

		public static readonly string[] tokenNames_ = new string[140]
		{
			"\"<0>\"", "\"EOF\"", "\"<2>\"", "\"NULL_TREE_LOOKAHEAD\"", "\"ELIST\"", "\"DLIST\"", "\"ESEPARATOR\"", "\"ASSEMBLY_ATTRIBUTE_BEGIN\"", "\"MODULE_ATTRIBUTE_BEGIN\"", "\"abstract\"",
			"\"and\"", "\"as\"", "\"break\"", "\"continue\"", "\"callable\"", "\"cast\"", "\"char\"", "\"class\"", "\"constructor\"", "\"def\"",
			"\"destructor\"", "\"do\"", "\"elif\"", "\"else\"", "\"end\"", "\"ensure\"", "\"enum\"", "\"event\"", "\"except\"", "\"failure\"",
			"\"final\"", "\"from\"", "\"for\"", "\"false\"", "\"get\"", "\"goto\"", "\"import\"", "\"interface\"", "\"internal\"", "\"is\"",
			"\"isa\"", "\"if\"", "\"in\"", "\"namespace\"", "\"new\"", "\"not\"", "\"null\"", "\"of\"", "\"or\"", "\"override\"",
			"\"public\"", "\"protected\"", "\"private\"", "\"raise\"", "\"ref\"", "\"return\"", "\"set\"", "\"self\"", "\"super\"", "\"static\"",
			"\"struct\"", "\"then\"", "\"try\"", "\"transient\"", "\"true\"", "\"typeof\"", "\"unless\"", "\"virtual\"", "\"partial\"", "\"while\"",
			"\"yield\"", "\"ID\"", "\"TRIPLE_QUOTED_STRING\"", "\"EOS\"", "\"NEWLINE\"", "\"LPAREN\"", "\"RPAREN\"", "\"DOUBLE_QUOTED_STRING\"", "\"SINGLE_QUOTED_STRING\"", "\"MULTIPLY\"",
			"\"LBRACK\"", "\"RBRACK\"", "\"ASSIGN\"", "\"SUBTRACT\"", "\"COMMA\"", "\"SPLICE_BEGIN\"", "\"DOT\"", "\"COLON\"", "\"NULLABLE_SUFFIX\"", "\"EXPONENTIATION\"",
			"\"BITWISE_OR\"", "\"LBRACE\"", "\"RBRACE\"", "\"QQ_BEGIN\"", "\"QQ_END\"", "\"INPLACE_BITWISE_OR\"", "\"INPLACE_EXCLUSIVE_OR\"", "\"INPLACE_BITWISE_AND\"", "\"INPLACE_SHIFT_LEFT\"", "\"INPLACE_SHIFT_RIGHT\"",
			"\"CMP_OPERATOR\"", "\"GREATER_THAN\"", "\"LESS_THAN\"", "\"ADD\"", "\"EXCLUSIVE_OR\"", "\"DIVISION\"", "\"MODULUS\"", "\"BITWISE_AND\"", "\"SHIFT_LEFT\"", "\"SHIFT_RIGHT\"",
			"\"LONG\"", "\"INCREMENT\"", "\"DECREMENT\"", "\"ONES_COMPLEMENT\"", "\"INT\"", "\"BACKTICK_QUOTED_STRING\"", "\"RE_LITERAL\"", "\"DOUBLE\"", "\"FLOAT\"", "\"TIMESPAN\"",
			"\"ID_SUFFIX\"", "\"LINE_CONTINUATION\"", "\"INTERPOLATED_EXPRESSION\"", "\"INTERPOLATED_REFERENCE\"", "\"SL_COMMENT\"", "\"ML_COMMENT\"", "\"WS\"", "\"X_RE_LITERAL\"", "\"DQS_ESC\"", "\"SQS_ESC\"",
			"\"SESC\"", "\"RE_CHAR\"", "\"X_RE_CHAR\"", "\"RE_ESC\"", "\"DIGIT_GROUP\"", "\"REVERSE_DIGIT_GROUP\"", "\"AT_SYMBOL\"", "\"ID_LETTER\"", "\"DIGIT\"", "\"HEXDIGIT\""
		};

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

		public static readonly BitSet tokenSet_12_ = new BitSet(mk_tokenSet_12_());

		public static readonly BitSet tokenSet_13_ = new BitSet(mk_tokenSet_13_());

		public static readonly BitSet tokenSet_14_ = new BitSet(mk_tokenSet_14_());

		public static readonly BitSet tokenSet_15_ = new BitSet(mk_tokenSet_15_());

		public static readonly BitSet tokenSet_16_ = new BitSet(mk_tokenSet_16_());

		public static readonly BitSet tokenSet_17_ = new BitSet(mk_tokenSet_17_());

		public static readonly BitSet tokenSet_18_ = new BitSet(mk_tokenSet_18_());

		public static readonly BitSet tokenSet_19_ = new BitSet(mk_tokenSet_19_());

		public static readonly BitSet tokenSet_20_ = new BitSet(mk_tokenSet_20_());

		public static readonly BitSet tokenSet_21_ = new BitSet(mk_tokenSet_21_());

		public static readonly BitSet tokenSet_22_ = new BitSet(mk_tokenSet_22_());

		public static readonly BitSet tokenSet_23_ = new BitSet(mk_tokenSet_23_());

		public static readonly BitSet tokenSet_24_ = new BitSet(mk_tokenSet_24_());

		public static readonly BitSet tokenSet_25_ = new BitSet(mk_tokenSet_25_());

		public static readonly BitSet tokenSet_26_ = new BitSet(mk_tokenSet_26_());

		public static readonly BitSet tokenSet_27_ = new BitSet(mk_tokenSet_27_());

		public static readonly BitSet tokenSet_28_ = new BitSet(mk_tokenSet_28_());

		public static readonly BitSet tokenSet_29_ = new BitSet(mk_tokenSet_29_());

		public static readonly BitSet tokenSet_30_ = new BitSet(mk_tokenSet_30_());

		public static readonly BitSet tokenSet_31_ = new BitSet(mk_tokenSet_31_());

		public static readonly BitSet tokenSet_32_ = new BitSet(mk_tokenSet_32_());

		public static readonly BitSet tokenSet_33_ = new BitSet(mk_tokenSet_33_());

		public static readonly BitSet tokenSet_34_ = new BitSet(mk_tokenSet_34_());

		public static readonly BitSet tokenSet_35_ = new BitSet(mk_tokenSet_35_());

		public static readonly BitSet tokenSet_36_ = new BitSet(mk_tokenSet_36_());

		public static readonly BitSet tokenSet_37_ = new BitSet(mk_tokenSet_37_());

		public static readonly BitSet tokenSet_38_ = new BitSet(mk_tokenSet_38_());

		public static readonly BitSet tokenSet_39_ = new BitSet(mk_tokenSet_39_());

		public static readonly BitSet tokenSet_40_ = new BitSet(mk_tokenSet_40_());

		public static readonly BitSet tokenSet_41_ = new BitSet(mk_tokenSet_41_());

		public static readonly BitSet tokenSet_42_ = new BitSet(mk_tokenSet_42_());

		public static readonly BitSet tokenSet_43_ = new BitSet(mk_tokenSet_43_());

		public static readonly BitSet tokenSet_44_ = new BitSet(mk_tokenSet_44_());

		public static readonly BitSet tokenSet_45_ = new BitSet(mk_tokenSet_45_());

		public static readonly BitSet tokenSet_46_ = new BitSet(mk_tokenSet_46_());

		public static readonly BitSet tokenSet_47_ = new BitSet(mk_tokenSet_47_());

		public static readonly BitSet tokenSet_48_ = new BitSet(mk_tokenSet_48_());

		public static readonly BitSet tokenSet_49_ = new BitSet(mk_tokenSet_49_());

		public static readonly BitSet tokenSet_50_ = new BitSet(mk_tokenSet_50_());

		public static readonly BitSet tokenSet_51_ = new BitSet(mk_tokenSet_51_());

		public static readonly BitSet tokenSet_52_ = new BitSet(mk_tokenSet_52_());

		public static readonly BitSet tokenSet_53_ = new BitSet(mk_tokenSet_53_());

		public static readonly BitSet tokenSet_54_ = new BitSet(mk_tokenSet_54_());

		public static readonly BitSet tokenSet_55_ = new BitSet(mk_tokenSet_55_());

		public static readonly BitSet tokenSet_56_ = new BitSet(mk_tokenSet_56_());

		public static readonly BitSet tokenSet_57_ = new BitSet(mk_tokenSet_57_());

		public static readonly BitSet tokenSet_58_ = new BitSet(mk_tokenSet_58_());

		public static readonly BitSet tokenSet_59_ = new BitSet(mk_tokenSet_59_());

		public static readonly BitSet tokenSet_60_ = new BitSet(mk_tokenSet_60_());

		public static readonly BitSet tokenSet_61_ = new BitSet(mk_tokenSet_61_());

		public static readonly BitSet tokenSet_62_ = new BitSet(mk_tokenSet_62_());

		public static readonly BitSet tokenSet_63_ = new BitSet(mk_tokenSet_63_());

		public static readonly BitSet tokenSet_64_ = new BitSet(mk_tokenSet_64_());

		public static readonly BitSet tokenSet_65_ = new BitSet(mk_tokenSet_65_());

		public static readonly BitSet tokenSet_66_ = new BitSet(mk_tokenSet_66_());

		public static readonly BitSet tokenSet_67_ = new BitSet(mk_tokenSet_67_());

		public static readonly BitSet tokenSet_68_ = new BitSet(mk_tokenSet_68_());

		public static readonly BitSet tokenSet_69_ = new BitSet(mk_tokenSet_69_());

		public static readonly BitSet tokenSet_70_ = new BitSet(mk_tokenSet_70_());

		public static readonly BitSet tokenSet_71_ = new BitSet(mk_tokenSet_71_());

		public static readonly BitSet tokenSet_72_ = new BitSet(mk_tokenSet_72_());

		public static readonly BitSet tokenSet_73_ = new BitSet(mk_tokenSet_73_());

		public static readonly BitSet tokenSet_74_ = new BitSet(mk_tokenSet_74_());

		public static readonly BitSet tokenSet_75_ = new BitSet(mk_tokenSet_75_());

		public static readonly BitSet tokenSet_76_ = new BitSet(mk_tokenSet_76_());

		public static readonly BitSet tokenSet_77_ = new BitSet(mk_tokenSet_77_());

		public static readonly BitSet tokenSet_78_ = new BitSet(mk_tokenSet_78_());

		public static readonly BitSet tokenSet_79_ = new BitSet(mk_tokenSet_79_());

		public static readonly BitSet tokenSet_80_ = new BitSet(mk_tokenSet_80_());

		public static readonly BitSet tokenSet_81_ = new BitSet(mk_tokenSet_81_());

		public static readonly BitSet tokenSet_82_ = new BitSet(mk_tokenSet_82_());

		public static readonly BitSet tokenSet_83_ = new BitSet(mk_tokenSet_83_());

		public static readonly BitSet tokenSet_84_ = new BitSet(mk_tokenSet_84_());

		public static readonly BitSet tokenSet_85_ = new BitSet(mk_tokenSet_85_());

		public static readonly BitSet tokenSet_86_ = new BitSet(mk_tokenSet_86_());

		public static readonly BitSet tokenSet_87_ = new BitSet(mk_tokenSet_87_());

		public static readonly BitSet tokenSet_88_ = new BitSet(mk_tokenSet_88_());

		public static readonly BitSet tokenSet_89_ = new BitSet(mk_tokenSet_89_());

		public static readonly BitSet tokenSet_90_ = new BitSet(mk_tokenSet_90_());

		public static readonly BitSet tokenSet_91_ = new BitSet(mk_tokenSet_91_());

		public static readonly BitSet tokenSet_92_ = new BitSet(mk_tokenSet_92_());

		public static readonly BitSet tokenSet_93_ = new BitSet(mk_tokenSet_93_());

		public static readonly BitSet tokenSet_94_ = new BitSet(mk_tokenSet_94_());

		public static readonly BitSet tokenSet_95_ = new BitSet(mk_tokenSet_95_());

		public static readonly BitSet tokenSet_96_ = new BitSet(mk_tokenSet_96_());

		public static readonly BitSet tokenSet_97_ = new BitSet(mk_tokenSet_97_());

		public static readonly BitSet tokenSet_98_ = new BitSet(mk_tokenSet_98_());

		public static readonly BitSet tokenSet_99_ = new BitSet(mk_tokenSet_99_());

		public static readonly BitSet tokenSet_100_ = new BitSet(mk_tokenSet_100_());

		public static readonly BitSet tokenSet_101_ = new BitSet(mk_tokenSet_101_());

		public static readonly BitSet tokenSet_102_ = new BitSet(mk_tokenSet_102_());

		public static readonly BitSet tokenSet_103_ = new BitSet(mk_tokenSet_103_());

		public static readonly BitSet tokenSet_104_ = new BitSet(mk_tokenSet_104_());

		public static readonly BitSet tokenSet_105_ = new BitSet(mk_tokenSet_105_());

		public static readonly BitSet tokenSet_106_ = new BitSet(mk_tokenSet_106_());

		public static readonly BitSet tokenSet_107_ = new BitSet(mk_tokenSet_107_());

		public static readonly BitSet tokenSet_108_ = new BitSet(mk_tokenSet_108_());

		public static readonly BitSet tokenSet_109_ = new BitSet(mk_tokenSet_109_());

		public static readonly BitSet tokenSet_110_ = new BitSet(mk_tokenSet_110_());

		public static readonly BitSet tokenSet_111_ = new BitSet(mk_tokenSet_111_());

		public static readonly BitSet tokenSet_112_ = new BitSet(mk_tokenSet_112_());

		public static readonly BitSet tokenSet_113_ = new BitSet(mk_tokenSet_113_());

		public static readonly BitSet tokenSet_114_ = new BitSet(mk_tokenSet_114_());

		public static readonly BitSet tokenSet_115_ = new BitSet(mk_tokenSet_115_());

		public static readonly BitSet tokenSet_116_ = new BitSet(mk_tokenSet_116_());

		public static readonly BitSet tokenSet_117_ = new BitSet(mk_tokenSet_117_());

		public static readonly BitSet tokenSet_118_ = new BitSet(mk_tokenSet_118_());

		public static readonly BitSet tokenSet_119_ = new BitSet(mk_tokenSet_119_());

		public static readonly BitSet tokenSet_120_ = new BitSet(mk_tokenSet_120_());

		public static readonly BitSet tokenSet_121_ = new BitSet(mk_tokenSet_121_());

		public static readonly BitSet tokenSet_122_ = new BitSet(mk_tokenSet_122_());

		public static readonly BitSet tokenSet_123_ = new BitSet(mk_tokenSet_123_());

		public static readonly BitSet tokenSet_124_ = new BitSet(mk_tokenSet_124_());

		public static readonly BitSet tokenSet_125_ = new BitSet(mk_tokenSet_125_());

		protected void ResetMemberData()
		{
			_modifiers = TypeMemberModifiers.None;
		}

		protected void AddAttributes(AttributeCollection target)
		{
			target.Extend(_attributes);
			_attributes.Clear();
		}

		private static bool IsMethodInvocationExpression(Expression e)
		{
			return NodeType.MethodInvocationExpression == e.NodeType;
		}

		protected bool IsValidMacroArgument(int token)
		{
			return 75 != token && 80 != token && 86 != token && 79 != token;
		}

		private LexicalInfo ToLexicalInfo(IToken token)
		{
			return SourceLocationFactory.ToLexicalInfo(token);
		}

		private MemberReferenceExpression MemberReferenceForToken(Expression target, IToken memberName)
		{
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(ToLexicalInfo(memberName));
			memberReferenceExpression.Target = target;
			memberReferenceExpression.Name = memberName.getText();
			return memberReferenceExpression;
		}

		protected void initialize()
		{
			tokenNames = tokenNames_;
		}

		protected WSABooParserBase(TokenBuffer tokenBuf, int k)
			: base(tokenBuf, k)
		{
			initialize();
		}

		public WSABooParserBase(TokenBuffer tokenBuf)
			: this(tokenBuf, 2)
		{
		}

		protected WSABooParserBase(TokenStream lexer, int k)
			: base(lexer, k)
		{
			initialize();
		}

		public WSABooParserBase(TokenStream lexer)
			: this(lexer, 2)
		{
		}

		public WSABooParserBase(ParserSharedInputState state)
			: base(state, 2)
		{
			initialize();
		}

		protected Module start(CompileUnit cu)
		{
			Module module = new Module();
			module.LexicalInfo = new LexicalInfo(getFilename(), 1, 1);
			cu.Modules.Add(module);
			try
			{
				parse_module(module);
				if (LA(1) == 1 && LA(2) == 1)
				{
					match(1);
				}
				else if (LA(1) != 1 || LA(2) != 1)
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_0_);
			}
			return module;
		}

		protected void parse_module(Module module)
		{
			try
			{
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_1_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_1_.member(LA(1)) || !tokenSet_2_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				docstring(module);
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_1_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_1_.member(LA(1)) || !tokenSet_2_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				switch (LA(1))
				{
				case 43:
					namespace_directive(module);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 6:
				case 7:
				case 8:
				case 9:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 19:
				case 26:
				case 30:
				case 31:
				case 32:
				case 33:
				case 35:
				case 36:
				case 37:
				case 38:
				case 41:
				case 44:
				case 46:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 55:
				case 57:
				case 58:
				case 59:
				case 60:
				case 62:
				case 63:
				case 64:
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 87:
				case 91:
				case 93:
				case 94:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 31 || LA(1) == 36)
					{
						import_directive(module);
						continue;
					}
					break;
				}
				while (true)
				{
					bool flag = true;
					bool flag2 = false;
					if (LA(1) == 71 && tokenSet_3_.member(LA(2)) && IsValidMacroArgument(LA(2)))
					{
						int pos = mark();
						flag2 = true;
						inputState.guessing++;
						try
						{
							match(71);
							if (tokenSet_4_.member(LA(1)))
							{
								expression();
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
						module_macro(module);
						continue;
					}
					if (tokenSet_5_.member(LA(1)) && tokenSet_6_.member(LA(2)))
					{
						type_member(module.Members);
						continue;
					}
					break;
				}
				globals(module);
				while (true)
				{
					bool flag = true;
					if (LA(1) == 7 || LA(1) == 8)
					{
						switch (LA(1))
						{
						case 7:
							assembly_attribute(module);
							break;
						case 8:
							module_attribute(module);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						eos();
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex2);
					recover(ex2, tokenSet_7_);
					return;
				}
				throw ex2;
			}
		}

		protected void eos()
		{
			try
			{
				switch (LA(1))
				{
				case 1:
					match(1);
					break;
				case 73:
				case 74:
				{
					int num = 0;
					while (true)
					{
						bool flag = true;
						if ((LA(1) != 73 && LA(1) != 74) || !tokenSet_8_.member(LA(2)))
						{
							break;
						}
						switch (LA(1))
						{
						case 73:
							match(73);
							break;
						case 74:
							match(74);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						num++;
					}
					if (num >= 1)
					{
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_8_);
					return;
				}
				throw ex;
			}
		}

		protected void docstring(Node node)
		{
			IToken token = null;
			try
			{
				if (LA(1) == 72 && tokenSet_9_.member(LA(2)))
				{
					token = LT(1);
					match(72);
					if (0 == inputState.guessing)
					{
						node.Documentation = DocStringFormatter.Format(token.getText());
					}
					if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_9_.member(LA(2)))
					{
						eos();
					}
					else if (!tokenSet_9_.member(LA(1)) || !tokenSet_10_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
				}
				else if (!tokenSet_9_.member(LA(1)) || !tokenSet_10_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_9_);
					return;
				}
				throw ex;
			}
		}

		protected void namespace_directive(Module container)
		{
			IToken token = null;
			NamespaceDeclaration namespaceDeclaration = null;
			try
			{
				token = LT(1);
				match(43);
				IToken token2 = identifier();
				if (0 == inputState.guessing)
				{
					namespaceDeclaration = new NamespaceDeclaration(SourceLocationFactory.ToLexicalInfo(token));
					namespaceDeclaration.Name = token2.getText();
					container.Namespace = namespaceDeclaration;
				}
				eos();
				docstring(namespaceDeclaration);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_11_);
					return;
				}
				throw ex;
			}
		}

		protected void import_directive(Module container)
		{
			Import import = null;
			try
			{
				switch (LA(1))
				{
				case 36:
					import = import_directive_();
					eos();
					break;
				case 31:
					import = import_directive_from_();
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing && import != null)
				{
					container.Imports.Add(import);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_11_);
					return;
				}
				throw ex;
			}
		}

		protected Expression expression()
		{
			IToken token = null;
			IToken token2 = null;
			Expression result = null;
			ExtendedGeneratorExpression extendedGeneratorExpression = null;
			GeneratorExpression generatorExpression = null;
			try
			{
				result = boolean_expression();
				switch (LA(1))
				{
				case 32:
					token = LT(1);
					match(32);
					if (0 == inputState.guessing)
					{
						generatorExpression = new GeneratorExpression(SourceLocationFactory.ToLexicalInfo(token));
						generatorExpression.Expression = result;
						result = generatorExpression;
					}
					generator_expression_body(generatorExpression);
					while (true)
					{
						bool flag = true;
						if (LA(1) != 32)
						{
							break;
						}
						token2 = LT(1);
						match(32);
						if (0 == inputState.guessing)
						{
							if (null == extendedGeneratorExpression)
							{
								extendedGeneratorExpression = new ExtendedGeneratorExpression(SourceLocationFactory.ToLexicalInfo(token));
								extendedGeneratorExpression.Items.Add(generatorExpression);
								result = extendedGeneratorExpression;
							}
							generatorExpression = new GeneratorExpression(SourceLocationFactory.ToLexicalInfo(token2));
							extendedGeneratorExpression.Items.Add(generatorExpression);
						}
						generator_expression_body(generatorExpression);
					}
					break;
				case 1:
				case 6:
				case 19:
				case 21:
				case 41:
				case 66:
				case 69:
				case 71:
				case 73:
				case 74:
				case 76:
				case 81:
				case 84:
				case 87:
				case 92:
				case 94:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_12_);
			}
			return result;
		}

		protected void module_macro(Module module)
		{
			Statement statement = null;
			try
			{
				statement = macro_stmt();
				if (0 == inputState.guessing)
				{
					module.Globals.Add(statement);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_13_);
					return;
				}
				throw ex;
			}
		}

		protected void type_member(TypeMemberCollection container)
		{
			try
			{
				attributes();
				modifiers();
				switch (LA(1))
				{
				case 14:
				case 17:
				case 26:
				case 37:
				case 60:
					type_definition(container);
					break;
				case 19:
					method(container);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_14_);
					return;
				}
				throw ex;
			}
		}

		protected void globals(Module container)
		{
			try
			{
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_15_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_16_.member(LA(1)) || !tokenSet_17_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				while (true)
				{
					bool flag = true;
					if (tokenSet_18_.member(LA(1)))
					{
						stmt(container.Globals.Statements);
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_19_);
					return;
				}
				throw ex;
			}
		}

		protected void assembly_attribute(Module module)
		{
			Attribute attribute = null;
			try
			{
				match(7);
				attribute = this.attribute();
				match(81);
				if (0 == inputState.guessing)
				{
					module.AssemblyAttributes.Add(attribute);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_20_);
					return;
				}
				throw ex;
			}
		}

		protected void module_attribute(Module module)
		{
			Attribute attribute = null;
			try
			{
				match(8);
				attribute = this.attribute();
				match(81);
				if (0 == inputState.guessing)
				{
					module.Attributes.Add(attribute);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_20_);
					return;
				}
				throw ex;
			}
		}

		protected MacroStatement macro_stmt()
		{
			IToken token = null;
			MacroStatement result = null;
			MacroStatement macroStatement = new MacroStatement();
			StatementModifier statementModifier = null;
			try
			{
				token = LT(1);
				match(71);
				expression_list(macroStatement.Arguments);
				if (LA(1) == 87 && tokenSet_21_.member(LA(2)))
				{
					begin_with_doc(macroStatement);
					macro_block(macroStatement.Body.Statements);
					end(macroStatement.Body);
					if (0 == inputState.guessing)
					{
						macroStatement.Annotate("compound");
					}
				}
				else if (LA(1) == 87 && tokenSet_21_.member(LA(2)))
				{
					macro_compound_stmt(macroStatement.Body);
					if (0 == inputState.guessing)
					{
						macroStatement.Annotate("compound");
					}
				}
				else
				{
					if (!tokenSet_22_.member(LA(1)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 1:
					case 73:
					case 74:
						eos();
						break;
					case 41:
					case 66:
					case 69:
						statementModifier = stmt_modifier();
						eos();
						if (0 == inputState.guessing)
						{
							macroStatement.Modifier = statementModifier;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					docstring(macroStatement);
				}
				if (0 == inputState.guessing)
				{
					macroStatement.Name = token.getText();
					macroStatement.LexicalInfo = ToLexicalInfo(token);
					result = macroStatement;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_23_);
			}
			return result;
		}

		protected Import import_directive_()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			Expression expression = null;
			IToken token5 = null;
			Import import = null;
			try
			{
				token = LT(1);
				match(36);
				expression = namespace_expression();
				if (0 == inputState.guessing && expression != null)
				{
					import = new Import(ToLexicalInfo(token), expression);
				}
				switch (LA(1))
				{
				case 31:
					match(31);
					switch (LA(1))
					{
					case 71:
						token5 = identifier();
						break;
					case 77:
						token2 = LT(1);
						match(77);
						if (0 == inputState.guessing)
						{
							token5 = token2;
						}
						break;
					case 78:
						token3 = LT(1);
						match(78);
						if (0 == inputState.guessing)
						{
							token5 = token3;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						import.AssemblyReference = new ReferenceExpression(ToLexicalInfo(token5), token5.getText());
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 11:
				case 73:
				case 74:
					break;
				}
				switch (LA(1))
				{
				case 11:
					match(11);
					token4 = LT(1);
					match(71);
					if (0 == inputState.guessing)
					{
						import.Alias = new ReferenceExpression(ToLexicalInfo(token4));
						import.Alias.Name = token4.getText();
					}
					break;
				case 1:
				case 73:
				case 74:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_20_);
			}
			return import;
		}

		protected Import import_directive_from_()
		{
			IToken token = null;
			Expression expression = null;
			ExpressionCollection ec = null;
			Import import = null;
			try
			{
				token = LT(1);
				match(31);
				expression = identifier_expression();
				match(36);
				if (0 == inputState.guessing)
				{
					MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(expression);
					ec = methodInvocationExpression.Arguments;
					import = new Import(ToLexicalInfo(token), methodInvocationExpression);
				}
				if (LA(1) == 79 && (LA(2) == 1 || LA(2) == 73 || LA(2) == 74))
				{
					match(79);
					if (0 == inputState.guessing)
					{
						import.Expression = expression;
					}
				}
				else
				{
					if (!tokenSet_24_.member(LA(1)) || !tokenSet_25_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression_list(ec);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_20_);
			}
			return import;
		}

		protected Expression namespace_expression()
		{
			Expression expression = null;
			ExpressionCollection ec = null;
			try
			{
				expression = identifier_expression();
				switch (LA(1))
				{
				case 75:
					match(75);
					if (0 == inputState.guessing)
					{
						MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(expression);
						ec = methodInvocationExpression.Arguments;
						expression = methodInvocationExpression;
					}
					expression_list(ec);
					match(76);
					break;
				case 1:
				case 11:
				case 31:
				case 73:
				case 74:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_26_);
			}
			return expression;
		}

		protected ReferenceExpression identifier_expression()
		{
			ReferenceExpression result = null;
			IToken token = null;
			try
			{
				token = identifier();
				if (0 == inputState.guessing && token != null)
				{
					result = new ReferenceExpression(ToLexicalInfo(token), token.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_27_);
			}
			return result;
		}

		protected void expression_list(ExpressionCollection ec)
		{
			Expression expression = null;
			try
			{
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						ec.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84)
						{
							match(84);
							expression = this.expression();
							if (0 == inputState.guessing)
							{
								ec.Add(expression);
							}
							continue;
						}
						break;
					}
					break;
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
				case 76:
				case 87:
				case 92:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_28_);
					return;
				}
				throw ex;
			}
		}

		protected IToken identifier()
		{
			IToken token = null;
			IToken token2 = null;
			_sbuilder.Length = 0;
			IToken token3 = null;
			try
			{
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					_sbuilder.Append(token.getText());
					token2 = token;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 86 && tokenSet_29_.member(LA(2)))
					{
						match(86);
						token3 = member();
						if (0 == inputState.guessing)
						{
							_sbuilder.Append('.');
							_sbuilder.Append(token3.getText());
						}
						continue;
					}
					break;
				}
				if (0 == inputState.guessing)
				{
					token2.setText(_sbuilder.ToString());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_30_);
			}
			return token2;
		}

		protected void attributes()
		{
			Attribute attribute = null;
			try
			{
				if (0 == inputState.guessing)
				{
					_attributes.Clear();
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) != 80)
					{
						break;
					}
					match(80);
					switch (LA(1))
					{
					case 63:
					case 71:
						attribute = this.attribute();
						if (0 == inputState.guessing && attribute != null)
						{
							_attributes.Add(attribute);
						}
						while (true)
						{
							flag = true;
							if (LA(1) == 84)
							{
								match(84);
								attribute = this.attribute();
								if (0 == inputState.guessing && attribute != null)
								{
									_attributes.Add(attribute);
								}
								continue;
							}
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 81:
						break;
					}
					match(81);
					switch (LA(1))
					{
					case 1:
					case 73:
					case 74:
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 9:
					case 11:
					case 14:
					case 17:
					case 19:
					case 26:
					case 27:
					case 30:
					case 34:
					case 37:
					case 38:
					case 44:
					case 49:
					case 50:
					case 51:
					case 52:
					case 54:
					case 56:
					case 57:
					case 59:
					case 60:
					case 63:
					case 67:
					case 68:
					case 71:
					case 79:
					case 80:
					case 87:
						break;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_31_);
					return;
				}
				throw ex;
			}
		}

		protected void modifiers()
		{
			try
			{
				if (0 == inputState.guessing)
				{
					_modifiers = TypeMemberModifiers.None;
				}
				while (true)
				{
					bool flag = true;
					if (tokenSet_32_.member(LA(1)))
					{
						type_member_modifier();
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_33_);
					return;
				}
				throw ex;
			}
		}

		protected void type_definition(TypeMemberCollection container)
		{
			try
			{
				switch (LA(1))
				{
				case 17:
				case 60:
					class_definition(container);
					break;
				case 37:
					interface_definition(container);
					break;
				case 26:
					enum_definition(container);
					break;
				case 14:
					callable_definition(container);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void method(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			Method method = null;
			TypeReference typeReference = null;
			TypeReference typeReference2 = null;
			ExplicitMemberInfo explicitMemberInfo = null;
			ParameterDeclarationCollection c = null;
			GenericParameterDeclarationCollection c2 = null;
			Block block = null;
			StatementCollection container2 = null;
			IToken token4 = null;
			try
			{
				token = LT(1);
				match(19);
				switch (LA(1))
				{
				case 27:
				case 34:
				case 38:
				case 50:
				case 51:
				case 54:
				case 56:
				case 71:
					if (LA(1) == 71 && LA(2) == 86)
					{
						explicitMemberInfo = explicit_member_info();
					}
					else if (!tokenSet_29_.member(LA(1)) || (LA(2) != 75 && LA(2) != 80))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token4 = member();
					if (0 == inputState.guessing)
					{
						method = ((explicitMemberInfo == null) ? new Method(SourceLocationFactory.ToLexicalInfo(token4)) : new Method(explicitMemberInfo.LexicalInfo));
						method.Name = token4.getText();
						method.ExplicitInfo = explicitMemberInfo;
					}
					break;
				case 18:
					token2 = LT(1);
					match(18);
					if (0 == inputState.guessing)
					{
						method = new Constructor(SourceLocationFactory.ToLexicalInfo(token2));
					}
					break;
				case 20:
					token3 = LT(1);
					match(20);
					if (0 == inputState.guessing)
					{
						method = new Destructor(SourceLocationFactory.ToLexicalInfo(token3));
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					method.Modifiers = _modifiers;
					AddAttributes(method.Attributes);
					c = method.Parameters;
					c2 = method.GenericParameters;
					block = method.Body;
					container2 = block.Statements;
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					switch (LA(1))
					{
					case 47:
						match(47);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					generic_parameter_declaration_list(c2);
					match(81);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 75:
					break;
				}
				match(75);
				parameter_declaration_list(c);
				match(76);
				attributes();
				if (0 == inputState.guessing)
				{
					AddAttributes(method.ReturnTypeAttributes);
				}
				switch (LA(1))
				{
				case 11:
					match(11);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						method.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 87:
					break;
				}
				begin_block_with_doc(method, block);
				this.block(container2);
				end(block);
				if (0 == inputState.guessing)
				{
					container.Add(method);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void class_definition(TypeMemberCollection container)
		{
			IToken token = null;
			TypeDefinition typeDefinition = null;
			TypeReferenceCollection container2 = null;
			TypeMemberCollection container3 = null;
			GenericParameterDeclarationCollection c = null;
			try
			{
				switch (LA(1))
				{
				case 17:
					match(17);
					if (0 == inputState.guessing)
					{
						typeDefinition = new ClassDefinition();
					}
					break;
				case 60:
					match(60);
					if (0 == inputState.guessing)
					{
						typeDefinition = new StructDefinition();
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					typeDefinition.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token);
					typeDefinition.Name = token.getText();
					typeDefinition.Modifiers = _modifiers;
					AddAttributes(typeDefinition.Attributes);
					container.Add(typeDefinition);
					container2 = typeDefinition.BaseTypes;
					container3 = typeDefinition.Members;
					c = typeDefinition.GenericParameters;
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					switch (LA(1))
					{
					case 47:
						match(47);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					generic_parameter_declaration_list(c);
					match(81);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 75:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 75:
					base_types(container2);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 87:
					break;
				}
				begin_with_doc(typeDefinition);
				while (true)
				{
					bool flag = true;
					switch (LA(1))
					{
					case 85:
						splice_type_definition_body(container3);
						break;
					case 9:
					case 14:
					case 17:
					case 19:
					case 26:
					case 27:
					case 30:
					case 37:
					case 38:
					case 44:
					case 49:
					case 50:
					case 51:
					case 52:
					case 57:
					case 59:
					case 60:
					case 63:
					case 67:
					case 68:
					case 71:
					case 80:
						type_definition_member(container3);
						break;
					default:
						end(typeDefinition);
						return;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void interface_definition(TypeMemberCollection container)
		{
			IToken token = null;
			InterfaceDefinition interfaceDefinition = null;
			TypeMemberCollection container2 = null;
			GenericParameterDeclarationCollection c = null;
			try
			{
				match(37);
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					interfaceDefinition = new InterfaceDefinition(SourceLocationFactory.ToLexicalInfo(token));
					interfaceDefinition.Name = token.getText();
					interfaceDefinition.Modifiers = _modifiers;
					AddAttributes(interfaceDefinition.Attributes);
					container.Add(interfaceDefinition);
					container2 = interfaceDefinition.Members;
					c = interfaceDefinition.GenericParameters;
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					switch (LA(1))
					{
					case 47:
						match(47);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					generic_parameter_declaration_list(c);
					match(81);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 75:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 75:
					base_types(interfaceDefinition.BaseTypes);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 87:
					break;
				}
				begin_with_doc(interfaceDefinition);
				while (true)
				{
					bool flag = true;
					if (tokenSet_35_.member(LA(1)))
					{
						attributes();
						switch (LA(1))
						{
						case 19:
							interface_method(container2);
							break;
						case 27:
							event_declaration(container2);
							break;
						case 57:
						case 71:
							interface_property(container2);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						continue;
					}
					break;
				}
				end(interfaceDefinition);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void enum_definition(TypeMemberCollection container)
		{
			IToken token = null;
			EnumDefinition enumDefinition = null;
			TypeMemberCollection container2 = null;
			try
			{
				match(26);
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					enumDefinition = new EnumDefinition(SourceLocationFactory.ToLexicalInfo(token));
				}
				begin_with_doc(enumDefinition);
				if (0 == inputState.guessing)
				{
					enumDefinition.Name = token.getText();
					enumDefinition.Modifiers = _modifiers;
					AddAttributes(enumDefinition.Attributes);
					container.Add(enumDefinition);
					container2 = enumDefinition.Members;
				}
				int num = 0;
				while (true)
				{
					bool flag = true;
					switch (LA(1))
					{
					case 71:
					case 80:
						enum_member(container2);
						break;
					case 85:
						splice_type_definition_body(container2);
						break;
					default:
						if (num < 1)
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						end(enumDefinition);
						return;
					}
					num++;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void callable_definition(TypeMemberCollection container)
		{
			IToken token = null;
			CallableDefinition callableDefinition = null;
			TypeReference typeReference = null;
			GenericParameterDeclarationCollection c = null;
			try
			{
				match(14);
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					callableDefinition = new CallableDefinition(SourceLocationFactory.ToLexicalInfo(token));
					callableDefinition.Name = token.getText();
					callableDefinition.Modifiers = _modifiers;
					AddAttributes(callableDefinition.Attributes);
					container.Add(callableDefinition);
					c = callableDefinition.GenericParameters;
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					switch (LA(1))
					{
					case 47:
						match(47);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					generic_parameter_declaration_list(c);
					match(81);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 75:
					break;
				}
				match(75);
				parameter_declaration_list(callableDefinition.Parameters);
				match(76);
				switch (LA(1))
				{
				case 11:
					match(11);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						callableDefinition.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 73:
				case 74:
					break;
				}
				eos();
				docstring(callableDefinition);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_34_);
					return;
				}
				throw ex;
			}
		}

		protected void generic_parameter_declaration_list(GenericParameterDeclarationCollection c)
		{
			try
			{
				generic_parameter_declaration(c);
				while (true)
				{
					bool flag = true;
					if (LA(1) == 84)
					{
						match(84);
						generic_parameter_declaration(c);
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_36_);
					return;
				}
				throw ex;
			}
		}

		protected void parameter_declaration_list(ParameterDeclarationCollection c)
		{
			bool flag = false;
			try
			{
				switch (LA(1))
				{
				case 54:
				case 71:
				case 79:
				case 80:
					flag = parameter_declaration(c);
					while (true)
					{
						bool flag2 = true;
						if (LA(1) == 84 && !flag)
						{
							match(84);
							flag = parameter_declaration(c);
							continue;
						}
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
				case 81:
				case 90:
					break;
				}
				if (0 == inputState.guessing)
				{
					c.HasParamArray = flag;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_37_);
					return;
				}
				throw ex;
			}
		}

		protected TypeReference type_reference()
		{
			TypeReference typeReference = null;
			IToken token = null;
			TypeReferenceCollection container = null;
			GenericTypeDefinitionReference genericTypeDefinitionReference = null;
			try
			{
				switch (LA(1))
				{
				case 85:
					typeReference = splice_type_reference();
					break;
				case 75:
					typeReference = array_type_reference();
					break;
				default:
				{
					bool flag = false;
					if (LA(1) == 14 && LA(2) == 75)
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							match(14);
							match(75);
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
						typeReference = callable_type_reference();
						break;
					}
					if ((LA(1) == 14 || LA(1) == 16 || LA(1) == 71) && tokenSet_38_.member(LA(2)))
					{
						token = type_name();
						if (LA(1) == 80 && tokenSet_39_.member(LA(2)))
						{
							match(80);
							switch (LA(1))
							{
							case 47:
								match(47);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 14:
							case 16:
							case 71:
							case 75:
							case 79:
							case 85:
								break;
							}
							switch (LA(1))
							{
							case 79:
								match(79);
								if (0 == inputState.guessing)
								{
									genericTypeDefinitionReference = new GenericTypeDefinitionReference(ToLexicalInfo(token));
									genericTypeDefinitionReference.Name = token.getText();
									genericTypeDefinitionReference.GenericPlaceholders = 1;
									typeReference = genericTypeDefinitionReference;
								}
								while (true)
								{
									bool flag2 = true;
									if (LA(1) == 84)
									{
										match(84);
										match(79);
										if (0 == inputState.guessing)
										{
											genericTypeDefinitionReference.GenericPlaceholders++;
										}
										continue;
									}
									break;
								}
								match(81);
								break;
							case 14:
							case 16:
							case 71:
							case 75:
							case 85:
								if (0 == inputState.guessing)
								{
									GenericTypeReference genericTypeReference = new GenericTypeReference(ToLexicalInfo(token), token.getText());
									container = genericTypeReference.GenericArguments;
									typeReference = genericTypeReference;
								}
								type_reference_list(container);
								match(81);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
						}
						else if (LA(1) == 47 && LA(2) == 79)
						{
							match(47);
							match(79);
							if (0 == inputState.guessing)
							{
								genericTypeDefinitionReference = new GenericTypeDefinitionReference(ToLexicalInfo(token));
								genericTypeDefinitionReference.Name = token.getText();
								genericTypeDefinitionReference.GenericPlaceholders = 1;
								typeReference = genericTypeDefinitionReference;
							}
						}
						else if (LA(1) == 47 && tokenSet_40_.member(LA(2)))
						{
							match(47);
							typeReference = type_reference();
							if (0 == inputState.guessing)
							{
								GenericTypeReference genericTypeReference = new GenericTypeReference(ToLexicalInfo(token), token.getText());
								genericTypeReference.GenericArguments.Add(typeReference);
								typeReference = genericTypeReference;
							}
						}
						else
						{
							if (!tokenSet_38_.member(LA(1)) || !tokenSet_41_.member(LA(2)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							if (0 == inputState.guessing)
							{
								SimpleTypeReference simpleTypeReference = new SimpleTypeReference(ToLexicalInfo(token));
								simpleTypeReference.Name = token.getText();
								typeReference = simpleTypeReference;
							}
						}
						if (LA(1) == 88 && tokenSet_38_.member(LA(2)))
						{
							match(88);
							if (0 == inputState.guessing)
							{
								GenericTypeReference genericTypeReference2 = new GenericTypeReference(typeReference.LexicalInfo, "System.Nullable");
								genericTypeReference2.GenericArguments.Add(typeReference);
								typeReference = genericTypeReference2;
							}
						}
						else if (!tokenSet_38_.member(LA(1)) || !tokenSet_41_.member(LA(2)))
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				while (true)
				{
					bool flag2 = true;
					if (LA(1) == 79 && tokenSet_38_.member(LA(2)))
					{
						match(79);
						if (0 == inputState.guessing)
						{
							typeReference = CodeFactory.EnumerableTypeReferenceFor(typeReference);
						}
						continue;
					}
					if (LA(1) == 89 && tokenSet_38_.member(LA(2)))
					{
						match(89);
						if (0 == inputState.guessing)
						{
							typeReference = CodeFactory.EnumerableTypeReferenceFor(CodeFactory.EnumerableTypeReferenceFor(typeReference));
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return typeReference;
		}

		protected void begin_with_doc(Node node)
		{
			try
			{
				match(87);
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_42_.member(LA(2)))
				{
					eos();
					docstring(node);
				}
				else if (!tokenSet_42_.member(LA(1)) || !tokenSet_43_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_42_);
					return;
				}
				throw ex;
			}
		}

		protected void enum_member(TypeMemberCollection container)
		{
			IToken token = null;
			EnumMember enumMember = null;
			IntegerLiteralExpression integerLiteralExpression = null;
			bool flag = false;
			try
			{
				attributes();
				token = LT(1);
				match(71);
				switch (LA(1))
				{
				case 82:
					match(82);
					if (LA(1) == 83 && (LA(2) == 83 || LA(2) == 110 || LA(2) == 114))
					{
						match(83);
						if (0 == inputState.guessing)
						{
							flag = true;
						}
					}
					else if ((LA(1) != 83 && LA(1) != 110 && LA(1) != 114) || !tokenSet_44_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					integerLiteralExpression = integer_literal();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 73:
				case 74:
					break;
				}
				if (0 == inputState.guessing)
				{
					enumMember = new EnumMember(SourceLocationFactory.ToLexicalInfo(token));
					enumMember.Name = token.getText();
					enumMember.Initializer = integerLiteralExpression;
					if (flag && null != integerLiteralExpression)
					{
						integerLiteralExpression.Value *= -1L;
					}
					AddAttributes(enumMember.Attributes);
					container.Add(enumMember);
				}
				eos();
				docstring(enumMember);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_45_);
					return;
				}
				throw ex;
			}
		}

		public void splice_type_definition_body(TypeMemberCollection container)
		{
			IToken token = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(85);
				expression = atom();
				eos();
				if (0 == inputState.guessing)
				{
					container.Add(new SpliceTypeDefinitionBody(expression));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_46_);
					return;
				}
				throw ex;
			}
		}

		protected void end(Node node)
		{
			IToken token = null;
			try
			{
				token = LT(1);
				match(24);
				if (0 == inputState.guessing)
				{
					node.EndSourceLocation = SourceLocationFactory.ToSourceLocation(token);
				}
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_47_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_47_.member(LA(1)) || !tokenSet_10_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_47_);
					return;
				}
				throw ex;
			}
		}

		protected IntegerLiteralExpression integer_literal()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			string text = null;
			IntegerLiteralExpression result = null;
			try
			{
				switch (LA(1))
				{
				case 83:
					token = LT(1);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 110:
				case 114:
					break;
				}
				switch (LA(1))
				{
				case 114:
					token2 = LT(1);
					match(114);
					if (0 == inputState.guessing)
					{
						text = ((token != null) ? (token.getText() + token2.getText()) : token2.getText());
						result = PrimitiveParser.ParseIntegerLiteralExpression(token2, text, asLong: false);
					}
					break;
				case 110:
					token3 = LT(1);
					match(110);
					if (0 == inputState.guessing)
					{
						text = ((token != null) ? (token.getText() + token3.getText()) : token3.getText());
						result = PrimitiveParser.ParseIntegerLiteralExpression(token3, text, asLong: true);
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected Attribute attribute()
		{
			IToken token = null;
			IToken token2 = null;
			Attribute attribute = null;
			try
			{
				switch (LA(1))
				{
				case 71:
					token2 = identifier();
					break;
				case 63:
					token = LT(1);
					match(63);
					if (0 == inputState.guessing)
					{
						token2 = token;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					attribute = new Attribute(ToLexicalInfo(token2), token2.getText());
				}
				switch (LA(1))
				{
				case 75:
					match(75);
					argument_list(attribute);
					match(76);
					break;
				case 81:
				case 84:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_48_);
			}
			return attribute;
		}

		protected void argument_list(INodeWithArguments node)
		{
			try
			{
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					argument(node);
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84)
						{
							match(84);
							argument(node);
							continue;
						}
						break;
					}
					break;
				case 76:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_49_);
					return;
				}
				throw ex;
			}
		}

		protected void base_types(TypeReferenceCollection container)
		{
			TypeReference typeReference = null;
			try
			{
				match(75);
				switch (LA(1))
				{
				case 14:
				case 16:
				case 71:
				case 75:
				case 85:
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						container.Add(typeReference);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84)
						{
							match(84);
							typeReference = type_reference();
							if (0 == inputState.guessing)
							{
								container.Add(typeReference);
							}
							continue;
						}
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
					break;
				}
				match(76);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_50_);
					return;
				}
				throw ex;
			}
		}

		protected Expression splice_expression()
		{
			IToken token = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(85);
				expression = atom();
				if (0 == inputState.guessing)
				{
					expression = new SpliceExpression(SourceLocationFactory.ToLexicalInfo(token), expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return expression;
		}

		public void type_definition_member(TypeMemberCollection container)
		{
			try
			{
				attributes();
				modifiers();
				switch (LA(1))
				{
				case 19:
					method(container);
					break;
				case 27:
					event_declaration(container);
					break;
				case 57:
				case 71:
					field_or_property(container);
					break;
				case 14:
				case 17:
				case 26:
				case 37:
				case 60:
					type_definition(container);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_51_);
					return;
				}
				throw ex;
			}
		}

		protected Expression atom()
		{
			Expression result = null;
			try
			{
				switch (LA(1))
				{
				case 6:
				case 33:
				case 46:
				case 57:
				case 58:
				case 64:
				case 72:
				case 77:
				case 78:
				case 80:
				case 83:
				case 91:
				case 93:
				case 110:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					result = literal();
					break;
				case 75:
					result = paren_expression();
					break;
				case 15:
					result = cast_expression();
					break;
				case 65:
					result = typeof_expression();
					break;
				case 85:
					result = splice_expression();
					break;
				case 86:
					result = omitted_member_expression();
					break;
				default:
				{
					bool flag = false;
					if (LA(1) == 16 && LA(2) == 75)
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							match(16);
							match(75);
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
						result = char_literal();
						break;
					}
					if ((LA(1) == 16 || LA(1) == 71) && tokenSet_38_.member(LA(2)))
					{
						result = reference_expression();
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return result;
		}

		protected void event_declaration(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			Event @event = null;
			TypeReference typeReference = null;
			try
			{
				token = LT(1);
				match(27);
				token2 = LT(1);
				match(71);
				match(11);
				typeReference = type_reference();
				eos();
				if (0 == inputState.guessing)
				{
					@event = new Event(SourceLocationFactory.ToLexicalInfo(token2), token2.getText(), typeReference);
					@event.Modifiers = _modifiers;
					AddAttributes(@event.Attributes);
					container.Add(@event);
				}
				docstring(@event);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_51_);
					return;
				}
				throw ex;
			}
		}

		protected void field_or_property(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			TypeMember typeMember = null;
			TypeReference type = null;
			Property property = null;
			Field field = null;
			ExplicitMemberInfo explicitMemberInfo = null;
			Expression expression = null;
			ParameterDeclarationCollection c = null;
			try
			{
				bool flag = false;
				if ((LA(1) == 57 || LA(1) == 71) && tokenSet_52_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						property_header();
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
					if (LA(1) == 71 && LA(2) == 86)
					{
						explicitMemberInfo = explicit_member_info();
					}
					else if ((LA(1) != 57 && LA(1) != 71) || !tokenSet_53_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 71:
						token = LT(1);
						match(71);
						if (0 == inputState.guessing)
						{
							token4 = token;
						}
						break;
					case 57:
						token2 = LT(1);
						match(57);
						if (0 == inputState.guessing)
						{
							token4 = token2;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						property = ((explicitMemberInfo == null) ? new Property(SourceLocationFactory.ToLexicalInfo(token4)) : new Property(explicitMemberInfo.LexicalInfo));
						property.Name = token4.getText();
						property.ExplicitInfo = explicitMemberInfo;
						AddAttributes(property.Attributes);
						c = property.Parameters;
					}
					switch (LA(1))
					{
					case 75:
					case 80:
						switch (LA(1))
						{
						case 80:
							match(80);
							break;
						case 75:
							match(75);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						parameter_declaration_list(c);
						switch (LA(1))
						{
						case 81:
							match(81);
							break;
						case 76:
							match(76);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 11:
					case 87:
						break;
					}
					switch (LA(1))
					{
					case 11:
						match(11);
						type = type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 87:
						break;
					}
					if (0 == inputState.guessing)
					{
						property.Type = type;
						typeMember = property;
						typeMember.Modifiers = _modifiers;
					}
					begin_with_doc(property);
					int num = 0;
					while (true)
					{
						bool flag2 = true;
						if (!tokenSet_54_.member(LA(1)))
						{
							break;
						}
						property_accessor(property);
						num++;
					}
					if (num < 1)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					end(property);
					if (0 == inputState.guessing)
					{
						container.Add(typeMember);
					}
					return;
				}
				if (LA(1) == 71 && tokenSet_55_.member(LA(2)))
				{
					token3 = LT(1);
					match(71);
					if (0 == inputState.guessing)
					{
						typeMember = (field = new Field(SourceLocationFactory.ToLexicalInfo(token3)));
						field.Name = token3.getText();
						field.Modifiers = _modifiers;
						AddAttributes(field.Attributes);
					}
					switch (LA(1))
					{
					case 11:
						match(11);
						type = type_reference();
						if (0 == inputState.guessing)
						{
							field.Type = type;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 1:
					case 73:
					case 74:
					case 82:
						break;
					}
					switch (LA(1))
					{
					case 82:
						match(82);
						expression = declaration_initializer();
						if (0 == inputState.guessing)
						{
							field.Initializer = expression;
						}
						break;
					case 1:
					case 73:
					case 74:
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					docstring(field);
					if (0 == inputState.guessing)
					{
						container.Add(typeMember);
					}
					return;
				}
				throw new NoViableAltException(LT(1), getFilename());
			}
			catch (RecognitionException ex2)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex2);
					recover(ex2, tokenSet_51_);
					return;
				}
				throw ex2;
			}
		}

		protected void interface_method(TypeMemberCollection container)
		{
			Method method = null;
			TypeReference typeReference = null;
			IToken token = null;
			try
			{
				match(19);
				token = member();
				if (0 == inputState.guessing)
				{
					method = new Method(SourceLocationFactory.ToLexicalInfo(token));
					method.Name = token.getText();
					AddAttributes(method.Attributes);
					container.Add(method);
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					switch (LA(1))
					{
					case 47:
						match(47);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					generic_parameter_declaration_list(method.GenericParameters);
					match(81);
					break;
				case 47:
					match(47);
					generic_parameter_declaration(method.GenericParameters);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 75:
					break;
				}
				match(75);
				parameter_declaration_list(method.Parameters);
				match(76);
				switch (LA(1))
				{
				case 11:
					match(11);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						method.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 73:
				case 74:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 1:
				case 73:
				case 74:
					eos();
					docstring(method);
					break;
				case 87:
					empty_block(method);
					switch (LA(1))
					{
					case 1:
					case 73:
					case 74:
						eos();
						break;
					case 19:
					case 24:
					case 27:
					case 57:
					case 71:
					case 80:
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_56_);
					return;
				}
				throw ex;
			}
		}

		protected void interface_property(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			Property property = null;
			TypeReference type = null;
			ParameterDeclarationCollection c = null;
			try
			{
				switch (LA(1))
				{
				case 71:
					token = LT(1);
					match(71);
					if (0 == inputState.guessing)
					{
						token3 = token;
					}
					break;
				case 57:
					token2 = LT(1);
					match(57);
					if (0 == inputState.guessing)
					{
						token3 = token2;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					property = new Property(SourceLocationFactory.ToLexicalInfo(token3));
					property.Name = token3.getText();
					AddAttributes(property.Attributes);
					container.Add(property);
					c = property.Parameters;
				}
				switch (LA(1))
				{
				case 75:
				case 80:
					switch (LA(1))
					{
					case 80:
						match(80);
						break;
					case 75:
						match(75);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					parameter_declaration_list(c);
					switch (LA(1))
					{
					case 81:
						match(81);
						break;
					case 76:
						match(76);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 11:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 11:
					match(11);
					type = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 87:
					break;
				}
				if (0 == inputState.guessing)
				{
					property.Type = type;
				}
				begin_with_doc(property);
				int num = 0;
				while (true)
				{
					bool flag = true;
					if (LA(1) != 34 && LA(1) != 56 && LA(1) != 80)
					{
						break;
					}
					interface_property_accessor(property);
					num++;
				}
				if (num < 1)
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				end(property);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_56_);
					return;
				}
				throw ex;
			}
		}

		protected IToken member()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			IToken token6 = null;
			IToken token7 = null;
			IToken token8 = null;
			IToken result = null;
			try
			{
				switch (LA(1))
				{
				case 71:
					token = LT(1);
					match(71);
					if (0 == inputState.guessing)
					{
						result = token;
					}
					break;
				case 56:
					token2 = LT(1);
					match(56);
					if (0 == inputState.guessing)
					{
						result = token2;
					}
					break;
				case 34:
					token3 = LT(1);
					match(34);
					if (0 == inputState.guessing)
					{
						result = token3;
					}
					break;
				case 38:
					token4 = LT(1);
					match(38);
					if (0 == inputState.guessing)
					{
						result = token4;
					}
					break;
				case 50:
					token5 = LT(1);
					match(50);
					if (0 == inputState.guessing)
					{
						result = token5;
					}
					break;
				case 51:
					token6 = LT(1);
					match(51);
					if (0 == inputState.guessing)
					{
						result = token6;
					}
					break;
				case 27:
					token7 = LT(1);
					match(27);
					if (0 == inputState.guessing)
					{
						result = token7;
					}
					break;
				case 54:
					token8 = LT(1);
					match(54);
					if (0 == inputState.guessing)
					{
						result = token8;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_30_);
			}
			return result;
		}

		protected void generic_parameter_declaration(GenericParameterDeclarationCollection c)
		{
			IToken token = null;
			GenericParameterDeclaration genericParameterDeclaration = null;
			try
			{
				token = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					genericParameterDeclaration = new GenericParameterDeclaration(SourceLocationFactory.ToLexicalInfo(token));
					genericParameterDeclaration.Name = token.getText();
					c.Add(genericParameterDeclaration);
				}
				if (LA(1) == 75 && tokenSet_57_.member(LA(2)))
				{
					match(75);
					generic_parameter_constraints(genericParameterDeclaration);
					match(76);
				}
				else if ((LA(1) != 75 && LA(1) != 81 && LA(1) != 84) || !tokenSet_58_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_59_);
					return;
				}
				throw ex;
			}
		}

		protected void empty_block(Node node)
		{
			try
			{
				begin();
				eos();
				end(node);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_60_);
					return;
				}
				throw ex;
			}
		}

		protected void interface_property_accessor(Property p)
		{
			IToken token = null;
			IToken token2 = null;
			Method method = null;
			try
			{
				attributes();
				if (LA(1) == 34 && null == p.Getter)
				{
					token = LT(1);
					match(34);
					if (0 == inputState.guessing)
					{
						Method method3 = (p.Getter = new Method(SourceLocationFactory.ToLexicalInfo(token)));
						method = method3;
						method.Name = "get";
					}
				}
				else
				{
					if (LA(1) != 56 || null != p.Setter)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token2 = LT(1);
					match(56);
					if (0 == inputState.guessing)
					{
						Method method3 = (p.Setter = new Method(SourceLocationFactory.ToLexicalInfo(token2)));
						method = method3;
						method.Name = "set";
					}
				}
				switch (LA(1))
				{
				case 1:
				case 73:
				case 74:
					eos();
					break;
				case 87:
					empty_block(method);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					AddAttributes(method.Attributes);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_61_);
					return;
				}
				throw ex;
			}
		}

		protected void begin()
		{
			try
			{
				match(87);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_62_);
					return;
				}
				throw ex;
			}
		}

		protected ExplicitMemberInfo explicit_member_info()
		{
			IToken token = null;
			IToken token2 = null;
			ExplicitMemberInfo explicitMemberInfo = null;
			_sbuilder.Length = 0;
			try
			{
				token = LT(1);
				match(71);
				match(86);
				if (0 == inputState.guessing)
				{
					explicitMemberInfo = new ExplicitMemberInfo(SourceLocationFactory.ToLexicalInfo(token));
					_sbuilder.Append(token.getText());
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 71 && LA(2) == 86)
					{
						token2 = LT(1);
						match(71);
						match(86);
						if (0 == inputState.guessing)
						{
							_sbuilder.Append('.');
							_sbuilder.Append(token2.getText());
						}
						continue;
					}
					break;
				}
				if (0 == inputState.guessing && explicitMemberInfo != null)
				{
					explicitMemberInfo.InterfaceType = new SimpleTypeReference(explicitMemberInfo.LexicalInfo);
					explicitMemberInfo.InterfaceType.Name = _sbuilder.ToString();
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_63_);
			}
			return explicitMemberInfo;
		}

		protected void begin_block_with_doc(Node node, Block block)
		{
			IToken token = null;
			try
			{
				token = LT(1);
				match(87);
				if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_64_.member(LA(2)))
				{
					eos();
					docstring(node);
				}
				else if (!tokenSet_64_.member(LA(1)) || !tokenSet_65_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					block.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_64_);
					return;
				}
				throw ex;
			}
		}

		protected void block(StatementCollection container)
		{
			try
			{
				switch (LA(1))
				{
				case 1:
				case 73:
				case 74:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 6:
				case 12:
				case 13:
				case 15:
				case 16:
				case 22:
				case 23:
				case 24:
				case 25:
				case 28:
				case 29:
				case 32:
				case 33:
				case 35:
				case 41:
				case 46:
				case 48:
				case 53:
				case 55:
				case 57:
				case 58:
				case 61:
				case 62:
				case 64:
				case 65:
				case 66:
				case 69:
				case 70:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 87:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (tokenSet_18_.member(LA(1)))
					{
						stmt(container);
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_66_);
					return;
				}
				throw ex;
			}
		}

		protected void property_header()
		{
			try
			{
				switch (LA(1))
				{
				case 71:
					match(71);
					break;
				case 57:
					match(57);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 86)
					{
						match(86);
						match(71);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 80:
					match(80);
					break;
				case 75:
					match(75);
					break;
				case 11:
				case 87:
					switch (LA(1))
					{
					case 11:
						match(11);
						type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 87:
						break;
					}
					match(87);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_0_);
					return;
				}
				throw ex;
			}
		}

		protected void property_accessor(Property p)
		{
			IToken token = null;
			IToken token2 = null;
			Method method = null;
			Block b = null;
			try
			{
				attributes();
				modifiers();
				if (LA(1) == 34 && null == p.Getter)
				{
					token = LT(1);
					match(34);
					if (0 == inputState.guessing)
					{
						method = (p.Getter = new Method(SourceLocationFactory.ToLexicalInfo(token)));
						method.Name = "get";
					}
				}
				else
				{
					if (LA(1) != 56 || null != p.Setter)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token2 = LT(1);
					match(56);
					if (0 == inputState.guessing)
					{
						method = (p.Setter = new Method(SourceLocationFactory.ToLexicalInfo(token2)));
						method.Name = "set";
					}
				}
				if (0 == inputState.guessing)
				{
					AddAttributes(method.Attributes);
					method.Modifiers = _modifiers;
					b = method.Body;
				}
				compound_stmt(b);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_67_);
					return;
				}
				throw ex;
			}
		}

		public Expression declaration_initializer()
		{
			Expression expression = null;
			try
			{
				bool flag = false;
				if (tokenSet_68_.member(LA(1)) && tokenSet_69_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						slicing_expression();
						switch (LA(1))
						{
						case 87:
							match(87);
							break;
						case 21:
							match(21);
							break;
						case 19:
							match(19);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
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
					expression = slicing_expression();
					expression = method_invocation_block(expression);
				}
				else if (tokenSet_70_.member(LA(1)) && tokenSet_25_.member(LA(2)))
				{
					expression = array_or_expression();
					eos();
				}
				else
				{
					if (LA(1) != 19 && LA(1) != 21 && LA(1) != 87)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = callable_expression();
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_71_);
			}
			return expression;
		}

		protected Expression slicing_expression()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			Expression expression = null;
			SlicingExpression slicingExpression = null;
			MethodInvocationExpression methodInvocationExpression = null;
			TypeReference typeReference = null;
			TypeReferenceCollection container = null;
			Expression expression2 = null;
			try
			{
				expression = atom();
				while (true)
				{
					bool flag = true;
					switch (LA(1))
					{
					case 80:
						token = LT(1);
						match(80);
						switch (LA(1))
						{
						case 47:
							match(47);
							if (0 == inputState.guessing)
							{
								GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression(SourceLocationFactory.ToLexicalInfo(token));
								genericReferenceExpression.Target = expression;
								expression = genericReferenceExpression;
								container = genericReferenceExpression.GenericArguments;
							}
							type_reference_list(container);
							break;
						case 6:
						case 15:
						case 16:
						case 33:
						case 45:
						case 46:
						case 57:
						case 58:
						case 64:
						case 65:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 85:
						case 86:
						case 87:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							if (0 == inputState.guessing)
							{
								slicingExpression = new SlicingExpression(SourceLocationFactory.ToLexicalInfo(token));
								slicingExpression.Target = expression;
								expression = slicingExpression;
							}
							slice(slicingExpression);
							while (true)
							{
								flag = true;
								if (LA(1) == 84)
								{
									match(84);
									slice(slicingExpression);
									continue;
								}
								break;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						match(81);
						continue;
					case 47:
						token2 = LT(1);
						match(47);
						typeReference = type_reference();
						if (0 == inputState.guessing)
						{
							GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression(SourceLocationFactory.ToLexicalInfo(token2));
							genericReferenceExpression.Target = expression;
							expression = genericReferenceExpression;
							genericReferenceExpression.GenericArguments.Add(typeReference);
						}
						continue;
					case 86:
						match(86);
						while (true)
						{
							flag = true;
							if (LA(1) == 74)
							{
								match(74);
								continue;
							}
							break;
						}
						expression = member_reference_expression(expression);
						continue;
					case 75:
						token3 = LT(1);
						match(75);
						if (0 == inputState.guessing)
						{
							methodInvocationExpression = new MethodInvocationExpression(SourceLocationFactory.ToLexicalInfo(token3));
							methodInvocationExpression.Target = expression;
							expression = methodInvocationExpression;
						}
						switch (LA(1))
						{
						case 6:
						case 15:
						case 16:
						case 33:
						case 45:
						case 46:
						case 57:
						case 58:
						case 64:
						case 65:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 85:
						case 86:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							argument(methodInvocationExpression);
							while (true)
							{
								flag = true;
								if (LA(1) == 84)
								{
									match(84);
									argument(methodInvocationExpression);
									continue;
								}
								break;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 76:
							break;
						}
						match(76);
						switch (LA(1))
						{
						case 91:
						{
							bool flag2 = false;
							if (LA(1) == 91 && tokenSet_72_.member(LA(2)))
							{
								int pos = mark();
								flag2 = true;
								inputState.guessing++;
								try
								{
									hash_literal_test();
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
								expression2 = hash_literal();
							}
							else
							{
								if (LA(1) != 91 || !tokenSet_72_.member(LA(2)))
								{
									throw new NoViableAltException(LT(1), getFilename());
								}
								expression2 = list_initializer();
							}
							if (0 == inputState.guessing)
							{
								expression = new CollectionInitializationExpression(expression, expression2);
							}
							break;
						}
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 1:
						case 6:
						case 10:
						case 11:
						case 15:
						case 19:
						case 21:
						case 23:
						case 32:
						case 39:
						case 40:
						case 41:
						case 42:
						case 45:
						case 47:
						case 48:
						case 66:
						case 69:
						case 71:
						case 73:
						case 74:
						case 75:
						case 76:
						case 79:
						case 80:
						case 81:
						case 82:
						case 83:
						case 84:
						case 86:
						case 87:
						case 89:
						case 90:
						case 92:
						case 94:
						case 95:
						case 96:
						case 97:
						case 98:
						case 99:
						case 100:
						case 101:
						case 102:
						case 103:
						case 104:
						case 105:
						case 106:
						case 107:
						case 108:
						case 109:
						case 111:
						case 112:
							break;
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_73_);
			}
			return expression;
		}

		protected MethodInvocationExpression method_invocation_block(Expression e)
		{
			Expression expression = null;
			MethodInvocationExpression methodInvocationExpression = null;
			try
			{
				expression = callable_expression();
				if (0 == inputState.guessing)
				{
					methodInvocationExpression = e as MethodInvocationExpression;
					if (null == methodInvocationExpression)
					{
						methodInvocationExpression = new MethodInvocationExpression(e.LexicalInfo, e);
					}
					methodInvocationExpression.Arguments.Add(expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_71_);
			}
			return methodInvocationExpression;
		}

		protected Expression array_or_expression()
		{
			IToken token = null;
			IToken token2 = null;
			Expression expression = null;
			ArrayLiteralExpression arrayLiteralExpression = null;
			try
			{
				switch (LA(1))
				{
				case 84:
					token = LT(1);
					match(84);
					if (0 == inputState.guessing)
					{
						expression = new ArrayLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
					}
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = this.expression();
					switch (LA(1))
					{
					case 84:
						token2 = LT(1);
						match(84);
						if (0 == inputState.guessing)
						{
							arrayLiteralExpression = new ArrayLiteralExpression(expression.LexicalInfo);
							arrayLiteralExpression.Items.Add(expression);
						}
						switch (LA(1))
						{
						case 6:
						case 15:
						case 16:
						case 33:
						case 45:
						case 46:
						case 57:
						case 58:
						case 64:
						case 65:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 85:
						case 86:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							expression = this.expression();
							if (0 == inputState.guessing)
							{
								arrayLiteralExpression.Items.Add(expression);
							}
							while (true)
							{
								bool flag = true;
								if (LA(1) == 84 && tokenSet_4_.member(LA(2)))
								{
									match(84);
									expression = this.expression();
									if (0 == inputState.guessing)
									{
										arrayLiteralExpression.Items.Add(expression);
									}
									continue;
								}
								break;
							}
							switch (LA(1))
							{
							case 84:
								match(84);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 1:
							case 19:
							case 21:
							case 41:
							case 66:
							case 69:
							case 73:
							case 74:
							case 76:
							case 87:
							case 92:
								break;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 1:
						case 19:
						case 21:
						case 41:
						case 66:
						case 69:
						case 73:
						case 74:
						case 76:
						case 87:
						case 92:
							break;
						}
						if (0 == inputState.guessing)
						{
							expression = arrayLiteralExpression;
						}
						break;
					case 1:
					case 19:
					case 21:
					case 41:
					case 66:
					case 69:
					case 73:
					case 74:
					case 76:
					case 87:
					case 92:
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_74_);
			}
			return expression;
		}

		protected Expression callable_expression()
		{
			IToken token = null;
			IToken token2 = null;
			Expression result = null;
			Block block = null;
			BlockExpression blockExpression = null;
			TypeReference typeReference = null;
			IToken token3 = null;
			try
			{
				switch (LA(1))
				{
				case 87:
					if (0 == inputState.guessing)
					{
						block = new Block();
					}
					compound_stmt(block);
					if (0 == inputState.guessing)
					{
						result = new BlockExpression(block.LexicalInfo, block);
					}
					break;
				case 19:
				case 21:
					switch (LA(1))
					{
					case 21:
						token = LT(1);
						match(21);
						if (0 == inputState.guessing)
						{
							token3 = token;
						}
						break;
					case 19:
						token2 = LT(1);
						match(19);
						if (0 == inputState.guessing)
						{
							token3 = token2;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						result = (blockExpression = new BlockExpression(SourceLocationFactory.ToLexicalInfo(token3)));
						block = blockExpression.Body;
					}
					switch (LA(1))
					{
					case 75:
						match(75);
						parameter_declaration_list(blockExpression.Parameters);
						match(76);
						switch (LA(1))
						{
						case 11:
							match(11);
							typeReference = type_reference();
							if (0 == inputState.guessing)
							{
								blockExpression.ReturnType = typeReference;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 87:
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 87:
						break;
					}
					compound_stmt(block);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_71_);
			}
			return result;
		}

		protected void compound_stmt(Block b)
		{
			IToken token = null;
			StatementCollection container = null;
			try
			{
				token = LT(1);
				match(87);
				if (0 == inputState.guessing)
				{
					b.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token);
					container = b.Statements;
				}
				block(container);
				end(b);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_75_);
					return;
				}
				throw ex;
			}
		}

		protected void stmt(StatementCollection container)
		{
			Statement statement = null;
			StatementModifier statementModifier = null;
			try
			{
				switch (LA(1))
				{
				case 32:
					statement = for_stmt();
					break;
				case 69:
					statement = while_stmt();
					break;
				case 41:
					statement = if_stmt();
					break;
				case 66:
					statement = unless_stmt();
					break;
				case 62:
					statement = try_stmt();
					break;
				case 55:
					statement = return_stmt();
					break;
				default:
				{
					bool flag = false;
					if (tokenSet_76_.member(LA(1)) && tokenSet_77_.member(LA(2)))
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							atom();
							int num = 0;
							while (true)
							{
								bool flag2 = true;
								if (LA(1) != 74)
								{
									break;
								}
								match(74);
								num++;
							}
							if (num < 1)
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							match(86);
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
						statement = expression_stmt();
						eos();
						break;
					}
					bool flag3 = false;
					if (LA(1) == 71 && tokenSet_3_.member(LA(2)) && IsValidMacroArgument(LA(2)))
					{
						int pos2 = mark();
						flag3 = true;
						inputState.guessing++;
						try
						{
							match(71);
							if (tokenSet_4_.member(LA(1)))
							{
								expression();
							}
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
						statement = macro_stmt();
						break;
					}
					bool flag4 = false;
					if (tokenSet_68_.member(LA(1)) && tokenSet_78_.member(LA(2)))
					{
						int pos3 = mark();
						flag4 = true;
						inputState.guessing++;
						try
						{
							slicing_expression();
							switch (LA(1))
							{
							case 82:
								match(82);
								break;
							case 19:
							case 21:
							case 87:
								switch (LA(1))
								{
								case 87:
									match(87);
									break;
								case 21:
									match(21);
									break;
								case 19:
									match(19);
									break;
								default:
									throw new NoViableAltException(LT(1), getFilename());
								}
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
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
						statement = assignment_or_method_invocation_with_block_stmt();
						break;
					}
					bool flag5 = false;
					if (LA(1) == 71 && (LA(2) == 11 || LA(2) == 84))
					{
						int pos4 = mark();
						flag5 = true;
						inputState.guessing++;
						try
						{
							declaration();
							match(84);
						}
						catch (RecognitionException)
						{
							flag5 = false;
						}
						rewind(pos4);
						inputState.guessing--;
					}
					if (flag5)
					{
						statement = unpack_stmt();
						break;
					}
					if (LA(1) == 71 && LA(2) == 11)
					{
						statement = declaration_stmt();
						break;
					}
					if (tokenSet_79_.member(LA(1)) && tokenSet_77_.member(LA(2)))
					{
						switch (LA(1))
						{
						case 35:
							statement = goto_stmt();
							break;
						case 87:
							statement = label_stmt();
							break;
						case 70:
							statement = yield_stmt();
							break;
						case 12:
							statement = break_stmt();
							break;
						case 13:
							statement = continue_stmt();
							break;
						case 53:
							statement = raise_stmt();
							break;
						case 6:
						case 15:
						case 16:
						case 33:
						case 46:
						case 57:
						case 58:
						case 64:
						case 65:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 85:
						case 86:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							statement = expression_stmt();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						switch (LA(1))
						{
						case 41:
						case 66:
						case 69:
							statementModifier = stmt_modifier();
							if (0 == inputState.guessing)
							{
								statement.Modifier = statementModifier;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 1:
						case 73:
						case 74:
							break;
						}
						eos();
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				if (0 == inputState.guessing && null != statement)
				{
					container.Add(statement);
				}
			}
			catch (RecognitionException ex5)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex5);
					recover(ex5, tokenSet_80_);
					return;
				}
				throw ex5;
			}
		}

		protected void type_member_modifier()
		{
			try
			{
				switch (LA(1))
				{
				case 59:
					match(59);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Static;
					}
					break;
				case 50:
					match(50);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Public;
					}
					break;
				case 51:
					match(51);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Protected;
					}
					break;
				case 52:
					match(52);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Private;
					}
					break;
				case 38:
					match(38);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Internal;
					}
					break;
				case 30:
					match(30);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Final;
					}
					break;
				case 63:
					match(63);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Transient;
					}
					break;
				case 49:
					match(49);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Override;
					}
					break;
				case 9:
					match(9);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Abstract;
					}
					break;
				case 67:
					match(67);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Virtual;
					}
					break;
				case 44:
					match(44);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.New;
					}
					break;
				case 68:
					match(68);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Partial;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_81_);
					return;
				}
				throw ex;
			}
		}

		protected ParameterModifiers parameter_modifier()
		{
			ParameterModifiers result = ParameterModifiers.None;
			try
			{
				match(54);
				if (0 == inputState.guessing)
				{
					result = ParameterModifiers.Ref;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_40_);
			}
			return result;
		}

		protected bool parameter_declaration(ParameterDeclarationCollection c)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			TypeReference type = null;
			ParameterModifiers parameterModifiers = ParameterModifiers.None;
			bool result = false;
			try
			{
				attributes();
				switch (LA(1))
				{
				case 79:
					match(79);
					if (0 == inputState.guessing)
					{
						result = true;
					}
					token = LT(1);
					match(71);
					switch (LA(1))
					{
					case 11:
						match(11);
						type = array_type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 76:
					case 81:
					case 84:
					case 90:
						break;
					}
					if (0 == inputState.guessing)
					{
						token3 = token;
					}
					break;
				case 54:
				case 71:
					switch (LA(1))
					{
					case 54:
						parameterModifiers = parameter_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 71:
						break;
					}
					token2 = LT(1);
					match(71);
					switch (LA(1))
					{
					case 11:
						match(11);
						type = type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 76:
					case 81:
					case 84:
					case 90:
						break;
					}
					if (0 == inputState.guessing)
					{
						token3 = token2;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					ParameterDeclaration parameterDeclaration = new ParameterDeclaration(SourceLocationFactory.ToLexicalInfo(token3));
					parameterDeclaration.Name = token3.getText();
					parameterDeclaration.Type = type;
					parameterDeclaration.Modifiers = parameterModifiers;
					AddAttributes(parameterDeclaration.Attributes);
					c.Add(parameterDeclaration);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_82_);
			}
			return result;
		}

		protected ArrayTypeReference array_type_reference()
		{
			IToken token = null;
			IToken token2 = null;
			TypeReference typeReference = null;
			ArrayTypeReference arrayTypeReference = null;
			IntegerLiteralExpression integerLiteralExpression = null;
			try
			{
				token = LT(1);
				match(75);
				if (0 == inputState.guessing)
				{
					arrayTypeReference = new ArrayTypeReference(SourceLocationFactory.ToLexicalInfo(token));
				}
				typeReference = type_reference();
				if (0 == inputState.guessing)
				{
					arrayTypeReference.ElementType = typeReference;
				}
				switch (LA(1))
				{
				case 84:
					match(84);
					integerLiteralExpression = integer_literal();
					if (0 == inputState.guessing)
					{
						arrayTypeReference.Rank = integerLiteralExpression;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
					break;
				}
				token2 = LT(1);
				match(76);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return arrayTypeReference;
		}

		protected void callable_parameter_declaration_list(ParameterDeclarationCollection c)
		{
			bool flag = false;
			try
			{
				switch (LA(1))
				{
				case 14:
				case 16:
				case 54:
				case 71:
				case 75:
				case 79:
				case 85:
					flag = callable_parameter_declaration(c);
					while (true)
					{
						bool flag2 = true;
						if (LA(1) == 84 && !flag)
						{
							match(84);
							flag = callable_parameter_declaration(c);
							continue;
						}
						break;
					}
					if (0 == inputState.guessing)
					{
						c.HasParamArray = flag;
					}
					break;
				case 76:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_49_);
					return;
				}
				throw ex;
			}
		}

		protected bool callable_parameter_declaration(ParameterDeclarationCollection c)
		{
			TypeReference typeReference = null;
			ParameterModifiers parameterModifiers = ParameterModifiers.None;
			bool result = false;
			try
			{
				switch (LA(1))
				{
				case 79:
					match(79);
					if (0 == inputState.guessing)
					{
						result = true;
					}
					typeReference = type_reference();
					break;
				case 14:
				case 16:
				case 54:
				case 71:
				case 75:
				case 85:
					switch (LA(1))
					{
					case 54:
						parameterModifiers = parameter_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 14:
					case 16:
					case 71:
					case 75:
					case 85:
						break;
					}
					typeReference = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					ParameterDeclaration parameterDeclaration = new ParameterDeclaration(typeReference.LexicalInfo);
					parameterDeclaration.Name = "arg" + c.Count;
					parameterDeclaration.Type = typeReference;
					parameterDeclaration.Modifiers = parameterModifiers;
					c.Add(parameterDeclaration);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_83_);
			}
			return result;
		}

		protected void generic_parameter_constraints(GenericParameterDeclaration gpd)
		{
			TypeReference typeReference = null;
			try
			{
				switch (LA(1))
				{
				case 17:
					match(17);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.ReferenceType;
					}
					break;
				case 60:
					match(60);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.ValueType;
					}
					break;
				case 18:
					match(18);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.Constructable;
					}
					break;
				case 14:
				case 16:
				case 71:
				case 75:
				case 85:
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						gpd.BaseTypes.Add(typeReference);
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				switch (LA(1))
				{
				case 84:
					match(84);
					generic_parameter_constraints(gpd);
					break;
				case 76:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_49_);
					return;
				}
				throw ex;
			}
		}

		protected CallableTypeReference callable_type_reference()
		{
			IToken token = null;
			CallableTypeReference callableTypeReference = null;
			TypeReference typeReference = null;
			ParameterDeclarationCollection c = null;
			try
			{
				token = LT(1);
				match(14);
				match(75);
				if (0 == inputState.guessing)
				{
					callableTypeReference = new CallableTypeReference(SourceLocationFactory.ToLexicalInfo(token));
					c = callableTypeReference.Parameters;
				}
				callable_parameter_declaration_list(c);
				match(76);
				if (LA(1) == 11 && tokenSet_40_.member(LA(2)))
				{
					match(11);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						callableTypeReference.ReturnType = typeReference;
					}
				}
				else if (!tokenSet_38_.member(LA(1)) || !tokenSet_41_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return callableTypeReference;
		}

		protected void type_reference_list(TypeReferenceCollection container)
		{
			TypeReference typeReference = null;
			try
			{
				typeReference = type_reference();
				if (0 == inputState.guessing)
				{
					container.Add(typeReference);
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 84)
					{
						match(84);
						typeReference = type_reference();
						if (0 == inputState.guessing)
						{
							container.Add(typeReference);
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_36_);
					return;
				}
				throw ex;
			}
		}

		protected SpliceTypeReference splice_type_reference()
		{
			IToken token = null;
			SpliceTypeReference result = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(85);
				expression = atom();
				if (0 == inputState.guessing)
				{
					result = new SpliceTypeReference(SourceLocationFactory.ToLexicalInfo(token), expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected IToken type_name()
		{
			IToken token = null;
			IToken token2 = null;
			IToken result = null;
			try
			{
				switch (LA(1))
				{
				case 71:
					result = identifier();
					break;
				case 14:
					token = LT(1);
					match(14);
					if (0 == inputState.guessing)
					{
						result = token;
					}
					break;
				case 16:
					token2 = LT(1);
					match(16);
					if (0 == inputState.guessing)
					{
						result = token2;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected MacroStatement closure_macro_stmt()
		{
			IToken token = null;
			MacroStatement result = null;
			MacroStatement macroStatement = new MacroStatement();
			try
			{
				token = LT(1);
				match(71);
				expression_list(macroStatement.Arguments);
				if (0 == inputState.guessing)
				{
					macroStatement.Name = token.getText();
					macroStatement.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token);
					result = macroStatement;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_84_);
			}
			return result;
		}

		protected void macro_block(StatementCollection container)
		{
			try
			{
				switch (LA(1))
				{
				case 1:
				case 73:
				case 74:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 6:
				case 9:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 19:
				case 26:
				case 30:
				case 32:
				case 33:
				case 35:
				case 37:
				case 38:
				case 41:
				case 44:
				case 46:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 55:
				case 57:
				case 58:
				case 59:
				case 60:
				case 62:
				case 63:
				case 64:
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 87:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					break;
				}
				int num = 0;
				while (true)
				{
					bool flag = true;
					if (tokenSet_18_.member(LA(1)) && tokenSet_85_.member(LA(2)))
					{
						stmt(container);
					}
					else
					{
						if (!tokenSet_5_.member(LA(1)) || !tokenSet_6_.member(LA(2)))
						{
							break;
						}
						type_member_stmt(container);
					}
					num++;
				}
				if (num >= 1)
				{
					return;
				}
				throw new NoViableAltException(LT(1), getFilename());
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_86_);
					return;
				}
				throw ex;
			}
		}

		protected void type_member_stmt(StatementCollection container)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection();
			try
			{
				type_member(typeMemberCollection);
				if (0 == inputState.guessing)
				{
					container.Add(new TypeMemberStatement(typeMemberCollection[0]));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_87_);
					return;
				}
				throw ex;
			}
		}

		protected void macro_compound_stmt(Block b)
		{
			IToken token = null;
			StatementCollection container = null;
			try
			{
				token = LT(1);
				match(87);
				if (0 == inputState.guessing)
				{
					b.LexicalInfo = ToLexicalInfo(token);
					container = b.Statements;
				}
				macro_block(container);
				end(b);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_23_);
					return;
				}
				throw ex;
			}
		}

		protected StatementModifier stmt_modifier()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			StatementModifier statementModifier = null;
			Expression expression = null;
			IToken token4 = null;
			StatementModifierType type = StatementModifierType.None;
			try
			{
				switch (LA(1))
				{
				case 41:
					token = LT(1);
					match(41);
					if (0 == inputState.guessing)
					{
						token4 = token;
						type = StatementModifierType.If;
					}
					break;
				case 66:
					token2 = LT(1);
					match(66);
					if (0 == inputState.guessing)
					{
						token4 = token2;
						type = StatementModifierType.Unless;
					}
					break;
				case 69:
					token3 = LT(1);
					match(69);
					if (0 == inputState.guessing)
					{
						token4 = token3;
						type = StatementModifierType.While;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				expression = boolean_expression();
				if (0 == inputState.guessing)
				{
					statementModifier = new StatementModifier(SourceLocationFactory.ToLexicalInfo(token4));
					statementModifier.Type = type;
					statementModifier.Condition = expression;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_88_);
			}
			return statementModifier;
		}

		protected GotoStatement goto_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			GotoStatement result = null;
			try
			{
				token = LT(1);
				match(35);
				token2 = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					result = new GotoStatement(SourceLocationFactory.ToLexicalInfo(token), new ReferenceExpression(SourceLocationFactory.ToLexicalInfo(token2), token2.getText()));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected LabelStatement label_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			LabelStatement result = null;
			try
			{
				token = LT(1);
				match(87);
				token2 = LT(1);
				match(71);
				if (0 == inputState.guessing)
				{
					result = new LabelStatement(SourceLocationFactory.ToLexicalInfo(token), token2.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected ForStatement for_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			ForStatement forStatement = null;
			Expression expression = null;
			DeclarationCollection dc = null;
			Block block = null;
			Block node = null;
			try
			{
				token = LT(1);
				match(32);
				if (0 == inputState.guessing)
				{
					forStatement = new ForStatement(SourceLocationFactory.ToLexicalInfo(token));
					dc = forStatement.Declarations;
					node = (block = forStatement.Block);
				}
				declaration_list(dc);
				match(42);
				expression = array_or_expression();
				if (0 == inputState.guessing)
				{
					forStatement.Iterator = expression;
				}
				begin();
				this.block(block.Statements);
				switch (LA(1))
				{
				case 48:
					token2 = LT(1);
					match(48);
					if (0 == inputState.guessing)
					{
						Block block3 = (forStatement.OrBlock = new Block(SourceLocationFactory.ToLexicalInfo(token2)));
						node = block3;
					}
					begin();
					this.block(forStatement.OrBlock.Statements);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
				case 61:
					break;
				}
				switch (LA(1))
				{
				case 61:
					token3 = LT(1);
					match(61);
					if (0 == inputState.guessing)
					{
						Block block3 = (forStatement.ThenBlock = new Block(SourceLocationFactory.ToLexicalInfo(token3)));
						node = block3;
					}
					begin();
					this.block(forStatement.ThenBlock.Statements);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
					break;
				}
				end(node);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return forStatement;
		}

		protected WhileStatement while_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			WhileStatement whileStatement = null;
			Expression expression = null;
			Block node = null;
			try
			{
				token = LT(1);
				match(69);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					whileStatement = new WhileStatement(SourceLocationFactory.ToLexicalInfo(token));
					whileStatement.Condition = expression;
					node = whileStatement.Block;
				}
				begin();
				this.block(whileStatement.Block.Statements);
				switch (LA(1))
				{
				case 48:
					token2 = LT(1);
					match(48);
					if (0 == inputState.guessing)
					{
						Block block2 = (whileStatement.OrBlock = new Block(SourceLocationFactory.ToLexicalInfo(token2)));
						node = block2;
					}
					begin();
					this.block(whileStatement.OrBlock.Statements);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
				case 61:
					break;
				}
				switch (LA(1))
				{
				case 61:
					token3 = LT(1);
					match(61);
					if (0 == inputState.guessing)
					{
						Block block2 = (whileStatement.ThenBlock = new Block(SourceLocationFactory.ToLexicalInfo(token3)));
						node = block2;
					}
					begin();
					this.block(whileStatement.ThenBlock.Statements);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
					break;
				}
				end(node);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return whileStatement;
		}

		protected IfStatement if_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IfStatement result = null;
			IfStatement ifStatement = null;
			Expression expression = null;
			Block node = null;
			try
			{
				token = LT(1);
				match(41);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					result = (ifStatement = new IfStatement(SourceLocationFactory.ToLexicalInfo(token)));
					ifStatement.Condition = expression;
					Block block2 = (ifStatement.TrueBlock = new Block());
					node = block2;
				}
				begin();
				this.block(ifStatement.TrueBlock.Statements);
				while (true)
				{
					bool flag = true;
					if (LA(1) == 22)
					{
						token2 = LT(1);
						match(22);
						expression = this.expression();
						if (0 == inputState.guessing)
						{
							ifStatement.FalseBlock = new Block();
							IfStatement ifStatement2 = new IfStatement(SourceLocationFactory.ToLexicalInfo(token2));
							Block block2 = (ifStatement2.TrueBlock = new Block());
							node = block2;
							ifStatement2.Condition = expression;
							ifStatement.FalseBlock.Add(ifStatement2);
							ifStatement = ifStatement2;
						}
						begin();
						this.block(ifStatement.TrueBlock.Statements);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 23:
					token3 = LT(1);
					match(23);
					if (0 == inputState.guessing)
					{
						Block block2 = (ifStatement.FalseBlock = new Block(SourceLocationFactory.ToLexicalInfo(token3)));
						node = block2;
					}
					begin();
					this.block(ifStatement.FalseBlock.Statements);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
					break;
				}
				end(node);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return result;
		}

		protected UnlessStatement unless_stmt()
		{
			IToken token = null;
			UnlessStatement unlessStatement = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(66);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					unlessStatement = new UnlessStatement(SourceLocationFactory.ToLexicalInfo(token));
					unlessStatement.Condition = expression;
				}
				compound_stmt(unlessStatement.Block);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return unlessStatement;
		}

		protected TryStatement try_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			TryStatement tryStatement = null;
			Block block = null;
			Block node = null;
			try
			{
				token = LT(1);
				match(62);
				if (0 == inputState.guessing)
				{
					tryStatement = new TryStatement(SourceLocationFactory.ToLexicalInfo(token));
				}
				begin();
				if (0 == inputState.guessing)
				{
					tryStatement.ProtectedBlock = new Block();
				}
				this.block(tryStatement.ProtectedBlock.Statements);
				if (0 == inputState.guessing)
				{
					node = tryStatement.ProtectedBlock;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 28)
					{
						node = exception_handler(tryStatement);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 29:
					token2 = LT(1);
					match(29);
					if (0 == inputState.guessing)
					{
						block = new Block(SourceLocationFactory.ToLexicalInfo(token2));
					}
					begin();
					this.block(block.Statements);
					if (0 == inputState.guessing)
					{
						node = (tryStatement.FailureBlock = block);
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
				case 25:
					break;
				}
				switch (LA(1))
				{
				case 25:
					token3 = LT(1);
					match(25);
					if (0 == inputState.guessing)
					{
						block = new Block(SourceLocationFactory.ToLexicalInfo(token3));
					}
					begin();
					this.block(block.Statements);
					if (0 == inputState.guessing)
					{
						node = (tryStatement.EnsureBlock = block);
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 24:
					break;
				}
				end(node);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return tryStatement;
		}

		protected ExpressionStatement expression_stmt()
		{
			ExpressionStatement result = null;
			Expression expression = null;
			try
			{
				expression = assignment_expression();
				if (0 == inputState.guessing)
				{
					result = new ExpressionStatement(expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected Statement assignment_or_method_invocation_with_block_stmt()
		{
			IToken token = null;
			Statement statement = null;
			Expression expression = null;
			Expression expression2 = null;
			StatementModifier modifier = null;
			BinaryOperatorType operator_ = BinaryOperatorType.None;
			IToken token2 = null;
			try
			{
				expression = slicing_expression();
				switch (LA(1))
				{
				case 19:
				case 21:
				case 87:
					expression = method_invocation_block(expression);
					if (0 == inputState.guessing)
					{
						statement = new ExpressionStatement(expression);
					}
					break;
				case 82:
					token = LT(1);
					match(82);
					if (0 == inputState.guessing)
					{
						token2 = token;
						operator_ = OperatorParser.ParseAssignment(token.getText());
					}
					switch (LA(1))
					{
					case 19:
					case 21:
					case 87:
						expression2 = callable_expression();
						break;
					case 6:
					case 15:
					case 16:
					case 33:
					case 45:
					case 46:
					case 57:
					case 58:
					case 64:
					case 65:
					case 71:
					case 72:
					case 75:
					case 77:
					case 78:
					case 79:
					case 80:
					case 83:
					case 84:
					case 85:
					case 86:
					case 91:
					case 93:
					case 110:
					case 111:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
						expression2 = array_or_expression();
						switch (LA(1))
						{
						case 19:
						case 21:
						case 87:
							expression2 = method_invocation_block(expression2);
							break;
						case 41:
						case 66:
						case 69:
							modifier = stmt_modifier();
							eos();
							break;
						case 1:
						case 73:
						case 74:
							eos();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						statement = new ExpressionStatement(new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token2), operator_, expression, expression2));
						statement.Modifier = modifier;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return statement;
		}

		protected ReturnStatement return_stmt()
		{
			IToken token = null;
			ReturnStatement returnStatement = null;
			Expression e = null;
			StatementModifier modifier = null;
			try
			{
				token = LT(1);
				match(55);
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 84:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					e = array_or_expression();
					switch (LA(1))
					{
					case 19:
					case 21:
					case 87:
						e = method_invocation_block(e);
						break;
					case 1:
					case 41:
					case 66:
					case 69:
					case 73:
					case 74:
						switch (LA(1))
						{
						case 41:
						case 66:
						case 69:
							modifier = stmt_modifier();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 1:
						case 73:
						case 74:
							break;
						}
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				case 19:
				case 21:
				case 87:
					e = callable_expression();
					break;
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
					switch (LA(1))
					{
					case 41:
					case 66:
					case 69:
						modifier = stmt_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 1:
					case 73:
					case 74:
						break;
					}
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					returnStatement = new ReturnStatement(SourceLocationFactory.ToLexicalInfo(token));
					returnStatement.Modifier = modifier;
					returnStatement.Expression = e;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return returnStatement;
		}

		protected Declaration declaration()
		{
			IToken token = null;
			Declaration declaration = null;
			TypeReference type = null;
			try
			{
				token = LT(1);
				match(71);
				switch (LA(1))
				{
				case 11:
					match(11);
					type = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 42:
				case 82:
				case 84:
					break;
				}
				if (0 == inputState.guessing)
				{
					declaration = new Declaration(SourceLocationFactory.ToLexicalInfo(token));
					declaration.Name = token.getText();
					declaration.Type = type;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_89_);
			}
			return declaration;
		}

		protected UnpackStatement unpack_stmt()
		{
			UnpackStatement unpackStatement = null;
			StatementModifier modifier = null;
			try
			{
				unpackStatement = unpack();
				switch (LA(1))
				{
				case 41:
				case 66:
				case 69:
					modifier = stmt_modifier();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 73:
				case 74:
					break;
				}
				eos();
				if (0 == inputState.guessing)
				{
					unpackStatement.Modifier = modifier;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return unpackStatement;
		}

		protected DeclarationStatement declaration_stmt()
		{
			IToken token = null;
			DeclarationStatement declarationStatement = null;
			TypeReference typeReference = null;
			Expression initializer = null;
			StatementModifier modifier = null;
			try
			{
				token = LT(1);
				match(71);
				match(11);
				typeReference = type_reference();
				switch (LA(1))
				{
				case 82:
					match(82);
					initializer = declaration_initializer();
					break;
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
					switch (LA(1))
					{
					case 41:
					case 66:
					case 69:
						modifier = stmt_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 1:
					case 73:
					case 74:
						break;
					}
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					Declaration declaration = new Declaration(SourceLocationFactory.ToLexicalInfo(token));
					declaration.Name = token.getText();
					declaration.Type = typeReference;
					declarationStatement = new DeclarationStatement(declaration.LexicalInfo);
					declarationStatement.Declaration = declaration;
					declarationStatement.Initializer = initializer;
					declarationStatement.Modifier = modifier;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_80_);
			}
			return declarationStatement;
		}

		protected YieldStatement yield_stmt()
		{
			IToken token = null;
			YieldStatement yieldStatement = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(70);
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 84:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = array_or_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
				case 92:
					break;
				}
				if (0 == inputState.guessing)
				{
					yieldStatement = new YieldStatement(SourceLocationFactory.ToLexicalInfo(token));
					yieldStatement.Expression = expression;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_84_);
			}
			return yieldStatement;
		}

		protected BreakStatement break_stmt()
		{
			IToken token = null;
			BreakStatement result = null;
			try
			{
				token = LT(1);
				match(12);
				if (0 == inputState.guessing)
				{
					result = new BreakStatement(SourceLocationFactory.ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected Statement continue_stmt()
		{
			IToken token = null;
			Statement result = null;
			try
			{
				token = LT(1);
				match(13);
				if (0 == inputState.guessing)
				{
					result = new ContinueStatement(SourceLocationFactory.ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected RaiseStatement raise_stmt()
		{
			IToken token = null;
			RaiseStatement raiseStatement = null;
			Expression exception = null;
			try
			{
				token = LT(1);
				match(53);
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					exception = expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
				case 92:
					break;
				}
				if (0 == inputState.guessing)
				{
					raiseStatement = new RaiseStatement(SourceLocationFactory.ToLexicalInfo(token));
					raiseStatement.Exception = exception;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_84_);
			}
			return raiseStatement;
		}

		protected Expression boolean_expression()
		{
			IToken token = null;
			Expression expression = null;
			Expression expression2 = null;
			try
			{
				expression = boolean_term();
				while (true)
				{
					bool flag = true;
					if (LA(1) == 48)
					{
						token = LT(1);
						match(48);
						expression2 = boolean_term();
						if (0 == inputState.guessing)
						{
							BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token));
							binaryExpression.Operator = BinaryOperatorType.Or;
							binaryExpression.Left = expression;
							binaryExpression.Right = expression2;
							expression = binaryExpression;
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_90_);
			}
			return expression;
		}

		protected Expression callable_or_expression()
		{
			Expression result = null;
			try
			{
				switch (LA(1))
				{
				case 19:
				case 21:
				case 87:
					result = callable_expression();
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 84:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					result = array_or_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_0_);
			}
			return result;
		}

		protected void closure_parameters_test()
		{
			try
			{
				switch (LA(1))
				{
				case 54:
					parameter_modifier();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 71:
					break;
				}
				match(71);
				switch (LA(1))
				{
				case 11:
					match(11);
					type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 84:
				case 90:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 84)
					{
						match(84);
						match(71);
						switch (LA(1))
						{
						case 11:
							match(11);
							type_reference();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 84:
						case 90:
							break;
						}
						continue;
					}
					break;
				}
				match(90);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_0_);
					return;
				}
				throw ex;
			}
		}

		protected void internal_closure_stmt(Block block)
		{
			Statement statement = null;
			StatementModifier statementModifier = null;
			try
			{
				switch (LA(1))
				{
				case 55:
					statement = return_expression_stmt();
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 53:
				case 57:
				case 58:
				case 64:
				case 65:
				case 70:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 84:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					switch (LA(1))
					{
					case 53:
						statement = raise_stmt();
						break;
					case 70:
						statement = yield_stmt();
						break;
					default:
					{
						bool flag = false;
						if (LA(1) == 71 && (LA(2) == 11 || LA(2) == 84))
						{
							int pos = mark();
							flag = true;
							inputState.guessing++;
							try
							{
								declaration();
								match(84);
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
							statement = unpack();
							break;
						}
						if (LA(1) == 71 && tokenSet_91_.member(LA(2)) && IsValidMacroArgument(LA(2)))
						{
							statement = closure_macro_stmt();
							break;
						}
						if (tokenSet_70_.member(LA(1)) && tokenSet_25_.member(LA(2)))
						{
							statement = closure_expression_stmt();
							break;
						}
						throw new NoViableAltException(LT(1), getFilename());
					}
					}
					switch (LA(1))
					{
					case 41:
					case 66:
					case 69:
						statementModifier = stmt_modifier();
						if (0 == inputState.guessing)
						{
							statement.Modifier = statementModifier;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 1:
					case 73:
					case 74:
					case 92:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing && null != statement)
				{
					block.Add(statement);
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex2);
					recover(ex2, tokenSet_92_);
					return;
				}
				throw ex2;
			}
		}

		protected ReturnStatement return_expression_stmt()
		{
			IToken token = null;
			ReturnStatement returnStatement = null;
			Expression expression = null;
			StatementModifier modifier = null;
			try
			{
				token = LT(1);
				match(55);
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 84:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = array_or_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 41:
				case 66:
				case 69:
				case 73:
				case 74:
				case 92:
					break;
				}
				switch (LA(1))
				{
				case 41:
				case 66:
				case 69:
					modifier = stmt_modifier();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 73:
				case 74:
				case 92:
					break;
				}
				if (0 == inputState.guessing)
				{
					returnStatement = new ReturnStatement(SourceLocationFactory.ToLexicalInfo(token));
					returnStatement.Modifier = modifier;
					returnStatement.Expression = expression;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_92_);
			}
			return returnStatement;
		}

		protected UnpackStatement unpack()
		{
			IToken token = null;
			Declaration declaration = null;
			UnpackStatement unpackStatement = new UnpackStatement();
			Expression expression = null;
			try
			{
				declaration = this.declaration();
				match(84);
				if (0 == inputState.guessing)
				{
					unpackStatement.Declarations.Add(declaration);
				}
				switch (LA(1))
				{
				case 71:
					declaration_list(unpackStatement.Declarations);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 82:
					break;
				}
				token = LT(1);
				match(82);
				expression = array_or_expression();
				if (0 == inputState.guessing)
				{
					unpackStatement.Expression = expression;
					unpackStatement.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_84_);
			}
			return unpackStatement;
		}

		protected Statement closure_expression_stmt()
		{
			Statement result = null;
			Expression expression = null;
			try
			{
				expression = array_or_expression();
				if (0 == inputState.guessing)
				{
					result = new ExpressionStatement(expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_84_);
			}
			return result;
		}

		protected Expression closure_expression()
		{
			IToken token = null;
			IToken token2 = null;
			Expression result = null;
			BlockExpression blockExpression = null;
			ParameterDeclarationCollection c = null;
			Block block = null;
			try
			{
				token = LT(1);
				match(91);
				if (0 == inputState.guessing)
				{
					result = (blockExpression = new BlockExpression(SourceLocationFactory.ToLexicalInfo(token)));
					blockExpression.Annotate("inline");
					c = blockExpression.Parameters;
					block = blockExpression.Body;
				}
				bool flag = false;
				if (tokenSet_93_.member(LA(1)) && tokenSet_94_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						closure_parameters_test();
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
					parameter_declaration_list(c);
					match(90);
				}
				else if (!tokenSet_95_.member(LA(1)) || !tokenSet_25_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				internal_closure_stmt(block);
				while (true)
				{
					bool flag2 = true;
					if (LA(1) == 1 || LA(1) == 73 || LA(1) == 74)
					{
						eos();
						switch (LA(1))
						{
						case 6:
						case 15:
						case 16:
						case 33:
						case 45:
						case 46:
						case 53:
						case 55:
						case 57:
						case 58:
						case 64:
						case 65:
						case 70:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 84:
						case 85:
						case 86:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							internal_closure_stmt(block);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 1:
						case 73:
						case 74:
						case 92:
							break;
						}
						continue;
					}
					break;
				}
				token2 = LT(1);
				match(92);
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return result;
		}

		protected Block exception_handler(TryStatement t)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			ExceptionHandler exceptionHandler = null;
			TypeReference typeReference = null;
			Expression expression = null;
			Block result = null;
			try
			{
				token = LT(1);
				match(28);
				switch (LA(1))
				{
				case 71:
					token2 = LT(1);
					match(71);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 11:
				case 41:
				case 66:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 11:
					match(11);
					typeReference = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 41:
				case 66:
				case 87:
					break;
				}
				switch (LA(1))
				{
				case 41:
				case 66:
					switch (LA(1))
					{
					case 41:
						match(41);
						break;
					case 66:
						token3 = LT(1);
						match(66);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = this.expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 87:
					break;
				}
				begin();
				if (0 == inputState.guessing)
				{
					exceptionHandler = new ExceptionHandler(SourceLocationFactory.ToLexicalInfo(token));
					exceptionHandler.Declaration = new Declaration();
					exceptionHandler.Declaration.Type = typeReference;
					if (token2 != null)
					{
						exceptionHandler.Declaration.LexicalInfo = SourceLocationFactory.ToLexicalInfo(token2);
						exceptionHandler.Declaration.Name = token2.getText();
					}
					else
					{
						exceptionHandler.Declaration.Name = null;
						exceptionHandler.Flags |= ExceptionHandlerFlags.Anonymous;
					}
					if (typeReference != null)
					{
						exceptionHandler.Declaration.LexicalInfo = typeReference.LexicalInfo;
					}
					else if (token2 != null)
					{
						exceptionHandler.Declaration.LexicalInfo = exceptionHandler.LexicalInfo;
					}
					if (typeReference == null)
					{
						exceptionHandler.Flags |= ExceptionHandlerFlags.Untyped;
					}
					if (expression != null)
					{
						if (token3 != null)
						{
							UnaryExpression unaryExpression = new UnaryExpression(SourceLocationFactory.ToLexicalInfo(token3));
							unaryExpression.Operator = UnaryOperatorType.LogicalNot;
							unaryExpression.Operand = expression;
							expression = unaryExpression;
						}
						exceptionHandler.FilterCondition = expression;
						exceptionHandler.Flags |= ExceptionHandlerFlags.Filter;
					}
					exceptionHandler.Block = new Block(SourceLocationFactory.ToLexicalInfo(token));
				}
				block(exceptionHandler.Block.Statements);
				if (0 == inputState.guessing)
				{
					result = exceptionHandler.Block;
					t.ExceptionHandlers.Add(exceptionHandler);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_96_);
			}
			return result;
		}

		protected Expression assignment_expression()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			IToken token6 = null;
			Expression expression = null;
			Expression expression2 = null;
			IToken token7 = null;
			BinaryOperatorType @operator = BinaryOperatorType.None;
			try
			{
				expression = conditional_expression();
				switch (LA(1))
				{
				case 82:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
					switch (LA(1))
					{
					case 82:
						token = LT(1);
						match(82);
						if (0 == inputState.guessing)
						{
							token7 = token;
							@operator = OperatorParser.ParseAssignment(token.getText());
						}
						break;
					case 95:
						token2 = LT(1);
						match(95);
						if (0 == inputState.guessing)
						{
							token7 = token2;
							@operator = BinaryOperatorType.InPlaceBitwiseOr;
						}
						break;
					case 96:
						token3 = LT(1);
						match(96);
						if (0 == inputState.guessing)
						{
							token7 = token3;
							@operator = BinaryOperatorType.InPlaceExclusiveOr;
						}
						break;
					case 97:
						token4 = LT(1);
						match(97);
						if (0 == inputState.guessing)
						{
							token7 = token4;
							@operator = BinaryOperatorType.InPlaceBitwiseAnd;
						}
						break;
					case 98:
						token5 = LT(1);
						match(98);
						if (0 == inputState.guessing)
						{
							token7 = token5;
							@operator = BinaryOperatorType.InPlaceShiftLeft;
						}
						break;
					case 99:
						token6 = LT(1);
						match(99);
						if (0 == inputState.guessing)
						{
							token7 = token6;
							@operator = BinaryOperatorType.InPlaceShiftRight;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression2 = assignment_expression();
					if (0 == inputState.guessing)
					{
						BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token7));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = expression2;
						expression = binaryExpression;
					}
					break;
				case 1:
				case 6:
				case 10:
				case 19:
				case 21:
				case 23:
				case 32:
				case 41:
				case 48:
				case 66:
				case 69:
				case 71:
				case 73:
				case 74:
				case 76:
				case 81:
				case 84:
				case 87:
				case 92:
				case 94:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_97_);
			}
			return expression;
		}

		protected void declaration_list(DeclarationCollection dc)
		{
			Declaration declaration = null;
			try
			{
				declaration = this.declaration();
				if (0 == inputState.guessing)
				{
					dc.Add(declaration);
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 84)
					{
						match(84);
						declaration = this.declaration();
						if (0 == inputState.guessing)
						{
							dc.Add(declaration);
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_98_);
					return;
				}
				throw ex;
			}
		}

		public void generator_expression_body(GeneratorExpression ge)
		{
			StatementModifier statementModifier = null;
			Expression expression = null;
			DeclarationCollection dc = ge?.Declarations;
			try
			{
				declaration_list(dc);
				match(42);
				expression = boolean_expression();
				if (0 == inputState.guessing)
				{
					ge.Iterator = expression;
				}
				if ((LA(1) == 41 || LA(1) == 66 || LA(1) == 69) && tokenSet_4_.member(LA(2)))
				{
					statementModifier = stmt_modifier();
					if (0 == inputState.guessing)
					{
						ge.Filter = statementModifier;
					}
				}
				else if (!tokenSet_88_.member(LA(1)) || !tokenSet_99_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_88_);
					return;
				}
				throw ex;
			}
		}

		protected Expression boolean_term()
		{
			IToken token = null;
			Expression expression = null;
			Expression expression2 = null;
			try
			{
				expression = not_expression();
				while (true)
				{
					bool flag = true;
					if (LA(1) == 10)
					{
						token = LT(1);
						match(10);
						expression2 = not_expression();
						if (0 == inputState.guessing)
						{
							BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token));
							binaryExpression.Operator = BinaryOperatorType.And;
							binaryExpression.Left = expression;
							binaryExpression.Right = expression2;
							expression = binaryExpression;
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_100_);
			}
			return expression;
		}

		protected Expression not_expression()
		{
			IToken token = null;
			Expression expression = null;
			try
			{
				switch (LA(1))
				{
				case 45:
					token = LT(1);
					match(45);
					expression = not_expression();
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = assignment_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing && token != null)
				{
					UnaryExpression unaryExpression = new UnaryExpression(SourceLocationFactory.ToLexicalInfo(token));
					unaryExpression.Operator = UnaryOperatorType.LogicalNot;
					unaryExpression.Operand = expression;
					expression = unaryExpression;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_97_);
			}
			return expression;
		}

		public QuasiquoteExpression ast_literal_expression()
		{
			IToken token = null;
			IToken token2 = null;
			Node node = null;
			QuasiquoteExpression quasiquoteExpression = null;
			try
			{
				token = LT(1);
				match(93);
				if (0 == inputState.guessing)
				{
					quasiquoteExpression = new QuasiquoteExpression(SourceLocationFactory.ToLexicalInfo(token));
				}
				bool flag = false;
				if (tokenSet_4_.member(LA(1)) && tokenSet_25_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						expression();
						match(94);
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
					node = expression();
					if (0 == inputState.guessing)
					{
						quasiquoteExpression.Node = node;
					}
				}
				else
				{
					if (!tokenSet_101_.member(LA(1)) || !tokenSet_2_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					if ((LA(1) == 1 || LA(1) == 73 || LA(1) == 74) && tokenSet_101_.member(LA(2)))
					{
						eos();
					}
					else if (!tokenSet_101_.member(LA(1)) || !tokenSet_2_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					ast_literal_block(quasiquoteExpression);
				}
				token2 = LT(1);
				match(94);
				if (0 == inputState.guessing)
				{
					quasiquoteExpression.EndSourceLocation = SourceLocationFactory.ToSourceLocation(token2);
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return quasiquoteExpression;
		}

		public void ast_literal_block(QuasiquoteExpression e)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection();
			Block block = new Block();
			StatementCollection statements = block.Statements;
			Node node = null;
			try
			{
				bool flag = false;
				if (tokenSet_1_.member(LA(1)) && tokenSet_2_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						ast_literal_module_prediction();
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
					ast_literal_module(e);
					return;
				}
				bool flag2 = false;
				if (tokenSet_102_.member(LA(1)) && tokenSet_103_.member(LA(2)))
				{
					int pos2 = mark();
					flag2 = true;
					inputState.guessing++;
					try
					{
						attributes();
						if (tokenSet_32_.member(LA(1)))
						{
							type_member_modifier();
						}
						else
						{
							if (!tokenSet_104_.member(LA(1)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							modifiers();
							switch (LA(1))
							{
							case 17:
								match(17);
								break;
							case 26:
								match(26);
								break;
							case 60:
								match(60);
								break;
							case 37:
								match(37);
								break;
							case 27:
								match(27);
								break;
							case 19:
								match(19);
								break;
							case 14:
								match(14);
								break;
							case 71:
								match(71);
								switch (LA(1))
								{
								case 11:
									match(11);
									type_reference();
									break;
								default:
									throw new NoViableAltException(LT(1), getFilename());
								case 87:
									break;
								}
								begin_with_doc(null);
								switch (LA(1))
								{
								case 34:
									match(34);
									break;
								case 56:
									match(56);
									break;
								default:
									throw new NoViableAltException(LT(1), getFilename());
								}
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
						}
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
					int num = 0;
					while (true)
					{
						bool flag3 = true;
						if (!tokenSet_102_.member(LA(1)))
						{
							break;
						}
						type_definition_member(typeMemberCollection);
						num++;
					}
					if (num < 1)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						if (typeMemberCollection.Count == 1)
						{
							e.Node = typeMemberCollection[0];
							return;
						}
						Module module = CodeFactory.NewQuasiquoteModule(e.LexicalInfo);
						module.Members = typeMemberCollection;
						e.Node = module;
					}
					return;
				}
				if (tokenSet_18_.member(LA(1)) && tokenSet_85_.member(LA(2)))
				{
					int num2 = 0;
					while (true)
					{
						bool flag3 = true;
						if (!tokenSet_18_.member(LA(1)))
						{
							break;
						}
						stmt(statements);
						num2++;
					}
					if (num2 < 1)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						e.Node = ((block.Statements.Count > 1) ? block : block.Statements[0]);
					}
					return;
				}
				throw new NoViableAltException(LT(1), getFilename());
			}
			catch (RecognitionException ex3)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex3);
					recover(ex3, tokenSet_105_);
					return;
				}
				throw ex3;
			}
		}

		public void ast_literal_module(QuasiquoteExpression e)
		{
			Module module = (Module)(e.Node = CodeFactory.NewQuasiquoteModule(e.LexicalInfo));
			try
			{
				parse_module(module);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_105_);
					return;
				}
				throw ex;
			}
		}

		public void ast_literal_module_prediction()
		{
			try
			{
				switch (LA(1))
				{
				case 1:
				case 73:
				case 74:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 36:
				case 43:
					break;
				}
				switch (LA(1))
				{
				case 43:
					match(43);
					break;
				case 36:
					match(36);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_0_);
					return;
				}
				throw ex;
			}
		}

		protected Expression conditional_expression()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			IToken token6 = null;
			IToken token7 = null;
			IToken token8 = null;
			Expression expression = null;
			Expression right = null;
			BinaryOperatorType @operator = BinaryOperatorType.None;
			IToken token9 = null;
			TypeReference typeReference = null;
			try
			{
				expression = sum();
				while (true)
				{
					bool flag = true;
					if (!tokenSet_106_.member(LA(1)))
					{
						break;
					}
					switch (LA(1))
					{
					case 39:
					case 42:
					case 45:
					case 100:
					case 101:
					case 102:
						switch (LA(1))
						{
						case 100:
							token = LT(1);
							match(100);
							if (0 == inputState.guessing)
							{
								@operator = OperatorParser.ParseComparison(token.getText());
								token9 = token;
							}
							break;
						case 101:
							token2 = LT(1);
							match(101);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.GreaterThan;
								token9 = token2;
							}
							break;
						case 102:
							token3 = LT(1);
							match(102);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.LessThan;
								token9 = token3;
							}
							break;
						case 45:
							token6 = LT(1);
							match(45);
							match(42);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.NotMember;
								token9 = token6;
							}
							break;
						case 42:
							token7 = LT(1);
							match(42);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.Member;
								token9 = token7;
							}
							break;
						default:
							if (LA(1) == 39 && LA(2) == 45)
							{
								token4 = LT(1);
								match(39);
								match(45);
								if (0 == inputState.guessing)
								{
									@operator = BinaryOperatorType.ReferenceInequality;
									token9 = token4;
								}
								break;
							}
							if (LA(1) == 39 && tokenSet_76_.member(LA(2)))
							{
								token5 = LT(1);
								match(39);
								if (0 == inputState.guessing)
								{
									@operator = BinaryOperatorType.ReferenceEquality;
									token9 = token5;
								}
								break;
							}
							throw new NoViableAltException(LT(1), getFilename());
						}
						right = sum();
						break;
					case 40:
						token8 = LT(1);
						match(40);
						typeReference = type_reference();
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.TypeTest;
							token9 = token8;
							right = new TypeofExpression(typeReference.LexicalInfo, typeReference);
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token9));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = right;
						expression = binaryExpression;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_107_);
			}
			return expression;
		}

		protected Expression sum()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			Expression expression = null;
			Expression expression2 = null;
			IToken token5 = null;
			BinaryOperatorType @operator = BinaryOperatorType.None;
			try
			{
				expression = term();
				while (true)
				{
					bool flag = true;
					if (!tokenSet_108_.member(LA(1)))
					{
						break;
					}
					switch (LA(1))
					{
					case 103:
						token = LT(1);
						match(103);
						if (0 == inputState.guessing)
						{
							token5 = token;
							@operator = BinaryOperatorType.Addition;
						}
						break;
					case 83:
						token2 = LT(1);
						match(83);
						if (0 == inputState.guessing)
						{
							token5 = token2;
							@operator = BinaryOperatorType.Subtraction;
						}
						break;
					case 90:
						token3 = LT(1);
						match(90);
						if (0 == inputState.guessing)
						{
							token5 = token3;
							@operator = BinaryOperatorType.BitwiseOr;
						}
						break;
					case 104:
						token4 = LT(1);
						match(104);
						if (0 == inputState.guessing)
						{
							token5 = token4;
							@operator = BinaryOperatorType.ExclusiveOr;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression2 = term();
					if (0 == inputState.guessing)
					{
						BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token5));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = expression2;
						expression = binaryExpression;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_109_);
			}
			return expression;
		}

		protected Expression term()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			Expression expression = null;
			Expression expression2 = null;
			IToken token5 = null;
			BinaryOperatorType @operator = BinaryOperatorType.None;
			try
			{
				expression = factor();
				while (true)
				{
					bool flag = true;
					if (!tokenSet_110_.member(LA(1)))
					{
						break;
					}
					switch (LA(1))
					{
					case 79:
						token = LT(1);
						match(79);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Multiply;
							token5 = token;
						}
						break;
					case 105:
						token2 = LT(1);
						match(105);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Division;
							token5 = token2;
						}
						break;
					case 106:
						token3 = LT(1);
						match(106);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Modulus;
							token5 = token3;
						}
						break;
					case 107:
						token4 = LT(1);
						match(107);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.BitwiseAnd;
							token5 = token4;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression2 = factor();
					if (0 == inputState.guessing)
					{
						BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token5));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = expression2;
						expression = binaryExpression;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_111_);
			}
			return expression;
		}

		protected Expression factor()
		{
			IToken token = null;
			IToken token2 = null;
			Expression expression = null;
			Expression expression2 = null;
			IToken token3 = null;
			BinaryOperatorType @operator = BinaryOperatorType.None;
			try
			{
				expression = exponentiation();
				while (true)
				{
					bool flag = true;
					if (LA(1) != 108 && LA(1) != 109)
					{
						break;
					}
					switch (LA(1))
					{
					case 108:
						token = LT(1);
						match(108);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.ShiftLeft;
							token3 = token;
						}
						break;
					case 109:
						token2 = LT(1);
						match(109);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.ShiftRight;
							token3 = token2;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression2 = exponentiation();
					if (0 == inputState.guessing)
					{
						BinaryExpression binaryExpression = new BinaryExpression(SourceLocationFactory.ToLexicalInfo(token3));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = expression2;
						expression = binaryExpression;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_112_);
			}
			return expression;
		}

		protected Expression exponentiation()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			Expression expression = null;
			Expression expression2 = null;
			TypeReference typeReference = null;
			try
			{
				expression = unary_expression();
				switch (LA(1))
				{
				case 11:
					token = LT(1);
					match(11);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						TryCastExpression tryCastExpression = new TryCastExpression(ToLexicalInfo(token));
						tryCastExpression.Target = expression;
						tryCastExpression.Type = typeReference;
						expression = tryCastExpression;
					}
					break;
				case 15:
					token2 = LT(1);
					match(15);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						CastExpression castExpression = new CastExpression(ToLexicalInfo(token2));
						castExpression.Target = expression;
						castExpression.Type = typeReference;
						expression = castExpression;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 6:
				case 10:
				case 19:
				case 21:
				case 23:
				case 32:
				case 39:
				case 40:
				case 41:
				case 42:
				case 45:
				case 48:
				case 66:
				case 69:
				case 71:
				case 73:
				case 74:
				case 76:
				case 79:
				case 81:
				case 82:
				case 83:
				case 84:
				case 87:
				case 89:
				case 90:
				case 92:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
				case 103:
				case 104:
				case 105:
				case 106:
				case 107:
				case 108:
				case 109:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 89 && tokenSet_76_.member(LA(2)))
					{
						token3 = LT(1);
						match(89);
						expression2 = exponentiation();
						if (0 == inputState.guessing)
						{
							BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token3));
							binaryExpression.Operator = BinaryOperatorType.Exponentiation;
							binaryExpression.Left = expression;
							binaryExpression.Right = expression2;
							expression = binaryExpression;
						}
						continue;
					}
					break;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_113_);
			}
			return expression;
		}

		protected Expression unary_expression()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			IToken token6 = null;
			IToken token7 = null;
			Expression expression = null;
			IToken token8 = null;
			UnaryOperatorType @operator = UnaryOperatorType.None;
			try
			{
				bool flag = false;
				if ((LA(1) == 83 || LA(1) == 110 || LA(1) == 114) && tokenSet_114_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						match(83);
						match(110);
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
					expression = integer_literal();
				}
				else if (tokenSet_115_.member(LA(1)) && tokenSet_76_.member(LA(2)))
				{
					switch (LA(1))
					{
					case 83:
						token = LT(1);
						match(83);
						if (0 == inputState.guessing)
						{
							token8 = token;
							@operator = UnaryOperatorType.UnaryNegation;
						}
						break;
					case 111:
						token2 = LT(1);
						match(111);
						if (0 == inputState.guessing)
						{
							token8 = token2;
							@operator = UnaryOperatorType.Increment;
						}
						break;
					case 112:
						token3 = LT(1);
						match(112);
						if (0 == inputState.guessing)
						{
							token8 = token3;
							@operator = UnaryOperatorType.Decrement;
						}
						break;
					case 113:
						token4 = LT(1);
						match(113);
						if (0 == inputState.guessing)
						{
							token8 = token4;
							@operator = UnaryOperatorType.OnesComplement;
						}
						break;
					case 79:
						token5 = LT(1);
						match(79);
						if (0 == inputState.guessing)
						{
							token8 = token5;
							@operator = UnaryOperatorType.Explode;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = unary_expression();
				}
				else
				{
					if (!tokenSet_68_.member(LA(1)) || !tokenSet_116_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = slicing_expression();
					switch (LA(1))
					{
					case 111:
						token6 = LT(1);
						match(111);
						if (0 == inputState.guessing)
						{
							token8 = token6;
							@operator = UnaryOperatorType.PostIncrement;
						}
						break;
					case 112:
						token7 = LT(1);
						match(112);
						if (0 == inputState.guessing)
						{
							token8 = token7;
							@operator = UnaryOperatorType.PostDecrement;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 1:
					case 6:
					case 10:
					case 11:
					case 15:
					case 19:
					case 21:
					case 23:
					case 32:
					case 39:
					case 40:
					case 41:
					case 42:
					case 45:
					case 48:
					case 66:
					case 69:
					case 71:
					case 73:
					case 74:
					case 76:
					case 79:
					case 81:
					case 82:
					case 83:
					case 84:
					case 87:
					case 89:
					case 90:
					case 92:
					case 94:
					case 95:
					case 96:
					case 97:
					case 98:
					case 99:
					case 100:
					case 101:
					case 102:
					case 103:
					case 104:
					case 105:
					case 106:
					case 107:
					case 108:
					case 109:
						break;
					}
				}
				if (0 == inputState.guessing && null != token8)
				{
					UnaryExpression unaryExpression = new UnaryExpression(ToLexicalInfo(token8));
					unaryExpression.Operator = @operator;
					unaryExpression.Operand = expression;
					expression = unaryExpression;
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_117_);
			}
			return expression;
		}

		protected Expression literal()
		{
			Expression result = null;
			try
			{
				switch (LA(1))
				{
				case 6:
				case 72:
				case 77:
				case 78:
				case 115:
					result = string_literal();
					break;
				case 80:
					result = list_literal();
					break;
				case 93:
					result = ast_literal_expression();
					break;
				case 116:
					result = re_literal();
					break;
				case 33:
				case 64:
					result = bool_literal();
					break;
				case 46:
					result = null_literal();
					break;
				case 57:
					result = self_literal();
					break;
				case 58:
					result = super_literal();
					break;
				default:
				{
					if ((LA(1) == 83 || LA(1) == 110 || LA(1) == 114) && tokenSet_118_.member(LA(2)))
					{
						result = integer_literal();
						break;
					}
					bool flag = false;
					if (LA(1) == 91 && tokenSet_72_.member(LA(2)))
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							hash_literal_test();
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
						result = hash_literal();
						break;
					}
					if (LA(1) == 91 && tokenSet_119_.member(LA(2)))
					{
						result = closure_expression();
						break;
					}
					if ((LA(1) == 83 || LA(1) == 117 || LA(1) == 118) && tokenSet_120_.member(LA(2)))
					{
						result = double_literal();
						break;
					}
					if ((LA(1) == 83 || LA(1) == 119) && tokenSet_121_.member(LA(2)))
					{
						result = timespan_literal();
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return result;
		}

		protected Expression char_literal()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			Expression result = null;
			try
			{
				token = LT(1);
				match(16);
				match(75);
				switch (LA(1))
				{
				case 78:
					token2 = LT(1);
					match(78);
					if (0 == inputState.guessing)
					{
						result = new CharLiteralExpression(SourceLocationFactory.ToLexicalInfo(token2), token2.getText());
					}
					break;
				case 114:
					token3 = LT(1);
					match(114);
					if (0 == inputState.guessing)
					{
						result = new CharLiteralExpression(SourceLocationFactory.ToLexicalInfo(token3), (char)PrimitiveParser.ParseInt(token3));
					}
					break;
				case 76:
					if (0 == inputState.guessing)
					{
						result = new MethodInvocationExpression(ToLexicalInfo(token), new ReferenceExpression(ToLexicalInfo(token), token.getText()));
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				match(76);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected ReferenceExpression reference_expression()
		{
			IToken token = null;
			IToken token2 = null;
			ReferenceExpression referenceExpression = null;
			IToken token3 = null;
			try
			{
				switch (LA(1))
				{
				case 71:
					token = LT(1);
					match(71);
					if (0 == inputState.guessing)
					{
						token3 = token;
					}
					break;
				case 16:
					token2 = LT(1);
					match(16);
					if (0 == inputState.guessing)
					{
						token3 = token2;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					referenceExpression = new ReferenceExpression(SourceLocationFactory.ToLexicalInfo(token3));
					referenceExpression.Name = token3.getText();
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return referenceExpression;
		}

		protected Expression paren_expression()
		{
			IToken token = null;
			Expression expression = null;
			Expression expression2 = null;
			Expression expression3 = null;
			try
			{
				bool flag = false;
				if (LA(1) == 75 && LA(2) == 47)
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						match(75);
						match(47);
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
					expression = typed_array();
				}
				else
				{
					if (LA(1) != 75 || !tokenSet_70_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token = LT(1);
					match(75);
					expression = array_or_expression();
					switch (LA(1))
					{
					case 41:
						match(41);
						expression2 = boolean_expression();
						match(23);
						expression3 = array_or_expression();
						if (0 == inputState.guessing)
						{
							ConditionalExpression conditionalExpression = new ConditionalExpression(SourceLocationFactory.ToLexicalInfo(token));
							conditionalExpression.Condition = expression2;
							conditionalExpression.TrueValue = expression;
							conditionalExpression.FalseValue = expression3;
							expression = conditionalExpression;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 76:
						break;
					}
					match(76);
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_38_);
			}
			return expression;
		}

		protected Expression cast_expression()
		{
			IToken token = null;
			Expression result = null;
			TypeReference typeReference = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(15);
				match(75);
				typeReference = type_reference();
				match(84);
				expression = this.expression();
				match(76);
				if (0 == inputState.guessing)
				{
					result = new CastExpression(SourceLocationFactory.ToLexicalInfo(token), expression, typeReference);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected Expression typeof_expression()
		{
			IToken token = null;
			Expression result = null;
			TypeReference typeReference = null;
			try
			{
				token = LT(1);
				match(65);
				match(75);
				typeReference = type_reference();
				match(76);
				if (0 == inputState.guessing)
				{
					result = new TypeofExpression(SourceLocationFactory.ToLexicalInfo(token), typeReference);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected Expression omitted_member_expression()
		{
			IToken token = null;
			Expression result = null;
			IToken token2 = null;
			try
			{
				token = LT(1);
				match(86);
				token2 = member();
				if (0 == inputState.guessing)
				{
					result = MemberReferenceForToken(new OmittedExpression(ToLexicalInfo(token)), token2);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected Expression typed_array()
		{
			IToken token = null;
			Expression result = null;
			ArrayLiteralExpression arrayLiteralExpression = null;
			TypeReference typeReference = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(75);
				match(47);
				typeReference = type_reference();
				match(87);
				if (0 == inputState.guessing)
				{
					result = (arrayLiteralExpression = new ArrayLiteralExpression(SourceLocationFactory.ToLexicalInfo(token)));
					arrayLiteralExpression.Type = new ArrayTypeReference(typeReference.LexicalInfo, typeReference);
				}
				switch (LA(1))
				{
				case 84:
					match(84);
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						arrayLiteralExpression.Items.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84 && tokenSet_4_.member(LA(2)))
						{
							match(84);
							expression = this.expression();
							if (0 == inputState.guessing)
							{
								arrayLiteralExpression.Items.Add(expression);
							}
							continue;
						}
						break;
					}
					switch (LA(1))
					{
					case 84:
						match(84);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 76:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				match(76);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected Statement method_invocation_with_block()
		{
			Statement result = null;
			MethodInvocationExpression methodInvocationExpression = null;
			Expression expression = null;
			try
			{
				match(76);
				switch (LA(1))
				{
				case 19:
				case 21:
				case 87:
					expression = callable_expression();
					if (0 == inputState.guessing)
					{
						methodInvocationExpression.Arguments.Add(expression);
					}
					break;
				case 1:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_0_);
			}
			return result;
		}

		protected void slice(SlicingExpression se)
		{
			Expression expression = null;
			Expression expression2 = null;
			Expression step = null;
			try
			{
				switch (LA(1))
				{
				case 87:
					match(87);
					if (0 == inputState.guessing)
					{
						expression = OmittedExpression.Default;
					}
					switch (LA(1))
					{
					case 6:
					case 15:
					case 16:
					case 33:
					case 45:
					case 46:
					case 57:
					case 58:
					case 64:
					case 65:
					case 71:
					case 72:
					case 75:
					case 77:
					case 78:
					case 79:
					case 80:
					case 83:
					case 85:
					case 86:
					case 91:
					case 93:
					case 110:
					case 111:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
						expression2 = this.expression();
						break;
					case 87:
						match(87);
						if (0 == inputState.guessing)
						{
							expression2 = OmittedExpression.Default;
						}
						step = this.expression();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 81:
					case 84:
						break;
					}
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = this.expression();
					switch (LA(1))
					{
					case 87:
						match(87);
						switch (LA(1))
						{
						case 6:
						case 15:
						case 16:
						case 33:
						case 45:
						case 46:
						case 57:
						case 58:
						case 64:
						case 65:
						case 71:
						case 72:
						case 75:
						case 77:
						case 78:
						case 79:
						case 80:
						case 83:
						case 85:
						case 86:
						case 91:
						case 93:
						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
							expression2 = this.expression();
							break;
						case 81:
						case 84:
						case 87:
							if (0 == inputState.guessing)
							{
								expression2 = OmittedExpression.Default;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						switch (LA(1))
						{
						case 87:
							match(87);
							step = this.expression();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 81:
						case 84:
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 81:
					case 84:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					se.Indices.Add(new Slice(expression, expression2, step));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_48_);
					return;
				}
				throw ex;
			}
		}

		protected Expression member_reference_expression(Expression target)
		{
			Expression result = null;
			IToken token = null;
			try
			{
				token = member();
				if (0 == inputState.guessing)
				{
					result = MemberReferenceForToken(target, token);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_122_);
			}
			return result;
		}

		protected void argument(INodeWithArguments node)
		{
			IToken token = null;
			IToken token2 = null;
			Expression expression = null;
			try
			{
				bool flag = false;
				if (LA(1) == 71 && LA(2) == 87)
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						match(71);
						match(87);
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
					token = LT(1);
					match(71);
					token2 = LT(1);
					match(87);
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						node.NamedArguments.Add(new ExpressionPair(SourceLocationFactory.ToLexicalInfo(token2), new ReferenceExpression(SourceLocationFactory.ToLexicalInfo(token), token.getText()), expression));
					}
				}
				else
				{
					if (!tokenSet_4_.member(LA(1)) || !tokenSet_123_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = this.expression();
					if (0 == inputState.guessing && null != expression)
					{
						node.Arguments.Add(expression);
					}
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex2);
					recover(ex2, tokenSet_83_);
					return;
				}
				throw ex2;
			}
		}

		protected void hash_literal_test()
		{
			try
			{
				match(91);
				switch (LA(1))
				{
				case 92:
					match(92);
					break;
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression();
					match(87);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_0_);
					return;
				}
				throw ex;
			}
		}

		protected HashLiteralExpression hash_literal()
		{
			IToken token = null;
			HashLiteralExpression hashLiteralExpression = null;
			ExpressionPair expressionPair = null;
			try
			{
				token = LT(1);
				match(91);
				if (0 == inputState.guessing)
				{
					hashLiteralExpression = new HashLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
				}
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expressionPair = expression_pair();
					if (0 == inputState.guessing)
					{
						hashLiteralExpression.Items.Add(expressionPair);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84 && tokenSet_4_.member(LA(2)))
						{
							match(84);
							expressionPair = expression_pair();
							if (0 == inputState.guessing)
							{
								hashLiteralExpression.Items.Add(expressionPair);
							}
							continue;
						}
						break;
					}
					switch (LA(1))
					{
					case 84:
						match(84);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 92:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 92:
					break;
				}
				match(92);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return hashLiteralExpression;
		}

		protected ListLiteralExpression list_initializer()
		{
			IToken token = null;
			ListLiteralExpression listLiteralExpression = null;
			ExpressionCollection items = null;
			try
			{
				token = LT(1);
				match(91);
				if (0 == inputState.guessing)
				{
					listLiteralExpression = new ListLiteralExpression(ToLexicalInfo(token));
					items = listLiteralExpression.Items;
				}
				list_items(items);
				match(92);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_122_);
			}
			return listLiteralExpression;
		}

		protected void list_items(ExpressionCollection items)
		{
			Expression expression = null;
			try
			{
				switch (LA(1))
				{
				case 6:
				case 15:
				case 16:
				case 33:
				case 45:
				case 46:
				case 57:
				case 58:
				case 64:
				case 65:
				case 71:
				case 72:
				case 75:
				case 77:
				case 78:
				case 79:
				case 80:
				case 83:
				case 85:
				case 86:
				case 91:
				case 93:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						items.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 84 && tokenSet_4_.member(LA(2)))
						{
							match(84);
							expression = this.expression();
							if (0 == inputState.guessing)
							{
								items.Add(expression);
							}
							continue;
						}
						break;
					}
					switch (LA(1))
					{
					case 84:
						match(84);
						break;
					case 81:
					case 92:
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				case 81:
				case 92:
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_124_);
					return;
				}
				throw ex;
			}
		}

		protected Expression string_literal()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			Expression expression = null;
			try
			{
				switch (LA(1))
				{
				case 6:
					expression = expression_interpolation();
					break;
				case 77:
					token = LT(1);
					match(77);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(SourceLocationFactory.ToLexicalInfo(token), token.getText());
						expression.Annotate("quote", "\"");
					}
					break;
				case 78:
					token2 = LT(1);
					match(78);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(SourceLocationFactory.ToLexicalInfo(token2), token2.getText());
						expression.Annotate("quote", "'");
					}
					break;
				case 72:
					token3 = LT(1);
					match(72);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(SourceLocationFactory.ToLexicalInfo(token3), token3.getText());
						expression.Annotate("quote", "\"\"\"");
					}
					break;
				case 115:
					token4 = LT(1);
					match(115);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(ToLexicalInfo(token4), token4.getText());
						expression.Annotate("quote", "`");
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return expression;
		}

		protected ListLiteralExpression list_literal()
		{
			IToken token = null;
			ListLiteralExpression listLiteralExpression = null;
			ExpressionCollection items = null;
			try
			{
				token = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					listLiteralExpression = new ListLiteralExpression(ToLexicalInfo(token));
					items = listLiteralExpression.Items;
				}
				list_items(items);
				match(81);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return listLiteralExpression;
		}

		protected RELiteralExpression re_literal()
		{
			IToken token = null;
			RELiteralExpression result = null;
			try
			{
				token = LT(1);
				match(116);
				if (0 == inputState.guessing)
				{
					result = new RELiteralExpression(SourceLocationFactory.ToLexicalInfo(token), token.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected BoolLiteralExpression bool_literal()
		{
			IToken token = null;
			IToken token2 = null;
			BoolLiteralExpression boolLiteralExpression = null;
			try
			{
				switch (LA(1))
				{
				case 64:
					token = LT(1);
					match(64);
					if (0 == inputState.guessing)
					{
						boolLiteralExpression = new BoolLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
						boolLiteralExpression.Value = true;
					}
					break;
				case 33:
					token2 = LT(1);
					match(33);
					if (0 == inputState.guessing)
					{
						boolLiteralExpression = new BoolLiteralExpression(SourceLocationFactory.ToLexicalInfo(token2));
						boolLiteralExpression.Value = false;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return boolLiteralExpression;
		}

		protected NullLiteralExpression null_literal()
		{
			IToken token = null;
			NullLiteralExpression result = null;
			try
			{
				token = LT(1);
				match(46);
				if (0 == inputState.guessing)
				{
					result = new NullLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected SelfLiteralExpression self_literal()
		{
			IToken token = null;
			SelfLiteralExpression result = null;
			try
			{
				token = LT(1);
				match(57);
				if (0 == inputState.guessing)
				{
					result = new SelfLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected SuperLiteralExpression super_literal()
		{
			IToken token = null;
			SuperLiteralExpression result = null;
			try
			{
				token = LT(1);
				match(58);
				if (0 == inputState.guessing)
				{
					result = new SuperLiteralExpression(SourceLocationFactory.ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected DoubleLiteralExpression double_literal()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			DoubleLiteralExpression result = null;
			try
			{
				switch (LA(1))
				{
				case 83:
				case 117:
					switch (LA(1))
					{
					case 83:
						token = LT(1);
						match(83);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 117:
						break;
					}
					token2 = LT(1);
					match(117);
					if (0 == inputState.guessing)
					{
						string text = token2.getText();
						if (token != null)
						{
							text = token.getText() + text;
						}
						result = new DoubleLiteralExpression(SourceLocationFactory.ToLexicalInfo(token2), PrimitiveParser.ParseDouble(token2, text));
					}
					break;
				case 118:
					token3 = LT(1);
					match(118);
					if (0 == inputState.guessing)
					{
						string text = token3.getText();
						text = text.Substring(0, text.Length - 1);
						if (token != null)
						{
							text = token.getText() + text;
						}
						result = new DoubleLiteralExpression(SourceLocationFactory.ToLexicalInfo(token3), PrimitiveParser.ParseDouble(token3, text, isSingle: true), isSingle: true);
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected TimeSpanLiteralExpression timespan_literal()
		{
			IToken token = null;
			IToken token2 = null;
			TimeSpanLiteralExpression result = null;
			try
			{
				switch (LA(1))
				{
				case 83:
					token = LT(1);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 119:
					break;
				}
				token2 = LT(1);
				match(119);
				if (0 == inputState.guessing)
				{
					string text = token2.getText();
					if (token != null)
					{
						text = token.getText() + text;
					}
					result = new TimeSpanLiteralExpression(SourceLocationFactory.ToLexicalInfo(token2), PrimitiveParser.ParseTimeSpan(token2, text));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return result;
		}

		protected ExpressionInterpolationExpression expression_interpolation()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			ExpressionInterpolationExpression expressionInterpolationExpression = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(6);
				if (0 == inputState.guessing)
				{
					LexicalInfo lexicalInfoProvider = SourceLocationFactory.ToLexicalInfo(token);
					expressionInterpolationExpression = new ExpressionInterpolationExpression(lexicalInfoProvider);
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) != 6 || !tokenSet_4_.member(LA(2)))
					{
						break;
					}
					match(6);
					expression = this.expression();
					switch (LA(1))
					{
					case 71:
					case 87:
						switch (LA(1))
						{
						case 87:
							token2 = LT(1);
							match(87);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 71:
							break;
						}
						token3 = LT(1);
						match(71);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 6:
						break;
					}
					if (0 == inputState.guessing && null != expression)
					{
						expressionInterpolationExpression.Expressions.Add(expression);
						if (null != token3)
						{
							expression.Annotate("formatString", token3.getText());
						}
					}
					match(6);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_38_);
			}
			return expressionInterpolationExpression;
		}

		protected ExpressionPair expression_pair()
		{
			IToken token = null;
			ExpressionPair result = null;
			Expression expression = null;
			Expression expression2 = null;
			try
			{
				expression = this.expression();
				token = LT(1);
				match(87);
				expression2 = this.expression();
				if (0 == inputState.guessing)
				{
					result = new ExpressionPair(SourceLocationFactory.ToLexicalInfo(token), expression, expression2);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_125_);
			}
			return result;
		}

		private void initializeFactory()
		{
		}

		private static long[] mk_tokenSet_0_()
		{
			return new long[3] { 2L, 0L, 0L };
		}

		private static long[] mk_tokenSet_1_()
		{
			return new long[4] { -2396378464097930302L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_2_()
		{
			return new long[4] { -2305843010073526334L, 72057594037927935L, 0L, 0L };
		}

		private static long[] mk_tokenSet_3_()
		{
			return new long[4] { 432453324957122626L, 71987225980170151L, 0L, 0L };
		}

		private static long[] mk_tokenSet_4_()
		{
			return new long[4] { 432451125933867072L, 71987225971779971L, 0L, 0L };
		}

		private static long[] mk_tokenSet_5_()
		{
			return new long[4] { -7485527524998757888L, 65560L, 0L, 0L };
		}

		private static long[] mk_tokenSet_6_()
		{
			return new long[4] { -7395455515135950336L, 131224L, 0L, 0L };
		}

		private static long[] mk_tokenSet_7_()
		{
			return new long[4] { 2L, 1073741824L, 0L, 0L };
		}

		private static long[] mk_tokenSet_8_()
		{
			return new long[4] { -146784805717054L, 71987227323396095L, 0L, 0L };
		}

		private static long[] mk_tokenSet_9_()
		{
			return new long[4] { -18196367687289918L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_10_()
		{
			return new long[4] { -62L, 72057594037927935L, 0L, 0L };
		}

		private static long[] mk_tokenSet_11_()
		{
			return new long[4] { -2396387260190952510L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_12_()
		{
			return new long[4] { 2199025877058L, 1351751332L, 0L, 0L };
		}

		private static long[] mk_tokenSet_13_()
		{
			return new long[4] { -2396387331057912894L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_14_()
		{
			return new long[4] { -2396387331041135678L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_15_()
		{
			return new long[4] { 5089140193940844994L, 71987227053912039L, 0L, 0L };
		}

		private static long[] mk_tokenSet_16_()
		{
			return new long[4] { 5089140193940844994L, 71987227053910503L, 0L, 0L };
		}

		private static long[] mk_tokenSet_17_()
		{
			return new long[4] { -2305843010074837054L, 72057594037927935L, 0L, 0L };
		}

		private static long[] mk_tokenSet_18_()
		{
			return new long[4] { 5089140193940844608L, 71987225980168679L, 0L, 0L };
		}

		private static long[] mk_tokenSet_19_()
		{
			return new long[4] { 386L, 1073741824L, 0L, 0L };
		}

		private static long[] mk_tokenSet_20_()
		{
			return new long[4] { 2L, 1536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_21_()
		{
			return new long[4] { -2396387331057913278L, 71987225980170239L, 0L, 0L };
		}

		private static long[] mk_tokenSet_22_()
		{
			return new long[4] { 2199023255554L, 1572L, 0L, 0L };
		}

		private static long[] mk_tokenSet_23_()
		{
			return new long[4] { -90262845999287358L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_24_()
		{
			return new long[4] { 432451125933867074L, 71987225971781507L, 0L, 0L };
		}

		private static long[] mk_tokenSet_25_()
		{
			return new long[4] { -2305843010085322814L, 72057594021146623L, 0L, 0L };
		}

		private static long[] mk_tokenSet_26_()
		{
			return new long[4] { 2147485698L, 1536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_27_()
		{
			return new long[4] { 70866962434L, 3584L, 0L, 0L };
		}

		private static long[] mk_tokenSet_28_()
		{
			return new long[4] { 2199023255554L, 276829732L, 0L, 0L };
		}

		private static long[] mk_tokenSet_29_()
		{
			return new long[4] { 93449984459931648L, 128L, 0L, 0L };
		}

		private static long[] mk_tokenSet_30_()
		{
			return new long[4] { 465718347336770L, 492580536032932L, 0L, 0L };
		}

		private static long[] mk_tokenSet_31_()
		{
			return new long[4] { -7251340327061403136L, 8421528L, 0L, 0L };
		}

		private static long[] mk_tokenSet_32_()
		{
			return new long[4] { -8638449167112338944L, 24L, 0L, 0L };
		}

		private static long[] mk_tokenSet_33_()
		{
			return new long[4] { 1369094441541451776L, 128L, 0L, 0L };
		}

		private static long[] mk_tokenSet_34_()
		{
			return new long[4] { -2396387330906917950L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_35_()
		{
			return new long[4] { 144115188210597888L, 65664L, 0L, 0L };
		}

		private static long[] mk_tokenSet_36_()
		{
			return new long[4] { 0L, 131072L, 0L, 0L };
		}

		private static long[] mk_tokenSet_37_()
		{
			return new long[4] { 0L, 67244032L, 0L, 0L };
		}

		private static long[] mk_tokenSet_38_()
		{
			return new long[4] { 465647480376386L, 492580536032932L, 0L, 0L };
		}

		private static long[] mk_tokenSet_39_()
		{
			return new long[4] { 140737488437248L, 2132096L, 0L, 0L };
		}

		private static long[] mk_tokenSet_40_()
		{
			return new long[4] { 81920L, 2099328L, 0L, 0L };
		}

		private static long[] mk_tokenSet_41_()
		{
			return new long[4] { -8796094070846L, 72057594037927935L, 0L, 0L };
		}

		private static long[] mk_tokenSet_42_()
		{
			return new long[4] { -2324329719689121214L, 71987225980170239L, 0L, 0L };
		}

		private static long[] mk_tokenSet_43_()
		{
			return new long[4] { -2306124485041849406L, 72057594021146623L, 0L, 0L };
		}

		private static long[] mk_tokenSet_44_()
		{
			return new long[4] { 2L, 1196268651021824L, 0L, 0L };
		}

		private static long[] mk_tokenSet_45_()
		{
			return new long[4] { 16777216L, 2162816L, 0L, 0L };
		}

		private static long[] mk_tokenSet_46_()
		{
			return new long[4] { -7341412336771907072L, 2162840L, 0L, 0L };
		}

		private static long[] mk_tokenSet_47_()
		{
			return new long[4] { -18205234647272510L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_48_()
		{
			return new long[4] { 0L, 1179648L, 0L, 0L };
		}

		private static long[] mk_tokenSet_49_()
		{
			return new long[4] { 0L, 4096L, 0L, 0L };
		}

		private static long[] mk_tokenSet_50_()
		{
			return new long[4] { 0L, 8388608L, 0L, 0L };
		}

		private static long[] mk_tokenSet_51_()
		{
			return new long[4] { -7341412336771907072L, 1075904664L, 0L, 0L };
		}

		private static long[] mk_tokenSet_52_()
		{
			return new long[4] { 2048L, 12650496L, 0L, 0L };
		}

		private static long[] mk_tokenSet_53_()
		{
			return new long[4] { 2048L, 8456192L, 0L, 0L };
		}

		private static long[] mk_tokenSet_54_()
		{
			return new long[4] { -8566391555894541824L, 65560L, 0L, 0L };
		}

		private static long[] mk_tokenSet_55_()
		{
			return new long[4] { 2050L, 263680L, 0L, 0L };
		}

		private static long[] mk_tokenSet_56_()
		{
			return new long[4] { 144115188227375104L, 65664L, 0L, 0L };
		}

		private static long[] mk_tokenSet_57_()
		{
			return new long[4] { 1152921504607322112L, 2099328L, 0L, 0L };
		}

		private static long[] mk_tokenSet_58_()
		{
			return new long[4] { 18014398509481984L, 8493184L, 0L, 0L };
		}

		private static long[] mk_tokenSet_59_()
		{
			return new long[4] { 0L, 1181696L, 0L, 0L };
		}

		private static long[] mk_tokenSet_60_()
		{
			return new long[4] { 216172799445172226L, 67200L, 0L, 0L };
		}

		private static long[] mk_tokenSet_61_()
		{
			return new long[4] { 72057611234574336L, 65536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_62_()
		{
			return new long[4] { 7395264678999470146L, 71987225980170215L, 0L, 0L };
		}

		private static long[] mk_tokenSet_63_()
		{
			return new long[4] { 237565172535787520L, 128L, 0L, 0L };
		}

		private static long[] mk_tokenSet_64_()
		{
			return new long[4] { 5089140193957621826L, 71987225980170215L, 0L, 0L };
		}

		private static long[] mk_tokenSet_65_()
		{
			return new long[4] { -2306124485043160126L, 72057594021146623L, 0L, 0L };
		}

		private static long[] mk_tokenSet_66_()
		{
			return new long[3] { 2306124485058625536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_67_()
		{
			return new long[4] { -8566391555877764608L, 65560L, 0L, 0L };
		}

		private static long[] mk_tokenSet_68_()
		{
			return new long[4] { 432415941561778240L, 71002063553259907L, 0L, 0L };
		}

		private static long[] mk_tokenSet_69_()
		{
			return new long[4] { -2306130532373892158L, 71987227390636031L, 0L, 0L };
		}

		private static long[] mk_tokenSet_70_()
		{
			return new long[4] { 432451125933867072L, 71987225972828547L, 0L, 0L };
		}

		private static long[] mk_tokenSet_71_()
		{
			return new long[4] { -90262845865069630L, 71987227053910527L, 0L, 0L };
		}

		private static long[] mk_tokenSet_72_()
		{
			return new long[4] { 432451125933867072L, 71987226240215427L, 0L, 0L };
		}

		private static long[] mk_tokenSet_73_()
		{
			return new long[4] { 324909992021058L, 492580514993828L, 0L, 0L };
		}

		private static long[] mk_tokenSet_74_()
		{
			return new long[4] { 2199025876994L, 276829732L, 0L, 0L };
		}

		private static long[] mk_tokenSet_75_()
		{
			return new long[4] { -18205234647272510L, 71987227053910527L, 0L, 0L };
		}

		private static long[] mk_tokenSet_76_()
		{
			return new long[4] { 432415941561778240L, 71987225971779971L, 0L, 0L };
		}

		private static long[] mk_tokenSet_77_()
		{
			return new long[4] { -2306124485062034494L, 72057594021146623L, 0L, 0L };
		}

		private static long[] mk_tokenSet_78_()
		{
			return new long[4] { -2306130532373892158L, 71987227390898175L, 0L, 0L };
		}

		private static long[] mk_tokenSet_79_()
		{
			return new long[4] { 441423175176269888L, 71987225980168643L, 0L, 0L };
		}

		private static long[] mk_tokenSet_80_()
		{
			return new long[4] { -90262845999287358L, 71987227053910527L, 0L, 0L };
		}

		private static long[] mk_tokenSet_81_()
		{
			return new long[4] { -7269354725570887168L, 152L, 0L, 0L };
		}

		private static long[] mk_tokenSet_82_()
		{
			return new long[4] { 0L, 68292608L, 0L, 0L };
		}

		private static long[] mk_tokenSet_83_()
		{
			return new long[4] { 0L, 1052672L, 0L, 0L };
		}

		private static long[] mk_tokenSet_84_()
		{
			return new long[4] { 2199023255554L, 268437028L, 0L, 0L };
		}

		private static long[] mk_tokenSet_85_()
		{
			return new long[4] { -2306124485059937342L, 72057594021146623L, 0L, 0L };
		}

		private static long[] mk_tokenSet_86_()
		{
			return new long[3] { 16777216L, 0L, 0L };
		}

		private static long[] mk_tokenSet_87_()
		{
			return new long[4] { -2396387331041136064L, 71987225980168703L, 0L, 0L };
		}

		private static long[] mk_tokenSet_88_()
		{
			return new long[4] { 2203320844354L, 1351751332L, 0L, 0L };
		}

		private static long[] mk_tokenSet_89_()
		{
			return new long[4] { 4398046511104L, 1310720L, 0L, 0L };
		}

		private static long[] mk_tokenSet_90_()
		{
			return new long[4] { 2203329232962L, 1351751332L, 0L, 0L };
		}

		private static long[] mk_tokenSet_91_()
		{
			return new long[4] { 432453324957122626L, 71987226240216999L, 0L, 0L };
		}

		private static long[] mk_tokenSet_92_()
		{
			return new long[4] { 2L, 268436992L, 0L, 0L };
		}

		private static long[] mk_tokenSet_93_()
		{
			return new long[4] { 18014398509481984L, 67207296L, 0L, 0L };
		}

		private static long[] mk_tokenSet_94_()
		{
			return new long[4] { -8745884914647201728L, 71987226040068547L, 0L, 0L };
		}

		private static long[] mk_tokenSet_95_()
		{
			return new long[4] { 477487122207572032L, 71987225972828611L, 0L, 0L };
		}

		private static long[] mk_tokenSet_96_()
		{
			return new long[3] { 855638016L, 0L, 0L };
		}

		private static long[] mk_tokenSet_97_()
		{
			return new long[4] { 283678305944642L, 1351751332L, 0L, 0L };
		}

		private static long[] mk_tokenSet_98_()
		{
			return new long[4] { 4398046511104L, 262144L, 0L, 0L };
		}

		private static long[] mk_tokenSet_99_()
		{
			return new long[4] { -90080805821612094L, 72057594037927935L, 0L, 0L };
		}

		private static long[] mk_tokenSet_100_()
		{
			return new long[4] { 283678305943618L, 1351751332L, 0L, 0L };
		}

		private static long[] mk_tokenSet_101_()
		{
			return new long[4] { -2396378463963712574L, 71987227053912063L, 0L, 0L };
		}

		private static long[] mk_tokenSet_102_()
		{
			return new long[4] { -7341412336788684288L, 65688L, 0L, 0L };
		}

		private static long[] mk_tokenSet_103_()
		{
			return new long[4] { -7251340327060092414L, 13045400L, 0L, 0L };
		}

		private static long[] mk_tokenSet_104_()
		{
			return new long[4] { -7485527524864540160L, 152L, 0L, 0L };
		}

		private static long[] mk_tokenSet_105_()
		{
			return new long[4] { 0L, 1073741824L, 0L, 0L };
		}

		private static long[] mk_tokenSet_106_()
		{
			return new long[4] { 41231686041600L, 481036337152L, 0L, 0L };
		}

		private static long[] mk_tokenSet_107_()
		{
			return new long[4] { 283678305944642L, 67924006564L, 0L, 0L };
		}

		private static long[] mk_tokenSet_108_()
		{
			return new long[4] { 0L, 1649335074816L, 0L, 0L };
		}

		private static long[] mk_tokenSet_109_()
		{
			return new long[4] { 324909991986242L, 548960343716L, 0L, 0L };
		}

		private static long[] mk_tokenSet_110_()
		{
			return new long[4] { 0L, 15393162821632L, 0L, 0L };
		}

		private static long[] mk_tokenSet_111_()
		{
			return new long[4] { 324909991986242L, 2198295418532L, 0L, 0L };
		}

		private static long[] mk_tokenSet_112_()
		{
			return new long[4] { 324909991986242L, 17591458240164L, 0L, 0L };
		}

		private static long[] mk_tokenSet_113_()
		{
			return new long[4] { 324909991986242L, 70368049927844L, 0L, 0L };
		}

		private static long[] mk_tokenSet_114_()
		{
			return new long[4] { 324909992021058L, 1266636700948132L, 0L, 0L };
		}

		private static long[] mk_tokenSet_115_()
		{
			return new long[4] { 0L, 985162419044352L, 0L, 0L };
		}

		private static long[] mk_tokenSet_116_()
		{
			return new long[4] { -2305843010074837054L, 72057594021150719L, 0L, 0L };
		}

		private static long[] mk_tokenSet_117_()
		{
			return new long[4] { 324909992021058L, 70368049927844L, 0L, 0L };
		}

		private static long[] mk_tokenSet_118_()
		{
			return new long[4] { 465647480376386L, 1688849187053220L, 0L, 0L };
		}

		private static long[] mk_tokenSet_119_()
		{
			return new long[4] { 495501520717054016L, 71987226039937475L, 0L, 0L };
		}

		private static long[] mk_tokenSet_120_()
		{
			return new long[4] { 465647480376386L, 9499779790773924L, 0L, 0L };
		}

		private static long[] mk_tokenSet_121_()
		{
			return new long[4] { 465647480376386L, 36521377554996900L, 0L, 0L };
		}

		private static long[] mk_tokenSet_122_()
		{
			return new long[4] { 465647480376386L, 492580519255716L, 0L, 0L };
		}

		private static long[] mk_tokenSet_123_()
		{
			return new long[4] { -2305843010085322814L, 72057594021150719L, 0L, 0L };
		}

		private static long[] mk_tokenSet_124_()
		{
			return new long[4] { 0L, 268566528L, 0L, 0L };
		}

		private static long[] mk_tokenSet_125_()
		{
			return new long[4] { 0L, 269484032L, 0L, 0L };
		}
	}
}
