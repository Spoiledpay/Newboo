namespace antlr.collections.impl
{
	public class ASTArray
	{
		public int size = 0;

		public AST[] array;

		public ASTArray(int capacity)
		{
			array = new AST[capacity];
		}

		public virtual ASTArray add(AST node)
		{
			array[size++] = node;
			return this;
		}
	}
}
