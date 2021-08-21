using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ForStatement : Statement
	{
		protected DeclarationCollection _declarations;

		protected Expression _iterator;

		protected Block _block;

		protected Block _orBlock;

		protected Block _thenBlock;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ForStatement;

		[XmlArrayItem(typeof(Declaration))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public DeclarationCollection Declarations
		{
			get
			{
				return _declarations ?? (_declarations = new DeclarationCollection(this));
			}
			set
			{
				if (_declarations != value)
				{
					_declarations = value;
					if (null != _declarations)
					{
						_declarations.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Iterator
		{
			get
			{
				return _iterator;
			}
			set
			{
				if (_iterator != value)
				{
					_iterator = value;
					if (null != _iterator)
					{
						_iterator.InitializeParent(this);
					}
				}
			}
		}

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
		public new ForStatement CloneNode()
		{
			return (ForStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ForStatement CleanClone()
		{
			return (ForStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnForStatement(this);
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
			ForStatement forStatement = (ForStatement)node;
			if (!Node.Matches(_modifier, forStatement._modifier))
			{
				return NoMatch("ForStatement._modifier");
			}
			if (!Node.AllMatch(_declarations, forStatement._declarations))
			{
				return NoMatch("ForStatement._declarations");
			}
			if (!Node.Matches(_iterator, forStatement._iterator))
			{
				return NoMatch("ForStatement._iterator");
			}
			if (!Node.Matches(_block, forStatement._block))
			{
				return NoMatch("ForStatement._block");
			}
			if (!Node.Matches(_orBlock, forStatement._orBlock))
			{
				return NoMatch("ForStatement._orBlock");
			}
			if (!Node.Matches(_thenBlock, forStatement._thenBlock))
			{
				return NoMatch("ForStatement._thenBlock");
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
			if (_declarations != null)
			{
				Declaration declaration = existing as Declaration;
				if (null != declaration)
				{
					Declaration newItem = (Declaration)newNode;
					if (_declarations.Replace(declaration, newItem))
					{
						return true;
					}
				}
			}
			if (_iterator == existing)
			{
				Iterator = (Expression)newNode;
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
			ForStatement forStatement = new ForStatement();
			forStatement._lexicalInfo = _lexicalInfo;
			forStatement._endSourceLocation = _endSourceLocation;
			forStatement._documentation = _documentation;
			forStatement._isSynthetic = _isSynthetic;
			forStatement._entity = _entity;
			if (_annotations != null)
			{
				forStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				forStatement._modifier = _modifier.Clone() as StatementModifier;
				forStatement._modifier.InitializeParent(forStatement);
			}
			if (null != _declarations)
			{
				forStatement._declarations = _declarations.Clone() as DeclarationCollection;
				forStatement._declarations.InitializeParent(forStatement);
			}
			if (null != _iterator)
			{
				forStatement._iterator = _iterator.Clone() as Expression;
				forStatement._iterator.InitializeParent(forStatement);
			}
			if (null != _block)
			{
				forStatement._block = _block.Clone() as Block;
				forStatement._block.InitializeParent(forStatement);
			}
			if (null != _orBlock)
			{
				forStatement._orBlock = _orBlock.Clone() as Block;
				forStatement._orBlock.InitializeParent(forStatement);
			}
			if (null != _thenBlock)
			{
				forStatement._thenBlock = _thenBlock.Clone() as Block;
				forStatement._thenBlock.InitializeParent(forStatement);
			}
			return forStatement;
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
			if (null != _declarations)
			{
				_declarations.ClearTypeSystemBindings();
			}
			if (null != _iterator)
			{
				_iterator.ClearTypeSystemBindings();
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

		public ForStatement()
		{
		}

		public ForStatement(Expression iterator)
		{
			Iterator = iterator;
		}

		public ForStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
