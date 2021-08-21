using System;

namespace Boo.Lang.Compiler
{
	public class CompilerWarningCollection : List<CompilerWarning>
	{
		public event EventHandler<CompilerWarningEventArgs> Adding;

		public override List<CompilerWarning> Add(CompilerWarning warning)
		{
			return OnAdding(warning) ? base.Add(warning) : this;
		}

		protected bool OnAdding(CompilerWarning warning)
		{
			EventHandler<CompilerWarningEventArgs> adding = this.Adding;
			if (null == adding)
			{
				return true;
			}
			CompilerWarningEventArgs compilerWarningEventArgs = new CompilerWarningEventArgs(warning);
			adding(this, compilerWarningEventArgs);
			return !compilerWarningEventArgs.IsCancelled;
		}

		public override string ToString()
		{
			return Builtins.join(this, "\n");
		}
	}
}
