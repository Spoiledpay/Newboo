using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CustomStatement : Statement
	{
		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CustomStatement;

		[GeneratedCode("astgen.boo", "1")]
		public new CustomStatement CloneNode()
		{
			return (CustomStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CustomStatement CleanClone()
		{
			return (CustomStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCustomStatement(this);
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
			CustomStatement customStatement = (CustomStatement)node;
			if (!Node.Matches(_modifier, customStatement._modifier))
			{
				return NoMatch("CustomStatement._modifier");
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
			CustomStatement customStatement = new CustomStatement();
			customStatement._lexicalInfo = _lexicalInfo;
			customStatement._endSourceLocation = _endSourceLocation;
			customStatement._documentation = _documentation;
			customStatement._isSynthetic = _isSynthetic;
			customStatement._entity = _entity;
			if (_annotations != null)
			{
				customStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				customStatement._modifier = _modifier.Clone() as StatementModifier;
				customStatement._modifier.InitializeParent(customStatement);
			}
			return customStatement;
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

		[GeneratedCode("astgen.boo", "1")]
		public CustomStatement()
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public CustomStatement(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
