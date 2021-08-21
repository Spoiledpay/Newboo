using System.Text;

namespace antlr
{
	public class ParseTreeToken : ParseTree
	{
		protected IToken token;

		public ParseTreeToken(IToken token)
		{
			this.token = token;
		}

		protected internal override int getLeftmostDerivation(StringBuilder buf, int step)
		{
			buf.Append(' ');
			buf.Append(ToString());
			return step;
		}

		public override string ToString()
		{
			if (token != null)
			{
				return token.getText();
			}
			return "<missing token>";
		}
	}
}
