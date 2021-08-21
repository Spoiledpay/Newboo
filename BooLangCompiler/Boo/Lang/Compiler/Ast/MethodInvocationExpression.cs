using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class MethodInvocationExpression : Expression, INodeWithArguments
	{
		protected Expression _target;

		protected ExpressionCollection _arguments;

		protected ExpressionPairCollection _namedArguments;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.MethodInvocationExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (_target != value)
				{
					_target = value;
					if (null != _target)
					{
						_target.InitializeParent(this);
					}
				}
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

		[XmlArray]
		[XmlArrayItem(typeof(ExpressionPair))]
		[GeneratedCode("astgen.boo", "1")]
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
		public new MethodInvocationExpression CloneNode()
		{
			return (MethodInvocationExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new MethodInvocationExpression CleanClone()
		{
			return (MethodInvocationExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnMethodInvocationExpression(this);
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
			MethodInvocationExpression methodInvocationExpression = (MethodInvocationExpression)node;
			if (!Node.Matches(_target, methodInvocationExpression._target))
			{
				return NoMatch("MethodInvocationExpression._target");
			}
			if (!Node.AllMatch(_arguments, methodInvocationExpression._arguments))
			{
				return NoMatch("MethodInvocationExpression._arguments");
			}
			if (!Node.AllMatch(_namedArguments, methodInvocationExpression._namedArguments))
			{
				return NoMatch("MethodInvocationExpression._namedArguments");
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
			if (_target == existing)
			{
				Target = (Expression)newNode;
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
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression._lexicalInfo = _lexicalInfo;
			methodInvocationExpression._endSourceLocation = _endSourceLocation;
			methodInvocationExpression._documentation = _documentation;
			methodInvocationExpression._isSynthetic = _isSynthetic;
			methodInvocationExpression._entity = _entity;
			if (_annotations != null)
			{
				methodInvocationExpression._annotations = (Hashtable)_annotations.Clone();
			}
			methodInvocationExpression._expressionType = _expressionType;
			if (null != _target)
			{
				methodInvocationExpression._target = _target.Clone() as Expression;
				methodInvocationExpression._target.InitializeParent(methodInvocationExpression);
			}
			if (null != _arguments)
			{
				methodInvocationExpression._arguments = _arguments.Clone() as ExpressionCollection;
				methodInvocationExpression._arguments.InitializeParent(methodInvocationExpression);
			}
			if (null != _namedArguments)
			{
				methodInvocationExpression._namedArguments = _namedArguments.Clone() as ExpressionPairCollection;
				methodInvocationExpression._namedArguments.InitializeParent(methodInvocationExpression);
			}
			return methodInvocationExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _target)
			{
				_target.ClearTypeSystemBindings();
			}
			if (null != _arguments)
			{
				_arguments.ClearTypeSystemBindings();
			}
			if (null != _namedArguments)
			{
				_namedArguments.ClearTypeSystemBindings();
			}
		}

		public MethodInvocationExpression()
		{
		}

		public MethodInvocationExpression(Expression target)
		{
			Target = target;
		}

		public MethodInvocationExpression(LexicalInfo li, Expression target)
			: base(li)
		{
			Target = target;
		}

		public MethodInvocationExpression(LexicalInfo li, Expression target, params Expression[] args)
			: base(li)
		{
			Target = target;
			Arguments.AddRange(args);
		}

		public MethodInvocationExpression(Expression target, params Expression[] args)
		{
			Target = target;
			Arguments.AddRange(args);
		}

		public MethodInvocationExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
