namespace antlr
{
	public class TokenBuffer
	{
		protected internal TokenStream input;

		protected internal int nMarkers = 0;

		protected internal int markerOffset = 0;

		protected internal int numToConsume = 0;

		internal TokenQueue queue;

		public TokenBuffer(TokenStream input_)
		{
			input = input_;
			queue = new TokenQueue(1);
		}

		public virtual void reset()
		{
			nMarkers = 0;
			markerOffset = 0;
			numToConsume = 0;
			queue.reset();
		}

		public virtual void consume()
		{
			numToConsume++;
		}

		protected virtual void fill(int amount)
		{
			syncConsume();
			while (queue.nbrEntries < amount + markerOffset)
			{
				queue.append(input.nextToken());
			}
		}

		public virtual TokenStream getInput()
		{
			return input;
		}

		public virtual int LA(int i)
		{
			fill(i);
			return queue.elementAt(markerOffset + i - 1).Type;
		}

		public virtual IToken LT(int i)
		{
			fill(i);
			return queue.elementAt(markerOffset + i - 1);
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

		protected virtual void syncConsume()
		{
			while (numToConsume > 0)
			{
				if (nMarkers > 0)
				{
					markerOffset++;
				}
				else
				{
					queue.removeFirst();
				}
				numToConsume--;
			}
		}
	}
}
