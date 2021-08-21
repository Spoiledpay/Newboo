namespace Boo.Lang.Compiler.Ast
{
	public class QuasiquoteExpander : DepthFirstTransformer
	{
		public static object PreserveLexicalInfoAnnotation = new object();

		private bool _preserveLexicalInfo;

		public override void OnBlock(Block node)
		{
			bool preserveLexicalInfo = _preserveLexicalInfo;
			_preserveLexicalInfo = preserveLexicalInfo || node.ContainsAnnotation(PreserveLexicalInfoAnnotation);
			base.OnBlock(node);
			_preserveLexicalInfo = preserveLexicalInfo;
		}

		public override void OnQuasiquoteExpression(QuasiquoteExpression node)
		{
			CodeSerializer codeSerializer = new CodeSerializer(_preserveLexicalInfo);
			ReplaceCurrentNode(codeSerializer.Serialize(node));
		}
	}
}
