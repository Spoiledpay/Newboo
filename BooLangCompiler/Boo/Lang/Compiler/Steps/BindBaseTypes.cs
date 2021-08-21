using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps.Inheritance;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	public class BindBaseTypes : AbstractFastVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void OnEnumDefinition(EnumDefinition node)
		{
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			Visit(node.Members);
			ResolveBaseTypesOf(node);
			CheckBaseTypes(node);
			if (!node.IsFinal && ((IType)node.Entity).IsFinal)
			{
				node.Modifiers |= TypeMemberModifiers.Final;
			}
		}

		private void ResolveBaseTypesOf(TypeDefinition node)
		{
			ResolveBaseTypes(new List<TypeDefinition>(), node);
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			ResolveBaseTypesOf(node);
			CheckInterfaceBaseTypes(node);
		}

		public override void OnMethod(Method node)
		{
		}

		private void CheckBaseTypes(ClassDefinition node)
		{
			IType type = null;
			foreach (TypeReference baseType in node.BaseTypes)
			{
				IType type2 = GetType(baseType);
				if (type2.IsInterface)
				{
					continue;
				}
				if (null != type)
				{
					Error(CompilerErrorFactory.ClassAlreadyHasBaseType(baseType, node.Name, type));
					continue;
				}
				type = type2;
				if (type.IsFinal && !TypeSystemServices.IsError(type))
				{
					Error(CompilerErrorFactory.CannotExtendFinalType(baseType, type));
				}
			}
			if (null == type)
			{
				node.BaseTypes.Insert(0, base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.ObjectType));
			}
		}

		private void CheckInterfaceBaseTypes(InterfaceDefinition node)
		{
			foreach (TypeReference baseType in node.BaseTypes)
			{
				IType type = GetType(baseType);
				if (!type.IsInterface)
				{
					Error(CompilerErrorFactory.InterfaceCanOnlyInheritFromInterface(baseType, GetType(node), type));
				}
			}
		}

		private void ResolveBaseTypes(List<TypeDefinition> visited, TypeDefinition node)
		{
			new BaseTypeResolution(base.Context, node, visited);
		}

		public TypeMember Reify(TypeMember member)
		{
			Visit(member);
			return member;
		}
	}
}
