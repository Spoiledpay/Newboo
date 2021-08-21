using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExpressionPairCollection : NodeCollection<ExpressionPair>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static ExpressionPairCollection FromArray(params ExpressionPair[] items)
		{
			ExpressionPairCollection expressionPairCollection = new ExpressionPairCollection();
			expressionPairCollection.AddRange(items);
			return expressionPairCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ExpressionPairCollection PopRange(int begin)
		{
			ExpressionPairCollection expressionPairCollection = new ExpressionPairCollection(base.ParentNode);
			expressionPairCollection.InnerList.AddRange(InternalPopRange(begin));
			return expressionPairCollection;
		}

		public ExpressionPairCollection()
		{
		}

		public ExpressionPairCollection(Node parent)
			: base(parent)
		{
		}
	}
}
