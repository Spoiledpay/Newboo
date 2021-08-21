using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class EntityFormatter
	{
		public virtual string FormatType(IType type)
		{
			if (type is IGenericParameter)
			{
				return type.Name;
			}
			ICallableType callableType = type as ICallableType;
			if (callableType != null && callableType.IsAnonymous)
			{
				return callableType.GetSignature().ToString();
			}
			IEnumerable<string> enumerable = GenericsFrom(type.ConstructedInfo, type.GenericInfo);
			if (enumerable != null)
			{
				return FormatGenericType(type.FullName, enumerable);
			}
			return type.FullName;
		}

		public virtual string FormatTypeMember(IMember member)
		{
			IMethod method = member as IMethod;
			if (method != null)
			{
				return FormatMethod(method);
			}
			return FormatType(member.DeclaringType) + "." + member.Name;
		}

		protected virtual string FormatMethod(IMethod method)
		{
			return FormatType(method.DeclaringType) + "." + FormatSignature(method);
		}

		protected virtual string FormatGenericType(string typeName, IEnumerable<string> genericArgs)
		{
			return typeName + FormatGenericArguments(genericArgs);
		}

		protected virtual string FormatGenericArguments(IEnumerable<string> genericArgs)
		{
			return string.Format("[of {0}]", Builtins.join(genericArgs, ", "));
		}

		public virtual string FormatSignature(IEntityWithParameters method)
		{
			StringBuilder stringBuilder = new StringBuilder(method.Name);
			IEnumerable<string> enumerable = GenericArgumentsOf(method);
			if (enumerable != null)
			{
				stringBuilder.Append(FormatGenericArguments(enumerable));
			}
			stringBuilder.Append("(");
			IParameter[] parameters = method.GetParameters();
			int num = (method.AcceptVarArgs ? (parameters.Length - 1) : (-1));
			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				if (i == num)
				{
					stringBuilder.Append('*');
				}
				stringBuilder.Append(FormatType(parameters[i].Type));
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		private IEnumerable<string> GenericsFrom(IGenericArgumentsProvider genericArgumentsProvider, IGenericParametersProvider genericParametersProvider)
		{
			if (genericArgumentsProvider != null)
			{
				return FormatTypes(genericArgumentsProvider.GenericArguments);
			}
			return genericParametersProvider?.GenericParameters.Select((IGenericParameter p) => p.Name);
		}

		private IEnumerable<string> GenericArgumentsOf(IEntityWithParameters entity)
		{
			IMethod method = entity as IMethod;
			if (method != null)
			{
				return GenericsFrom(method.ConstructedInfo, method.GenericInfo);
			}
			return null;
		}

		private IEnumerable<string> FormatTypes(IEnumerable<IType> genericArguments)
		{
			return genericArguments.Select((IType a) => FormatType(a));
		}
	}
}
