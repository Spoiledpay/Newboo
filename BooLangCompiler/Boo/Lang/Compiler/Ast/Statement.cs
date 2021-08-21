using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(RaiseStatement))]
	[XmlInclude(typeof(BreakStatement))]
	[XmlInclude(typeof(ReturnStatement))]
	[XmlInclude(typeof(YieldStatement))]
	[XmlInclude(typeof(UnpackStatement))]
	[XmlInclude(typeof(ExpressionStatement))]
	[XmlInclude(typeof(MacroStatement))]
	[XmlInclude(typeof(TypeMemberStatement))]
	[XmlInclude(typeof(ContinueStatement))]
	[XmlInclude(typeof(DeclarationStatement))]
	[XmlInclude(typeof(TryStatement))]
	[XmlInclude(typeof(IfStatement))]
	[XmlInclude(typeof(UnlessStatement))]
	[XmlInclude(typeof(ForStatement))]
	[XmlInclude(typeof(WhileStatement))]
	public abstract class Statement : Node
	{
		protected StatementModifier _modifier;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public StatementModifier Modifier
		{
			get
			{
				return _modifier;
			}
			set
			{
				if (_modifier != value)
				{
					_modifier = value;
					if (null != _modifier)
					{
						_modifier.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Statement CloneNode()
		{
			return (Statement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Statement CleanClone()
		{
			return (Statement)base.CleanClone();
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
			Statement statement = (Statement)node;
			if (!Node.Matches(_modifier, statement._modifier))
			{
				return NoMatch("Statement._modifier");
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
				Modifier = (StatementModifier)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			throw new InvalidOperationException("Cannot clone abstract class: Statement");
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
		}

		public static Statement Lift(Statement node)
		{
			return node;
		}

		public static Statement Lift(Expression node)
		{
			ExpressionStatement expressionStatement = new ExpressionStatement(node);
			expressionStatement.IsSynthetic = node.IsSynthetic;
			return expressionStatement;
		}

		public Statement()
		{
		}

		public Statement(StatementModifier modifier)
		{
			Modifier = modifier;
		}

		public Statement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public virtual Block ToBlock()
		{
			Block block = new Block(base.LexicalInfo);
			block.Add(this);
			return block;
		}

		public void ReplaceBy(Statement other)
		{
			Block block = (Block)base.ParentNode;
			if (null == block)
			{
				throw new InvalidOperationException();
			}
			block.Statements.Replace(this, other);
		}
	}
}
