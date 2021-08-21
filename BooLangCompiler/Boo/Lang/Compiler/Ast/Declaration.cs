using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Declaration : Node
	{
		protected string _name;

		protected TypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Declaration;

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
		[XmlElement]
		public TypeReference Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (_type != value)
				{
					_type = value;
					if (null != _type)
					{
						_type.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Declaration CloneNode()
		{
			return (Declaration)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Declaration CleanClone()
		{
			return (Declaration)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnDeclaration(this);
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
			Declaration declaration = (Declaration)node;
			if (_name != declaration._name)
			{
				return NoMatch("Declaration._name");
			}
			if (!Node.Matches(_type, declaration._type))
			{
				return NoMatch("Declaration._type");
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
			if (_type == existing)
			{
				Type = (TypeReference)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Declaration declaration = new Declaration();
			declaration._lexicalInfo = _lexicalInfo;
			declaration._endSourceLocation = _endSourceLocation;
			declaration._documentation = _documentation;
			declaration._isSynthetic = _isSynthetic;
			declaration._entity = _entity;
			if (_annotations != null)
			{
				declaration._annotations = (Hashtable)_annotations.Clone();
			}
			declaration._name = _name;
			if (null != _type)
			{
				declaration._type = _type.Clone() as TypeReference;
				declaration._type.InitializeParent(declaration);
			}
			return declaration;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
		}

		public Declaration()
		{
		}

		public Declaration(string name, TypeReference type)
			: this(LexicalInfo.Empty, name, type)
		{
		}

		public Declaration(LexicalInfo token, string name, TypeReference type)
			: base(token)
		{
			Name = name;
			Type = type;
		}

		public Declaration(LexicalInfo lexicalInfo, string name)
			: this(lexicalInfo, name, null)
		{
		}

		public Declaration(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
