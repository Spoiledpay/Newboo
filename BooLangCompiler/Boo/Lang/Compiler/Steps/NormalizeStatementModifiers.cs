using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	public class NormalizeStatementModifiers : AbstractTransformerCompilerStep, IStatementReifier, INodeReifier<Statement>, IExpressionReifier, INodeReifier<Expression>, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void Run()
		{
			Visit(base.CompileUnit.Modules);
		}

		public override void LeaveUnpackStatement(UnpackStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveExpressionStatement(ExpressionStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveRaiseStatement(RaiseStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveReturnStatement(ReturnStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveBreakStatement(BreakStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveContinueStatement(ContinueStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveGotoStatement(GotoStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveYieldStatement(YieldStatement node)
		{
			LeaveStatement(node);
		}

		public override void LeaveLabelStatement(LabelStatement node)
		{
			if (null != node.Modifier)
			{
				base.Warnings.Add(CompilerWarningFactory.ModifiersInLabelsHaveNoEffect(node.Modifier));
			}
		}

		public override void LeaveMacroStatement(MacroStatement node)
		{
			LeaveStatement(node);
		}

		public static Statement CreateModifiedStatement(StatementModifier modifier, Statement node)
		{
			Block block;
			Statement result = MapStatementModifier(modifier, out block);
			block.Add(node);
			return result;
		}

		public static Statement MapStatementModifier(StatementModifier modifier, out Block block)
		{
			switch (modifier.Type)
			{
			case StatementModifierType.If:
			{
				IfStatement ifStatement = new IfStatement(modifier.LexicalInfo);
				ifStatement.Condition = modifier.Condition;
				ifStatement.TrueBlock = new Block();
				block = ifStatement.TrueBlock;
				return ifStatement;
			}
			case StatementModifierType.Unless:
			{
				UnlessStatement unlessStatement = new UnlessStatement(modifier.LexicalInfo);
				unlessStatement.Condition = modifier.Condition;
				block = unlessStatement.Block;
				return unlessStatement;
			}
			case StatementModifierType.While:
			{
				WhileStatement whileStatement = new WhileStatement(modifier.LexicalInfo);
				whileStatement.Condition = modifier.Condition;
				block = whileStatement.Block;
				return whileStatement;
			}
			default:
				throw CompilerErrorFactory.NotImplemented(modifier, $"modifier {modifier.Type} supported");
			}
		}

		public void LeaveStatement(Statement node)
		{
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				node.Modifier = null;
				ReplaceCurrentNode(CreateModifiedStatement(modifier, node));
			}
		}

		public TypeMember Reify(TypeMember member)
		{
			return Visit(member);
		}

		public Statement Reify(Statement node)
		{
			return Visit(node);
		}

		public Expression Reify(Expression node)
		{
			return Visit(node);
		}
	}
}
