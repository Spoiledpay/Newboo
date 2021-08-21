using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	public abstract class ArrayEqualityComparerBase<T> : IEqualityComparer<T[]>
	{
		public bool Equals(T[] x, T[] y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int i = 0; i < x.Length; i++)
			{
				if (!AreEqual(x[i], y[i]))
				{
					return false;
				}
			}
			return true;
		}

		protected abstract bool AreEqual(T xi, T yi);

		public int GetHashCode(T[] args)
		{
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				num ^= i ^ args[i].GetHashCode();
			}
			return num;
		}
	}
}
