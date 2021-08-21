namespace Boo.Lang.Compiler.Ast
{
	public sealed class OmittedExpression : Expression
	{
		public static readonly Expression Default = new OmittedExpression();

		public override NodeType NodeType => NodeType.OmittedExpression;

		public OmittedExpression()
		{
		}

		public OmittedExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public override object Clone()
		{
			return this;
		}

		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnOmittedExpression(this);
		}

		public override bool Matches(Node node)
		{
			return node is OmittedExpression;
		}
	}
}
