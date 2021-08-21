using System.Text;
using antlr.collections;

namespace antlr
{
	public abstract class ParseTree : BaseAST
	{
		public string getLeftmostDerivationStep(int step)
		{
			if (step <= 0)
			{
				return ToString();
			}
			StringBuilder stringBuilder = new StringBuilder(2000);
			getLeftmostDerivation(stringBuilder, step);
			return stringBuilder.ToString();
		}

		public string getLeftmostDerivation(int maxSteps)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			stringBuilder.Append("    " + ToString());
			stringBuilder.Append("\n");
			for (int i = 1; i < maxSteps; i++)
			{
				stringBuilder.Append(" =>");
				stringBuilder.Append(getLeftmostDerivationStep(i));
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		protected internal abstract int getLeftmostDerivation(StringBuilder buf, int step);

		public override void initialize(int i, string s)
		{
		}

		public override void initialize(AST ast)
		{
		}

		public override void initialize(IToken token)
		{
		}
	}
}
