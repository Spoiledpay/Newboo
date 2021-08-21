using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class DoubleLiteralExpression : LiteralExpression
	{
		protected double _value;

		protected bool _isSingle;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.DoubleLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public double Value
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
		public bool IsSingle
		{
			get
			{
				return _isSingle;
			}
			set
			{
				_isSingle = value;
			}
		}

		public override object ValueObject => Value;

		[GeneratedCode("astgen.boo", "1")]
		public new DoubleLiteralExpression CloneNode()
		{
			return (DoubleLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new DoubleLiteralExpression CleanClone()
		{
			return (DoubleLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnDoubleLiteralExpression(this);
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
			DoubleLiteralExpression doubleLiteralExpression = (DoubleLiteralExpression)node;
			if (_value != doubleLiteralExpression._value)
			{
				return NoMatch("DoubleLiteralExpression._value");
			}
			if (_isSingle != doubleLiteralExpression._isSingle)
			{
				return NoMatch("DoubleLiteralExpression._isSingle");
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
			DoubleLiteralExpression doubleLiteralExpression = new DoubleLiteralExpression();
			doubleLiteralExpression._lexicalInfo = _lexicalInfo;
			doubleLiteralExpression._endSourceLocation = _endSourceLocation;
			doubleLiteralExpression._documentation = _documentation;
			doubleLiteralExpression._isSynthetic = _isSynthetic;
			doubleLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				doubleLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			doubleLiteralExpression._expressionType = _expressionType;
			doubleLiteralExpression._value = _value;
			doubleLiteralExpression._isSingle = _isSingle;
			return doubleLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public DoubleLiteralExpression()
		{
		}

		public DoubleLiteralExpression(double value)
			: this(value, isSingle: false)
		{
		}

		public DoubleLiteralExpression(double value, bool isSingle)
			: this(LexicalInfo.Empty, value, isSingle)
		{
		}

		public DoubleLiteralExpression(LexicalInfo lexicalInfo, double value)
			: this(lexicalInfo, value, isSingle: false)
		{
		}

		public DoubleLiteralExpression(LexicalInfo lexicalInfo, double value, bool isSingle)
			: base(lexicalInfo)
		{
			Value = value;
			IsSingle = isSingle;
		}

		public DoubleLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
