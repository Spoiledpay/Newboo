using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class DeclarationStatement : Statement
	{
		protected Declaration _declaration;

		protected Expression _initializer;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.DeclarationStatement;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
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
		public Expression Initializer
		{
			get
			{
				return _initializer;
			}
			set
			{
				if (_initializer != value)
				{
					_initializer = value;
					if (null != _initializer)
					{
						_initializer.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new DeclarationStatement CloneNode()
		{
			return (DeclarationStatement)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new DeclarationStatement CleanClone()
		{
			return (DeclarationStatement)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnDeclarationStatement(this);
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
			DeclarationStatement declarationStatement = (DeclarationStatement)node;
			if (!Node.Matches(_modifier, declarationStatement._modifier))
			{
				return NoMatch("DeclarationStatement._modifier");
			}
			if (!Node.Matches(_declaration, declarationStatement._declaration))
			{
				return NoMatch("DeclarationStatement._declaration");
			}
			if (!Node.Matches(_initializer, declarationStatement._initializer))
			{
				return NoMatch("DeclarationStatement._initializer");
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
			if (_declaration == existing)
			{
				Declaration = (Declaration)newNode;
				return true;
			}
			if (_initializer == existing)
			{
				Initializer = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			DeclarationStatement declarationStatement = new DeclarationStatement();
			declarationStatement._lexicalInfo = _lexicalInfo;
			declarationStatement._endSourceLocation = _endSourceLocation;
			declarationStatement._documentation = _documentation;
			declarationStatement._isSynthetic = _isSynthetic;
			declarationStatement._entity = _entity;
			if (_annotations != null)
			{
				declarationStatement._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modifier)
			{
				declarationStatement._modifier = _modifier.Clone() as StatementModifier;
				declarationStatement._modifier.InitializeParent(declarationStatement);
			}
			if (null != _declaration)
			{
				declarationStatement._declaration = _declaration.Clone() as Declaration;
				declarationStatement._declaration.InitializeParent(declarationStatement);
			}
			if (null != _initializer)
			{
				declarationStatement._initializer = _initializer.Clone() as Expression;
				declarationStatement._initializer.InitializeParent(declarationStatement);
			}
			return declarationStatement;
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
			if (null != _declaration)
			{
				_declaration.ClearTypeSystemBindings();
			}
			if (null != _initializer)
			{
				_initializer.ClearTypeSystemBindings();
			}
		}

		public DeclarationStatement()
		{
		}

		public DeclarationStatement(Declaration declaration, Expression initializer)
			: this(LexicalInfo.Empty, declaration, initializer)
		{
		}

		public DeclarationStatement(LexicalInfo token, Declaration declaration, Expression initializer)
			: base(token)
		{
			Declaration = declaration;
			Initializer = initializer;
		}

		public DeclarationStatement(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
