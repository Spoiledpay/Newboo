using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Local : Node
	{
		protected string _name;

		protected bool _privateScope;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Local;

		[GeneratedCode("astgen.boo", "1")]
		[XmlAttribute]
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

		public bool PrivateScope
		{
			get
			{
				return _privateScope;
			}
			set
			{
				_privateScope = true;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Local CloneNode()
		{
			return (Local)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Local CleanClone()
		{
			return (Local)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnLocal(this);
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
			Local local = (Local)node;
			if (_name != local._name)
			{
				return NoMatch("Local._name");
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
			Local local = new Local();
			local._lexicalInfo = _lexicalInfo;
			local._endSourceLocation = _endSourceLocation;
			local._documentation = _documentation;
			local._isSynthetic = _isSynthetic;
			local._entity = _entity;
			if (_annotations != null)
			{
				local._annotations = (Hashtable)_annotations.Clone();
			}
			local._name = _name;
			return local;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public Local()
		{
		}

		public Local(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			_name = name;
		}

		public Local(ReferenceExpression reference, bool privateScope)
		{
			_name = reference.Name;
			base.LexicalInfo = reference.LexicalInfo;
			_privateScope = privateScope;
		}

		public Local(Declaration declaration, bool privateScope)
		{
			_name = declaration.Name;
			base.LexicalInfo = declaration.LexicalInfo;
			_privateScope = privateScope;
		}

		public Local(string name, bool privateScope)
		{
			_name = name;
			_privateScope = privateScope;
		}
	}
}
