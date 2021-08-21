namespace Boo.Lang.Compiler.Ast
{
	public interface INodeWithArguments
	{
		ExpressionCollection Arguments { get; }

		ExpressionPairCollection NamedArguments { get; }
	}
}
