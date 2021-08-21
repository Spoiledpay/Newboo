using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public delegate IType InvocationTypeInferenceRule(MethodInvocationExpression invocation, IMethod method);
}
