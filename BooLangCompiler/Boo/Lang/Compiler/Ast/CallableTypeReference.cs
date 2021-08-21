using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CallableTypeReference : TypeReference, INodeWithParameters
	{
		protected ParameterDeclarationCollection _parameters;

		protected TypeReference _returnType;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CallableTypeReference;

		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArrayItem(typeof(ParameterDeclaration))]
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

		[GeneratedCode("astgen.boo", "1")]
		public new CallableTypeReference CloneNode()
		{
			return (CallableTypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CallableTypeReference CleanClone()
		{
			return (CallableTypeReference)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCallableTypeReference(this);
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
			CallableTypeReference callableTypeReference = (CallableTypeReference)node;
			if (_isPointer != callableTypeReference._isPointer)
			{
				return NoMatch("CallableTypeReference._isPointer");
			}
			if (!Node.AllMatch(_parameters, callableTypeReference._parameters))
			{
				return NoMatch("CallableTypeReference._parameters");
			}
			if (!Node.Matches(_returnType, callableTypeReference._returnType))
			{
				return NoMatch("CallableTypeReference._returnType");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			CallableTypeReference callableTypeReference = new CallableTypeReference();
			callableTypeReference._lexicalInfo = _lexicalInfo;
			callableTypeReference._endSourceLocation = _endSourceLocation;
			callableTypeReference._documentation = _documentation;
			callableTypeReference._isSynthetic = _isSynthetic;
			callableTypeReference._entity = _entity;
			if (_annotations != null)
			{
				callableTypeReference._annotations = (Hashtable)_annotations.Clone();
			}
			callableTypeReference._isPointer = _isPointer;
			if (null != _parameters)
			{
				callableTypeReference._parameters = _parameters.Clone() as ParameterDeclarationCollection;
				callableTypeReference._parameters.InitializeParent(callableTypeReference);
			}
			if (null != _returnType)
			{
				callableTypeReference._returnType = _returnType.Clone() as TypeReference;
				callableTypeReference._returnType.InitializeParent(callableTypeReference);
			}
			return callableTypeReference;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _parameters)
			{
				_parameters.ClearTypeSystemBindings();
			}
			if (null != _returnType)
			{
				_returnType.ClearTypeSystemBindings();
			}
		}

		public CallableTypeReference()
		{
		}

		public CallableTypeReference(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
