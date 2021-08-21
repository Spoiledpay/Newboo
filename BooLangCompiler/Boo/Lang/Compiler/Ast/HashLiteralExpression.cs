using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class HashLiteralExpression : LiteralExpression
	{
		protected ExpressionPairCollection _items;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.HashLiteralExpression;

		[XmlArray]
		[XmlArrayItem(typeof(ExpressionPair))]
		[GeneratedCode("astgen.boo", "1")]
		public ExpressionPairCollection Items
		{
			get
			{
				return _items ?? (_items = new ExpressionPairCollection(this));
			}
			set
			{
				if (_items != value)
				{
					_items = value;
					if (null != _items)
					{
						_items.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new HashLiteralExpression CloneNode()
		{
			return (HashLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new HashLiteralExpression CleanClone()
		{
			return (HashLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnHashLiteralExpression(this);
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
			HashLiteralExpression hashLiteralExpression = (HashLiteralExpression)node;
			if (!Node.AllMatch(_items, hashLiteralExpression._items))
			{
				return NoMatch("HashLiteralExpression._items");
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
			if (_items != null)
			{
				ExpressionPair expressionPair = existing as ExpressionPair;
				if (null != expressionPair)
				{
					ExpressionPair newItem = (ExpressionPair)newNode;
					if (_items.Replace(expressionPair, newItem))
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
			HashLiteralExpression hashLiteralExpression = new HashLiteralExpression();
			hashLiteralExpression._lexicalInfo = _lexicalInfo;
			hashLiteralExpression._endSourceLocation = _endSourceLocation;
			hashLiteralExpression._documentation = _documentation;
			hashLiteralExpression._isSynthetic = _isSynthetic;
			hashLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				hashLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			hashLiteralExpression._expressionType = _expressionType;
			if (null != _items)
			{
				hashLiteralExpression._items = _items.Clone() as ExpressionPairCollection;
				hashLiteralExpression._items.InitializeParent(hashLiteralExpression);
			}
			return hashLiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _items)
			{
				_items.ClearTypeSystemBindings();
			}
		}

		public HashLiteralExpression()
		{
		}

		public HashLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public HashLiteralExpression(params ExpressionPair[] items)
		{
			Items.AddRange(items);
		}
	}
}
