using System.Text;
using antlr;
using antlr.collections.impl;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Parser
{
	public abstract class BooParserBase : LLkParser
	{
		public const int EOF = 1;

		public const int NULL_TREE_LOOKAHEAD = 3;

		public const int INDENT = 4;

		public const int DEDENT = 5;

		public const int ELIST = 6;

		public const int DLIST = 7;

		public const int ESEPARATOR = 8;

		public const int EOL = 9;

		public const int ASSEMBLY_ATTRIBUTE_BEGIN = 10;

		public const int MODULE_ATTRIBUTE_BEGIN = 11;

		public const int ABSTRACT = 12;

		public const int AND = 13;

		public const int AS = 14;

		public const int BREAK = 15;

		public const int CONTINUE = 16;

		public const int CALLABLE = 17;

		public const int CAST = 18;

		public const int CHAR = 19;

		public const int CLASS = 20;

		public const int CONSTRUCTOR = 21;

		public const int DEF = 22;

		public const int DESTRUCTOR = 23;

		public const int DO = 24;

		public const int ELIF = 25;

		public const int ELSE = 26;

		public const int ENSURE = 27;

		public const int ENUM = 28;

		public const int EVENT = 29;

		public const int EXCEPT = 30;

		public const int FAILURE = 31;

		public const int FINAL = 32;

		public const int FROM = 33;

		public const int FOR = 34;

		public const int FALSE = 35;

		public const int GET = 36;

		public const int GOTO = 37;

		public const int IMPORT = 38;

		public const int INTERFACE = 39;

		public const int INTERNAL = 40;

		public const int IS = 41;

		public const int ISA = 42;

		public const int IF = 43;

		public const int IN = 44;

		public const int NAMESPACE = 45;

		public const int NEW = 46;

		public const int NOT = 47;

		public const int NULL = 48;

		public const int OF = 49;

		public const int OR = 50;

		public const int OVERRIDE = 51;

		public const int PASS = 52;

		public const int PARTIAL = 53;

		public const int PUBLIC = 54;

		public const int PROTECTED = 55;

		public const int PRIVATE = 56;

		public const int RAISE = 57;

		public const int REF = 58;

		public const int RETURN = 59;

		public const int SET = 60;

		public const int SELF = 61;

		public const int SUPER = 62;

		public const int STATIC = 63;

		public const int STRUCT = 64;

		public const int THEN = 65;

		public const int TRY = 66;

		public const int TRANSIENT = 67;

		public const int TRUE = 68;

		public const int TYPEOF = 69;

		public const int UNLESS = 70;

		public const int VIRTUAL = 71;

		public const int WHILE = 72;

		public const int YIELD = 73;

		public const int TRIPLE_QUOTED_STRING = 74;

		public const int EOS = 75;

		public const int LPAREN = 76;

		public const int RPAREN = 77;

		public const int DOUBLE_QUOTED_STRING = 78;

		public const int SINGLE_QUOTED_STRING = 79;

		public const int ID = 80;

		public const int MULTIPLY = 81;

		public const int LBRACK = 82;

		public const int RBRACK = 83;

		public const int ASSIGN = 84;

		public const int COMMA = 85;

		public const int SPLICE_BEGIN = 86;

		public const int DOT = 87;

		public const int COLON = 88;

		public const int NULLABLE_SUFFIX = 89;

		public const int EXPONENTIATION = 90;

		public const int BITWISE_OR = 91;

		public const int LBRACE = 92;

		public const int RBRACE = 93;

		public const int QQ_BEGIN = 94;

		public const int QQ_END = 95;

		public const int INPLACE_BITWISE_OR = 96;

		public const int INPLACE_EXCLUSIVE_OR = 97;

		public const int INPLACE_BITWISE_AND = 98;

		public const int INPLACE_SHIFT_LEFT = 99;

		public const int INPLACE_SHIFT_RIGHT = 100;

		public const int CMP_OPERATOR = 101;

		public const int GREATER_THAN = 102;

		public const int LESS_THAN = 103;

		public const int ADD = 104;

		public const int SUBTRACT = 105;

		public const int EXCLUSIVE_OR = 106;

		public const int DIVISION = 107;

		public const int MODULUS = 108;

		public const int BITWISE_AND = 109;

		public const int SHIFT_LEFT = 110;

		public const int SHIFT_RIGHT = 111;

		public const int LONG = 112;

		public const int INCREMENT = 113;

		public const int DECREMENT = 114;

		public const int ONES_COMPLEMENT = 115;

		public const int INT = 116;

		public const int BACKTICK_QUOTED_STRING = 117;

		public const int RE_LITERAL = 118;

		public const int DOUBLE = 119;

		public const int FLOAT = 120;

		public const int TIMESPAN = 121;

		public const int ID_SUFFIX = 122;

		public const int LINE_CONTINUATION = 123;

		public const int INTERPOLATED_EXPRESSION = 124;

		public const int INTERPOLATED_REFERENCE = 125;

		public const int SL_COMMENT = 126;

		public const int ML_COMMENT = 127;

		public const int WS = 128;

		public const int X_RE_LITERAL = 129;

		public const int NEWLINE = 130;

		public const int DQS_ESC = 131;

		public const int SQS_ESC = 132;

		public const int SESC = 133;

		public const int RE_CHAR = 134;

		public const int X_RE_CHAR = 135;

		public const int RE_OPTIONS = 136;

		public const int RE_ESC = 137;

		public const int DIGIT_GROUP = 138;

		public const int REVERSE_DIGIT_GROUP = 139;

		public const int AT_SYMBOL = 140;

		public const int ID_LETTER = 141;

		public const int DIGIT = 142;

		public const int HEXDIGIT = 143;

		protected StringBuilder _sbuilder = new StringBuilder();

		protected AttributeCollection _attributes = new AttributeCollection();

		protected TypeMemberModifiers _modifiers = TypeMemberModifiers.None;

		protected bool _inArray;

		protected bool _compact = false;

		public static readonly string[] tokenNames_ = new string[144]
		{
			"\"<0>\"", "\"EOF\"", "\"<2>\"", "\"NULL_TREE_LOOKAHEAD\"", "\"INDENT\"", "\"DEDENT\"", "\"ELIST\"", "\"DLIST\"", "\"ESEPARATOR\"", "\"EOL\"",
			"\"ASSEMBLY_ATTRIBUTE_BEGIN\"", "\"MODULE_ATTRIBUTE_BEGIN\"", "\"abstract\"", "\"and\"", "\"as\"", "\"break\"", "\"continue\"", "\"callable\"", "\"cast\"", "\"char\"",
			"\"class\"", "\"constructor\"", "\"def\"", "\"destructor\"", "\"do\"", "\"elif\"", "\"else\"", "\"ensure\"", "\"enum\"", "\"event\"",
			"\"except\"", "\"failure\"", "\"final\"", "\"from\"", "\"for\"", "\"false\"", "\"get\"", "\"goto\"", "\"import\"", "\"interface\"",
			"\"internal\"", "\"is\"", "\"isa\"", "\"if\"", "\"in\"", "\"namespace\"", "\"new\"", "\"not\"", "\"null\"", "\"of\"",
			"\"or\"", "\"override\"", "\"pass\"", "\"partial\"", "\"public\"", "\"protected\"", "\"private\"", "\"raise\"", "\"ref\"", "\"return\"",
			"\"set\"", "\"self\"", "\"super\"", "\"static\"", "\"struct\"", "\"then\"", "\"try\"", "\"transient\"", "\"true\"", "\"typeof\"",
			"\"unless\"", "\"virtual\"", "\"while\"", "\"yield\"", "\"TRIPLE_QUOTED_STRING\"", "\"EOS\"", "\"LPAREN\"", "\"RPAREN\"", "\"DOUBLE_QUOTED_STRING\"", "\"SINGLE_QUOTED_STRING\"",
			"\"ID\"", "\"MULTIPLY\"", "\"LBRACK\"", "\"RBRACK\"", "\"ASSIGN\"", "\"COMMA\"", "\"SPLICE_BEGIN\"", "\"DOT\"", "\"COLON\"", "\"NULLABLE_SUFFIX\"",
			"\"EXPONENTIATION\"", "\"BITWISE_OR\"", "\"LBRACE\"", "\"RBRACE\"", "\"QQ_BEGIN\"", "\"QQ_END\"", "\"INPLACE_BITWISE_OR\"", "\"INPLACE_EXCLUSIVE_OR\"", "\"INPLACE_BITWISE_AND\"", "\"INPLACE_SHIFT_LEFT\"",
			"\"INPLACE_SHIFT_RIGHT\"", "\"CMP_OPERATOR\"", "\"GREATER_THAN\"", "\"LESS_THAN\"", "\"ADD\"", "\"SUBTRACT\"", "\"EXCLUSIVE_OR\"", "\"DIVISION\"", "\"MODULUS\"", "\"BITWISE_AND\"",
			"\"SHIFT_LEFT\"", "\"SHIFT_RIGHT\"", "\"LONG\"", "\"INCREMENT\"", "\"DECREMENT\"", "\"ONES_COMPLEMENT\"", "\"INT\"", "\"BACKTICK_QUOTED_STRING\"", "\"RE_LITERAL\"", "\"DOUBLE\"",
			"\"FLOAT\"", "\"TIMESPAN\"", "\"ID_SUFFIX\"", "\"LINE_CONTINUATION\"", "\"INTERPOLATED_EXPRESSION\"", "\"INTERPOLATED_REFERENCE\"", "\"SL_COMMENT\"", "\"ML_COMMENT\"", "\"WS\"", "\"X_RE_LITERAL\"",
			"\"NEWLINE\"", "\"DQS_ESC\"", "\"SQS_ESC\"", "\"SESC\"", "\"RE_CHAR\"", "\"X_RE_CHAR\"", "\"RE_OPTIONS\"", "\"RE_ESC\"", "\"DIGIT_GROUP\"", "\"REVERSE_DIGIT_GROUP\"",
			"\"AT_SYMBOL\"", "\"ID_LETTER\"", "\"DIGIT\"", "\"HEXDIGIT\""
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

		public static readonly BitSet tokenSet_126_ = new BitSet(mk_tokenSet_126_());

		public static readonly BitSet tokenSet_127_ = new BitSet(mk_tokenSet_127_());

		public static readonly BitSet tokenSet_128_ = new BitSet(mk_tokenSet_128_());

		protected void ResetMemberData()
		{
			_modifiers = TypeMemberModifiers.None;
		}

		protected void AddAttributes(AttributeCollection target)
		{
			target?.Extend(_attributes);
			_attributes.Clear();
		}

		private static bool IsMethodInvocationExpression(Expression e)
		{
			return NodeType.MethodInvocationExpression == e.NodeType;
		}

		protected bool IsValidMacroArgument(int token)
		{
			return 76 != token && 82 != token && 87 != token && 81 != token;
		}

		protected bool IsValidClosureMacroArgument(int token)
		{
			if (!IsValidMacroArgument(token))
			{
				return false;
			}
			return 105 != token;
		}

		private LexicalInfo ToLexicalInfo(IToken token)
		{
			return SourceLocationFactory.ToLexicalInfo(token);
		}

		private void SetEndSourceLocation(Node node, IToken token)
		{
			node.EndSourceLocation = SourceLocationFactory.ToSourceLocation(token);
		}

		private MemberReferenceExpression MemberReferenceForToken(Expression target, IToken memberName)
		{
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(ToLexicalInfo(memberName));
			memberReferenceExpression.Target = target;
			memberReferenceExpression.Name = memberName.getText();
			return memberReferenceExpression;
		}

		protected abstract void EmitIndexedPropertyDeprecationWarning(Property deprecated);

		protected abstract void EmitTransientKeywordDeprecationWarning(LexicalInfo location);

		protected void initialize()
		{
			tokenNames = tokenNames_;
		}

		protected BooParserBase(TokenBuffer tokenBuf, int k)
			: base(tokenBuf, k)
		{
			initialize();
		}

		public BooParserBase(TokenBuffer tokenBuf)
			: this(tokenBuf, 2)
		{
		}

		protected BooParserBase(TokenStream lexer, int k)
			: base(lexer, k)
		{
			initialize();
		}

		public BooParserBase(TokenStream lexer)
			: this(lexer, 2)
		{
		}

		public BooParserBase(ParserSharedInputState state)
			: base(state, 2)
		{
			initialize();
		}

		protected Module start(CompileUnit cu)
		{
			IToken token = null;
			Module module = new Module();
			module.LexicalInfo = new LexicalInfo(getFilename(), 1, 1);
			cu.Modules.Add(module);
			try
			{
				parse_module(module);
				token = LT(1);
				match(1);
				if (0 == inputState.guessing)
				{
					SetEndSourceLocation(module, token);
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
				if ((LA(1) == 9 || LA(1) == 75) && tokenSet_1_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_1_.member(LA(1)) || !tokenSet_2_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				docstring(module);
				if ((LA(1) == 9 || LA(1) == 75) && tokenSet_1_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_1_.member(LA(1)) || !tokenSet_3_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				switch (LA(1))
				{
				case 45:
					namespace_directive(module);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 5:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 32:
				case 33:
				case 34:
				case 35:
				case 37:
				case 38:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 33 || LA(1) == 38)
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
					if ((LA(1) == 65 || LA(1) == 80) && tokenSet_4_.member(LA(2)) && IsValidMacroArgument(LA(2)))
					{
						int pos = mark();
						flag2 = true;
						inputState.guessing++;
						try
						{
							macro_name();
							if (tokenSet_5_.member(LA(1)))
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
					if (tokenSet_6_.member(LA(1)) && tokenSet_7_.member(LA(2)))
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
					if (LA(1) == 10 || LA(1) == 11)
					{
						switch (LA(1))
						{
						case 10:
							assembly_attribute(module);
							break;
						case 11:
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
					recover(ex2, tokenSet_8_);
					return;
				}
				throw ex2;
			}
		}

		protected void eos()
		{
			try
			{
				int num = 0;
				while (true)
				{
					bool flag = true;
					if ((LA(1) != 9 && LA(1) != 75) || !tokenSet_9_.member(LA(2)))
					{
						break;
					}
					switch (LA(1))
					{
					case 9:
						match(9);
						break;
					case 75:
						match(75);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
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
					recover(ex, tokenSet_9_);
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
				if (LA(1) == 74 && tokenSet_10_.member(LA(2)))
				{
					token = LT(1);
					match(74);
					if (0 == inputState.guessing)
					{
						node.Documentation = DocStringFormatter.Format(token.getText());
					}
					if ((LA(1) == 9 || LA(1) == 75) && tokenSet_10_.member(LA(2)))
					{
						eos();
					}
					else if (!tokenSet_10_.member(LA(1)) || !tokenSet_11_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
				}
				else if (!tokenSet_10_.member(LA(1)) || !tokenSet_11_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_10_);
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
				match(45);
				IToken token2 = identifier();
				if (0 == inputState.guessing)
				{
					namespaceDeclaration = new NamespaceDeclaration(ToLexicalInfo(token));
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
					recover(ex, tokenSet_12_);
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
				import = LA(1) switch
				{
					38 => import_directive_(), 
					33 => import_directive_from_(), 
					_ => throw new NoViableAltException(LT(1), getFilename()), 
				};
				if (0 == inputState.guessing && import != null)
				{
					container.Imports.Add(import);
				}
				eos();
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_12_);
					return;
				}
				throw ex;
			}
		}

		protected IToken macro_name()
		{
			IToken token = null;
			IToken token2 = null;
			IToken result = null;
			try
			{
				switch (LA(1))
				{
				case 80:
					token = LT(1);
					match(80);
					if (0 == inputState.guessing)
					{
						result = token;
					}
					break;
				case 65:
					token2 = LT(1);
					match(65);
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
				recover(ex, tokenSet_13_);
			}
			return result;
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
				if (LA(1) == 34 && LA(2) == 80)
				{
					token = LT(1);
					match(34);
					if (0 == inputState.guessing)
					{
						generatorExpression = new GeneratorExpression(ToLexicalInfo(token));
						generatorExpression.Expression = result;
						result = generatorExpression;
					}
					generator_expression_body(generatorExpression);
					while (true)
					{
						bool flag = true;
						if (LA(1) != 34 || LA(2) != 80)
						{
							break;
						}
						token2 = LT(1);
						match(34);
						if (0 == inputState.guessing)
						{
							if (null == extendedGeneratorExpression)
							{
								extendedGeneratorExpression = new ExtendedGeneratorExpression(ToLexicalInfo(token));
								extendedGeneratorExpression.Items.Add(generatorExpression);
								result = extendedGeneratorExpression;
							}
							generatorExpression = new GeneratorExpression(ToLexicalInfo(token2));
							extendedGeneratorExpression.Items.Add(generatorExpression);
						}
						generator_expression_body(generatorExpression);
					}
				}
				else if (!tokenSet_14_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
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
				recover(ex, tokenSet_14_);
			}
			return result;
		}

		protected void module_macro(Module module)
		{
			MacroStatement macroStatement = null;
			try
			{
				macroStatement = macro_stmt();
				if (0 == inputState.guessing)
				{
					module.Globals.Add(macroStatement);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_16_);
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
				case 17:
				case 20:
				case 28:
				case 39:
				case 64:
					type_definition(container);
					break;
				case 22:
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
					recover(ex, tokenSet_16_);
					return;
				}
				throw ex;
			}
		}

		protected void globals(Module container)
		{
			try
			{
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 15:
				case 16:
				case 18:
				case 19:
				case 22:
				case 34:
				case 35:
				case 37:
				case 43:
				case 48:
				case 57:
				case 59:
				case 61:
				case 62:
				case 65:
				case 66:
				case 68:
				case 69:
				case 70:
				case 72:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (tokenSet_17_.member(LA(1)))
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
					recover(ex, tokenSet_18_);
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
				match(10);
				attribute = this.attribute();
				match(83);
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
					recover(ex, tokenSet_19_);
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
				match(11);
				attribute = this.attribute();
				match(83);
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
					recover(ex, tokenSet_19_);
					return;
				}
				throw ex;
			}
		}

		protected MacroStatement macro_stmt()
		{
			MacroStatement result = null;
			MacroStatement macroStatement = new MacroStatement();
			StatementModifier statementModifier = null;
			IToken token = null;
			try
			{
				token = macro_name();
				expression_list(macroStatement.Arguments);
				if (LA(1) == 88 && (LA(2) == 4 || LA(2) == 9 || LA(2) == 75))
				{
					begin_with_doc(macroStatement);
					macro_block(macroStatement.Body.Statements);
					end(macroStatement.Body);
					if (0 == inputState.guessing)
					{
						macroStatement.Annotate("compound");
					}
				}
				else if (LA(1) == 88 && tokenSet_20_.member(LA(2)))
				{
					macro_compound_stmt(macroStatement.Body);
					if (0 == inputState.guessing)
					{
						macroStatement.Annotate("compound");
					}
				}
				else
				{
					if (!tokenSet_21_.member(LA(1)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 9:
					case 75:
						eos();
						break;
					case 43:
					case 70:
					case 72:
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
				recover(ex, tokenSet_22_);
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
				match(38);
				expression = namespace_expression();
				if (0 == inputState.guessing && expression != null)
				{
					import = new Import(ToLexicalInfo(token), expression);
				}
				switch (LA(1))
				{
				case 33:
					match(33);
					switch (LA(1))
					{
					case 65:
					case 80:
						token5 = identifier();
						break;
					case 78:
						token2 = LT(1);
						match(78);
						if (0 == inputState.guessing)
						{
							token5 = token2;
						}
						break;
					case 79:
						token3 = LT(1);
						match(79);
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
				case 9:
				case 14:
				case 75:
				case 95:
					break;
				}
				switch (LA(1))
				{
				case 14:
					match(14);
					token4 = LT(1);
					match(80);
					if (0 == inputState.guessing)
					{
						import.Alias = new ReferenceExpression(ToLexicalInfo(token4));
						import.Alias.Name = token4.getText();
					}
					break;
				case 9:
				case 75:
				case 95:
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
				recover(ex, tokenSet_23_);
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
				match(33);
				expression = identifier_expression();
				match(38);
				if (0 == inputState.guessing)
				{
					MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(expression);
					ec = methodInvocationExpression.Arguments;
					import = new Import(ToLexicalInfo(token), methodInvocationExpression);
				}
				if (LA(1) == 81 && (LA(2) == 9 || LA(2) == 75))
				{
					match(81);
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
				recover(ex, tokenSet_19_);
			}
			return import;
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
				recover(ex, tokenSet_26_);
			}
			return result;
		}

		protected IToken identifier()
		{
			IToken token = null;
			_sbuilder.Length = 0;
			IToken token2 = null;
			IToken token3 = null;
			try
			{
				token2 = macro_name();
				if (0 == inputState.guessing)
				{
					if (token2 != null)
					{
						_sbuilder.Append(token2.getText());
					}
					token = token2;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) != 87 || !tokenSet_27_.member(LA(2)))
					{
						break;
					}
					match(87);
					token3 = member();
					if (0 == inputState.guessing)
					{
						_sbuilder.Append('.');
						if (token3 != null)
						{
							_sbuilder.Append(token3.getText());
						}
					}
				}
				if (0 == inputState.guessing)
				{
					token?.setText(_sbuilder.ToString());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_13_);
			}
			return token;
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
				case 76:
					match(76);
					if (0 == inputState.guessing)
					{
						MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(expression);
						ec = methodInvocationExpression.Arguments;
						expression = methodInvocationExpression;
					}
					expression_list(ec);
					match(77);
					break;
				case 9:
				case 14:
				case 33:
				case 75:
				case 95:
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
				recover(ex, tokenSet_28_);
			}
			return expression;
		}

		protected void expression_list(ExpressionCollection ec)
		{
			Expression expression = null;
			try
			{
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						ec.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85)
						{
							match(85);
							expression = this.expression();
							if (0 == inputState.guessing && expression != null)
							{
								ec.Add(expression);
							}
							continue;
						}
						break;
					}
					break;
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
				case 77:
				case 88:
				case 93:
				case 95:
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
					recover(ex, tokenSet_29_);
					return;
				}
				throw ex;
			}
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
					if (LA(1) != 82)
					{
						break;
					}
					match(82);
					switch (LA(1))
					{
					case 65:
					case 67:
					case 80:
						attribute = this.attribute();
						if (0 == inputState.guessing && attribute != null)
						{
							_attributes.Add(attribute);
						}
						while (true)
						{
							flag = true;
							if (LA(1) == 85)
							{
								match(85);
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
					case 83:
						break;
					}
					match(83);
					switch (LA(1))
					{
					case 9:
					case 75:
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 12:
					case 14:
					case 17:
					case 20:
					case 22:
					case 28:
					case 29:
					case 32:
					case 36:
					case 39:
					case 40:
					case 46:
					case 51:
					case 53:
					case 54:
					case 55:
					case 56:
					case 58:
					case 60:
					case 61:
					case 63:
					case 64:
					case 65:
					case 67:
					case 71:
					case 80:
					case 81:
					case 82:
					case 86:
					case 88:
						break;
					}
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_30_);
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
					if (tokenSet_31_.member(LA(1)))
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
					recover(ex, tokenSet_32_);
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
				case 20:
				case 64:
					class_definition(container);
					break;
				case 39:
					interface_definition(container);
					break;
				case 28:
					enum_definition(container);
					break;
				case 17:
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
					recover(ex, tokenSet_22_);
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
			IToken token4 = null;
			Method method = null;
			TypeReference typeReference = null;
			TypeReference typeReference2 = null;
			ExplicitMemberInfo explicitMemberInfo = null;
			ParameterDeclarationCollection c = null;
			GenericParameterDeclarationCollection c2 = null;
			Block block = null;
			StatementCollection container2 = null;
			Expression expression = null;
			TypeMember item = null;
			IToken token5 = null;
			try
			{
				token = LT(1);
				match(22);
				switch (LA(1))
				{
				case 29:
				case 36:
				case 40:
				case 54:
				case 55:
				case 58:
				case 60:
				case 80:
				case 86:
					if (LA(1) == 80 && LA(2) == 87)
					{
						explicitMemberInfo = explicit_member_info();
					}
					else if (!tokenSet_33_.member(LA(1)) || !tokenSet_34_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 29:
					case 36:
					case 40:
					case 54:
					case 55:
					case 58:
					case 60:
					case 80:
						token5 = member();
						break;
					case 86:
						token2 = LT(1);
						match(86);
						expression = atom();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						IToken token6 = token5 ?? token2;
						method = ((explicitMemberInfo == null) ? new Method(ToLexicalInfo(token6)) : new Method(explicitMemberInfo.LexicalInfo));
						method.Name = token6.getText();
						method.ExplicitInfo = explicitMemberInfo;
						item = ((expression == null) ? ((TypeMember)method) : ((TypeMember)new SpliceTypeMember(method, expression)));
					}
					break;
				case 21:
					token3 = LT(1);
					match(21);
					if (0 == inputState.guessing)
					{
						item = (method = new Constructor(ToLexicalInfo(token3)));
					}
					break;
				case 23:
					token4 = LT(1);
					match(23);
					if (0 == inputState.guessing)
					{
						item = (method = new Destructor(ToLexicalInfo(token4)));
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
				case 82:
					match(82);
					switch (LA(1))
					{
					case 49:
						match(49);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
						break;
					}
					generic_parameter_declaration_list(c2);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
					break;
				}
				match(76);
				parameter_declaration_list(c);
				match(77);
				attributes();
				if (0 == inputState.guessing)
				{
					AddAttributes(method.ReturnTypeAttributes);
				}
				switch (LA(1))
				{
				case 14:
					match(14);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						method.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
					break;
				}
				begin_block_with_doc(method, block);
				this.block(container2);
				end(block);
				if (0 == inputState.guessing)
				{
					container.Add(item);
					method.EndSourceLocation = block.EndSourceLocation;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_22_);
					return;
				}
				throw ex;
			}
		}

		protected void class_definition(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			TypeDefinition typeDefinition = null;
			TypeReferenceCollection container2 = null;
			TypeMemberCollection container3 = null;
			GenericParameterDeclarationCollection c = null;
			Expression nameExpression = null;
			try
			{
				switch (LA(1))
				{
				case 20:
					match(20);
					if (0 == inputState.guessing)
					{
						typeDefinition = new ClassDefinition();
					}
					break;
				case 64:
					match(64);
					if (0 == inputState.guessing)
					{
						typeDefinition = new StructDefinition();
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				switch (LA(1))
				{
				case 80:
					token = LT(1);
					match(80);
					break;
				case 86:
					token2 = LT(1);
					match(86);
					nameExpression = atom();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					IToken token3 = token ?? token2;
					typeDefinition.LexicalInfo = ToLexicalInfo(token3);
					typeDefinition.Name = token3.getText();
					typeDefinition.Modifiers = _modifiers;
					AddAttributes(typeDefinition.Attributes);
					container2 = typeDefinition.BaseTypes;
					container3 = typeDefinition.Members;
					c = typeDefinition.GenericParameters;
					if (token != null)
					{
						container.Add(typeDefinition);
					}
					else
					{
						container.Add(new SpliceTypeMember(typeDefinition, nameExpression));
					}
				}
				switch (LA(1))
				{
				case 82:
					match(82);
					switch (LA(1))
					{
					case 49:
						match(49);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
						break;
					}
					generic_parameter_declaration_list(c);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 76:
					base_types(container2);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
					break;
				}
				begin_with_doc(typeDefinition);
				switch (LA(1))
				{
				case 52:
					match(52);
					eos();
					break;
				case 9:
				case 12:
				case 17:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 39:
				case 40:
				case 46:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 61:
				case 63:
				case 64:
				case 65:
				case 67:
				case 71:
				case 75:
				case 80:
				case 82:
				case 86:
				{
					switch (LA(1))
					{
					case 9:
					case 75:
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 12:
					case 17:
					case 20:
					case 22:
					case 28:
					case 29:
					case 32:
					case 39:
					case 40:
					case 46:
					case 51:
					case 53:
					case 54:
					case 55:
					case 56:
					case 61:
					case 63:
					case 64:
					case 65:
					case 67:
					case 71:
					case 80:
					case 82:
					case 86:
						break;
					}
					int num = 0;
					while (true)
					{
						bool flag = true;
						bool flag2 = false;
						if (LA(1) == 86 && tokenSet_34_.member(LA(2)))
						{
							int pos = mark();
							flag2 = true;
							inputState.guessing++;
							try
							{
								splice_expression();
								eos();
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
							splice_type_definition_body(container3);
						}
						else
						{
							if (!tokenSet_35_.member(LA(1)) || !tokenSet_36_.member(LA(2)))
							{
								break;
							}
							type_definition_member(container3);
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
				end(typeDefinition);
			}
			catch (RecognitionException ex2)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex2);
					recover(ex2, tokenSet_22_);
					return;
				}
				throw ex2;
			}
		}

		protected void interface_definition(TypeMemberCollection container)
		{
			IToken token = null;
			IToken token2 = null;
			InterfaceDefinition interfaceDefinition = null;
			TypeMemberCollection container2 = null;
			GenericParameterDeclarationCollection c = null;
			Expression nameExpression = null;
			try
			{
				match(39);
				switch (LA(1))
				{
				case 80:
					token = LT(1);
					match(80);
					break;
				case 86:
					token2 = LT(1);
					match(86);
					nameExpression = atom();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					IToken token3 = token ?? token2;
					interfaceDefinition = new InterfaceDefinition(ToLexicalInfo(token3));
					interfaceDefinition.Name = token3.getText();
					interfaceDefinition.Modifiers = _modifiers;
					AddAttributes(interfaceDefinition.Attributes);
					if (token != null)
					{
						container.Add(interfaceDefinition);
					}
					else
					{
						container.Add(new SpliceTypeMember(interfaceDefinition, nameExpression));
					}
					container2 = interfaceDefinition.Members;
					c = interfaceDefinition.GenericParameters;
				}
				switch (LA(1))
				{
				case 82:
					match(82);
					switch (LA(1))
					{
					case 49:
						match(49);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
						break;
					}
					generic_parameter_declaration_list(c);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 76:
					base_types(interfaceDefinition.BaseTypes);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
					break;
				}
				begin_with_doc(interfaceDefinition);
				switch (LA(1))
				{
				case 52:
					match(52);
					eos();
					break;
				case 22:
				case 29:
				case 61:
				case 80:
				case 82:
				{
					int num = 0;
					while (true)
					{
						bool flag = true;
						if (!tokenSet_37_.member(LA(1)))
						{
							break;
						}
						attributes();
						switch (LA(1))
						{
						case 22:
							interface_method(container2);
							break;
						case 29:
							event_declaration(container2);
							break;
						case 61:
						case 80:
							interface_property(container2);
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
				end(interfaceDefinition);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_22_);
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
				match(28);
				token = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					enumDefinition = new EnumDefinition(ToLexicalInfo(token));
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
				switch (LA(1))
				{
				case 52:
					match(52);
					eos();
					break;
				case 80:
				case 82:
				case 86:
				{
					int num = 0;
					while (true)
					{
						bool flag = true;
						switch (LA(1))
						{
						case 80:
						case 82:
							enum_member(container2);
							goto IL_014e;
						case 86:
							splice_type_definition_body(container2);
							goto IL_014e;
						}
						break;
						IL_014e:
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
				end(enumDefinition);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_22_);
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
				match(17);
				token = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					callableDefinition = new CallableDefinition(ToLexicalInfo(token));
					callableDefinition.Name = token.getText();
					callableDefinition.Modifiers = _modifiers;
					AddAttributes(callableDefinition.Attributes);
					container.Add(callableDefinition);
					c = callableDefinition.GenericParameters;
				}
				switch (LA(1))
				{
				case 82:
					match(82);
					switch (LA(1))
					{
					case 49:
						match(49);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
						break;
					}
					generic_parameter_declaration_list(c);
					match(83);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
					break;
				}
				match(76);
				parameter_declaration_list(callableDefinition.Parameters);
				match(77);
				switch (LA(1))
				{
				case 14:
					match(14);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						callableDefinition.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 75:
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
					recover(ex, tokenSet_22_);
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
					if (LA(1) == 85)
					{
						match(85);
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
					recover(ex, tokenSet_38_);
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
				case 58:
				case 80:
				case 81:
				case 82:
				case 86:
					flag = parameter_declaration(c);
					while (true)
					{
						bool flag2 = true;
						if (LA(1) == 85 && !flag)
						{
							match(85);
							flag = parameter_declaration(c);
							continue;
						}
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 77:
				case 83:
				case 91:
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
					recover(ex, tokenSet_39_);
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
				case 86:
					typeReference = splice_type_reference();
					break;
				case 76:
					typeReference = array_type_reference();
					break;
				default:
				{
					bool flag = false;
					if (LA(1) == 17 && LA(2) == 76)
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							match(17);
							match(76);
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
					if (tokenSet_40_.member(LA(1)) && tokenSet_41_.member(LA(2)))
					{
						token = type_name();
						if (LA(1) == 82 && tokenSet_42_.member(LA(2)))
						{
							match(82);
							switch (LA(1))
							{
							case 49:
								match(49);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 17:
							case 19:
							case 65:
							case 76:
							case 80:
							case 81:
							case 86:
								break;
							}
							switch (LA(1))
							{
							case 81:
								match(81);
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
									if (LA(1) == 85)
									{
										match(85);
										match(81);
										if (0 == inputState.guessing)
										{
											genericTypeDefinitionReference.GenericPlaceholders++;
										}
										continue;
									}
									break;
								}
								match(83);
								break;
							case 17:
							case 19:
							case 65:
							case 76:
							case 80:
							case 86:
								if (0 == inputState.guessing)
								{
									GenericTypeReference genericTypeReference = new GenericTypeReference(ToLexicalInfo(token), token.getText());
									container = genericTypeReference.GenericArguments;
									typeReference = genericTypeReference;
								}
								type_reference_list(container);
								match(83);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
						}
						else if (LA(1) == 49 && LA(2) == 81)
						{
							match(49);
							match(81);
							if (0 == inputState.guessing)
							{
								genericTypeDefinitionReference = new GenericTypeDefinitionReference(ToLexicalInfo(token));
								genericTypeDefinitionReference.Name = token.getText();
								genericTypeDefinitionReference.GenericPlaceholders = 1;
								typeReference = genericTypeDefinitionReference;
							}
						}
						else if (LA(1) == 49 && tokenSet_43_.member(LA(2)))
						{
							match(49);
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
							if (!tokenSet_41_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
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
						if (LA(1) == 89 && tokenSet_41_.member(LA(2)))
						{
							match(89);
							if (0 == inputState.guessing)
							{
								GenericTypeReference genericTypeReference2 = new GenericTypeReference(typeReference.LexicalInfo, "System.Nullable");
								genericTypeReference2.GenericArguments.Add(typeReference);
								typeReference = genericTypeReference2;
							}
						}
						else if (!tokenSet_41_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
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
					if (LA(1) == 81 && tokenSet_41_.member(LA(2)))
					{
						match(81);
						if (0 == inputState.guessing)
						{
							typeReference = CodeFactory.EnumerableTypeReferenceFor(typeReference);
						}
						continue;
					}
					if (LA(1) == 90 && tokenSet_41_.member(LA(2)))
					{
						match(90);
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
				recover(ex2, tokenSet_41_);
			}
			return typeReference;
		}

		protected void begin_with_doc(Node node)
		{
			try
			{
				match(88);
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					docstring(node);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 4:
					break;
				}
				match(4);
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_44_);
					return;
				}
				throw ex;
			}
		}

		protected void enum_member(TypeMemberCollection container)
		{
			IToken token = null;
			EnumMember enumMember = null;
			Expression initializer = null;
			try
			{
				attributes();
				token = LT(1);
				match(80);
				switch (LA(1))
				{
				case 84:
					match(84);
					initializer = simple_initializer();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 75:
					break;
				}
				if (0 == inputState.guessing)
				{
					enumMember = new EnumMember(ToLexicalInfo(token));
					enumMember.Name = token.getText();
					enumMember.Initializer = initializer;
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
				match(86);
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
				match(5);
				if (0 == inputState.guessing)
				{
					SetEndSourceLocation(node, token);
				}
				if ((LA(1) == 9 || LA(1) == 75) && tokenSet_47_.member(LA(2)))
				{
					eos();
				}
				else if (!tokenSet_47_.member(LA(1)) || !tokenSet_48_.member(LA(2)))
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

		public Expression simple_initializer()
		{
			Expression result = null;
			try
			{
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					result = array_or_expression();
					break;
				case 22:
				case 24:
				case 88:
					result = callable_expression();
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
				recover(ex, tokenSet_22_);
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
				case 65:
				case 80:
					token2 = identifier();
					break;
				case 67:
					token = LT(1);
					match(67);
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
				case 76:
					match(76);
					argument_list(attribute);
					match(77);
					break;
				case 83:
				case 85:
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
				recover(ex, tokenSet_49_);
			}
			return attribute;
		}

		protected void argument_list(INodeWithArguments node)
		{
			try
			{
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					argument(node);
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85)
						{
							match(85);
							argument(node);
							continue;
						}
						break;
					}
					break;
				case 77:
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
					recover(ex, tokenSet_50_);
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
				case 8:
				case 35:
				case 48:
				case 61:
				case 62:
				case 68:
				case 74:
				case 78:
				case 79:
				case 82:
				case 92:
				case 94:
				case 105:
				case 112:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					result = literal();
					break;
				case 76:
					result = paren_expression();
					break;
				case 18:
					result = cast_expression();
					break;
				case 69:
					result = typeof_expression();
					break;
				case 86:
					result = splice_expression();
					break;
				case 87:
					result = omitted_member_expression();
					break;
				default:
				{
					bool flag = false;
					if (LA(1) == 19 && LA(2) == 76)
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							match(19);
							match(76);
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
					if ((LA(1) == 19 || LA(1) == 65 || LA(1) == 80) && tokenSet_41_.member(LA(2)))
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
				recover(ex2, tokenSet_41_);
			}
			return result;
		}

		protected void base_types(TypeReferenceCollection container)
		{
			TypeReference typeReference = null;
			try
			{
				match(76);
				switch (LA(1))
				{
				case 17:
				case 19:
				case 65:
				case 76:
				case 80:
				case 86:
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						container.Add(typeReference);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85)
						{
							match(85);
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
				case 77:
					break;
				}
				match(77);
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

		protected Expression splice_expression()
		{
			IToken token = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(86);
				expression = atom();
				if (0 == inputState.guessing)
				{
					expression = new SpliceExpression(ToLexicalInfo(token), expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 22:
					method(container);
					break;
				case 29:
					event_declaration(container);
					break;
				case 61:
				case 65:
				case 80:
				case 86:
					field_or_property(container);
					break;
				case 17:
				case 20:
				case 28:
				case 39:
				case 64:
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
					recover(ex, tokenSet_52_);
					return;
				}
				throw ex;
			}
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
				match(29);
				token2 = LT(1);
				match(80);
				match(14);
				typeReference = type_reference();
				eos();
				if (0 == inputState.guessing)
				{
					@event = new Event(ToLexicalInfo(token2), token2.getText(), typeReference);
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
					recover(ex, tokenSet_52_);
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
			IToken token5 = null;
			IToken token6 = null;
			IToken token7 = null;
			TypeMember typeMember = null;
			TypeReference type = null;
			Property property = null;
			Field field = null;
			ExplicitMemberInfo explicitMemberInfo = null;
			Expression expression = null;
			ParameterDeclarationCollection c = null;
			Expression expression2 = null;
			try
			{
				bool flag = false;
				if ((LA(1) == 61 || LA(1) == 80 || LA(1) == 86) && tokenSet_53_.member(LA(2)))
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
					if (LA(1) == 80 && LA(2) == 87)
					{
						explicitMemberInfo = explicit_member_info();
					}
					else if ((LA(1) != 61 && LA(1) != 80 && LA(1) != 86) || !tokenSet_53_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 80:
						token = LT(1);
						match(80);
						if (0 == inputState.guessing)
						{
							token7 = token;
						}
						break;
					case 86:
						token2 = LT(1);
						match(86);
						expression2 = atom();
						if (0 == inputState.guessing)
						{
							token7 = token2;
						}
						break;
					case 61:
						token3 = LT(1);
						match(61);
						if (0 == inputState.guessing)
						{
							token7 = token3;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					if (0 == inputState.guessing)
					{
						property = ((explicitMemberInfo == null) ? new Property(ToLexicalInfo(token7)) : new Property(explicitMemberInfo.LexicalInfo));
						property.Name = token7.getText();
						property.ExplicitInfo = explicitMemberInfo;
						AddAttributes(property.Attributes);
						c = property.Parameters;
						typeMember = property;
					}
					switch (LA(1))
					{
					case 76:
						token4 = LT(1);
						match(76);
						parameter_declaration_list(c);
						match(77);
						if (0 == inputState.guessing)
						{
							EmitIndexedPropertyDeprecationWarning(property);
						}
						break;
					case 82:
						match(82);
						parameter_declaration_list(c);
						match(83);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 14:
					case 88:
						break;
					}
					switch (LA(1))
					{
					case 14:
						match(14);
						type = type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 88:
						break;
					}
					if (0 == inputState.guessing)
					{
						property.Type = type;
						property.Modifiers = _modifiers;
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
				}
				else
				{
					bool flag3 = false;
					if ((LA(1) == 65 || LA(1) == 80) && tokenSet_4_.member(LA(2)))
					{
						int pos2 = mark();
						flag3 = true;
						inputState.guessing++;
						try
						{
							macro_name();
							expression_list(null);
							switch (LA(1))
							{
							case 9:
							case 75:
								eos();
								break;
							case 88:
								begin_with_doc(null);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
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
						typeMember = member_macro();
					}
					else
					{
						if ((LA(1) != 80 && LA(1) != 86) || !tokenSet_55_.member(LA(2)))
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						switch (LA(1))
						{
						case 80:
							token5 = LT(1);
							match(80);
							break;
						case 86:
							token6 = LT(1);
							match(86);
							expression2 = atom();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						if (0 == inputState.guessing)
						{
							IToken token8 = token5 ?? token6;
							field = new Field(ToLexicalInfo(token8));
							field.Name = token8.getText();
							field.Modifiers = _modifiers;
							AddAttributes(field.Attributes);
							typeMember = field;
						}
						switch (LA(1))
						{
						case 14:
							match(14);
							type = type_reference();
							if (0 == inputState.guessing)
							{
								field.Type = type;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 9:
						case 75:
						case 84:
							break;
						}
						switch (LA(1))
						{
						case 84:
							match(84);
							expression = declaration_initializer();
							if (0 == inputState.guessing)
							{
								field.Initializer = expression;
							}
							break;
						case 9:
						case 75:
							eos();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						docstring(field);
					}
				}
				if (0 == inputState.guessing)
				{
					if (null != expression2)
					{
						typeMember = new SpliceTypeMember(typeMember, expression2);
					}
					container.Add(typeMember);
				}
			}
			catch (RecognitionException ex3)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex3);
					recover(ex3, tokenSet_52_);
					return;
				}
				throw ex3;
			}
		}

		protected void interface_method(TypeMemberCollection container)
		{
			IToken token = null;
			Method method = null;
			TypeReference typeReference = null;
			Expression expression = null;
			IToken token2 = null;
			try
			{
				match(22);
				switch (LA(1))
				{
				case 29:
				case 36:
				case 40:
				case 54:
				case 55:
				case 58:
				case 60:
				case 80:
					token2 = member();
					break;
				case 86:
					token = LT(1);
					match(86);
					expression = atom();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					IToken token3 = token2 ?? token;
					method = new Method(ToLexicalInfo(token3));
					method.Name = token3.getText();
					AddAttributes(method.Attributes);
					if (expression != null)
					{
						container.Add(new SpliceTypeMember(method, expression));
					}
					else
					{
						container.Add(method);
					}
				}
				switch (LA(1))
				{
				case 82:
					match(82);
					switch (LA(1))
					{
					case 49:
						match(49);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
						break;
					}
					generic_parameter_declaration_list(method.GenericParameters);
					match(83);
					break;
				case 49:
					match(49);
					generic_parameter_declaration(method.GenericParameters);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 76:
					break;
				}
				match(76);
				parameter_declaration_list(method.Parameters);
				match(77);
				switch (LA(1))
				{
				case 14:
					match(14);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						method.ReturnType = typeReference;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 75:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					docstring(method);
					break;
				case 88:
					empty_block(method);
					switch (LA(1))
					{
					case 9:
					case 75:
						eos();
						break;
					case 5:
					case 22:
					case 29:
					case 61:
					case 80:
					case 82:
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
				case 80:
					token = LT(1);
					match(80);
					if (0 == inputState.guessing)
					{
						token3 = token;
					}
					break;
				case 61:
					token2 = LT(1);
					match(61);
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
					property = new Property(ToLexicalInfo(token3));
					property.Name = token3.getText();
					AddAttributes(property.Attributes);
					container.Add(property);
					c = property.Parameters;
				}
				switch (LA(1))
				{
				case 76:
				case 82:
					switch (LA(1))
					{
					case 82:
						match(82);
						break;
					case 76:
						match(76);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					parameter_declaration_list(c);
					switch (LA(1))
					{
					case 83:
						match(83);
						break;
					case 77:
						match(77);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 14:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 14:
					match(14);
					type = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
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
					if (LA(1) != 36 && LA(1) != 60 && LA(1) != 82)
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
				case 80:
					token = LT(1);
					match(80);
					if (0 == inputState.guessing)
					{
						result = token;
					}
					break;
				case 60:
					token2 = LT(1);
					match(60);
					if (0 == inputState.guessing)
					{
						result = token2;
					}
					break;
				case 36:
					token3 = LT(1);
					match(36);
					if (0 == inputState.guessing)
					{
						result = token3;
					}
					break;
				case 40:
					token4 = LT(1);
					match(40);
					if (0 == inputState.guessing)
					{
						result = token4;
					}
					break;
				case 54:
					token5 = LT(1);
					match(54);
					if (0 == inputState.guessing)
					{
						result = token5;
					}
					break;
				case 55:
					token6 = LT(1);
					match(55);
					if (0 == inputState.guessing)
					{
						result = token6;
					}
					break;
				case 29:
					token7 = LT(1);
					match(29);
					if (0 == inputState.guessing)
					{
						result = token7;
					}
					break;
				case 58:
					token8 = LT(1);
					match(58);
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
				recover(ex, tokenSet_13_);
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
				match(80);
				if (0 == inputState.guessing)
				{
					genericParameterDeclaration = new GenericParameterDeclaration(ToLexicalInfo(token));
					genericParameterDeclaration.Name = token.getText();
					c.Add(genericParameterDeclaration);
				}
				if (LA(1) == 76 && tokenSet_57_.member(LA(2)))
				{
					match(76);
					generic_parameter_constraints(genericParameterDeclaration);
					match(77);
				}
				else if ((LA(1) != 76 && LA(1) != 83 && LA(1) != 85) || !tokenSet_58_.member(LA(2)))
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
				match(52);
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
				if (LA(1) == 36 && null == p.Getter)
				{
					token = LT(1);
					match(36);
					if (0 == inputState.guessing)
					{
						Method method3 = (p.Getter = new Method(ToLexicalInfo(token)));
						method = method3;
						method.Name = "get";
					}
				}
				else
				{
					if (LA(1) != 60 || null != p.Setter)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token2 = LT(1);
					match(60);
					if (0 == inputState.guessing)
					{
						Method method3 = (p.Setter = new Method(ToLexicalInfo(token2)));
						method = method3;
						method.Name = "set";
					}
				}
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					break;
				case 88:
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
				match(88);
				match(4);
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
				match(80);
				match(87);
				if (0 == inputState.guessing)
				{
					explicitMemberInfo = new ExplicitMemberInfo(ToLexicalInfo(token));
					_sbuilder.Append(token.getText());
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 80 && LA(2) == 87)
					{
						token2 = LT(1);
						match(80);
						match(87);
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
				match(88);
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					docstring(node);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 4:
					break;
				}
				token = LT(1);
				match(4);
				if (0 == inputState.guessing)
				{
					block.LexicalInfo = ToLexicalInfo(token);
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
				case 9:
				case 75:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 8:
				case 15:
				case 16:
				case 18:
				case 19:
				case 22:
				case 34:
				case 35:
				case 37:
				case 43:
				case 48:
				case 52:
				case 57:
				case 59:
				case 61:
				case 62:
				case 65:
				case 66:
				case 68:
				case 69:
				case 70:
				case 72:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				switch (LA(1))
				{
				case 52:
					match(52);
					eos();
					break;
				case 8:
				case 15:
				case 16:
				case 18:
				case 19:
				case 22:
				case 34:
				case 35:
				case 37:
				case 43:
				case 48:
				case 57:
				case 59:
				case 61:
				case 62:
				case 65:
				case 66:
				case 68:
				case 69:
				case 70:
				case 72:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				{
					int num = 0;
					while (true)
					{
						bool flag = true;
						if (!tokenSet_17_.member(LA(1)))
						{
							break;
						}
						stmt(container);
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
					recover(ex, tokenSet_65_);
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
				case 80:
					match(80);
					break;
				case 61:
					match(61);
					break;
				case 86:
					match(86);
					atom();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 87)
					{
						match(87);
						match(80);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 82:
					match(82);
					break;
				case 14:
				case 76:
				case 88:
					switch (LA(1))
					{
					case 76:
						match(76);
						parameter_declaration_list(null);
						match(77);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 14:
					case 88:
						break;
					}
					switch (LA(1))
					{
					case 14:
						match(14);
						type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 88:
						break;
					}
					begin_with_doc(null);
					attributes();
					modifiers();
					switch (LA(1))
					{
					case 36:
						match(36);
						break;
					case 60:
						match(60);
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
				if (LA(1) == 36 && p != null && null == p.Getter)
				{
					token = LT(1);
					match(36);
					if (0 == inputState.guessing)
					{
						method = (p.Getter = new Method(ToLexicalInfo(token)));
						method.Name = "get";
					}
				}
				else
				{
					if (LA(1) != 60 || p == null || null != p.Setter)
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token2 = LT(1);
					match(60);
					if (0 == inputState.guessing)
					{
						method = (p.Setter = new Method(ToLexicalInfo(token2)));
						method.Name = "set";
					}
				}
				if (0 == inputState.guessing)
				{
					AddAttributes(method.Attributes);
					method.Modifiers = _modifiers;
					b = method.Body;
				}
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					break;
				case 88:
					compound_stmt(b);
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
					recover(ex, tokenSet_66_);
					return;
				}
				throw ex;
			}
		}

		protected TypeMember member_macro()
		{
			MacroStatement macroStatement = null;
			TypeMember typeMember = null;
			try
			{
				macroStatement = macro_stmt();
				if (0 == inputState.guessing)
				{
					typeMember = new StatementTypeMember(macroStatement);
					typeMember.Modifiers = _modifiers;
					AddAttributes(typeMember.Attributes);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_52_);
			}
			return typeMember;
		}

		public Expression declaration_initializer()
		{
			Expression expression = null;
			try
			{
				bool flag = false;
				if (tokenSet_34_.member(LA(1)) && tokenSet_67_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						slicing_expression();
						switch (LA(1))
						{
						case 88:
							match(88);
							break;
						case 24:
							match(24);
							break;
						case 22:
							match(22);
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
				else if (tokenSet_68_.member(LA(1)) && tokenSet_69_.member(LA(2)))
				{
					expression = array_or_expression();
					eos();
				}
				else
				{
					if (LA(1) != 22 && LA(1) != 24 && LA(1) != 88)
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
				recover(ex2, tokenSet_22_);
			}
			return expression;
		}

		protected Expression slicing_expression()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			Expression expression = null;
			SlicingExpression slicingExpression = null;
			MethodInvocationExpression methodInvocationExpression = null;
			IToken token5 = null;
			TypeReference typeReference = null;
			TypeReferenceCollection container = null;
			Expression expression2 = null;
			Expression expression3 = null;
			try
			{
				expression = atom();
				while (true)
				{
					bool flag = true;
					if (LA(1) == 82 && tokenSet_70_.member(LA(2)))
					{
						token = LT(1);
						match(82);
						switch (LA(1))
						{
						case 49:
							match(49);
							if (0 == inputState.guessing)
							{
								GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression(ToLexicalInfo(token));
								genericReferenceExpression.Target = expression;
								expression = genericReferenceExpression;
								container = genericReferenceExpression.GenericArguments;
							}
							type_reference_list(container);
							break;
						case 8:
						case 18:
						case 19:
						case 35:
						case 47:
						case 48:
						case 61:
						case 62:
						case 65:
						case 68:
						case 69:
						case 74:
						case 76:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 86:
						case 87:
						case 88:
						case 92:
						case 94:
						case 105:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
						case 120:
						case 121:
							if (0 == inputState.guessing)
							{
								slicingExpression = new SlicingExpression(ToLexicalInfo(token));
								slicingExpression.Target = expression;
								expression = slicingExpression;
							}
							slice(slicingExpression);
							while (true)
							{
								flag = true;
								if (LA(1) == 85)
								{
									match(85);
									slice(slicingExpression);
									continue;
								}
								break;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						match(83);
						continue;
					}
					if (LA(1) == 49)
					{
						token2 = LT(1);
						match(49);
						typeReference = type_reference();
						if (0 == inputState.guessing)
						{
							GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression(ToLexicalInfo(token2));
							genericReferenceExpression.Target = expression;
							expression = genericReferenceExpression;
							genericReferenceExpression.GenericArguments.Add(typeReference);
						}
						continue;
					}
					if (LA(1) == 87 && tokenSet_33_.member(LA(2)))
					{
						match(87);
						switch (LA(1))
						{
						case 29:
						case 36:
						case 40:
						case 54:
						case 55:
						case 58:
						case 60:
						case 80:
							token5 = member();
							if (0 == inputState.guessing)
							{
								expression = MemberReferenceForToken(expression, token5);
							}
							break;
						case 86:
							token3 = LT(1);
							match(86);
							expression2 = atom();
							if (0 == inputState.guessing)
							{
								expression = new SpliceMemberReferenceExpression(ToLexicalInfo(token3), expression, expression2);
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						continue;
					}
					if (LA(1) != 76 || !tokenSet_71_.member(LA(2)))
					{
						break;
					}
					token4 = LT(1);
					match(76);
					if (0 == inputState.guessing)
					{
						methodInvocationExpression = new MethodInvocationExpression(ToLexicalInfo(token4));
						methodInvocationExpression.Target = expression;
						expression = methodInvocationExpression;
					}
					switch (LA(1))
					{
					case 8:
					case 18:
					case 19:
					case 35:
					case 47:
					case 48:
					case 61:
					case 62:
					case 65:
					case 68:
					case 69:
					case 74:
					case 76:
					case 78:
					case 79:
					case 80:
					case 81:
					case 82:
					case 86:
					case 87:
					case 92:
					case 94:
					case 105:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
						argument(methodInvocationExpression);
						while (true)
						{
							flag = true;
							if (LA(1) == 85)
							{
								match(85);
								argument(methodInvocationExpression);
								continue;
							}
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 77:
						break;
					}
					match(77);
					if (LA(1) == 92 && tokenSet_72_.member(LA(2)))
					{
						bool flag2 = false;
						if (LA(1) == 92 && tokenSet_72_.member(LA(2)))
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
							expression3 = hash_literal();
						}
						else
						{
							if (LA(1) != 92 || !tokenSet_72_.member(LA(2)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							expression3 = list_initializer();
						}
						if (0 == inputState.guessing)
						{
							expression = new CollectionInitializationExpression(expression, expression3);
						}
					}
					else if (!tokenSet_73_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
					{
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
				recover(ex2, tokenSet_74_);
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
					methodInvocationExpression = (e as MethodInvocationExpression) ?? new MethodInvocationExpression(e.LexicalInfo, e);
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
				recover(ex, tokenSet_22_);
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
				case 85:
					token = LT(1);
					match(85);
					if (0 == inputState.guessing)
					{
						expression = new ArrayLiteralExpression(ToLexicalInfo(token));
					}
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = this.expression();
					switch (LA(1))
					{
					case 85:
						token2 = LT(1);
						match(85);
						if (0 == inputState.guessing)
						{
							arrayLiteralExpression = new ArrayLiteralExpression(expression.LexicalInfo);
							arrayLiteralExpression.Items.Add(expression);
						}
						if (tokenSet_5_.member(LA(1)) && tokenSet_75_.member(LA(2)))
						{
							expression = this.expression();
							if (0 == inputState.guessing)
							{
								arrayLiteralExpression.Items.Add(expression);
							}
							while (true)
							{
								bool flag = true;
								if (LA(1) == 85 && tokenSet_5_.member(LA(2)))
								{
									match(85);
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
							case 85:
								match(85);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 1:
							case 5:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12:
							case 15:
							case 16:
							case 17:
							case 18:
							case 19:
							case 20:
							case 22:
							case 24:
							case 28:
							case 29:
							case 32:
							case 34:
							case 35:
							case 37:
							case 39:
							case 40:
							case 43:
							case 46:
							case 48:
							case 51:
							case 53:
							case 54:
							case 55:
							case 56:
							case 57:
							case 59:
							case 61:
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
							case 76:
							case 77:
							case 78:
							case 79:
							case 80:
							case 81:
							case 82:
							case 86:
							case 87:
							case 88:
							case 92:
							case 93:
							case 94:
							case 95:
							case 105:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
								break;
							}
						}
						else if (!tokenSet_76_.member(LA(1)) || !tokenSet_77_.member(LA(2)))
						{
							throw new NoViableAltException(LT(1), getFilename());
						}
						if (0 == inputState.guessing)
						{
							expression = arrayLiteralExpression;
						}
						break;
					case 1:
					case 5:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 22:
					case 24:
					case 28:
					case 29:
					case 32:
					case 34:
					case 35:
					case 37:
					case 39:
					case 40:
					case 43:
					case 46:
					case 48:
					case 51:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
					case 59:
					case 61:
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
					case 76:
					case 77:
					case 78:
					case 79:
					case 80:
					case 81:
					case 82:
					case 86:
					case 87:
					case 88:
					case 92:
					case 93:
					case 94:
					case 95:
					case 105:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
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
				recover(ex, tokenSet_76_);
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
				case 88:
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
				case 22:
				case 24:
					switch (LA(1))
					{
					case 24:
						token = LT(1);
						match(24);
						if (0 == inputState.guessing)
						{
							token3 = token;
						}
						break;
					case 22:
						token2 = LT(1);
						match(22);
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
						result = (blockExpression = new BlockExpression(ToLexicalInfo(token3)));
						block = blockExpression.Body;
					}
					switch (LA(1))
					{
					case 76:
						match(76);
						parameter_declaration_list(blockExpression.Parameters);
						match(77);
						switch (LA(1))
						{
						case 14:
							match(14);
							typeReference = type_reference();
							if (0 == inputState.guessing)
							{
								blockExpression.ReturnType = typeReference;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 88:
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 88:
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
				recover(ex, tokenSet_22_);
			}
			return result;
		}

		protected void compound_stmt(Block b)
		{
			IToken token = null;
			StatementCollection container = null;
			try
			{
				if (LA(1) == 88 && tokenSet_78_.member(LA(2)))
				{
					single_line_block(b);
					return;
				}
				if (LA(1) == 88 && LA(2) == 4)
				{
					match(88);
					token = LT(1);
					match(4);
					if (0 == inputState.guessing)
					{
						b.LexicalInfo = ToLexicalInfo(token);
						container = b.Statements;
					}
					block(container);
					end(b);
					return;
				}
				throw new NoViableAltException(LT(1), getFilename());
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

		protected void stmt(StatementCollection container)
		{
			Statement statement = null;
			StatementModifier statementModifier = null;
			try
			{
				switch (LA(1))
				{
				case 22:
					statement = nested_function();
					break;
				case 34:
					statement = for_stmt();
					break;
				case 72:
					statement = while_stmt();
					break;
				case 43:
					statement = if_stmt();
					break;
				case 70:
					statement = unless_stmt();
					break;
				case 66:
					statement = try_stmt();
					break;
				case 59:
					statement = return_stmt();
					break;
				default:
				{
					bool flag = false;
					if ((LA(1) == 65 || LA(1) == 80) && tokenSet_4_.member(LA(2)) && IsValidMacroArgument(LA(2)))
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							macro_name();
							if (tokenSet_5_.member(LA(1)))
							{
								expression();
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
						statement = macro_stmt();
						break;
					}
					bool flag2 = false;
					if (tokenSet_34_.member(LA(1)) && tokenSet_79_.member(LA(2)))
					{
						int pos2 = mark();
						flag2 = true;
						inputState.guessing++;
						try
						{
							slicing_expression();
							switch (LA(1))
							{
							case 84:
								match(84);
								break;
							case 22:
							case 24:
							case 88:
								switch (LA(1))
								{
								case 88:
									match(88);
									break;
								case 24:
									match(24);
									break;
								case 22:
									match(22);
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
							flag2 = false;
						}
						rewind(pos2);
						inputState.guessing--;
					}
					if (flag2)
					{
						statement = assignment_or_method_invocation_with_block_stmt();
						break;
					}
					bool flag3 = false;
					if (LA(1) == 80 && (LA(2) == 14 || LA(2) == 85))
					{
						int pos3 = mark();
						flag3 = true;
						inputState.guessing++;
						try
						{
							declaration();
							match(85);
						}
						catch (RecognitionException)
						{
							flag3 = false;
						}
						rewind(pos3);
						inputState.guessing--;
					}
					if (flag3)
					{
						statement = unpack_stmt();
						break;
					}
					if (LA(1) == 80 && LA(2) == 14)
					{
						statement = declaration_stmt();
						break;
					}
					if (tokenSet_80_.member(LA(1)) && tokenSet_81_.member(LA(2)))
					{
						switch (LA(1))
						{
						case 37:
							statement = goto_stmt();
							break;
						case 88:
							statement = label_stmt();
							break;
						case 73:
							statement = yield_stmt();
							break;
						case 15:
							statement = break_stmt();
							break;
						case 16:
							statement = continue_stmt();
							break;
						case 57:
							statement = raise_stmt();
							break;
						case 8:
						case 18:
						case 19:
						case 35:
						case 48:
						case 61:
						case 62:
						case 65:
						case 68:
						case 69:
						case 74:
						case 76:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 86:
						case 87:
						case 92:
						case 94:
						case 105:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
						case 120:
						case 121:
							statement = expression_stmt();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						}
						switch (LA(1))
						{
						case 43:
						case 70:
						case 72:
							statementModifier = stmt_modifier();
							if (0 == inputState.guessing && statement != null)
							{
								statement.Modifier = statementModifier;
							}
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 9:
						case 75:
							break;
						}
						eos();
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				}
				}
				if (0 == inputState.guessing && statement != null && container != null)
				{
					container.Add(statement);
				}
			}
			catch (RecognitionException ex4)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex4);
					recover(ex4, tokenSet_82_);
					return;
				}
				throw ex4;
			}
		}

		protected void type_member_modifier()
		{
			IToken token = null;
			try
			{
				switch (LA(1))
				{
				case 63:
					match(63);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Static;
					}
					break;
				case 54:
					match(54);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Public;
					}
					break;
				case 55:
					match(55);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Protected;
					}
					break;
				case 56:
					match(56);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Private;
					}
					break;
				case 40:
					match(40);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Internal;
					}
					break;
				case 32:
					match(32);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Final;
					}
					break;
				case 67:
					token = LT(1);
					match(67);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Transient;
						EmitTransientKeywordDeprecationWarning(ToLexicalInfo(token));
					}
					break;
				case 51:
					match(51);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Override;
					}
					break;
				case 12:
					match(12);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Abstract;
					}
					break;
				case 71:
					match(71);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.Virtual;
					}
					break;
				case 46:
					match(46);
					if (0 == inputState.guessing)
					{
						_modifiers |= TypeMemberModifiers.New;
					}
					break;
				case 53:
					match(53);
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
					recover(ex, tokenSet_83_);
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
				match(58);
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
				recover(ex, tokenSet_43_);
			}
			return result;
		}

		protected bool parameter_declaration(ParameterDeclarationCollection c)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			TypeReference type = null;
			ParameterModifiers parameterModifiers = ParameterModifiers.None;
			bool result = false;
			Expression expression = null;
			try
			{
				attributes();
				switch (LA(1))
				{
				case 81:
					match(81);
					if (0 == inputState.guessing)
					{
						result = true;
					}
					switch (LA(1))
					{
					case 80:
						token = LT(1);
						match(80);
						if (0 == inputState.guessing)
						{
							token5 = token;
						}
						break;
					case 86:
						token2 = LT(1);
						match(86);
						expression = atom();
						if (0 == inputState.guessing)
						{
							token5 = token2;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 14:
						match(14);
						type = array_type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 77:
					case 83:
					case 85:
					case 91:
						break;
					}
					break;
				case 58:
				case 80:
				case 86:
					switch (LA(1))
					{
					case 58:
						parameterModifiers = parameter_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 80:
					case 86:
						break;
					}
					switch (LA(1))
					{
					case 80:
						token3 = LT(1);
						match(80);
						if (0 == inputState.guessing)
						{
							token5 = token3;
						}
						break;
					case 86:
						token4 = LT(1);
						match(86);
						expression = atom();
						if (0 == inputState.guessing)
						{
							token5 = token4;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					switch (LA(1))
					{
					case 14:
						match(14);
						type = type_reference();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 77:
					case 83:
					case 85:
					case 91:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					ParameterDeclaration parameterDeclaration = new ParameterDeclaration(ToLexicalInfo(token5));
					parameterDeclaration.Name = token5.getText();
					parameterDeclaration.Type = type;
					parameterDeclaration.Modifiers = parameterModifiers;
					AddAttributes(parameterDeclaration.Attributes);
					c.Add((expression != null) ? new SpliceParameterDeclaration(parameterDeclaration, expression) : parameterDeclaration);
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
				match(76);
				if (0 == inputState.guessing)
				{
					arrayTypeReference = new ArrayTypeReference(ToLexicalInfo(token));
				}
				typeReference = type_reference();
				if (0 == inputState.guessing)
				{
					arrayTypeReference.ElementType = typeReference;
				}
				switch (LA(1))
				{
				case 85:
					match(85);
					integerLiteralExpression = integer_literal();
					if (0 == inputState.guessing)
					{
						arrayTypeReference.Rank = integerLiteralExpression;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 77:
					break;
				}
				token2 = LT(1);
				match(77);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 17:
				case 19:
				case 58:
				case 65:
				case 76:
				case 80:
				case 81:
				case 86:
					flag = callable_parameter_declaration(c);
					while (true)
					{
						bool flag2 = true;
						if (LA(1) == 85 && !flag)
						{
							match(85);
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
				case 77:
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
					recover(ex, tokenSet_50_);
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
				case 81:
					match(81);
					if (0 == inputState.guessing)
					{
						result = true;
					}
					typeReference = type_reference();
					break;
				case 17:
				case 19:
				case 58:
				case 65:
				case 76:
				case 80:
				case 86:
					switch (LA(1))
					{
					case 58:
						parameterModifiers = parameter_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 17:
					case 19:
					case 65:
					case 76:
					case 80:
					case 86:
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
				recover(ex, tokenSet_85_);
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
				case 20:
					match(20);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.ReferenceType;
					}
					break;
				case 64:
					match(64);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.ValueType;
					}
					break;
				case 21:
					match(21);
					if (0 == inputState.guessing)
					{
						gpd.Constraints |= GenericParameterConstraints.Constructable;
					}
					break;
				case 17:
				case 19:
				case 65:
				case 76:
				case 80:
				case 86:
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
				case 85:
					match(85);
					generic_parameter_constraints(gpd);
					break;
				case 77:
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
					recover(ex, tokenSet_50_);
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
				match(17);
				match(76);
				if (0 == inputState.guessing)
				{
					callableTypeReference = new CallableTypeReference(ToLexicalInfo(token));
					c = callableTypeReference.Parameters;
				}
				callable_parameter_declaration_list(c);
				match(77);
				if (LA(1) == 14 && tokenSet_43_.member(LA(2)))
				{
					match(14);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						callableTypeReference.ReturnType = typeReference;
					}
				}
				else if (!tokenSet_41_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
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
				recover(ex, tokenSet_41_);
			}
			return callableTypeReference;
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
				case 105:
					token = LT(1);
					match(105);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 112:
				case 116:
					break;
				}
				switch (LA(1))
				{
				case 116:
					token2 = LT(1);
					match(116);
					if (0 == inputState.guessing)
					{
						text = ((token != null) ? (token.getText() + token2.getText()) : token2.getText());
						result = PrimitiveParser.ParseIntegerLiteralExpression(token2, text, asLong: false);
					}
					break;
				case 112:
					token3 = LT(1);
					match(112);
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
				recover(ex, tokenSet_41_);
			}
			return result;
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
					if (LA(1) == 85)
					{
						match(85);
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
					recover(ex, tokenSet_38_);
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
				match(86);
				expression = atom();
				if (0 == inputState.guessing)
				{
					result = new SpliceTypeReference(ToLexicalInfo(token), expression);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 65:
				case 80:
					result = identifier();
					break;
				case 17:
					token = LT(1);
					match(17);
					if (0 == inputState.guessing)
					{
						result = token;
					}
					break;
				case 19:
					token2 = LT(1);
					match(19);
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
				recover(ex, tokenSet_41_);
			}
			return result;
		}

		protected void single_line_block(Block b)
		{
			IToken token = null;
			StatementCollection container = b?.Statements;
			IToken token2 = null;
			try
			{
				match(88);
				switch (LA(1))
				{
				case 52:
					match(52);
					break;
				case 8:
				case 15:
				case 16:
				case 18:
				case 19:
				case 35:
				case 37:
				case 48:
				case 57:
				case 59:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					simple_stmt(container);
					while (true)
					{
						bool flag = true;
						if (LA(1) == 75)
						{
							match(75);
							switch (LA(1))
							{
							case 8:
							case 15:
							case 16:
							case 18:
							case 19:
							case 35:
							case 37:
							case 48:
							case 57:
							case 59:
							case 61:
							case 62:
							case 65:
							case 68:
							case 69:
							case 73:
							case 74:
							case 76:
							case 78:
							case 79:
							case 80:
							case 81:
							case 82:
							case 86:
							case 87:
							case 88:
							case 92:
							case 94:
							case 105:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
								simple_stmt(container);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 9:
							case 75:
								break;
							}
							continue;
						}
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				int num = 0;
				while (true)
				{
					bool flag = true;
					if (LA(1) != 9 || !tokenSet_47_.member(LA(2)))
					{
						break;
					}
					token = LT(1);
					match(9);
					if (0 == inputState.guessing)
					{
						token2 = token;
					}
					num++;
				}
				if (num < 1)
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					SetEndSourceLocation(b, token2);
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

		protected void simple_stmt(StatementCollection container)
		{
			Statement statement = null;
			_compact = true;
			try
			{
				if ((LA(1) == 65 || LA(1) == 80) && tokenSet_24_.member(LA(2)) && IsValidMacroArgument(LA(2)))
				{
					statement = closure_macro_stmt();
				}
				else
				{
					bool flag = false;
					if (tokenSet_34_.member(LA(1)) && tokenSet_86_.member(LA(2)))
					{
						int pos = mark();
						flag = true;
						inputState.guessing++;
						try
						{
							slicing_expression();
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
						statement = assignment_or_method_invocation();
					}
					else if (LA(1) == 59)
					{
						statement = return_expression_stmt();
					}
					else
					{
						bool flag2 = false;
						if (LA(1) == 80 && (LA(2) == 14 || LA(2) == 85))
						{
							int pos2 = mark();
							flag2 = true;
							inputState.guessing++;
							try
							{
								declaration();
								match(85);
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
							statement = unpack();
						}
						else if (LA(1) == 80 && LA(2) == 14)
						{
							statement = declaration_stmt();
						}
						else
						{
							if (!tokenSet_80_.member(LA(1)) || !tokenSet_87_.member(LA(2)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							switch (LA(1))
							{
							case 37:
								statement = goto_stmt();
								break;
							case 88:
								statement = label_stmt();
								break;
							case 73:
								statement = yield_stmt();
								break;
							case 15:
								statement = break_stmt();
								break;
							case 16:
								statement = continue_stmt();
								break;
							case 57:
								statement = raise_stmt();
								break;
							case 8:
							case 18:
							case 19:
							case 35:
							case 48:
							case 61:
							case 62:
							case 65:
							case 68:
							case 69:
							case 74:
							case 76:
							case 78:
							case 79:
							case 80:
							case 81:
							case 82:
							case 86:
							case 87:
							case 92:
							case 94:
							case 105:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
								statement = expression_stmt();
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
						}
					}
				}
				if (0 == inputState.guessing && null != statement)
				{
					container.Add(statement);
				}
				if (0 == inputState.guessing)
				{
					_compact = false;
				}
			}
			catch (RecognitionException ex3)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex3);
					recover(ex3, tokenSet_19_);
					return;
				}
				throw ex3;
			}
		}

		protected MacroStatement closure_macro_stmt()
		{
			MacroStatement result = null;
			IToken token = null;
			MacroStatement macroStatement = new MacroStatement();
			try
			{
				token = macro_name();
				expression_list(macroStatement.Arguments);
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
				recover(ex, tokenSet_88_);
			}
			return result;
		}

		protected void macro_block(StatementCollection container)
		{
			try
			{
				switch (LA(1))
				{
				case 9:
				case 75:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 8:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 52:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				switch (LA(1))
				{
				case 52:
					match(52);
					eos();
					break;
				case 8:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				{
					int num = 0;
					while (true)
					{
						bool flag = true;
						if (tokenSet_17_.member(LA(1)) && tokenSet_89_.member(LA(2)))
						{
							stmt(container);
						}
						else
						{
							if (!tokenSet_35_.member(LA(1)) || !tokenSet_36_.member(LA(2)))
							{
								break;
							}
							type_member_stmt(container);
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
					recover(ex, tokenSet_65_);
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
				type_definition_member(typeMemberCollection);
				if (0 != inputState.guessing)
				{
					return;
				}
				foreach (TypeMember item in typeMemberCollection)
				{
					container.Add(new TypeMemberStatement(item));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_52_);
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
				if (LA(1) == 88 && tokenSet_78_.member(LA(2)))
				{
					single_line_block(b);
					return;
				}
				if (LA(1) == 88 && LA(2) == 4)
				{
					match(88);
					token = LT(1);
					match(4);
					if (0 == inputState.guessing)
					{
						b.LexicalInfo = ToLexicalInfo(token);
						container = b.Statements;
					}
					macro_block(container);
					end(b);
					return;
				}
				throw new NoViableAltException(LT(1), getFilename());
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_22_);
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
				case 43:
					token = LT(1);
					match(43);
					if (0 == inputState.guessing)
					{
						token4 = token;
						type = StatementModifierType.If;
					}
					break;
				case 70:
					token2 = LT(1);
					match(70);
					if (0 == inputState.guessing)
					{
						token4 = token2;
						type = StatementModifierType.Unless;
					}
					break;
				case 72:
					token3 = LT(1);
					match(72);
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
					statementModifier = new StatementModifier(ToLexicalInfo(token4));
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
				recover(ex, tokenSet_14_);
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
				match(37);
				token2 = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					result = new GotoStatement(ToLexicalInfo(token), new ReferenceExpression(ToLexicalInfo(token2), token2.getText()));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_21_);
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
				match(88);
				token2 = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					result = new LabelStatement(ToLexicalInfo(token), token2.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_21_);
			}
			return result;
		}

		protected Statement nested_function()
		{
			IToken token = null;
			IToken token2 = null;
			Statement result = null;
			BlockExpression blockExpression = null;
			Block b = null;
			TypeReference typeReference = null;
			try
			{
				token = LT(1);
				match(22);
				token2 = LT(1);
				match(80);
				if (0 == inputState.guessing)
				{
					blockExpression = new BlockExpression(ToLexicalInfo(token2));
					b = blockExpression.Body;
				}
				switch (LA(1))
				{
				case 76:
					match(76);
					parameter_declaration_list(blockExpression.Parameters);
					match(77);
					switch (LA(1))
					{
					case 14:
						match(14);
						typeReference = type_reference();
						if (0 == inputState.guessing)
						{
							blockExpression.ReturnType = typeReference;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 88:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
					break;
				}
				compound_stmt(b);
				if (0 == inputState.guessing)
				{
					string text = token2.getText();
					result = new DeclarationStatement(ToLexicalInfo(token), new Declaration(ToLexicalInfo(token2), text), blockExpression);
					blockExpression[BlockExpression.ClosureNameAnnotation] = text;
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

		protected ForStatement for_stmt()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			ForStatement forStatement = null;
			Expression expression = null;
			DeclarationCollection dc = null;
			Block b = null;
			try
			{
				token = LT(1);
				match(34);
				if (0 == inputState.guessing)
				{
					forStatement = new ForStatement(ToLexicalInfo(token));
					dc = forStatement.Declarations;
					b = forStatement.Block;
				}
				declaration_list(dc);
				match(44);
				expression = array_or_expression();
				if (0 == inputState.guessing)
				{
					forStatement.Iterator = expression;
				}
				compound_stmt(b);
				switch (LA(1))
				{
				case 50:
					token2 = LT(1);
					match(50);
					if (0 == inputState.guessing)
					{
						forStatement.OrBlock = new Block(ToLexicalInfo(token2));
					}
					compound_stmt(forStatement.OrBlock);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				if (LA(1) == 65 && LA(2) == 88)
				{
					token3 = LT(1);
					match(65);
					if (0 == inputState.guessing)
					{
						forStatement.ThenBlock = new Block(ToLexicalInfo(token3));
					}
					compound_stmt(forStatement.ThenBlock);
				}
				else if (!tokenSet_82_.member(LA(1)) || !tokenSet_48_.member(LA(2)))
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
				recover(ex, tokenSet_82_);
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
			try
			{
				token = LT(1);
				match(72);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					whileStatement = new WhileStatement(ToLexicalInfo(token));
					whileStatement.Condition = expression;
				}
				compound_stmt(whileStatement.Block);
				switch (LA(1))
				{
				case 50:
					token2 = LT(1);
					match(50);
					if (0 == inputState.guessing)
					{
						whileStatement.OrBlock = new Block(ToLexicalInfo(token2));
					}
					compound_stmt(whileStatement.OrBlock);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				if (LA(1) == 65 && LA(2) == 88)
				{
					token3 = LT(1);
					match(65);
					if (0 == inputState.guessing)
					{
						whileStatement.ThenBlock = new Block(ToLexicalInfo(token3));
					}
					compound_stmt(whileStatement.ThenBlock);
				}
				else if (!tokenSet_82_.member(LA(1)) || !tokenSet_48_.member(LA(2)))
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
				recover(ex, tokenSet_82_);
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
			try
			{
				token = LT(1);
				match(43);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					result = (ifStatement = new IfStatement(ToLexicalInfo(token)));
					ifStatement.Condition = expression;
					ifStatement.TrueBlock = new Block();
				}
				compound_stmt(ifStatement.TrueBlock);
				while (true)
				{
					bool flag = true;
					if (LA(1) == 25)
					{
						token2 = LT(1);
						match(25);
						expression = this.expression();
						if (0 == inputState.guessing)
						{
							ifStatement.FalseBlock = new Block();
							IfStatement ifStatement2 = new IfStatement(ToLexicalInfo(token2));
							ifStatement2.TrueBlock = new Block();
							ifStatement2.Condition = expression;
							ifStatement.FalseBlock.Add(ifStatement2);
							ifStatement = ifStatement2;
						}
						compound_stmt(ifStatement.TrueBlock);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 26:
					token3 = LT(1);
					match(26);
					if (0 == inputState.guessing)
					{
						ifStatement.FalseBlock = new Block(ToLexicalInfo(token3));
					}
					compound_stmt(ifStatement.FalseBlock);
					break;
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
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
				recover(ex, tokenSet_82_);
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
				match(70);
				expression = this.expression();
				if (0 == inputState.guessing)
				{
					unlessStatement = new UnlessStatement(ToLexicalInfo(token));
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
				recover(ex, tokenSet_82_);
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
			Block block2 = null;
			try
			{
				token = LT(1);
				match(66);
				if (0 == inputState.guessing)
				{
					tryStatement = new TryStatement(ToLexicalInfo(token));
				}
				compound_stmt(tryStatement.ProtectedBlock);
				while (true)
				{
					bool flag = true;
					if (LA(1) == 30)
					{
						exception_handler(tryStatement);
						continue;
					}
					break;
				}
				switch (LA(1))
				{
				case 31:
					token2 = LT(1);
					match(31);
					if (0 == inputState.guessing)
					{
						block2 = new Block(ToLexicalInfo(token2));
					}
					compound_stmt(block2);
					if (0 == inputState.guessing)
					{
						tryStatement.FailureBlock = block2;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 27:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					break;
				}
				switch (LA(1))
				{
				case 27:
					token3 = LT(1);
					match(27);
					if (0 == inputState.guessing)
					{
						block2 = new Block(ToLexicalInfo(token3));
					}
					compound_stmt(block2);
					if (0 == inputState.guessing)
					{
						tryStatement.EnsureBlock = block2;
					}
					break;
				case 1:
				case 5:
				case 8:
				case 10:
				case 11:
				case 12:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 88:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
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
				recover(ex, tokenSet_82_);
			}
			return tryStatement;
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
				case 22:
				case 24:
				case 88:
					expression = method_invocation_block(expression);
					if (0 == inputState.guessing)
					{
						statement = new ExpressionStatement(expression);
					}
					break;
				case 84:
					token = LT(1);
					match(84);
					if (0 == inputState.guessing)
					{
						token2 = token;
						operator_ = OperatorParser.ParseAssignment(token.getText());
					}
					if (tokenSet_68_.member(LA(1)) && tokenSet_90_.member(LA(2)) && _compact)
					{
						expression2 = array_or_expression();
					}
					else
					{
						bool flag = false;
						if (LA(1) == 22 || LA(1) == 24 || LA(1) == 88)
						{
							int pos = mark();
							flag = true;
							inputState.guessing++;
							try
							{
								switch (LA(1))
								{
								case 88:
									match(88);
									break;
								case 22:
									match(22);
									break;
								case 24:
									match(24);
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
							expression2 = callable_expression();
						}
						else
						{
							if (!tokenSet_68_.member(LA(1)) || !tokenSet_91_.member(LA(2)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							expression2 = array_or_expression();
							switch (LA(1))
							{
							case 22:
							case 24:
							case 88:
								expression2 = method_invocation_block(expression2);
								break;
							case 43:
							case 70:
							case 72:
								modifier = stmt_modifier();
								eos();
								break;
							case 9:
							case 75:
								eos();
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							}
						}
					}
					if (0 == inputState.guessing)
					{
						statement = new ExpressionStatement(new BinaryExpression(ToLexicalInfo(token2), operator_, expression, expression2));
						statement.Modifier = modifier;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_82_);
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
				match(59);
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					e = array_or_expression();
					switch (LA(1))
					{
					case 22:
					case 24:
					case 88:
						e = method_invocation_block(e);
						break;
					case 9:
					case 43:
					case 70:
					case 72:
					case 75:
						switch (LA(1))
						{
						case 43:
						case 70:
						case 72:
							modifier = stmt_modifier();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 9:
						case 75:
							break;
						}
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				case 22:
				case 24:
				case 88:
					e = callable_expression();
					break;
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
					switch (LA(1))
					{
					case 43:
					case 70:
					case 72:
						modifier = stmt_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 9:
					case 75:
						break;
					}
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					returnStatement = new ReturnStatement(ToLexicalInfo(token));
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
				recover(ex, tokenSet_82_);
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
				match(80);
				switch (LA(1))
				{
				case 14:
					match(14);
					type = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 44:
				case 84:
				case 85:
					break;
				}
				if (0 == inputState.guessing)
				{
					declaration = new Declaration(ToLexicalInfo(token));
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
				recover(ex, tokenSet_92_);
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
				case 43:
				case 70:
				case 72:
					modifier = stmt_modifier();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 75:
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
				recover(ex, tokenSet_82_);
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
				match(80);
				match(14);
				typeReference = type_reference();
				switch (LA(1))
				{
				case 84:
					match(84);
					if (tokenSet_93_.member(LA(1)) && tokenSet_94_.member(LA(2)) && _compact)
					{
						initializer = simple_initializer();
						break;
					}
					if (tokenSet_93_.member(LA(1)) && tokenSet_95_.member(LA(2)))
					{
						initializer = declaration_initializer();
						break;
					}
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
					if (_compact)
					{
						throw new SemanticException("!_compact");
					}
					switch (LA(1))
					{
					case 43:
					case 70:
					case 72:
						modifier = stmt_modifier();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 9:
					case 75:
						break;
					}
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					Declaration declaration = new Declaration(ToLexicalInfo(token));
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
				recover(ex, tokenSet_22_);
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
				match(73);
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = array_or_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
				case 93:
				case 95:
					break;
				}
				if (0 == inputState.guessing)
				{
					yieldStatement = new YieldStatement(ToLexicalInfo(token));
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
				recover(ex, tokenSet_88_);
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
				match(15);
				if (0 == inputState.guessing)
				{
					result = new BreakStatement(ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_21_);
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
				match(16);
				if (0 == inputState.guessing)
				{
					result = new ContinueStatement(ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_21_);
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
				match(57);
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					exception = expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
				case 93:
				case 95:
					break;
				}
				if (0 == inputState.guessing)
				{
					raiseStatement = new RaiseStatement(ToLexicalInfo(token));
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
				recover(ex, tokenSet_88_);
			}
			return raiseStatement;
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
				recover(ex, tokenSet_21_);
			}
			return result;
		}

		protected Statement assignment_or_method_invocation()
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
				token = LT(1);
				match(84);
				if (0 == inputState.guessing)
				{
					token2 = token;
					operator_ = OperatorParser.ParseAssignment(token.getText());
				}
				expression2 = array_or_expression();
				if (0 == inputState.guessing)
				{
					statement = new ExpressionStatement(new BinaryExpression(ToLexicalInfo(token2), operator_, expression, expression2));
					statement.Modifier = modifier;
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_19_);
			}
			return statement;
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
				match(59);
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = array_or_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 9:
				case 43:
				case 70:
				case 72:
				case 75:
				case 93:
				case 95:
					break;
				}
				if ((LA(1) == 43 || LA(1) == 70 || LA(1) == 72) && !_compact)
				{
					modifier = stmt_modifier();
				}
				else if (!tokenSet_96_.member(LA(1)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing)
				{
					returnStatement = new ReturnStatement(ToLexicalInfo(token));
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
				recover(ex, tokenSet_96_);
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
				match(85);
				if (0 == inputState.guessing)
				{
					unpackStatement.Declarations.Add(declaration);
				}
				switch (LA(1))
				{
				case 80:
					declaration_list(unpackStatement.Declarations);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 84:
					break;
				}
				token = LT(1);
				match(84);
				expression = array_or_expression();
				if (0 == inputState.guessing)
				{
					unpackStatement.Expression = expression;
					unpackStatement.LexicalInfo = ToLexicalInfo(token);
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
			return unpackStatement;
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
					if (LA(1) == 50)
					{
						token = LT(1);
						match(50);
						expression2 = boolean_term();
						if (0 == inputState.guessing)
						{
							BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token));
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
				recover(ex, tokenSet_97_);
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
				case 22:
				case 24:
				case 88:
					result = callable_expression();
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
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
				case 58:
					parameter_modifier();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 80:
					break;
				}
				match(80);
				switch (LA(1))
				{
				case 14:
					match(14);
					type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 85:
				case 91:
					break;
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 85)
					{
						match(85);
						match(80);
						switch (LA(1))
						{
						case 14:
							match(14);
							type_reference();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 85:
						case 91:
							break;
						}
						continue;
					}
					break;
				}
				match(91);
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
				case 59:
					statement = return_expression_stmt();
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 57:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					switch (LA(1))
					{
					case 57:
						statement = raise_stmt();
						break;
					case 73:
						statement = yield_stmt();
						break;
					default:
					{
						bool flag = false;
						if (LA(1) == 80 && (LA(2) == 14 || LA(2) == 85))
						{
							int pos = mark();
							flag = true;
							inputState.guessing++;
							try
							{
								declaration();
								match(85);
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
						if ((LA(1) == 65 || LA(1) == 80) && tokenSet_98_.member(LA(2)) && IsValidClosureMacroArgument(LA(2)))
						{
							statement = closure_macro_stmt();
							break;
						}
						if (tokenSet_68_.member(LA(1)) && tokenSet_99_.member(LA(2)))
						{
							statement = closure_expression_stmt();
							break;
						}
						throw new NoViableAltException(LT(1), getFilename());
					}
					}
					switch (LA(1))
					{
					case 43:
					case 70:
					case 72:
						statementModifier = stmt_modifier();
						if (0 == inputState.guessing)
						{
							statement.Modifier = statementModifier;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 9:
					case 75:
					case 93:
					case 95:
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
					recover(ex2, tokenSet_96_);
					return;
				}
				throw ex2;
			}
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
				recover(ex, tokenSet_88_);
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
				match(92);
				if (0 == inputState.guessing)
				{
					result = (blockExpression = new BlockExpression(ToLexicalInfo(token)));
					blockExpression.Annotate("inline");
					c = blockExpression.Parameters;
					block = blockExpression.Body;
				}
				bool flag = false;
				if (tokenSet_100_.member(LA(1)) && tokenSet_101_.member(LA(2)))
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
					match(91);
				}
				else if (!tokenSet_102_.member(LA(1)) || !tokenSet_103_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				internal_closure_stmt(block);
				while (true)
				{
					bool flag2 = true;
					if (LA(1) == 9 || LA(1) == 75)
					{
						eos();
						switch (LA(1))
						{
						case 8:
						case 18:
						case 19:
						case 35:
						case 47:
						case 48:
						case 57:
						case 59:
						case 61:
						case 62:
						case 65:
						case 68:
						case 69:
						case 73:
						case 74:
						case 76:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 85:
						case 86:
						case 87:
						case 92:
						case 94:
						case 105:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
						case 120:
						case 121:
							internal_closure_stmt(block);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 9:
						case 75:
						case 93:
							break;
						}
						continue;
					}
					break;
				}
				token2 = LT(1);
				match(93);
				if (0 == inputState.guessing)
				{
					block.EndSourceLocation = SourceLocationFactory.ToEndSourceLocation(token2);
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_41_);
			}
			return result;
		}

		protected void exception_handler(TryStatement t)
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			ExceptionHandler exceptionHandler = null;
			TypeReference typeReference = null;
			Expression expression = null;
			try
			{
				token = LT(1);
				match(30);
				switch (LA(1))
				{
				case 80:
					token2 = LT(1);
					match(80);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 14:
				case 43:
				case 70:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 14:
					match(14);
					typeReference = type_reference();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 43:
				case 70:
				case 88:
					break;
				}
				switch (LA(1))
				{
				case 43:
				case 70:
					switch (LA(1))
					{
					case 43:
						match(43);
						break;
					case 70:
						token3 = LT(1);
						match(70);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = boolean_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 88:
					break;
				}
				if (0 == inputState.guessing)
				{
					exceptionHandler = new ExceptionHandler(ToLexicalInfo(token));
					exceptionHandler.Declaration = new Declaration();
					exceptionHandler.Declaration.Type = typeReference;
					if (token2 != null)
					{
						exceptionHandler.Declaration.LexicalInfo = ToLexicalInfo(token2);
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
							UnaryExpression unaryExpression = new UnaryExpression(ToLexicalInfo(token3));
							unaryExpression.Operator = UnaryOperatorType.LogicalNot;
							unaryExpression.Operand = expression;
							expression = unaryExpression;
						}
						exceptionHandler.FilterCondition = expression;
						exceptionHandler.Flags |= ExceptionHandlerFlags.Filter;
					}
				}
				compound_stmt(exceptionHandler.Block);
				if (0 == inputState.guessing)
				{
					t.ExceptionHandlers.Add(exceptionHandler);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 == inputState.guessing)
				{
					reportError(ex);
					recover(ex, tokenSet_104_);
					return;
				}
				throw ex;
			}
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
				case 84:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
					switch (LA(1))
					{
					case 84:
						token = LT(1);
						match(84);
						if (0 == inputState.guessing)
						{
							token7 = token;
							@operator = OperatorParser.ParseAssignment(token.getText());
						}
						break;
					case 96:
						token2 = LT(1);
						match(96);
						if (0 == inputState.guessing)
						{
							token7 = token2;
							@operator = BinaryOperatorType.InPlaceBitwiseOr;
						}
						break;
					case 97:
						token3 = LT(1);
						match(97);
						if (0 == inputState.guessing)
						{
							token7 = token3;
							@operator = BinaryOperatorType.InPlaceExclusiveOr;
						}
						break;
					case 98:
						token4 = LT(1);
						match(98);
						if (0 == inputState.guessing)
						{
							token7 = token4;
							@operator = BinaryOperatorType.InPlaceBitwiseAnd;
						}
						break;
					case 99:
						token5 = LT(1);
						match(99);
						if (0 == inputState.guessing)
						{
							token7 = token5;
							@operator = BinaryOperatorType.InPlaceShiftLeft;
						}
						break;
					case 100:
						token6 = LT(1);
						match(100);
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
						BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token7));
						binaryExpression.Operator = @operator;
						binaryExpression.Left = expression;
						binaryExpression.Right = expression2;
						expression = binaryExpression;
					}
					break;
				case 1:
				case 5:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 22:
				case 24:
				case 26:
				case 28:
				case 29:
				case 32:
				case 34:
				case 35:
				case 37:
				case 39:
				case 40:
				case 43:
				case 46:
				case 48:
				case 50:
				case 51:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 59:
				case 61:
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
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 85:
				case 86:
				case 87:
				case 88:
				case 92:
				case 93:
				case 94:
				case 95:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
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
				recover(ex, tokenSet_105_);
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
					if (LA(1) == 85)
					{
						match(85);
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
					recover(ex, tokenSet_106_);
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
				match(44);
				expression = boolean_expression();
				if (0 == inputState.guessing)
				{
					ge.Iterator = expression;
				}
				if ((LA(1) == 43 || LA(1) == 70 || LA(1) == 72) && tokenSet_5_.member(LA(2)))
				{
					statementModifier = stmt_modifier();
					if (0 == inputState.guessing)
					{
						ge.Filter = statementModifier;
					}
				}
				else if (!tokenSet_14_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
				{
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
					if (LA(1) == 13)
					{
						token = LT(1);
						match(13);
						expression2 = not_expression();
						if (0 == inputState.guessing)
						{
							BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token));
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
				recover(ex, tokenSet_107_);
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
				case 47:
					token = LT(1);
					match(47);
					expression = not_expression();
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = assignment_expression();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (0 == inputState.guessing && token != null)
				{
					UnaryExpression unaryExpression = new UnaryExpression(ToLexicalInfo(token));
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
				recover(ex, tokenSet_105_);
			}
			return expression;
		}

		public QuasiquoteExpression ast_literal_expression()
		{
			IToken token = null;
			IToken token2 = null;
			QuasiquoteExpression quasiquoteExpression = null;
			try
			{
				token = LT(1);
				match(94);
				if (0 == inputState.guessing)
				{
					quasiquoteExpression = new QuasiquoteExpression(ToLexicalInfo(token));
				}
				switch (LA(1))
				{
				case 4:
					match(4);
					ast_literal_block(quasiquoteExpression);
					match(5);
					switch (LA(1))
					{
					case 9:
					case 75:
						eos();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 95:
						break;
					}
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 38:
				case 47:
				case 48:
				case 57:
				case 59:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 73:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 85:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					ast_literal_closure(quasiquoteExpression);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				token2 = LT(1);
				match(95);
				if (0 == inputState.guessing)
				{
					SetEndSourceLocation(quasiquoteExpression, token2);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				if (tokenSet_108_.member(LA(1)) && tokenSet_109_.member(LA(2)))
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
				if (tokenSet_35_.member(LA(1)) && tokenSet_36_.member(LA(2)))
				{
					int pos2 = mark();
					flag2 = true;
					inputState.guessing++;
					try
					{
						attributes();
						if (tokenSet_31_.member(LA(1)))
						{
							type_member_modifier();
						}
						else
						{
							if (!tokenSet_110_.member(LA(1)))
							{
								throw new NoViableAltException(LT(1), getFilename());
							}
							modifiers();
							switch (LA(1))
							{
							case 20:
								match(20);
								break;
							case 28:
								match(28);
								break;
							case 64:
								match(64);
								break;
							case 39:
								match(39);
								break;
							case 29:
								match(29);
								break;
							case 22:
								match(22);
								break;
							case 17:
								match(17);
								break;
							case 80:
							case 86:
								switch (LA(1))
								{
								case 80:
									match(80);
									break;
								case 86:
									splice_expression();
									break;
								default:
									throw new NoViableAltException(LT(1), getFilename());
								}
								switch (LA(1))
								{
								case 14:
									match(14);
									type_reference();
									break;
								default:
									throw new NoViableAltException(LT(1), getFilename());
								case 88:
									break;
								}
								begin_with_doc(null);
								switch (LA(1))
								{
								case 36:
									match(36);
									break;
								case 60:
									match(60);
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
						if (!tokenSet_35_.member(LA(1)))
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
				if (tokenSet_17_.member(LA(1)) && tokenSet_89_.member(LA(2)))
				{
					int num2 = 0;
					while (true)
					{
						bool flag3 = true;
						if (!tokenSet_17_.member(LA(1)))
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
					recover(ex3, tokenSet_65_);
					return;
				}
				throw ex3;
			}
		}

		public void ast_literal_closure(QuasiquoteExpression e)
		{
			IToken token = null;
			Block block = null;
			Node node = null;
			try
			{
				bool flag = false;
				if (tokenSet_5_.member(LA(1)) && tokenSet_111_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						expression();
						switch (LA(1))
						{
						case 88:
							match(88);
							break;
						case 95:
							match(95);
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
					node = expression();
					if (0 == inputState.guessing)
					{
						e.Node = node;
					}
					switch (LA(1))
					{
					case 88:
						token = LT(1);
						match(88);
						node = expression();
						if (0 == inputState.guessing)
						{
							e.Node = new ExpressionPair(ToLexicalInfo(token), (Expression)e.Node, (Expression)node);
						}
						break;
					case 95:
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					return;
				}
				if (LA(1) == 38)
				{
					node = import_directive_();
					if (0 == inputState.guessing)
					{
						e.Node = node;
					}
					return;
				}
				if (tokenSet_102_.member(LA(1)) && tokenSet_99_.member(LA(2)))
				{
					if (0 == inputState.guessing)
					{
						block = new Block();
					}
					internal_closure_stmt(block);
					while (true)
					{
						bool flag2 = true;
						if (LA(1) == 9 || LA(1) == 75)
						{
							eos();
							switch (LA(1))
							{
							case 8:
							case 18:
							case 19:
							case 35:
							case 47:
							case 48:
							case 57:
							case 59:
							case 61:
							case 62:
							case 65:
							case 68:
							case 69:
							case 73:
							case 74:
							case 76:
							case 78:
							case 79:
							case 80:
							case 81:
							case 82:
							case 85:
							case 86:
							case 87:
							case 92:
							case 94:
							case 105:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
								internal_closure_stmt(block);
								break;
							default:
								throw new NoViableAltException(LT(1), getFilename());
							case 9:
							case 75:
							case 95:
								break;
							}
							continue;
						}
						break;
					}
					if (0 == inputState.guessing)
					{
						e.Node = block;
						if (block.Statements.Count == 1)
						{
							e.Node = block.Statements[0];
						}
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
					recover(ex2, tokenSet_112_);
					return;
				}
				throw ex2;
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
					recover(ex, tokenSet_65_);
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
				case 9:
				case 75:
					eos();
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 38:
				case 45:
					break;
				}
				switch (LA(1))
				{
				case 45:
					match(45);
					break;
				case 38:
					match(38);
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
					if (!tokenSet_113_.member(LA(1)))
					{
						break;
					}
					switch (LA(1))
					{
					case 41:
					case 44:
					case 47:
					case 101:
					case 102:
					case 103:
						switch (LA(1))
						{
						case 101:
							token = LT(1);
							match(101);
							if (0 == inputState.guessing)
							{
								@operator = OperatorParser.ParseComparison(token.getText());
								token9 = token;
							}
							break;
						case 102:
							token2 = LT(1);
							match(102);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.GreaterThan;
								token9 = token2;
							}
							break;
						case 103:
							token3 = LT(1);
							match(103);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.LessThan;
								token9 = token3;
							}
							break;
						case 47:
							token6 = LT(1);
							match(47);
							match(44);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.NotMember;
								token9 = token6;
							}
							break;
						case 44:
							token7 = LT(1);
							match(44);
							if (0 == inputState.guessing)
							{
								@operator = BinaryOperatorType.Member;
								token9 = token7;
							}
							break;
						default:
							if (LA(1) == 41 && LA(2) == 47)
							{
								token4 = LT(1);
								match(41);
								match(47);
								if (0 == inputState.guessing)
								{
									@operator = BinaryOperatorType.ReferenceInequality;
									token9 = token4;
								}
								break;
							}
							if (LA(1) == 41 && tokenSet_114_.member(LA(2)))
							{
								token5 = LT(1);
								match(41);
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
					case 42:
						token8 = LT(1);
						match(42);
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
						BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token9));
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
				recover(ex, tokenSet_115_);
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
					if (!tokenSet_116_.member(LA(1)) || !tokenSet_114_.member(LA(2)))
					{
						break;
					}
					switch (LA(1))
					{
					case 104:
						token = LT(1);
						match(104);
						if (0 == inputState.guessing)
						{
							token5 = token;
							@operator = BinaryOperatorType.Addition;
						}
						break;
					case 105:
						token2 = LT(1);
						match(105);
						if (0 == inputState.guessing)
						{
							token5 = token2;
							@operator = BinaryOperatorType.Subtraction;
						}
						break;
					case 91:
						token3 = LT(1);
						match(91);
						if (0 == inputState.guessing)
						{
							token5 = token3;
							@operator = BinaryOperatorType.BitwiseOr;
						}
						break;
					case 106:
						token4 = LT(1);
						match(106);
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
						BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token5));
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
				recover(ex, tokenSet_117_);
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
					if (!tokenSet_118_.member(LA(1)) || !tokenSet_114_.member(LA(2)))
					{
						break;
					}
					switch (LA(1))
					{
					case 81:
						token = LT(1);
						match(81);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Multiply;
							token5 = token;
						}
						break;
					case 107:
						token2 = LT(1);
						match(107);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Division;
							token5 = token2;
						}
						break;
					case 108:
						token3 = LT(1);
						match(108);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.Modulus;
							token5 = token3;
						}
						break;
					case 109:
						token4 = LT(1);
						match(109);
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
						BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token5));
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
				recover(ex, tokenSet_119_);
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
					if (LA(1) != 110 && LA(1) != 111)
					{
						break;
					}
					switch (LA(1))
					{
					case 110:
						token = LT(1);
						match(110);
						if (0 == inputState.guessing)
						{
							@operator = BinaryOperatorType.ShiftLeft;
							token3 = token;
						}
						break;
					case 111:
						token2 = LT(1);
						match(111);
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
						BinaryExpression binaryExpression = new BinaryExpression(ToLexicalInfo(token3));
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
				recover(ex, tokenSet_120_);
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
				if (LA(1) == 14)
				{
					token = LT(1);
					match(14);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						TryCastExpression tryCastExpression = new TryCastExpression(ToLexicalInfo(token));
						tryCastExpression.Target = expression;
						tryCastExpression.Type = typeReference;
						expression = tryCastExpression;
					}
				}
				else if (LA(1) == 18 && tokenSet_43_.member(LA(2)))
				{
					token2 = LT(1);
					match(18);
					typeReference = type_reference();
					if (0 == inputState.guessing)
					{
						CastExpression castExpression = new CastExpression(ToLexicalInfo(token2));
						castExpression.Target = expression;
						castExpression.Type = typeReference;
						expression = castExpression;
					}
				}
				else if (!tokenSet_121_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				while (true)
				{
					bool flag = true;
					if (LA(1) == 90 && tokenSet_114_.member(LA(2)))
					{
						token3 = LT(1);
						match(90);
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
				recover(ex, tokenSet_121_);
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
				if ((LA(1) == 105 || LA(1) == 112 || LA(1) == 116) && tokenSet_74_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						match(105);
						match(112);
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
				else if (tokenSet_122_.member(LA(1)) && tokenSet_114_.member(LA(2)))
				{
					switch (LA(1))
					{
					case 105:
						token = LT(1);
						match(105);
						if (0 == inputState.guessing)
						{
							token8 = token;
							@operator = UnaryOperatorType.UnaryNegation;
						}
						break;
					case 113:
						token2 = LT(1);
						match(113);
						if (0 == inputState.guessing)
						{
							token8 = token2;
							@operator = UnaryOperatorType.Increment;
						}
						break;
					case 114:
						token3 = LT(1);
						match(114);
						if (0 == inputState.guessing)
						{
							token8 = token3;
							@operator = UnaryOperatorType.Decrement;
						}
						break;
					case 115:
						token4 = LT(1);
						match(115);
						if (0 == inputState.guessing)
						{
							token8 = token4;
							@operator = UnaryOperatorType.OnesComplement;
						}
						break;
					case 81:
						token5 = LT(1);
						match(81);
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
					if (!tokenSet_34_.member(LA(1)) || !tokenSet_123_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					expression = slicing_expression();
					if (LA(1) == 113 && tokenSet_74_.member(LA(2)))
					{
						token6 = LT(1);
						match(113);
						if (0 == inputState.guessing)
						{
							token8 = token6;
							@operator = UnaryOperatorType.PostIncrement;
						}
					}
					else if (LA(1) == 114 && tokenSet_74_.member(LA(2)))
					{
						token7 = LT(1);
						match(114);
						if (0 == inputState.guessing)
						{
							token8 = token7;
							@operator = UnaryOperatorType.PostDecrement;
						}
					}
					else if (!tokenSet_74_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
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
				recover(ex2, tokenSet_74_);
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
				case 8:
				case 74:
				case 78:
				case 79:
				case 117:
					result = string_literal();
					break;
				case 82:
					result = list_literal();
					break;
				case 94:
					result = ast_literal_expression();
					break;
				case 118:
					result = re_literal();
					break;
				case 35:
				case 68:
					result = bool_literal();
					break;
				case 48:
					result = null_literal();
					break;
				case 61:
					result = self_literal();
					break;
				case 62:
					result = super_literal();
					break;
				default:
				{
					if ((LA(1) == 105 || LA(1) == 112 || LA(1) == 116) && tokenSet_41_.member(LA(2)))
					{
						result = integer_literal();
						break;
					}
					bool flag = false;
					if (LA(1) == 92 && tokenSet_72_.member(LA(2)))
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
					if (LA(1) == 92 && tokenSet_124_.member(LA(2)))
					{
						result = closure_expression();
						break;
					}
					if ((LA(1) == 105 || LA(1) == 119 || LA(1) == 120) && tokenSet_41_.member(LA(2)))
					{
						result = double_literal();
						break;
					}
					if ((LA(1) == 105 || LA(1) == 121) && tokenSet_41_.member(LA(2)))
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
				recover(ex2, tokenSet_41_);
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
				match(19);
				match(76);
				switch (LA(1))
				{
				case 79:
					token2 = LT(1);
					match(79);
					if (0 == inputState.guessing)
					{
						result = new CharLiteralExpression(ToLexicalInfo(token2), token2.getText());
					}
					break;
				case 116:
					token3 = LT(1);
					match(116);
					if (0 == inputState.guessing)
					{
						result = new CharLiteralExpression(ToLexicalInfo(token3), (char)PrimitiveParser.ParseInt(token3));
					}
					break;
				case 77:
					if (0 == inputState.guessing)
					{
						result = new MethodInvocationExpression(ToLexicalInfo(token), new ReferenceExpression(ToLexicalInfo(token), token.getText()));
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				match(77);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
			}
			return result;
		}

		protected ReferenceExpression reference_expression()
		{
			IToken token = null;
			ReferenceExpression result = null;
			IToken token2 = null;
			try
			{
				switch (LA(1))
				{
				case 65:
				case 80:
					token2 = macro_name();
					break;
				case 19:
					token = LT(1);
					match(19);
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
					result = new ReferenceExpression(ToLexicalInfo(token2), token2.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
			}
			return result;
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
				if (LA(1) == 76 && LA(2) == 49)
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						match(76);
						match(49);
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
					if (LA(1) != 76 || !tokenSet_68_.member(LA(2)))
					{
						throw new NoViableAltException(LT(1), getFilename());
					}
					token = LT(1);
					match(76);
					expression = array_or_expression();
					switch (LA(1))
					{
					case 43:
						match(43);
						expression2 = boolean_expression();
						match(26);
						expression3 = array_or_expression();
						if (0 == inputState.guessing)
						{
							ConditionalExpression conditionalExpression = new ConditionalExpression(ToLexicalInfo(token));
							conditionalExpression.Condition = expression2;
							conditionalExpression.TrueValue = expression;
							conditionalExpression.FalseValue = expression3;
							expression = conditionalExpression;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 77:
						break;
					}
					match(77);
				}
			}
			catch (RecognitionException ex2)
			{
				if (0 != inputState.guessing)
				{
					throw ex2;
				}
				reportError(ex2);
				recover(ex2, tokenSet_41_);
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
				match(18);
				match(76);
				typeReference = type_reference();
				match(85);
				expression = this.expression();
				match(77);
				if (0 == inputState.guessing)
				{
					result = new CastExpression(ToLexicalInfo(token), expression, typeReference);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(69);
				match(76);
				typeReference = type_reference();
				match(77);
				if (0 == inputState.guessing)
				{
					result = new TypeofExpression(ToLexicalInfo(token), typeReference);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(87);
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
				recover(ex, tokenSet_41_);
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
				match(76);
				match(49);
				typeReference = type_reference();
				match(88);
				if (0 == inputState.guessing)
				{
					result = (arrayLiteralExpression = new ArrayLiteralExpression(ToLexicalInfo(token)));
					arrayLiteralExpression.Type = new ArrayTypeReference(typeReference.LexicalInfo, typeReference);
				}
				switch (LA(1))
				{
				case 85:
					match(85);
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						arrayLiteralExpression.Items.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85 && tokenSet_5_.member(LA(2)))
						{
							match(85);
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
					case 85:
						match(85);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 77:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				}
				match(77);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 88:
					match(88);
					if (0 == inputState.guessing)
					{
						expression = OmittedExpression.Default;
					}
					switch (LA(1))
					{
					case 8:
					case 18:
					case 19:
					case 35:
					case 47:
					case 48:
					case 61:
					case 62:
					case 65:
					case 68:
					case 69:
					case 74:
					case 76:
					case 78:
					case 79:
					case 80:
					case 81:
					case 82:
					case 86:
					case 87:
					case 92:
					case 94:
					case 105:
					case 112:
					case 113:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
						expression2 = this.expression();
						break;
					case 88:
						match(88);
						if (0 == inputState.guessing)
						{
							expression2 = OmittedExpression.Default;
						}
						step = this.expression();
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 83:
					case 85:
						break;
					}
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = this.expression();
					switch (LA(1))
					{
					case 88:
						match(88);
						switch (LA(1))
						{
						case 8:
						case 18:
						case 19:
						case 35:
						case 47:
						case 48:
						case 61:
						case 62:
						case 65:
						case 68:
						case 69:
						case 74:
						case 76:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 86:
						case 87:
						case 92:
						case 94:
						case 105:
						case 112:
						case 113:
						case 114:
						case 115:
						case 116:
						case 117:
						case 118:
						case 119:
						case 120:
						case 121:
							expression2 = this.expression();
							break;
						case 83:
						case 85:
						case 88:
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
						case 88:
							match(88);
							step = this.expression();
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 83:
						case 85:
							break;
						}
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 83:
					case 85:
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
					recover(ex, tokenSet_49_);
					return;
				}
				throw ex;
			}
		}

		protected void argument(INodeWithArguments node)
		{
			Expression expression = null;
			ExpressionPair expressionPair = null;
			try
			{
				bool flag = false;
				if (tokenSet_5_.member(LA(1)) && tokenSet_125_.member(LA(2)))
				{
					int pos = mark();
					flag = true;
					inputState.guessing++;
					try
					{
						expression_pair();
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
					expressionPair = expression_pair();
					if (0 == inputState.guessing && expressionPair != null)
					{
						node.NamedArguments.Add(expressionPair);
					}
					return;
				}
				if (tokenSet_5_.member(LA(1)) && tokenSet_126_.member(LA(2)))
				{
					expression = this.expression();
					if (0 == inputState.guessing && expression != null)
					{
						node.Arguments.Add(expression);
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
					recover(ex2, tokenSet_85_);
					return;
				}
				throw ex2;
			}
		}

		protected void hash_literal_test()
		{
			try
			{
				match(92);
				switch (LA(1))
				{
				case 93:
					match(93);
					break;
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression();
					match(88);
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
				match(92);
				if (0 == inputState.guessing)
				{
					hashLiteralExpression = new HashLiteralExpression(ToLexicalInfo(token));
				}
				switch (LA(1))
				{
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expressionPair = expression_pair();
					if (0 == inputState.guessing)
					{
						hashLiteralExpression.Items.Add(expressionPair);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85 && tokenSet_5_.member(LA(2)))
						{
							match(85);
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
					case 85:
						match(85);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 93:
						break;
					}
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 93:
					break;
				}
				match(93);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(92);
				if (0 == inputState.guessing)
				{
					listLiteralExpression = new ListLiteralExpression(ToLexicalInfo(token));
					items = listLiteralExpression.Items;
				}
				list_items(items);
				match(93);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_73_);
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
				case 8:
				case 18:
				case 19:
				case 35:
				case 47:
				case 48:
				case 61:
				case 62:
				case 65:
				case 68:
				case 69:
				case 74:
				case 76:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 86:
				case 87:
				case 92:
				case 94:
				case 105:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
					expression = this.expression();
					if (0 == inputState.guessing)
					{
						items.Add(expression);
					}
					while (true)
					{
						bool flag = true;
						if (LA(1) == 85 && tokenSet_5_.member(LA(2)))
						{
							match(85);
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
					case 85:
						match(85);
						break;
					case 83:
					case 93:
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					}
					break;
				case 83:
				case 93:
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
					recover(ex, tokenSet_127_);
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
				case 8:
					expression = expression_interpolation();
					break;
				case 78:
					token = LT(1);
					match(78);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(ToLexicalInfo(token), token.getText());
						expression.Annotate("quote", "\"");
					}
					break;
				case 79:
					token2 = LT(1);
					match(79);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(ToLexicalInfo(token2), token2.getText());
						expression.Annotate("quote", "'");
					}
					break;
				case 74:
					token3 = LT(1);
					match(74);
					if (0 == inputState.guessing)
					{
						expression = new StringLiteralExpression(ToLexicalInfo(token3), token3.getText());
						expression.Annotate("quote", "\"\"\"");
					}
					break;
				case 117:
					token4 = LT(1);
					match(117);
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
				recover(ex, tokenSet_41_);
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
				match(82);
				if (0 == inputState.guessing)
				{
					listLiteralExpression = new ListLiteralExpression(ToLexicalInfo(token));
					items = listLiteralExpression.Items;
				}
				list_items(items);
				match(83);
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(118);
				if (0 == inputState.guessing)
				{
					result = new RELiteralExpression(ToLexicalInfo(token), token.getText());
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 68:
					token = LT(1);
					match(68);
					if (0 == inputState.guessing)
					{
						boolLiteralExpression = new BoolLiteralExpression(ToLexicalInfo(token));
						boolLiteralExpression.Value = true;
					}
					break;
				case 35:
					token2 = LT(1);
					match(35);
					if (0 == inputState.guessing)
					{
						boolLiteralExpression = new BoolLiteralExpression(ToLexicalInfo(token2));
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
				recover(ex, tokenSet_41_);
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
				match(48);
				if (0 == inputState.guessing)
				{
					result = new NullLiteralExpression(ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(61);
				if (0 == inputState.guessing)
				{
					result = new SelfLiteralExpression(ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				match(62);
				if (0 == inputState.guessing)
				{
					result = new SuperLiteralExpression(ToLexicalInfo(token));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
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
				case 105:
				case 119:
					switch (LA(1))
					{
					case 105:
						token = LT(1);
						match(105);
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
						result = new DoubleLiteralExpression(ToLexicalInfo(token2), PrimitiveParser.ParseDouble(token2, text));
					}
					break;
				case 120:
					token3 = LT(1);
					match(120);
					if (0 == inputState.guessing)
					{
						string text = token3.getText();
						text = text.Substring(0, text.Length - 1);
						if (token != null)
						{
							text = token.getText() + text;
						}
						result = new DoubleLiteralExpression(ToLexicalInfo(token3), PrimitiveParser.ParseDouble(token3, text, isSingle: true), isSingle: true);
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
				recover(ex, tokenSet_41_);
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
				case 105:
					token = LT(1);
					match(105);
					break;
				default:
					throw new NoViableAltException(LT(1), getFilename());
				case 121:
					break;
				}
				token2 = LT(1);
				match(121);
				if (0 == inputState.guessing)
				{
					string text = token2.getText();
					if (token != null)
					{
						text = token.getText() + text;
					}
					result = new TimeSpanLiteralExpression(ToLexicalInfo(token2), PrimitiveParser.ParseTimeSpan(token2, text));
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_41_);
			}
			return result;
		}

		protected ExpressionInterpolationExpression expression_interpolation()
		{
			IToken token = null;
			IToken token2 = null;
			IToken token3 = null;
			IToken token4 = null;
			IToken token5 = null;
			IToken token6 = null;
			ExpressionInterpolationExpression expressionInterpolationExpression = null;
			Expression expression = null;
			LexicalInfo lexicalInfo = null;
			try
			{
				if (LA(1) == 8 && LA(2) == 8)
				{
					token = LT(1);
					match(8);
				}
				else if (LA(1) != 8 || !tokenSet_5_.member(LA(2)))
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				int num = 0;
				while (true)
				{
					bool flag = true;
					if (LA(1) != 8 || !tokenSet_5_.member(LA(2)))
					{
						break;
					}
					token2 = LT(1);
					match(8);
					if (0 == inputState.guessing && lexicalInfo == null)
					{
						lexicalInfo = ToLexicalInfo(token2);
						expressionInterpolationExpression = new ExpressionInterpolationExpression(lexicalInfo);
					}
					expression = this.expression();
					switch (LA(1))
					{
					case 80:
					case 88:
						switch (LA(1))
						{
						case 88:
							token3 = LT(1);
							match(88);
							break;
						default:
							throw new NoViableAltException(LT(1), getFilename());
						case 80:
							break;
						}
						token4 = LT(1);
						match(80);
						break;
					default:
						throw new NoViableAltException(LT(1), getFilename());
					case 8:
						break;
					}
					if (0 == inputState.guessing && null != expression)
					{
						expressionInterpolationExpression.Expressions.Add(expression);
						if (null != token4)
						{
							expression.Annotate("formatString", token4.getText());
						}
					}
					token5 = LT(1);
					match(8);
					num++;
				}
				if (num < 1)
				{
					throw new NoViableAltException(LT(1), getFilename());
				}
				if (LA(1) == 8 && tokenSet_41_.member(LA(2)))
				{
					token6 = LT(1);
					match(8);
				}
				else if (!tokenSet_41_.member(LA(1)) || !tokenSet_15_.member(LA(2)))
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
				recover(ex, tokenSet_41_);
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
				match(88);
				expression2 = this.expression();
				if (0 == inputState.guessing)
				{
					result = new ExpressionPair(ToLexicalInfo(token), expression, expression2);
				}
			}
			catch (RecognitionException ex)
			{
				if (0 != inputState.guessing)
				{
					throw ex;
				}
				reportError(ex);
				recover(ex, tokenSet_128_);
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
			return new long[4] { -1447509329730101470L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_2_()
		{
			return new long[4] { -5629502990328014L, 288230376118149119L, 0L, 0L };
		}

		private static long[] mk_tokenSet_3_()
		{
			return new long[4] { -5664695952351438L, 288230376118149119L, 0L, 0L };
		}

		private static long[] mk_tokenSet_4_()
		{
			return new long[4] { 6917960070559695616L, 287951101570309490L, 0L, 0L };
		}

		private static long[] mk_tokenSet_5_()
		{
			return new long[4] { 6917951274466672896L, 287951101553529906L, 0L, 0L };
		}

		private static long[] mk_tokenSet_6_()
		{
			return new long[4] { -9085940225639575552L, 262281L, 0L, 0L };
		}

		private static long[] mk_tokenSet_7_()
		{
			return new long[4] { -7644788275614183424L, 4784267L, 0L, 0L };
		}

		private static long[] mk_tokenSet_8_()
		{
			return new long[3] { 34L, 0L, 0L };
		}

		private static long[] mk_tokenSet_9_()
		{
			return new long[4] { -587139236503758L, 287951104256761855L, 0L, 0L };
		}

		private static long[] mk_tokenSet_10_()
		{
			return new long[4] { -1447509329193230542L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_11_()
		{
			return new long[4] { -8398L, 288230376118149119L, 0L, 0L };
		}

		private static long[] mk_tokenSet_12_()
		{
			return new long[4] { -1447544514102190302L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_13_()
		{
			return new long[4] { -1445690736876978398L, 288230376151711743L, 0L, 0L };
		}

		private static long[] mk_tokenSet_14_()
		{
			return new long[4] { -1447544797016383710L, 287951104257294335L, 0L, 0L };
		}

		private static long[] mk_tokenSet_15_()
		{
			return new long[4] { -35184372089038L, 288230376151711743L, 0L, 0L };
		}

		private static long[] mk_tokenSet_16_()
		{
			return new long[4] { -1447544797570031838L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_17_()
		{
			return new long[4] { 7638395428073734400L, 287951101570307958L, 0L, 0L };
		}

		private static long[] mk_tokenSet_18_()
		{
			return new long[3] { 3106L, 0L, 0L };
		}

		private static long[] mk_tokenSet_19_()
		{
			return new long[4] { 512L, 2048L, 0L, 0L };
		}

		private static long[] mk_tokenSet_20_()
		{
			return new long[4] { 7642890214424019216L, 287951101570307634L, 0L, 0L };
		}

		private static long[] mk_tokenSet_21_()
		{
			return new long[4] { 8796093022720L, 2368L, 0L, 0L };
		}

		private static long[] mk_tokenSet_22_()
		{
			return new long[4] { -1447544797033160926L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_23_()
		{
			return new long[4] { 512L, 2147485696L, 0L, 0L };
		}

		private static long[] mk_tokenSet_24_()
		{
			return new long[4] { 6917951274466673408L, 287951101553531954L, 0L, 0L };
		}

		private static long[] mk_tokenSet_25_()
		{
			return new long[4] { -4538787482829006L, 288230373970665471L, 0L, 0L };
		}

		private static long[] mk_tokenSet_26_()
		{
			return new long[4] { 283467858432L, 2147489792L, 0L, 0L };
		}

		private static long[] mk_tokenSet_27_()
		{
			return new long[4] { 1495196245054980096L, 65536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_28_()
		{
			return new long[4] { 8589951488L, 2147485696L, 0L, 0L };
		}

		private static long[] mk_tokenSet_29_()
		{
			return new long[4] { 8796093022720L, 2701142336L, 0L, 0L };
		}

		private static long[] mk_tokenSet_30_()
		{
			return new long[4] { -5338945266410958848L, 21168267L, 0L, 0L };
		}

		private static long[] mk_tokenSet_31_()
		{
			return new long[4] { -9085940775669198848L, 136L, 0L, 0L };
		}

		private static long[] mk_tokenSet_32_()
		{
			return new long[4] { 3458765133106511872L, 4259843L, 0L, 0L };
		}

		private static long[] mk_tokenSet_33_()
		{
			return new long[4] { 1495196245054980096L, 4259840L, 0L, 0L };
		}

		private static long[] mk_tokenSet_34_()
		{
			return new long[4] { 6917810536978317568L, 284010451879449650L, 0L, 0L };
		}

		private static long[] mk_tokenSet_35_()
		{
			return new long[4] { -6780097215889010688L, 4522123L, 0L, 0L };
		}

		private static long[] mk_tokenSet_36_()
		{
			return new long[4] { -726828205054471424L, 287951101571882491L, 0L, 0L };
		}

		private static long[] mk_tokenSet_37_()
		{
			return new long[4] { 2305843009754759168L, 327680L, 0L, 0L };
		}

		private static long[] mk_tokenSet_38_()
		{
			return new long[4] { 0L, 524288L, 0L, 0L };
		}

		private static long[] mk_tokenSet_39_()
		{
			return new long[4] { 0L, 134750208L, 0L, 0L };
		}

		private static long[] mk_tokenSet_40_()
		{
			return new long[4] { 655360L, 65538L, 0L, 0L };
		}

		private static long[] mk_tokenSet_41_()
		{
			return new long[4] { -1445691020344819934L, 288230376151711743L, 0L, 0L };
		}

		private static long[] mk_tokenSet_42_()
		{
			return new long[4] { 562949954076672L, 4395010L, 0L, 0L };
		}

		private static long[] mk_tokenSet_43_()
		{
			return new long[4] { 655360L, 4263938L, 0L, 0L };
		}

		private static long[] mk_tokenSet_44_()
		{
			return new long[4] { -290119624079469824L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_45_()
		{
			return new long[4] { 32L, 4521984L, 0L, 0L };
		}

		private static long[] mk_tokenSet_46_()
		{
			return new long[4] { -6780097215889010656L, 4522123L, 0L, 0L };
		}

		private static long[] mk_tokenSet_47_()
		{
			return new long[4] { -293497320343888094L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_48_()
		{
			return new long[4] { -4538792589402318L, 288230376118149119L, 0L, 0L };
		}

		private static long[] mk_tokenSet_49_()
		{
			return new long[4] { 0L, 2621440L, 0L, 0L };
		}

		private static long[] mk_tokenSet_50_()
		{
			return new long[4] { 0L, 8192L, 0L, 0L };
		}

		private static long[] mk_tokenSet_51_()
		{
			return new long[4] { 0L, 16777216L, 0L, 0L };
		}

		private static long[] mk_tokenSet_52_()
		{
			return new long[4] { -1447544797033164512L, 287951101570308095L, 0L, 0L };
		}

		private static long[] mk_tokenSet_53_()
		{
			return new long[4] { 6917810536978333952L, 284010451896226866L, 0L, 0L };
		}

		private static long[] mk_tokenSet_54_()
		{
			return new long[4] { -7933019202342875136L, 262280L, 0L, 0L };
		}

		private static long[] mk_tokenSet_55_()
		{
			return new long[4] { 6917810536978334464L, 284010451880500274L, 0L, 0L };
		}

		private static long[] mk_tokenSet_56_()
		{
			return new long[4] { 2305843009754759200L, 327680L, 0L, 0L };
		}

		private static long[] mk_tokenSet_57_()
		{
			return new long[4] { 3801088L, 4263939L, 0L, 0L };
		}

		private static long[] mk_tokenSet_58_()
		{
			return new long[4] { 288230376151711744L, 21442560L, 0L, 0L };
		}

		private static long[] mk_tokenSet_59_()
		{
			return new long[4] { 0L, 2625536L, 0L, 0L };
		}

		private static long[] mk_tokenSet_60_()
		{
			return new long[4] { 3458764583081083424L, 329728L, 0L, 0L };
		}

		private static long[] mk_tokenSet_61_()
		{
			return new long[4] { 1152921573326323744L, 262144L, 0L, 0L };
		}

		private static long[] mk_tokenSet_62_()
		{
			return new long[3] { 4503599627370496L, 0L, 0L };
		}

		private static long[] mk_tokenSet_63_()
		{
			return new long[4] { 3801039254268674048L, 4259840L, 0L, 0L };
		}

		private static long[] mk_tokenSet_64_()
		{
			return new long[4] { 7642899027701105408L, 287951101570310006L, 0L, 0L };
		}

		private static long[] mk_tokenSet_65_()
		{
			return new long[3] { 32L, 0L, 0L };
		}

		private static long[] mk_tokenSet_66_()
		{
			return new long[4] { -7933019202342875104L, 262280L, 0L, 0L };
		}

		private static long[] mk_tokenSet_67_()
		{
			return new long[4] { 9134286684753232144L, 287951102244017714L, 0L, 0L };
		}

		private static long[] mk_tokenSet_68_()
		{
			return new long[4] { 6917951274466672896L, 287951101555627058L, 0L, 0L };
		}

		private static long[] mk_tokenSet_69_()
		{
			return new long[4] { 9135436791074808592L, 288230373953887794L, 0L, 0L };
		}

		private static long[] mk_tokenSet_70_()
		{
			return new long[4] { 6918514224420094208L, 287951101570307122L, 0L, 0L };
		}

		private static long[] mk_tokenSet_71_()
		{
			return new long[4] { 6917951274466672896L, 287951101553538098L, 0L, 0L };
		}

		private static long[] mk_tokenSet_72_()
		{
			return new long[4] { 6917951274466672896L, 287951102090400818L, 0L, 0L };
		}

		private static long[] mk_tokenSet_73_()
		{
			return new long[4] { -1445691020344819934L, 288230376118157311L, 0L, 0L };
		}

		private static long[] mk_tokenSet_74_()
		{
			return new long[4] { -1446253970298241246L, 288230376118157311L, 0L, 0L };
		}

		private static long[] mk_tokenSet_75_()
		{
			return new long[4] { -4538796055986382L, 288230376118157311L, 0L, 0L };
		}

		private static long[] mk_tokenSet_76_()
		{
			return new long[4] { -1447544797016383710L, 287951104254672895L, 0L, 0L };
		}

		private static long[] mk_tokenSet_77_()
		{
			return new long[4] { -35192962023630L, 288230376151711743L, 0L, 0L };
		}

		private static long[] mk_tokenSet_78_()
		{
			return new long[4] { 7642890214424019200L, 287951101570307634L, 0L, 0L };
		}

		private static long[] mk_tokenSet_79_()
		{
			return new long[4] { 9134286684753232144L, 287951102245066290L, 0L, 0L };
		}

		private static long[] mk_tokenSet_80_()
		{
			return new long[4] { 7061925862493225216L, 287951101570307634L, 0L, 0L };
		}

		private static long[] mk_tokenSet_81_()
		{
			return new long[4] { 9134319670081110800L, 288230373953888114L, 0L, 0L };
		}

		private static long[] mk_tokenSet_82_()
		{
			return new long[4] { -1447544797033161438L, 287951101570308095L, 0L, 0L };
		}

		private static long[] mk_tokenSet_83_()
		{
			return new long[4] { -5627175642562686976L, 4259979L, 0L, 0L };
		}

		private static long[] mk_tokenSet_84_()
		{
			return new long[4] { 0L, 136847360L, 0L, 0L };
		}

		private static long[] mk_tokenSet_85_()
		{
			return new long[4] { 0L, 2105344L, 0L, 0L };
		}

		private static long[] mk_tokenSet_86_()
		{
			return new long[4] { 9134286684732260624L, 287951102228289074L, 0L, 0L };
		}

		private static long[] mk_tokenSet_87_()
		{
			return new long[4] { 9134310873988088592L, 288230373953887794L, 0L, 0L };
		}

		private static long[] mk_tokenSet_88_()
		{
			return new long[4] { 8796093022720L, 2684356928L, 0L, 0L };
		}

		private static long[] mk_tokenSet_89_()
		{
			return new long[4] { 9134319670102082320L, 288230373970665330L, 0L, 0L };
		}

		private static long[] mk_tokenSet_90_()
		{
			return new long[4] { -4538796072764110L, 288230373970663423L, 0L, 0L };
		}

		private static long[] mk_tokenSet_91_()
		{
			return new long[4] { 9135445587188802320L, 288230373970665330L, 0L, 0L };
		}

		private static long[] mk_tokenSet_92_()
		{
			return new long[4] { 17592186044416L, 3145728L, 0L, 0L };
		}

		private static long[] mk_tokenSet_93_()
		{
			return new long[4] { 6917951274487644416L, 287951101572404274L, 0L, 0L };
		}

		private static long[] mk_tokenSet_94_()
		{
			return new long[4] { -35196445393102L, 288230373970665471L, 0L, 0L };
		}

		private static long[] mk_tokenSet_95_()
		{
			return new long[4] { 9139940528162202384L, 288230373970665010L, 0L, 0L };
		}

		private static long[] mk_tokenSet_96_()
		{
			return new long[4] { 512L, 2684356608L, 0L, 0L };
		}

		private static long[] mk_tokenSet_97_()
		{
			return new long[4] { -1447544796949274846L, 287951104257294335L, 0L, 0L };
		}

		private static long[] mk_tokenSet_98_()
		{
			return new long[4] { 6917960070559695616L, 287951104237886834L, 0L, 0L };
		}

		private static long[] mk_tokenSet_99_()
		{
			return new long[4] { 9135445587167830800L, 288230376101371762L, 0L, 0L };
		}

		private static long[] mk_tokenSet_100_()
		{
			return new long[4] { 288230376151711744L, 138870784L, 0L, 0L };
		}

		private static long[] mk_tokenSet_101_()
		{
			return new long[4] { 7638527214845968640L, 287951101690369594L, 0L, 0L };
		}

		private static long[] mk_tokenSet_102_()
		{
			return new long[4] { 7638527214845952256L, 287951101555627570L, 0L, 0L };
		}

		private static long[] mk_tokenSet_103_()
		{
			return new long[4] { 9135445587167830800L, 288230373953888114L, 0L, 0L };
		}

		private static long[] mk_tokenSet_104_()
		{
			return new long[4] { -1447544793677718238L, 287951101570308095L, 0L, 0L };
		}

		private static long[] mk_tokenSet_105_()
		{
			return new long[4] { -1446418897042424030L, 287951104257294335L, 0L, 0L };
		}

		private static long[] mk_tokenSet_106_()
		{
			return new long[4] { 17592186044416L, 1048576L, 0L, 0L };
		}

		private static long[] mk_tokenSet_107_()
		{
			return new long[4] { -1446418897042432222L, 287951104257294335L, 0L, 0L };
		}

		private static long[] mk_tokenSet_108_()
		{
			return new long[4] { -1447509329730101472L, 287951101570310143L, 0L, 0L };
		}

		private static long[] mk_tokenSet_109_()
		{
			return new long[4] { -5629502990328016L, 288230376118149119L, 0L, 0L };
		}

		private static long[] mk_tokenSet_110_()
		{
			return new long[4] { -9085940225102704640L, 4259977L, 0L, 0L };
		}

		private static long[] mk_tokenSet_111_()
		{
			return new long[4] { 9135436791074808080L, 288230376118146610L, 0L, 0L };
		}

		private static long[] mk_tokenSet_112_()
		{
			return new long[4] { 0L, 2147483648L, 0L, 0L };
		}

		private static long[] mk_tokenSet_113_()
		{
			return new long[4] { 164926744166400L, 962072674304L, 0L, 0L };
		}

		private static long[] mk_tokenSet_114_()
		{
			return new long[4] { 6917810536978317568L, 287951101553529906L, 0L, 0L };
		}

		private static long[] mk_tokenSet_115_()
		{
			return new long[4] { -1446418897042424030L, 287951237402329087L, 0L, 0L };
		}

		private static long[] mk_tokenSet_116_()
		{
			return new long[4] { 0L, 7696715612160L, 0L, 0L };
		}

		private static long[] mk_tokenSet_117_()
		{
			return new long[4] { -1446253970298257630L, 287952199475003391L, 0L, 0L };
		}

		private static long[] mk_tokenSet_118_()
		{
			return new long[4] { 0L, 61572651286528L, 0L, 0L };
		}

		private static long[] mk_tokenSet_119_()
		{
			return new long[4] { -1446253970298257630L, 287957697167359999L, 0L, 0L };
		}

		private static long[] mk_tokenSet_120_()
		{
			return new long[4] { -1446253970298257630L, 288019269818515455L, 0L, 0L };
		}

		private static long[] mk_tokenSet_121_()
		{
			return new long[4] { -1446253970298257630L, 288230376118157311L, 0L, 0L };
		}

		private static long[] mk_tokenSet_122_()
		{
			return new long[4] { 0L, 3942848697335808L, 0L, 0L };
		}

		private static long[] mk_tokenSet_123_()
		{
			return new long[4] { -4538795988877518L, 288230376118157311L, 0L, 0L };
		}

		private static long[] mk_tokenSet_124_()
		{
			return new long[4] { 7926757590997664000L, 287951101689845298L, 0L, 0L };
		}

		private static long[] mk_tokenSet_125_()
		{
			return new long[4] { 9135436791074808080L, 288230373970662962L, 0L, 0L };
		}

		private static long[] mk_tokenSet_126_()
		{
			return new long[4] { 9135436791074808080L, 288230373953893938L, 0L, 0L };
		}

		private static long[] mk_tokenSet_127_()
		{
			return new long[4] { 0L, 537395200L, 0L, 0L };
		}

		private static long[] mk_tokenSet_128_()
		{
			return new long[4] { 0L, 538976256L, 0L, 0L };
		}
	}
}
