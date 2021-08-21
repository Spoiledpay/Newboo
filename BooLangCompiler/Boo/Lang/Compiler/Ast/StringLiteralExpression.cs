using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class StringLiteralExpression : LiteralExpression
	{
		protected string _value;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.StringLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public override object ValueObject => Value;

		[GeneratedCode("astgen.boo", "1")]
		public new StringLiteralExpression CloneNode()
		{
			return (StringLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new StringLiteralExpression CleanClone()
		{
			return (StringLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnStringLiteralExpression(this);
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
			StringLiteralExpression stringLiteralExpression = (StringLiteralExpression)node;
			if (_value != stringLiteralExpression._value)
			{
				return NoMatch("StringLiteralExpression._value");
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
			StringLiteralExpression stringLiteralExpression = new StringLiteralExpression();
			stringLiteralExpression._lexicalInfo = _lexicalInfo;
			stringLiteralExpression._endSourceLocation = _endSourceLocation;
			stringLiteralExpression._documentation = _documentation;
			stringLiteralExpression._isSynthetic = _isSynthetic;
			stringLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				stringLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			stringLiteralExpression._expressionType = _expressionType;
			stringLiteralExpression._value = _value;
			return stringLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public StringLiteralExpression()
		{
		}

		public StringLiteralExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public StringLiteralExpression(string value)
		{
			Value = value;
		}

		public StringLiteralExpression(LexicalInfo lexicalInfoProvider, string value)
			: base(lexicalInfoProvider)
		{
			Value = value;
		}
	}
}
