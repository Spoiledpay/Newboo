using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class LabelStatement : Statement
	{
		protected string _name;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.LabelStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlAttribute]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new LabelStatement CloneNode()
		{
			return (LabelStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new LabelStatement CleanClone()
		{
			return (LabelStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnLabelStatement(this);
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
			LabelStatement labelStatement = (LabelStatement)node;
			if (!Node.Matches(_modifier, labelStatement._modifier))
			{
				return NoMatch("LabelStatement._modifier");
			}
			if (_name != labelStatement._name)
			{
				return NoMatch("LabelStatement._name");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			LabelStatement labelStatement = new LabelStatement();
			labelStatement._lexicalInfo = _lexicalInfo;
			labelStatement._endSourceLocation = _endSourceLocation;
			labelStatement._documentation = _documentation;
			labelStatement._isSynthetic = _isSynthetic;
			labelStatement._entity = _entity;
			if (_annotations != null)
			{
				labelStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				labelStatement._modifier = _modifier.Clone() as StatementModifier;
				labelStatement._modifier.InitializeParent(labelStatement);
			}
			labelStatement._name = _name;
			return labelStatement;
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
		}

		public LabelStatement()
		{
		}

		public LabelStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public LabelStatement(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			Name = name;
		}
	}
}
