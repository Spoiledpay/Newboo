using System;
using System.Collections;

namespace antlr.collections
{
	public interface AST : ICloneable
	{
		int Type { get; set; }

		void addChild(AST c);

		bool Equals(AST t);

		bool EqualsList(AST t);

		bool EqualsListPartial(AST t);

		bool EqualsTree(AST t);

		bool EqualsTreePartial(AST t);

		IEnumerator findAll(AST tree);

		IEnumerator findAllPartial(AST subtree);

		AST getFirstChild();

		AST getNextSibling();

		string getText();

		int getNumberOfChildren();

		void initialize(int t, string txt);

		void initialize(AST t);

		void initialize(IToken t);

		void setFirstChild(AST c);

		void setNextSibling(AST n);

		void setText(string text);

		void setType(int ttype);

		new string ToString();

		string ToStringList();

		string ToStringTree();
	}
}
