using System;
using System.Text;

namespace antlr
{
	[Serializable]
	public class NoViableAltForCharException : RecognitionException
	{
		public char foundChar;

		public override string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("unexpected char: ");
				if (foundChar >= ' ' && foundChar <= '~')
				{
					stringBuilder.Append('\'');
					stringBuilder.Append(foundChar);
					stringBuilder.Append('\'');
				}
				else
				{
					stringBuilder.Append("0x");
					int num = foundChar;
					stringBuilder.Append(num.ToString("X"));
				}
				return stringBuilder.ToString();
			}
		}

		public NoViableAltForCharException(char c, CharScanner scanner)
			: base("NoViableAlt", scanner.getFilename(), scanner.getLine(), scanner.getColumn())
		{
			foundChar = c;
		}

		public NoViableAltForCharException(char c, string fileName, int line, int column)
			: base("NoViableAlt", fileName, line, column)
		{
			foundChar = c;
		}
	}
}
