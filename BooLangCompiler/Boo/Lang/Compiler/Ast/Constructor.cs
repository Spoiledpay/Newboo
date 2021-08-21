using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Constructor : Method
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Constructor;

		[GeneratedCode("astgen.boo", "1")]
		public new Constructor CloneNode()
		{
			return (Constructor)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Constructor CleanClone()
		{
			return (Constructor)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnConstructor(this);
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
			Constructor constructor = (Constructor)node;
			if (_modifiers != constructor._modifiers)
			{
				return NoMatch("Constructor._modifiers");
			}
			if (_name != constructor._name)
			{
				return NoMatch("Constructor._name");
			}
			if (!Node.AllMatch(_attributes, constructor._attributes))
			{
				return NoMatch("Constructor._attributes");
			}
			if (!Node.AllMatch(_parameters, constructor._parameters))
			{
				return NoMatch("Constructor._parameters");
			}
			if (!Node.AllMatch(_genericParameters, constructor._genericParameters))
			{
				return NoMatch("Constructor._genericParameters");
			}
			if (!Node.Matches(_returnType, constructor._returnType))
			{
				return NoMatch("Constructor._returnType");
			}
			if (!Node.AllMatch(_returnTypeAttributes, constructor._returnTypeAttributes))
			{
				return NoMatch("Constructor._returnTypeAttributes");
			}
			if (!Node.Matches(_body, constructor._body))
			{
				return NoMatch("Constructor._body");
			}
			if (!Node.AllMatch(_locals, constructor._locals))
			{
				return NoMatch("Constructor._locals");
			}
			if (_implementationFlags != constructor._implementationFlags)
			{
				return NoMatch("Constructor._implementationFlags");
			}
			if (!Node.Matches(_explicitInfo, constructor._explicitInfo))
			{
				return NoMatch("Constructor._explicitInfo");
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
				base.Body = (Block)newNode;
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
				base.ExplicitInfo = (ExplicitMemberInfo)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Constructor constructor = new Constructor();
			constructor._lexicalInfo = _lexicalInfo;
			constructor._endSourceLocation = _endSourceLocation;
			constructor._documentation = _documentation;
			constructor._isSynthetic = _isSynthetic;
			constructor._entity = _entity;
			if (_annotations != null)
			{
				constructor._annotations = (Hashtable)_annotations.Clone();
			}
			constructor._modifiers = _modifiers;
			constructor._name = _name;
			if (null != _attributes)
			{
				constructor._attributes = _attributes.Clone() as AttributeCollection;
				constructor._attributes.InitializeParent(constructor);
			}
			if (null != _parameters)
			{
				constructor._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				constructor._parameters.InitializeParent(constructor);
			}
			if (null != _genericParameters)
			{
				constructor._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				constructor._genericParameters.InitializeParent(constructor);
			}
			if (null != _returnType)
			{
				constructor._returnType = _returnType.Clone() as TypeReference;
				constructor._returnType.InitializeParent(constructor);
			}
			if (null != _returnTypeAttributes)
			{
				constructor._returnTypeAttributes = _returnTypeAttributes.Clone() as AttributeCollection;
				constructor._returnTypeAttributes.InitializeParent(constructor);
			}
			if (null != _body)
			{
				constructor._body = _body.Clone() as Block;
				constructor._body.InitializeParent(constructor);
			}
			if (null != _locals)
			{
				constructor._locals = _locals.Clone() as LocalCollection;
				constructor._locals.InitializeParent(constructor);
			}
			constructor._implementationFlags = _implementationFlags;
			if (null != _explicitInfo)
			{
				constructor._explicitInfo = _explicitInfo.Clone() as ExplicitMemberInfo;
				constructor._explicitInfo.InitializeParent(constructor);
			}
			return constructor;
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

		public Constructor()
		{
			_name = "constructor";
		}

		public Constructor(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
			_name = "constructor";
		}
	}
}
