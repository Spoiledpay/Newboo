using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class InjectImplicitBooleanConversions : AbstractNamespaceSensitiveVisitorCompilerStep
	{
		private IMethod _String_IsNullOrEmpty;

		private Method _currentMethod;

		private IMethod String_IsNullOrEmpty
		{
			get
			{
				if (_String_IsNullOrEmpty != null)
				{
					return _String_IsNullOrEmpty;
				}
				return _String_IsNullOrEmpty = base.TypeSystemServices.Map(Methods.Of<string, bool>(string.IsNullOrEmpty));
			}
		}

		public override void Dispose()
		{
			_String_IsNullOrEmpty = null;
			_currentMethod = null;
			base.Dispose();
		}

		public override void OnMethod(Method node)
		{
			_currentMethod = node;
			Visit(node.Body);
		}

		public override void OnConstructor(Constructor node)
		{
			OnMethod(node);
		}

		public override void LeaveUnlessStatement(UnlessStatement node)
		{
			node.Condition = AssertBoolContext(node.Condition);
		}

		public override void LeaveIfStatement(IfStatement node)
		{
			node.Condition = AssertBoolContext(node.Condition);
		}

		public override void LeaveConditionalExpression(ConditionalExpression node)
		{
			node.Condition = AssertBoolContext(node.Condition);
		}

		public override void LeaveWhileStatement(WhileStatement node)
		{
			node.Condition = AssertBoolContext(node.Condition);
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			UnaryOperatorType @operator = node.Operator;
			if (@operator == UnaryOperatorType.LogicalNot)
			{
				node.Operand = AssertBoolContext(node.Operand);
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			switch (node.Operator)
			{
			case BinaryOperatorType.Or:
			case BinaryOperatorType.And:
				BindLogicalOperator(node);
				break;
			}
		}

		private void BindLogicalOperator(BinaryExpression node)
		{
			if (IsLogicalCondition(node))
			{
				BindLogicalOperatorCondition(node);
			}
			else
			{
				BindLogicalOperatorExpression(node);
			}
		}

		public static bool IsLogicalCondition(Expression node)
		{
			Node node2 = node;
			while (IsLogicalExpression(node2.ParentNode))
			{
				node2 = node2.ParentNode;
			}
			return IsConditionOfConditionalStatement(node2);
		}

		private static bool IsConditionOfConditionalStatement(Node condition)
		{
			ConditionalStatement conditionalStatement = condition.ParentNode as ConditionalStatement;
			return conditionalStatement != null && conditionalStatement.Condition == condition;
		}

		private static bool IsLogicalExpression(Node node)
		{
			return node.NodeType switch
			{
				NodeType.BinaryExpression => AstUtil.GetBinaryOperatorKind((BinaryExpression)node) == BinaryOperatorKind.Logical, 
				NodeType.UnaryExpression => ((UnaryExpression)node).Operator == UnaryOperatorType.LogicalNot, 
				_ => false, 
			};
		}

		private void BindLogicalOperatorExpression(BinaryExpression node)
		{
			Expression expression = AssertBoolContext(node.Left);
			if (expression != node.Left)
			{
				InternalLocal local = DeclareTempLocal(GetExpressionType(node.Left));
				ReferenceExpression referenceExpression = base.CodeBuilder.CreateReference(local);
				Expression right = node.Right;
				ConditionalExpression conditionalExpression2;
				if (node.Operator != BinaryOperatorType.And)
				{
					ConditionalExpression conditionalExpression = new ConditionalExpression(node.LexicalInfo);
					conditionalExpression.Condition = expression;
					conditionalExpression.TrueValue = referenceExpression;
					conditionalExpression.FalseValue = right;
					conditionalExpression2 = conditionalExpression;
				}
				else
				{
					ConditionalExpression conditionalExpression3 = new ConditionalExpression(node.LexicalInfo);
					conditionalExpression3.Condition = expression;
					conditionalExpression3.TrueValue = right;
					conditionalExpression3.FalseValue = referenceExpression;
					conditionalExpression2 = conditionalExpression3;
				}
				ConditionalExpression conditionalExpression4 = conditionalExpression2;
				if (expression.ReplaceNodes((Node n) => n == node.Left, base.CodeBuilder.CreateAssignment(referenceExpression.CloneNode(), node.Left)) != 1)
				{
					throw new InvalidOperationException();
				}
				BindExpressionType(conditionalExpression4, GetMostGenericType(node));
				node.ParentNode.Replace(node, conditionalExpression4);
			}
		}

		private IType GetMostGenericType(BinaryExpression node)
		{
			return base.TypeSystemServices.GetMostGenericType(GetExpressionType(node.Left), GetExpressionType(node.Right));
		}

		protected InternalLocal DeclareTempLocal(IType localType)
		{
			return base.CodeBuilder.DeclareTempLocal(_currentMethod, localType);
		}

		private void BindLogicalOperatorCondition(BinaryExpression node)
		{
			node.Left = AssertBoolContext(node.Left);
			node.Right = AssertBoolContext(node.Right);
			BindExpressionType(node, GetMostGenericType(node));
		}

		private Expression AssertBoolContext(Expression expression)
		{
			IType expressionType = GetExpressionType(expression);
			if (base.TypeSystemServices.IsNumberOrBool(expressionType) || expressionType.IsEnum)
			{
				return expression;
			}
			IMethod method = base.TypeSystemServices.FindImplicitConversionOperator(expressionType, base.TypeSystemServices.BoolType);
			if (method != null)
			{
				return base.CodeBuilder.CreateMethodInvocation(method, expression);
			}
			if (TypeSystemServices.IsNullable(expressionType))
			{
				return base.CodeBuilder.CreateMethodInvocation(expression, base.NameResolutionService.ResolveMethod(expressionType, "get_HasValue"));
			}
			if (base.TypeSystemServices.StringType == expressionType)
			{
				UnaryExpression unaryExpression = new UnaryExpression(UnaryOperatorType.LogicalNot, base.CodeBuilder.CreateMethodInvocation(String_IsNullOrEmpty, expression));
				BindExpressionType(unaryExpression, base.TypeSystemServices.BoolType);
				return unaryExpression;
			}
			if (!expressionType.IsValueType)
			{
				return expression;
			}
			Error(CompilerErrorFactory.BoolExpressionRequired(expression, expressionType));
			return expression;
		}
	}
}
