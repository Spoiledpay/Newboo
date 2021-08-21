using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckIdentifiers : AbstractVisitorCompilerStep
	{
		internal static bool IsValidName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			char c = name[0];
			if (name.Length == 1 && c == '@')
			{
				return true;
			}
			return char.IsLetter(c) || c == '(' || c == '_' || c == '$';
		}

		private void CheckName(Node node, string name)
		{
			if (!node.IsSynthetic && !IsValidName(name))
			{
				base.Errors.Add(CompilerErrorFactory.InvalidName(node, name));
			}
		}

		private void CheckParameterUniqueness(Method method)
		{
			List list = new List();
			foreach (ParameterDeclaration parameter in method.Parameters)
			{
				if (list.Contains(parameter.Name))
				{
					base.Errors.Add(CompilerErrorFactory.DuplicateParameterName(parameter, parameter.Name, GetEntity(method)));
				}
				list.Add(parameter.Name);
			}
		}

		public override void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
			CheckName(node, node.Name);
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			CheckName(node, node.Name);
		}

		public override void OnSimpleTypeReference(SimpleTypeReference node)
		{
			CheckName(node, node.Name);
		}

		public override void OnGenericTypeReference(GenericTypeReference node)
		{
			CheckName(node, node.Name);
		}

		public override void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
			CheckName(node, node.Name);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.OnMemberReferenceExpression(node);
			CheckName(node, node.Name);
		}

		public override void OnLabelStatement(LabelStatement node)
		{
			base.OnLabelStatement(node);
			CheckName(node, node.Name);
		}

		public override void OnDeclaration(Declaration node)
		{
			base.OnDeclaration(node);
			if (!(node.ParentNode is ExceptionHandler) || ((node.ParentNode as ExceptionHandler).Flags & ExceptionHandlerFlags.Anonymous) == 0)
			{
				CheckName(node, node.Name);
			}
		}

		public override void LeaveAttribute(Attribute node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveConstructor(Constructor node)
		{
			CheckParameterUniqueness(node);
		}

		public override void LeaveMethod(Method node)
		{
			CheckParameterUniqueness(node);
			CheckName(node, node.Name);
		}

		public override void LeaveParameterDeclaration(ParameterDeclaration node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveImport(Import node)
		{
			if (null != node.Alias)
			{
				CheckName(node, node.Alias.Name);
			}
		}

		public override void LeaveClassDefinition(ClassDefinition node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveStructDefinition(StructDefinition node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveInterfaceDefinition(InterfaceDefinition node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveEnumDefinition(EnumDefinition node)
		{
			CheckName(node, node.Name);
			foreach (EnumMember member in node.Members)
			{
				if (member.Initializer.NodeType != NodeType.IntegerLiteralExpression)
				{
					base.Errors.Add(CompilerErrorFactory.EnumMemberMustBeConstant(member));
				}
			}
		}

		public override void LeaveEvent(Event node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveField(Field node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveProperty(Property node)
		{
			CheckName(node, node.Name);
		}

		public override void LeaveEnumMember(EnumMember node)
		{
			CheckName(node, node.Name);
		}
	}
}
