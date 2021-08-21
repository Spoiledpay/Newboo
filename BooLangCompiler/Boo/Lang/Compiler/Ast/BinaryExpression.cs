using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class BinaryExpression : Expression
	{
		protected BinaryOperatorType _operator;

		protected Expression _left;

		protected Expression _right;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.BinaryExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public BinaryOperatorType Operator
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
		public Expression Left
		{
			get
			{
				return _left;
			}
			set
			{
				if (_left != value)
				{
					_left = value;
					if (null != _left)
					{
						_left.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Right
		{
			get
			{
				return _right;
			}
			set
			{
				if (_right != value)
				{
					_right = value;
					if (null != _right)
					{
						_right.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new BinaryExpression CloneNode()
		{
			return (BinaryExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new BinaryExpression CleanClone()
		{
			return (BinaryExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnBinaryExpression(this);
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
			BinaryExpression binaryExpression = (BinaryExpression)node;
			if (_operator != binaryExpression._operator)
			{
				return NoMatch("BinaryExpression._operator");
			}
			if (!Node.Matches(_left, binaryExpression._left))
			{
				return NoMatch("BinaryExpression._left");
			}
			if (!Node.Matches(_right, binaryExpression._right))
			{
				return NoMatch("BinaryExpression._right");
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
			if (_left == existing)
			{
				Left = (Expression)newNode;
				return true;
			}
			if (_right == existing)
			{
				Right = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			BinaryExpression binaryExpression = new BinaryExpression();
			binaryExpression._lexicalInfo = _lexicalInfo;
			binaryExpression._endSourceLocation = _endSourceLocation;
			binaryExpression._documentation = _documentation;
			binaryExpression._isSynthetic = _isSynthetic;
			binaryExpression._entity = _entity;
			if (_annotations != null)
			{
				binaryExpression._annotations = (Hashtable)_annotations.Clone();
			}
			binaryExpression._expressionType = _expressionType;
			binaryExpression._operator = _operator;
			if (null != _left)
			{
				binaryExpression._left = _left.Clone() as Expression;
				binaryExpression._left.InitializeParent(binaryExpression);
			}
			if (null != _right)
			{
				binaryExpression._right = _right.Clone() as Expression;
				binaryExpression._right.InitializeParent(binaryExpression);
			}
			return binaryExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _left)
			{
				_left.ClearTypeSystemBindings();
			}
			if (null != _right)
			{
				_right.ClearTypeSystemBindings();
			}
		}

		public BinaryExpression()
		{
		}

		public BinaryExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public BinaryExpression(BinaryOperatorType operator_, Expression left, Expression right)
			: this(LexicalInfo.Empty, operator_, left, right)
		{
		}

		public BinaryExpression(LexicalInfo lexicalInfoProvider, BinaryOperatorType operator_, Expression left, Expression right)
			: base(lexicalInfoProvider)
		{
			Operator = operator_;
			Left = left;
			Right = right;
		}
	}
}
