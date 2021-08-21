using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ClassDefinition : TypeDefinition
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ClassDefinition;

		[GeneratedCode("astgen.boo", "1")]
		public new ClassDefinition CloneNode()
		{
			return (ClassDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ClassDefinition CleanClone()
		{
			return (ClassDefinition)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnClassDefinition(this);
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
			ClassDefinition classDefinition = (ClassDefinition)node;
			if (_modifiers != classDefinition._modifiers)
			{
				return NoMatch("ClassDefinition._modifiers");
			}
			if (_name != classDefinition._name)
			{
				return NoMatch("ClassDefinition._name");
			}
			if (!Node.AllMatch(_attributes, classDefinition._attributes))
			{
				return NoMatch("ClassDefinition._attributes");
			}
			if (!Node.AllMatch(_members, classDefinition._members))
			{
				return NoMatch("ClassDefinition._members");
			}
			if (!Node.AllMatch(_baseTypes, classDefinition._baseTypes))
			{
				return NoMatch("ClassDefinition._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, classDefinition._genericParameters))
			{
				return NoMatch("ClassDefinition._genericParameters");
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
			ClassDefinition classDefinition = new ClassDefinition();
			classDefinition._lexicalInfo = _lexicalInfo;
			classDefinition._endSourceLocation = _endSourceLocation;
			classDefinition._documentation = _documentation;
			classDefinition._isSynthetic = _isSynthetic;
			classDefinition._entity = _entity;
			if (_annotations != null)
			{
				classDefinition._annotations = (Hashtable)_annotations.Clone();
			}
			classDefinition._modifiers = _modifiers;
			classDefinition._name = _name;
			if (null != _attributes)
			{
				classDefinition._attributes = _attributes.Clone() as AttributeCollection;
				classDefinition._attributes.InitializeParent(classDefinition);
			}
			if (null != _members)
			{
				classDefinition._members = _members.Clone() as TypeMemberCollection;
				classDefinition._members.InitializeParent(classDefinition);
			}
			if (null != _baseTypes)
			{
				classDefinition._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				classDefinition._baseTypes.InitializeParent(classDefinition);
			}
			if (null != _genericParameters)
			{
				classDefinition._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				classDefinition._genericParameters.InitializeParent(classDefinition);
			}
			return classDefinition;
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

		public ClassDefinition()
		{
		}

		public ClassDefinition(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
