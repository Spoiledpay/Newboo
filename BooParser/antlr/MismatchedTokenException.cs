using System;
using System.Text;
using antlr.collections;
using antlr.collections.impl;

namespace antlr
{
	[Serializable]
	public class MismatchedTokenException : RecognitionException
	{
		public enum TokenTypeEnum
		{
			TokenType = 1,
			NotTokenType,
			RangeType,
			NotRangeType,
			SetType,
			NotSetType
		}

		internal string[] tokenNames;

		public IToken token;

		public AST node;

		internal string tokenText = null;

		public TokenTypeEnum mismatchType;

		public int expecting;

		public int upper;

		public BitSet bset;

		public override string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				switch (mismatchType)
				{
				case TokenTypeEnum.TokenType:
					stringBuilder.Append("expecting " + tokenName(expecting) + ", found '" + tokenText + "'");
					break;
				case TokenTypeEnum.NotTokenType:
					stringBuilder.Append("expecting anything but " + tokenName(expecting) + "; got it anyway");
					break;
				case TokenTypeEnum.RangeType:
					stringBuilder.Append("expecting token in range: " + tokenName(expecting) + ".." + tokenName(upper) + ", found '" + tokenText + "'");
					break;
				case TokenTypeEnum.NotRangeType:
					stringBuilder.Append("expecting token NOT in range: " + tokenName(expecting) + ".." + tokenName(upper) + ", found '" + tokenText + "'");
					break;
				case TokenTypeEnum.SetType:
				case TokenTypeEnum.NotSetType:
				{
					stringBuilder.Append("expecting " + ((mismatchType == TokenTypeEnum.NotSetType) ? "NOT " : "") + "one of (");
					int[] array = bset.toArray();
					for (int i = 0; i < array.Length; i++)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(tokenName(array[i]));
					}
					stringBuilder.Append("), found '" + tokenText + "'");
					break;
				}
				default:
					stringBuilder.Append(base.Message);
					break;
				}
				return stringBuilder.ToString();
			}
		}

		public MismatchedTokenException()
			: base("Mismatched Token: expecting any AST node", "<AST>", -1, -1)
		{
		}

		public MismatchedTokenException(string[] tokenNames_, AST node_, int lower, int upper_, bool matchNot)
			: base("Mismatched Token", "<AST>", -1, -1)
		{
			tokenNames = tokenNames_;
			node = node_;
			if (node_ == null)
			{
				tokenText = "<empty tree>";
			}
			else
			{
				tokenText = node_.ToString();
			}
			mismatchType = (matchNot ? TokenTypeEnum.NotRangeType : TokenTypeEnum.RangeType);
			expecting = lower;
			upper = upper_;
		}

		public MismatchedTokenException(string[] tokenNames_, AST node_, int expecting_, bool matchNot)
			: base("Mismatched Token", "<AST>", -1, -1)
		{
			tokenNames = tokenNames_;
			node = node_;
			if (node_ == null)
			{
				tokenText = "<empty tree>";
			}
			else
			{
				tokenText = node_.ToString();
			}
			mismatchType = ((!matchNot) ? TokenTypeEnum.TokenType : TokenTypeEnum.NotTokenType);
			expecting = expecting_;
		}

		public MismatchedTokenException(string[] tokenNames_, AST node_, BitSet set_, bool matchNot)
			: base("Mismatched Token", "<AST>", -1, -1)
		{
			tokenNames = tokenNames_;
			node = node_;
			if (node_ == null)
			{
				tokenText = "<empty tree>";
			}
			else
			{
				tokenText = node_.ToString();
			}
			mismatchType = (matchNot ? TokenTypeEnum.NotSetType : TokenTypeEnum.SetType);
			bset = set_;
		}

		public MismatchedTokenException(string[] tokenNames_, IToken token_, int lower, int upper_, bool matchNot, string fileName_)
			: base("Mismatched Token", fileName_, token_.getLine(), token_.getColumn())
		{
			tokenNames = tokenNames_;
			token = token_;
			tokenText = token_.getText();
			mismatchType = (matchNot ? TokenTypeEnum.NotRangeType : TokenTypeEnum.RangeType);
			expecting = lower;
			upper = upper_;
		}

		public MismatchedTokenException(string[] tokenNames_, IToken token_, int expecting_, bool matchNot, string fileName_)
			: base("Mismatched Token", fileName_, token_.getLine(), token_.getColumn())
		{
			tokenNames = tokenNames_;
			token = token_;
			tokenText = token_.getText();
			mismatchType = ((!matchNot) ? TokenTypeEnum.TokenType : TokenTypeEnum.NotTokenType);
			expecting = expecting_;
		}

		public MismatchedTokenException(string[] tokenNames_, IToken token_, BitSet set_, bool matchNot, string fileName_)
			: base("Mismatched Token", fileName_, token_.getLine(), token_.getColumn())
		{
			tokenNames = tokenNames_;
			token = token_;
			tokenText = token_.getText();
			mismatchType = (matchNot ? TokenTypeEnum.NotSetType : TokenTypeEnum.SetType);
			bset = set_;
		}

		private string tokenName(int tokenType)
		{
			if (tokenType == 0)
			{
				return "<Set of tokens>";
			}
			if (tokenType < 0 || tokenType >= tokenNames.Length)
			{
				return "<" + tokenType + ">";
			}
			return tokenNames[tokenType];
		}
	}
}
