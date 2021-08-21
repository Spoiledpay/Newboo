using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ContinueStatement : Statement
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ContinueStatement;

		[GeneratedCode("astgen.boo", "1")]
		public new ContinueStatement CloneNode()
		{
			return (ContinueStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ContinueStatement CleanClone()
		{
			return (ContinueStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnContinueStatement(this);
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
			ContinueStatement continueStatement = (ContinueStatement)node;
			if (!Node.Matches(_modifier, continueStatement._modifier))
			{
				return NoMatch("ContinueStatement._modifier");
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
			ContinueStatement continueStatement = new ContinueStatement();
			continueStatement._lexicalInfo = _lexicalInfo;
			continueStatement._endSourceLocation = _endSourceLocation;
			continueStatement._documentation = _documentation;
			continueStatement._isSynthetic = _isSynthetic;
			continueStatement._entity = _entity;
			if (_annotations != null)
			{
				continueStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				continueStatement._modifier = _modifier.Clone() as StatementModifier;
				continueStatement._modifier.InitializeParent(continueStatement);
			}
			return continueStatement;
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

		public ContinueStatement()
		{
		}

		public ContinueStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
