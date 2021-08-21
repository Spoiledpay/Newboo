using System;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class EnvironmentProperty<T>
	{
		private T _value;

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnChange();
			}
		}

		public event EventHandler Changed;

		private void OnChange()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}
	}
}
