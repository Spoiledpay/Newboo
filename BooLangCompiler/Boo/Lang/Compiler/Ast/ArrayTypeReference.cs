using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ArrayTypeReference : TypeReference
	{
		protected TypeReference _elementType;

		protected IntegerLiteralExpression _rank;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ArrayTypeReference;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public TypeReference ElementType
		{
			get
			{
				return _elementType;
			}
			set
			{
				if (_elementType != value)
				{
					_elementType = value;
					if (null != _elementType)
					{
						_elementType.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public IntegerLiteralExpression Rank
		{
			get
			{
				return _rank;
			}
			set
			{
				if (_rank != value)
				{
					_rank = value;
					if (null != _rank)
					{
						_rank.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ArrayTypeReference CloneNode()
		{
			return (ArrayTypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ArrayTypeReference CleanClone()
		{
			return (ArrayTypeReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnArrayTypeReference(this);
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
			ArrayTypeReference arrayTypeReference = (ArrayTypeReference)node;
			if (_isPointer != arrayTypeReference._isPointer)
			{
				return NoMatch("ArrayTypeReference._isPointer");
			}
			if (!Node.Matches(_elementType, arrayTypeReference._elementType))
			{
				return NoMatch("ArrayTypeReference._elementType");
			}
			if (!Node.Matches(_rank, arrayTypeReference._rank))
			{
				return NoMatch("ArrayTypeReference._rank");
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
			if (_elementType == existing)
			{
				ElementType = (TypeReference)newNode;
				return true;
			}
			if (_rank == existing)
			{
				Rank = (IntegerLiteralExpression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ArrayTypeReference arrayTypeReference = new ArrayTypeReference();
			arrayTypeReference._lexicalInfo = _lexicalInfo;
			arrayTypeReference._endSourceLocation = _endSourceLocation;
			arrayTypeReference._documentation = _documentation;
			arrayTypeReference._isSynthetic = _isSynthetic;
			arrayTypeReference._entity = _entity;
			if (_annotations != null)
			{
				arrayTypeReference._annotations = (Hashtable)_annotations.Clone();
			}
			arrayTypeReference._isPointer = _isPointer;
			if (null != _elementType)
			{
				arrayTypeReference._elementType = _elementType.Clone() as TypeReference;
				arrayTypeReference._elementType.InitializeParent(arrayTypeReference);
			}
			if (null != _rank)
			{
				arrayTypeReference._rank = _rank.Clone() as IntegerLiteralExpression;
				arrayTypeReference._rank.InitializeParent(arrayTypeReference);
			}
			return arrayTypeReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _elementType)
			{
				_elementType.ClearTypeSystemBindings();
			}
			if (null != _rank)
			{
				_rank.ClearTypeSystemBindings();
			}
		}

		public ArrayTypeReference()
		{
		}

		public ArrayTypeReference(TypeReference elementType)
			: this(LexicalInfo.Empty, elementType)
		{
		}

		public ArrayTypeReference(LexicalInfo lexicalInfo, TypeReference elementType)
			: this(lexicalInfo, elementType, new IntegerLiteralExpression(1L))
		{
		}

		public ArrayTypeReference(TypeReference elementType, IntegerLiteralExpression rank)
			: this(LexicalInfo.Empty, elementType, rank)
		{
		}

		public ArrayTypeReference(LexicalInfo lexicalInfo, TypeReference elementType, IntegerLiteralExpression rank)
			: base(lexicalInfo)
		{
			ElementType = elementType;
			Rank = rank;
		}

		public ArrayTypeReference(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public override string ToString()
		{
			return string.Concat("(", _elementType, ")");
		}
	}
}
