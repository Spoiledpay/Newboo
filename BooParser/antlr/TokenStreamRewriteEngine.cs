using System.Collections;
using System.Text;
using antlr.collections.impl;

namespace antlr
{
	public class TokenStreamRewriteEngine : TokenStream
	{
		protected class RewriteOperation
		{
			protected internal int index;

			protected internal string text;

			protected RewriteOperation(int index, string text)
			{
				this.index = index;
				this.text = text;
			}

			public virtual int execute(StringBuilder buf)
			{
				return index;
			}
		}

		protected class InsertBeforeOp : RewriteOperation
		{
			public InsertBeforeOp(int index, string text)
				: base(index, text)
			{
			}

			public override int execute(StringBuilder buf)
			{
				buf.Append(text);
				return index;
			}
		}

		protected class ReplaceOp : RewriteOperation
		{
			protected int lastIndex;

			public ReplaceOp(int from, int to, string text)
				: base(from, text)
			{
				lastIndex = to;
			}

			public override int execute(StringBuilder buf)
			{
				if (text != null)
				{
					buf.Append(text);
				}
				return lastIndex + 1;
			}
		}

		protected class DeleteOp : ReplaceOp
		{
			public DeleteOp(int from, int to)
				: base(from, to, null)
			{
			}
		}

		public class RewriteOperationComparer : IComparer
		{
			public static readonly RewriteOperationComparer Default = new RewriteOperationComparer();

			public virtual int Compare(object o1, object o2)
			{
				RewriteOperation rewriteOperation = (RewriteOperation)o1;
				RewriteOperation rewriteOperation2 = (RewriteOperation)o2;
				if (rewriteOperation.index < rewriteOperation2.index)
				{
					return -1;
				}
				if (rewriteOperation.index > rewriteOperation2.index)
				{
					return 1;
				}
				return 0;
			}
		}

		public const int MIN_TOKEN_INDEX = 0;

		public const string DEFAULT_PROGRAM_NAME = "default";

		public const int PROGRAM_INIT_SIZE = 100;

		protected IList tokens;

		protected IDictionary programs = null;

		protected IDictionary lastRewriteTokenIndexes = null;

		protected int index = 0;

		protected TokenStream stream;

		protected BitSet discardMask = new BitSet();

		public TokenStreamRewriteEngine(TokenStream upstream)
			: this(upstream, 1000)
		{
		}

		public TokenStreamRewriteEngine(TokenStream upstream, int initialSize)
		{
			stream = upstream;
			tokens = new ArrayList(initialSize);
			programs = new Hashtable();
			programs["default"] = new ArrayList(100);
			lastRewriteTokenIndexes = new Hashtable();
		}

		public IToken nextToken()
		{
			TokenWithIndex tokenWithIndex;
			do
			{
				tokenWithIndex = (TokenWithIndex)stream.nextToken();
				if (tokenWithIndex != null)
				{
					tokenWithIndex.setIndex(index);
					if (tokenWithIndex.Type != 1)
					{
						tokens.Add(tokenWithIndex);
					}
					index++;
				}
			}
			while (tokenWithIndex != null && discardMask.member(tokenWithIndex.Type));
			return tokenWithIndex;
		}

		public void rollback(int instructionIndex)
		{
			rollback("default", instructionIndex);
		}

		public void rollback(string programName, int instructionIndex)
		{
			ArrayList arrayList = (ArrayList)programs[programName];
			if (arrayList != null)
			{
				programs[programName] = arrayList.GetRange(0, instructionIndex);
			}
		}

		public void deleteProgram()
		{
			deleteProgram("default");
		}

		public void deleteProgram(string programName)
		{
			rollback(programName, 0);
		}

		protected void addToSortedRewriteList(RewriteOperation op)
		{
			addToSortedRewriteList("default", op);
		}

		protected void addToSortedRewriteList(string programName, RewriteOperation op)
		{
			ArrayList arrayList = (ArrayList)getProgram(programName);
			if (op.index >= getLastRewriteTokenIndex(programName))
			{
				arrayList.Add(op);
				setLastRewriteTokenIndex(programName, op.index);
				return;
			}
			int num = arrayList.BinarySearch(op, RewriteOperationComparer.Default);
			if (num < 0)
			{
				arrayList.Insert(-num - 1, op);
			}
		}

		public void insertAfter(IToken t, string text)
		{
			insertAfter("default", t, text);
		}

		public void insertAfter(int index, string text)
		{
			insertAfter("default", index, text);
		}

		public void insertAfter(string programName, IToken t, string text)
		{
			insertAfter(programName, ((TokenWithIndex)t).getIndex(), text);
		}

		public void insertAfter(string programName, int index, string text)
		{
			insertBefore(programName, index + 1, text);
		}

		public void insertBefore(IToken t, string text)
		{
			insertBefore("default", t, text);
		}

		public void insertBefore(int index, string text)
		{
			insertBefore("default", index, text);
		}

		public void insertBefore(string programName, IToken t, string text)
		{
			insertBefore(programName, ((TokenWithIndex)t).getIndex(), text);
		}

		public void insertBefore(string programName, int index, string text)
		{
			addToSortedRewriteList(programName, new InsertBeforeOp(index, text));
		}

		public void replace(int index, string text)
		{
			replace("default", index, index, text);
		}

		public void replace(int from, int to, string text)
		{
			replace("default", from, to, text);
		}

		public void replace(IToken indexT, string text)
		{
			replace("default", indexT, indexT, text);
		}

		public void replace(IToken from, IToken to, string text)
		{
			replace("default", from, to, text);
		}

		public void replace(string programName, int from, int to, string text)
		{
			addToSortedRewriteList(new ReplaceOp(from, to, text));
		}

		public void replace(string programName, IToken from, IToken to, string text)
		{
			replace(programName, ((TokenWithIndex)from).getIndex(), ((TokenWithIndex)to).getIndex(), text);
		}

		public void delete(int index)
		{
			delete("default", index, index);
		}

		public void delete(int from, int to)
		{
			delete("default", from, to);
		}

		public void delete(IToken indexT)
		{
			delete("default", indexT, indexT);
		}

		public void delete(IToken from, IToken to)
		{
			delete("default", from, to);
		}

		public void delete(string programName, int from, int to)
		{
			replace(programName, from, to, null);
		}

		public void delete(string programName, IToken from, IToken to)
		{
			replace(programName, from, to, null);
		}

		public void discard(int ttype)
		{
			discardMask.add(ttype);
		}

		public TokenWithIndex getToken(int i)
		{
			return (TokenWithIndex)tokens[i];
		}

		public int getTokenStreamSize()
		{
			return tokens.Count;
		}

		public string ToOriginalString()
		{
			return ToOriginalString(0, getTokenStreamSize() - 1);
		}

		public string ToOriginalString(int start, int end)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = start; i >= 0 && i <= end && i < tokens.Count; i++)
			{
				stringBuilder.Append(getToken(i).getText());
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			return ToString(0, getTokenStreamSize());
		}

		public string ToString(string programName)
		{
			return ToString(programName, 0, getTokenStreamSize());
		}

		public string ToString(int start, int end)
		{
			return ToString("default", start, end);
		}

		public string ToString(string programName, int start, int end)
		{
			IList list = (IList)programs[programName];
			if (list == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = start;
			while (num2 >= 0 && num2 <= end && num2 < tokens.Count)
			{
				if (num < list.Count)
				{
					RewriteOperation rewriteOperation = (RewriteOperation)list[num];
					while (num2 == rewriteOperation.index && num < list.Count)
					{
						num2 = rewriteOperation.execute(stringBuilder);
						num++;
						if (num < list.Count)
						{
							rewriteOperation = (RewriteOperation)list[num];
						}
					}
				}
				if (num2 < end)
				{
					stringBuilder.Append(getToken(num2).getText());
					num2++;
				}
			}
			for (int i = num; i < list.Count; i++)
			{
				RewriteOperation rewriteOperation = (RewriteOperation)list[i];
				rewriteOperation.execute(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		public string ToDebugString()
		{
			return ToDebugString(0, getTokenStreamSize());
		}

		public string ToDebugString(int start, int end)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = start; i >= 0 && i <= end && i < tokens.Count; i++)
			{
				stringBuilder.Append(getToken(i));
			}
			return stringBuilder.ToString();
		}

		public int getLastRewriteTokenIndex()
		{
			return getLastRewriteTokenIndex("default");
		}

		protected int getLastRewriteTokenIndex(string programName)
		{
			object obj = lastRewriteTokenIndexes[programName];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		protected void setLastRewriteTokenIndex(string programName, int i)
		{
			lastRewriteTokenIndexes[programName] = i;
		}

		protected IList getProgram(string name)
		{
			IList list = (IList)programs[name];
			if (list == null)
			{
				list = initializeProgram(name);
			}
			return list;
		}

		private IList initializeProgram(string name)
		{
			IList list = new ArrayList(100);
			programs[name] = list;
			return list;
		}
	}
}
