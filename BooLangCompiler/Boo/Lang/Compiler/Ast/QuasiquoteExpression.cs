using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class QuasiquoteExpression : LiteralExpression
	{
		protected Node _node;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.QuasiquoteExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Node Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new QuasiquoteExpression CloneNode()
		{
			return (QuasiquoteExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new QuasiquoteExpression CleanClone()
		{
			return (QuasiquoteExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnQuasiquoteExpression(this);
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
			QuasiquoteExpression quasiquoteExpression = (QuasiquoteExpression)node;
			if (_node != quasiquoteExpression._node)
			{
				return NoMatch("QuasiquoteExpression._node");
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
			QuasiquoteExpression quasiquoteExpression = new QuasiquoteExpression();
			quasiquoteExpression._lexicalInfo = _lexicalInfo;
			quasiquoteExpression._endSourceLocation = _endSourceLocation;
			quasiquoteExpression._documentation = _documentation;
			quasiquoteExpression._isSynthetic = _isSynthetic;
			quasiquoteExpression._entity = _entity;
			if (_annotations != null)
			{
				quasiquoteExpression._annotations = (Hashtable)_annotations.Clone();
			}
			quasiquoteExpression._expressionType = _expressionType;
			if (null != _node)
			{
				quasiquoteExpression._node = _node.Clone() as Node;
				quasiquoteExpression._node.InitializeParent(quasiquoteExpression);
			}
			return quasiquoteExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public QuasiquoteExpression()
		{
		}

		public QuasiquoteExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public QuasiquoteExpression(Node node)
			: this(node.LexicalInfo)
		{
			_node = node;
		}
	}
}
