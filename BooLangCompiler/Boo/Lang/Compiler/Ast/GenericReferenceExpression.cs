using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GenericReferenceExpression : Expression
	{
		protected Expression _target;

		protected TypeReferenceCollection _genericArguments;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GenericReferenceExpression;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (_target != value)
				{
					_target = value;
					if (null != _target)
					{
						_target.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(TypeReference))]
		public TypeReferenceCollection GenericArguments
		{
			get
			{
				return _genericArguments ?? (_genericArguments = new TypeReferenceCollection(this));
			}
			set
			{
				if (_genericArguments != value)
				{
					_genericArguments = value;
					if (null != _genericArguments)
					{
						_genericArguments.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericReferenceExpression CloneNode()
		{
			return (GenericReferenceExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GenericReferenceExpression CleanClone()
		{
			return (GenericReferenceExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGenericReferenceExpression(this);
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
			GenericReferenceExpression genericReferenceExpression = (GenericReferenceExpression)node;
			if (!Node.Matches(_target, genericReferenceExpression._target))
			{
				return NoMatch("GenericReferenceExpression._target");
			}
			if (!Node.AllMatch(_genericArguments, genericReferenceExpression._genericArguments))
			{
				return NoMatch("GenericReferenceExpression._genericArguments");
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
			if (_target == existing)
			{
				Target = (Expression)newNode;
				return true;
			}
			if (_genericArguments != null)
			{
				TypeReference typeReference = existing as TypeReference;
				if (null != typeReference)
				{
					TypeReference newItem = (TypeReference)newNode;
					if (_genericArguments.Replace(typeReference, newItem))
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
			GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression();
			genericReferenceExpression._lexicalInfo = _lexicalInfo;
			genericReferenceExpression._endSourceLocation = _endSourceLocation;
			genericReferenceExpression._documentation = _documentation;
			genericReferenceExpression._isSynthetic = _isSynthetic;
			genericReferenceExpression._entity = _entity;
			if (_annotations != null)
			{
				genericReferenceExpression._annotations = (Hashtable)_annotations.Clone();
			}
			genericReferenceExpression._expressionType = _expressionType;
			if (null != _target)
			{
				genericReferenceExpression._target = _target.Clone() as Expression;
				genericReferenceExpression._target.InitializeParent(genericReferenceExpression);
			}
			if (null != _genericArguments)
			{
				genericReferenceExpression._genericArguments = _genericArguments.Clone() as TypeReferenceCollection;
				genericReferenceExpression._genericArguments.InitializeParent(genericReferenceExpression);
			}
			return genericReferenceExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _target)
			{
				_target.ClearTypeSystemBindings();
			}
			if (null != _genericArguments)
			{
				_genericArguments.ClearTypeSystemBindings();
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public GenericReferenceExpression()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public GenericReferenceExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
