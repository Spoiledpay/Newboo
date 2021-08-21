namespace antlr
{
	public abstract class FileLineFormatter
	{
		private static FileLineFormatter formatter = new DefaultFileLineFormatter();

		public static FileLineFormatter getFormatter()
		{
			return formatter;
		}

		public static void setFormatter(FileLineFormatter f)
		{
			formatter = f;
		}

		public abstract string getFormatString(string fileName, int line, int column);
	}
}
