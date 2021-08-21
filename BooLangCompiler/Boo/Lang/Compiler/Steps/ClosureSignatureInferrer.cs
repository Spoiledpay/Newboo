using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	internal class ClosureSignatureInferrer
	{
		private BlockExpression _closure;

		private IType[] _inputTypes;

		public BlockExpression Closure => _closure;

		public IType[] ParameterTypes => _inputTypes;

		protected BooCodeBuilder CodeBuilder => My<BooCodeBuilder>.Instance;

		public MethodInvocationExpression MethodInvocationContext
		{
			get
			{
				MethodInvocationExpression methodInvocationExpression = Closure.ParentNode as MethodInvocationExpression;
				if (methodInvocationExpression != null && methodInvocationExpression.Arguments.Contains(Closure))
				{
					return methodInvocationExpression;
				}
				return null;
			}
		}

		public ClosureSignatureInferrer(BlockExpression closure)
		{
			_closure = closure;
			InitializeInputTypes();
		}

		private void InitializeInputTypes()
		{
			_inputTypes = Array.ConvertAll(Closure.Parameters.ToArray(), (ParameterDeclaration pd) => (pd.Type == null) ? null : (pd.Type.Entity as IType));
		}

		public ICallableType InferCallableType()
		{
			return (GetTypeFromMethodInvocationContext() ?? GetTypeFromDeclarationContext() ?? GetTypeFromBinaryExpressionContext() ?? GetTypeFromCastContext()) as ICallableType;
		}

		private IType GetTypeFromBinaryExpressionContext()
		{
			BinaryExpression binaryExpression = Closure.ParentNode as BinaryExpression;
			if (binaryExpression == null || Closure != binaryExpression.Right)
			{
				return null;
			}
			return binaryExpression.Left.ExpressionType;
		}

		private IType GetTypeFromDeclarationContext()
		{
			TypeReference typeReference = null;
			DeclarationStatement declarationStatement = Closure.ParentNode as DeclarationStatement;
			if (declarationStatement != null)
			{
				typeReference = declarationStatement.Declaration.Type;
			}
			Field field = Closure.ParentNode as Field;
			if (field != null)
			{
				typeReference = field.Type;
			}
			if (typeReference != null)
			{
				return typeReference.Entity as IType;
			}
			return null;
		}

		private IType GetTypeFromMethodInvocationContext()
		{
			if (MethodInvocationContext == null)
			{
				return null;
			}
			IMethod method = MethodInvocationContext.Target.Entity as IMethod;
			if (method == null)
			{
				return null;
			}
			int num = MethodInvocationContext.Arguments.IndexOf(Closure);
			IParameter[] parameters = method.GetParameters();
			if (num < parameters.Length)
			{
				return parameters[num].Type;
			}
			if (method.AcceptVarArgs)
			{
				return parameters[parameters.Length - 1].Type;
			}
			return null;
		}

		private IType GetTypeFromCastContext()
		{
			TryCastExpression tryCastExpression = Closure.ParentNode as TryCastExpression;
			if (tryCastExpression != null)
			{
				return tryCastExpression.Type.Entity as IType;
			}
			CastExpression castExpression = Closure.ParentNode as CastExpression;
			if (castExpression != null)
			{
				return castExpression.Type.Entity as IType;
			}
			return null;
		}

		private void InferInputTypesFromContextType(ICallableType type)
		{
			CallableSignature signature = type.GetSignature();
			for (int i = 0; i < Math.Min(ParameterTypes.Length, signature.Parameters.Length); i++)
			{
				if (ParameterTypes[i] == null)
				{
					ParameterTypes[i] = signature.Parameters[i].Type;
				}
			}
		}

		public bool HasUntypedInputParameters()
		{
			return Array.IndexOf(ParameterTypes, null) != -1;
		}
	}
}
