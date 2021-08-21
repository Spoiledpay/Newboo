using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TypeofExpression : Expression
	{
		protected TypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.TypeofExpression;

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
		public new TypeofExpression CloneNode()
		{
			return (TypeofExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeofExpression CleanClone()
		{
			return (TypeofExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnTypeofExpression(this);
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
			TypeofExpression typeofExpression = (TypeofExpression)node;
			if (!Node.Matches(_type, typeofExpression._type))
			{
				return NoMatch("TypeofExpression._type");
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
			TypeofExpression typeofExpression = new TypeofExpression();
			typeofExpression._lexicalInfo = _lexicalInfo;
			typeofExpression._endSourceLocation = _endSourceLocation;
			typeofExpression._documentation = _documentation;
			typeofExpression._isSynthetic = _isSynthetic;
			typeofExpression._entity = _entity;
			if (_annotations != null)
			{
				typeofExpression._annotations = (Hashtable)_annotations.Clone();
			}
			typeofExpression._expressionType = _expressionType;
			if (null != _type)
			{
				typeofExpression._type = _type.Clone() as TypeReference;
				typeofExpression._type.InitializeParent(typeofExpression);
			}
			return typeofExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
		}

		public TypeofExpression()
		{
		}

		public TypeofExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public TypeofExpression(LexicalInfo lexicalInfo, TypeReference typeReference)
			: base(lexicalInfo)
		{
			Type = typeReference;
		}
	}
}
