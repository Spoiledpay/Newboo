using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Property : TypeMember, INodeWithParameters, IExplicitMember
	{
		protected ParameterDeclarationCollection _parameters;

		protected Method _getter;

		protected Method _setter;

		protected TypeReference _type;

		protected ExplicitMemberInfo _explicitInfo;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Property;

		[XmlArrayItem(typeof(ParameterDeclaration))]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		public ParameterDeclarationCollection Parameters
		{
			get
			{
				return _parameters ?? (_parameters = new ParameterDeclarationCollection(this));
			}
			set
			{
				if (_parameters != value)
				{
					_parameters = value;
					if (null != _parameters)
					{
						_parameters.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Method Getter
		{
			get
			{
				return _getter;
			}
			set
			{
				if (_getter != value)
				{
					_getter = value;
					if (null != _getter)
					{
						_getter.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Method Setter
		{
			get
			{
				return _setter;
			}
			set
			{
				if (_setter != value)
				{
					_setter = value;
					if (null != _setter)
					{
						_setter.InitializeParent(this);
					}
				}
			}
		}

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
		public ExplicitMemberInfo ExplicitInfo
		{
			get
			{
				return _explicitInfo;
			}
			set
			{
				if (_explicitInfo != value)
				{
					_explicitInfo = value;
					if (null != _explicitInfo)
					{
						_explicitInfo.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Property CloneNode()
		{
			return (Property)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Property CleanClone()
		{
			return (Property)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnProperty(this);
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
			Property property = (Property)node;
			if (_modifiers != property._modifiers)
			{
				return NoMatch("Property._modifiers");
			}
			if (_name != property._name)
			{
				return NoMatch("Property._name");
			}
			if (!Node.AllMatch(_attributes, property._attributes))
			{
				return NoMatch("Property._attributes");
			}
			if (!Node.AllMatch(_parameters, property._parameters))
			{
				return NoMatch("Property._parameters");
			}
			if (!Node.Matches(_getter, property._getter))
			{
				return NoMatch("Property._getter");
			}
			if (!Node.Matches(_setter, property._setter))
			{
				return NoMatch("Property._setter");
			}
			if (!Node.Matches(_type, property._type))
			{
				return NoMatch("Property._type");
			}
			if (!Node.Matches(_explicitInfo, property._explicitInfo))
			{
				return NoMatch("Property._explicitInfo");
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
			if (_parameters != null)
			{
				ParameterDeclaration parameterDeclaration = existing as ParameterDeclaration;
				if (null != parameterDeclaration)
				{
					ParameterDeclaration newItem2 = (ParameterDeclaration)newNode;
					if (_parameters.Replace(parameterDeclaration, newItem2))
					{
						return true;
					}
				}
			}
			if (_getter == existing)
			{
				Getter = (Method)newNode;
				return true;
			}
			if (_setter == existing)
			{
				Setter = (Method)newNode;
				return true;
			}
			if (_type == existing)
			{
				Type = (TypeReference)newNode;
				return true;
			}
			if (_explicitInfo == existing)
			{
				ExplicitInfo = (ExplicitMemberInfo)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Property property = new Property();
			property._lexicalInfo = _lexicalInfo;
			property._endSourceLocation = _endSourceLocation;
			property._documentation = _documentation;
			property._isSynthetic = _isSynthetic;
			property._entity = _entity;
			if (_annotations != null)
			{
				property._annotations = (Hashtable)_annotations.Clone();
			}
			property._modifiers = _modifiers;
			property._name = _name;
			if (null != _attributes)
			{
				property._attributes = _attributes.Clone() as AttributeCollection;
				property._attributes.InitializeParent(property);
			}
			if (null != _parameters)
			{
				property._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				property._parameters.InitializeParent(property);
			}
			if (null != _getter)
			{
				property._getter = _getter.Clone() as Method;
				property._getter.InitializeParent(property);
			}
			if (null != _setter)
			{
				property._setter = _setter.Clone() as Method;
				property._setter.InitializeParent(property);
			}
			if (null != _type)
			{
				property._type = _type.Clone() as TypeReference;
				property._type.InitializeParent(property);
			}
			if (null != _explicitInfo)
			{
				property._explicitInfo = _explicitInfo.Clone() as ExplicitMemberInfo;
				property._explicitInfo.InitializeParent(property);
			}
			return property;
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
			if (null != _parameters)
			{
				_parameters.ClearTypeSystemBindings();
			}
			if (null != _getter)
			{
				_getter.ClearTypeSystemBindings();
			}
			if (null != _setter)
			{
				_setter.ClearTypeSystemBindings();
			}
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
			if (null != _explicitInfo)
			{
				_explicitInfo.ClearTypeSystemBindings();
			}
		}

		public Property()
		{
		}

		public Property(string name)
		{
			base.Name = name;
		}

		public Property(Method getter, Method setter, TypeReference type)
		{
			Getter = getter;
			Setter = setter;
			Type = type;
		}

		public Property(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
