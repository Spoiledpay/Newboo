using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class ExpandVarArgsMethodInvocations : AbstractFastVisitorCompilerStep
	{
		public override void Run()
		{
			if (base.Errors.Count == 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			base.OnMethodInvocationExpression(node);
			IMethod method = node.Target.Entity as IMethod;
			if (method != null && method.AcceptVarArgs)
			{
				ExpandInvocation(node, method.GetParameters());
				return;
			}
			ICallableType callableType = node.Target.ExpressionType as ICallableType;
			if (callableType != null)
			{
				CallableSignature signature = callableType.GetSignature();
				if (signature.AcceptVarArgs)
				{
					ExpandInvocation(node, signature.Parameters);
				}
			}
		}

		protected virtual void ExpandInvocation(MethodInvocationExpression node, IParameter[] parameters)
		{
			if (AstUtil.InvocationEndsWithExplodeExpression(node))
			{
				node.Arguments.ReplaceAt(-1, ((UnaryExpression)node.Arguments[-1]).Operand);
				return;
			}
			int num = parameters.Length - 1;
			IType type = parameters[num].Type;
			ExpressionCollection items = node.Arguments.PopRange(num);
			node.Arguments.Add(base.CodeBuilder.CreateArray(type, items));
		}
	}
}
