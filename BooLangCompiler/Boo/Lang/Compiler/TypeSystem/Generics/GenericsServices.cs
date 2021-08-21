using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericsServices : AbstractCompilerComponent
	{
		public GenericsServices()
			: base(CompilerContext.Current)
		{
		}

		public IEntity ConstructEntity(Node constructionNode, IEntity definition, IType[] typeArguments)
		{
			if (definition == null || TypeSystemServices.IsError(definition))
			{
				return TypeSystemServices.ErrorEntity;
			}
			if (definition.IsAmbiguous())
			{
				return ConstructAmbiguousEntity(constructionNode, (Ambiguous)definition, typeArguments);
			}
			if (!CheckGenericConstruction(constructionNode, definition, typeArguments, reportErrors: true))
			{
				return TypeSystemServices.ErrorEntity;
			}
			return MakeGenericEntity(definition, typeArguments);
		}

		private IEntity ConstructAmbiguousEntity(Node constructionNode, Ambiguous ambiguousDefinition, IType[] typeArguments)
		{
			GenericConstructionChecker genericConstructionChecker = new GenericConstructionChecker(typeArguments, constructionNode);
			List<IEntity> list = new List<IEntity>(ambiguousDefinition.Entities);
			bool flag = false;
			foreach (Predicate<IEntity> check in genericConstructionChecker.Checks)
			{
				list = list.Collect(check);
				if (list.Count == 0)
				{
					base.Errors.Add(genericConstructionChecker.Errors[0]);
					return TypeSystemServices.ErrorEntity;
				}
				if (flag)
				{
					genericConstructionChecker.ReportErrors(base.Errors);
				}
				genericConstructionChecker.DiscardErrors();
				if (list.Count == 1)
				{
					flag = true;
				}
			}
			IEntity[] entities = Array.ConvertAll(list.ToArray(), (IEntity def) => MakeGenericEntity(def, typeArguments));
			return Entities.EntityFromList(entities);
		}

		private IEntity MakeGenericEntity(IEntity definition, IType[] typeArguments)
		{
			if (IsGenericType(definition))
			{
				return ((IType)definition).GenericInfo.ConstructType(typeArguments);
			}
			if (IsGenericMethod(definition))
			{
				return ((IMethod)definition).GenericInfo.ConstructMethod(typeArguments);
			}
			return TypeSystemServices.ErrorEntity;
		}

		public bool CheckGenericConstruction(IEntity definition, IType[] typeArguments)
		{
			return CheckGenericConstruction(null, definition, typeArguments, reportErrors: false);
		}

		public bool CheckGenericConstruction(Node node, IEntity definition, IType[] typeArguments, bool reportErrors)
		{
			GenericConstructionChecker genericConstructionChecker = new GenericConstructionChecker(typeArguments, node);
			foreach (Predicate<IEntity> check in genericConstructionChecker.Checks)
			{
				if (check(definition))
				{
					continue;
				}
				if (reportErrors)
				{
					genericConstructionChecker.ReportErrors(base.Errors);
				}
				return false;
			}
			return true;
		}

		public IType[] InferMethodGenericArguments(IMethod method, ExpressionCollection arguments)
		{
			if (method.GenericInfo == null)
			{
				return null;
			}
			GenericParameterInferrer genericParameterInferrer = new GenericParameterInferrer(base.Context, method, arguments);
			if (genericParameterInferrer.Run())
			{
				return genericParameterInferrer.GetInferredTypes();
			}
			return null;
		}

		public static bool IsGenericMethod(IEntity entity)
		{
			IMethod method = entity as IMethod;
			return method != null && method.GenericInfo != null;
		}

		public static bool IsGenericType(IEntity entity)
		{
			IType type = entity as IType;
			return type != null && type.GenericInfo != null;
		}

		public static bool IsGenericParameter(IEntity entity)
		{
			return entity is IGenericParameter;
		}

		public static bool AreOfSameGenerity(IMethod lhs, IMethod rhs)
		{
			return GetMethodGenerity(lhs) == GetMethodGenerity(rhs);
		}

		public static IEnumerable<IGenericParameter> FindGenericParameters(IType type)
		{
			IGenericParameter genericParameter = type as IGenericParameter;
			if (genericParameter != null)
			{
				yield return genericParameter;
				yield break;
			}
			if (type is IArrayType)
			{
				foreach (IGenericParameter item in FindGenericParameters(type.ElementType))
				{
					yield return item;
				}
				yield break;
			}
			if (type.ConstructedInfo != null)
			{
				try
				{
					IType[] genericArguments = type.ConstructedInfo.GenericArguments;
					foreach (IType typeArgument in genericArguments)
					{
						foreach (IGenericParameter item2 in FindGenericParameters(typeArgument))
						{
							yield return item2;
						}
					}
				}
				finally
				{
				}
				yield break;
			}
			ICallableType callableType = type as ICallableType;
			if (callableType == null)
			{
				yield break;
			}
			CallableSignature signature = callableType.GetSignature();
			foreach (IGenericParameter item3 in FindGenericParameters(signature.ReturnType))
			{
				yield return item3;
			}
			try
			{
				IParameter[] parameters = signature.Parameters;
				foreach (IParameter parameter in parameters)
				{
					foreach (IGenericParameter item4 in FindGenericParameters(parameter.Type))
					{
						yield return item4;
					}
				}
			}
			finally
			{
			}
		}

		public static IEnumerable<IType> FindConstructedTypes(IType type, IType definition)
		{
			while (type != null)
			{
				if (type.ConstructedInfo != null && type.ConstructedInfo.GenericDefinition == definition)
				{
					yield return type;
				}
				IType[] interfaces = type.GetInterfaces();
				if (interfaces != null)
				{
					try
					{
						IType[] array = interfaces;
						foreach (IType interfaceType in array)
						{
							foreach (IType item in FindConstructedTypes(interfaceType, definition))
							{
								yield return item;
							}
						}
					}
					finally
					{
					}
				}
				type = type.BaseType;
			}
		}

		public static IType FindConstructedType(IType type, IType definition)
		{
			IType type2 = null;
			foreach (IType item in FindConstructedTypes(type, definition))
			{
				if (type2 == null)
				{
					type2 = item;
				}
				else if (type2 != item)
				{
					return null;
				}
			}
			return type2;
		}

		public static bool HasConstructedType(IType type, IType definition)
		{
			return FindConstructedTypes(type, definition).GetEnumerator().MoveNext();
		}

		public static bool IsOpenGenericType(IType type)
		{
			return GetTypeGenerity(type) != 0;
		}

		public static IGenericParameter[] GetGenericParameters(IEntity definition)
		{
			if (IsGenericType(definition))
			{
				return ((IType)definition).GenericInfo.GenericParameters;
			}
			if (IsGenericMethod(definition))
			{
				return ((IMethod)definition).GenericInfo.GenericParameters;
			}
			return null;
		}

		public static int GetTypeGenerity(IType type)
		{
			if (type.IsByRef || type.IsArray)
			{
				return GetTypeGenerity(type.ElementType);
			}
			if (type is IGenericParameter)
			{
				return 1;
			}
			int num = 0;
			if (type.GenericInfo != null)
			{
				num += type.GenericInfo.GenericParameters.Length;
			}
			if (type.ConstructedInfo != null)
			{
				IType[] genericArguments = type.ConstructedInfo.GenericArguments;
				foreach (IType type2 in genericArguments)
				{
					num += GetTypeGenerity(type2);
				}
			}
			return num;
		}

		public static int GetMethodGenerity(IMethod method)
		{
			IConstructedMethodInfo constructedInfo = method.ConstructedInfo;
			if (constructedInfo != null)
			{
				return constructedInfo.GenericArguments.Length;
			}
			IGenericMethodInfo genericInfo = method.GenericInfo;
			if (genericInfo != null)
			{
				return genericInfo.GenericParameters.Length;
			}
			return 0;
		}
	}
}
