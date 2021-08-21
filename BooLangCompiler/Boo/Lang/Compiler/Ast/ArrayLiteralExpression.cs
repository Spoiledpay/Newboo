using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ArrayLiteralExpression : ListLiteralExpression
	{
		protected ArrayTypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ArrayLiteralExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public ArrayTypeReference Type
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
		public new ArrayLiteralExpression CloneNode()
		{
			return (ArrayLiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ArrayLiteralExpression CleanClone()
		{
			return (ArrayLiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnArrayLiteralExpression(this);
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
			ArrayLiteralExpression arrayLiteralExpression = (ArrayLiteralExpression)node;
			if (!Node.AllMatch(_items, arrayLiteralExpression._items))
			{
				return NoMatch("ArrayLiteralExpression._items");
			}
			if (!Node.Matches(_type, arrayLiteralExpression._type))
			{
				return NoMatch("ArrayLiteralExpression._type");
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
			if (_type == existing)
			{
				Type = (ArrayTypeReference)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ArrayLiteralExpression arrayLiteralExpression = new ArrayLiteralExpression();
			arrayLiteralExpression._lexicalInfo = _lexicalInfo;
			arrayLiteralExpression._endSourceLocation = _endSourceLocation;
			arrayLiteralExpression._documentation = _documentation;
			arrayLiteralExpression._isSynthetic = _isSynthetic;
			arrayLiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				arrayLiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			arrayLiteralExpression._expressionType = _expressionType;
			if (null != _items)
			{
				arrayLiteralExpression._items = _items.Clone() as ExpressionCollection;
				arrayLiteralExpression._items.InitializeParent(arrayLiteralExpression);
			}
			if (null != _type)
			{
				arrayLiteralExpression._type = _type.Clone() as ArrayTypeReference;
				arrayLiteralExpression._type.InitializeParent(arrayLiteralExpression);
			}
			return arrayLiteralExpression;
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
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
		}

		public ArrayLiteralExpression()
		{
		}

		public ArrayLiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
