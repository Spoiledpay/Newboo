using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceTypeReference : TypeReference
	{
		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceTypeReference;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			set
			{
				if (_expression != value)
				{
					_expression = value;
					if (null != _expression)
					{
						_expression.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceTypeReference CloneNode()
		{
			return (SpliceTypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceTypeReference CleanClone()
		{
			return (SpliceTypeReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceTypeReference(this);
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
			SpliceTypeReference spliceTypeReference = (SpliceTypeReference)node;
			if (_isPointer != spliceTypeReference._isPointer)
			{
				return NoMatch("SpliceTypeReference._isPointer");
			}
			if (!Node.Matches(_expression, spliceTypeReference._expression))
			{
				return NoMatch("SpliceTypeReference._expression");
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
			if (_expression == existing)
			{
				Expression = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			SpliceTypeReference spliceTypeReference = new SpliceTypeReference();
			spliceTypeReference._lexicalInfo = _lexicalInfo;
			spliceTypeReference._endSourceLocation = _endSourceLocation;
			spliceTypeReference._documentation = _documentation;
			spliceTypeReference._isSynthetic = _isSynthetic;
			spliceTypeReference._entity = _entity;
			if (_annotations != null)
			{
				spliceTypeReference._annotations = (Hashtable)_annotations.Clone();
			}
			spliceTypeReference._isPointer = _isPointer;
			if (null != _expression)
			{
				spliceTypeReference._expression = _expression.Clone() as Expression;
				spliceTypeReference._expression.InitializeParent(spliceTypeReference);
			}
			return spliceTypeReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
		}

		public SpliceTypeReference()
		{
		}

		public SpliceTypeReference(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SpliceTypeReference(LexicalInfo lexicalInfo, Expression e)
			: base(lexicalInfo)
		{
			Expression = e;
		}
	}
}
