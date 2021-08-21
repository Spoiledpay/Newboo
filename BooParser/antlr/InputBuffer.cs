using System.Collections;
using System.Text;

namespace antlr
{
	public abstract class InputBuffer
	{
		protected internal int nMarkers = 0;

		protected internal int markerOffset = 0;

		protected internal int numToConsume = 0;

		protected ArrayList queue;

		public InputBuffer()
		{
			queue = new ArrayList();
		}

		public virtual void commit()
		{
			nMarkers--;
		}

		public virtual char consume()
		{
			numToConsume++;
			return LA(1);
		}

		public abstract void fill(int amount);

		public virtual string getLAChars()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char[] array = new char[queue.Count - markerOffset];
			queue.CopyTo(array, markerOffset);
			stringBuilder.Append(array);
			return stringBuilder.ToString();
		}

		public virtual string getMarkedChars()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char[] array = new char[queue.Count - markerOffset];
			queue.CopyTo(array, markerOffset);
			stringBuilder.Append(array);
			return stringBuilder.ToString();
		}

		public virtual bool isMarked()
		{
			return nMarkers != 0;
		}

		public virtual char LA(int i)
		{
			fill(i);
			return (char)queue[markerOffset + i - 1];
		}

		public virtual int mark()
		{
			syncConsume();
			nMarkers++;
			return markerOffset;
		}

		public virtual void rewind(int mark)
		{
			syncConsume();
			markerOffset = mark;
			nMarkers--;
		}

		public virtual void reset()
		{
			nMarkers = 0;
			markerOffset = 0;
			numToConsume = 0;
			queue.Clear();
		}

		protected internal virtual void syncConsume()
		{
			if (numToConsume > 0)
			{
				if (nMarkers > 0)
				{
					markerOffset += numToConsume;
				}
				else
				{
					queue.RemoveRange(0, numToConsume);
				}
				numToConsume = 0;
			}
		}
	}
}
