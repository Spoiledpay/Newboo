using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class InterfaceDefinition : TypeDefinition
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.InterfaceDefinition;

		[GeneratedCode("astgen.boo", "1")]
		public new InterfaceDefinition CloneNode()
		{
			return (InterfaceDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new InterfaceDefinition CleanClone()
		{
			return (InterfaceDefinition)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnInterfaceDefinition(this);
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
			InterfaceDefinition interfaceDefinition = (InterfaceDefinition)node;
			if (_modifiers != interfaceDefinition._modifiers)
			{
				return NoMatch("InterfaceDefinition._modifiers");
			}
			if (_name != interfaceDefinition._name)
			{
				return NoMatch("InterfaceDefinition._name");
			}
			if (!Node.AllMatch(_attributes, interfaceDefinition._attributes))
			{
				return NoMatch("InterfaceDefinition._attributes");
			}
			if (!Node.AllMatch(_members, interfaceDefinition._members))
			{
				return NoMatch("InterfaceDefinition._members");
			}
			if (!Node.AllMatch(_baseTypes, interfaceDefinition._baseTypes))
			{
				return NoMatch("InterfaceDefinition._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, interfaceDefinition._genericParameters))
			{
				return NoMatch("InterfaceDefinition._genericParameters");
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
			if (_members != null)
			{
				TypeMember typeMember = existing as TypeMember;
				if (null != typeMember)
				{
					TypeMember newItem2 = (TypeMember)newNode;
					if (_members.Replace(typeMember, newItem2))
					{
						return true;
					}
				}
			}
			if (_baseTypes != null)
			{
				TypeReference typeReference = existing as TypeReference;
				if (null != typeReference)
				{
					TypeReference newItem3 = (TypeReference)newNode;
					if (_baseTypes.Replace(typeReference, newItem3))
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
					GenericParameterDeclaration newItem4 = (GenericParameterDeclaration)newNode;
					if (_genericParameters.Replace(genericParameterDeclaration, newItem4))
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
			InterfaceDefinition interfaceDefinition = new InterfaceDefinition();
			interfaceDefinition._lexicalInfo = _lexicalInfo;
			interfaceDefinition._endSourceLocation = _endSourceLocation;
			interfaceDefinition._documentation = _documentation;
			interfaceDefinition._isSynthetic = _isSynthetic;
			interfaceDefinition._entity = _entity;
			if (_annotations != null)
			{
				interfaceDefinition._annotations = (Hashtable)_annotations.Clone();
			}
			interfaceDefinition._modifiers = _modifiers;
			interfaceDefinition._name = _name;
			if (null != _attributes)
			{
				interfaceDefinition._attributes = _attributes.Clone() as AttributeCollection;
				interfaceDefinition._attributes.InitializeParent(interfaceDefinition);
			}
			if (null != _members)
			{
				interfaceDefinition._members = _members.Clone() as TypeMemberCollection;
				interfaceDefinition._members.InitializeParent(interfaceDefinition);
			}
			if (null != _baseTypes)
			{
				interfaceDefinition._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				interfaceDefinition._baseTypes.InitializeParent(interfaceDefinition);
			}
			if (null != _genericParameters)
			{
				interfaceDefinition._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				interfaceDefinition._genericParameters.InitializeParent(interfaceDefinition);
			}
			return interfaceDefinition;
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
			if (null != _members)
			{
				_members.ClearTypeSystemBindings();
			}
			if (null != _baseTypes)
			{
				_baseTypes.ClearTypeSystemBindings();
			}
			if (null != _genericParameters)
			{
				_genericParameters.ClearTypeSystemBindings();
			}
		}

		public InterfaceDefinition()
		{
		}

		public InterfaceDefinition(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
