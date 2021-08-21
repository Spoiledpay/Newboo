namespace Boo.Lang.Compiler.Ast
{
	public static class MethodExtensions
	{
		public static bool IsPropertyAccessor(this Method method)
		{
			return method.ParentNode != null && method.ParentNode.NodeType == NodeType.Property;
		}
	}
}
