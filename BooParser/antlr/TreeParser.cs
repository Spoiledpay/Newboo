using System;
using antlr.collections;
using antlr.collections.impl;

namespace antlr
{
	public class TreeParser
	{
		public static ASTNULLType ASTNULL = new ASTNULLType();

		protected internal AST retTree_;

		protected internal TreeParserSharedInputState inputState;

		protected internal string[] tokenNames;

		protected internal AST returnAST;

		protected internal ASTFactory astFactory = new ASTFactory();

		protected internal int traceDepth = 0;

		public TreeParser()
		{
			inputState = new TreeParserSharedInputState();
		}

		public virtual AST getAST()
		{
			return returnAST;
		}

		public virtual ASTFactory getASTFactory()
		{
			return astFactory;
		}

		public virtual void resetState()
		{
			traceDepth = 0;
			returnAST = null;
			retTree_ = null;
			inputState.reset();
		}

		public virtual string getTokenName(int num)
		{
			return tokenNames[num];
		}

		public virtual string[] getTokenNames()
		{
			return tokenNames;
		}

		protected internal virtual void match(AST t, int ttype)
		{
			if (t == null || t == ASTNULL || t.Type != ttype)
			{
				throw new MismatchedTokenException(getTokenNames(), t, ttype, matchNot: false);
			}
		}

		public virtual void match(AST t, BitSet b)
		{
			if (t == null || t == ASTNULL || !b.member(t.Type))
			{
				throw new MismatchedTokenException(getTokenNames(), t, b, matchNot: false);
			}
		}

		protected internal virtual void matchNot(AST t, int ttype)
		{
			if (t == null || t == ASTNULL || t.Type == ttype)
			{
				throw new MismatchedTokenException(getTokenNames(), t, ttype, matchNot: true);
			}
		}

		[Obsolete("De-activated since version 2.7.2.6 as it cannot be overidden.", true)]
		public static void panic()
		{
			Console.Error.WriteLine("TreeWalker: panic");
			Environment.Exit(1);
		}

		public virtual void reportError(RecognitionException ex)
		{
			Console.Error.WriteLine(ex.ToString());
		}

		public virtual void reportError(string s)
		{
			Console.Error.WriteLine("error: " + s);
		}

		public virtual void reportWarning(string s)
		{
			Console.Error.WriteLine("warning: " + s);
		}

		public virtual void setASTFactory(ASTFactory f)
		{
			astFactory = f;
		}

		public virtual void setASTNodeType(string nodeType)
		{
			setASTNodeClass(nodeType);
		}

		public virtual void setASTNodeClass(string nodeType)
		{
			astFactory.setASTNodeType(nodeType);
		}

		public virtual void traceIndent()
		{
			for (int i = 0; i < traceDepth; i++)
			{
				Console.Out.Write(" ");
			}
		}

		public virtual void traceIn(string rname, AST t)
		{
			traceDepth++;
			traceIndent();
			Console.Out.WriteLine("> " + rname + "(" + ((t != null) ? t.ToString() : "null") + ")" + ((inputState.guessing > 0) ? " [guessing]" : ""));
		}

		public virtual void traceOut(string rname, AST t)
		{
			traceIndent();
			Console.Out.WriteLine("< " + rname + "(" + ((t != null) ? t.ToString() : "null") + ")" + ((inputState.guessing > 0) ? " [guessing]" : ""));
			traceDepth--;
		}
	}
}
