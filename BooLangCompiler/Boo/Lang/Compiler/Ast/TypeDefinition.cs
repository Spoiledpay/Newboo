using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(EnumDefinition))]
	[XmlInclude(typeof(InterfaceDefinition))]
	[XmlInclude(typeof(Module))]
	[XmlInclude(typeof(ClassDefinition))]
	[XmlInclude(typeof(StructDefinition))]
	public abstract class TypeDefinition : TypeMember, INodeWithGenericParameters
	{
		protected TypeMemberCollection _members;

		protected TypeReferenceCollection _baseTypes;

		protected GenericParameterDeclarationCollection _genericParameters;

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(TypeMember))]
		public TypeMemberCollection Members
		{
			get
			{
				return _members ?? (_members = new TypeMemberCollection(this));
			}
			set
			{
				if (_members != value)
				{
					_members = value;
					if (null != _members)
					{
						_members.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArrayItem(typeof(TypeReference))]
		public TypeReferenceCollection BaseTypes
		{
			get
			{
				return _baseTypes ?? (_baseTypes = new TypeReferenceCollection(this));
			}
			set
			{
				if (_baseTypes != value)
				{
					_baseTypes = value;
					if (null != _baseTypes)
					{
						_baseTypes.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(GenericParameterDeclaration))]
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

		public override string FullName => QualifiedName;

		public string QualifiedName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (base.ParentNode != null)
				{
					if (base.ParentNode.NodeType == NodeType.Module)
					{
						if (EnclosingNamespace != null)
						{
							stringBuilder.Append(EnclosingNamespace.Name).Append(".");
						}
					}
					else
					{
						TypeDefinition typeDefinition = base.ParentNode as TypeDefinition;
						if (typeDefinition != null)
						{
							stringBuilder.Append(typeDefinition.QualifiedName).Append(".");
						}
					}
				}
				stringBuilder.Append(base.Name);
				return stringBuilder.ToString();
			}
		}

		public bool IsNested
		{
			get
			{
				TypeDefinition declaringType = DeclaringType;
				if (declaringType == null || declaringType.NodeType == NodeType.Module)
				{
					return false;
				}
				return true;
			}
		}

		public bool HasMethods => HasMemberOfType(NodeType.Method);

		public bool HasGenericParameters => _genericParameters != null && _genericParameters.Count > 0;

		public bool HasInstanceConstructor => null != GetConstructor(0, false, null);

		public bool HasDeclaredInstanceConstructor => null != GetConstructor(0, false, false);

		public bool HasStaticConstructor => null != GetConstructor(0, true, null);

		public bool HasDeclaredStaticConstructor => null != GetConstructor(0, true, false);

		[GeneratedCode("astgen.boo", "1")]
		public new TypeDefinition CloneNode()
		{
			return (TypeDefinition)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeDefinition CleanClone()
		{
			return (TypeDefinition)base.CleanClone();
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
			TypeDefinition typeDefinition = (TypeDefinition)node;
			if (_modifiers != typeDefinition._modifiers)
			{
				return NoMatch("TypeDefinition._modifiers");
			}
			if (_name != typeDefinition._name)
			{
				return NoMatch("TypeDefinition._name");
			}
			if (!Node.AllMatch(_attributes, typeDefinition._attributes))
			{
				return NoMatch("TypeDefinition._attributes");
			}
			if (!Node.AllMatch(_members, typeDefinition._members))
			{
				return NoMatch("TypeDefinition._members");
			}
			if (!Node.AllMatch(_baseTypes, typeDefinition._baseTypes))
			{
				return NoMatch("TypeDefinition._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, typeDefinition._genericParameters))
			{
				return NoMatch("TypeDefinition._genericParameters");
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
			throw new InvalidOperationException("Cannot clone abstract class: TypeDefinition");
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

		protected TypeDefinition()
		{
		}

		protected TypeDefinition(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public bool HasMemberOfType(NodeType memberType)
		{
			foreach (TypeMember member in _members)
			{
				if (memberType == member.NodeType)
				{
					return true;
				}
			}
			return false;
		}

		public Constructor GetConstructor(int index)
		{
			return GetConstructor(index, null, null);
		}

		public Constructor GetStaticConstructor()
		{
			return GetConstructor(0, true, null);
		}

		public void Merge(TypeDefinition node)
		{
			if (null == node)
			{
				throw new ArgumentNullException("node");
			}
			if (NodeType != node.NodeType)
			{
				throw new ArgumentException($"Cannot merge {node.NodeType} into a {NodeType}.");
			}
			if (!object.ReferenceEquals(this, node))
			{
				base.Attributes.AddRange(node.Attributes);
				AddNonMatchingBaseTypes(node);
				Members.AddRange(node.Members);
			}
		}

		private void AddNonMatchingBaseTypes(TypeDefinition node)
		{
			TypeReferenceCollection baseTypes = BaseTypes;
			foreach (TypeReference baseType in node.BaseTypes)
			{
				if (!baseTypes.Contains(baseType.Matches))
				{
					baseTypes.Add(baseType);
				}
			}
		}

		protected Constructor GetConstructor(int index, bool? isStatic, bool? isSynthetic)
		{
			int num = 0;
			foreach (TypeMember member in _members)
			{
				if (NodeType.Constructor == member.NodeType && ((!isStatic.HasValue || member.IsStatic == isStatic) & (!isSynthetic.HasValue || member.IsSynthetic == isSynthetic)))
				{
					if (num == index)
					{
						return (Constructor)member;
					}
					num++;
				}
			}
			return null;
		}
	}
}
