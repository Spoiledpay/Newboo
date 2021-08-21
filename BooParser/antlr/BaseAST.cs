using System;
using System.Collections;
using System.IO;
using System.Text;
using antlr.collections;

namespace antlr
{
	[Serializable]
	public abstract class BaseAST : AST, ICloneable
	{
		protected internal BaseAST down;

		protected internal BaseAST right;

		private static bool verboseStringConversion = false;

		private static string[] tokenNames = null;

		public virtual int Type
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public virtual void addChild(AST node)
		{
			if (node == null)
			{
				return;
			}
			BaseAST baseAST = down;
			if (baseAST != null)
			{
				while (baseAST.right != null)
				{
					baseAST = baseAST.right;
				}
				baseAST.right = (BaseAST)node;
			}
			else
			{
				down = (BaseAST)node;
			}
		}

		private void doWorkForFindAll(ArrayList v, AST target, bool partialMatch)
		{
			for (AST aST = this; aST != null; aST = aST.getNextSibling())
			{
				if ((partialMatch && aST.EqualsTreePartial(target)) || (!partialMatch && aST.EqualsTree(target)))
				{
					v.Add(aST);
				}
				if (aST.getFirstChild() != null)
				{
					((BaseAST)aST.getFirstChild()).doWorkForFindAll(v, target, partialMatch);
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((AST)obj);
		}

		public virtual bool Equals(AST t)
		{
			if (t == null)
			{
				return false;
			}
			return object.Equals(getText(), t.getText()) && Type == t.Type;
		}

		public virtual bool EqualsList(AST t)
		{
			if (t == null)
			{
				return false;
			}
			AST aST = this;
			while (aST != null && t != null)
			{
				if (!aST.Equals(t))
				{
					return false;
				}
				if (aST.getFirstChild() != null)
				{
					if (!aST.getFirstChild().EqualsList(t.getFirstChild()))
					{
						return false;
					}
				}
				else if (t.getFirstChild() != null)
				{
					return false;
				}
				aST = aST.getNextSibling();
				t = t.getNextSibling();
			}
			if (aST == null && t == null)
			{
				return true;
			}
			return false;
		}

		public virtual bool EqualsListPartial(AST sub)
		{
			if (sub == null)
			{
				return true;
			}
			AST aST = this;
			while (aST != null && sub != null)
			{
				if (!aST.Equals(sub))
				{
					return false;
				}
				if (aST.getFirstChild() != null && !aST.getFirstChild().EqualsListPartial(sub.getFirstChild()))
				{
					return false;
				}
				aST = aST.getNextSibling();
				sub = sub.getNextSibling();
			}
			if (aST == null && sub != null)
			{
				return false;
			}
			return true;
		}

		public virtual bool EqualsTree(AST t)
		{
			if (!Equals(t))
			{
				return false;
			}
			if (getFirstChild() != null)
			{
				if (!getFirstChild().EqualsList(t.getFirstChild()))
				{
					return false;
				}
			}
			else if (t.getFirstChild() != null)
			{
				return false;
			}
			return true;
		}

		public virtual bool EqualsTreePartial(AST sub)
		{
			if (sub == null)
			{
				return true;
			}
			if (!Equals(sub))
			{
				return false;
			}
			if (getFirstChild() != null && !getFirstChild().EqualsListPartial(sub.getFirstChild()))
			{
				return false;
			}
			return true;
		}

		public virtual IEnumerator findAll(AST target)
		{
			ArrayList arrayList = new ArrayList(10);
			if (target == null)
			{
				return null;
			}
			doWorkForFindAll(arrayList, target, partialMatch: false);
			return arrayList.GetEnumerator();
		}

		public virtual IEnumerator findAllPartial(AST sub)
		{
			ArrayList arrayList = new ArrayList(10);
			if (sub == null)
			{
				return null;
			}
			doWorkForFindAll(arrayList, sub, partialMatch: true);
			return arrayList.GetEnumerator();
		}

		public virtual AST getFirstChild()
		{
			return down;
		}

		public virtual AST getNextSibling()
		{
			return right;
		}

		public virtual string getText()
		{
			return "";
		}

		public int getNumberOfChildren()
		{
			BaseAST baseAST = down;
			int num = 0;
			if (baseAST != null)
			{
				num = 1;
				while (baseAST.right != null)
				{
					baseAST = baseAST.right;
					num++;
				}
			}
			return num;
		}

		public abstract void initialize(int t, string txt);

		public abstract void initialize(AST t);

		public abstract void initialize(IToken t);

		public virtual void removeChildren()
		{
			down = null;
		}

		public virtual void setFirstChild(AST c)
		{
			down = (BaseAST)c;
		}

		public virtual void setNextSibling(AST n)
		{
			right = (BaseAST)n;
		}

		public virtual void setText(string text)
		{
		}

		public virtual void setType(int ttype)
		{
			Type = ttype;
		}

		public static void setVerboseStringConversion(bool verbose, string[] names)
		{
			verboseStringConversion = verbose;
			tokenNames = names;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (verboseStringConversion && string.Compare(getText(), tokenNames[Type], ignoreCase: true) != 0 && 0 != string.Compare(getText(), StringUtils.stripFrontBack(tokenNames[Type], "\"", "\""), ignoreCase: true))
			{
				stringBuilder.Append('[');
				stringBuilder.Append(getText());
				stringBuilder.Append(",<");
				stringBuilder.Append(tokenNames[Type]);
				stringBuilder.Append(">]");
				return stringBuilder.ToString();
			}
			return getText();
		}

		public virtual string ToStringList()
		{
			string text = "";
			if (((AST)this).getFirstChild() != null)
			{
				text += " (";
			}
			text = text + " " + ToString();
			if (((AST)this).getFirstChild() != null)
			{
				text += ((BaseAST)((AST)this).getFirstChild()).ToStringList();
			}
			if (((AST)this).getFirstChild() != null)
			{
				text += " )";
			}
			if (((AST)this).getNextSibling() != null)
			{
				text += ((BaseAST)((AST)this).getNextSibling()).ToStringList();
			}
			return text;
		}

		public virtual string ToStringTree()
		{
			string text = "";
			if (((AST)this).getFirstChild() != null)
			{
				text += " (";
			}
			text = text + " " + ToString();
			if (((AST)this).getFirstChild() != null)
			{
				text += ((BaseAST)((AST)this).getFirstChild()).ToStringList();
			}
			if (((AST)this).getFirstChild() != null)
			{
				text += " )";
			}
			return text;
		}

		public virtual string ToTree()
		{
			return ToTree(string.Empty);
		}

		public virtual string ToTree(string prefix)
		{
			StringBuilder stringBuilder = new StringBuilder(prefix);
			if (getNextSibling() == null)
			{
				stringBuilder.Append("+--");
			}
			else
			{
				stringBuilder.Append("|--");
			}
			stringBuilder.Append(ToString());
			stringBuilder.Append(Environment.NewLine);
			if (getFirstChild() != null)
			{
				if (getNextSibling() == null)
				{
					stringBuilder.Append(((BaseAST)getFirstChild()).ToTree(prefix + "   "));
				}
				else
				{
					stringBuilder.Append(((BaseAST)getFirstChild()).ToTree(prefix + "|  "));
				}
			}
			if (getNextSibling() != null)
			{
				stringBuilder.Append(((BaseAST)getNextSibling()).ToTree(prefix));
			}
			return stringBuilder.ToString();
		}

		public static string decode(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '&')
				{
					char c2 = text[i + 1];
					char c3 = text[i + 2];
					char c4 = text[i + 3];
					char c5 = text[i + 4];
					char c6 = text[i + 5];
					if (c2 == 'a' && c3 == 'm' && c4 == 'p' && c5 == ';')
					{
						stringBuilder.Append("&");
						i += 5;
					}
					else if (c2 == 'l' && c3 == 't' && c4 == ';')
					{
						stringBuilder.Append("<");
						i += 4;
					}
					else if (c2 == 'g' && c3 == 't' && c4 == ';')
					{
						stringBuilder.Append(">");
						i += 4;
					}
					else if (c2 == 'q' && c3 == 'u' && c4 == 'o' && c5 == 't' && c6 == ';')
					{
						stringBuilder.Append("\"");
						i += 6;
					}
					else if (c2 == 'a' && c3 == 'p' && c4 == 'o' && c5 == 's' && c6 == ';')
					{
						stringBuilder.Append("'");
						i += 6;
					}
					else
					{
						stringBuilder.Append("&");
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string encode(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in text)
			{
				switch (c)
				{
				case '&':
					stringBuilder.Append("&amp;");
					break;
				case '<':
					stringBuilder.Append("&lt;");
					break;
				case '>':
					stringBuilder.Append("&gt;");
					break;
				case '"':
					stringBuilder.Append("&quot;");
					break;
				case '\'':
					stringBuilder.Append("&apos;");
					break;
				default:
					stringBuilder.Append(c);
					break;
				}
			}
			return stringBuilder.ToString();
		}

		public virtual void xmlSerializeNode(TextWriter outWriter)
		{
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append("<");
			stringBuilder.Append(GetType().FullName + " ");
			stringBuilder.Append("text=\"" + encode(getText()) + "\" type=\"" + Type + "\"/>");
			outWriter.Write(stringBuilder.ToString());
		}

		public virtual void xmlSerializeRootOpen(TextWriter outWriter)
		{
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append("<");
			stringBuilder.Append(GetType().FullName + " ");
			stringBuilder.Append("text=\"" + encode(getText()) + "\" type=\"" + Type + "\">\n");
			outWriter.Write(stringBuilder.ToString());
		}

		public virtual void xmlSerializeRootClose(TextWriter outWriter)
		{
			outWriter.Write("</" + GetType().FullName + ">\n");
		}

		public virtual void xmlSerialize(TextWriter outWriter)
		{
			for (AST aST = this; aST != null; aST = aST.getNextSibling())
			{
				if (aST.getFirstChild() == null)
				{
					((BaseAST)aST).xmlSerializeNode(outWriter);
				}
				else
				{
					((BaseAST)aST).xmlSerializeRootOpen(outWriter);
					((BaseAST)aST.getFirstChild()).xmlSerialize(outWriter);
					((BaseAST)aST).xmlSerializeRootClose(outWriter);
				}
			}
		}

		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
