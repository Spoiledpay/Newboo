using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	internal sealed class SafeComparer<T> : IEqualityComparer<T>
	{
		private static SafeComparer<T> _instance;

		public static SafeComparer<T> Instance => _instance ?? (_instance = new SafeComparer<T>());

		public bool Equals(T x, T y)
		{
			return object.Equals(x, y);
		}

		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}
	}
}
