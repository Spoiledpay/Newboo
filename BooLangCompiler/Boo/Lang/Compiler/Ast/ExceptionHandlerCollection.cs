using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExceptionHandlerCollection : NodeCollection<ExceptionHandler>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static ExceptionHandlerCollection FromArray(params ExceptionHandler[] items)
		{
			ExceptionHandlerCollection exceptionHandlerCollection = new ExceptionHandlerCollection();
			exceptionHandlerCollection.AddRange(items);
			return exceptionHandlerCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ExceptionHandlerCollection PopRange(int begin)
		{
			ExceptionHandlerCollection exceptionHandlerCollection = new ExceptionHandlerCollection(base.ParentNode);
			exceptionHandlerCollection.InnerList.AddRange(InternalPopRange(begin));
			return exceptionHandlerCollection;
		}

		public ExceptionHandlerCollection()
		{
		}

		public ExceptionHandlerCollection(Node parent)
			: base(parent)
		{
		}
	}
}
