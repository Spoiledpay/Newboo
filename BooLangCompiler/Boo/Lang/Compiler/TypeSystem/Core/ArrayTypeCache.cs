using System;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class ArrayTypeCache
	{
		private MemoizedFunction<int, IArrayType> _arrayTypes;

		public ArrayTypeCache(IType elementType)
		{
			Func<int, IArrayType> function = (int newRank) => new ArrayType(elementType, newRank);
			_arrayTypes = new MemoizedFunction<int, IArrayType>(function);
		}

		public IArrayType MakeArrayType(int rank)
		{
			return _arrayTypes.Invoke(rank);
		}
	}
}
