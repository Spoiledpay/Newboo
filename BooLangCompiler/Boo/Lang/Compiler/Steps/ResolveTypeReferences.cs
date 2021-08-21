using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	public class ResolveTypeReferences : AbstractNamespaceSensitiveVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>, ITypeReferenceReifier
	{
		public override void OnMethod(Method node)
		{
			if (node.GenericParameters.Count != 0)
			{
				EnterNamespace((INamespace)TypeSystemServices.GetEntity(node));
				base.OnMethod(node);
				LeaveNamespace();
			}
			else
			{
				base.OnMethod(node);
			}
		}

		public override void LeaveArrayTypeReference(ArrayTypeReference node)
		{
			base.NameResolutionService.ResolveArrayTypeReference(node);
		}

		public override void OnSimpleTypeReference(SimpleTypeReference node)
		{
			base.NameResolutionService.ResolveSimpleTypeReference(node);
		}

		public override void LeaveGenericTypeReference(GenericTypeReference node)
		{
			base.NameResolutionService.ResolveSimpleTypeReference(node);
		}

		public override void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
			base.NameResolutionService.ResolveSimpleTypeReference(node);
		}

		public override void LeaveCallableTypeReference(CallableTypeReference node)
		{
			node.Entity = base.TypeSystemServices.GetConcreteCallableType(node, CallableSignatureFor(node));
		}

		private CallableSignature CallableSignatureFor(CallableTypeReference node)
		{
			IParameter[] array = new IParameter[node.Parameters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new InternalParameter(node.Parameters[i], i);
			}
			IType returnType = ((node.ReturnType != null) ? GetType(node.ReturnType) : base.TypeSystemServices.VoidType);
			return new CallableSignature(array, returnType, node.Parameters.HasParamArray);
		}

		public TypeMember Reify(TypeMember member)
		{
			base.NameResolutionService.EnterNamespace((INamespace)GetEntity(member.DeclaringType));
			Visit(member);
			return member;
		}

		public void Reify(TypeReference node)
		{
			node.Accept(this);
		}
	}
}
