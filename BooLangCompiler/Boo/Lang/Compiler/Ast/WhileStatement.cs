using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class WhileStatement : ConditionalStatement
	{
		protected Block _block;

		protected Block _orBlock;

		protected Block _thenBlock;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.WhileStatement;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block OrBlock
		{
			get
			{
				return _orBlock;
			}
			set
			{
				if (_orBlock != value)
				{
					_orBlock = value;
					if (null != _orBlock)
					{
						_orBlock.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block ThenBlock
		{
			get
			{
				return _thenBlock;
			}
			set
			{
				if (_thenBlock != value)
				{
					_thenBlock = value;
					if (null != _thenBlock)
					{
						_thenBlock.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new WhileStatement CloneNode()
		{
			return (WhileStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new WhileStatement CleanClone()
		{
			return (WhileStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnWhileStatement(this);
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
			WhileStatement whileStatement = (WhileStatement)node;
			if (!Node.Matches(_modifier, whileStatement._modifier))
			{
				return NoMatch("WhileStatement._modifier");
			}
			if (!Node.Matches(_condition, whileStatement._condition))
			{
				return NoMatch("WhileStatement._condition");
			}
			if (!Node.Matches(_block, whileStatement._block))
			{
				return NoMatch("WhileStatement._block");
			}
			if (!Node.Matches(_orBlock, whileStatement._orBlock))
			{
				return NoMatch("WhileStatement._orBlock");
			}
			if (!Node.Matches(_thenBlock, whileStatement._thenBlock))
			{
				return NoMatch("WhileStatement._thenBlock");
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
			if (_orBlock == existing)
			{
				OrBlock = (Block)newNode;
				return true;
			}
			if (_thenBlock == existing)
			{
				ThenBlock = (Block)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			WhileStatement whileStatement = new WhileStatement();
			whileStatement._lexicalInfo = _lexicalInfo;
			whileStatement._endSourceLocation = _endSourceLocation;
			whileStatement._documentation = _documentation;
			whileStatement._isSynthetic = _isSynthetic;
			whileStatement._entity = _entity;
			if (_annotations != null)
			{
				whileStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				whileStatement._modifier = _modifier.Clone() as StatementModifier;
				whileStatement._modifier.InitializeParent(whileStatement);
			}
			if (null != _condition)
			{
				whileStatement._condition = _condition.Clone() as Expression;
				whileStatement._condition.InitializeParent(whileStatement);
			}
			if (null != _block)
			{
				whileStatement._block = _block.Clone() as Block;
				whileStatement._block.InitializeParent(whileStatement);
			}
			if (null != _orBlock)
			{
				whileStatement._orBlock = _orBlock.Clone() as Block;
				whileStatement._orBlock.InitializeParent(whileStatement);
			}
			if (null != _thenBlock)
			{
				whileStatement._thenBlock = _thenBlock.Clone() as Block;
				whileStatement._thenBlock.InitializeParent(whileStatement);
			}
			return whileStatement;
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
			if (null != _orBlock)
			{
				_orBlock.ClearTypeSystemBindings();
			}
			if (null != _thenBlock)
			{
				_thenBlock.ClearTypeSystemBindings();
			}
		}

		public WhileStatement()
		{
		}

		public WhileStatement(Expression condition)
		{
			base.Condition = condition;
		}

		public WhileStatement(Expression condition, Block block)
		{
			base.Condition = condition;
			Block = block;
		}

		public WhileStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
