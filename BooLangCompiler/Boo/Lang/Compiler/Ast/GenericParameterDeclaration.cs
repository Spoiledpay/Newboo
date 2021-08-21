using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GenericParameterDeclaration : Node
	{
		protected string _name;

		protected TypeReferenceCollection _baseTypes;

		protected GenericParameterConstraints _constraints;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GenericParameterDeclaration;

		[XmlAttribute]
		[GeneratedCode("astgen.boo", "1")]
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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public GenericParameterConstraints Constraints
		{
			get
			{
				return _constraints;
			}
			set
			{
				_constraints = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericParameterDeclaration CloneNode()
		{
			return (GenericParameterDeclaration)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericParameterDeclaration CleanClone()
		{
			return (GenericParameterDeclaration)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGenericParameterDeclaration(this);
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
			GenericParameterDeclaration genericParameterDeclaration = (GenericParameterDeclaration)node;
			if (_name != genericParameterDeclaration._name)
			{
				return NoMatch("GenericParameterDeclaration._name");
			}
			if (!Node.AllMatch(_baseTypes, genericParameterDeclaration._baseTypes))
			{
				return NoMatch("GenericParameterDeclaration._baseTypes");
			}
			if (_constraints != genericParameterDeclaration._constraints)
			{
				return NoMatch("GenericParameterDeclaration._constraints");
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
			if (_baseTypes != null)
			{
				TypeReference typeReference = existing as TypeReference;
				if (null != typeReference)
				{
					TypeReference newItem = (TypeReference)newNode;
					if (_baseTypes.Replace(typeReference, newItem))
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
			GenericParameterDeclaration genericParameterDeclaration = new GenericParameterDeclaration();
			genericParameterDeclaration._lexicalInfo = _lexicalInfo;
			genericParameterDeclaration._endSourceLocation = _endSourceLocation;
			genericParameterDeclaration._documentation = _documentation;
			genericParameterDeclaration._isSynthetic = _isSynthetic;
			genericParameterDeclaration._entity = _entity;
			if (_annotations != null)
			{
				genericParameterDeclaration._annotations = (Hashtable)_annotations.Clone();
			}
			genericParameterDeclaration._name = _name;
			if (null != _baseTypes)
			{
				genericParameterDeclaration._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				genericParameterDeclaration._baseTypes.InitializeParent(genericParameterDeclaration);
			}
			genericParameterDeclaration._constraints = _constraints;
			return genericParameterDeclaration;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _baseTypes)
			{
				_baseTypes.ClearTypeSystemBindings();
			}
		}

		public static GenericParameterDeclaration Lift(SimpleTypeReference simpleTypeRef)
		{
			return new GenericParameterDeclaration(simpleTypeRef.Name);
		}

		public static GenericParameterDeclaration Lift(ReferenceExpression referenceExpression)
		{
			return new GenericParameterDeclaration(referenceExpression.Name);
		}

		public GenericParameterDeclaration()
		{
		}

		public GenericParameterDeclaration(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public GenericParameterDeclaration(string name)
		{
			Name = name;
		}
	}
}
