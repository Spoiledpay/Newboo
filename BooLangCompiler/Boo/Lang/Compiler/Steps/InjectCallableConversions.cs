using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class InjectCallableConversions : AbstractVisitorCompilerStep
	{
		private sealed class AdaptorRecord
		{
			public readonly ICallableType To;

			public readonly ICallableType From;

			public readonly ClassDefinition Adaptor;

			public AdaptorRecord(ICallableType to, ICallableType from, ClassDefinition adaptor)
			{
				To = to;
				From = from;
				Adaptor = adaptor;
			}
		}

		private IMethod _current;

		private IType _asyncResultType;

		private IMethod _asyncResultTypeAsyncDelegateGetter;

		private readonly List<AdaptorRecord> _adaptors = new List<AdaptorRecord>();

		public override void Run()
		{
			if (base.Errors.Count == 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void LeaveExpressionStatement(ExpressionStatement node)
		{
			Expression expression = ConvertExpression(node.Expression);
			if (expression != null)
			{
				node.Expression = expression;
			}
		}

		public override void LeaveReturnStatement(ReturnStatement node)
		{
			Expression expression = node.Expression;
			if (null != expression && HasReturnType(_current))
			{
				Expression expression2 = Convert(_current.ReturnType, expression);
				if (null != expression2)
				{
					node.Expression = expression2;
				}
			}
		}

		public override void LeaveExpressionPair(ExpressionPair pair)
		{
			Expression expression = ConvertExpression(pair.First);
			if (null != expression)
			{
				pair.First = expression;
			}
			expression = ConvertExpression(pair.Second);
			if (null != expression)
			{
				pair.Second = expression;
			}
		}

		public override void LeaveListLiteralExpression(ListLiteralExpression node)
		{
			ConvertExpressions(node.Items);
		}

		public override void LeaveArrayLiteralExpression(ArrayLiteralExpression node)
		{
			IType elementType = GetExpressionType(node).ElementType;
			for (int i = 0; i < node.Items.Count; i++)
			{
				Expression expression = Convert(elementType, node.Items[i]);
				if (null != expression)
				{
					node.Items.ReplaceAt(i, expression);
				}
			}
		}

		public override void LeaveMethodInvocationExpression(MethodInvocationExpression node)
		{
			IParameter[] array = ParametersFor(node.Target);
			if (array != null)
			{
				ConvertMethodInvocation(node, array);
			}
		}

		private static IParameter[] ParametersFor(Expression callableExpression)
		{
			IMethod method = callableExpression.Entity as IMethod;
			if (method != null)
			{
				return method.GetParameters();
			}
			return (callableExpression.ExpressionType as ICallableType)?.GetSignature().Parameters;
		}

		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			if (IsEndInvokeOnStandaloneMethodReference(node) && AstUtil.IsTargetOfMethodInvocation(node))
			{
				ReplaceEndInvokeTargetByGetAsyncDelegate((MethodInvocationExpression)node.ParentNode);
				return;
			}
			Expression expression = ConvertExpression(node.Target);
			if (null != expression)
			{
				node.Target = expression;
			}
		}

		public override void LeaveCastExpression(CastExpression node)
		{
			Expression expression = Convert(node.ExpressionType, node.Target);
			if (expression != null)
			{
				node.Target = expression;
			}
		}

		public override void LeaveConditionalExpression(ConditionalExpression node)
		{
			Expression expression = Convert(node.ExpressionType, node.TrueValue);
			if (expression != null)
			{
				node.TrueValue = expression;
			}
			Expression expression2 = Convert(node.ExpressionType, node.FalseValue);
			if (expression2 != null)
			{
				node.FalseValue = expression2;
			}
		}

		public override void LeaveTryCastExpression(TryCastExpression node)
		{
			Expression expression = Convert(node.ExpressionType, node.Target);
			if (null != expression)
			{
				node.Target = expression;
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (BinaryOperatorType.Assign == node.Operator)
			{
				Expression expression = Convert(node.Left.ExpressionType, node.Right);
				if (null != expression)
				{
					node.Right = expression;
				}
			}
		}

		public override void LeaveGeneratorExpression(GeneratorExpression node)
		{
			Expression expression = Convert(GetConcreteExpressionType(node.Expression), node.Expression);
			if (null != expression)
			{
				node.Expression = expression;
			}
		}

		private void InitializeAsyncResultType()
		{
			if (_asyncResultType == null)
			{
				Type typeFromHandle = typeof(AsyncResult);
				_asyncResultType = base.TypeSystemServices.Map(typeFromHandle);
				_asyncResultTypeAsyncDelegateGetter = base.TypeSystemServices.Map(Methods.GetterOf((AsyncResult r) => r.AsyncDelegate));
			}
		}

		public override void Dispose()
		{
			_asyncResultType = null;
			_asyncResultTypeAsyncDelegateGetter = null;
			_adaptors.Clear();
			base.Dispose();
		}

		public override void OnMethod(Method node)
		{
			_current = GetEntity(node);
			Visit(node.Body);
		}

		private bool HasReturnType(IMethod method)
		{
			return base.TypeSystemServices.VoidType != method.ReturnType;
		}

		private bool IsMethodReference(Expression node)
		{
			IEntity entity = GetEntity(node);
			return EntityType.Method == entity.EntityType;
		}

		private static bool IsNotTargetOfMethodInvocation(Expression node)
		{
			MethodInvocationExpression methodInvocationExpression = node.ParentNode as MethodInvocationExpression;
			return methodInvocationExpression == null || methodInvocationExpression.Target != node;
		}

		private bool IsStandaloneMethodReference(Expression node)
		{
			return (node is ReferenceExpression || node is GenericReferenceExpression) && IsMethodReference(node) && IsNotTargetOfMethodInvocation(node);
		}

		private void ConvertMethodInvocation(MethodInvocationExpression node, IParameter[] parameters)
		{
			ExpressionCollection arguments = node.Arguments;
			for (int i = 0; i < parameters.Length; i++)
			{
				Expression expression = Convert(parameters[i].Type, arguments[i]);
				if (null != expression)
				{
					arguments.ReplaceAt(i, expression);
				}
			}
		}

		private void ConvertExpressions(ExpressionCollection items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Expression expression = ConvertExpression(items[i]);
				if (null != expression)
				{
					items.ReplaceAt(i, expression);
				}
			}
		}

		private Expression ConvertExpression(Expression expression)
		{
			return Convert(expression.ExpressionType, expression);
		}

		private Expression Convert(IType expectedType, Expression argument)
		{
			if (IsStandaloneMethodReference(argument))
			{
				return ConvertMethodReference(expectedType, argument);
			}
			ICallableType callableType = expectedType as ICallableType;
			if (callableType != null)
			{
				IType expressionType = GetExpressionType(argument);
				if (expectedType != expressionType && !expressionType.IsNull())
				{
					return Adapt(callableType, argument);
				}
			}
			return null;
		}

		private Expression ConvertMethodReference(IType expectedType, Expression argument)
		{
			ICallableType callableType = expectedType as ICallableType;
			if (callableType != null)
			{
				ICallableType callableType2 = (ICallableType)GetExpressionType(argument);
				if (callableType2.GetSignature() != callableType.GetSignature())
				{
					return Adapt(callableType, CreateDelegate(GetConcreteExpressionType(argument), argument));
				}
				return CreateDelegate(expectedType, argument);
			}
			return CreateDelegate(GetConcreteExpressionType(argument), argument);
		}

		private Expression Adapt(ICallableType expected, Expression callable)
		{
			ICallableType callableType = GetExpressionType(callable) as ICallableType;
			if (null == callableType)
			{
				return null;
			}
			ClassDefinition adaptor = GetAdaptor(expected, callableType);
			Method method = (Method)adaptor.Members["Adapt"];
			return base.CodeBuilder.CreateMethodInvocation((IMethod)method.Entity, callable);
		}

		private ClassDefinition GetAdaptor(ICallableType to, ICallableType from)
		{
			return FindAdaptor(to, from) ?? CreateAdaptor(to, from);
		}

		private ClassDefinition FindAdaptor(ICallableType to, ICallableType from)
		{
			foreach (AdaptorRecord adaptor in _adaptors)
			{
				if (from == adaptor.From && to == adaptor.To)
				{
					return adaptor.Adaptor;
				}
			}
			return null;
		}

		private ClassDefinition CreateAdaptor(ICallableType to, ICallableType from)
		{
			BooClassBuilder booClassBuilder = base.CodeBuilder.CreateClass("$adaptor$" + from.Name + "$" + to.Name + "$" + _adaptors.Count);
			booClassBuilder.AddBaseType(base.TypeSystemServices.ObjectType);
			booClassBuilder.Modifiers = TypeMemberModifiers.Internal | TypeMemberModifiers.Final;
			Field field = booClassBuilder.AddField("$from", from);
			BooMethodBuilder booMethodBuilder = booClassBuilder.AddConstructor();
			ParameterDeclaration parameter = booMethodBuilder.AddParameter("from", from);
			booMethodBuilder.Body.Add(base.CodeBuilder.CreateSuperConstructorInvocation(base.TypeSystemServices.ObjectType));
			booMethodBuilder.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(field), base.CodeBuilder.CreateReference(parameter)));
			CallableSignature signature = to.GetSignature();
			BooMethodBuilder booMethodBuilder2 = booClassBuilder.AddMethod("Invoke", signature.ReturnType);
			IParameter[] parameters = signature.Parameters;
			foreach (IParameter parameter2 in parameters)
			{
				booMethodBuilder2.AddParameter(parameter2.Name, parameter2.Type, parameter2.IsByRef);
			}
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(field), GetInvokeMethod(from));
			int num = from.GetSignature().Parameters.Length;
			for (int j = 0; j < num; j++)
			{
				methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(booMethodBuilder2.Parameters[j]));
			}
			if (signature.ReturnType != base.TypeSystemServices.VoidType && from.GetSignature().ReturnType != base.TypeSystemServices.VoidType)
			{
				booMethodBuilder2.Body.Add(new ReturnStatement(methodInvocationExpression));
			}
			else
			{
				booMethodBuilder2.Body.Add(methodInvocationExpression);
			}
			BooMethodBuilder booMethodBuilder3 = booClassBuilder.AddMethod("Adapt", to);
			booMethodBuilder3.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Static;
			parameter = booMethodBuilder3.AddParameter("from", from);
			booMethodBuilder3.Body.Add(new ReturnStatement(base.CodeBuilder.CreateConstructorInvocation(to.GetConstructors().First(), base.CodeBuilder.CreateConstructorInvocation((IConstructor)booMethodBuilder.Entity, base.CodeBuilder.CreateReference(parameter)), base.CodeBuilder.CreateAddressOfExpression(booMethodBuilder2.Entity))));
			RegisterAdaptor(to, from, booClassBuilder.ClassDefinition);
			return booClassBuilder.ClassDefinition;
		}

		private void RegisterAdaptor(ICallableType to, ICallableType from, ClassDefinition adaptor)
		{
			_adaptors.Add(new AdaptorRecord(to, from, adaptor));
			base.TypeSystemServices.GetCompilerGeneratedTypesModule().Members.Add(adaptor);
		}

		private bool IsEndInvokeOnStandaloneMethodReference(MemberReferenceExpression node)
		{
			if (IsStandaloneMethodReference(node.Target))
			{
				return node.Entity.Name == "EndInvoke";
			}
			return false;
		}

		private void ReplaceEndInvokeTargetByGetAsyncDelegate(MethodInvocationExpression node)
		{
			InitializeAsyncResultType();
			Expression last = node.Arguments.Last;
			MemberReferenceExpression memberReferenceExpression = (MemberReferenceExpression)node.Target;
			IType declaringType = ((IMember)memberReferenceExpression.Entity).DeclaringType;
			memberReferenceExpression.Target = base.CodeBuilder.CreateCast(declaringType, base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateCast(_asyncResultType, last.CloneNode()), _asyncResultTypeAsyncDelegateGetter));
		}

		private Expression CreateDelegate(IType type, Expression source)
		{
			IMethod method = (IMethod)GetEntity(source);
			Expression arg = (method.IsStatic ? base.CodeBuilder.CreateNullLiteral() : ((MemberReferenceExpression)source).Target);
			return base.CodeBuilder.CreateConstructorInvocation(GetConcreteType(type).GetConstructors().First(), arg, base.CodeBuilder.CreateAddressOfExpression(method));
		}

		private static IType GetConcreteType(IType type)
		{
			AnonymousCallableType anonymousCallableType = type as AnonymousCallableType;
			return (anonymousCallableType == null) ? type : anonymousCallableType.ConcreteType;
		}

		private IMethod GetInvokeMethod(ICallableType type)
		{
			return base.NameResolutionService.ResolveMethod(type, "Invoke");
		}
	}
}
