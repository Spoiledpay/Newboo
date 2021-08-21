using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TypeDefinitionCollection : NodeCollection<TypeDefinition>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static TypeDefinitionCollection FromArray(params TypeDefinition[] items)
		{
			TypeDefinitionCollection typeDefinitionCollection = new TypeDefinitionCollection();
			typeDefinitionCollection.AddRange(items);
			return typeDefinitionCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public TypeDefinitionCollection PopRange(int begin)
		{
			TypeDefinitionCollection typeDefinitionCollection = new TypeDefinitionCollection(base.ParentNode);
			typeDefinitionCollection.InnerList.AddRange(InternalPopRange(begin));
			return typeDefinitionCollection;
		}

		public TypeDefinitionCollection()
		{
		}

		public TypeDefinitionCollection(Node parent)
			: base(parent)
		{
		}
	}
}
