using System;

namespace antlr
{
	[Serializable]
	public class ANTLRException : Exception
	{
		public ANTLRException()
		{
		}

		public ANTLRException(string s)
			: base(s)
		{
		}

		public ANTLRException(string s, Exception inner)
			: base(s, inner)
		{
		}
	}
}
