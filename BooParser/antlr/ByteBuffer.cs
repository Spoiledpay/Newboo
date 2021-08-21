using System;
using System.IO;

namespace antlr
{
	public class ByteBuffer : InputBuffer
	{
		private const int BUF_SIZE = 16;

		[NonSerialized]
		internal Stream input;

		private byte[] buf = new byte[16];

		public ByteBuffer(Stream input_)
		{
			input = input_;
		}

		public override void fill(int amount)
		{
			syncConsume();
			int num = amount + markerOffset - queue.Count;
			while (num > 0)
			{
				int num2 = input.Read(buf, 0, 16);
				for (int i = 0; i < num2; i++)
				{
					queue.Add((char)buf[i]);
				}
				if (num2 < 16)
				{
					queue.Add(CharScanner.EOF_CHAR);
					break;
				}
				num -= num2;
			}
		}
	}
}
