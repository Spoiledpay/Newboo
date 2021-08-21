using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	[Serializable]
	public class BindGenericParameters : AbstractFastVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void OnGenericParameterDeclaration(GenericParameterDeclaration node)
		{
			if (node.Entity == null)
			{
				node.Entity = new InternalGenericParameter(base.TypeSystemServices, node);
			}
		}

		public TypeMember Reify(TypeMember node)
		{
			Visit(node);
			return node;
		}
	}
}
