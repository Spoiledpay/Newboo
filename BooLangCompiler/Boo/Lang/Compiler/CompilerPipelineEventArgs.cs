using System;

namespace Boo.Lang.Compiler
{
	public class CompilerPipelineEventArgs : EventArgs
	{
		public readonly CompilerContext Context;

		public CompilerPipelineEventArgs(CompilerContext context)
		{
			Context = context;
		}
	}
}
