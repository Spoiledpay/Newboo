using System;
using System.Collections;

namespace antlr.debug
{
	public class InputBufferEventSupport
	{
		protected internal const int CONSUME = 0;

		protected internal const int LA = 1;

		protected internal const int MARK = 2;

		protected internal const int REWIND = 3;

		private object source;

		private ArrayList inputBufferListeners;

		private InputBufferEventArgs inputBufferEvent;

		public virtual ArrayList InputBufferListeners => inputBufferListeners;

		public InputBufferEventSupport(object source)
		{
			inputBufferEvent = new InputBufferEventArgs();
			this.source = source;
		}

		public virtual void addInputBufferListener(InputBufferListener l)
		{
			if (inputBufferListeners == null)
			{
				inputBufferListeners = new ArrayList();
			}
			inputBufferListeners.Add(l);
		}

		public virtual void fireConsume(char c)
		{
			inputBufferEvent.setValues(0, c, 0);
			fireEvents(0, inputBufferListeners);
		}

		public virtual void fireEvent(int type, Listener l)
		{
			switch (type)
			{
			case 0:
				((InputBufferListener)l).inputBufferConsume(source, inputBufferEvent);
				break;
			case 1:
				((InputBufferListener)l).inputBufferLA(source, inputBufferEvent);
				break;
			case 2:
				((InputBufferListener)l).inputBufferMark(source, inputBufferEvent);
				break;
			case 3:
				((InputBufferListener)l).inputBufferRewind(source, inputBufferEvent);
				break;
			default:
				throw new ArgumentException("bad type " + type + " for fireEvent()");
			}
		}

		public virtual void fireEvents(int type, ArrayList listeners)
		{
			ArrayList arrayList = null;
			Listener listener = null;
			lock (this)
			{
				if (listeners == null)
				{
					return;
				}
				arrayList = (ArrayList)listeners.Clone();
			}
			if (arrayList != null)
			{
				for (int i = 0; i < arrayList.Count; i++)
				{
					listener = (Listener)arrayList[i];
					fireEvent(type, listener);
				}
			}
		}

		public virtual void fireLA(char c, int la)
		{
			inputBufferEvent.setValues(1, c, la);
			fireEvents(1, inputBufferListeners);
		}

		public virtual void fireMark(int pos)
		{
			inputBufferEvent.setValues(2, ' ', pos);
			fireEvents(2, inputBufferListeners);
		}

		public virtual void fireRewind(int pos)
		{
			inputBufferEvent.setValues(3, ' ', pos);
			fireEvents(3, inputBufferListeners);
		}

		protected internal virtual void refresh(ArrayList listeners)
		{
			ArrayList arrayList;
			lock (listeners)
			{
				arrayList = (ArrayList)listeners.Clone();
			}
			if (arrayList != null)
			{
				for (int i = 0; i < arrayList.Count; i++)
				{
					((Listener)arrayList[i]).refresh();
				}
			}
		}

		public virtual void refreshListeners()
		{
			refresh(inputBufferListeners);
		}

		public virtual void removeInputBufferListener(InputBufferListener l)
		{
			if (inputBufferListeners != null)
			{
				ArrayList arrayList = inputBufferListeners;
				arrayList.Contains(l);
				arrayList.Remove(l);
			}
		}
	}
}
