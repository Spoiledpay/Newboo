using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class InferredType
	{
		private delegate bool Relation<T>(T item1, T item2);

		private IType _resultingType = null;

		private readonly List<IType> _lowerBounds = new List<IType>();

		private readonly List<IType> _upperBounds = new List<IType>();

		private readonly IDictionary<InferredType, bool> _dependencies = new Dictionary<InferredType, bool>();

		private readonly IDictionary<InferredType, bool> _dependants = new Dictionary<InferredType, bool>();

		public IType ResultingType => _resultingType;

		public bool Fixed => _resultingType != null;

		public bool HasBounds => _lowerBounds.Count != 0 || _upperBounds.Count != 0;

		public bool HasDependencies => _dependencies.Count != 0;

		public bool HasDependants => _dependants.Count != 0;

		public void ApplyLowerBound(IType type)
		{
			_lowerBounds.Add(type);
		}

		public void ApplyUpperBound(IType type)
		{
			_upperBounds.Add(type);
		}

		public void SetDependencyOn(InferredType dependee)
		{
			_dependencies[dependee] = true;
			dependee._dependants[this] = true;
		}

		public void RemoveDependencyOn(InferredType dependee)
		{
			_dependencies.Remove(dependee);
			dependee._dependants.Remove(this);
		}

		private void ShortenDependencies()
		{
			foreach (InferredType item in _dependants.Keys.ToList())
			{
				foreach (InferredType item2 in _dependencies.Keys.ToList())
				{
					item.SetDependencyOn(item2);
				}
				item.RemoveDependencyOn(this);
			}
		}

		public bool Fix()
		{
			if (!HasBounds)
			{
				return false;
			}
			IType type = FindSink(_lowerBounds, (IType t1, IType t2) => IsAssignableFrom(t1, t2));
			IType type2 = FindSink(_upperBounds, (IType t1, IType t2) => IsAssignableFrom(t2, t1));
			if (type == null)
			{
				return Fix(type2);
			}
			if (type2 == null)
			{
				return Fix(type);
			}
			if (IsAssignableFrom(type2, type))
			{
				return Fix(type);
			}
			return false;
		}

		private bool IsAssignableFrom(IType t1, IType t2)
		{
			return TypeCompatibilityRules.IsAssignableFrom(t1, t2);
		}

		private bool Fix(IType type)
		{
			_resultingType = type;
			if (type == null)
			{
				return false;
			}
			ShortenDependencies();
			return true;
		}

		private IType FindSink(IEnumerable<IType> types, Relation<IType> relation)
		{
			IType type = null;
			foreach (IType type2 in types)
			{
				if (type == null || relation(type2, type))
				{
					type = type2;
				}
			}
			if (type == null)
			{
				return null;
			}
			foreach (IType type3 in types)
			{
				if (!relation(type, type3))
				{
					return null;
				}
			}
			return type;
		}
	}
}
