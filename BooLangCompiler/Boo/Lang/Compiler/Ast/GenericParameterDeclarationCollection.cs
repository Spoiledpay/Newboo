using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GenericParameterDeclarationCollection : NodeCollection<GenericParameterDeclaration>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static GenericParameterDeclarationCollection FromArray(params GenericParameterDeclaration[] items)
		{
			GenericParameterDeclarationCollection genericParameterDeclarationCollection = new GenericParameterDeclarationCollection();
			genericParameterDeclarationCollection.AddRange(items);
			return genericParameterDeclarationCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public GenericParameterDeclarationCollection PopRange(int begin)
		{
			GenericParameterDeclarationCollection genericParameterDeclarationCollection = new GenericParameterDeclarationCollection(base.ParentNode);
			genericParameterDeclarationCollection.InnerList.AddRange(InternalPopRange(begin));
			return genericParameterDeclarationCollection;
		}

		public GenericParameterDeclarationCollection()
		{
		}

		public GenericParameterDeclarationCollection(Node parent)
			: base(parent)
		{
		}

		public string ToCodeString()
		{
			string[] value = Array.ConvertAll(ToArray(), (GenericParameterDeclaration gpd) => gpd.Name);
			return string.Join(", ", value);
		}
	}
}
