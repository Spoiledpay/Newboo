using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class RemoveDeadCode : AbstractTransformerCompilerStep
	{
		public override bool EnterRaiseStatement(RaiseStatement node)
		{
			RemoveUnreachableCode(node);
			return false;
		}

		public override void OnReturnStatement(ReturnStatement node)
		{
			RemoveUnreachableCode(node);
		}

		public override bool EnterBreakStatement(BreakStatement node)
		{
			RemoveUnreachableCode(node);
			return false;
		}

		public override bool EnterContinueStatement(ContinueStatement node)
		{
			RemoveUnreachableCode(node);
			return false;
		}

		public override bool EnterMethodInvocationExpression(MethodInvocationExpression node)
		{
			return false;
		}

		private void RemoveUnreachableCode(Statement node)
		{
			Block block = node.ParentNode as Block;
			if (null != block)
			{
				int num = DetectUnreachableCode(block, node);
				if (-1 != num)
				{
					RemoveStatements(block, num);
				}
			}
		}

		private int DetectUnreachableCode(Block block, Statement limit)
		{
			bool flag = false;
			int num = 0;
			foreach (Statement statement in block.Statements)
			{
				if (IsSwitchBuiltin(statement))
				{
					return -1;
				}
				if (flag && statement is LabelStatement)
				{
					return -1;
				}
				if (statement == limit)
				{
					flag = true;
				}
				else if (flag)
				{
					if (!statement.IsSynthetic)
					{
						base.Warnings.Add(CompilerWarningFactory.UnreachableCodeDetected(statement));
					}
					return num;
				}
				num++;
			}
			return -1;
		}

		private static bool IsSwitchBuiltin(Statement stmt)
		{
			ExpressionStatement expressionStatement = stmt as ExpressionStatement;
			if (expressionStatement != null)
			{
				MethodInvocationExpression methodInvocationExpression = expressionStatement.Expression as MethodInvocationExpression;
				if (methodInvocationExpression != null && BuiltinFunction.Switch == methodInvocationExpression.Target.Entity)
				{
					return true;
				}
			}
			return false;
		}

		private static void RemoveStatements(Block block, int fromIndex)
		{
			for (int num = block.Statements.Count - 1; num >= fromIndex; num--)
			{
				block.Statements.RemoveAt(num);
			}
		}

		public override void OnTryStatement(TryStatement node)
		{
			if (node.ProtectedBlock.IsEmpty)
			{
				if (node.EnsureBlock != null && !node.EnsureBlock.IsEmpty)
				{
					ReplaceCurrentNode(node.EnsureBlock);
				}
				else
				{
					RemoveCurrentNode();
				}
			}
			else
			{
				base.OnTryStatement(node);
			}
		}
	}
}
