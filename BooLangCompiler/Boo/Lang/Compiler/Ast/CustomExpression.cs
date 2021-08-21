using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CustomExpression : Expression
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CustomExpression;

		[GeneratedCode("astgen.boo", "1")]
		public new CustomExpression CloneNode()
		{
			return (CustomExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CustomExpression CleanClone()
		{
			return (CustomExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCustomExpression(this);
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
			CustomExpression customExpression = (CustomExpression)node;
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
			CustomExpression customExpression = new CustomExpression();
			customExpression._lexicalInfo = _lexicalInfo;
			customExpression._endSourceLocation = _endSourceLocation;
			customExpression._documentation = _documentation;
			customExpression._isSynthetic = _isSynthetic;
			customExpression._entity = _entity;
			if (_annotations != null)
			{
				customExpression._annotations = (Hashtable)_annotations.Clone();
			}
			customExpression._expressionType = _expressionType;
			return customExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		[GeneratedCode("astgen.boo", "1")]
		public CustomExpression()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public CustomExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
