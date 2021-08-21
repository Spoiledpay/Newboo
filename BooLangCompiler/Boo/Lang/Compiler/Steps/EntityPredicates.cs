using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	internal static class EntityPredicates
	{
		public static bool IsInternalEntity(IEntity entity)
		{
			return entity is IInternalEntity;
		}

		public static bool IsNonInternalEntity(IEntity entity)
		{
			return !IsInternalEntity(entity);
		}
	}
}
