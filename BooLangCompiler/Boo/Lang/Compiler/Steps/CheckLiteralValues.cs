using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckLiteralValues : AbstractFastVisitorCompilerStep
	{
		private bool _checked;

		public override void OnModule(Module node)
		{
			Visit(node.Members);
		}

		public override void OnBlock(Block block)
		{
			bool @checked = _checked;
			_checked = AstAnnotations.IsChecked(block, base.Parameters.Checked);
			Visit(block.Statements);
			_checked = @checked;
		}

		public override void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			if (!_checked)
			{
				return;
			}
			base.OnArrayLiteralExpression(node);
			IType elementType = GetExpressionType(node).ElementType;
			if (!base.TypeSystemServices.IsPrimitiveNumber(elementType))
			{
				return;
			}
			foreach (Expression item in node.Items)
			{
				IntegerLiteralExpression integerLiteralExpression = item as IntegerLiteralExpression;
				if (integerLiteralExpression != null)
				{
					AssertLiteralInRange(integerLiteralExpression, elementType);
				}
			}
		}

		public override void OnBinaryExpression(BinaryExpression node)
		{
			if (!_checked)
			{
				return;
			}
			base.OnBinaryExpression(node);
			if (node.Operator == BinaryOperatorType.Assign && node.Right.NodeType == NodeType.IntegerLiteralExpression)
			{
				IType expressionType = GetExpressionType(node.Left);
				if (base.TypeSystemServices.IsPrimitiveNumber(expressionType))
				{
					AssertLiteralInRange((IntegerLiteralExpression)node.Right, expressionType);
				}
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (!_checked)
			{
				return;
			}
			base.OnMethodInvocationExpression(node);
			if (0 == node.Arguments.Count)
			{
				return;
			}
			IMethod method = node.Target.Entity as IMethod;
			if (null == method)
			{
				return;
			}
			IParameter[] parameters = method.GetParameters();
			if (parameters.Length != node.Arguments.Count)
			{
				return;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if (node.Arguments[i].NodeType == NodeType.IntegerLiteralExpression && base.TypeSystemServices.IsPrimitiveNumber(parameters[i].Type))
				{
					AssertLiteralInRange((IntegerLiteralExpression)node.Arguments[i], parameters[i].Type);
				}
			}
		}

		public override void OnExpressionStatement(ExpressionStatement node)
		{
			if (_checked)
			{
				base.OnExpressionStatement(node);
				IntegerLiteralExpression integerLiteralExpression = node.Expression as IntegerLiteralExpression;
				if (null != integerLiteralExpression)
				{
					AssertLiteralInRange(integerLiteralExpression, GetExpressionType(integerLiteralExpression));
				}
			}
		}

		private void AssertLiteralInRange(IntegerLiteralExpression literal, IType type)
		{
			bool flag = true;
			if (type == base.TypeSystemServices.ByteType)
			{
				flag = literal.Value >= 0 && literal.Value <= 255;
			}
			else if (type == base.TypeSystemServices.SByteType)
			{
				flag = literal.Value >= -128 && literal.Value <= 127;
			}
			else if (type == base.TypeSystemServices.ShortType)
			{
				flag = literal.Value >= -32768 && literal.Value <= 32767;
			}
			else if (type == base.TypeSystemServices.UShortType)
			{
				flag = literal.Value >= 0 && literal.Value <= 65535;
			}
			else if (type == base.TypeSystemServices.IntType)
			{
				flag = literal.Value >= int.MinValue && literal.Value <= int.MaxValue;
			}
			else if (type == base.TypeSystemServices.UIntType)
			{
				flag = literal.Value >= 0 && literal.Value <= uint.MaxValue;
			}
			else if (type == base.TypeSystemServices.LongType)
			{
				flag = literal.Value >= long.MinValue && literal.Value <= long.MaxValue;
			}
			if (!flag)
			{
				Error(CompilerErrorFactory.ConstantCannotBeConverted(literal, type));
			}
		}
	}
}
