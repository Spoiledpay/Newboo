using System.Collections;

namespace antlr.debug
{
	public class DebuggingInputBuffer : InputBuffer
	{
		private InputBuffer buffer;

		private InputBufferEventSupport inputBufferEventSupport;

		private bool debugMode = true;

		public virtual ArrayList InputBufferListeners => inputBufferEventSupport.InputBufferListeners;

		public virtual bool DebugMode
		{
			set
			{
				debugMode = value;
			}
		}

		public DebuggingInputBuffer(InputBuffer buffer)
		{
			this.buffer = buffer;
			inputBufferEventSupport = new InputBufferEventSupport(this);
		}

		public virtual void addInputBufferListener(InputBufferListener l)
		{
			inputBufferEventSupport.addInputBufferListener(l);
		}

		public override char consume()
		{
			char c = ' ';
			try
			{
				c = buffer.LA(1);
			}
			catch (CharStreamException)
			{
			}
			buffer.consume();
			if (debugMode)
			{
				inputBufferEventSupport.fireConsume(c);
			}
			return c;
		}

		public override void fill(int a)
		{
			buffer.fill(a);
		}

		public virtual bool isDebugMode()
		{
			return debugMode;
		}

		public override bool isMarked()
		{
			return buffer.isMarked();
		}

		public override char LA(int i)
		{
			char c = buffer.LA(i);
			if (debugMode)
			{
				inputBufferEventSupport.fireLA(c, i);
			}
			return c;
		}

		public override int mark()
		{
			int num = buffer.mark();
			inputBufferEventSupport.fireMark(num);
			return num;
		}

		public virtual void removeInputBufferListener(InputBufferListener l)
		{
			if (inputBufferEventSupport != null)
			{
				inputBufferEventSupport.removeInputBufferListener(l);
			}
		}

		public override void rewind(int mark)
		{
			buffer.rewind(mark);
			inputBufferEventSupport.fireRewind(mark);
		}
	}
}
