using System;
using System.Collections;

namespace antlr
{
	public class TokenStreamSelector : TokenStream
	{
		protected internal Hashtable inputStreamNames;

		protected internal TokenStream input;

		protected internal Stack streamStack = new Stack();

		public TokenStreamSelector()
		{
			inputStreamNames = new Hashtable();
		}

		public virtual void addInputStream(TokenStream stream, string key)
		{
			inputStreamNames[key] = stream;
		}

		public virtual TokenStream getCurrentStream()
		{
			return input;
		}

		public virtual TokenStream getStream(string sname)
		{
			TokenStream tokenStream = (TokenStream)inputStreamNames[sname];
			if (tokenStream == null)
			{
				throw new ArgumentException("TokenStream " + sname + " not found");
			}
			return tokenStream;
		}

		public virtual IToken nextToken()
		{
			while (true)
			{
				try
				{
					return input.nextToken();
				}
				catch (TokenStreamRetryException)
				{
				}
			}
		}

		public virtual TokenStream pop()
		{
			TokenStream tokenStream = (TokenStream)streamStack.Pop();
			select(tokenStream);
			return tokenStream;
		}

		public virtual void push(TokenStream stream)
		{
			streamStack.Push(input);
			select(stream);
		}

		public virtual void push(string sname)
		{
			streamStack.Push(input);
			select(sname);
		}

		public virtual void retry()
		{
			throw new TokenStreamRetryException();
		}

		public virtual void select(TokenStream stream)
		{
			input = stream;
		}

		public virtual void select(string sname)
		{
			input = getStream(sname);
		}
	}
}
