using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(MemberReferenceExpression))]
	[XmlInclude(typeof(GenericReferenceExpression))]
	public class ReferenceExpression : Expression
	{
		protected string _name;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ReferenceExpression;

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
		public new ReferenceExpression CloneNode()
		{
			return (ReferenceExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ReferenceExpression CleanClone()
		{
			return (ReferenceExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnReferenceExpression(this);
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
			ReferenceExpression referenceExpression = (ReferenceExpression)node;
			if (_name != referenceExpression._name)
			{
				return NoMatch("ReferenceExpression._name");
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
			ReferenceExpression referenceExpression = new ReferenceExpression();
			referenceExpression._lexicalInfo = _lexicalInfo;
			referenceExpression._endSourceLocation = _endSourceLocation;
			referenceExpression._documentation = _documentation;
			referenceExpression._isSynthetic = _isSynthetic;
			referenceExpression._entity = _entity;
			if (_annotations != null)
			{
				referenceExpression._annotations = (Hashtable)_annotations.Clone();
			}
			referenceExpression._expressionType = _expressionType;
			referenceExpression._name = _name;
			return referenceExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public new static ReferenceExpression Lift(string reference)
		{
			return AstUtil.CreateReferenceExpression(reference);
		}

		public ReferenceExpression()
		{
		}

		public ReferenceExpression(string name)
		{
			Name = name;
		}

		public ReferenceExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public ReferenceExpression(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			Name = name;
		}
	}
}
