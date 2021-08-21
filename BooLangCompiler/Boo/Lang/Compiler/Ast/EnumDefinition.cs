using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class EnumDefinition : TypeDefinition
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.EnumDefinition;

		[GeneratedCode("astgen.boo", "1")]
		public new EnumDefinition CloneNode()
		{
			return (EnumDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new EnumDefinition CleanClone()
		{
			return (EnumDefinition)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnEnumDefinition(this);
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
			EnumDefinition enumDefinition = (EnumDefinition)node;
			if (_modifiers != enumDefinition._modifiers)
			{
				return NoMatch("EnumDefinition._modifiers");
			}
			if (_name != enumDefinition._name)
			{
				return NoMatch("EnumDefinition._name");
			}
			if (!Node.AllMatch(_attributes, enumDefinition._attributes))
			{
				return NoMatch("EnumDefinition._attributes");
			}
			if (!Node.AllMatch(_members, enumDefinition._members))
			{
				return NoMatch("EnumDefinition._members");
			}
			if (!Node.AllMatch(_baseTypes, enumDefinition._baseTypes))
			{
				return NoMatch("EnumDefinition._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, enumDefinition._genericParameters))
			{
				return NoMatch("EnumDefinition._genericParameters");
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
			EnumDefinition enumDefinition = new EnumDefinition();
			enumDefinition._lexicalInfo = _lexicalInfo;
			enumDefinition._endSourceLocation = _endSourceLocation;
			enumDefinition._documentation = _documentation;
			enumDefinition._isSynthetic = _isSynthetic;
			enumDefinition._entity = _entity;
			if (_annotations != null)
			{
				enumDefinition._annotations = (Hashtable)_annotations.Clone();
			}
			enumDefinition._modifiers = _modifiers;
			enumDefinition._name = _name;
			if (null != _attributes)
			{
				enumDefinition._attributes = _attributes.Clone() as AttributeCollection;
				enumDefinition._attributes.InitializeParent(enumDefinition);
			}
			if (null != _members)
			{
				enumDefinition._members = _members.Clone() as TypeMemberCollection;
				enumDefinition._members.InitializeParent(enumDefinition);
			}
			if (null != _baseTypes)
			{
				enumDefinition._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				enumDefinition._baseTypes.InitializeParent(enumDefinition);
			}
			if (null != _genericParameters)
			{
				enumDefinition._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				enumDefinition._genericParameters.InitializeParent(enumDefinition);
			}
			return enumDefinition;
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

		public EnumDefinition()
		{
		}

		public EnumDefinition(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
