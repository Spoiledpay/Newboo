using System.Text;
using antlr.collections;

namespace antlr
{
	public class ParseTreeRule : ParseTree
	{
		public const int INVALID_ALT = -1;

		protected string ruleName;

		protected int altNumber;

		public ParseTreeRule(string ruleName)
			: this(ruleName, -1)
		{
		}

		public ParseTreeRule(string ruleName, int altNumber)
		{
			this.ruleName = ruleName;
			this.altNumber = altNumber;
		}

		public string getRuleName()
		{
			return ruleName;
		}

		protected internal override int getLeftmostDerivation(StringBuilder buf, int step)
		{
			int result = 0;
			if (step <= 0)
			{
				buf.Append(' ');
				buf.Append(ToString());
				return result;
			}
			AST aST = getFirstChild();
			result = 1;
			while (aST != null)
			{
				if (result >= step || aST is ParseTreeToken)
				{
					buf.Append(' ');
					buf.Append(aST.ToString());
				}
				else
				{
					int step2 = step - result;
					int leftmostDerivation = ((ParseTree)aST).getLeftmostDerivation(buf, step2);
					result += leftmostDerivation;
				}
				aST = aST.getNextSibling();
			}
			return result;
		}

		public override string ToString()
		{
			if (altNumber == -1)
			{
				return '<' + ruleName + '>';
			}
			return '<' + ruleName + "[" + altNumber + "]>";
		}
	}
}
