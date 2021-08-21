using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(ArrayLiteralExpression))]
	public class ListLiteralExpression : LiteralExpression
	{
		protected ExpressionCollection _items;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ListLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlArrayItem(typeof(Expression))]
		[XmlArray]
		public ExpressionCollection Items
		{
			get
			{
				return _items ?? (_items = new ExpressionCollection(this));
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
		public new ListLiteralExpression CloneNode()
		{
			return (ListLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ListLiteralExpression CleanClone()
		{
			return (ListLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnListLiteralExpression(this);
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
			ListLiteralExpression listLiteralExpression = (ListLiteralExpression)node;
			if (!Node.AllMatch(_items, listLiteralExpression._items))
			{
				return NoMatch("ListLiteralExpression._items");
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
				Expression expression = existing as Expression;
				if (null != expression)
				{
					Expression newItem = (Expression)newNode;
					if (_items.Replace(expression, newItem))
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
			ListLiteralExpression listLiteralExpression = new ListLiteralExpression();
			listLiteralExpression._lexicalInfo = _lexicalInfo;
			listLiteralExpression._endSourceLocation = _endSourceLocation;
			listLiteralExpression._documentation = _documentation;
			listLiteralExpression._isSynthetic = _isSynthetic;
			listLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				listLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			listLiteralExpression._expressionType = _expressionType;
			if (null != _items)
			{
				listLiteralExpression._items = _items.Clone() as ExpressionCollection;
				listLiteralExpression._items.InitializeParent(listLiteralExpression);
			}
			return listLiteralExpression;
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

		public ListLiteralExpression()
		{
		}

		public ListLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
