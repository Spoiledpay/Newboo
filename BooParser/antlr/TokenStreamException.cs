using System;

namespace antlr
{
	[Serializable]
	public class TokenStreamException : ANTLRException
	{
		public TokenStreamException()
		{
		}

		public TokenStreamException(string s)
			: base(s)
		{
		}
	}
}
