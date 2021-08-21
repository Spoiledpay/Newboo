using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class LocalCollection : NodeCollection<Local>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static LocalCollection FromArray(params Local[] items)
		{
			LocalCollection localCollection = new LocalCollection();
			localCollection.AddRange(items);
			return localCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public LocalCollection PopRange(int begin)
		{
			LocalCollection localCollection = new LocalCollection(base.ParentNode);
			localCollection.InnerList.AddRange(InternalPopRange(begin));
			return localCollection;
		}

		public LocalCollection()
		{
		}

		public LocalCollection(Node parent)
			: base(parent)
		{
		}
	}
}
