using System;
using System.IO;

namespace Boo.Lang.Compiler.IO
{
	[Serializable]
	public class ReaderInput : ICompilerInput
	{
		private string _name;

		private TextReader _reader;

		public string Name => _name;

		public ReaderInput(string name, TextReader reader)
		{
			if (null == name)
			{
				throw new ArgumentNullException("name");
			}
			if (null == reader)
			{
				throw new ArgumentNullException("reader");
			}
			_name = name;
			_reader = reader;
		}

		public TextReader Open()
		{
			return _reader;
		}
	}
}
