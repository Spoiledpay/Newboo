using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class UnpackStatement : Statement
	{
		protected DeclarationCollection _declarations;

		protected Expression _expression;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.UnpackStatement;

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

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
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

		[GeneratedCode("astgen.boo", "1")]
		public new UnpackStatement CloneNode()
		{
			return (UnpackStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new UnpackStatement CleanClone()
		{
			return (UnpackStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnUnpackStatement(this);
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
			UnpackStatement unpackStatement = (UnpackStatement)node;
			if (!Node.Matches(_modifier, unpackStatement._modifier))
			{
				return NoMatch("UnpackStatement._modifier");
			}
			if (!Node.AllMatch(_declarations, unpackStatement._declarations))
			{
				return NoMatch("UnpackStatement._declarations");
			}
			if (!Node.Matches(_expression, unpackStatement._expression))
			{
				return NoMatch("UnpackStatement._expression");
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
			if (_expression == existing)
			{
				Expression = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			UnpackStatement unpackStatement = new UnpackStatement();
			unpackStatement._lexicalInfo = _lexicalInfo;
			unpackStatement._endSourceLocation = _endSourceLocation;
			unpackStatement._documentation = _documentation;
			unpackStatement._isSynthetic = _isSynthetic;
			unpackStatement._entity = _entity;
			if (_annotations != null)
			{
				unpackStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				unpackStatement._modifier = _modifier.Clone() as StatementModifier;
				unpackStatement._modifier.InitializeParent(unpackStatement);
			}
			if (null != _declarations)
			{
				unpackStatement._declarations = _declarations.Clone() as DeclarationCollection;
				unpackStatement._declarations.InitializeParent(unpackStatement);
			}
			if (null != _expression)
			{
				unpackStatement._expression = _expression.Clone() as Expression;
				unpackStatement._expression.InitializeParent(unpackStatement);
			}
			return unpackStatement;
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
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
		}

		public UnpackStatement()
		{
		}

		public UnpackStatement(Expression expression)
		{
			Expression = expression;
		}

		public UnpackStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
