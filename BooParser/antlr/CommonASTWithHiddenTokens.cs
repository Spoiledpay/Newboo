using System;
using antlr.collections;

namespace antlr
{
	public class CommonASTWithHiddenTokens : CommonAST
	{
		public class CommonASTWithHiddenTokensCreator : ASTNodeCreator
		{
			public override string ASTNodeTypeName => typeof(CommonASTWithHiddenTokens).FullName;

			public override AST Create()
			{
				return new CommonASTWithHiddenTokens();
			}
		}

		public new static readonly CommonASTWithHiddenTokensCreator Creator = new CommonASTWithHiddenTokensCreator();

		protected internal IHiddenStreamToken hiddenBefore;

		protected internal IHiddenStreamToken hiddenAfter;

		public CommonASTWithHiddenTokens()
		{
		}

		public CommonASTWithHiddenTokens(IToken tok)
			: base(tok)
		{
		}

		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		protected CommonASTWithHiddenTokens(CommonASTWithHiddenTokens another)
			: base(another)
		{
			hiddenBefore = another.hiddenBefore;
			hiddenAfter = another.hiddenAfter;
		}

		public virtual IHiddenStreamToken getHiddenAfter()
		{
			return hiddenAfter;
		}

		public virtual IHiddenStreamToken getHiddenBefore()
		{
			return hiddenBefore;
		}

		public override void initialize(AST t)
		{
			hiddenBefore = ((CommonASTWithHiddenTokens)t).getHiddenBefore();
			hiddenAfter = ((CommonASTWithHiddenTokens)t).getHiddenAfter();
			base.initialize(t);
		}

		public override void initialize(IToken tok)
		{
			IHiddenStreamToken hiddenStreamToken = (IHiddenStreamToken)tok;
			base.initialize((IToken)hiddenStreamToken);
			hiddenBefore = hiddenStreamToken.getHiddenBefore();
			hiddenAfter = hiddenStreamToken.getHiddenAfter();
		}

		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		public override object Clone()
		{
			return new CommonASTWithHiddenTokens(this);
		}
	}
}
