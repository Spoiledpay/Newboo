using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class IntegerLiteralExpression : LiteralExpression
	{
		protected long _value;

		protected bool _isLong;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.IntegerLiteralExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public long Value
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

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public bool IsLong
		{
			get
			{
				return _isLong;
			}
			set
			{
				_isLong = value;
			}
		}

		public override object ValueObject => Value;

		[GeneratedCode("astgen.boo", "1")]
		public new IntegerLiteralExpression CloneNode()
		{
			return (IntegerLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new IntegerLiteralExpression CleanClone()
		{
			return (IntegerLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnIntegerLiteralExpression(this);
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
			IntegerLiteralExpression integerLiteralExpression = (IntegerLiteralExpression)node;
			if (_value != integerLiteralExpression._value)
			{
				return NoMatch("IntegerLiteralExpression._value");
			}
			if (_isLong != integerLiteralExpression._isLong)
			{
				return NoMatch("IntegerLiteralExpression._isLong");
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
			IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression();
			integerLiteralExpression._lexicalInfo = _lexicalInfo;
			integerLiteralExpression._endSourceLocation = _endSourceLocation;
			integerLiteralExpression._documentation = _documentation;
			integerLiteralExpression._isSynthetic = _isSynthetic;
			integerLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				integerLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			integerLiteralExpression._expressionType = _expressionType;
			integerLiteralExpression._value = _value;
			integerLiteralExpression._isLong = _isLong;
			return integerLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public IntegerLiteralExpression()
		{
		}

		public IntegerLiteralExpression(long value)
			: this(LexicalInfo.Empty, value)
		{
		}

		public IntegerLiteralExpression(LexicalInfo lexicalInfo, long value)
			: this(lexicalInfo, value, value > int.MaxValue)
		{
		}

		public IntegerLiteralExpression(LexicalInfo lexicalInfo, long value, bool isLong)
			: base(lexicalInfo)
		{
			Value = value;
			IsLong = isLong;
		}

		public IntegerLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
