using System;

namespace Boo.Lang.Compiler
{
	public class CancellableEventArgs : EventArgs
	{
		private bool _cancelled;

		public bool IsCancelled => _cancelled;

		public void Cancel()
		{
			_cancelled = true;
		}
	}
}
