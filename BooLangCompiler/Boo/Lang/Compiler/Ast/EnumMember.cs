using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class EnumMember : TypeMember
	{
		protected Expression _initializer;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.EnumMember;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
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

		public override bool IsStatic => true;

		[GeneratedCode("astgen.boo", "1")]
		public new EnumMember CloneNode()
		{
			return (EnumMember)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new EnumMember CleanClone()
		{
			return (EnumMember)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnEnumMember(this);
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
			EnumMember enumMember = (EnumMember)node;
			if (_modifiers != enumMember._modifiers)
			{
				return NoMatch("EnumMember._modifiers");
			}
			if (_name != enumMember._name)
			{
				return NoMatch("EnumMember._name");
			}
			if (!Node.AllMatch(_attributes, enumMember._attributes))
			{
				return NoMatch("EnumMember._attributes");
			}
			if (!Node.Matches(_initializer, enumMember._initializer))
			{
				return NoMatch("EnumMember._initializer");
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
			EnumMember enumMember = new EnumMember();
			enumMember._lexicalInfo = _lexicalInfo;
			enumMember._endSourceLocation = _endSourceLocation;
			enumMember._documentation = _documentation;
			enumMember._isSynthetic = _isSynthetic;
			enumMember._entity = _entity;
			if (_annotations != null)
			{
				enumMember._annotations = (Hashtable)_annotations.Clone();
			}
			enumMember._modifiers = _modifiers;
			enumMember._name = _name;
			if (null != _attributes)
			{
				enumMember._attributes = _attributes.Clone() as AttributeCollection;
				enumMember._attributes.InitializeParent(enumMember);
			}
			if (null != _initializer)
			{
				enumMember._initializer = _initializer.Clone() as Expression;
				enumMember._initializer.InitializeParent(enumMember);
			}
			return enumMember;
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
			if (null != _initializer)
			{
				_initializer.ClearTypeSystemBindings();
			}
		}

		public static EnumMember Lift(ReferenceExpression node)
		{
			return new EnumMember(node.LexicalInfo, node.Name);
		}

		public EnumMember()
		{
		}

		public EnumMember(IntegerLiteralExpression initializer)
		{
			Initializer = initializer;
		}

		public EnumMember(LexicalInfo token, IntegerLiteralExpression initializer)
			: base(token)
		{
			Initializer = initializer;
		}

		public EnumMember(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public EnumMember(LexicalInfo lexicalInfoProvider, string name)
			: base(lexicalInfoProvider)
		{
			base.Name = name;
		}

		public EnumMember(string name)
		{
			base.Name = name;
		}
	}
}
