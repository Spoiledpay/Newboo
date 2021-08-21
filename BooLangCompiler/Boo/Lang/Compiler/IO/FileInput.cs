using System;
using System.IO;

namespace Boo.Lang.Compiler.IO
{
	[Serializable]
	public class FileInput : ICompilerInput
	{
		private readonly string _fname;

		public string Name => _fname;

		public FileInput(string fname)
		{
			if (null == fname)
			{
				throw new ArgumentNullException("fname");
			}
			_fname = fname;
		}

		public TextReader Open()
		{
			try
			{
				return File.OpenText(_fname);
			}
			catch (FileNotFoundException)
			{
				throw CompilerErrorFactory.FileNotFound(_fname);
			}
			catch (Exception error)
			{
				throw CompilerErrorFactory.InputError(_fname, error);
			}
		}
	}
}
