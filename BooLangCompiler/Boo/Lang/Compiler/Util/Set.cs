using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.Util
{
	public class Set<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private readonly Dictionary<T, bool> _elements = new Dictionary<T, bool>();

		public int Count => _elements.Count;

		public bool IsReadOnly => false;

		public Set()
		{
		}

		public Set(IEnumerable<T> elements)
		{
			foreach (T element in elements)
			{
				Add(element);
			}
		}

		public void Add(T element)
		{
			_elements[element] = true;
		}

		public void Clear()
		{
			_elements.Clear();
		}

		public bool Contains(T element)
		{
			return _elements.ContainsKey(element);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_elements.Keys.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _elements.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Remove(T element)
		{
			return _elements.Remove(element);
		}

		public void RemoveAll(Predicate<T> predicate)
		{
			List<T> list = new List<T>();
			foreach (T key in _elements.Keys)
			{
				if (predicate(key))
				{
					list.Add(key);
				}
			}
			foreach (T item in list)
			{
				Remove(item);
			}
		}

		public override string ToString()
		{
			return "{" + Builtins.join(this) + "}";
		}

		public bool ContainsAll(IEnumerable<T> elements)
		{
			foreach (T element in elements)
			{
				if (!Contains(element))
				{
					return false;
				}
			}
			return true;
		}

		public T[] ToArray()
		{
			T[] array = new T[Count];
			CopyTo(array, 0);
			return array;
		}
	}
}
