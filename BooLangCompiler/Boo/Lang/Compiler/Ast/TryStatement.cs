using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class TryStatement : Statement
	{
		protected Block _protectedBlock;

		protected ExceptionHandlerCollection _exceptionHandlers;

		protected Block _failureBlock;

		protected Block _ensureBlock;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.TryStatement;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block ProtectedBlock
		{
			get
			{
				if (_protectedBlock == null)
				{
					_protectedBlock = new Block();
					_protectedBlock.InitializeParent(this);
				}
				return _protectedBlock;
			}
			set
			{
				if (_protectedBlock != value)
				{
					_protectedBlock = value;
					if (null != _protectedBlock)
					{
						_protectedBlock.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[XmlArrayItem(typeof(ExceptionHandler))]
		[GeneratedCode("astgen.boo", "1")]
		public ExceptionHandlerCollection ExceptionHandlers
		{
			get
			{
				return _exceptionHandlers ?? (_exceptionHandlers = new ExceptionHandlerCollection(this));
			}
			set
			{
				if (_exceptionHandlers != value)
				{
					_exceptionHandlers = value;
					if (null != _exceptionHandlers)
					{
						_exceptionHandlers.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block FailureBlock
		{
			get
			{
				return _failureBlock;
			}
			set
			{
				if (_failureBlock != value)
				{
					_failureBlock = value;
					if (null != _failureBlock)
					{
						_failureBlock.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Block EnsureBlock
		{
			get
			{
				return _ensureBlock;
			}
			set
			{
				if (_ensureBlock != value)
				{
					_ensureBlock = value;
					if (null != _ensureBlock)
					{
						_ensureBlock.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TryStatement CloneNode()
		{
			return (TryStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TryStatement CleanClone()
		{
			return (TryStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnTryStatement(this);
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
			TryStatement tryStatement = (TryStatement)node;
			if (!Node.Matches(_modifier, tryStatement._modifier))
			{
				return NoMatch("TryStatement._modifier");
			}
			if (!Node.Matches(_protectedBlock, tryStatement._protectedBlock))
			{
				return NoMatch("TryStatement._protectedBlock");
			}
			if (!Node.AllMatch(_exceptionHandlers, tryStatement._exceptionHandlers))
			{
				return NoMatch("TryStatement._exceptionHandlers");
			}
			if (!Node.Matches(_failureBlock, tryStatement._failureBlock))
			{
				return NoMatch("TryStatement._failureBlock");
			}
			if (!Node.Matches(_ensureBlock, tryStatement._ensureBlock))
			{
				return NoMatch("TryStatement._ensureBlock");
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
			if (_protectedBlock == existing)
			{
				ProtectedBlock = (Block)newNode;
				return true;
			}
			if (_exceptionHandlers != null)
			{
				ExceptionHandler exceptionHandler = existing as ExceptionHandler;
				if (null != exceptionHandler)
				{
					ExceptionHandler newItem = (ExceptionHandler)newNode;
					if (_exceptionHandlers.Replace(exceptionHandler, newItem))
					{
						return true;
					}
				}
			}
			if (_failureBlock == existing)
			{
				FailureBlock = (Block)newNode;
				return true;
			}
			if (_ensureBlock == existing)
			{
				EnsureBlock = (Block)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			TryStatement tryStatement = new TryStatement();
			tryStatement._lexicalInfo = _lexicalInfo;
			tryStatement._endSourceLocation = _endSourceLocation;
			tryStatement._documentation = _documentation;
			tryStatement._isSynthetic = _isSynthetic;
			tryStatement._entity = _entity;
			if (_annotations != null)
			{
				tryStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				tryStatement._modifier = _modifier.Clone() as StatementModifier;
				tryStatement._modifier.InitializeParent(tryStatement);
			}
			if (null != _protectedBlock)
			{
				tryStatement._protectedBlock = _protectedBlock.Clone() as Block;
				tryStatement._protectedBlock.InitializeParent(tryStatement);
			}
			if (null != _exceptionHandlers)
			{
				tryStatement._exceptionHandlers = _exceptionHandlers.Clone() as ExceptionHandlerCollection;
				tryStatement._exceptionHandlers.InitializeParent(tryStatement);
			}
			if (null != _failureBlock)
			{
				tryStatement._failureBlock = _failureBlock.Clone() as Block;
				tryStatement._failureBlock.InitializeParent(tryStatement);
			}
			if (null != _ensureBlock)
			{
				tryStatement._ensureBlock = _ensureBlock.Clone() as Block;
				tryStatement._ensureBlock.InitializeParent(tryStatement);
			}
			return tryStatement;
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
			if (null != _protectedBlock)
			{
				_protectedBlock.ClearTypeSystemBindings();
			}
			if (null != _exceptionHandlers)
			{
				_exceptionHandlers.ClearTypeSystemBindings();
			}
			if (null != _failureBlock)
			{
				_failureBlock.ClearTypeSystemBindings();
			}
			if (null != _ensureBlock)
			{
				_ensureBlock.ClearTypeSystemBindings();
			}
		}

		public TryStatement()
		{
		}

		public TryStatement(Block ensureBlock)
		{
			EnsureBlock = ensureBlock;
		}

		public TryStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
