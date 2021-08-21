using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(Destructor))]
	[XmlInclude(typeof(Constructor))]
	public class Method : CallableDefinition, IExplicitMember, INodeWithBody
	{
		protected Block _body;

		protected LocalCollection _locals;

		protected MethodImplementationFlags _implementationFlags;

		protected ExplicitMemberInfo _explicitInfo;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Method;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block Body
		{
			get
			{
				if (_body == null)
				{
					_body = new Block();
					_body.InitializeParent(this);
				}
				return _body;
			}
			set
			{
				if (_body != value)
				{
					_body = value;
					if (null != _body)
					{
						_body.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[XmlArrayItem(typeof(Local))]
		[GeneratedCode("astgen.boo", "1")]
		public LocalCollection Locals
		{
			get
			{
				return _locals ?? (_locals = new LocalCollection(this));
			}
			set
			{
				if (_locals != value)
				{
					_locals = value;
					if (null != _locals)
					{
						_locals.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public MethodImplementationFlags ImplementationFlags
		{
			get
			{
				return _implementationFlags;
			}
			set
			{
				_implementationFlags = value;
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

		public bool IsRuntime => MethodImplementationFlags.Runtime == (_implementationFlags & MethodImplementationFlags.Runtime);

		public override TypeDefinition DeclaringType
		{
			get
			{
				if (null != base.ParentNode && (NodeType.Property == base.ParentNode.NodeType || NodeType.Event == base.ParentNode.NodeType))
				{
					return base.ParentNode.ParentNode as TypeDefinition;
				}
				return base.DeclaringType;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Method CloneNode()
		{
			return (Method)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Method CleanClone()
		{
			return (Method)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnMethod(this);
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
			Method method = (Method)node;
			if (_modifiers != method._modifiers)
			{
				return NoMatch("Method._modifiers");
			}
			if (_name != method._name)
			{
				return NoMatch("Method._name");
			}
			if (!Node.AllMatch(_attributes, method._attributes))
			{
				return NoMatch("Method._attributes");
			}
			if (!Node.AllMatch(_parameters, method._parameters))
			{
				return NoMatch("Method._parameters");
			}
			if (!Node.AllMatch(_genericParameters, method._genericParameters))
			{
				return NoMatch("Method._genericParameters");
			}
			if (!Node.Matches(_returnType, method._returnType))
			{
				return NoMatch("Method._returnType");
			}
			if (!Node.AllMatch(_returnTypeAttributes, method._returnTypeAttributes))
			{
				return NoMatch("Method._returnTypeAttributes");
			}
			if (!Node.Matches(_body, method._body))
			{
				return NoMatch("Method._body");
			}
			if (!Node.AllMatch(_locals, method._locals))
			{
				return NoMatch("Method._locals");
			}
			if (_implementationFlags != method._implementationFlags)
			{
				return NoMatch("Method._implementationFlags");
			}
			if (!Node.Matches(_explicitInfo, method._explicitInfo))
			{
				return NoMatch("Method._explicitInfo");
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
				base.ReturnType = (TypeReference)newNode;
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
			if (_body == existing)
			{
				Body = (Block)newNode;
				return true;
			}
			if (_locals != null)
			{
				Local local = existing as Local;
				if (null != local)
				{
					Local newItem4 = (Local)newNode;
					if (_locals.Replace(local, newItem4))
					{
						return true;
					}
				}
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
			Method method = new Method();
			method._lexicalInfo = _lexicalInfo;
			method._endSourceLocation = _endSourceLocation;
			method._documentation = _documentation;
			method._isSynthetic = _isSynthetic;
			method._entity = _entity;
			if (_annotations != null)
			{
				method._annotations = (Hashtable)_annotations.Clone();
			}
			method._modifiers = _modifiers;
			method._name = _name;
			if (null != _attributes)
			{
				method._attributes = _attributes.Clone() as AttributeCollection;
				method._attributes.InitializeParent(method);
			}
			if (null != _parameters)
			{
				method._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				method._parameters.InitializeParent(method);
			}
			if (null != _genericParameters)
			{
				method._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				method._genericParameters.InitializeParent(method);
			}
			if (null != _returnType)
			{
				method._returnType = _returnType.Clone() as TypeReference;
				method._returnType.InitializeParent(method);
			}
			if (null != _returnTypeAttributes)
			{
				method._returnTypeAttributes = _returnTypeAttributes.Clone() as AttributeCollection;
				method._returnTypeAttributes.InitializeParent(method);
			}
			if (null != _body)
			{
				method._body = _body.Clone() as Block;
				method._body.InitializeParent(method);
			}
			if (null != _locals)
			{
				method._locals = _locals.Clone() as LocalCollection;
				method._locals.InitializeParent(method);
			}
			method._implementationFlags = _implementationFlags;
			if (null != _explicitInfo)
			{
				method._explicitInfo = _explicitInfo.Clone() as ExplicitMemberInfo;
				method._explicitInfo.InitializeParent(method);
			}
			return method;
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
			if (null != _body)
			{
				_body.ClearTypeSystemBindings();
			}
			if (null != _locals)
			{
				_locals.ClearTypeSystemBindings();
			}
			if (null != _explicitInfo)
			{
				_explicitInfo.ClearTypeSystemBindings();
			}
		}

		public Method()
		{
		}

		public Method(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public Method(string name)
		{
			base.Name = name;
		}
	}
}
