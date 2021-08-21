using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CallableDefinition : TypeMember, INodeWithParameters, INodeWithGenericParameters
	{
		protected ParameterDeclarationCollection _parameters;

		protected GenericParameterDeclarationCollection _genericParameters;

		protected TypeReference _returnType;

		protected AttributeCollection _returnTypeAttributes;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CallableDefinition;

		[XmlArrayItem(typeof(ParameterDeclaration))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
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

		[XmlArrayItem(typeof(GenericParameterDeclaration))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public GenericParameterDeclarationCollection GenericParameters
		{
			get
			{
				return _genericParameters ?? (_genericParameters = new GenericParameterDeclarationCollection(this));
			}
			set
			{
				if (_genericParameters != value)
				{
					_genericParameters = value;
					if (null != _genericParameters)
					{
						_genericParameters.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public TypeReference ReturnType
		{
			get
			{
				return _returnType;
			}
			set
			{
				if (_returnType != value)
				{
					_returnType = value;
					if (null != _returnType)
					{
						_returnType.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[XmlArrayItem(typeof(Attribute))]
		[GeneratedCode("astgen.boo", "1")]
		public AttributeCollection ReturnTypeAttributes
		{
			get
			{
				return _returnTypeAttributes ?? (_returnTypeAttributes = new AttributeCollection(this));
			}
			set
			{
				if (_returnTypeAttributes != value)
				{
					_returnTypeAttributes = value;
					if (null != _returnTypeAttributes)
					{
						_returnTypeAttributes.InitializeParent(this);
					}
				}
			}
		}

		public override string FullName
		{
			get
			{
				if (NodeType.CallableDefinition == NodeType)
				{
					NamespaceDeclaration enclosingNamespace = EnclosingNamespace;
					if (null != enclosingNamespace)
					{
						return enclosingNamespace.Name + "." + base.Name;
					}
					return base.Name;
				}
				if (GenericParameters.Count > 0)
				{
					return $"{base.FullName}[of {GenericParameters.ToCodeString()}]";
				}
				return base.FullName;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CallableDefinition CloneNode()
		{
			return (CallableDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CallableDefinition CleanClone()
		{
			return (CallableDefinition)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCallableDefinition(this);
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
			CallableDefinition callableDefinition = (CallableDefinition)node;
			if (_modifiers != callableDefinition._modifiers)
			{
				return NoMatch("CallableDefinition._modifiers");
			}
			if (_name != callableDefinition._name)
			{
				return NoMatch("CallableDefinition._name");
			}
			if (!Node.AllMatch(_attributes, callableDefinition._attributes))
			{
				return NoMatch("CallableDefinition._attributes");
			}
			if (!Node.AllMatch(_parameters, callableDefinition._parameters))
			{
				return NoMatch("CallableDefinition._parameters");
			}
			if (!Node.AllMatch(_genericParameters, callableDefinition._genericParameters))
			{
				return NoMatch("CallableDefinition._genericParameters");
			}
			if (!Node.Matches(_returnType, callableDefinition._returnType))
			{
				return NoMatch("CallableDefinition._returnType");
			}
			if (!Node.AllMatch(_returnTypeAttributes, callableDefinition._returnTypeAttributes))
			{
				return NoMatch("CallableDefinition._returnTypeAttributes");
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
			if (_genericParameters != null)
			{
				GenericParameterDeclaration genericParameterDeclaration = existing as GenericParameterDeclaration;
				if (null != genericParameterDeclaration)
				{
					GenericParameterDeclaration newItem3 = (GenericParameterDeclaration)newNode;
					if (_genericParameters.Replace(genericParameterDeclaration, newItem3))
					{
						return true;
					}
				}
			}
			if (_returnType == existing)
			{
				ReturnType = (TypeReference)newNode;
				return true;
			}
			if (_returnTypeAttributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_returnTypeAttributes.Replace(attribute, newItem))
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
			CallableDefinition callableDefinition = new CallableDefinition();
			callableDefinition._lexicalInfo = _lexicalInfo;
			callableDefinition._endSourceLocation = _endSourceLocation;
			callableDefinition._documentation = _documentation;
			callableDefinition._isSynthetic = _isSynthetic;
			callableDefinition._entity = _entity;
			if (_annotations != null)
			{
				callableDefinition._annotations = (Hashtable)_annotations.Clone();
			}
			callableDefinition._modifiers = _modifiers;
			callableDefinition._name = _name;
			if (null != _attributes)
			{
				callableDefinition._attributes = _attributes.Clone() as AttributeCollection;
				callableDefinition._attributes.InitializeParent(callableDefinition);
			}
			if (null != _parameters)
			{
				callableDefinition._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				callableDefinition._parameters.InitializeParent(callableDefinition);
			}
			if (null != _genericParameters)
			{
				callableDefinition._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				callableDefinition._genericParameters.InitializeParent(callableDefinition);
			}
			if (null != _returnType)
			{
				callableDefinition._returnType = _returnType.Clone() as TypeReference;
				callableDefinition._returnType.InitializeParent(callableDefinition);
			}
			if (null != _returnTypeAttributes)
			{
				callableDefinition._returnTypeAttributes = _returnTypeAttributes.Clone() as AttributeCollection;
				callableDefinition._returnTypeAttributes.InitializeParent(callableDefinition);
			}
			return callableDefinition;
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
			if (null != _genericParameters)
			{
				_genericParameters.ClearTypeSystemBindings();
			}
			if (null != _returnType)
			{
				_returnType.ClearTypeSystemBindings();
			}
			if (null != _returnTypeAttributes)
			{
				_returnTypeAttributes.ClearTypeSystemBindings();
			}
		}

		public CallableDefinition()
		{
		}

		public CallableDefinition(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
