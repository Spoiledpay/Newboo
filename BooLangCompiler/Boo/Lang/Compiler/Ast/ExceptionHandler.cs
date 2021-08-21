using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExceptionHandler : Node
	{
		protected Declaration _declaration;

		protected Expression _filterCondition;

		protected ExceptionHandlerFlags _flags;

		protected Block _block;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExceptionHandler;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Declaration Declaration
		{
			get
			{
				return _declaration;
			}
			set
			{
				if (_declaration != value)
				{
					_declaration = value;
					if (null != _declaration)
					{
						_declaration.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression FilterCondition
		{
			get
			{
				return _filterCondition;
			}
			set
			{
				if (_filterCondition != value)
				{
					_filterCondition = value;
					if (null != _filterCondition)
					{
						_filterCondition.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public ExceptionHandlerFlags Flags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
			}
		}

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

		[GeneratedCode("astgen.boo", "1")]
		public new ExceptionHandler CloneNode()
		{
			return (ExceptionHandler)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExceptionHandler CleanClone()
		{
			return (ExceptionHandler)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExceptionHandler(this);
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
			ExceptionHandler exceptionHandler = (ExceptionHandler)node;
			if (!Node.Matches(_declaration, exceptionHandler._declaration))
			{
				return NoMatch("ExceptionHandler._declaration");
			}
			if (!Node.Matches(_filterCondition, exceptionHandler._filterCondition))
			{
				return NoMatch("ExceptionHandler._filterCondition");
			}
			if (_flags != exceptionHandler._flags)
			{
				return NoMatch("ExceptionHandler._flags");
			}
			if (!Node.Matches(_block, exceptionHandler._block))
			{
				return NoMatch("ExceptionHandler._block");
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
			if (_declaration == existing)
			{
				Declaration = (Declaration)newNode;
				return true;
			}
			if (_filterCondition == existing)
			{
				FilterCondition = (Expression)newNode;
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
			ExceptionHandler exceptionHandler = new ExceptionHandler();
			exceptionHandler._lexicalInfo = _lexicalInfo;
			exceptionHandler._endSourceLocation = _endSourceLocation;
			exceptionHandler._documentation = _documentation;
			exceptionHandler._isSynthetic = _isSynthetic;
			exceptionHandler._entity = _entity;
			if (_annotations != null)
			{
				exceptionHandler._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _declaration)
			{
				exceptionHandler._declaration = _declaration.Clone() as Declaration;
				exceptionHandler._declaration.InitializeParent(exceptionHandler);
			}
			if (null != _filterCondition)
			{
				exceptionHandler._filterCondition = _filterCondition.Clone() as Expression;
				exceptionHandler._filterCondition.InitializeParent(exceptionHandler);
			}
			exceptionHandler._flags = _flags;
			if (null != _block)
			{
				exceptionHandler._block = _block.Clone() as Block;
				exceptionHandler._block.InitializeParent(exceptionHandler);
			}
			return exceptionHandler;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _declaration)
			{
				_declaration.ClearTypeSystemBindings();
			}
			if (null != _filterCondition)
			{
				_filterCondition.ClearTypeSystemBindings();
			}
			if (null != _block)
			{
				_block.ClearTypeSystemBindings();
			}
		}

		public ExceptionHandler()
		{
		}

		public ExceptionHandler(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
