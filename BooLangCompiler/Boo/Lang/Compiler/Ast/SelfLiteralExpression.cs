using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SelfLiteralExpression : LiteralExpression
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SelfLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		public new SelfLiteralExpression CloneNode()
		{
			return (SelfLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SelfLiteralExpression CleanClone()
		{
			return (SelfLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSelfLiteralExpression(this);
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
			SelfLiteralExpression selfLiteralExpression = (SelfLiteralExpression)node;
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
			SelfLiteralExpression selfLiteralExpression = new SelfLiteralExpression();
			selfLiteralExpression._lexicalInfo = _lexicalInfo;
			selfLiteralExpression._endSourceLocation = _endSourceLocation;
			selfLiteralExpression._documentation = _documentation;
			selfLiteralExpression._isSynthetic = _isSynthetic;
			selfLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				selfLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			selfLiteralExpression._expressionType = _expressionType;
			return selfLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public SelfLiteralExpression()
		{
		}

		public SelfLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
