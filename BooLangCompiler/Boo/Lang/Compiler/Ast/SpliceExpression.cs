using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceExpression : Expression
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			set
			{
				if (_expression != value)
				{
					_expression = value;
					if (null != _expression)
					{
						_expression.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceExpression CloneNode()
		{
			return (SpliceExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceExpression CleanClone()
		{
			return (SpliceExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceExpression(this);
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
			SpliceExpression spliceExpression = (SpliceExpression)node;
			if (!Node.Matches(_expression, spliceExpression._expression))
			{
				return NoMatch("SpliceExpression._expression");
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
			if (_expression == existing)
			{
				Expression = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			SpliceExpression spliceExpression = new SpliceExpression();
			spliceExpression._lexicalInfo = _lexicalInfo;
			spliceExpression._endSourceLocation = _endSourceLocation;
			spliceExpression._documentation = _documentation;
			spliceExpression._isSynthetic = _isSynthetic;
			spliceExpression._entity = _entity;
			if (_annotations != null)
			{
				spliceExpression._annotations = (Hashtable)_annotations.Clone();
			}
			spliceExpression._expressionType = _expressionType;
			if (null != _expression)
			{
				spliceExpression._expression = _expression.Clone() as Expression;
				spliceExpression._expression.InitializeParent(spliceExpression);
			}
			return spliceExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
		}

		public SpliceExpression()
		{
		}

		public SpliceExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SpliceExpression(Expression e)
			: this(e.LexicalInfo, e)
		{
		}

		public SpliceExpression(LexicalInfo lexicalInfo, Expression e)
			: base(lexicalInfo)
		{
			Expression = e;
		}
	}
}
