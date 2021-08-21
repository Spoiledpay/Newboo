using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class MemberReferenceExpression : ReferenceExpression
	{
		protected Expression _target;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.MemberReferenceExpression;

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
		public new MemberReferenceExpression CloneNode()
		{
			return (MemberReferenceExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new MemberReferenceExpression CleanClone()
		{
			return (MemberReferenceExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnMemberReferenceExpression(this);
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
			MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)node;
			if (_name != memberReferenceExpression._name)
			{
				return NoMatch("MemberReferenceExpression._name");
			}
			if (!Node.Matches(_target, memberReferenceExpression._target))
			{
				return NoMatch("MemberReferenceExpression._target");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression();
			memberReferenceExpression._lexicalInfo = _lexicalInfo;
			memberReferenceExpression._endSourceLocation = _endSourceLocation;
			memberReferenceExpression._documentation = _documentation;
			memberReferenceExpression._isSynthetic = _isSynthetic;
			memberReferenceExpression._entity = _entity;
			if (_annotations != null)
			{
				memberReferenceExpression._annotations = (Hashtable)_annotations.Clone();
			}
			memberReferenceExpression._expressionType = _expressionType;
			memberReferenceExpression._name = _name;
			if (null != _target)
			{
				memberReferenceExpression._target = _target.Clone() as Expression;
				memberReferenceExpression._target.InitializeParent(memberReferenceExpression);
			}
			return memberReferenceExpression;
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
		}

		public MemberReferenceExpression()
		{
		}

		public MemberReferenceExpression(LexicalInfo lexicalInfo, Expression target, string name)
			: base(lexicalInfo)
		{
			Target = target;
			base.Name = name;
		}

		public MemberReferenceExpression(Expression target, string name)
		{
			Target = target;
			base.Name = name;
		}

		public MemberReferenceExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
