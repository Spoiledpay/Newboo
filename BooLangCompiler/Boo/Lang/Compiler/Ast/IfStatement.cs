using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class IfStatement : ConditionalStatement
	{
		protected Block _trueBlock;

		protected Block _falseBlock;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.IfStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Block TrueBlock
		{
			get
			{
				return _trueBlock;
			}
			set
			{
				if (_trueBlock != value)
				{
					_trueBlock = value;
					if (null != _trueBlock)
					{
						_trueBlock.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block FalseBlock
		{
			get
			{
				return _falseBlock;
			}
			set
			{
				if (_falseBlock != value)
				{
					_falseBlock = value;
					if (null != _falseBlock)
					{
						_falseBlock.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new IfStatement CloneNode()
		{
			return (IfStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new IfStatement CleanClone()
		{
			return (IfStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnIfStatement(this);
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
			IfStatement ifStatement = (IfStatement)node;
			if (!Node.Matches(_modifier, ifStatement._modifier))
			{
				return NoMatch("IfStatement._modifier");
			}
			if (!Node.Matches(_condition, ifStatement._condition))
			{
				return NoMatch("IfStatement._condition");
			}
			if (!Node.Matches(_trueBlock, ifStatement._trueBlock))
			{
				return NoMatch("IfStatement._trueBlock");
			}
			if (!Node.Matches(_falseBlock, ifStatement._falseBlock))
			{
				return NoMatch("IfStatement._falseBlock");
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
			if (_trueBlock == existing)
			{
				TrueBlock = (Block)newNode;
				return true;
			}
			if (_falseBlock == existing)
			{
				FalseBlock = (Block)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			IfStatement ifStatement = new IfStatement();
			ifStatement._lexicalInfo = _lexicalInfo;
			ifStatement._endSourceLocation = _endSourceLocation;
			ifStatement._documentation = _documentation;
			ifStatement._isSynthetic = _isSynthetic;
			ifStatement._entity = _entity;
			if (_annotations != null)
			{
				ifStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				ifStatement._modifier = _modifier.Clone() as StatementModifier;
				ifStatement._modifier.InitializeParent(ifStatement);
			}
			if (null != _condition)
			{
				ifStatement._condition = _condition.Clone() as Expression;
				ifStatement._condition.InitializeParent(ifStatement);
			}
			if (null != _trueBlock)
			{
				ifStatement._trueBlock = _trueBlock.Clone() as Block;
				ifStatement._trueBlock.InitializeParent(ifStatement);
			}
			if (null != _falseBlock)
			{
				ifStatement._falseBlock = _falseBlock.Clone() as Block;
				ifStatement._falseBlock.InitializeParent(ifStatement);
			}
			return ifStatement;
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
			if (null != _trueBlock)
			{
				_trueBlock.ClearTypeSystemBindings();
			}
			if (null != _falseBlock)
			{
				_falseBlock.ClearTypeSystemBindings();
			}
		}

		public IfStatement()
		{
		}

		public IfStatement(Expression condition, Block trueBlock, Block falseBlock)
			: this(LexicalInfo.Empty, condition, trueBlock, falseBlock)
		{
		}

		public IfStatement(LexicalInfo token, Expression condition, Block trueBlock, Block falseBlock)
			: base(token)
		{
			base.Condition = condition;
			TrueBlock = trueBlock;
			FalseBlock = falseBlock;
		}

		public IfStatement(LexicalInfo token)
			: base(token)
		{
		}
	}
}
