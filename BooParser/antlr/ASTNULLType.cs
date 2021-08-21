using System;
using System.Collections;
using antlr.collections;

namespace antlr
{
	public class ASTNULLType : AST, ICloneable
	{
		public virtual int Type
		{
			get
			{
				return 3;
			}
			set
			{
			}
		}

		public virtual void addChild(AST c)
		{
		}

		public virtual bool Equals(AST t)
		{
			return false;
		}

		public virtual bool EqualsList(AST t)
		{
			return false;
		}

		public virtual bool EqualsListPartial(AST t)
		{
			return false;
		}

		public virtual bool EqualsTree(AST t)
		{
			return false;
		}

		public virtual bool EqualsTreePartial(AST t)
		{
			return false;
		}

		public virtual IEnumerator findAll(AST tree)
		{
			return null;
		}

		public virtual IEnumerator findAllPartial(AST subtree)
		{
			return null;
		}

		public virtual AST getFirstChild()
		{
			return this;
		}

		public virtual AST getNextSibling()
		{
			return this;
		}

		public virtual string getText()
		{
			return "<ASTNULL>";
		}

		public int getNumberOfChildren()
		{
			return 0;
		}

		public virtual void initialize(int t, string txt)
		{
		}

		public virtual void initialize(AST t)
		{
		}

		public virtual void initialize(IToken t)
		{
		}

		public virtual void setFirstChild(AST c)
		{
		}

		public virtual void setNextSibling(AST n)
		{
		}

		public virtual void setText(string text)
		{
		}

		public virtual void setType(int ttype)
		{
			Type = ttype;
		}

		public override string ToString()
		{
			return getText();
		}

		public virtual string ToStringList()
		{
			return getText();
		}

		public virtual string ToStringTree()
		{
			return getText();
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
