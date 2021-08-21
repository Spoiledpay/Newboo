using System;
using antlr.collections;

namespace antlr
{
	public class DumpASTVisitor : ASTVisitor
	{
		protected int level = 0;

		private void tabs()
		{
			for (int i = 0; i < level; i++)
			{
				Console.Out.Write("   ");
			}
		}

		public void visit(AST node)
		{
			bool flag = false;
			for (AST aST = node; aST != null; aST = aST.getNextSibling())
			{
				if (aST.getFirstChild() != null)
				{
					flag = false;
					break;
				}
			}
			for (AST aST = node; aST != null; aST = aST.getNextSibling())
			{
				if (!flag || aST == node)
				{
					tabs();
				}
				if (aST.getText() == null)
				{
					Console.Out.Write("nil");
				}
				else
				{
					Console.Out.Write(aST.getText());
				}
				Console.Out.Write(" [" + aST.Type + "] ");
				if (flag)
				{
					Console.Out.Write(" ");
				}
				else
				{
					Console.Out.WriteLine("");
				}
				if (aST.getFirstChild() != null)
				{
					level++;
					visit(aST.getFirstChild());
					level--;
				}
			}
			if (flag)
			{
				Console.Out.WriteLine("");
			}
		}
	}
}
