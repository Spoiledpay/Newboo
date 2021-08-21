using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class MacroStatement : Statement, INodeWithBody
	{
		protected string _name;

		protected ExpressionCollection _arguments;

		protected Block _body;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.MacroStatement;

		[GeneratedCode("astgen.boo", "1")]
		[XmlAttribute]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[XmlArrayItem(typeof(Expression))]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		public ExpressionCollection Arguments
		{
			get
			{
				return _arguments ?? (_arguments = new ExpressionCollection(this));
			}
			set
			{
				if (_arguments != value)
				{
					_arguments = value;
					if (null != _arguments)
					{
						_arguments.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block Body
		{
			get
			{
				if (_body == null)
				{
					_body = new Block();
					_body.InitializeParent(this);
				}
				return _body;
			}
			set
			{
				if (_body != value)
				{
					_body = value;
					if (null != _body)
					{
						_body.InitializeParent(this);
					}
				}
			}
		}

		[XmlIgnore]
		[Obsolete("Use Body property instead of Block.")]
		public Block Block
		{
			get
			{
				return Body;
			}
			set
			{
				Body = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new MacroStatement CloneNode()
		{
			return (MacroStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new MacroStatement CleanClone()
		{
			return (MacroStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnMacroStatement(this);
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
			MacroStatement macroStatement = (MacroStatement)node;
			if (!Node.Matches(_modifier, macroStatement._modifier))
			{
				return NoMatch("MacroStatement._modifier");
			}
			if (_name != macroStatement._name)
			{
				return NoMatch("MacroStatement._name");
			}
			if (!Node.AllMatch(_arguments, macroStatement._arguments))
			{
				return NoMatch("MacroStatement._arguments");
			}
			if (!Node.Matches(_body, macroStatement._body))
			{
				return NoMatch("MacroStatement._body");
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
			if (_arguments != null)
			{
				Expression expression = existing as Expression;
				if (null != expression)
				{
					Expression newItem = (Expression)newNode;
					if (_arguments.Replace(expression, newItem))
					{
						return true;
					}
				}
			}
			if (_body == existing)
			{
				Body = (Block)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			MacroStatement macroStatement = new MacroStatement();
			macroStatement._lexicalInfo = _lexicalInfo;
			macroStatement._endSourceLocation = _endSourceLocation;
			macroStatement._documentation = _documentation;
			macroStatement._isSynthetic = _isSynthetic;
			macroStatement._entity = _entity;
			if (_annotations != null)
			{
				macroStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				macroStatement._modifier = _modifier.Clone() as StatementModifier;
				macroStatement._modifier.InitializeParent(macroStatement);
			}
			macroStatement._name = _name;
			if (null != _arguments)
			{
				macroStatement._arguments = _arguments.Clone() as ExpressionCollection;
				macroStatement._arguments.InitializeParent(macroStatement);
			}
			if (null != _body)
			{
				macroStatement._body = _body.Clone() as Block;
				macroStatement._body.InitializeParent(macroStatement);
			}
			return macroStatement;
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
			if (null != _arguments)
			{
				_arguments.ClearTypeSystemBindings();
			}
			if (null != _body)
			{
				_body.ClearTypeSystemBindings();
			}
		}

		public MacroStatement()
		{
		}

		public MacroStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public MacroStatement(LexicalInfo lexicalInfoProvider, string name)
			: base(lexicalInfoProvider)
		{
			Name = name;
		}

		public MacroStatement(string name)
			: this(LexicalInfo.Empty, name)
		{
		}

		public override string ToString()
		{
			if (Arguments.Count == 0)
			{
				return _name;
			}
			return _name + " " + Builtins.join(Arguments, ", ");
		}

		public MacroStatement GetParentMacroByName(string name)
		{
			MacroStatement ancestor = GetAncestor<MacroStatement>();
			while (null != ancestor)
			{
				if (ancestor.Name == name)
				{
					return ancestor;
				}
				if (ancestor.Name == "macro" && name == (ancestor.Arguments[0] as ReferenceExpression).Name)
				{
					return ancestor;
				}
				ancestor = ancestor.GetAncestor<MacroStatement>();
			}
			return null;
		}
	}
}
