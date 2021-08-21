using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceTypeDefinitionBody : TypeMember
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceTypeDefinitionBody;

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
		public new SpliceTypeDefinitionBody CloneNode()
		{
			return (SpliceTypeDefinitionBody)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceTypeDefinitionBody CleanClone()
		{
			return (SpliceTypeDefinitionBody)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceTypeDefinitionBody(this);
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
			SpliceTypeDefinitionBody spliceTypeDefinitionBody = (SpliceTypeDefinitionBody)node;
			if (_modifiers != spliceTypeDefinitionBody._modifiers)
			{
				return NoMatch("SpliceTypeDefinitionBody._modifiers");
			}
			if (_name != spliceTypeDefinitionBody._name)
			{
				return NoMatch("SpliceTypeDefinitionBody._name");
			}
			if (!Node.AllMatch(_attributes, spliceTypeDefinitionBody._attributes))
			{
				return NoMatch("SpliceTypeDefinitionBody._attributes");
			}
			if (!Node.Matches(_expression, spliceTypeDefinitionBody._expression))
			{
				return NoMatch("SpliceTypeDefinitionBody._expression");
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
			SpliceTypeDefinitionBody spliceTypeDefinitionBody = new SpliceTypeDefinitionBody();
			spliceTypeDefinitionBody._lexicalInfo = _lexicalInfo;
			spliceTypeDefinitionBody._endSourceLocation = _endSourceLocation;
			spliceTypeDefinitionBody._documentation = _documentation;
			spliceTypeDefinitionBody._isSynthetic = _isSynthetic;
			spliceTypeDefinitionBody._entity = _entity;
			if (_annotations != null)
			{
				spliceTypeDefinitionBody._annotations = (Hashtable)_annotations.Clone();
			}
			spliceTypeDefinitionBody._modifiers = _modifiers;
			spliceTypeDefinitionBody._name = _name;
			if (null != _attributes)
			{
				spliceTypeDefinitionBody._attributes = _attributes.Clone() as AttributeCollection;
				spliceTypeDefinitionBody._attributes.InitializeParent(spliceTypeDefinitionBody);
			}
			if (null != _expression)
			{
				spliceTypeDefinitionBody._expression = _expression.Clone() as Expression;
				spliceTypeDefinitionBody._expression.InitializeParent(spliceTypeDefinitionBody);
			}
			return spliceTypeDefinitionBody;
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
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
		}

		public SpliceTypeDefinitionBody(Expression body)
		{
			Expression = body;
		}

		[GeneratedCode("astgen.boo", "1")]
		public SpliceTypeDefinitionBody()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public SpliceTypeDefinitionBody(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
