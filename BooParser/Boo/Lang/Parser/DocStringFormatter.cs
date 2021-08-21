namespace Boo.Lang.Parser
{
	public class DocStringFormatter
	{
		public static string Format(string s)
		{
			if (s.Length == 0)
			{
				return string.Empty;
			}
			s = s.Replace("\r\n", "\n");
			int num = s.Length;
			int num2 = 0;
			if ('\n' == s[0])
			{
				num2++;
				num--;
			}
			if ('\n' == s[s.Length - 1])
			{
				num--;
			}
			if (num > 0)
			{
				return s.Substring(num2, num);
			}
			return string.Empty;
		}
	}
}
