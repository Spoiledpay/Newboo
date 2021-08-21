using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Block : Statement
	{
		protected StatementCollection _statements;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Block;

		[XmlArrayItem(typeof(Statement))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public StatementCollection Statements
		{
			get
			{
				return _statements ?? (_statements = new StatementCollection(this));
			}
			set
			{
				if (_statements != value)
				{
					_statements = value;
					if (null != _statements)
					{
						_statements.InitializeParent(this);
					}
				}
			}
		}

		public bool IsEmpty => _statements == null || Statements.IsEmpty;

		[Obsolete("HasStatements is Obsolete, use IsEmpty instead")]
		public bool HasStatements => !IsEmpty;

		public Statement FirstStatement => IsEmpty ? null : Statements.First;

		public Statement LastStatement => IsEmpty ? null : Statements.Last;

		[GeneratedCode("astgen.boo", "1")]
		public new Block CloneNode()
		{
			return (Block)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Block CleanClone()
		{
			return (Block)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnBlock(this);
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
			Block block = (Block)node;
			if (!Node.Matches(_modifier, block._modifier))
			{
				return NoMatch("Block._modifier");
			}
			if (!Node.AllMatch(_statements, block._statements))
			{
				return NoMatch("Block._statements");
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
			if (_statements != null)
			{
				Statement statement = existing as Statement;
				if (null != statement)
				{
					Statement newItem = (Statement)newNode;
					if (_statements.Replace(statement, newItem))
					{
						return true;
					}
				}
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Block block = new Block();
			block._lexicalInfo = _lexicalInfo;
			block._endSourceLocation = _endSourceLocation;
			block._documentation = _documentation;
			block._isSynthetic = _isSynthetic;
			block._entity = _entity;
			if (_annotations != null)
			{
				block._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				block._modifier = _modifier.Clone() as StatementModifier;
				block._modifier.InitializeParent(block);
			}
			if (null != _statements)
			{
				block._statements = _statements.Clone() as StatementCollection;
				block._statements.InitializeParent(block);
			}
			return block;
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
			if (null != _statements)
			{
				_statements.ClearTypeSystemBindings();
			}
		}

		public Block()
		{
		}

		public Block(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public Block(params Statement[] statements)
		{
			Statements.AddRange(statements);
		}

		public void Clear()
		{
			_statements = null;
		}

		public bool StartsWith<T>() where T : Statement
		{
			return !IsEmpty && (Statements.StartsWith<T>() || (FirstStatement is Block && ((Block)FirstStatement).StartsWith<T>()));
		}

		public bool EndsWith<T>() where T : Statement
		{
			return !IsEmpty && (Statements.EndsWith<T>() || (LastStatement is Block && ((Block)LastStatement).EndsWith<T>()));
		}

		public override Block ToBlock()
		{
			return this;
		}

		public void Add(Statement stmt)
		{
			Block block = stmt as Block;
			if (block != null)
			{
				Add(block);
			}
			else
			{
				Statements.Add(stmt);
			}
		}

		public void Add(Block block)
		{
			if (block.HasAnnotations)
			{
				Statements.Add(block);
			}
			else
			{
				Statements.AddRange(block.Statements);
			}
		}

		public void Add(Expression expression)
		{
			Statements.Add(StatementFor(expression));
		}

		public void Insert(int index, Expression expression)
		{
			Statements.Insert(index, StatementFor(expression));
		}

		public void Insert(int index, Statement stmt)
		{
			Statements.Insert(index, stmt);
		}

		private static Statement StatementFor(Expression expression)
		{
			return Statement.Lift(expression);
		}

		public Statement Simplify()
		{
			if (IsEmpty)
			{
				return this;
			}
			if (Statements.Count > 1 || base.HasAnnotations)
			{
				return this;
			}
			return Statements[0];
		}
	}
}
