using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Attribute : Node, INodeWithArguments
	{
		protected string _name;

		protected ExpressionCollection _arguments;

		protected ExpressionPairCollection _namedArguments;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Attribute;

		[XmlAttribute]
		[GeneratedCode("astgen.boo", "1")]
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
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
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

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(ExpressionPair))]
		public ExpressionPairCollection NamedArguments
		{
			get
			{
				return _namedArguments ?? (_namedArguments = new ExpressionPairCollection(this));
			}
			set
			{
				if (_namedArguments != value)
				{
					_namedArguments = value;
					if (null != _namedArguments)
					{
						_namedArguments.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Attribute CloneNode()
		{
			return (Attribute)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Attribute CleanClone()
		{
			return (Attribute)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnAttribute(this);
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
			Attribute attribute = (Attribute)node;
			if (_name != attribute._name)
			{
				return NoMatch("Attribute._name");
			}
			if (!Node.AllMatch(_arguments, attribute._arguments))
			{
				return NoMatch("Attribute._arguments");
			}
			if (!Node.AllMatch(_namedArguments, attribute._namedArguments))
			{
				return NoMatch("Attribute._namedArguments");
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
			if (_namedArguments != null)
			{
				ExpressionPair expressionPair = existing as ExpressionPair;
				if (null != expressionPair)
				{
					ExpressionPair newItem2 = (ExpressionPair)newNode;
					if (_namedArguments.Replace(expressionPair, newItem2))
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
			Attribute attribute = new Attribute();
			attribute._lexicalInfo = _lexicalInfo;
			attribute._endSourceLocation = _endSourceLocation;
			attribute._documentation = _documentation;
			attribute._isSynthetic = _isSynthetic;
			attribute._entity = _entity;
			if (_annotations != null)
			{
				attribute._annotations = (Hashtable)_annotations.Clone();
			}
			attribute._name = _name;
			if (null != _arguments)
			{
				attribute._arguments = _arguments.Clone() as ExpressionCollection;
				attribute._arguments.InitializeParent(attribute);
			}
			if (null != _namedArguments)
			{
				attribute._namedArguments = _namedArguments.Clone() as ExpressionPairCollection;
				attribute._namedArguments.InitializeParent(attribute);
			}
			return attribute;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _arguments)
			{
				_arguments.ClearTypeSystemBindings();
			}
			if (null != _namedArguments)
			{
				_namedArguments.ClearTypeSystemBindings();
			}
		}

		public Attribute()
		{
		}

		public Attribute(string name)
		{
			Name = name;
		}

		public Attribute(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public Attribute(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			Name = name;
		}
	}
}
