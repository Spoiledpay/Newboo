using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SliceCollection : NodeCollection<Slice>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static SliceCollection FromArray(params Slice[] items)
		{
			SliceCollection sliceCollection = new SliceCollection();
			sliceCollection.AddRange(items);
			return sliceCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public SliceCollection PopRange(int begin)
		{
			SliceCollection sliceCollection = new SliceCollection(base.ParentNode);
			sliceCollection.InnerList.AddRange(InternalPopRange(begin));
			return sliceCollection;
		}

		public SliceCollection()
		{
		}

		public SliceCollection(Node parent)
			: base(parent)
		{
		}
	}
}
