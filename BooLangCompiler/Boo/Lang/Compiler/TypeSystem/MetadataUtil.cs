using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem
{
	public static class MetadataUtil
	{
		public static bool IsAttributeDefined(TypeMember member, IType attributeType)
		{
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in member.Attributes)
			{
				if (IsOfType(attribute, attributeType))
				{
					return true;
				}
			}
			return false;
		}

		public static IEnumerable<Boo.Lang.Compiler.Ast.Attribute> GetCustomAttributes(TypeMember member, IType attributeType)
		{
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in member.Attributes)
			{
				if (IsOfType(attribute, attributeType))
				{
					yield return attribute;
				}
			}
		}

		private static bool IsOfType(Boo.Lang.Compiler.Ast.Attribute attribute, IType attributeType)
		{
			IEntity entity = TypeSystemServices.GetEntity(attribute);
			if (entity == attributeType)
			{
				return true;
			}
			IConstructor constructor = entity as IConstructor;
			return constructor != null && constructor.DeclaringType == attributeType;
		}

		public static bool IsAttributeDefined(MemberInfo member, Type attributeType)
		{
			return System.Attribute.IsDefined(member, attributeType);
		}
	}
}
