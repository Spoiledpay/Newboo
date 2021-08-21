using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	public sealed class ArrayEqualityComparer<T> : ArrayEqualityComparerBase<T> where T : class
	{
		private static readonly IEqualityComparer<T[]> _default = new ArrayEqualityComparer<T>();

		public static IEqualityComparer<T[]> Default => _default;

		private ArrayEqualityComparer()
		{
		}

		protected override bool AreEqual(T xi, T yi)
		{
			return object.Equals(xi, yi);
		}
	}
}
