using System;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractCompilerStep : AbstractCompilerComponent, ICompilerStep, ICompilerComponent, IDisposable
	{
		public abstract void Run();

		public virtual void Dispose()
		{
		}
	}
}
