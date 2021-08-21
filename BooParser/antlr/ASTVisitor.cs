using antlr.collections;

namespace antlr
{
	public interface ASTVisitor
	{
		void visit(AST node);
	}
}
