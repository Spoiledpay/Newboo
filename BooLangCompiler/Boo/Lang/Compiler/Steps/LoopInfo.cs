using System.Reflection.Emit;

namespace Boo.Lang.Compiler.Steps
{
	internal sealed class LoopInfo
	{
		public Label BreakLabel;

		public Label ContinueLabel;

		public int TryBlockDepth;

		public LoopInfo(Label breakLabel, Label continueLabel, int tryBlockDepth)
		{
			BreakLabel = breakLabel;
			ContinueLabel = continueLabel;
			TryBlockDepth = tryBlockDepth;
		}
	}
}
