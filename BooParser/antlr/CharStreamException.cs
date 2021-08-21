using System;

namespace antlr
{
	[Serializable]
	public class CharStreamException : ANTLRException
	{
		public CharStreamException(string s)
			: base(s)
		{
		}
	}
}
