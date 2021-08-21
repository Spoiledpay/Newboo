using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ReturnStatement : Statement
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ReturnStatement;

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
		public new ReturnStatement CloneNode()
		{
			return (ReturnStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ReturnStatement CleanClone()
		{
			return (ReturnStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnReturnStatement(this);
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
			ReturnStatement returnStatement = (ReturnStatement)node;
			if (!Node.Matches(_modifier, returnStatement._modifier))
			{
				return NoMatch("ReturnStatement._modifier");
			}
			if (!Node.Matches(_expression, returnStatement._expression))
			{
				return NoMatch("ReturnStatement._expression");
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
			ReturnStatement returnStatement = new ReturnStatement();
			returnStatement._lexicalInfo = _lexicalInfo;
			returnStatement._endSourceLocation = _endSourceLocation;
			returnStatement._documentation = _documentation;
			returnStatement._isSynthetic = _isSynthetic;
			returnStatement._entity = _entity;
			if (_annotations != null)
			{
				returnStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				returnStatement._modifier = _modifier.Clone() as StatementModifier;
				returnStatement._modifier.InitializeParent(returnStatement);
			}
			if (null != _expression)
			{
				returnStatement._expression = _expression.Clone() as Expression;
				returnStatement._expression.InitializeParent(returnStatement);
			}
			return returnStatement;
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

		public ReturnStatement()
		{
		}

		public ReturnStatement(Expression expression)
		{
			Expression = expression;
		}

		public ReturnStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public ReturnStatement(LexicalInfo li, Expression expression, StatementModifier modifier)
			: base(li)
		{
			Expression = expression;
			base.Modifier = modifier;
		}

		public ReturnStatement(LexicalInfo li, Expression expression)
			: base(li)
		{
			Expression = expression;
		}

		public ReturnStatement(Expression expression, StatementModifier modifier)
		{
			Expression = expression;
			base.Modifier = modifier;
		}
	}
}
