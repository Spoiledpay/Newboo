using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class BreakStatement : Statement
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.BreakStatement;

		[GeneratedCode("astgen.boo", "1")]
		public new BreakStatement CloneNode()
		{
			return (BreakStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new BreakStatement CleanClone()
		{
			return (BreakStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnBreakStatement(this);
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
			BreakStatement breakStatement = (BreakStatement)node;
			if (!Node.Matches(_modifier, breakStatement._modifier))
			{
				return NoMatch("BreakStatement._modifier");
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
			BreakStatement breakStatement = new BreakStatement();
			breakStatement._lexicalInfo = _lexicalInfo;
			breakStatement._endSourceLocation = _endSourceLocation;
			breakStatement._documentation = _documentation;
			breakStatement._isSynthetic = _isSynthetic;
			breakStatement._entity = _entity;
			if (_annotations != null)
			{
				breakStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				breakStatement._modifier = _modifier.Clone() as StatementModifier;
				breakStatement._modifier.InitializeParent(breakStatement);
			}
			return breakStatement;
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

		public BreakStatement()
		{
		}

		public BreakStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
