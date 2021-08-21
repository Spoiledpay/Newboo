using System;
using System.Text;
using antlr.collections.impl;

namespace antlr
{
	[Serializable]
	public class MismatchedCharException : RecognitionException
	{
		public enum CharTypeEnum
		{
			CharType = 1,
			NotCharType,
			RangeType,
			NotRangeType,
			SetType,
			NotSetType
		}

		public CharTypeEnum mismatchType;

		public int foundChar;

		public int expecting;

		public int upper;

		public BitSet bset;

		public CharScanner scanner;

		public override string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				switch (mismatchType)
				{
				case CharTypeEnum.CharType:
					stringBuilder.Append("expecting ");
					appendCharName(stringBuilder, expecting);
					stringBuilder.Append(", found ");
					appendCharName(stringBuilder, foundChar);
					break;
				case CharTypeEnum.NotCharType:
					stringBuilder.Append("expecting anything but '");
					appendCharName(stringBuilder, expecting);
					stringBuilder.Append("'; got it anyway");
					break;
				case CharTypeEnum.RangeType:
				case CharTypeEnum.NotRangeType:
					stringBuilder.Append("expecting token ");
					if (mismatchType == CharTypeEnum.NotRangeType)
					{
						stringBuilder.Append("NOT ");
					}
					stringBuilder.Append("in range: ");
					appendCharName(stringBuilder, expecting);
					stringBuilder.Append("..");
					appendCharName(stringBuilder, upper);
					stringBuilder.Append(", found ");
					appendCharName(stringBuilder, foundChar);
					break;
				case CharTypeEnum.SetType:
				case CharTypeEnum.NotSetType:
				{
					stringBuilder.Append("expecting " + ((mismatchType == CharTypeEnum.NotSetType) ? "NOT " : "") + "one of (");
					int[] array = bset.toArray();
					for (int i = 0; i < array.Length; i++)
					{
						appendCharName(stringBuilder, array[i]);
					}
					stringBuilder.Append("), found ");
					appendCharName(stringBuilder, foundChar);
					break;
				}
				default:
					stringBuilder.Append(base.Message);
					break;
				}
				return stringBuilder.ToString();
			}
		}

		public MismatchedCharException()
			: base("Mismatched char")
		{
		}

		public MismatchedCharException(char c, char lower, char upper_, bool matchNot, CharScanner scanner_)
			: base("Mismatched char", scanner_.getFilename(), scanner_.getLine(), scanner_.getColumn())
		{
			mismatchType = (matchNot ? CharTypeEnum.NotRangeType : CharTypeEnum.RangeType);
			foundChar = c;
			expecting = lower;
			upper = upper_;
			scanner = scanner_;
		}

		public MismatchedCharException(char c, char expecting_, bool matchNot, CharScanner scanner_)
			: base("Mismatched char", scanner_.getFilename(), scanner_.getLine(), scanner_.getColumn())
		{
			mismatchType = ((!matchNot) ? CharTypeEnum.CharType : CharTypeEnum.NotCharType);
			foundChar = c;
			expecting = expecting_;
			scanner = scanner_;
		}

		public MismatchedCharException(char c, BitSet set_, bool matchNot, CharScanner scanner_)
			: base("Mismatched char", scanner_.getFilename(), scanner_.getLine(), scanner_.getColumn())
		{
			mismatchType = (matchNot ? CharTypeEnum.NotSetType : CharTypeEnum.SetType);
			foundChar = c;
			bset = set_;
			scanner = scanner_;
		}

		private void appendCharName(StringBuilder sb, int c)
		{
			switch (c)
			{
			case 65535:
				sb.Append("'<EOF>'");
				return;
			case 10:
				sb.Append("'\\n'");
				return;
			case 13:
				sb.Append("'\\r'");
				return;
			case 9:
				sb.Append("'\\t'");
				return;
			}
			sb.Append('\'');
			sb.Append((char)c);
			sb.Append('\'');
		}
	}
}
