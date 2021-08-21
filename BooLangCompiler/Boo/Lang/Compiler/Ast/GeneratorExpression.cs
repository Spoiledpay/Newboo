using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class GeneratorExpression : Expression
	{
		protected Expression _expression;

		protected DeclarationCollection _declarations;

		protected Expression _iterator;

		protected StatementModifier _filter;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.GeneratorExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			set
			{
				if (_expression != value)
				{
					_expression = value;
					if (null != _expression)
					{
						_expression.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[XmlArrayItem(typeof(Declaration))]
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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public StatementModifier Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				if (_filter != value)
				{
					_filter = value;
					if (null != _filter)
					{
						_filter.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GeneratorExpression CloneNode()
		{
			return (GeneratorExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new GeneratorExpression CleanClone()
		{
			return (GeneratorExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnGeneratorExpression(this);
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
			GeneratorExpression generatorExpression = (GeneratorExpression)node;
			if (!Node.Matches(_expression, generatorExpression._expression))
			{
				return NoMatch("GeneratorExpression._expression");
			}
			if (!Node.AllMatch(_declarations, generatorExpression._declarations))
			{
				return NoMatch("GeneratorExpression._declarations");
			}
			if (!Node.Matches(_iterator, generatorExpression._iterator))
			{
				return NoMatch("GeneratorExpression._iterator");
			}
			if (!Node.Matches(_filter, generatorExpression._filter))
			{
				return NoMatch("GeneratorExpression._filter");
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
			if (_expression == existing)
			{
				Expression = (Expression)newNode;
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
			if (_filter == existing)
			{
				Filter = (StatementModifier)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			GeneratorExpression generatorExpression = new GeneratorExpression();
			generatorExpression._lexicalInfo = _lexicalInfo;
			generatorExpression._endSourceLocation = _endSourceLocation;
			generatorExpression._documentation = _documentation;
			generatorExpression._isSynthetic = _isSynthetic;
			generatorExpression._entity = _entity;
			if (_annotations != null)
			{
				generatorExpression._annotations = (Hashtable)_annotations.Clone();
			}
			generatorExpression._expressionType = _expressionType;
			if (null != _expression)
			{
				generatorExpression._expression = _expression.Clone() as Expression;
				generatorExpression._expression.InitializeParent(generatorExpression);
			}
			if (null != _declarations)
			{
				generatorExpression._declarations = _declarations.Clone() as DeclarationCollection;
				generatorExpression._declarations.InitializeParent(generatorExpression);
			}
			if (null != _iterator)
			{
				generatorExpression._iterator = _iterator.Clone() as Expression;
				generatorExpression._iterator.InitializeParent(generatorExpression);
			}
			if (null != _filter)
			{
				generatorExpression._filter = _filter.Clone() as StatementModifier;
				generatorExpression._filter.InitializeParent(generatorExpression);
			}
			return generatorExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
			if (null != _declarations)
			{
				_declarations.ClearTypeSystemBindings();
			}
			if (null != _iterator)
			{
				_iterator.ClearTypeSystemBindings();
			}
			if (null != _filter)
			{
				_filter.ClearTypeSystemBindings();
			}
		}

		public GeneratorExpression()
		{
		}

		public GeneratorExpression(Expression expression, Expression iterator, StatementModifier filter)
		{
			Expression = expression;
			Iterator = iterator;
			Filter = filter;
		}

		public GeneratorExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
