using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	[Serializable]
	public class BindTypeDefinitions : AbstractTransformerCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		private InternalTypeSystemProvider _internalTypeSystemProvider;

		public override void Run()
		{
			_internalTypeSystemProvider = My<InternalTypeSystemProvider>.Instance;
			Visit(base.CompileUnit.Modules);
		}

		public override void OnModule(Module node)
		{
			EnsureEntityFor(node);
			Visit(node.Members);
		}

		public override void OnStructDefinition(StructDefinition node)
		{
			ClassDefinition classDefinition = new ClassDefinition(node.LexicalInfo);
			classDefinition.Name = node.Name;
			classDefinition.Attributes = node.Attributes;
			classDefinition.Modifiers = node.Modifiers;
			classDefinition.Members = node.Members;
			classDefinition.GenericParameters = node.GenericParameters;
			classDefinition.BaseTypes = node.BaseTypes;
			classDefinition.BaseTypes.Insert(0, base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.ValueTypeType));
			foreach (TypeMember member in classDefinition.Members)
			{
				if (!member.IsVisibilitySet)
				{
					switch (member.NodeType)
					{
					case NodeType.Field:
						member.Visibility = base.Context.Parameters.DefaultFieldVisibility;
						break;
					case NodeType.Property:
						member.Visibility = base.Context.Parameters.DefaultPropertyVisibility;
						break;
					case NodeType.Method:
						member.Visibility = base.Context.Parameters.DefaultMethodVisibility;
						break;
					}
					if (member.IsProtected)
					{
						member.Visibility = TypeMemberModifiers.Public;
					}
				}
			}
			OnClassDefinition(classDefinition);
			ReplaceCurrentNode(classDefinition);
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			EnsureEntityFor(node);
			Visit(node.Members);
		}

		private void EnsureEntityFor(TypeMember node)
		{
			_internalTypeSystemProvider.EntityFor(node);
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			EnsureEntityFor(node);
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			EnsureEntityFor(node);
		}

		public override void OnMethod(Method method)
		{
		}

		public override void OnProperty(Property property)
		{
		}

		public override void OnField(Field field)
		{
		}

		public TypeMember Reify(TypeMember member)
		{
			return Visit(member);
		}
	}
}
