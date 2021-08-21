using System;
using System.IO;

namespace Boo.Lang.Compiler.IO
{
	[Serializable]
	public class StringInput : ReaderInput
	{
		public StringInput(string name, string contents)
			: base(name, new StringReader(contents))
		{
		}
	}
}
