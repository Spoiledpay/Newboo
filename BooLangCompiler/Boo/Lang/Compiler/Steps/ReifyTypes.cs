using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	public class ReifyTypes : AbstractVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		private IMethod _currentMethod;

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			TryToReify(node.Right, GetExpressionType(node.Left));
		}

		public override void LeaveCastExpression(CastExpression node)
		{
			TryToReify(node.Target, GetExpressionType(node));
		}

		public override void LeaveTryCastExpression(TryCastExpression node)
		{
			TryToReify(node.Target, GetExpressionType(node));
		}

		public override bool EnterMethod(Method node)
		{
			_currentMethod = GetEntity(node);
			return true;
		}

		public override void OnBlockExpression(BlockExpression node)
		{
		}

		public override void LeaveReturnStatement(ReturnStatement node)
		{
			if (node.Expression != null)
			{
				TryToReify(node.Expression, _currentMethod.ReturnType);
			}
		}

		public override void LeaveYieldStatement(YieldStatement node)
		{
			if (node.Expression != null)
			{
				TryToReify(node.Expression, GeneratorItemTypeFrom(_currentMethod.ReturnType) ?? base.TypeSystemServices.ObjectArrayType);
			}
		}

		public override void LeaveMethodInvocationExpression(MethodInvocationExpression node)
		{
			IEntityWithParameters entityWithParameters = node.Target.Entity as IEntityWithParameters;
			if (entityWithParameters == null)
			{
				return;
			}
			IParameter[] parameters = entityWithParameters.GetParameters();
			if (IsVarArgsInvocation(node, entityWithParameters))
			{
				int num = parameters.Length - 1;
				for (int i = 0; i < num; i++)
				{
					TryToReify(node.Arguments[i], parameters[i].Type);
				}
				IType elementType = parameters[num].Type.ElementType;
				for (int i = num; i < node.Arguments.Count; i++)
				{
					TryToReify(node.Arguments[i], elementType);
				}
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					TryToReify(node.Arguments[i], parameters[i].Type);
				}
			}
		}

		private static bool IsVarArgsInvocation(MethodInvocationExpression node, IEntityWithParameters entityWithParameters)
		{
			return entityWithParameters.AcceptVarArgs && !AstUtil.InvocationEndsWithExplodeExpression(node);
		}

		private void TryToReify(Expression candidate, IType expectedType)
		{
			if (IsEmptyArrayLiteral(candidate))
			{
				ReifyArrayLiteralType(ArrayTypeFor(expectedType), candidate);
			}
			else if (candidate.NodeType == NodeType.IntegerLiteralExpression && base.TypeSystemServices.IsIntegerNumber(expectedType))
			{
				BindExpressionType(candidate, expectedType);
			}
		}

		private IArrayType ArrayTypeFor(IType expectedType)
		{
			IArrayType arrayType = expectedType as IArrayType;
			if (arrayType != null)
			{
				return arrayType;
			}
			IType type = GeneratorItemTypeFrom(expectedType);
			if (type != null)
			{
				return type.MakeArrayType(1);
			}
			return base.TypeSystemServices.ObjectArrayType;
		}

		private IType GeneratorItemTypeFrom(IType expectedType)
		{
			return base.TypeSystemServices.IsGenericGeneratorReturnType(expectedType) ? expectedType.ConstructedInfo.GenericArguments[0] : null;
		}

		private void ReifyArrayLiteralType(IArrayType expectedArrayType, Expression array)
		{
			UnaryExpression unaryExpression = array as UnaryExpression;
			if (unaryExpression != null)
			{
				ReifyExplodeExpression(expectedArrayType, unaryExpression);
			}
			else
			{
				ReifyArrayLiteralExpression(expectedArrayType, (ArrayLiteralExpression)array);
			}
			BindExpressionType(array, expectedArrayType);
		}

		private void ReifyExplodeExpression(IArrayType expectedArrayType, UnaryExpression explodeExpression)
		{
			if (explodeExpression.Operator != UnaryOperatorType.Explode)
			{
				throw new InvalidOperationException();
			}
			ReifyArrayLiteralType(expectedArrayType, explodeExpression.Operand);
		}

		private void ReifyArrayLiteralExpression(IArrayType expectedArrayType, ArrayLiteralExpression arrayLiteralExpression)
		{
			arrayLiteralExpression.Type = (ArrayTypeReference)base.CodeBuilder.CreateTypeReference(expectedArrayType);
		}

		private static bool IsEmptyArrayLiteral(Expression e)
		{
			return e.ExpressionType == EmptyArrayType.Default;
		}

		public TypeMember Reify(TypeMember member)
		{
			Visit(member);
			return member;
		}
	}
}
