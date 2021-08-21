using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TypeReferenceCollection : NodeCollection<TypeReference>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static TypeReferenceCollection FromArray(params TypeReference[] items)
		{
			TypeReferenceCollection typeReferenceCollection = new TypeReferenceCollection();
			typeReferenceCollection.AddRange(items);
			return typeReferenceCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public TypeReferenceCollection PopRange(int begin)
		{
			TypeReferenceCollection typeReferenceCollection = new TypeReferenceCollection(base.ParentNode);
			typeReferenceCollection.InnerList.AddRange(InternalPopRange(begin));
			return typeReferenceCollection;
		}

		public TypeReferenceCollection()
		{
		}

		public TypeReferenceCollection(Node parent)
			: base(parent)
		{
		}
	}
}
