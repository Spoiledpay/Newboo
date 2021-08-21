using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Destructor : Method
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Destructor;

		[GeneratedCode("astgen.boo", "1")]
		public new Destructor CloneNode()
		{
			return (Destructor)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Destructor CleanClone()
		{
			return (Destructor)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnDestructor(this);
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
			Destructor destructor = (Destructor)node;
			if (_modifiers != destructor._modifiers)
			{
				return NoMatch("Destructor._modifiers");
			}
			if (_name != destructor._name)
			{
				return NoMatch("Destructor._name");
			}
			if (!Node.AllMatch(_attributes, destructor._attributes))
			{
				return NoMatch("Destructor._attributes");
			}
			if (!Node.AllMatch(_parameters, destructor._parameters))
			{
				return NoMatch("Destructor._parameters");
			}
			if (!Node.AllMatch(_genericParameters, destructor._genericParameters))
			{
				return NoMatch("Destructor._genericParameters");
			}
			if (!Node.Matches(_returnType, destructor._returnType))
			{
				return NoMatch("Destructor._returnType");
			}
			if (!Node.AllMatch(_returnTypeAttributes, destructor._returnTypeAttributes))
			{
				return NoMatch("Destructor._returnTypeAttributes");
			}
			if (!Node.Matches(_body, destructor._body))
			{
				return NoMatch("Destructor._body");
			}
			if (!Node.AllMatch(_locals, destructor._locals))
			{
				return NoMatch("Destructor._locals");
			}
			if (_implementationFlags != destructor._implementationFlags)
			{
				return NoMatch("Destructor._implementationFlags");
			}
			if (!Node.Matches(_explicitInfo, destructor._explicitInfo))
			{
				return NoMatch("Destructor._explicitInfo");
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
			Destructor destructor = new Destructor();
			destructor._lexicalInfo = _lexicalInfo;
			destructor._endSourceLocation = _endSourceLocation;
			destructor._documentation = _documentation;
			destructor._isSynthetic = _isSynthetic;
			destructor._entity = _entity;
			if (_annotations != null)
			{
				destructor._annotations = (Hashtable)_annotations.Clone();
			}
			destructor._modifiers = _modifiers;
			destructor._name = _name;
			if (null != _attributes)
			{
				destructor._attributes = _attributes.Clone() as AttributeCollection;
				destructor._attributes.InitializeParent(destructor);
			}
			if (null != _parameters)
			{
				destructor._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				destructor._parameters.InitializeParent(destructor);
			}
			if (null != _genericParameters)
			{
				destructor._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				destructor._genericParameters.InitializeParent(destructor);
			}
			if (null != _returnType)
			{
				destructor._returnType = _returnType.Clone() as TypeReference;
				destructor._returnType.InitializeParent(destructor);
			}
			if (null != _returnTypeAttributes)
			{
				destructor._returnTypeAttributes = _returnTypeAttributes.Clone() as AttributeCollection;
				destructor._returnTypeAttributes.InitializeParent(destructor);
			}
			if (null != _body)
			{
				destructor._body = _body.Clone() as Block;
				destructor._body.InitializeParent(destructor);
			}
			if (null != _locals)
			{
				destructor._locals = _locals.Clone() as LocalCollection;
				destructor._locals.InitializeParent(destructor);
			}
			destructor._implementationFlags = _implementationFlags;
			if (null != _explicitInfo)
			{
				destructor._explicitInfo = _explicitInfo.Clone() as ExplicitMemberInfo;
				destructor._explicitInfo.InitializeParent(destructor);
			}
			return destructor;
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

		public Destructor()
		{
			_name = "destructor";
		}

		public Destructor(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
			_name = "destructor";
		}
	}
}
