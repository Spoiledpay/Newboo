namespace antlr
{
	public class StringUtils
	{
		public static string stripBack(string s, char c)
		{
			while (s.Length > 0 && s[s.Length - 1] == c)
			{
				s = s.Substring(0, s.Length - 1);
			}
			return s;
		}

		public static string stripBack(string s, string remove)
		{
			bool flag;
			do
			{
				flag = false;
				foreach (char c in remove)
				{
					while (s.Length > 0 && s[s.Length - 1] == c)
					{
						flag = true;
						s = s.Substring(0, s.Length - 1);
					}
				}
			}
			while (flag);
			return s;
		}

		public static string stripFront(string s, char c)
		{
			while (s.Length > 0 && s[0] == c)
			{
				s = s.Substring(1);
			}
			return s;
		}

		public static string stripFront(string s, string remove)
		{
			bool flag;
			do
			{
				flag = false;
				foreach (char c in remove)
				{
					while (s.Length > 0 && s[0] == c)
					{
						flag = true;
						s = s.Substring(1);
					}
				}
			}
			while (flag);
			return s;
		}

		public static string stripFrontBack(string src, string head, string tail)
		{
			int num = src.IndexOf(head);
			int num2 = src.LastIndexOf(tail);
			if (num == -1 || num2 == -1)
			{
				return src;
			}
			return src.Substring(num + 1, num2 - (num + 1));
		}
	}
}
