using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GeneratorExpressionCollection : NodeCollection<GeneratorExpression>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static GeneratorExpressionCollection FromArray(params GeneratorExpression[] items)
		{
			GeneratorExpressionCollection generatorExpressionCollection = new GeneratorExpressionCollection();
			generatorExpressionCollection.AddRange(items);
			return generatorExpressionCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public GeneratorExpressionCollection PopRange(int begin)
		{
			GeneratorExpressionCollection generatorExpressionCollection = new GeneratorExpressionCollection(base.ParentNode);
			generatorExpressionCollection.InnerList.AddRange(InternalPopRange(begin));
			return generatorExpressionCollection;
		}

		public GeneratorExpressionCollection()
		{
		}

		public GeneratorExpressionCollection(Node parent)
			: base(parent)
		{
		}
	}
}
