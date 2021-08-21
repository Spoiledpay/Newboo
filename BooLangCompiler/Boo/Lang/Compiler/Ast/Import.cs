using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Import : Node
	{
		protected Expression _expression;

		protected ReferenceExpression _assemblyReference;

		protected ReferenceExpression _alias;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Import;

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

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public ReferenceExpression AssemblyReference
		{
			get
			{
				return _assemblyReference;
			}
			set
			{
				if (_assemblyReference != value)
				{
					_assemblyReference = value;
					if (null != _assemblyReference)
					{
						_assemblyReference.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public ReferenceExpression Alias
		{
			get
			{
				return _alias;
			}
			set
			{
				if (_alias != value)
				{
					_alias = value;
					if (null != _alias)
					{
						_alias.InitializeParent(this);
					}
				}
			}
		}

		public string Namespace => NamespaceFrom(Expression);

		[GeneratedCode("astgen.boo", "1")]
		public new Import CloneNode()
		{
			return (Import)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Import CleanClone()
		{
			return (Import)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnImport(this);
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
			Import import = (Import)node;
			if (!Node.Matches(_expression, import._expression))
			{
				return NoMatch("Import._expression");
			}
			if (!Node.Matches(_assemblyReference, import._assemblyReference))
			{
				return NoMatch("Import._assemblyReference");
			}
			if (!Node.Matches(_alias, import._alias))
			{
				return NoMatch("Import._alias");
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
			if (_assemblyReference == existing)
			{
				AssemblyReference = (ReferenceExpression)newNode;
				return true;
			}
			if (_alias == existing)
			{
				Alias = (ReferenceExpression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Import import = new Import();
			import._lexicalInfo = _lexicalInfo;
			import._endSourceLocation = _endSourceLocation;
			import._documentation = _documentation;
			import._isSynthetic = _isSynthetic;
			import._entity = _entity;
			if (_annotations != null)
			{
				import._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _expression)
			{
				import._expression = _expression.Clone() as Expression;
				import._expression.InitializeParent(import);
			}
			if (null != _assemblyReference)
			{
				import._assemblyReference = _assemblyReference.Clone() as ReferenceExpression;
				import._assemblyReference.InitializeParent(import);
			}
			if (null != _alias)
			{
				import._alias = _alias.Clone() as ReferenceExpression;
				import._alias.InitializeParent(import);
			}
			return import;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _expression)
			{
				_expression.ClearTypeSystemBindings();
			}
			if (null != _assemblyReference)
			{
				_assemblyReference.ClearTypeSystemBindings();
			}
			if (null != _alias)
			{
				_alias.ClearTypeSystemBindings();
			}
		}

		public Import()
		{
		}

		public Import(string namespace_)
			: this(LexicalInfo.Empty, namespace_)
		{
		}

		public Import(Expression namespace_, ReferenceExpression assemblyReference, ReferenceExpression alias)
		{
			Expression = namespace_;
			AssemblyReference = assemblyReference;
			Alias = alias;
		}

		public Import(LexicalInfo lexicalInfo, string namespace_)
			: this(lexicalInfo, new StringLiteralExpression(namespace_))
		{
		}

		public Import(LexicalInfo lexicalInfo, Expression namespace_)
			: base(lexicalInfo)
		{
			Expression = namespace_;
		}

		public Import(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		private string NamespaceFrom(Expression e)
		{
			if (e == null)
			{
				return null;
			}
			ReferenceExpression referenceExpression = e as ReferenceExpression;
			if (referenceExpression != null)
			{
				return referenceExpression.ToString();
			}
			StringLiteralExpression stringLiteralExpression = e as StringLiteralExpression;
			if (stringLiteralExpression != null)
			{
				return stringLiteralExpression.Value;
			}
			return NamespaceFrom(((MethodInvocationExpression)e).Target);
		}

		public override string ToString()
		{
			return Namespace;
		}
	}
}
