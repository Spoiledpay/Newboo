using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ConditionalExpression : Expression
	{
		protected Expression _condition;

		protected Expression _trueValue;

		protected Expression _falseValue;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ConditionalExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Condition
		{
			get
			{
				return _condition;
			}
			set
			{
				if (_condition != value)
				{
					_condition = value;
					if (null != _condition)
					{
						_condition.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression TrueValue
		{
			get
			{
				return _trueValue;
			}
			set
			{
				if (_trueValue != value)
				{
					_trueValue = value;
					if (null != _trueValue)
					{
						_trueValue.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression FalseValue
		{
			get
			{
				return _falseValue;
			}
			set
			{
				if (_falseValue != value)
				{
					_falseValue = value;
					if (null != _falseValue)
					{
						_falseValue.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ConditionalExpression CloneNode()
		{
			return (ConditionalExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ConditionalExpression CleanClone()
		{
			return (ConditionalExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnConditionalExpression(this);
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
			ConditionalExpression conditionalExpression = (ConditionalExpression)node;
			if (!Node.Matches(_condition, conditionalExpression._condition))
			{
				return NoMatch("ConditionalExpression._condition");
			}
			if (!Node.Matches(_trueValue, conditionalExpression._trueValue))
			{
				return NoMatch("ConditionalExpression._trueValue");
			}
			if (!Node.Matches(_falseValue, conditionalExpression._falseValue))
			{
				return NoMatch("ConditionalExpression._falseValue");
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
			if (_condition == existing)
			{
				Condition = (Expression)newNode;
				return true;
			}
			if (_trueValue == existing)
			{
				TrueValue = (Expression)newNode;
				return true;
			}
			if (_falseValue == existing)
			{
				FalseValue = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ConditionalExpression conditionalExpression = new ConditionalExpression();
			conditionalExpression._lexicalInfo = _lexicalInfo;
			conditionalExpression._endSourceLocation = _endSourceLocation;
			conditionalExpression._documentation = _documentation;
			conditionalExpression._isSynthetic = _isSynthetic;
			conditionalExpression._entity = _entity;
			if (_annotations != null)
			{
				conditionalExpression._annotations = (Hashtable)_annotations.Clone();
			}
			conditionalExpression._expressionType = _expressionType;
			if (null != _condition)
			{
				conditionalExpression._condition = _condition.Clone() as Expression;
				conditionalExpression._condition.InitializeParent(conditionalExpression);
			}
			if (null != _trueValue)
			{
				conditionalExpression._trueValue = _trueValue.Clone() as Expression;
				conditionalExpression._trueValue.InitializeParent(conditionalExpression);
			}
			if (null != _falseValue)
			{
				conditionalExpression._falseValue = _falseValue.Clone() as Expression;
				conditionalExpression._falseValue.InitializeParent(conditionalExpression);
			}
			return conditionalExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _condition)
			{
				_condition.ClearTypeSystemBindings();
			}
			if (null != _trueValue)
			{
				_trueValue.ClearTypeSystemBindings();
			}
			if (null != _falseValue)
			{
				_falseValue.ClearTypeSystemBindings();
			}
		}

		public ConditionalExpression()
		{
		}

		public ConditionalExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
