using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceTypeMember : TypeMember
	{
		protected TypeMember _typeMember;

		protected Expression _nameExpression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceTypeMember;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public TypeMember TypeMember
		{
			get
			{
				return _typeMember;
			}
			set
			{
				if (_typeMember != value)
				{
					_typeMember = value;
					if (null != _typeMember)
					{
						_typeMember.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression NameExpression
		{
			get
			{
				return _nameExpression;
			}
			set
			{
				if (_nameExpression != value)
				{
					_nameExpression = value;
					if (null != _nameExpression)
					{
						_nameExpression.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceTypeMember CloneNode()
		{
			return (SpliceTypeMember)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceTypeMember CleanClone()
		{
			return (SpliceTypeMember)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceTypeMember(this);
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
			SpliceTypeMember spliceTypeMember = (SpliceTypeMember)node;
			if (_modifiers != spliceTypeMember._modifiers)
			{
				return NoMatch("SpliceTypeMember._modifiers");
			}
			if (_name != spliceTypeMember._name)
			{
				return NoMatch("SpliceTypeMember._name");
			}
			if (!Node.AllMatch(_attributes, spliceTypeMember._attributes))
			{
				return NoMatch("SpliceTypeMember._attributes");
			}
			if (!Node.Matches(_typeMember, spliceTypeMember._typeMember))
			{
				return NoMatch("SpliceTypeMember._typeMember");
			}
			if (!Node.Matches(_nameExpression, spliceTypeMember._nameExpression))
			{
				return NoMatch("SpliceTypeMember._nameExpression");
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
			if (_attributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_attributes.Replace(attribute, newItem))
					{
						return true;
					}
				}
			}
			if (_typeMember == existing)
			{
				TypeMember = (TypeMember)newNode;
				return true;
			}
			if (_nameExpression == existing)
			{
				NameExpression = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			SpliceTypeMember spliceTypeMember = new SpliceTypeMember();
			spliceTypeMember._lexicalInfo = _lexicalInfo;
			spliceTypeMember._endSourceLocation = _endSourceLocation;
			spliceTypeMember._documentation = _documentation;
			spliceTypeMember._isSynthetic = _isSynthetic;
			spliceTypeMember._entity = _entity;
			if (_annotations != null)
			{
				spliceTypeMember._annotations = (Hashtable)_annotations.Clone();
			}
			spliceTypeMember._modifiers = _modifiers;
			spliceTypeMember._name = _name;
			if (null != _attributes)
			{
				spliceTypeMember._attributes = _attributes.Clone() as AttributeCollection;
				spliceTypeMember._attributes.InitializeParent(spliceTypeMember);
			}
			if (null != _typeMember)
			{
				spliceTypeMember._typeMember = _typeMember.Clone() as TypeMember;
				spliceTypeMember._typeMember.InitializeParent(spliceTypeMember);
			}
			if (null != _nameExpression)
			{
				spliceTypeMember._nameExpression = _nameExpression.Clone() as Expression;
				spliceTypeMember._nameExpression.InitializeParent(spliceTypeMember);
			}
			return spliceTypeMember;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _attributes)
			{
				_attributes.ClearTypeSystemBindings();
			}
			if (null != _typeMember)
			{
				_typeMember.ClearTypeSystemBindings();
			}
			if (null != _nameExpression)
			{
				_nameExpression.ClearTypeSystemBindings();
			}
		}

		public SpliceTypeMember()
		{
		}

		public SpliceTypeMember(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SpliceTypeMember(TypeMember typeMember, Expression nameExpression)
			: this(typeMember.LexicalInfo, typeMember, nameExpression)
		{
		}

		public SpliceTypeMember(LexicalInfo lexicalInfo, TypeMember typeMember, Expression nameExpression)
			: base(lexicalInfo)
		{
			TypeMember = typeMember;
			NameExpression = nameExpression;
		}
	}
}
