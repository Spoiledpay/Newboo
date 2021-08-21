using System;

namespace Boo.Lang.Compiler
{
	public interface ICompilerStep : ICompilerComponent, IDisposable
	{
		void Run();
	}
}
