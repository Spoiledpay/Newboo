using System.Text;

namespace antlr
{
	public class DefaultFileLineFormatter : FileLineFormatter
	{
		public override string getFormatString(string fileName, int line, int column)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (fileName != null)
			{
				stringBuilder.Append(fileName + ":");
			}
			if (line != -1)
			{
				if (fileName == null)
				{
					stringBuilder.Append("line ");
				}
				stringBuilder.Append(line);
				if (column != -1)
				{
					stringBuilder.Append(":" + column);
				}
				stringBuilder.Append(":");
			}
			stringBuilder.Append(" ");
			return stringBuilder.ToString();
		}
	}
}
