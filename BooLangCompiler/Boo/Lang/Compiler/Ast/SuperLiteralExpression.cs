using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SuperLiteralExpression : LiteralExpression
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SuperLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		public new SuperLiteralExpression CloneNode()
		{
			return (SuperLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SuperLiteralExpression CleanClone()
		{
			return (SuperLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSuperLiteralExpression(this);
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
			SuperLiteralExpression superLiteralExpression = (SuperLiteralExpression)node;
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
			SuperLiteralExpression superLiteralExpression = new SuperLiteralExpression();
			superLiteralExpression._lexicalInfo = _lexicalInfo;
			superLiteralExpression._endSourceLocation = _endSourceLocation;
			superLiteralExpression._documentation = _documentation;
			superLiteralExpression._isSynthetic = _isSynthetic;
			superLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				superLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			superLiteralExpression._expressionType = _expressionType;
			return superLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public SuperLiteralExpression()
		{
		}

		public SuperLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
