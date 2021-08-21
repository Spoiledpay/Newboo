using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SimpleTypeReference : TypeReference
	{
		protected string _name;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SimpleTypeReference;

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

		[GeneratedCode("astgen.boo", "1")]
		public new SimpleTypeReference CloneNode()
		{
			return (SimpleTypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SimpleTypeReference CleanClone()
		{
			return (SimpleTypeReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSimpleTypeReference(this);
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
			SimpleTypeReference simpleTypeReference = (SimpleTypeReference)node;
			if (_isPointer != simpleTypeReference._isPointer)
			{
				return NoMatch("SimpleTypeReference._isPointer");
			}
			if (_name != simpleTypeReference._name)
			{
				return NoMatch("SimpleTypeReference._name");
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
			SimpleTypeReference simpleTypeReference = new SimpleTypeReference();
			simpleTypeReference._lexicalInfo = _lexicalInfo;
			simpleTypeReference._endSourceLocation = _endSourceLocation;
			simpleTypeReference._documentation = _documentation;
			simpleTypeReference._isSynthetic = _isSynthetic;
			simpleTypeReference._entity = _entity;
			if (_annotations != null)
			{
				simpleTypeReference._annotations = (Hashtable)_annotations.Clone();
			}
			simpleTypeReference._isPointer = _isPointer;
			simpleTypeReference._name = _name;
			return simpleTypeReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public SimpleTypeReference()
		{
		}

		public SimpleTypeReference(string name)
		{
			Name = name;
		}

		public SimpleTypeReference(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			Name = name;
		}

		public SimpleTypeReference(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public override string ToString()
		{
			return _name;
		}
	}
}
