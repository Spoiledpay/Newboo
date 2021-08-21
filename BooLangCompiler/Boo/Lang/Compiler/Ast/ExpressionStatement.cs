using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExpressionStatement : Statement
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExpressionStatement;

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
						base.LexicalInfo = value.LexicalInfo;
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionStatement CloneNode()
		{
			return (ExpressionStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionStatement CleanClone()
		{
			return (ExpressionStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExpressionStatement(this);
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
			ExpressionStatement expressionStatement = (ExpressionStatement)node;
			if (!Node.Matches(_modifier, expressionStatement._modifier))
			{
				return NoMatch("ExpressionStatement._modifier");
			}
			if (!Node.Matches(_expression, expressionStatement._expression))
			{
				return NoMatch("ExpressionStatement._expression");
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
			if (_modifier == existing)
			{
				base.Modifier = (StatementModifier)newNode;
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
			ExpressionStatement expressionStatement = new ExpressionStatement();
			expressionStatement._lexicalInfo = _lexicalInfo;
			expressionStatement._endSourceLocation = _endSourceLocation;
			expressionStatement._documentation = _documentation;
			expressionStatement._isSynthetic = _isSynthetic;
			expressionStatement._entity = _entity;
			if (_annotations != null)
			{
				expressionStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				expressionStatement._modifier = _modifier.Clone() as StatementModifier;
				expressionStatement._modifier.InitializeParent(expressionStatement);
			}
			if (null != _expression)
			{
				expressionStatement._expression = _expression.Clone() as Expression;
				expressionStatement._expression.InitializeParent(expressionStatement);
			}
			return expressionStatement;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _modifier)
			{
				_modifier.ClearTypeSystemBindings();
			}
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
		}

		public ExpressionStatement()
		{
		}

		public ExpressionStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public ExpressionStatement(Expression expression)
		{
			Expression = expression;
		}

		public ExpressionStatement(LexicalInfo lexicalInfo, Expression expression)
			: base(lexicalInfo)
		{
			Expression = expression;
		}

		public ExpressionStatement(LexicalInfo lexicalInfo, Expression expression, StatementModifier modifier)
			: base(lexicalInfo)
		{
			Expression = expression;
			base.Modifier = modifier;
		}
	}
}
