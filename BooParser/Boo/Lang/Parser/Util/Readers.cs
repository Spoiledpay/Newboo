using System.IO;

namespace Boo.Lang.Parser.Util
{
	internal static class Readers
	{
		public static bool IsEmpty(TextReader reader)
		{
			return reader.Peek() == -1;
		}
	}
}
