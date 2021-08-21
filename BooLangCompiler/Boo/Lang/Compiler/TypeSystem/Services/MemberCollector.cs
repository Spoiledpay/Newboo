using System.Collections.Generic;
using System.Linq;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public static class MemberCollector
	{
		public static IEntity[] CollectAllMembers(INamespace entity)
		{
			List<IEntity> list = new List<IEntity>();
			CollectAllMembers(list, entity);
			return list.ToArray();
		}

		private static void CollectAllMembers(List<IEntity> members, INamespace entity)
		{
			IType type = entity as IType;
			if (null != type)
			{
				members.ExtendUnique(type.GetMembers());
				CollectBaseTypeMembers(members, type.BaseType);
			}
			else
			{
				members.Extend(entity.GetMembers());
			}
		}

		private static void CollectBaseTypeMembers(List<IEntity> members, IType baseType)
		{
			if (null != baseType)
			{
				members.Extend(from m in baseType.GetMembers()
					where !(m is IConstructor) && !IsHiddenBy(m, members)
					select m);
				CollectBaseTypeMembers(members, baseType.BaseType);
			}
		}

		private static bool IsHiddenBy(IEntity entity, IEnumerable<IEntity> members)
		{
			IMethod i = entity as IMethod;
			if (i != null)
			{
				return members.OfType<IMethod>().Any((IMethod existing) => SameNameAndSignature(i, existing));
			}
			return members.OfType<IEntity>().Any((IEntity existing) => existing.Name == entity.Name);
		}

		private static bool SameNameAndSignature(IMethod method, IMethod existing)
		{
			if (method.Name != existing.Name)
			{
				return false;
			}
			return method.CallableType == existing.CallableType;
		}
	}
}
