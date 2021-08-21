using System;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public static class EntityExtensions
	{
		public static string DisplayName(this IEntity entity)
		{
			EntityFormatter instance = My<EntityFormatter>.Instance;
			IType type = entity as IType;
			if (type != null)
			{
				return instance.FormatType(type);
			}
			IMember member = entity as IMember;
			if (member != null)
			{
				return instance.FormatTypeMember(member);
			}
			INamespace @namespace = entity as INamespace;
			if (@namespace != null)
			{
				return @namespace.FullName;
			}
			throw new NotSupportedException(entity.GetType().ToString());
		}

		public static bool IsAmbiguous(this IEntity entity)
		{
			return entity != null && entity.EntityType == EntityType.Ambiguous;
		}

		public static bool IsIndexedProperty(this IEntity entity)
		{
			IProperty property = entity as IProperty;
			return property != null && property.GetParameters().Length > 0;
		}
	}
}
