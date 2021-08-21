using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceParameterDeclaration : ParameterDeclaration
	{
		protected ParameterDeclaration _parameterDeclaration;

		protected Expression _nameExpression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceParameterDeclaration;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public ParameterDeclaration ParameterDeclaration
		{
			get
			{
				return _parameterDeclaration;
			}
			set
			{
				if (_parameterDeclaration != value)
				{
					_parameterDeclaration = value;
					if (null != _parameterDeclaration)
					{
						_parameterDeclaration.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression NameExpression
		{
			get
			{
				return _nameExpression;
			}
			set
			{
				if (_nameExpression != value)
				{
					_nameExpression = value;
					if (null != _nameExpression)
					{
						_nameExpression.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceParameterDeclaration CloneNode()
		{
			return (SpliceParameterDeclaration)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceParameterDeclaration CleanClone()
		{
			return (SpliceParameterDeclaration)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceParameterDeclaration(this);
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
			SpliceParameterDeclaration spliceParameterDeclaration = (SpliceParameterDeclaration)node;
			if (_name != spliceParameterDeclaration._name)
			{
				return NoMatch("SpliceParameterDeclaration._name");
			}
			if (!Node.Matches(_type, spliceParameterDeclaration._type))
			{
				return NoMatch("SpliceParameterDeclaration._type");
			}
			if (_modifiers != spliceParameterDeclaration._modifiers)
			{
				return NoMatch("SpliceParameterDeclaration._modifiers");
			}
			if (!Node.AllMatch(_attributes, spliceParameterDeclaration._attributes))
			{
				return NoMatch("SpliceParameterDeclaration._attributes");
			}
			if (!Node.Matches(_parameterDeclaration, spliceParameterDeclaration._parameterDeclaration))
			{
				return NoMatch("SpliceParameterDeclaration._parameterDeclaration");
			}
			if (!Node.Matches(_nameExpression, spliceParameterDeclaration._nameExpression))
			{
				return NoMatch("SpliceParameterDeclaration._nameExpression");
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
			if (_type == existing)
			{
				base.Type = (TypeReference)newNode;
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
			if (_parameterDeclaration == existing)
			{
				ParameterDeclaration = (ParameterDeclaration)newNode;
				return true;
			}
			if (_nameExpression == existing)
			{
				NameExpression = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			SpliceParameterDeclaration spliceParameterDeclaration = new SpliceParameterDeclaration();
			spliceParameterDeclaration._lexicalInfo = _lexicalInfo;
			spliceParameterDeclaration._endSourceLocation = _endSourceLocation;
			spliceParameterDeclaration._documentation = _documentation;
			spliceParameterDeclaration._isSynthetic = _isSynthetic;
			spliceParameterDeclaration._entity = _entity;
			if (_annotations != null)
			{
				spliceParameterDeclaration._annotations = (Hashtable)_annotations.Clone();
			}
			spliceParameterDeclaration._name = _name;
			if (null != _type)
			{
				spliceParameterDeclaration._type = _type.Clone() as TypeReference;
				spliceParameterDeclaration._type.InitializeParent(spliceParameterDeclaration);
			}
			spliceParameterDeclaration._modifiers = _modifiers;
			if (null != _attributes)
			{
				spliceParameterDeclaration._attributes = _attributes.Clone() as AttributeCollection;
				spliceParameterDeclaration._attributes.InitializeParent(spliceParameterDeclaration);
			}
			if (null != _parameterDeclaration)
			{
				spliceParameterDeclaration._parameterDeclaration = _parameterDeclaration.Clone() as ParameterDeclaration;
				spliceParameterDeclaration._parameterDeclaration.InitializeParent(spliceParameterDeclaration);
			}
			if (null != _nameExpression)
			{
				spliceParameterDeclaration._nameExpression = _nameExpression.Clone() as Expression;
				spliceParameterDeclaration._nameExpression.InitializeParent(spliceParameterDeclaration);
			}
			return spliceParameterDeclaration;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
			if (null != _attributes)
			{
				_attributes.ClearTypeSystemBindings();
			}
			if (null != _parameterDeclaration)
			{
				_parameterDeclaration.ClearTypeSystemBindings();
			}
			if (null != _nameExpression)
			{
				_nameExpression.ClearTypeSystemBindings();
			}
		}

		public SpliceParameterDeclaration()
		{
		}

		public SpliceParameterDeclaration(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SpliceParameterDeclaration(ParameterDeclaration pd, Expression nameExpression)
			: base(pd.LexicalInfo)
		{
			ParameterDeclaration = pd;
			NameExpression = nameExpression;
		}
	}
}
