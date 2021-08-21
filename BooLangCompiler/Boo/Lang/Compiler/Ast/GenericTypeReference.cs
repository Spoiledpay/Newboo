using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GenericTypeReference : SimpleTypeReference
	{
		protected TypeReferenceCollection _genericArguments;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GenericTypeReference;

		[XmlArrayItem(typeof(TypeReference))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public TypeReferenceCollection GenericArguments
		{
			get
			{
				return _genericArguments ?? (_genericArguments = new TypeReferenceCollection(this));
			}
			set
			{
				if (_genericArguments != value)
				{
					_genericArguments = value;
					if (null != _genericArguments)
					{
						_genericArguments.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericTypeReference CloneNode()
		{
			return (GenericTypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericTypeReference CleanClone()
		{
			return (GenericTypeReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGenericTypeReference(this);
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
			GenericTypeReference genericTypeReference = (GenericTypeReference)node;
			if (_isPointer != genericTypeReference._isPointer)
			{
				return NoMatch("GenericTypeReference._isPointer");
			}
			if (_name != genericTypeReference._name)
			{
				return NoMatch("GenericTypeReference._name");
			}
			if (!Node.AllMatch(_genericArguments, genericTypeReference._genericArguments))
			{
				return NoMatch("GenericTypeReference._genericArguments");
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
			if (_genericArguments != null)
			{
				TypeReference typeReference = existing as TypeReference;
				if (null != typeReference)
				{
					TypeReference newItem = (TypeReference)newNode;
					if (_genericArguments.Replace(typeReference, newItem))
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
			GenericTypeReference genericTypeReference = new GenericTypeReference();
			genericTypeReference._lexicalInfo = _lexicalInfo;
			genericTypeReference._endSourceLocation = _endSourceLocation;
			genericTypeReference._documentation = _documentation;
			genericTypeReference._isSynthetic = _isSynthetic;
			genericTypeReference._entity = _entity;
			if (_annotations != null)
			{
				genericTypeReference._annotations = (Hashtable)_annotations.Clone();
			}
			genericTypeReference._isPointer = _isPointer;
			genericTypeReference._name = _name;
			if (null != _genericArguments)
			{
				genericTypeReference._genericArguments = _genericArguments.Clone() as TypeReferenceCollection;
				genericTypeReference._genericArguments.InitializeParent(genericTypeReference);
			}
			return genericTypeReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _genericArguments)
			{
				_genericArguments.ClearTypeSystemBindings();
			}
		}

		public GenericTypeReference()
		{
		}

		public GenericTypeReference(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public GenericTypeReference(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			base.Name = name;
		}

		public GenericTypeReference(string name, params TypeReference[] genericArguments)
			: base(name)
		{
			GenericArguments.AddRange(genericArguments);
		}

		public override string ToString()
		{
			return ToCodeString();
		}
	}
}
