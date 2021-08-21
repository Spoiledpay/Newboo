using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class ConstantFolding : AbstractTransformerCompilerStep
	{
		public const string FoldedExpression = "foldedExpression";

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void OnModule(Module node)
		{
			Visit(node.Members);
		}

		private object GetLiteralValue(Expression node)
		{
			switch (node.NodeType)
			{
			case NodeType.CastExpression:
				return GetLiteralValue(((CastExpression)node).Target);
			case NodeType.BoolLiteralExpression:
				return ((BoolLiteralExpression)node).Value;
			case NodeType.IntegerLiteralExpression:
				return ((IntegerLiteralExpression)node).Value;
			case NodeType.DoubleLiteralExpression:
				return ((DoubleLiteralExpression)node).Value;
			case NodeType.MemberReferenceExpression:
			{
				IField field = node.Entity as IField;
				if (field != null && field.IsLiteral)
				{
					if (!field.Type.IsEnum)
					{
						Expression expression = field.StaticValue as Expression;
						return (expression != null) ? GetLiteralValue(expression) : field.StaticValue;
					}
					object staticValue = field.StaticValue;
					if (staticValue != null && staticValue != Error.Default)
					{
						return staticValue;
					}
				}
				break;
			}
			}
			return null;
		}

		public override void LeaveEnumMember(EnumMember node)
		{
			if (node.Initializer.NodeType == NodeType.IntegerLiteralExpression)
			{
				return;
			}
			IType expressionType = node.Initializer.ExpressionType;
			if (expressionType != null && (base.TypeSystemServices.IsIntegerNumber(expressionType) || expressionType.IsEnum))
			{
				object literalValue = GetLiteralValue(node.Initializer);
				if (literalValue != null && literalValue != Error.Default)
				{
					node.Initializer = new IntegerLiteralExpression(Convert.ToInt64(literalValue));
				}
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (AstUtil.GetBinaryOperatorKind(node.Operator) == BinaryOperatorKind.Assignment || node.Operator == BinaryOperatorType.ReferenceEquality || node.Operator == BinaryOperatorType.ReferenceInequality || node.Left.ExpressionType == null || null == node.Right.ExpressionType)
			{
				return;
			}
			object literalValue = GetLiteralValue(node.Left);
			object literalValue2 = GetLiteralValue(node.Right);
			if (literalValue == null || null == literalValue2)
			{
				return;
			}
			Expression expression = null;
			IType expressionType = GetExpressionType(node.Left);
			IType expressionType2 = GetExpressionType(node.Right);
			if (base.TypeSystemServices.BoolType == expressionType && base.TypeSystemServices.BoolType == expressionType2)
			{
				expression = GetFoldedBoolLiteral(node.Operator, Convert.ToBoolean(literalValue), Convert.ToBoolean(literalValue2));
			}
			else if (base.TypeSystemServices.IsFloatingPointNumber(expressionType) || base.TypeSystemServices.IsFloatingPointNumber(expressionType2))
			{
				expression = GetFoldedDoubleLiteral(node.Operator, Convert.ToDouble(literalValue), Convert.ToDouble(literalValue2));
			}
			else if (base.TypeSystemServices.IsIntegerNumber(expressionType) || expressionType.IsEnum)
			{
				bool flag = base.TypeSystemServices.IsSignedNumber(expressionType);
				bool flag2 = base.TypeSystemServices.IsSignedNumber(expressionType2);
				if (flag == flag2)
				{
					expression = (flag ? GetFoldedIntegerLiteral(node.Operator, Convert.ToInt64(literalValue), Convert.ToInt64(literalValue2)) : GetFoldedIntegerLiteral(node.Operator, Convert.ToUInt64(literalValue), Convert.ToUInt64(literalValue2)));
				}
			}
			if (null != expression)
			{
				expression.LexicalInfo = node.LexicalInfo;
				expression.ExpressionType = GetExpressionType(node);
				expression.Annotate("foldedExpression", node);
				ReplaceCurrentNode(expression);
			}
		}

		public override void LeaveUnaryExpression(UnaryExpression node)
		{
			if (node.Operator == UnaryOperatorType.Explode || node.Operator == UnaryOperatorType.AddressOf || node.Operator == UnaryOperatorType.Indirection || node.Operator == UnaryOperatorType.LogicalNot || null == node.Operand.ExpressionType)
			{
				return;
			}
			object literalValue = GetLiteralValue(node.Operand);
			if (null != literalValue)
			{
				Expression expression = null;
				IType expressionType = GetExpressionType(node.Operand);
				if (base.TypeSystemServices.IsFloatingPointNumber(expressionType))
				{
					expression = GetFoldedDoubleLiteral(node.Operator, Convert.ToDouble(literalValue));
				}
				else if (base.TypeSystemServices.IsIntegerNumber(expressionType) || expressionType.IsEnum)
				{
					expression = GetFoldedIntegerLiteral(node.Operator, Convert.ToInt64(literalValue));
				}
				if (null != expression)
				{
					expression.LexicalInfo = node.LexicalInfo;
					expression.ExpressionType = GetExpressionType(node);
					ReplaceCurrentNode(expression);
				}
			}
		}

		private static BoolLiteralExpression GetFoldedBoolLiteral(BinaryOperatorType @operator, bool lhs, bool rhs)
		{
			bool value;
			switch (@operator)
			{
			case BinaryOperatorType.Equality:
				value = lhs == rhs;
				break;
			case BinaryOperatorType.Inequality:
				value = lhs != rhs;
				break;
			case BinaryOperatorType.BitwiseOr:
				value = lhs || rhs;
				break;
			case BinaryOperatorType.BitwiseAnd:
				value = lhs && rhs;
				break;
			case BinaryOperatorType.ExclusiveOr:
				value = lhs ^ rhs;
				break;
			case BinaryOperatorType.And:
				value = lhs && rhs;
				break;
			case BinaryOperatorType.Or:
				value = lhs || rhs;
				break;
			default:
				return null;
			}
			return new BoolLiteralExpression(value);
		}

		private static LiteralExpression GetFoldedIntegerLiteral(BinaryOperatorType @operator, long lhs, long rhs)
		{
			long value;
			switch (@operator)
			{
			case BinaryOperatorType.Addition:
				value = lhs + rhs;
				break;
			case BinaryOperatorType.Subtraction:
				value = lhs - rhs;
				break;
			case BinaryOperatorType.Multiply:
				value = lhs * rhs;
				break;
			case BinaryOperatorType.Division:
				value = lhs / rhs;
				break;
			case BinaryOperatorType.Modulus:
				value = lhs % rhs;
				break;
			case BinaryOperatorType.Exponentiation:
				value = (long)Math.Pow(lhs, rhs);
				break;
			case BinaryOperatorType.BitwiseOr:
				value = lhs | rhs;
				break;
			case BinaryOperatorType.BitwiseAnd:
				value = lhs & rhs;
				break;
			case BinaryOperatorType.ExclusiveOr:
				value = lhs ^ rhs;
				break;
			case BinaryOperatorType.ShiftLeft:
				value = lhs << (int)rhs;
				break;
			case BinaryOperatorType.ShiftRight:
				value = lhs >> (int)rhs;
				break;
			case BinaryOperatorType.LessThan:
				return new BoolLiteralExpression(lhs < rhs);
			case BinaryOperatorType.LessThanOrEqual:
				return new BoolLiteralExpression(lhs <= rhs);
			case BinaryOperatorType.GreaterThan:
				return new BoolLiteralExpression(lhs > rhs);
			case BinaryOperatorType.GreaterThanOrEqual:
				return new BoolLiteralExpression(lhs >= rhs);
			case BinaryOperatorType.Equality:
				return new BoolLiteralExpression(lhs == rhs);
			case BinaryOperatorType.Inequality:
				return new BoolLiteralExpression(lhs != rhs);
			default:
				return null;
			}
			return new IntegerLiteralExpression(value);
		}

		private static LiteralExpression GetFoldedIntegerLiteral(BinaryOperatorType @operator, ulong lhs, ulong rhs)
		{
			ulong value;
			switch (@operator)
			{
			case BinaryOperatorType.Addition:
				value = lhs + rhs;
				break;
			case BinaryOperatorType.Subtraction:
				value = lhs - rhs;
				break;
			case BinaryOperatorType.Multiply:
				value = lhs * rhs;
				break;
			case BinaryOperatorType.Division:
				value = lhs / rhs;
				break;
			case BinaryOperatorType.Modulus:
				value = lhs % rhs;
				break;
			case BinaryOperatorType.Exponentiation:
				value = (ulong)Math.Pow(lhs, rhs);
				break;
			case BinaryOperatorType.BitwiseOr:
				value = lhs | rhs;
				break;
			case BinaryOperatorType.BitwiseAnd:
				value = lhs & rhs;
				break;
			case BinaryOperatorType.ExclusiveOr:
				value = lhs ^ rhs;
				break;
			case BinaryOperatorType.ShiftLeft:
				value = lhs << (int)rhs;
				break;
			case BinaryOperatorType.ShiftRight:
				value = lhs >> (int)rhs;
				break;
			case BinaryOperatorType.LessThan:
				return new BoolLiteralExpression(lhs < rhs);
			case BinaryOperatorType.LessThanOrEqual:
				return new BoolLiteralExpression(lhs <= rhs);
			case BinaryOperatorType.GreaterThan:
				return new BoolLiteralExpression(lhs > rhs);
			case BinaryOperatorType.GreaterThanOrEqual:
				return new BoolLiteralExpression(lhs >= rhs);
			case BinaryOperatorType.Equality:
				return new BoolLiteralExpression(lhs == rhs);
			case BinaryOperatorType.Inequality:
				return new BoolLiteralExpression(lhs != rhs);
			default:
				return null;
			}
			return new IntegerLiteralExpression((long)value);
		}

		private static LiteralExpression GetFoldedDoubleLiteral(BinaryOperatorType @operator, double lhs, double rhs)
		{
			double value;
			switch (@operator)
			{
			case BinaryOperatorType.Addition:
				value = lhs + rhs;
				break;
			case BinaryOperatorType.Subtraction:
				value = lhs - rhs;
				break;
			case BinaryOperatorType.Multiply:
				value = lhs * rhs;
				break;
			case BinaryOperatorType.Division:
				value = lhs / rhs;
				break;
			case BinaryOperatorType.Modulus:
				value = lhs % rhs;
				break;
			case BinaryOperatorType.Exponentiation:
				value = Math.Pow(lhs, rhs);
				break;
			case BinaryOperatorType.LessThan:
				return new BoolLiteralExpression(lhs < rhs);
			case BinaryOperatorType.LessThanOrEqual:
				return new BoolLiteralExpression(lhs <= rhs);
			case BinaryOperatorType.GreaterThan:
				return new BoolLiteralExpression(lhs > rhs);
			case BinaryOperatorType.GreaterThanOrEqual:
				return new BoolLiteralExpression(lhs >= rhs);
			case BinaryOperatorType.Equality:
				return new BoolLiteralExpression(lhs == rhs);
			case BinaryOperatorType.Inequality:
				return new BoolLiteralExpression(lhs != rhs);
			default:
				return null;
			}
			return new DoubleLiteralExpression(value);
		}

		private static LiteralExpression GetFoldedIntegerLiteral(UnaryOperatorType @operator, long operand)
		{
			long value;
			switch (@operator)
			{
			case UnaryOperatorType.UnaryNegation:
				value = -operand;
				break;
			case UnaryOperatorType.OnesComplement:
				value = ~operand;
				break;
			default:
				return null;
			}
			return new IntegerLiteralExpression(value);
		}

		private static LiteralExpression GetFoldedDoubleLiteral(UnaryOperatorType @operator, double operand)
		{
			if (@operator == UnaryOperatorType.UnaryNegation)
			{
				double value = 0.0 - operand;
				return new DoubleLiteralExpression(value);
			}
			return null;
		}
	}
}
