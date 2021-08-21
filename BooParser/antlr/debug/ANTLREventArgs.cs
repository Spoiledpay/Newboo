using System;

namespace antlr.debug
{
	public abstract class ANTLREventArgs : EventArgs
	{
		private int type_;

		public virtual int Type
		{
			get
			{
				return type_;
			}
			set
			{
				type_ = value;
			}
		}

		public ANTLREventArgs()
		{
		}

		public ANTLREventArgs(int type)
		{
			Type = type;
		}

		internal void setValues(int type)
		{
			Type = type;
		}
	}
}
