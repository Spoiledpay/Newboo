using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TimeSpanLiteralExpression : LiteralExpression
	{
		protected TimeSpan _value;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.TimeSpanLiteralExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public TimeSpan Value
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
		public new TimeSpanLiteralExpression CloneNode()
		{
			return (TimeSpanLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TimeSpanLiteralExpression CleanClone()
		{
			return (TimeSpanLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnTimeSpanLiteralExpression(this);
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
			TimeSpanLiteralExpression timeSpanLiteralExpression = (TimeSpanLiteralExpression)node;
			if (_value != timeSpanLiteralExpression._value)
			{
				return NoMatch("TimeSpanLiteralExpression._value");
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
			TimeSpanLiteralExpression timeSpanLiteralExpression = new TimeSpanLiteralExpression();
			timeSpanLiteralExpression._lexicalInfo = _lexicalInfo;
			timeSpanLiteralExpression._endSourceLocation = _endSourceLocation;
			timeSpanLiteralExpression._documentation = _documentation;
			timeSpanLiteralExpression._isSynthetic = _isSynthetic;
			timeSpanLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				timeSpanLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			timeSpanLiteralExpression._expressionType = _expressionType;
			timeSpanLiteralExpression._value = _value;
			return timeSpanLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public TimeSpanLiteralExpression()
		{
		}

		public TimeSpanLiteralExpression(TimeSpan value)
		{
			Value = value;
		}

		public TimeSpanLiteralExpression(LexicalInfo lexicalInfoProvider, TimeSpan value)
			: base(lexicalInfoProvider)
		{
			Value = value;
		}
	}
}
