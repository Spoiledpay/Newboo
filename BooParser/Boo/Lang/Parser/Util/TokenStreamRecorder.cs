using System.Collections;
using antlr;

namespace Boo.Lang.Parser.Util
{
	public class TokenStreamRecorder : TokenStream
	{
		private TokenStreamSelector _selector;

		private Queue _queue = new Queue();

		public int Count => _queue.Count;

		public TokenStreamRecorder(TokenStreamSelector selector)
		{
			_selector = selector;
		}

		public void Enqueue(IToken token)
		{
			_queue.Enqueue(token);
		}

		public IToken Dequeue()
		{
			return (IToken)_queue.Dequeue();
		}

		public int RecordUntil(TokenStream stream, int closeToken, int openToken)
		{
			int num = 0;
			int num2 = 1;
			IToken token = stream.nextToken();
			while (true)
			{
				bool flag = true;
				if (closeToken == token.Type)
				{
					num2--;
					if (0 == num2)
					{
						break;
					}
				}
				else if (openToken == token.Type)
				{
					num2++;
				}
				else if (token.Type < 4)
				{
					break;
				}
				Enqueue(token);
				num++;
				token = stream.nextToken();
			}
			return num;
		}

		public IToken nextToken()
		{
			if (_queue.Count > 0)
			{
				return Dequeue();
			}
			return _selector.pop().nextToken();
		}
	}
}
