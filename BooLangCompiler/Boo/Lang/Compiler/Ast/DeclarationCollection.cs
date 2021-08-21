using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class DeclarationCollection : NodeCollection<Declaration>
	{
		public Declaration this[string name]
		{
			get
			{
				foreach (Declaration inner in base.InnerList)
				{
					if (name == inner.Name)
					{
						return inner;
					}
				}
				return null;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public static DeclarationCollection FromArray(params Declaration[] items)
		{
			DeclarationCollection declarationCollection = new DeclarationCollection();
			declarationCollection.AddRange(items);
			return declarationCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public DeclarationCollection PopRange(int begin)
		{
			DeclarationCollection declarationCollection = new DeclarationCollection(base.ParentNode);
			declarationCollection.InnerList.AddRange(InternalPopRange(begin));
			return declarationCollection;
		}

		public DeclarationCollection()
		{
		}

		public DeclarationCollection(Node parent)
			: base(parent)
		{
		}
	}
}
