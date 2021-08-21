using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CharLiteralExpression : StringLiteralExpression
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CharLiteralExpression;

		public override object ValueObject => base.Value[0];

		[GeneratedCode("astgen.boo", "1")]
		public new CharLiteralExpression CloneNode()
		{
			return (CharLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CharLiteralExpression CleanClone()
		{
			return (CharLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCharLiteralExpression(this);
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
			CharLiteralExpression charLiteralExpression = (CharLiteralExpression)node;
			if (_value != charLiteralExpression._value)
			{
				return NoMatch("CharLiteralExpression._value");
			}
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
			CharLiteralExpression charLiteralExpression = new CharLiteralExpression();
			charLiteralExpression._lexicalInfo = _lexicalInfo;
			charLiteralExpression._endSourceLocation = _endSourceLocation;
			charLiteralExpression._documentation = _documentation;
			charLiteralExpression._isSynthetic = _isSynthetic;
			charLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				charLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			charLiteralExpression._expressionType = _expressionType;
			charLiteralExpression._value = _value;
			return charLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public CharLiteralExpression()
		{
		}

		public CharLiteralExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public CharLiteralExpression(char value)
			: this(LexicalInfo.Empty, value)
		{
		}

		public CharLiteralExpression(LexicalInfo lexicalInfo, string value)
			: base(lexicalInfo)
		{
			_value = value;
		}

		public CharLiteralExpression(LexicalInfo lexicalInfo, char value)
			: base(lexicalInfo)
		{
			_value = string.Concat(value);
		}
	}
}
