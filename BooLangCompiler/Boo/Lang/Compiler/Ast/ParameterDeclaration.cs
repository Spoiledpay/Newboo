using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ParameterDeclaration : Node, INodeWithAttributes
	{
		protected string _name;

		protected TypeReference _type;

		protected ParameterModifiers _modifiers;

		protected AttributeCollection _attributes;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ParameterDeclaration;

		[GeneratedCode("astgen.boo", "1")]
		[XmlAttribute]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
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

		[XmlAttribute]
		[DefaultValue(ParameterModifiers.None)]
		[GeneratedCode("astgen.boo", "1")]
		public ParameterModifiers Modifiers
		{
			get
			{
				return _modifiers;
			}
			set
			{
				_modifiers = value;
			}
		}

		[XmlArrayItem(typeof(Attribute))]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		public AttributeCollection Attributes
		{
			get
			{
				return _attributes ?? (_attributes = new AttributeCollection(this));
			}
			set
			{
				if (_attributes != value)
				{
					_attributes = value;
					if (null != _attributes)
					{
						_attributes.InitializeParent(this);
					}
				}
			}
		}

		public bool IsByRef => (Modifiers & ParameterModifiers.Ref) == ParameterModifiers.Ref;

		[XmlIgnore]
		public bool IsParamArray
		{
			get
			{
				INodeWithParameters nodeWithParameters = base.ParentNode as INodeWithParameters;
				return nodeWithParameters != null && nodeWithParameters.Parameters.HasParamArray && this == nodeWithParameters.Parameters.Last;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ParameterDeclaration CloneNode()
		{
			return (ParameterDeclaration)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ParameterDeclaration CleanClone()
		{
			return (ParameterDeclaration)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnParameterDeclaration(this);
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
			ParameterDeclaration parameterDeclaration = (ParameterDeclaration)node;
			if (_name != parameterDeclaration._name)
			{
				return NoMatch("ParameterDeclaration._name");
			}
			if (!Node.Matches(_type, parameterDeclaration._type))
			{
				return NoMatch("ParameterDeclaration._type");
			}
			if (_modifiers != parameterDeclaration._modifiers)
			{
				return NoMatch("ParameterDeclaration._modifiers");
			}
			if (!Node.AllMatch(_attributes, parameterDeclaration._attributes))
			{
				return NoMatch("ParameterDeclaration._attributes");
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
				Type = (TypeReference)newNode;
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ParameterDeclaration parameterDeclaration = new ParameterDeclaration();
			parameterDeclaration._lexicalInfo = _lexicalInfo;
			parameterDeclaration._endSourceLocation = _endSourceLocation;
			parameterDeclaration._documentation = _documentation;
			parameterDeclaration._isSynthetic = _isSynthetic;
			parameterDeclaration._entity = _entity;
			if (_annotations != null)
			{
				parameterDeclaration._annotations = (Hashtable)_annotations.Clone();
			}
			parameterDeclaration._name = _name;
			if (null != _type)
			{
				parameterDeclaration._type = _type.Clone() as TypeReference;
				parameterDeclaration._type.InitializeParent(parameterDeclaration);
			}
			parameterDeclaration._modifiers = _modifiers;
			if (null != _attributes)
			{
				parameterDeclaration._attributes = _attributes.Clone() as AttributeCollection;
				parameterDeclaration._attributes.InitializeParent(parameterDeclaration);
			}
			return parameterDeclaration;
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
		}

		public static ParameterDeclaration Lift(Expression e)
		{
			if (e == null)
			{
				return null;
			}
			return e.NodeType switch
			{
				NodeType.TryCastExpression => Lift((TryCastExpression)e), 
				NodeType.ReferenceExpression => Lift((ReferenceExpression)e), 
				_ => throw new NotImplementedException(e.ToCodeString()), 
			};
		}

		public static ParameterDeclaration Lift(ReferenceExpression referenceExpression)
		{
			return new ParameterDeclaration(referenceExpression.Name, null);
		}

		public static ParameterDeclaration Lift(TryCastExpression castExpression)
		{
			return new ParameterDeclaration(ParameterNameFrom(castExpression.Target), castExpression.Type.CloneNode());
		}

		private static string ParameterNameFrom(Expression target)
		{
			return target.NodeType switch
			{
				NodeType.ReferenceExpression => ((ReferenceExpression)target).Name, 
				NodeType.SelfLiteralExpression => "self", 
				_ => throw new ArgumentException(target.ToCodeString()), 
			};
		}

		public ParameterDeclaration()
		{
		}

		public ParameterDeclaration(string name, TypeReference type, ParameterModifiers modifiers)
		{
			Name = name;
			Type = type;
			Modifiers = modifiers;
		}

		public ParameterDeclaration(string name, TypeReference type)
			: this(name, type, ParameterModifiers.None)
		{
		}

		public ParameterDeclaration(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
