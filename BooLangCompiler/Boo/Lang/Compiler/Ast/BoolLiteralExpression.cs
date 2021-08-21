using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class BoolLiteralExpression : LiteralExpression
	{
		protected bool _value;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.BoolLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public bool Value
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
		public new BoolLiteralExpression CloneNode()
		{
			return (BoolLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new BoolLiteralExpression CleanClone()
		{
			return (BoolLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnBoolLiteralExpression(this);
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
			BoolLiteralExpression boolLiteralExpression = (BoolLiteralExpression)node;
			if (_value != boolLiteralExpression._value)
			{
				return NoMatch("BoolLiteralExpression._value");
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
			BoolLiteralExpression boolLiteralExpression = new BoolLiteralExpression();
			boolLiteralExpression._lexicalInfo = _lexicalInfo;
			boolLiteralExpression._endSourceLocation = _endSourceLocation;
			boolLiteralExpression._documentation = _documentation;
			boolLiteralExpression._isSynthetic = _isSynthetic;
			boolLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				boolLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			boolLiteralExpression._expressionType = _expressionType;
			boolLiteralExpression._value = _value;
			return boolLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public BoolLiteralExpression()
		{
		}

		public BoolLiteralExpression(bool value)
		{
			Value = value;
		}

		public BoolLiteralExpression(LexicalInfo token, bool value)
			: base(token)
		{
			Value = value;
		}

		public BoolLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
