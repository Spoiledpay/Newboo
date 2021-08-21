using System;

namespace antlr
{
	[Serializable]
	public class SemanticException : RecognitionException
	{
		public SemanticException(string s)
			: base(s)
		{
		}

		[Obsolete("Replaced by SemanticException(string, string, int, int) since version 2.7.2.6", false)]
		public SemanticException(string s, string fileName, int line)
			: this(s, fileName, line, -1)
		{
		}

		public SemanticException(string s, string fileName, int line, int column)
			: base(s, fileName, line, column)
		{
		}
	}
}
