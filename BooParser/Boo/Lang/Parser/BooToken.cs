using antlr;

namespace Boo.Lang.Parser
{
	public class BooToken : CommonToken
	{
		public class BooTokenCreator : TokenCreator
		{
			public override string TokenTypeName => typeof(BooToken).FullName;

			public override IToken Create()
			{
				return new BooToken();
			}
		}

		public static readonly TokenCreator TokenCreator = new BooTokenCreator();

		protected string _fname;

		public BooToken()
		{
		}

		public BooToken(int type, string text, string fname, int line, int column)
		{
			setType(type);
			setText(text);
			setFilename(fname);
			setLine(line);
			setColumn(column);
		}

		public override void setFilename(string name)
		{
			_fname = name;
		}

		public override string getFilename()
		{
			return _fname;
		}
	}
}
