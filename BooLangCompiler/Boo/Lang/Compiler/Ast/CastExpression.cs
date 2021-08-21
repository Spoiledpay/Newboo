using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CastExpression : Expression
	{
		protected Expression _target;

		protected TypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CastExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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
		[XmlElement]
		public TypeReference Type
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
		public new CastExpression CloneNode()
		{
			return (CastExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CastExpression CleanClone()
		{
			return (CastExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCastExpression(this);
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
			CastExpression castExpression = (CastExpression)node;
			if (!Node.Matches(_target, castExpression._target))
			{
				return NoMatch("CastExpression._target");
			}
			if (!Node.Matches(_type, castExpression._type))
			{
				return NoMatch("CastExpression._type");
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
			if (_type == existing)
			{
				Type = (TypeReference)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			CastExpression castExpression = new CastExpression();
			castExpression._lexicalInfo = _lexicalInfo;
			castExpression._endSourceLocation = _endSourceLocation;
			castExpression._documentation = _documentation;
			castExpression._isSynthetic = _isSynthetic;
			castExpression._entity = _entity;
			if (_annotations != null)
			{
				castExpression._annotations = (Hashtable)_annotations.Clone();
			}
			castExpression._expressionType = _expressionType;
			if (null != _target)
			{
				castExpression._target = _target.Clone() as Expression;
				castExpression._target.InitializeParent(castExpression);
			}
			if (null != _type)
			{
				castExpression._type = _type.Clone() as TypeReference;
				castExpression._type.InitializeParent(castExpression);
			}
			return castExpression;
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
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
		}

		public CastExpression()
		{
		}

		public CastExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public CastExpression(LexicalInfo lexicalInfo, Expression target, TypeReference type)
			: base(lexicalInfo)
		{
			Target = target;
			Type = type;
		}

		public CastExpression(Expression target, TypeReference type)
		{
			Target = target;
			Type = type;
		}
	}
}
