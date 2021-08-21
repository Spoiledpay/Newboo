using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class YieldStatement : Statement
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.YieldStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
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
		public new YieldStatement CloneNode()
		{
			return (YieldStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new YieldStatement CleanClone()
		{
			return (YieldStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnYieldStatement(this);
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
			YieldStatement yieldStatement = (YieldStatement)node;
			if (!Node.Matches(_modifier, yieldStatement._modifier))
			{
				return NoMatch("YieldStatement._modifier");
			}
			if (!Node.Matches(_expression, yieldStatement._expression))
			{
				return NoMatch("YieldStatement._expression");
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
			YieldStatement yieldStatement = new YieldStatement();
			yieldStatement._lexicalInfo = _lexicalInfo;
			yieldStatement._endSourceLocation = _endSourceLocation;
			yieldStatement._documentation = _documentation;
			yieldStatement._isSynthetic = _isSynthetic;
			yieldStatement._entity = _entity;
			if (_annotations != null)
			{
				yieldStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				yieldStatement._modifier = _modifier.Clone() as StatementModifier;
				yieldStatement._modifier.InitializeParent(yieldStatement);
			}
			if (null != _expression)
			{
				yieldStatement._expression = _expression.Clone() as Expression;
				yieldStatement._expression.InitializeParent(yieldStatement);
			}
			return yieldStatement;
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

		public YieldStatement()
		{
		}

		public YieldStatement(Expression expression)
		{
			Expression = expression;
		}

		public YieldStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public YieldStatement(LexicalInfo lexicalInfo, Expression expression)
			: base(lexicalInfo)
		{
			Expression = expression;
		}
	}
}
