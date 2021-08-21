using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class UnlessStatement : ConditionalStatement
	{
		protected Block _block;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.UnlessStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Block Block
		{
			get
			{
				if (_block == null)
				{
					_block = new Block();
					_block.InitializeParent(this);
				}
				return _block;
			}
			set
			{
				if (_block != value)
				{
					_block = value;
					if (null != _block)
					{
						_block.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new UnlessStatement CloneNode()
		{
			return (UnlessStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new UnlessStatement CleanClone()
		{
			return (UnlessStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnUnlessStatement(this);
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
			UnlessStatement unlessStatement = (UnlessStatement)node;
			if (!Node.Matches(_modifier, unlessStatement._modifier))
			{
				return NoMatch("UnlessStatement._modifier");
			}
			if (!Node.Matches(_condition, unlessStatement._condition))
			{
				return NoMatch("UnlessStatement._condition");
			}
			if (!Node.Matches(_block, unlessStatement._block))
			{
				return NoMatch("UnlessStatement._block");
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
			if (_condition == existing)
			{
				base.Condition = (Expression)newNode;
				return true;
			}
			if (_block == existing)
			{
				Block = (Block)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			UnlessStatement unlessStatement = new UnlessStatement();
			unlessStatement._lexicalInfo = _lexicalInfo;
			unlessStatement._endSourceLocation = _endSourceLocation;
			unlessStatement._documentation = _documentation;
			unlessStatement._isSynthetic = _isSynthetic;
			unlessStatement._entity = _entity;
			if (_annotations != null)
			{
				unlessStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				unlessStatement._modifier = _modifier.Clone() as StatementModifier;
				unlessStatement._modifier.InitializeParent(unlessStatement);
			}
			if (null != _condition)
			{
				unlessStatement._condition = _condition.Clone() as Expression;
				unlessStatement._condition.InitializeParent(unlessStatement);
			}
			if (null != _block)
			{
				unlessStatement._block = _block.Clone() as Block;
				unlessStatement._block.InitializeParent(unlessStatement);
			}
			return unlessStatement;
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
			if (null != _condition)
			{
				_condition.ClearTypeSystemBindings();
			}
			if (null != _block)
			{
				_block.ClearTypeSystemBindings();
			}
		}

		public UnlessStatement()
		{
		}

		public UnlessStatement(Expression condition)
		{
			base.Condition = condition;
		}

		public UnlessStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
