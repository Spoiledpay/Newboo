using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	public sealed class ValueTypeArrayEqualityComparer<T> : ArrayEqualityComparerBase<T> where T : struct
	{
		private static readonly IEqualityComparer<T[]> _default = new ValueTypeArrayEqualityComparer<T>();

		public static IEqualityComparer<T[]> Default => _default;

		private ValueTypeArrayEqualityComparer()
		{
		}

		protected override bool AreEqual(T xi, T yi)
		{
			return xi.Equals(yi);
		}
	}
}
