using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ImportCollection : NodeCollection<Import>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static ImportCollection FromArray(params Import[] items)
		{
			ImportCollection importCollection = new ImportCollection();
			importCollection.AddRange(items);
			return importCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ImportCollection PopRange(int begin)
		{
			ImportCollection importCollection = new ImportCollection(base.ParentNode);
			importCollection.InnerList.AddRange(InternalPopRange(begin));
			return importCollection;
		}

		public ImportCollection()
		{
		}

		public ImportCollection(Node parent)
			: base(parent)
		{
		}
	}
}
