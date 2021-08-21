using System.Collections.Generic;
using System.IO;

namespace Boo.Lang.Compiler
{
	public class CompilerErrorCollection : List<CompilerError>
	{
		public override string ToString()
		{
			return ToString(verbose: false);
		}

		public string ToString(bool verbose)
		{
			StringWriter stringWriter = new StringWriter();
			using (IEnumerator<CompilerError> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompilerError current = enumerator.Current;
					stringWriter.WriteLine(current.ToString(verbose));
				}
			}
			return stringWriter.ToString();
		}
	}
}
