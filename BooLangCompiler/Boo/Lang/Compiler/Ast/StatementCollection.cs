using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class StatementCollection : NodeCollection<Statement>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static StatementCollection FromArray(params Statement[] items)
		{
			StatementCollection statementCollection = new StatementCollection();
			statementCollection.AddRange(items);
			return statementCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public StatementCollection PopRange(int begin)
		{
			StatementCollection statementCollection = new StatementCollection(base.ParentNode);
			statementCollection.InnerList.AddRange(InternalPopRange(begin));
			return statementCollection;
		}

		public StatementCollection()
		{
		}

		public StatementCollection(Node parent)
			: base(parent)
		{
		}
	}
}
