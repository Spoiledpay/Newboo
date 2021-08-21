namespace Boo.Lang.Compiler
{
	public class CompilerWarningEventArgs : CancellableEventArgs
	{
		private readonly CompilerWarning _warning;

		public CompilerWarning Warning => _warning;

		public CompilerWarningEventArgs(CompilerWarning warning)
		{
			_warning = warning;
		}
	}
}
