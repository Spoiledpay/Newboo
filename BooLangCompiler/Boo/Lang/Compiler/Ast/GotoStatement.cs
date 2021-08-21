using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GotoStatement : Statement
	{
		protected ReferenceExpression _label;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GotoStatement;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public ReferenceExpression Label
		{
			get
			{
				return _label;
			}
			set
			{
				if (_label != value)
				{
					_label = value;
					if (null != _label)
					{
						_label.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GotoStatement CloneNode()
		{
			return (GotoStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GotoStatement CleanClone()
		{
			return (GotoStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGotoStatement(this);
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
			GotoStatement gotoStatement = (GotoStatement)node;
			if (!Node.Matches(_modifier, gotoStatement._modifier))
			{
				return NoMatch("GotoStatement._modifier");
			}
			if (!Node.Matches(_label, gotoStatement._label))
			{
				return NoMatch("GotoStatement._label");
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
			if (_label == existing)
			{
				Label = (ReferenceExpression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			GotoStatement gotoStatement = new GotoStatement();
			gotoStatement._lexicalInfo = _lexicalInfo;
			gotoStatement._endSourceLocation = _endSourceLocation;
			gotoStatement._documentation = _documentation;
			gotoStatement._isSynthetic = _isSynthetic;
			gotoStatement._entity = _entity;
			if (_annotations != null)
			{
				gotoStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				gotoStatement._modifier = _modifier.Clone() as StatementModifier;
				gotoStatement._modifier.InitializeParent(gotoStatement);
			}
			if (null != _label)
			{
				gotoStatement._label = _label.Clone() as ReferenceExpression;
				gotoStatement._label.InitializeParent(gotoStatement);
			}
			return gotoStatement;
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
			if (null != _label)
			{
				_label.ClearTypeSystemBindings();
			}
		}

		public GotoStatement()
		{
		}

		public GotoStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public GotoStatement(LexicalInfo lexicalInfo, ReferenceExpression label)
			: base(lexicalInfo)
		{
			Label = label;
		}
	}
}
