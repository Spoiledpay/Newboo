using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class StatementTypeMember : TypeMember
	{
		protected Statement _statement;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.StatementTypeMember;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Statement Statement
		{
			get
			{
				return _statement;
			}
			set
			{
				if (_statement != value)
				{
					_statement = value;
					if (null != _statement)
					{
						_statement.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new StatementTypeMember CloneNode()
		{
			return (StatementTypeMember)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new StatementTypeMember CleanClone()
		{
			return (StatementTypeMember)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnStatementTypeMember(this);
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
			StatementTypeMember statementTypeMember = (StatementTypeMember)node;
			if (_modifiers != statementTypeMember._modifiers)
			{
				return NoMatch("StatementTypeMember._modifiers");
			}
			if (_name != statementTypeMember._name)
			{
				return NoMatch("StatementTypeMember._name");
			}
			if (!Node.AllMatch(_attributes, statementTypeMember._attributes))
			{
				return NoMatch("StatementTypeMember._attributes");
			}
			if (!Node.Matches(_statement, statementTypeMember._statement))
			{
				return NoMatch("StatementTypeMember._statement");
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
			if (_attributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_attributes.Replace(attribute, newItem))
					{
						return true;
					}
				}
			}
			if (_statement == existing)
			{
				Statement = (Statement)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			StatementTypeMember statementTypeMember = new StatementTypeMember();
			statementTypeMember._lexicalInfo = _lexicalInfo;
			statementTypeMember._endSourceLocation = _endSourceLocation;
			statementTypeMember._documentation = _documentation;
			statementTypeMember._isSynthetic = _isSynthetic;
			statementTypeMember._entity = _entity;
			if (_annotations != null)
			{
				statementTypeMember._annotations = (Hashtable)_annotations.Clone();
			}
			statementTypeMember._modifiers = _modifiers;
			statementTypeMember._name = _name;
			if (null != _attributes)
			{
				statementTypeMember._attributes = _attributes.Clone() as AttributeCollection;
				statementTypeMember._attributes.InitializeParent(statementTypeMember);
			}
			if (null != _statement)
			{
				statementTypeMember._statement = _statement.Clone() as Statement;
				statementTypeMember._statement.InitializeParent(statementTypeMember);
			}
			return statementTypeMember;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _attributes)
			{
				_attributes.ClearTypeSystemBindings();
			}
			if (null != _statement)
			{
				_statement.ClearTypeSystemBindings();
			}
		}

		public StatementTypeMember()
		{
		}

		public StatementTypeMember(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public StatementTypeMember(Statement macro)
			: base(macro.LexicalInfo)
		{
			Statement = macro;
		}
	}
}
