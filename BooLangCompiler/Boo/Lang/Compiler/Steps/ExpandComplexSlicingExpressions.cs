using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class ExpandComplexSlicingExpressions : MethodTrackingVisitorCompilerStep
	{
		private EnvironmentProvision<RuntimeMethodCache> _methodCache;

		protected RuntimeMethodCache MethodCache => _methodCache.Instance;

		public override void OnSlicingExpression(SlicingExpression node)
		{
			base.OnSlicingExpression(node);
			if (node.IsComplexSlicing() && !node.IsTargetOfAssignment())
			{
				CompleteOmittedExpressions(node);
				ExpandComplexSlicing(node);
			}
		}

		private void CompleteOmittedExpressions(SlicingExpression node)
		{
			foreach (Slice item in node.Indices.Where((Slice slice) => slice.Begin == OmittedExpression.Default))
			{
				item.Begin = base.CodeBuilder.CreateIntegerLiteral(0);
			}
		}

		private void ExpandComplexSlicing(SlicingExpression node)
		{
			IType expressionType = GetExpressionType(node.Target);
			if (IsString(expressionType))
			{
				ExpandComplexStringSlicing(node);
			}
			else if (IsList(expressionType))
			{
				ExpandComplexListSlicing(node);
			}
			else if (expressionType.IsArray)
			{
				ExpandComplexArraySlicing(node);
			}
			else
			{
				NotImplemented(node, "complex slicing for anything but lists, arrays and strings");
			}
		}

		private bool IsString(IType targetType)
		{
			return base.TypeSystemServices.StringType == targetType;
		}

		private bool IsList(IType targetType)
		{
			return IsAssignableFrom(base.TypeSystemServices.ListType, targetType);
		}

		private void ExpandComplexListSlicing(SlicingExpression node)
		{
			Slice slice = node.Indices[0];
			MethodInvocationExpression methodInvocationExpression;
			if (IsNullOrOmitted(slice.End))
			{
				methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(node.Target, MethodCache.List_GetRange1);
				methodInvocationExpression.Arguments.Add(slice.Begin);
			}
			else
			{
				methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(node.Target, MethodCache.List_GetRange2);
				methodInvocationExpression.Arguments.Add(slice.Begin);
				methodInvocationExpression.Arguments.Add(slice.End);
			}
			node.ParentNode.Replace(node, methodInvocationExpression);
		}

		private void ExpandComplexArraySlicing(SlicingExpression node)
		{
			if (node.Indices.Count > 1)
			{
				MethodInvocationExpression methodInvocationExpression = null;
				ArrayLiteralExpression arrayLiteralExpression = new ArrayLiteralExpression();
				ArrayLiteralExpression arrayLiteralExpression2 = new ArrayLiteralExpression();
				ArrayLiteralExpression arrayLiteralExpression3 = new ArrayLiteralExpression();
				for (int i = 0; i < node.Indices.Count; i++)
				{
					arrayLiteralExpression3.Items.Add(node.Indices[i].Begin);
					if (node.Indices[i].End == null)
					{
						BinaryExpression binaryExpression = new BinaryExpression(BinaryOperatorType.Addition, node.Indices[i].Begin, new IntegerLiteralExpression(1L));
						arrayLiteralExpression3.Items.Add(binaryExpression);
						BindExpressionType(binaryExpression, GetExpressionType(node.Indices[i].Begin));
						arrayLiteralExpression.Items.Add(new BoolLiteralExpression(value: false));
						arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: true));
					}
					else if (node.Indices[i].End == OmittedExpression.Default)
					{
						IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression(0L);
						arrayLiteralExpression3.Items.Add(integerLiteralExpression);
						BindExpressionType(integerLiteralExpression, GetExpressionType(node.Indices[i].Begin));
						arrayLiteralExpression.Items.Add(new BoolLiteralExpression(value: true));
						arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: false));
					}
					else
					{
						arrayLiteralExpression3.Items.Add(node.Indices[i].End);
						arrayLiteralExpression.Items.Add(new BoolLiteralExpression(value: false));
						arrayLiteralExpression2.Items.Add(new BoolLiteralExpression(value: false));
					}
				}
				methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_GetMultiDimensionalRange1, node.Target, arrayLiteralExpression3);
				methodInvocationExpression.Arguments.Add(arrayLiteralExpression);
				methodInvocationExpression.Arguments.Add(arrayLiteralExpression2);
				BindExpressionType(arrayLiteralExpression3, base.TypeSystemServices.Map(typeof(int[])));
				BindExpressionType(arrayLiteralExpression, base.TypeSystemServices.Map(typeof(bool[])));
				BindExpressionType(arrayLiteralExpression2, base.TypeSystemServices.Map(typeof(bool[])));
				node.ParentNode.Replace(node, methodInvocationExpression);
			}
			else
			{
				Slice slice = node.Indices[0];
				MethodInvocationExpression methodInvocationExpression = (IsNullOrOmitted(slice.End) ? base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_GetRange1, node.Target, slice.Begin) : base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_GetRange2, node.Target, slice.Begin, slice.End));
				node.ParentNode.Replace(node, methodInvocationExpression);
			}
		}

		private static bool IsNullOrOmitted(Expression expression)
		{
			return expression == null || expression == OmittedExpression.Default;
		}

		private static bool NeedsNormalization(Expression index)
		{
			return index.NodeType != NodeType.IntegerLiteralExpression || ((IntegerLiteralExpression)index).Value < 0;
		}

		private void ExpandComplexStringSlicing(SlicingExpression node)
		{
			node.ParentNode.Replace(node, ComplexStringSlicingExpressionFor(node));
		}

		private MethodInvocationExpression ComplexStringSlicingExpressionFor(SlicingExpression node)
		{
			Slice slice = node.Indices[0];
			if (IsNullOrOmitted(slice.End))
			{
				if (NeedsNormalization(slice.Begin))
				{
					MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo);
					methodInvocationExpression.ExpressionType = base.TypeSystemServices.StringType;
					InternalLocal local = DeclareTempLocal(base.TypeSystemServices.StringType);
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(local), node.Target));
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(local), MethodCache.String_Substring_Int, base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_NormalizeStringIndex, base.CodeBuilder.CreateReference(local), slice.Begin)));
					return methodInvocationExpression;
				}
				return base.CodeBuilder.CreateMethodInvocation(node.Target, MethodCache.String_Substring_Int, slice.Begin);
			}
			return base.CodeBuilder.CreateMethodInvocation(MethodCache.RuntimeServices_Mid, node.Target, slice.Begin, slice.End);
		}

		private static bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		protected InternalLocal DeclareTempLocal(IType localType)
		{
			return base.CodeBuilder.DeclareTempLocal(base.CurrentMethod, localType);
		}

		public override void Dispose()
		{
			_methodCache = default(EnvironmentProvision<RuntimeMethodCache>);
			base.Dispose();
		}
	}
}
