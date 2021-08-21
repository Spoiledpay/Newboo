using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class AttributeCollection : NodeCollection<Attribute>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static AttributeCollection FromArray(params Attribute[] items)
		{
			AttributeCollection attributeCollection = new AttributeCollection();
			attributeCollection.AddRange(items);
			return attributeCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public AttributeCollection PopRange(int begin)
		{
			AttributeCollection attributeCollection = new AttributeCollection(base.ParentNode);
			attributeCollection.InnerList.AddRange(InternalPopRange(begin));
			return attributeCollection;
		}

		public AttributeCollection()
		{
		}

		public AttributeCollection(Node parent)
			: base(parent)
		{
		}

		public bool Contains(string attributeName)
		{
			using (IEnumerator<Attribute> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Attribute current = enumerator.Current;
					if (attributeName == current.Name)
					{
						return true;
					}
				}
			}
			return false;
		}

		public IEnumerable<Attribute> Get(string attributeName)
		{
			return this.Where((Attribute attribute) => 0 == string.Compare(attributeName, attribute.Name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
