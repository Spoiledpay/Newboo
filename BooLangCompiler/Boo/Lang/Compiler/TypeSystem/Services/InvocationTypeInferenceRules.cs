using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class InvocationTypeInferenceRules : AbstractCompilerComponent
	{
		public static class BuiltinRules
		{
			public static IType ArrayOfTypeReferencedByFirstArgument(MethodInvocationExpression invocation, IMethod method)
			{
				return TypeSystemServices.GetReferencedType(invocation.Arguments[0])?.MakeArrayType(1);
			}

			public static IType TypeReferencedByFirstArgument(MethodInvocationExpression invocation, IMethod method)
			{
				return TypeSystemServices.GetReferencedType(invocation.Arguments[0]);
			}

			public static IType TypeReferencedBySecondArgument(MethodInvocationExpression invocation, IMethod method)
			{
				return TypeSystemServices.GetReferencedType(invocation.Arguments[1]);
			}

			public static IType TypeOfFirstArgument(MethodInvocationExpression invocation, IMethod method)
			{
				return TypeSystemServices.GetExpressionType(invocation.Arguments[0]);
			}

			public static IType NoTypeInference(MethodInvocationExpression invocation, IMethod method)
			{
				return null;
			}
		}

		private Dictionary<IMethod, InvocationTypeInferenceRule> _invocationTypeInferenceRules = new Dictionary<IMethod, InvocationTypeInferenceRule>();

		private TypeInferenceRuleProvider _provider = My<TypeInferenceRuleProvider>.Instance;

		public void RegisterTypeInferenceRuleFor(IMethod method, InvocationTypeInferenceRule rule)
		{
			_invocationTypeInferenceRules.Add(method, rule);
		}

		public void RegisterTypeInferenceRuleFor(IMethod[] methods, InvocationTypeInferenceRule rule)
		{
			foreach (IMethod method in methods)
			{
				RegisterTypeInferenceRuleFor(method, rule);
			}
		}

		public IType ApplyTo(MethodInvocationExpression invocation, IMethod method)
		{
			if (!_invocationTypeInferenceRules.TryGetValue(method, out var value))
			{
				value = ResolveRuleFor(invocation, method);
				_invocationTypeInferenceRules.Add(method, value);
			}
			return value(invocation, method);
		}

		public InvocationTypeInferenceRules()
			: base(CompilerContext.Current)
		{
			IMethod method2 = Map(Methods.Of<IEnumerable, Array>(Builtins.array));
			IMethod Array_TypedEnumerableConstructor = Map(Methods.Of<Type, IEnumerable, Array>(Builtins.array));
			IMethod method3 = Map(Methods.Of<Type, int[], Array>(Builtins.matrix));
			RegisterTypeInferenceRuleFor(method3, (MethodInvocationExpression invocation, IMethod method) => TypeSystemServices.GetReferencedType(invocation.Arguments[0])?.MakeArrayType(invocation.Arguments.Count - 1));
			RegisterTypeInferenceRuleFor(method2, delegate(MethodInvocationExpression invocation, IMethod method)
			{
				IType enumeratorItemType = base.TypeSystemServices.GetEnumeratorItemType(TypeSystemServices.GetExpressionType(invocation.Arguments[0]));
				if (base.TypeSystemServices.ObjectType == enumeratorItemType)
				{
					return null;
				}
				invocation.Target.Entity = Array_TypedEnumerableConstructor;
				invocation.ExpressionType = Array_TypedEnumerableConstructor.ReturnType;
				invocation.Arguments.Insert(0, base.CodeBuilder.CreateReference(enumeratorItemType));
				return enumeratorItemType.MakeArrayType(1);
			});
		}

		private IMethod Map(MethodInfo method)
		{
			return base.TypeSystemServices.Map(method);
		}

		private InvocationTypeInferenceRule ResolveRuleFor(MethodInvocationExpression invocation, IMethod method)
		{
			ExternalMethod externalMethod = method as ExternalMethod;
			if (externalMethod != null)
			{
				string text = TypeInferenceRuleFor(externalMethod.MethodInfo);
				if (text != null)
				{
					return ResolveRule(invocation, method, text);
				}
			}
			return BuiltinRules.NoTypeInference;
		}

		private string TypeInferenceRuleFor(MethodBase method)
		{
			return _provider.TypeInferenceRuleFor(method);
		}

		private InvocationTypeInferenceRule ResolveRule(MethodInvocationExpression invocation, IMethod method, string rule)
		{
			MethodInfo method2 = typeof(BuiltinRules).GetMethod(rule);
			if (method2 != null)
			{
				return (InvocationTypeInferenceRule)Delegate.CreateDelegate(typeof(InvocationTypeInferenceRule), method2);
			}
			base.Warnings.Add(CompilerWarningFactory.CustomWarning(invocation, $"Unknown type inference rule '{rule}' on method '{method}'."));
			return BuiltinRules.NoTypeInference;
		}
	}
}
