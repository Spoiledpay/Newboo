using antlr.collections;

namespace antlr
{
	public abstract class ASTNodeCreator
	{
		public abstract string ASTNodeTypeName { get; }

		public abstract AST Create();
	}
}
