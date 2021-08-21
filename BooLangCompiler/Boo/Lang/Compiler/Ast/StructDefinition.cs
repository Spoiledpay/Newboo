using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class StructDefinition : TypeDefinition
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.StructDefinition;

		[GeneratedCode("astgen.boo", "1")]
		public new StructDefinition CloneNode()
		{
			return (StructDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new StructDefinition CleanClone()
		{
			return (StructDefinition)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnStructDefinition(this);
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
			StructDefinition structDefinition = (StructDefinition)node;
			if (_modifiers != structDefinition._modifiers)
			{
				return NoMatch("StructDefinition._modifiers");
			}
			if (_name != structDefinition._name)
			{
				return NoMatch("StructDefinition._name");
			}
			if (!Node.AllMatch(_attributes, structDefinition._attributes))
			{
				return NoMatch("StructDefinition._attributes");
			}
			if (!Node.AllMatch(_members, structDefinition._members))
			{
				return NoMatch("StructDefinition._members");
			}
			if (!Node.AllMatch(_baseTypes, structDefinition._baseTypes))
			{
				return NoMatch("StructDefinition._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, structDefinition._genericParameters))
			{
				return NoMatch("StructDefinition._genericParameters");
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
			StructDefinition structDefinition = new StructDefinition();
			structDefinition._lexicalInfo = _lexicalInfo;
			structDefinition._endSourceLocation = _endSourceLocation;
			structDefinition._documentation = _documentation;
			structDefinition._isSynthetic = _isSynthetic;
			structDefinition._entity = _entity;
			if (_annotations != null)
			{
				structDefinition._annotations = (Hashtable)_annotations.Clone();
			}
			structDefinition._modifiers = _modifiers;
			structDefinition._name = _name;
			if (null != _attributes)
			{
				structDefinition._attributes = _attributes.Clone() as AttributeCollection;
				structDefinition._attributes.InitializeParent(structDefinition);
			}
			if (null != _members)
			{
				structDefinition._members = _members.Clone() as TypeMemberCollection;
				structDefinition._members.InitializeParent(structDefinition);
			}
			if (null != _baseTypes)
			{
				structDefinition._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				structDefinition._baseTypes.InitializeParent(structDefinition);
			}
			if (null != _genericParameters)
			{
				structDefinition._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				structDefinition._genericParameters.InitializeParent(structDefinition);
			}
			return structDefinition;
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

		public StructDefinition()
		{
		}

		public StructDefinition(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
