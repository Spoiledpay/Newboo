using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TryCastExpression : Expression
	{
		protected Expression _target;

		protected TypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.TryCastExpression;

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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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
		public new TryCastExpression CloneNode()
		{
			return (TryCastExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TryCastExpression CleanClone()
		{
			return (TryCastExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnTryCastExpression(this);
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
			TryCastExpression tryCastExpression = (TryCastExpression)node;
			if (!Node.Matches(_target, tryCastExpression._target))
			{
				return NoMatch("TryCastExpression._target");
			}
			if (!Node.Matches(_type, tryCastExpression._type))
			{
				return NoMatch("TryCastExpression._type");
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
			TryCastExpression tryCastExpression = new TryCastExpression();
			tryCastExpression._lexicalInfo = _lexicalInfo;
			tryCastExpression._endSourceLocation = _endSourceLocation;
			tryCastExpression._documentation = _documentation;
			tryCastExpression._isSynthetic = _isSynthetic;
			tryCastExpression._entity = _entity;
			if (_annotations != null)
			{
				tryCastExpression._annotations = (Hashtable)_annotations.Clone();
			}
			tryCastExpression._expressionType = _expressionType;
			if (null != _target)
			{
				tryCastExpression._target = _target.Clone() as Expression;
				tryCastExpression._target.InitializeParent(tryCastExpression);
			}
			if (null != _type)
			{
				tryCastExpression._type = _type.Clone() as TypeReference;
				tryCastExpression._type.InitializeParent(tryCastExpression);
			}
			return tryCastExpression;
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

		public TryCastExpression()
		{
		}

		public TryCastExpression(Expression target, TypeReference type)
			: this(LexicalInfo.Empty, target, type)
		{
		}

		public TryCastExpression(LexicalInfo lexicalInfo, Expression target, TypeReference type)
			: base(lexicalInfo)
		{
			Target = target;
			Type = type;
		}

		public TryCastExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public static explicit operator Field(TryCastExpression tce)
		{
			BinaryExpression binaryExpression = tce.Target as BinaryExpression;
			if (binaryExpression == null || binaryExpression.Operator != BinaryOperatorType.Assign)
			{
				throw new InvalidCastException("Only an assignment can be converted to a Field.");
			}
			Field field = new Field();
			field.LexicalInfo = binaryExpression.LexicalInfo;
			field.Modifiers = TypeMemberModifiers.Protected;
			field.Name = ((ReferenceExpression)binaryExpression.Left).Name;
			field.Type = tce.Type;
			field.Initializer = binaryExpression;
			return field;
		}
	}
}
