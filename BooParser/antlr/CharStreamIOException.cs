using System;
using System.IO;

namespace antlr
{
	[Serializable]
	public class CharStreamIOException : CharStreamException
	{
		public IOException io;

		public CharStreamIOException(IOException io)
			: base(io.Message)
		{
			this.io = io;
		}
	}
}
