using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class StatementModifier : Node
	{
		protected StatementModifierType _type;

		protected Expression _condition;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.StatementModifier;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public StatementModifierType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

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
		public new StatementModifier CloneNode()
		{
			return (StatementModifier)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new StatementModifier CleanClone()
		{
			return (StatementModifier)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnStatementModifier(this);
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
			StatementModifier statementModifier = (StatementModifier)node;
			if (_type != statementModifier._type)
			{
				return NoMatch("StatementModifier._type");
			}
			if (!Node.Matches(_condition, statementModifier._condition))
			{
				return NoMatch("StatementModifier._condition");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			StatementModifier statementModifier = new StatementModifier();
			statementModifier._lexicalInfo = _lexicalInfo;
			statementModifier._endSourceLocation = _endSourceLocation;
			statementModifier._documentation = _documentation;
			statementModifier._isSynthetic = _isSynthetic;
			statementModifier._entity = _entity;
			if (_annotations != null)
			{
				statementModifier._annotations = (Hashtable)_annotations.Clone();
			}
			statementModifier._type = _type;
			if (null != _condition)
			{
				statementModifier._condition = _condition.Clone() as Expression;
				statementModifier._condition.InitializeParent(statementModifier);
			}
			return statementModifier;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _condition)
			{
				_condition.ClearTypeSystemBindings();
			}
		}

		public StatementModifier()
		{
		}

		public StatementModifier(StatementModifierType type, Expression condition)
		{
			Type = type;
			Condition = condition;
		}

		public StatementModifier(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
