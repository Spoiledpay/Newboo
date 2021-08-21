using System.IO;

namespace Boo.Lang.Compiler
{
	public interface ICompilerInput
	{
		string Name { get; }

		TextReader Open();
	}
}
