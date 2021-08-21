using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class RaiseStatement : Statement
	{
		protected Expression _exception;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.RaiseStatement;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Exception
		{
			get
			{
				return _exception;
			}
			set
			{
				if (_exception != value)
				{
					_exception = value;
					if (null != _exception)
					{
						_exception.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new RaiseStatement CloneNode()
		{
			return (RaiseStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new RaiseStatement CleanClone()
		{
			return (RaiseStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnRaiseStatement(this);
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
			RaiseStatement raiseStatement = (RaiseStatement)node;
			if (!Node.Matches(_modifier, raiseStatement._modifier))
			{
				return NoMatch("RaiseStatement._modifier");
			}
			if (!Node.Matches(_exception, raiseStatement._exception))
			{
				return NoMatch("RaiseStatement._exception");
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
			if (_exception == existing)
			{
				Exception = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			RaiseStatement raiseStatement = new RaiseStatement();
			raiseStatement._lexicalInfo = _lexicalInfo;
			raiseStatement._endSourceLocation = _endSourceLocation;
			raiseStatement._documentation = _documentation;
			raiseStatement._isSynthetic = _isSynthetic;
			raiseStatement._entity = _entity;
			if (_annotations != null)
			{
				raiseStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				raiseStatement._modifier = _modifier.Clone() as StatementModifier;
				raiseStatement._modifier.InitializeParent(raiseStatement);
			}
			if (null != _exception)
			{
				raiseStatement._exception = _exception.Clone() as Expression;
				raiseStatement._exception.InitializeParent(raiseStatement);
			}
			return raiseStatement;
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
			if (null != _exception)
			{
				_exception.ClearTypeSystemBindings();
			}
		}

		public RaiseStatement()
		{
		}

		public RaiseStatement(Expression exception)
		{
			Exception = exception;
		}

		public RaiseStatement(LexicalInfo lexicalInfo, Expression exception)
			: base(lexicalInfo)
		{
			Exception = exception;
		}

		public RaiseStatement(LexicalInfo li, Expression exception, StatementModifier modifier)
			: base(li)
		{
			Exception = exception;
			base.Modifier = modifier;
		}

		public RaiseStatement(Expression exception, StatementModifier modifier)
		{
			Exception = exception;
			base.Modifier = modifier;
		}

		public RaiseStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
