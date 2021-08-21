using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Environments
{
	[CompilerGlobalScope]
	public static class MyIdiom
	{
		[Meta]
		public static Expression my(ReferenceExpression typeReference)
		{
			GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression(typeReference.LexicalInfo);
			genericReferenceExpression.Target = AstUtil.CreateReferenceExpression(typeReference.LexicalInfo, "Boo.Lang.Environments.My");
			genericReferenceExpression.GenericArguments.Add(TypeReference.Lift(typeReference));
			return new MemberReferenceExpression(typeReference.LexicalInfo, genericReferenceExpression, "Instance");
		}
	}
}
