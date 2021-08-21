using System;
using antlr.collections;

namespace antlr
{
	public class CommonAST : BaseAST
	{
		public class CommonASTCreator : ASTNodeCreator
		{
			public override string ASTNodeTypeName => typeof(CommonAST).FullName;

			public override AST Create()
			{
				return new CommonAST();
			}
		}

		public static readonly CommonASTCreator Creator = new CommonASTCreator();

		internal int ttype = 0;

		internal string text;

		public override int Type
		{
			get
			{
				return ttype;
			}
			set
			{
				ttype = value;
			}
		}

		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		protected CommonAST(CommonAST another)
		{
			ttype = another.ttype;
			text = ((another.text == null) ? null : string.Copy(another.text));
		}

		public override string getText()
		{
			return text;
		}

		public override void initialize(int t, string txt)
		{
			Type = t;
			setText(txt);
		}

		public override void initialize(AST t)
		{
			setText(t.getText());
			Type = t.Type;
		}

		public CommonAST()
		{
		}

		public CommonAST(IToken tok)
		{
			initialize(tok);
		}

		public override void initialize(IToken tok)
		{
			setText(tok.getText());
			Type = tok.Type;
		}

		public override void setText(string text_)
		{
			text = text_;
		}

		public override void setType(int ttype_)
		{
			Type = ttype_;
		}

		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		public override object Clone()
		{
			return new CommonAST(this);
		}
	}
}
