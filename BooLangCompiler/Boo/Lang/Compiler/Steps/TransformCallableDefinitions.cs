using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class TransformCallableDefinitions : AbstractTransformerCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void OnMethod(Method node)
		{
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			Visit(node.Members);
		}

		public override void OnCallableDefinition(CallableDefinition node)
		{
			CompleteOmittedReturnType(node);
			CompleteOmittedParameterTypes(node);
			ClassDefinition replacement = My<CallableTypeBuilder>.Instance.ForCallableDefinition(node);
			ReplaceCurrentNode(replacement);
		}

		private void CompleteOmittedReturnType(CallableDefinition node)
		{
			if (node.ReturnType == null)
			{
				node.ReturnType = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.VoidType);
			}
		}

		private void CompleteOmittedParameterTypes(CallableDefinition node)
		{
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters.Count == 0)
			{
				return;
			}
			foreach (ParameterDeclaration item in parameters)
			{
				if (item.Type == null)
				{
					item.Type = base.CodeBuilder.CreateTypeReference(item.IsParamArray ? base.TypeSystemServices.ObjectArrayType : base.TypeSystemServices.ObjectType);
				}
			}
		}

		public TypeMember Reify(TypeMember node)
		{
			return Visit(node);
		}
	}
}
