using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExpressionPair : Node
	{
		protected Expression _first;

		protected Expression _second;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExpressionPair;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression First
		{
			get
			{
				return _first;
			}
			set
			{
				if (_first != value)
				{
					_first = value;
					if (null != _first)
					{
						_first.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Second
		{
			get
			{
				return _second;
			}
			set
			{
				if (_second != value)
				{
					_second = value;
					if (null != _second)
					{
						_second.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionPair CloneNode()
		{
			return (ExpressionPair)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExpressionPair CleanClone()
		{
			return (ExpressionPair)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExpressionPair(this);
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
			ExpressionPair expressionPair = (ExpressionPair)node;
			if (!Node.Matches(_first, expressionPair._first))
			{
				return NoMatch("ExpressionPair._first");
			}
			if (!Node.Matches(_second, expressionPair._second))
			{
				return NoMatch("ExpressionPair._second");
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
			if (_first == existing)
			{
				First = (Expression)newNode;
				return true;
			}
			if (_second == existing)
			{
				Second = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ExpressionPair expressionPair = new ExpressionPair();
			expressionPair._lexicalInfo = _lexicalInfo;
			expressionPair._endSourceLocation = _endSourceLocation;
			expressionPair._documentation = _documentation;
			expressionPair._isSynthetic = _isSynthetic;
			expressionPair._entity = _entity;
			if (_annotations != null)
			{
				expressionPair._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _first)
			{
				expressionPair._first = _first.Clone() as Expression;
				expressionPair._first.InitializeParent(expressionPair);
			}
			if (null != _second)
			{
				expressionPair._second = _second.Clone() as Expression;
				expressionPair._second.InitializeParent(expressionPair);
			}
			return expressionPair;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _first)
			{
				_first.ClearTypeSystemBindings();
			}
			if (null != _second)
			{
				_second.ClearTypeSystemBindings();
			}
		}

		public ExpressionPair()
		{
		}

		public ExpressionPair(Expression first, Expression second)
			: this(LexicalInfo.Empty, first, second)
		{
		}

		public ExpressionPair(LexicalInfo token, Expression first, Expression second)
			: base(token)
		{
			First = first;
			Second = second;
		}

		public ExpressionPair(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
