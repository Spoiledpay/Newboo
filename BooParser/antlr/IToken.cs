namespace antlr
{
	public interface IToken
	{
		int Type { get; set; }

		int getColumn();

		void setColumn(int c);

		int getLine();

		void setLine(int l);

		string getFilename();

		void setFilename(string name);

		string getText();

		void setText(string t);
	}
}
