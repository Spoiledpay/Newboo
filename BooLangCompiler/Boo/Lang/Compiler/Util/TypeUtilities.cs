using System;

namespace Boo.Lang.Compiler.Util
{
	public class TypeUtilities
	{
		public static string GetFullName(Type type)
		{
			if (type.IsByRef)
			{
				return "ref " + GetFullName(type.GetElementType());
			}
			if (type.DeclaringType != null)
			{
				return GetFullName(type.DeclaringType) + "." + TypeName(type);
			}
			string @namespace = type.Namespace;
			if (string.IsNullOrEmpty(@namespace))
			{
				return TypeName(type);
			}
			return @namespace + "." + TypeName(type);
		}

		public static string TypeName(Type type)
		{
			return RemoveGenericSuffixFrom(type.Name);
		}

		public static string RemoveGenericSuffixFrom(string typeName)
		{
			int num = typeName.LastIndexOf('`');
			if (num < 0)
			{
				return typeName;
			}
			return typeName.Substring(0, num);
		}
	}
}
