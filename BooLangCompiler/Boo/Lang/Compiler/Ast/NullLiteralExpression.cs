using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class NullLiteralExpression : LiteralExpression
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.NullLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		public new NullLiteralExpression CloneNode()
		{
			return (NullLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new NullLiteralExpression CleanClone()
		{
			return (NullLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnNullLiteralExpression(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Matches(Node node)
		{
			if (node == null)
			{
				return false;
			}
			if (NodeType != node.NodeType)
			{
				return false;
			}
			NullLiteralExpression nullLiteralExpression = (NullLiteralExpression)node;
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			NullLiteralExpression nullLiteralExpression = new NullLiteralExpression();
			nullLiteralExpression._lexicalInfo = _lexicalInfo;
			nullLiteralExpression._endSourceLocation = _endSourceLocation;
			nullLiteralExpression._documentation = _documentation;
			nullLiteralExpression._isSynthetic = _isSynthetic;
			nullLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				nullLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			nullLiteralExpression._expressionType = _expressionType;
			return nullLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public NullLiteralExpression()
		{
		}

		public NullLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
