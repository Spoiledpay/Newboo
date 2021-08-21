using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public static class Entities
	{
		public static IEntity PreferInternalEntitiesOverExternalOnes(IEntity entity)
		{
			Ambiguous ambiguous = entity as Ambiguous;
			if (null == ambiguous)
			{
				return entity;
			}
			if (!ambiguous.Any(EntityPredicates.IsInternalEntity) || !ambiguous.Any(EntityPredicates.IsNonInternalEntity))
			{
				return entity;
			}
			return EntityFromList(ambiguous.Select(EntityPredicates.IsInternalEntity));
		}

		public static IEntity EntityFromList(ICollection<IEntity> entities)
		{
			return entities.Count switch
			{
				0 => null, 
				1 => entities.First(), 
				_ => new Ambiguous(entities), 
			};
		}

		public static bool IsFlagSet(EntityType flags, EntityType flag)
		{
			return (flags & flag) != 0;
		}
	}
}
