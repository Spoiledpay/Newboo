using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class NormalizeTypeAndMemberDefinitions : AbstractVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void OnModule(Boo.Lang.Compiler.Ast.Module node)
		{
			Visit(node.Members);
		}

		private void LeaveTypeDefinition(TypeDefinition node)
		{
			if (!node.IsVisibilitySet)
			{
				node.Modifiers |= base.Context.Parameters.DefaultTypeVisibility;
			}
			node.Name = NormalizeName(node.Name);
		}

		public override void LeaveExplicitMemberInfo(ExplicitMemberInfo node)
		{
			((TypeMember)node.ParentNode).Modifiers |= TypeMemberModifiers.Private | TypeMemberModifiers.Virtual;
		}

		public override void LeaveEnumDefinition(EnumDefinition node)
		{
			LeaveTypeDefinition(node);
		}

		public override void LeaveInterfaceDefinition(InterfaceDefinition node)
		{
			LeaveTypeDefinition(node);
		}

		public override void LeaveClassDefinition(ClassDefinition node)
		{
			LeaveTypeDefinition(node);
			if (!node.HasInstanceConstructor && !node.IsStatic)
			{
				node.Members.Add(AstUtil.CreateDefaultConstructor(node));
			}
		}

		public override void LeaveStructDefinition(StructDefinition node)
		{
			LeaveTypeDefinition(node);
		}

		public override void LeaveField(Field node)
		{
			if (!node.IsVisibilitySet)
			{
				node.Visibility = base.Context.Parameters.DefaultFieldVisibility;
				if (node.IsProtected && node.DeclaringType.IsFinal)
				{
					node.Visibility = TypeMemberModifiers.Private;
				}
			}
			LeaveMember(node);
		}

		public override void LeaveProperty(Property node)
		{
			NormalizeDefaultItemProperty(node);
			NormalizePropertyModifiers(node);
			LeaveMember(node);
		}

		private void NormalizeDefaultItemProperty(Property node)
		{
			if (IsDefaultItemProperty(node))
			{
				node.Name = "Item";
				TypeDefinition declaringType = node.DeclaringType;
				if (declaringType != null)
				{
					AddDefaultMemberAttribute(declaringType, node);
				}
			}
		}

		private void AddDefaultMemberAttribute(TypeDefinition type, Property node)
		{
			if (!ContainsDefaultMemberAttribute(type))
			{
				Attribute attribute = base.CodeBuilder.CreateAttribute(DefaultMemberAttributeStringConstructor(), new StringLiteralExpression(node.Name));
				attribute.LexicalInfo = node.LexicalInfo;
				type.Attributes.Add(attribute);
			}
		}

		private IConstructor DefaultMemberAttributeStringConstructor()
		{
			return base.TypeSystemServices.Map(Methods.ConstructorOf(() => new DefaultMemberAttribute(null)));
		}

		private static bool ContainsDefaultMemberAttribute(TypeDefinition t)
		{
			foreach (Attribute attribute in t.Attributes)
			{
				if (attribute.Name.IndexOf("DefaultMember") >= 0)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsDefaultItemProperty(Property node)
		{
			return (node.Name == "Item" || node.Name == "self") && node.Parameters.Count > 0 && !node.IsStatic;
		}

		private void NormalizePropertyModifiers(Property node)
		{
			if (IsInterfaceMember(node))
			{
				node.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Abstract;
			}
			else if (!node.IsVisibilitySet && null == node.ExplicitInfo)
			{
				node.Modifiers |= base.Context.Parameters.DefaultPropertyVisibility;
			}
			if (null != node.Getter)
			{
				SetPropertyAccessorModifiers(node, node.Getter);
				node.Getter.Name = "get_" + node.Name;
			}
			if (null != node.Setter)
			{
				SetPropertyAccessorModifiers(node, node.Setter);
				node.Setter.Name = "set_" + node.Name;
			}
		}

		private static bool IsInterfaceMember(TypeMember node)
		{
			TypeDefinition declaringType = node.DeclaringType;
			if (null == declaringType)
			{
				throw CompilerErrorFactory.NotImplemented(node, $"{node.GetType().Name} '{node.Name}' is not attached to any type. It should probably have been consumed by a macro but it hasn't.");
			}
			return NodeType.InterfaceDefinition == declaringType.NodeType;
		}

		private void SetPropertyAccessorModifiers(Property property, Method accessor)
		{
			if (!accessor.IsVisibilitySet)
			{
				accessor.Modifiers |= property.Visibility;
			}
			if (property.IsStatic)
			{
				accessor.Modifiers |= TypeMemberModifiers.Static;
			}
			if (property.IsVirtual)
			{
				accessor.Modifiers |= TypeMemberModifiers.Virtual;
			}
			if (property.IsAbstract)
			{
				accessor.Modifiers |= TypeMemberModifiers.Abstract;
			}
			else if (accessor.IsAbstract)
			{
				property.Modifiers |= TypeMemberModifiers.Abstract;
			}
		}

		public override void LeaveEvent(Event node)
		{
			if (IsInterfaceMember(node))
			{
				node.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Abstract;
			}
			else if (!node.IsVisibilitySet)
			{
				node.Modifiers |= base.Context.Parameters.DefaultEventVisibility;
			}
			LeaveMember(node);
		}

		public override void LeaveMethod(Method node)
		{
			if (IsInterfaceMember(node))
			{
				node.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Abstract;
			}
			else if (!node.IsVisibilitySet && node.ExplicitInfo == null && node.ParentNode.NodeType != NodeType.Property)
			{
				node.Modifiers |= base.Context.Parameters.DefaultMethodVisibility;
			}
			if (node.Name != null && node.Name.StartsWith("op_"))
			{
				node.Modifiers |= TypeMemberModifiers.Static;
			}
			LeaveMember(node);
		}

		public override void OnDestructor(Destructor node)
		{
			Method method = base.CodeBuilder.CreateMethod("Finalize", base.TypeSystemServices.VoidType, TypeMemberModifiers.Protected | TypeMemberModifiers.Override);
			method.LexicalInfo = node.LexicalInfo;
			MethodInvocationExpression expression = new MethodInvocationExpression(new SuperLiteralExpression());
			Block block = new Block();
			Block block2 = new Block();
			block2.Add(expression);
			TryStatement tryStatement = new TryStatement();
			tryStatement.EnsureBlock = block2;
			tryStatement.ProtectedBlock = node.Body;
			block.Add(tryStatement);
			method.Body = block;
			node.ParentNode.Replace(node, method);
		}

		private void LeaveMember(TypeMember node)
		{
			if (node.IsAbstract && !IsInterfaceMember(node))
			{
				node.DeclaringType.Modifiers |= TypeMemberModifiers.Abstract;
			}
			node.Name = NormalizeName(node.Name);
		}

		public override void LeaveConstructor(Constructor node)
		{
			if (!node.IsVisibilitySet)
			{
				if (!node.IsStatic)
				{
					node.Modifiers |= base.Context.Parameters.DefaultMethodVisibility;
				}
				else
				{
					node.Modifiers |= TypeMemberModifiers.Private;
				}
			}
		}

		public TypeMember Reify(TypeMember member)
		{
			Visit(member);
			return member;
		}

		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			node.Name = NormalizeName(node.Name);
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			node.Name = NormalizeName(node.Name);
		}

		protected string NormalizeName(string name)
		{
			if (name != null && name.Length > 1 && name.StartsWith("@"))
			{
				name = name.Substring(1, name.Length - 1);
			}
			return name;
		}
	}
}
