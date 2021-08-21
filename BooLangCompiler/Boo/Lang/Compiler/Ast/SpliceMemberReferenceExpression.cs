using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SpliceMemberReferenceExpression : Expression
	{
		protected Expression _target;

		protected Expression _nameExpression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SpliceMemberReferenceExpression;

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
		public new SpliceMemberReferenceExpression CloneNode()
		{
			return (SpliceMemberReferenceExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SpliceMemberReferenceExpression CleanClone()
		{
			return (SpliceMemberReferenceExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSpliceMemberReferenceExpression(this);
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
			SpliceMemberReferenceExpression spliceMemberReferenceExpression = (SpliceMemberReferenceExpression)node;
			if (!Node.Matches(_target, spliceMemberReferenceExpression._target))
			{
				return NoMatch("SpliceMemberReferenceExpression._target");
			}
			if (!Node.Matches(_nameExpression, spliceMemberReferenceExpression._nameExpression))
			{
				return NoMatch("SpliceMemberReferenceExpression._nameExpression");
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
			SpliceMemberReferenceExpression spliceMemberReferenceExpression = new SpliceMemberReferenceExpression();
			spliceMemberReferenceExpression._lexicalInfo = _lexicalInfo;
			spliceMemberReferenceExpression._endSourceLocation = _endSourceLocation;
			spliceMemberReferenceExpression._documentation = _documentation;
			spliceMemberReferenceExpression._isSynthetic = _isSynthetic;
			spliceMemberReferenceExpression._entity = _entity;
			if (_annotations != null)
			{
				spliceMemberReferenceExpression._annotations = (Hashtable)_annotations.Clone();
			}
			spliceMemberReferenceExpression._expressionType = _expressionType;
			if (null != _target)
			{
				spliceMemberReferenceExpression._target = _target.Clone() as Expression;
				spliceMemberReferenceExpression._target.InitializeParent(spliceMemberReferenceExpression);
			}
			if (null != _nameExpression)
			{
				spliceMemberReferenceExpression._nameExpression = _nameExpression.Clone() as Expression;
				spliceMemberReferenceExpression._nameExpression.InitializeParent(spliceMemberReferenceExpression);
			}
			return spliceMemberReferenceExpression;
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
			if (null != _nameExpression)
			{
				_nameExpression.ClearTypeSystemBindings();
			}
		}

		public SpliceMemberReferenceExpression()
		{
		}

		public SpliceMemberReferenceExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SpliceMemberReferenceExpression(LexicalInfo lexicalInfo, Expression target, Expression nameExpression)
			: base(lexicalInfo)
		{
			Target = target;
			NameExpression = nameExpression;
		}
	}
}
