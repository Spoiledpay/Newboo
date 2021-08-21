using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class BlockExpression : Expression, INodeWithParameters, INodeWithBody
	{
		protected ParameterDeclarationCollection _parameters;

		protected TypeReference _returnType;

		protected Block _body;

		public static string ClosureNameAnnotation = "ClosureName";

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.BlockExpression;

		[XmlArray]
		[XmlArrayItem(typeof(ParameterDeclaration))]
		[GeneratedCode("astgen.boo", "1")]
		public ParameterDeclarationCollection Parameters
		{
			get
			{
				return _parameters ?? (_parameters = new ParameterDeclarationCollection(this));
			}
			set
			{
				if (_parameters != value)
				{
					_parameters = value;
					if (null != _parameters)
					{
						_parameters.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public TypeReference ReturnType
		{
			get
			{
				return _returnType;
			}
			set
			{
				if (_returnType != value)
				{
					_returnType = value;
					if (null != _returnType)
					{
						_returnType.InitializeParent(this);
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

		public bool IsSimpleBlock => !HasReturnType && !HasParameters;

		public bool HasReturnType => _returnType != null;

		public bool HasParameters => _parameters != null && _parameters.Count > 0;

		[GeneratedCode("astgen.boo", "1")]
		public new BlockExpression CloneNode()
		{
			return (BlockExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new BlockExpression CleanClone()
		{
			return (BlockExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnBlockExpression(this);
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
			BlockExpression blockExpression = (BlockExpression)node;
			if (!Node.AllMatch(_parameters, blockExpression._parameters))
			{
				return NoMatch("BlockExpression._parameters");
			}
			if (!Node.Matches(_returnType, blockExpression._returnType))
			{
				return NoMatch("BlockExpression._returnType");
			}
			if (!Node.Matches(_body, blockExpression._body))
			{
				return NoMatch("BlockExpression._body");
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
			if (_parameters != null)
			{
				ParameterDeclaration parameterDeclaration = existing as ParameterDeclaration;
				if (null != parameterDeclaration)
				{
					ParameterDeclaration newItem = (ParameterDeclaration)newNode;
					if (_parameters.Replace(parameterDeclaration, newItem))
					{
						return true;
					}
				}
			}
			if (_returnType == existing)
			{
				ReturnType = (TypeReference)newNode;
				return true;
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
			BlockExpression blockExpression = new BlockExpression();
			blockExpression._lexicalInfo = _lexicalInfo;
			blockExpression._endSourceLocation = _endSourceLocation;
			blockExpression._documentation = _documentation;
			blockExpression._isSynthetic = _isSynthetic;
			blockExpression._entity = _entity;
			if (_annotations != null)
			{
				blockExpression._annotations = (Hashtable)_annotations.Clone();
			}
			blockExpression._expressionType = _expressionType;
			if (null != _parameters)
			{
				blockExpression._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				blockExpression._parameters.InitializeParent(blockExpression);
			}
			if (null != _returnType)
			{
				blockExpression._returnType = _returnType.Clone() as TypeReference;
				blockExpression._returnType.InitializeParent(blockExpression);
			}
			if (null != _body)
			{
				blockExpression._body = _body.Clone() as Block;
				blockExpression._body.InitializeParent(blockExpression);
			}
			return blockExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _parameters)
			{
				_parameters.ClearTypeSystemBindings();
			}
			if (null != _returnType)
			{
				_returnType.ClearTypeSystemBindings();
			}
			if (null != _body)
			{
				_body.ClearTypeSystemBindings();
			}
		}

		public BlockExpression()
		{
		}

		public BlockExpression(LexicalInfo lexicalInfo, Block body)
			: base(lexicalInfo)
		{
			Body = body;
		}

		public BlockExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public BlockExpression(Block body)
			: base(body.LexicalInfo)
		{
			Body = body;
		}
	}
}
