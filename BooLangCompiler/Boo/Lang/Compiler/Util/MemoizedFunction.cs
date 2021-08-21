using System;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	public class MemoizedFunction<TArg, TResult>
	{
		private readonly IEqualityComparer<TArg> _comparer;

		private readonly Func<TArg, TResult> _function;

		private readonly IDictionary<TArg, TResult> _cachedValues;

		public ICollection<TResult> Values => _cachedValues.Values;

		public MemoizedFunction(Func<TArg, TResult> function)
			: this((IEqualityComparer<TArg>)SafeComparer<TArg>.Instance, function)
		{
		}

		public MemoizedFunction(IEqualityComparer<TArg> comparer, Func<TArg, TResult> function)
			: this(comparer, function, (IDictionary<TArg, TResult>)new Dictionary<TArg, TResult>(comparer))
		{
		}

		private MemoizedFunction(IEqualityComparer<TArg> comparer, Func<TArg, TResult> function, IDictionary<TArg, TResult> cachedValues)
		{
			_cachedValues = cachedValues;
			_function = function;
			_comparer = comparer;
		}

		public MemoizedFunction<TArg, TResult> Clone()
		{
			return new MemoizedFunction<TArg, TResult>(_comparer, _function, new Dictionary<TArg, TResult>(_cachedValues, _comparer));
		}

		public TResult Invoke(TArg arg)
		{
			if (_cachedValues.TryGetValue(arg, out var value))
			{
				return value;
			}
			TResult val = _function(arg);
			_cachedValues.Add(arg, val);
			return val;
		}

		public void Clear(TArg arg)
		{
			_cachedValues.Remove(arg);
		}

		public void Clear()
		{
			_cachedValues.Clear();
		}

		public bool TryGetValue(TArg arg, out TResult result)
		{
			return _cachedValues.TryGetValue(arg, out result);
		}

		public void Add(TArg arg, TResult result)
		{
			_cachedValues.Add(arg, result);
		}
	}
	public class MemoizedFunction<T1, T2, TResult>
	{
		private readonly Dictionary<T1, Dictionary<T2, TResult>> _cache;

		private readonly Func<T1, T2, TResult> _func;

		public MemoizedFunction(Func<T1, T2, TResult> func)
			: this((IEqualityComparer<T1>)SafeComparer<T1>.Instance, func)
		{
		}

		public MemoizedFunction(IEqualityComparer<T1> comparer, Func<T1, T2, TResult> func)
		{
			_cache = new Dictionary<T1, Dictionary<T2, TResult>>(comparer);
			_func = func;
		}

		public void Clear(T1 arg1)
		{
			_cache.Remove(arg1);
		}

		public void Clear()
		{
			_cache.Clear();
		}

		public TResult Invoke(T1 arg1, T2 arg2)
		{
			if (_cache.TryGetValue(arg1, out var value) && value.TryGetValue(arg2, out var value2))
			{
				return value2;
			}
			TResult val = _func(arg1, arg2);
			if (value == null)
			{
				value = new Dictionary<T2, TResult>();
				_cache.Add(arg1, value);
			}
			value.Add(arg2, val);
			return val;
		}
	}
}
