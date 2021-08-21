using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessMethodBodiesWithDuckTyping : ProcessMethodBodies
	{
		private bool Ducky => _context.Parameters.Ducky;

		protected override IEntity CantResolveAmbiguousMethodInvocation(MethodInvocationExpression node, IEntity[] entities)
		{
			if (!Ducky || base.CallableResolutionService.ValidCandidates.Count == 0)
			{
				return base.CantResolveAmbiguousMethodInvocation(node, entities);
			}
			NormalizeMethodInvocationTarget(node);
			BindQuack(node.Target);
			BindDuck(node);
			return null;
		}

		private void NormalizeMethodInvocationTarget(MethodInvocationExpression node)
		{
			if (node.Target.NodeType == NodeType.ReferenceExpression)
			{
				node.Target = MemberReferenceFromReference((ReferenceExpression)node.Target, base.CallableResolutionService.ValidCandidates[0].Method);
			}
		}

		protected override void ProcessBuiltinInvocation(MethodInvocationExpression node, BuiltinFunction function)
		{
			if (base.TypeSystemServices.IsQuackBuiltin(function))
			{
				BindDuck(node);
			}
			else
			{
				base.ProcessBuiltinInvocation(node, function);
			}
		}

		protected override void ProcessAssignment(BinaryExpression node)
		{
			if (base.TypeSystemServices.IsQuackBuiltin(node.Left.Entity))
			{
				BindDuck(node);
			}
			else
			{
				ProcessStaticallyTypedAssignment(node);
			}
		}

		protected virtual void ProcessStaticallyTypedAssignment(BinaryExpression node)
		{
			base.ProcessAssignment(node);
		}

		protected override bool ShouldRebindMember(IEntity entity)
		{
			return entity == null || base.TypeSystemServices.IsQuackBuiltin(entity);
		}

		protected override void NamedArgumentNotFound(IType type, ReferenceExpression name)
		{
			if (!base.TypeSystemServices.IsDuckType(type))
			{
				base.NamedArgumentNotFound(type, name);
			}
			else
			{
				BindQuack(name);
			}
		}

		protected override void AddResolvedNamedArgumentToEval(MethodInvocationExpression eval, ExpressionPair pair, ReferenceExpression instance)
		{
			if (!base.TypeSystemServices.IsQuackBuiltin(pair.First))
			{
				base.AddResolvedNamedArgumentToEval(eval, pair, instance);
				return;
			}
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(pair.First.LexicalInfo, instance.CloneNode(), ((ReferenceExpression)pair.First).Name);
			BindQuack(memberReferenceExpression);
			eval.Arguments.Add(base.CodeBuilder.CreateAssignment(pair.First.LexicalInfo, memberReferenceExpression, pair.Second));
		}

		protected override void MemberNotFound(MemberReferenceExpression node, INamespace ns)
		{
			if (IsDuckTyped(node.Target))
			{
				BindQuack(node);
			}
			else
			{
				base.MemberNotFound(node, ns);
			}
		}

		protected virtual bool IsDuckTyped(Expression e)
		{
			return base.TypeSystemServices.IsDuckTyped(e);
		}

		protected void BindQuack(Expression node)
		{
			Bind(node, BuiltinFunction.Quack);
			BindDuck(node);
		}

		protected void BindDuck(Expression node)
		{
			BindExpressionType(node, base.TypeSystemServices.DuckType);
		}

		protected override bool ProcessMethodInvocationWithInvalidParameters(MethodInvocationExpression node, IMethod targetMethod)
		{
			if (!base.TypeSystemServices.IsSystemObject(targetMethod.DeclaringType))
			{
				return false;
			}
			MemberReferenceExpression memberReferenceExpression = node.Target as MemberReferenceExpression;
			if (null == memberReferenceExpression)
			{
				return false;
			}
			if (!IsDuckTyped(memberReferenceExpression.Target))
			{
				return false;
			}
			BindQuack(node.Target);
			BindDuck(node);
			return true;
		}

		protected override void ProcessInvocationOnUnknownCallableExpression(MethodInvocationExpression node)
		{
			if (IsDuckTyped(node.Target))
			{
				BindDuck(node);
			}
			else
			{
				base.ProcessInvocationOnUnknownCallableExpression(node);
			}
		}

		public override void LeaveSlicingExpression(SlicingExpression node)
		{
			if (IsDuckTyped(node.Target) && !HasDefaultMember(node.Target))
			{
				BindDuck(node);
			}
			else
			{
				base.LeaveSlicingExpression(node);
			}
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			if (IsDuckTyped(node.Operand) && node.Operator == UnaryOperatorType.UnaryNegation)
			{
				BindDuck(node);
			}
			else
			{
				base.LeaveUnaryExpression(node);
			}
		}

		protected override bool ResolveRuntimeOperator(BinaryExpression node, string operatorName, MethodInvocationExpression mie)
		{
			if ((IsDuckTyped(node.Left) || IsDuckTyped(node.Right)) && (AstUtil.IsOverloadableOperator(node.Operator) || BinaryOperatorType.Or == node.Operator || BinaryOperatorType.And == node.Operator))
			{
				BindDuck(node);
				return true;
			}
			return base.ResolveRuntimeOperator(node, operatorName, mie);
		}

		protected override void CheckBuiltinUsage(ReferenceExpression node, IEntity entity)
		{
			if (!base.TypeSystemServices.IsQuackBuiltin(entity))
			{
				base.CheckBuiltinUsage(node, entity);
			}
		}

		private bool HasDefaultMember(Expression expression)
		{
			IType expressionType = GetExpressionType(expression);
			return expressionType != null && null != base.TypeSystemServices.GetDefaultMember(expressionType);
		}
	}
}
