using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps.MacroProcessing;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	public class MacroAndAttributeExpansion : AbstractCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>, IStatementReifier, INodeReifier<Statement>, IExpressionReifier, INodeReifier<Expression>
	{
		private BindAndApplyAttributes _attributes = new BindAndApplyAttributes();

		private MacroExpander _macroExpander = new MacroExpander();

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_attributes.Initialize(context);
			_macroExpander.Initialize(context);
		}

		public override void Run()
		{
			RunExpansionIterations();
		}

		private void RunExpansionIterations()
		{
			int num = 0;
			do
			{
				bool flag = true;
				if (!ApplyAttributesAndExpandMacros() && !BubbleResultingTypeMemberStatementsUp())
				{
					return;
				}
				num++;
			}
			while (num <= base.Parameters.MaxExpansionIterations);
			throw new CompilerError("Too many expansions.");
		}

		private bool BubbleResultingTypeMemberStatementsUp()
		{
			return TypeMemberStatementBubbler.BubbleTypeMemberStatementsUp(base.CompileUnit);
		}

		private bool ApplyAttributesAndExpandMacros()
		{
			bool flag = _attributes.BindAndApply();
			bool flag2 = _macroExpander.ExpandAll();
			return flag || flag2;
		}

		public TypeMember Reify(TypeMember node)
		{
			ApplyAttributesAndExpandMacros();
			return node;
		}

		public Statement Reify(Statement node)
		{
			Statement statement = node;
			if (node is MacroStatement)
			{
				Node parentNode = node.ParentNode;
				statement = new Block(node);
				parentNode.Replace(node, statement);
			}
			ApplyAttributesAndExpandMacros();
			return statement;
		}

		public Expression Reify(Expression node)
		{
			ApplyAttributesAndExpandMacros();
			return node;
		}
	}
}
