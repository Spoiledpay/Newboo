using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class NamespaceDeclaration : Node
	{
		protected string _name;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.NamespaceDeclaration;

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
		public new NamespaceDeclaration CloneNode()
		{
			return (NamespaceDeclaration)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new NamespaceDeclaration CleanClone()
		{
			return (NamespaceDeclaration)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnNamespaceDeclaration(this);
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
			NamespaceDeclaration namespaceDeclaration = (NamespaceDeclaration)node;
			if (_name != namespaceDeclaration._name)
			{
				return NoMatch("NamespaceDeclaration._name");
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
			NamespaceDeclaration namespaceDeclaration = new NamespaceDeclaration();
			namespaceDeclaration._lexicalInfo = _lexicalInfo;
			namespaceDeclaration._endSourceLocation = _endSourceLocation;
			namespaceDeclaration._documentation = _documentation;
			namespaceDeclaration._isSynthetic = _isSynthetic;
			namespaceDeclaration._entity = _entity;
			if (_annotations != null)
			{
				namespaceDeclaration._annotations = (Hashtable)_annotations.Clone();
			}
			namespaceDeclaration._name = _name;
			return namespaceDeclaration;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public NamespaceDeclaration()
		{
		}

		public NamespaceDeclaration(string name)
		{
			Name = name;
		}

		public NamespaceDeclaration(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
