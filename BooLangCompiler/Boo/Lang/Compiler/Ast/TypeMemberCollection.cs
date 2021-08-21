using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TypeMemberCollection : NodeCollection<TypeMember>
	{
		public TypeMember this[string name]
		{
			get
			{
				foreach (TypeMember inner in base.InnerList)
				{
					if (inner.Name == name)
					{
						return inner;
					}
				}
				return null;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public static TypeMemberCollection FromArray(params TypeMember[] items)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection();
			typeMemberCollection.AddRange(items);
			return typeMemberCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public TypeMemberCollection PopRange(int begin)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection(base.ParentNode);
			typeMemberCollection.InnerList.AddRange(InternalPopRange(begin));
			return typeMemberCollection;
		}

		public static TypeMemberCollection FromArray(params object[] items)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection();
			foreach (object obj in items)
			{
				TypeMember typeMember = obj as TypeMember;
				if (typeMember != null)
				{
					typeMemberCollection.Add(typeMember);
				}
				else
				{
					typeMemberCollection.AddRange((IEnumerable<TypeMember>)obj);
				}
			}
			return typeMemberCollection;
		}

		public TypeMemberCollection()
		{
		}

		public TypeMemberCollection(Node parent)
			: base(parent)
		{
		}
	}
}
