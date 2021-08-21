using System.Text;

namespace Boo.Lang.Compiler.Services
{
	public class UniqueNameProvider
	{
		private int _localIndex;

		public virtual string GetUniqueName(params string[] components)
		{
			string text = "$" + ++_localIndex;
			if (components == null || components.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in components)
			{
				stringBuilder.Append("$");
				stringBuilder.Append(value);
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
	}
}
