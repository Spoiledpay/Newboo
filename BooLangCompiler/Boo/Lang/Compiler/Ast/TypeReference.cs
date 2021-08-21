using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(ArrayTypeReference))]
	[XmlInclude(typeof(GenericTypeDefinitionReference))]
	[XmlInclude(typeof(SimpleTypeReference))]
	[XmlInclude(typeof(GenericTypeReference))]
	public abstract class TypeReference : Node
	{
		protected bool _isPointer;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public bool IsPointer
		{
			get
			{
				return _isPointer;
			}
			set
			{
				_isPointer = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeReference CloneNode()
		{
			return (TypeReference)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeReference CleanClone()
		{
			return (TypeReference)base.CleanClone();
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
			TypeReference typeReference = (TypeReference)node;
			if (_isPointer != typeReference._isPointer)
			{
				return NoMatch("TypeReference._isPointer");
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
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			throw new InvalidOperationException("Cannot clone abstract class: TypeReference");
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public static TypeReference Lift(Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (type.IsGenericType)
			{
				return LiftGenericType(type);
			}
			return new SimpleTypeReference(FullNameOf(type));
		}

		public static TypeReference Lift(string name)
		{
			if (name == null)
			{
				return null;
			}
			return new SimpleTypeReference(name);
		}

		public static TypeReference Lift(TypeReference node)
		{
			return node?.CloneNode();
		}

		public static TypeReference Lift(TypeDefinition node)
		{
			if (node == null)
			{
				return null;
			}
			if (node.HasGenericParameters)
			{
				return LiftGenericTypeDefinition(node);
			}
			return new SimpleTypeReference(node.FullName);
		}

		private static TypeReference LiftGenericTypeDefinition(TypeDefinition node)
		{
			GenericTypeReference genericTypeReference = new GenericTypeReference(node.LexicalInfo, node.QualifiedName);
			foreach (GenericParameterDeclaration genericParameter in node.GenericParameters)
			{
				genericTypeReference.GenericArguments.Add(Lift(genericParameter.Name));
			}
			return genericTypeReference;
		}

		public static TypeReference Lift(Expression e)
		{
			if (e == null)
			{
				return null;
			}
			return e.NodeType switch
			{
				NodeType.TypeofExpression => Lift((TypeofExpression)e), 
				NodeType.GenericReferenceExpression => Lift((GenericReferenceExpression)e), 
				NodeType.ReferenceExpression => Lift((ReferenceExpression)e), 
				NodeType.MemberReferenceExpression => Lift((MemberReferenceExpression)e), 
				_ => throw new NotImplementedException(e.ToCodeString()), 
			};
		}

		public static TypeReference Lift(ReferenceExpression e)
		{
			if (e == null)
			{
				return null;
			}
			return new SimpleTypeReference(e.LexicalInfo, e.ToString());
		}

		public static TypeReference Lift(TypeofExpression e)
		{
			return e?.Type.CloneNode();
		}

		public static TypeReference Lift(GenericReferenceExpression e)
		{
			if (e == null)
			{
				return null;
			}
			GenericTypeReference genericTypeReference = new GenericTypeReference(e.LexicalInfo);
			genericTypeReference.Name = TypeNameFor(e.Target);
			genericTypeReference.GenericArguments.ExtendWithClones(e.GenericArguments);
			return genericTypeReference;
		}

		private static string TypeNameFor(Expression target)
		{
			return target.ToString();
		}

		private static string FullNameOf(Type type)
		{
			return TypeUtilities.GetFullName(type);
		}

		private static TypeReference LiftGenericType(Type type)
		{
			GenericTypeReference genericTypeReference = new GenericTypeReference();
			genericTypeReference.Name = FullNameOf(type);
			GenericTypeReference genericTypeReference2 = genericTypeReference;
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type type2 in genericArguments)
			{
				genericTypeReference2.GenericArguments.Add(Lift(type2));
			}
			return genericTypeReference2;
		}

		public TypeReference()
		{
		}

		public TypeReference(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
