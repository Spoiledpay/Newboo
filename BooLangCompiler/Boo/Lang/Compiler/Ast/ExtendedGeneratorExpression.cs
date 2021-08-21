using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExtendedGeneratorExpression : Expression
	{
		protected GeneratorExpressionCollection _items;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExtendedGeneratorExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(GeneratorExpression))]
		public GeneratorExpressionCollection Items
		{
			get
			{
				return _items ?? (_items = new GeneratorExpressionCollection(this));
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
		public new ExtendedGeneratorExpression CloneNode()
		{
			return (ExtendedGeneratorExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExtendedGeneratorExpression CleanClone()
		{
			return (ExtendedGeneratorExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExtendedGeneratorExpression(this);
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
			ExtendedGeneratorExpression extendedGeneratorExpression = (ExtendedGeneratorExpression)node;
			if (!Node.AllMatch(_items, extendedGeneratorExpression._items))
			{
				return NoMatch("ExtendedGeneratorExpression._items");
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
				GeneratorExpression generatorExpression = existing as GeneratorExpression;
				if (null != generatorExpression)
				{
					GeneratorExpression newItem = (GeneratorExpression)newNode;
					if (_items.Replace(generatorExpression, newItem))
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
			ExtendedGeneratorExpression extendedGeneratorExpression = new ExtendedGeneratorExpression();
			extendedGeneratorExpression._lexicalInfo = _lexicalInfo;
			extendedGeneratorExpression._endSourceLocation = _endSourceLocation;
			extendedGeneratorExpression._documentation = _documentation;
			extendedGeneratorExpression._isSynthetic = _isSynthetic;
			extendedGeneratorExpression._entity = _entity;
			if (_annotations != null)
			{
				extendedGeneratorExpression._annotations = (Hashtable)_annotations.Clone();
			}
			extendedGeneratorExpression._expressionType = _expressionType;
			if (null != _items)
			{
				extendedGeneratorExpression._items = _items.Clone() as GeneratorExpressionCollection;
				extendedGeneratorExpression._items.InitializeParent(extendedGeneratorExpression);
			}
			return extendedGeneratorExpression;
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

		public ExtendedGeneratorExpression()
		{
		}

		public ExtendedGeneratorExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
