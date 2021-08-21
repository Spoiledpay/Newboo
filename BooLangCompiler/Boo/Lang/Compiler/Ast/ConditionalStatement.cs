using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public abstract class ConditionalStatement : Statement
	{
		protected Expression _condition;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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
		public new ConditionalStatement CloneNode()
		{
			return (ConditionalStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ConditionalStatement CleanClone()
		{
			return (ConditionalStatement)base.CleanClone();
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
			ConditionalStatement conditionalStatement = (ConditionalStatement)node;
			if (!Node.Matches(_modifier, conditionalStatement._modifier))
			{
				return NoMatch("ConditionalStatement._modifier");
			}
			if (!Node.Matches(_condition, conditionalStatement._condition))
			{
				return NoMatch("ConditionalStatement._condition");
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
			if (_condition == existing)
			{
				Condition = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			throw new InvalidOperationException("Cannot clone abstract class: ConditionalStatement");
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
			if (null != _condition)
			{
				_condition.ClearTypeSystemBindings();
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public ConditionalStatement()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public ConditionalStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
