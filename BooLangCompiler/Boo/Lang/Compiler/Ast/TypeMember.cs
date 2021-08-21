using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(TypeDefinition))]
	[XmlInclude(typeof(EnumMember))]
	[XmlInclude(typeof(Field))]
	[XmlInclude(typeof(Method))]
	[XmlInclude(typeof(Property))]
	public abstract class TypeMember : Node, INodeWithAttributes
	{
		protected TypeMemberModifiers _modifiers;

		protected string _name;

		protected AttributeCollection _attributes;

		[XmlAttribute]
		[GeneratedCode("astgen.boo", "1")]
		[DefaultValue(TypeMemberModifiers.None)]
		public TypeMemberModifiers Modifiers
		{
			get
			{
				return _modifiers;
			}
			set
			{
				_modifiers = value;
			}
		}

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

		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		[XmlArrayItem(typeof(Attribute))]
		public AttributeCollection Attributes
		{
			get
			{
				return _attributes ?? (_attributes = new AttributeCollection(this));
			}
			set
			{
				if (_attributes != value)
				{
					_attributes = value;
					if (null != _attributes)
					{
						_attributes.InitializeParent(this);
					}
				}
			}
		}

		public virtual TypeDefinition DeclaringType => base.ParentNode as TypeDefinition;

		public virtual string FullName
		{
			get
			{
				if (null != base.ParentNode)
				{
					return DeclaringType.FullName + "." + Name;
				}
				return Name;
			}
		}

		public virtual NamespaceDeclaration EnclosingNamespace => EnclosingModule?.Namespace;

		public Module EnclosingModule => GetAncestor<Module>();

		public TypeMemberModifiers Visibility
		{
			get
			{
				return _modifiers & TypeMemberModifiers.VisibilityMask;
			}
			set
			{
				_modifiers &= ~TypeMemberModifiers.VisibilityMask;
				_modifiers |= value;
			}
		}

		public bool IsVisibilitySet => IsPublic | IsInternal | IsPrivate | IsProtected;

		public bool IsVisible
		{
			get
			{
				if (IsPrivate || IsInternal)
				{
					return false;
				}
				TypeMember declaringType = DeclaringType;
				while (declaringType != null && !(declaringType is Module))
				{
					if (!declaringType.IsPublic)
					{
						return false;
					}
					declaringType = declaringType.DeclaringType;
				}
				return true;
			}
		}

		public bool IsAbstract => IsModifierSet(TypeMemberModifiers.Abstract);

		public bool IsOverride => IsModifierSet(TypeMemberModifiers.Override);

		public bool IsVirtual => IsModifierSet(TypeMemberModifiers.Virtual);

		public bool IsNew => IsModifierSet(TypeMemberModifiers.New);

		public virtual bool IsStatic => IsModifierSet(TypeMemberModifiers.Static);

		public bool IsPublic => IsModifierSet(TypeMemberModifiers.Public);

		public bool IsInternal => IsModifierSet(TypeMemberModifiers.Internal);

		public bool IsProtected => IsModifierSet(TypeMemberModifiers.Protected);

		public bool IsPrivate => IsModifierSet(TypeMemberModifiers.Private);

		public bool IsFinal => IsModifierSet(TypeMemberModifiers.Final);

		public bool IsTransient => HasTransientModifier || IsStatic;

		public bool HasTransientModifier => IsModifierSet(TypeMemberModifiers.Transient);

		public bool IsPartial => IsModifierSet(TypeMemberModifiers.Partial);

		[GeneratedCode("astgen.boo", "1")]
		public new TypeMember CloneNode()
		{
			return (TypeMember)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new TypeMember CleanClone()
		{
			return (TypeMember)base.CleanClone();
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
			TypeMember typeMember = (TypeMember)node;
			if (_modifiers != typeMember._modifiers)
			{
				return NoMatch("TypeMember._modifiers");
			}
			if (_name != typeMember._name)
			{
				return NoMatch("TypeMember._name");
			}
			if (!Node.AllMatch(_attributes, typeMember._attributes))
			{
				return NoMatch("TypeMember._attributes");
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
			if (_attributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_attributes.Replace(attribute, newItem))
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
			throw new InvalidOperationException("Cannot clone abstract class: TypeMember");
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _attributes)
			{
				_attributes.ClearTypeSystemBindings();
			}
		}

		public static TypeMember Lift(TypeMember member)
		{
			return member;
		}

		public static TypeMember Lift(Statement stmt)
		{
			TypeMemberStatement typeMemberStatement = stmt as TypeMemberStatement;
			if (null != typeMemberStatement)
			{
				return Lift(typeMemberStatement);
			}
			DeclarationStatement declarationStatement = stmt as DeclarationStatement;
			if (null != declarationStatement)
			{
				return Lift(declarationStatement);
			}
			ExpressionStatement expressionStatement = stmt as ExpressionStatement;
			if (null != expressionStatement)
			{
				return Lift(expressionStatement);
			}
			throw new NotImplementedException(stmt.ToCodeString());
		}

		public static TypeMember Lift(TypeMemberStatement stmt)
		{
			return stmt.TypeMember;
		}

		public static TypeMember Lift(DeclarationStatement stmt)
		{
			BlockExpression blockExpression = stmt.Initializer as BlockExpression;
			if (blockExpression != null && blockExpression.ContainsAnnotation(BlockExpression.ClosureNameAnnotation))
			{
				return Lift(blockExpression);
			}
			Field field = new Field(stmt.LexicalInfo);
			field.Name = stmt.Declaration.Name;
			field.Type = stmt.Declaration.Type;
			field.Initializer = stmt.Initializer;
			return field;
		}

		public static TypeMember Lift(ExpressionStatement stmt)
		{
			Expression expression = stmt.Expression;
			BlockExpression blockExpression = expression as BlockExpression;
			if (blockExpression != null)
			{
				return Lift(blockExpression);
			}
			throw new NotImplementedException(stmt.ToCodeString());
		}

		private static TypeMember Lift(BlockExpression closure)
		{
			Method method = new Method(closure.LexicalInfo);
			method.Name = (string)closure[BlockExpression.ClosureNameAnnotation];
			method.Parameters = closure.Parameters;
			method.ReturnType = closure.ReturnType;
			method.Body = closure.Body;
			return method;
		}

		public static TypeMemberCollection Lift(Block block)
		{
			TypeMemberCollection typeMemberCollection = new TypeMemberCollection();
			LiftBlockInto(typeMemberCollection, block);
			return typeMemberCollection;
		}

		private static void LiftBlockInto(TypeMemberCollection collection, Block block)
		{
			foreach (Statement statement in block.Statements)
			{
				Block block2 = statement as Block;
				if (block2 != null)
				{
					LiftBlockInto(collection, block2);
				}
				else
				{
					collection.Add(Lift(statement));
				}
			}
		}

		protected TypeMember()
		{
		}

		protected TypeMember(TypeMemberModifiers modifiers, string name)
		{
			Modifiers = modifiers;
			Name = name;
		}

		protected TypeMember(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}

		public bool IsModifierSet(TypeMemberModifiers modifiers)
		{
			return modifiers == (_modifiers & modifiers);
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
