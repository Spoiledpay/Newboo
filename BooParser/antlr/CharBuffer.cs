using System;
using System.IO;

namespace antlr
{
	public class CharBuffer : InputBuffer
	{
		private const int BUF_SIZE = 16;

		[NonSerialized]
		internal TextReader input;

		private char[] buf = new char[16];

		public CharBuffer(TextReader input_)
		{
			input = input_;
		}

		public override void fill(int amount)
		{
			try
			{
				syncConsume();
				int num = amount + markerOffset - queue.Count;
				while (num > 0)
				{
					int num2 = input.Read(buf, 0, 16);
					for (int i = 0; i < num2; i++)
					{
						queue.Add(buf[i]);
					}
					if (num2 < 16)
					{
						queue.Add(CharScanner.EOF_CHAR);
						break;
					}
					num -= num2;
				}
			}
			catch (IOException io)
			{
				throw new CharStreamIOException(io);
			}
		}
	}
}
