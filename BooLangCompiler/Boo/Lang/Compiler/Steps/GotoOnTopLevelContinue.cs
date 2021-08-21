using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class GotoOnTopLevelContinue : DepthFirstTransformer
	{
		private LabelStatement _label;

		private int _usage;

		public int UsageCount => _usage;

		public GotoOnTopLevelContinue(LabelStatement label)
		{
			_label = label;
		}

		public override void OnContinueStatement(ContinueStatement node)
		{
			ReplaceCurrentNode(NewGoto(node));
			_usage++;
		}

		public override void OnWhileStatement(WhileStatement node)
		{
		}

		public override void OnForStatement(ForStatement node)
		{
		}

		public override void OnBlockExpression(BlockExpression node)
		{
		}

		public GotoStatement NewGoto(Node sourceNode)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(sourceNode.LexicalInfo, _label.Name);
			referenceExpression.Entity = _label.Entity;
			ReferenceExpression label = referenceExpression;
			return new GotoStatement(sourceNode.LexicalInfo, label);
		}
	}
}
