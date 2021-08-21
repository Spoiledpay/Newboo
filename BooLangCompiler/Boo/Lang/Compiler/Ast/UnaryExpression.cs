using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class UnaryExpression : Expression
	{
		protected UnaryOperatorType _operator;

		protected Expression _operand;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.UnaryExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public UnaryOperatorType Operator
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Operand
		{
			get
			{
				return _operand;
			}
			set
			{
				if (_operand != value)
				{
					_operand = value;
					if (null != _operand)
					{
						_operand.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new UnaryExpression CloneNode()
		{
			return (UnaryExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new UnaryExpression CleanClone()
		{
			return (UnaryExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnUnaryExpression(this);
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
			UnaryExpression unaryExpression = (UnaryExpression)node;
			if (_operator != unaryExpression._operator)
			{
				return NoMatch("UnaryExpression._operator");
			}
			if (!Node.Matches(_operand, unaryExpression._operand))
			{
				return NoMatch("UnaryExpression._operand");
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
			if (_operand == existing)
			{
				Operand = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			UnaryExpression unaryExpression = new UnaryExpression();
			unaryExpression._lexicalInfo = _lexicalInfo;
			unaryExpression._endSourceLocation = _endSourceLocation;
			unaryExpression._documentation = _documentation;
			unaryExpression._isSynthetic = _isSynthetic;
			unaryExpression._entity = _entity;
			if (_annotations != null)
			{
				unaryExpression._annotations = (Hashtable)_annotations.Clone();
			}
			unaryExpression._expressionType = _expressionType;
			unaryExpression._operator = _operator;
			if (null != _operand)
			{
				unaryExpression._operand = _operand.Clone() as Expression;
				unaryExpression._operand.InitializeParent(unaryExpression);
			}
			return unaryExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _operand)
			{
				_operand.ClearTypeSystemBindings();
			}
		}

		public UnaryExpression()
		{
		}

		public UnaryExpression(LexicalInfo lexicalInfo, UnaryOperatorType operator_, Expression operand)
			: base(lexicalInfo)
		{
			Operator = operator_;
			Operand = operand;
		}

		public UnaryExpression(UnaryOperatorType operator_, Expression operand)
			: this(LexicalInfo.Empty, operator_, operand)
		{
		}

		public UnaryExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
