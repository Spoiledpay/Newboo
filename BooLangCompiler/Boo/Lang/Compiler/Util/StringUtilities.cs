using System;

namespace Boo.Lang.Compiler.Util
{
	internal sealed class StringUtilities
	{
		public static string GetSoundex(string s)
		{
			if (s.Length < 2)
			{
				return null;
			}
			char[] array = "?0000".ToCharArray();
			string text = s.ToLowerInvariant();
			int length = text.Length;
			char c = ' ';
			int num = 1;
			array[0] = text[0];
			for (int i = 1; i < length; i++)
			{
				char c2 = text[i];
				char c3 = ' ';
				if (c2 == 'b' || c2 == 'f' || c2 == 'p' || c2 == 'v')
				{
					c3 = '1';
				}
				if (c2 == 'c' || c2 == 'g' || c2 == 'j' || c2 == 'k' || c2 == 'q' || c2 == 's' || c2 == 'x' || c2 == 'z')
				{
					c3 = '2';
				}
				if (c2 == 'd' || c2 == 't')
				{
					c3 = '3';
				}
				if (c2 == 'l')
				{
					c3 = '4';
				}
				if (c2 == 'm' || c2 == 'n')
				{
					c3 = '5';
				}
				if (c2 == 'r')
				{
					c3 = '6';
				}
				if (c3 == c)
				{
					continue;
				}
				c = c3;
				if (c3 != ' ')
				{
					array[num] = c3;
					num++;
					if (num > 4)
					{
						break;
					}
				}
			}
			return new string(array);
		}

		public static int GetDistance(string s, string t)
		{
			int length = s.Length;
			int length2 = t.Length;
			int[,] array = new int[length + 1, length2 + 1];
			if (length == 0)
			{
				return length2;
			}
			if (length2 == 0)
			{
				return length;
			}
			int num = 0;
			while (num <= length)
			{
				array[num, 0] = num++;
			}
			int num2 = 0;
			while (num2 <= length2)
			{
				array[0, num2] = num2++;
			}
			for (num = 1; num <= length; num++)
			{
				for (num2 = 1; num2 <= length2; num2++)
				{
					int num3 = ((!(t.Substring(num2 - 1, 1) == s.Substring(num - 1, 1))) ? 1 : 0);
					array[num, num2] = Math.Min(Math.Min(array[num - 1, num2] + 1, array[num, num2 - 1] + 1), array[num - 1, num2 - 1] + num3);
				}
			}
			return array[length, length2];
		}
	}
}
