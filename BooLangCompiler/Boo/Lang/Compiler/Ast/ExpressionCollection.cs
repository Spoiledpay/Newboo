using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExpressionCollection : NodeCollection<Expression>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static ExpressionCollection FromArray(params Expression[] items)
		{
			ExpressionCollection expressionCollection = new ExpressionCollection();
			expressionCollection.AddRange(items);
			return expressionCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ExpressionCollection PopRange(int begin)
		{
			ExpressionCollection expressionCollection = new ExpressionCollection(base.ParentNode);
			expressionCollection.InnerList.AddRange(InternalPopRange(begin));
			return expressionCollection;
		}

		public ExpressionCollection()
		{
		}

		public ExpressionCollection(Node parent)
			: base(parent)
		{
		}
	}
}
