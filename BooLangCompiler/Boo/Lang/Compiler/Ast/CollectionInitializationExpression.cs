using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CollectionInitializationExpression : Expression
	{
		protected Expression _collection;

		protected Expression _initializer;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CollectionInitializationExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Collection
		{
			get
			{
				return _collection;
			}
			set
			{
				if (_collection != value)
				{
					_collection = value;
					if (null != _collection)
					{
						_collection.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Initializer
		{
			get
			{
				return _initializer;
			}
			set
			{
				if (_initializer != value)
				{
					_initializer = value;
					if (null != _initializer)
					{
						_initializer.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CollectionInitializationExpression CloneNode()
		{
			return (CollectionInitializationExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CollectionInitializationExpression CleanClone()
		{
			return (CollectionInitializationExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCollectionInitializationExpression(this);
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
			CollectionInitializationExpression collectionInitializationExpression = (CollectionInitializationExpression)node;
			if (!Node.Matches(_collection, collectionInitializationExpression._collection))
			{
				return NoMatch("CollectionInitializationExpression._collection");
			}
			if (!Node.Matches(_initializer, collectionInitializationExpression._initializer))
			{
				return NoMatch("CollectionInitializationExpression._initializer");
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
			if (_collection == existing)
			{
				Collection = (Expression)newNode;
				return true;
			}
			if (_initializer == existing)
			{
				Initializer = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			CollectionInitializationExpression collectionInitializationExpression = new CollectionInitializationExpression();
			collectionInitializationExpression._lexicalInfo = _lexicalInfo;
			collectionInitializationExpression._endSourceLocation = _endSourceLocation;
			collectionInitializationExpression._documentation = _documentation;
			collectionInitializationExpression._isSynthetic = _isSynthetic;
			collectionInitializationExpression._entity = _entity;
			if (_annotations != null)
			{
				collectionInitializationExpression._annotations = (Hashtable)_annotations.Clone();
			}
			collectionInitializationExpression._expressionType = _expressionType;
			if (null != _collection)
			{
				collectionInitializationExpression._collection = _collection.Clone() as Expression;
				collectionInitializationExpression._collection.InitializeParent(collectionInitializationExpression);
			}
			if (null != _initializer)
			{
				collectionInitializationExpression._initializer = _initializer.Clone() as Expression;
				collectionInitializationExpression._initializer.InitializeParent(collectionInitializationExpression);
			}
			return collectionInitializationExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _collection)
			{
				_collection.ClearTypeSystemBindings();
			}
			if (null != _initializer)
			{
				_initializer.ClearTypeSystemBindings();
			}
		}

		public CollectionInitializationExpression()
		{
		}

		public CollectionInitializationExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public CollectionInitializationExpression(Expression collection, Expression initializer)
			: base(collection.LexicalInfo)
		{
			Collection = collection;
			Initializer = initializer;
		}
	}
}
