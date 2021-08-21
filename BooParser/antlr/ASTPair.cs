using System.Collections;
using antlr.collections;

namespace antlr
{
	public class ASTPair
	{
		private static Queue instancePool_ = new Queue();

		public AST root;

		public AST child;

		public static ASTPair GetInstance()
		{
			if (instancePool_.Count > 0)
			{
				return (ASTPair)instancePool_.Dequeue();
			}
			return new ASTPair();
		}

		public static void PutInstance(ASTPair p)
		{
			if (p != null)
			{
				p.reset();
				instancePool_.Enqueue(p);
			}
		}

		public void advanceChildToEnd()
		{
			if (child != null)
			{
				while (child.getNextSibling() != null)
				{
					child = child.getNextSibling();
				}
			}
		}

		public virtual ASTPair copy()
		{
			ASTPair instance = GetInstance();
			instance.root = root;
			instance.child = child;
			return instance;
		}

		private void reset()
		{
			root = null;
			child = null;
		}

		public override string ToString()
		{
			string text = ((root == null) ? "null" : root.getText());
			string text2 = ((child == null) ? "null" : child.getText());
			return "[" + text + "," + text2 + "]";
		}
	}
}
