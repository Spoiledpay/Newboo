using System;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class Ambiguous : IEntity
	{
		public static readonly IEntity[] NoEntities = new IEntity[0];

		private IEntity[] _entities;

		public string Name => _entities[0].Name;

		public string FullName => _entities[0].FullName;

		public EntityType EntityType => EntityType.Ambiguous;

		public IEntity[] Entities => _entities;

		public Ambiguous(ICollection<IEntity> entities)
			: this(ToArray(entities))
		{
		}

		public Ambiguous(IEntity[] entities)
		{
			if (null == entities)
			{
				throw new ArgumentNullException("entities");
			}
			if (0 == entities.Length)
			{
				throw new ArgumentException("entities");
			}
			_entities = entities;
		}

		public System.Collections.Generic.List<IEntity> Select(EntityPredicate predicate)
		{
			System.Collections.Generic.List<IEntity> list = new System.Collections.Generic.List<IEntity>();
			IEntity[] entities = _entities;
			foreach (IEntity entity in entities)
			{
				if (predicate(entity))
				{
					list.Add(entity);
				}
			}
			return list;
		}

		public override string ToString()
		{
			return string.Format("Ambiguous({0})", Builtins.join(_entities, ", "));
		}

		private static IEntity[] ToArray(ICollection<IEntity> entities)
		{
			if (entities.Count == 0)
			{
				return NoEntities;
			}
			IEntity[] array = new IEntity[entities.Count];
			entities.CopyTo(array, 0);
			return array;
		}

		public bool AllEntitiesAre(EntityType entityType)
		{
			IEntity[] entities = _entities;
			foreach (IEntity entity in entities)
			{
				if (entityType != entity.EntityType)
				{
					return false;
				}
			}
			return true;
		}

		public bool Any(EntityPredicate predicate)
		{
			IEntity[] entities = _entities;
			foreach (IEntity entity in entities)
			{
				if (predicate(entity))
				{
					return true;
				}
			}
			return false;
		}
	}
}
