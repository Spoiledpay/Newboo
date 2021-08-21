using System;

namespace antlr
{
	[Serializable]
	public class RecognitionException : ANTLRException
	{
		public string fileName;

		public int line;

		public int column;

		public RecognitionException()
			: base("parsing error")
		{
			fileName = null;
			line = -1;
			column = -1;
		}

		public RecognitionException(string s)
			: base(s)
		{
			fileName = null;
			line = -1;
			column = -1;
		}

		public RecognitionException(string s, string fileName_, int line_, int column_)
			: base(s)
		{
			fileName = fileName_;
			line = line_;
			column = column_;
		}

		public virtual string getFilename()
		{
			return fileName;
		}

		public virtual int getLine()
		{
			return line;
		}

		public virtual int getColumn()
		{
			return column;
		}

		[Obsolete("Replaced by Message property since version 2.7.0", true)]
		public virtual string getErrorMessage()
		{
			return Message;
		}

		public override string ToString()
		{
			return FileLineFormatter.getFormatter().getFormatString(fileName, line, column) + Message;
		}
	}
}
