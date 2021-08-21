using System.Linq;

namespace Boo.Lang.Compiler.Ast
{
	public static class AstNodePredicates
	{
		public static bool IsComplexSlicing(this SlicingExpression node)
		{
			return node.Indices.Any(IsComplexSlice);
		}

		public static bool IsTargetOfAssignment(this Expression node)
		{
			BinaryExpression binaryExpression = node.ParentNode as BinaryExpression;
			if (binaryExpression == null)
			{
				return false;
			}
			return node == binaryExpression.Left && AstUtil.IsAssignment(binaryExpression);
		}

		public static bool IsComplexSlice(Slice slice)
		{
			return slice.End != null || slice.Step != null || slice.Begin == OmittedExpression.Default;
		}
	}
}
