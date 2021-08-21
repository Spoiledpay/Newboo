using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExpressionInterpolationExpression : Expression
	{
		protected ExpressionCollection _expressions;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExpressionInterpolationExpression;

		[XmlArrayItem(typeof(Expression))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public ExpressionCollection Expressions
		{
			get
			{
				return _expressions ?? (_expressions = new ExpressionCollection(this));
			}
			set
			{
				if (_expressions != value)
				{
					_expressions = value;
					if (null != _expressions)
					{
						_expressions.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionInterpolationExpression CloneNode()
		{
			return (ExpressionInterpolationExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionInterpolationExpression CleanClone()
		{
			return (ExpressionInterpolationExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExpressionInterpolationExpression(this);
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
			ExpressionInterpolationExpression expressionInterpolationExpression = (ExpressionInterpolationExpression)node;
			if (!Node.AllMatch(_expressions, expressionInterpolationExpression._expressions))
			{
				return NoMatch("ExpressionInterpolationExpression._expressions");
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
			if (_expressions != null)
			{
				Expression expression = existing as Expression;
				if (null != expression)
				{
					Expression newItem = (Expression)newNode;
					if (_expressions.Replace(expression, newItem))
					{
						return true;
					}
				}
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ExpressionInterpolationExpression expressionInterpolationExpression = new ExpressionInterpolationExpression();
			expressionInterpolationExpression._lexicalInfo = _lexicalInfo;
			expressionInterpolationExpression._endSourceLocation = _endSourceLocation;
			expressionInterpolationExpression._documentation = _documentation;
			expressionInterpolationExpression._isSynthetic = _isSynthetic;
			expressionInterpolationExpression._entity = _entity;
			if (_annotations != null)
			{
				expressionInterpolationExpression._annotations = (Hashtable)_annotations.Clone();
			}
			expressionInterpolationExpression._expressionType = _expressionType;
			if (null != _expressions)
			{
				expressionInterpolationExpression._expressions = _expressions.Clone() as ExpressionCollection;
				expressionInterpolationExpression._expressions.InitializeParent(expressionInterpolationExpression);
			}
			return expressionInterpolationExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _expressions)
			{
				_expressions.ClearTypeSystemBindings();
			}
		}

		public ExpressionInterpolationExpression()
		{
		}

		public ExpressionInterpolationExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
