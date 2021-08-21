using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GenericTypeDefinitionReference : SimpleTypeReference
	{
		protected int _genericPlaceholders;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GenericTypeDefinitionReference;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public int GenericPlaceholders
		{
			get
			{
				return _genericPlaceholders;
			}
			set
			{
				_genericPlaceholders = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericTypeDefinitionReference CloneNode()
		{
			return (GenericTypeDefinitionReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericTypeDefinitionReference CleanClone()
		{
			return (GenericTypeDefinitionReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGenericTypeDefinitionReference(this);
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
			GenericTypeDefinitionReference genericTypeDefinitionReference = (GenericTypeDefinitionReference)node;
			if (_isPointer != genericTypeDefinitionReference._isPointer)
			{
				return NoMatch("GenericTypeDefinitionReference._isPointer");
			}
			if (_name != genericTypeDefinitionReference._name)
			{
				return NoMatch("GenericTypeDefinitionReference._name");
			}
			if (_genericPlaceholders != genericTypeDefinitionReference._genericPlaceholders)
			{
				return NoMatch("GenericTypeDefinitionReference._genericPlaceholders");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			GenericTypeDefinitionReference genericTypeDefinitionReference = new GenericTypeDefinitionReference();
			genericTypeDefinitionReference._lexicalInfo = _lexicalInfo;
			genericTypeDefinitionReference._endSourceLocation = _endSourceLocation;
			genericTypeDefinitionReference._documentation = _documentation;
			genericTypeDefinitionReference._isSynthetic = _isSynthetic;
			genericTypeDefinitionReference._entity = _entity;
			if (_annotations != null)
			{
				genericTypeDefinitionReference._annotations = (Hashtable)_annotations.Clone();
			}
			genericTypeDefinitionReference._isPointer = _isPointer;
			genericTypeDefinitionReference._name = _name;
			genericTypeDefinitionReference._genericPlaceholders = _genericPlaceholders;
			return genericTypeDefinitionReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public GenericTypeDefinitionReference()
		{
		}

		public GenericTypeDefinitionReference(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public override string ToString()
		{
			return ToCodeString();
		}
	}
}
