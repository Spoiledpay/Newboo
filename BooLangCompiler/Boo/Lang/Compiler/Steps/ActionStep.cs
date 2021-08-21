using System;

namespace Boo.Lang.Compiler.Steps
{
	public class ActionStep : ICompilerStep, ICompilerComponent, IDisposable
	{
		private readonly Action _action;

		public ActionStep(Action action)
		{
			_action = action;
		}

		public void Run()
		{
			_action();
		}

		public void Initialize(CompilerContext context)
		{
		}

		public void Dispose()
		{
		}
	}
}
