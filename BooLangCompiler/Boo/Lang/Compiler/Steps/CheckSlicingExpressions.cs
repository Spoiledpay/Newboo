using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckSlicingExpressions : AbstractFastVisitorCompilerStep
	{
		private EnvironmentProvision<TypeChecker> _typeChecker;

		private TypeChecker TypeChecker => _typeChecker.Instance;

		public override void OnSlicingExpression(SlicingExpression node)
		{
			base.OnSlicingExpression(node);
			IArrayType arrayType = GetExpressionType(node.Target) as IArrayType;
			if (arrayType != null && arrayType.Rank != node.Indices.Count)
			{
				Error(CompilerErrorFactory.InvalidArrayRank(node, node.Target.ToCodeString(), arrayType.Rank, node.Indices.Count));
			}
		}

		public override void OnSlice(Slice node)
		{
			base.OnSlice(node);
			if (!base.TypeSystemServices.IsDuckTyped((Expression)node.ParentNode))
			{
				AssertInt(node.Begin);
				AssertOptionalInt(node.End);
				if (node.Step != null)
				{
					CompilerErrorFactory.NotImplemented(node.Step, "slicing step");
				}
			}
		}

		private void AssertInt(Expression e)
		{
			AssertExpressionTypeIsCompatibleWith(base.TypeSystemServices.IntType, e);
		}

		private void AssertExpressionTypeIsCompatibleWith(IType expectedType, Expression e)
		{
			TypeChecker.AssertTypeCompatibility(e, expectedType, GetExpressionType(e));
		}

		private void AssertOptionalInt(Expression e)
		{
			if (IsNotOmitted(e))
			{
				AssertInt(e);
			}
		}

		private static bool IsNotOmitted(Expression e)
		{
			return e != null && e != OmittedExpression.Default;
		}

		public override void Dispose()
		{
			_typeChecker = default(EnvironmentProvision<TypeChecker>);
			base.Dispose();
		}
	}
}
