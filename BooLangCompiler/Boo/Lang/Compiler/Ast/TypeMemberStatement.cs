using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TypeMemberStatement : Statement
	{
		protected TypeMember _typeMember;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.TypeMemberStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
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

		public TypeMember InsertionPoint { get; set; }

		[GeneratedCode("astgen.boo", "1")]
		public new TypeMemberStatement CloneNode()
		{
			return (TypeMemberStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeMemberStatement CleanClone()
		{
			return (TypeMemberStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnTypeMemberStatement(this);
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
			TypeMemberStatement typeMemberStatement = (TypeMemberStatement)node;
			if (!Node.Matches(_modifier, typeMemberStatement._modifier))
			{
				return NoMatch("TypeMemberStatement._modifier");
			}
			if (!Node.Matches(_typeMember, typeMemberStatement._typeMember))
			{
				return NoMatch("TypeMemberStatement._typeMember");
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
			if (_modifier == existing)
			{
				base.Modifier = (StatementModifier)newNode;
				return true;
			}
			if (_typeMember == existing)
			{
				TypeMember = (TypeMember)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			TypeMemberStatement typeMemberStatement = new TypeMemberStatement();
			typeMemberStatement._lexicalInfo = _lexicalInfo;
			typeMemberStatement._endSourceLocation = _endSourceLocation;
			typeMemberStatement._documentation = _documentation;
			typeMemberStatement._isSynthetic = _isSynthetic;
			typeMemberStatement._entity = _entity;
			if (_annotations != null)
			{
				typeMemberStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				typeMemberStatement._modifier = _modifier.Clone() as StatementModifier;
				typeMemberStatement._modifier.InitializeParent(typeMemberStatement);
			}
			if (null != _typeMember)
			{
				typeMemberStatement._typeMember = _typeMember.Clone() as TypeMember;
				typeMemberStatement._typeMember.InitializeParent(typeMemberStatement);
			}
			return typeMemberStatement;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _modifier)
			{
				_modifier.ClearTypeSystemBindings();
			}
			if (null != _typeMember)
			{
				_typeMember.ClearTypeSystemBindings();
			}
		}

		public TypeMemberStatement(TypeMember typeMember)
			: base(typeMember.LexicalInfo)
		{
			TypeMember = typeMember;
		}

		[GeneratedCode("astgen.boo", "1")]
		public TypeMemberStatement()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public TypeMemberStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
