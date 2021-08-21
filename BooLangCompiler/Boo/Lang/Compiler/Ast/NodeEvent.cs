namespace Boo.Lang.Compiler.Ast
{
	public delegate void NodeEvent<T>(T node) where T : Node;
}
