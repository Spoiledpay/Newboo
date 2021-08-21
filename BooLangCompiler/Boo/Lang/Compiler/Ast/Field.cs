using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Field : TypeMember
	{
		protected TypeReference _type;

		protected Expression _initializer;

		protected bool _isVolatile;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Field;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public TypeReference Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (_type != value)
				{
					_type = value;
					if (null != _type)
					{
						_type.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Initializer
		{
			get
			{
				return _initializer;
			}
			set
			{
				if (_initializer != value)
				{
					_initializer = value;
					if (null != _initializer)
					{
						_initializer.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public bool IsVolatile
		{
			get
			{
				return _isVolatile;
			}
			set
			{
				_isVolatile = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Field CloneNode()
		{
			return (Field)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Field CleanClone()
		{
			return (Field)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnField(this);
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
			Field field = (Field)node;
			if (_modifiers != field._modifiers)
			{
				return NoMatch("Field._modifiers");
			}
			if (_name != field._name)
			{
				return NoMatch("Field._name");
			}
			if (!Node.AllMatch(_attributes, field._attributes))
			{
				return NoMatch("Field._attributes");
			}
			if (!Node.Matches(_type, field._type))
			{
				return NoMatch("Field._type");
			}
			if (!Node.Matches(_initializer, field._initializer))
			{
				return NoMatch("Field._initializer");
			}
			if (_isVolatile != field._isVolatile)
			{
				return NoMatch("Field._isVolatile");
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
			if (_type == existing)
			{
				Type = (TypeReference)newNode;
				return true;
			}
			if (_initializer == existing)
			{
				Initializer = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Field field = new Field();
			field._lexicalInfo = _lexicalInfo;
			field._endSourceLocation = _endSourceLocation;
			field._documentation = _documentation;
			field._isSynthetic = _isSynthetic;
			field._entity = _entity;
			if (_annotations != null)
			{
				field._annotations = (Hashtable)_annotations.Clone();
			}
			field._modifiers = _modifiers;
			field._name = _name;
			if (null != _attributes)
			{
				field._attributes = _attributes.Clone() as AttributeCollection;
				field._attributes.InitializeParent(field);
			}
			if (null != _type)
			{
				field._type = _type.Clone() as TypeReference;
				field._type.InitializeParent(field);
			}
			if (null != _initializer)
			{
				field._initializer = _initializer.Clone() as Expression;
				field._initializer.InitializeParent(field);
			}
			field._isVolatile = _isVolatile;
			return field;
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
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
			if (null != _initializer)
			{
				_initializer.ClearTypeSystemBindings();
			}
		}

		public Field()
		{
		}

		public Field(TypeReference type, Expression initializer)
		{
			Type = type;
			Initializer = initializer;
		}

		public Field(LexicalInfo lexicalInfo, TypeReference type, Expression initializer)
			: base(lexicalInfo)
		{
			Type = type;
			Initializer = initializer;
		}

		public Field(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
