using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Ast
{
	public class NodeCollection<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ICloneable, IEquatable<NodeCollection<T>> where T : Node
	{
		private Node _parent;

		private readonly List<T> _list;

		public T this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				if (_list[index] != value)
				{
					_list[index] = value;
					OnChanged();
				}
			}
		}

		public Node ParentNode => _parent;

		public bool IsEmpty => 0 == _list.Count;

		public int Count => _list.Count;

		public bool IsReadOnly => false;

		public bool IsSynchronized => false;

		public object SyncRoot => this;

		internal List<T> InnerList => _list;

		public T First => IsEmpty ? null : _list[0];

		public T Last => IsEmpty ? null : _list[Count - 1];

		public event EventHandler Changed;

		protected NodeCollection()
		{
			_list = new List<T>();
		}

		protected NodeCollection(Node parent)
		{
			_parent = parent;
			_list = new List<T>();
		}

		protected NodeCollection(Node parent, IEnumerable<T> list)
		{
			if (null == list)
			{
				throw new ArgumentNullException("list");
			}
			_parent = parent;
			_list = new List<T>(list);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void CopyTo(Array array, int index)
		{
			((ICollection)_list).CopyTo(array, index);
		}

		public void CopyTo(T[] array, int index)
		{
			_list.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public void Clear()
		{
			_list.Clear();
			OnChanged();
		}

		public T[] ToArray()
		{
			return _list.ToArray();
		}

		public TOut[] ToArray<TOut>(Function<T, TOut> selector)
		{
			return _list.ToArray(selector);
		}

		public T[] ToReverseArray()
		{
			T[] array = ToArray();
			Array.Reverse(array);
			return array;
		}

		public IEnumerable<T> Except<UnwantedNodeType>() where UnwantedNodeType : T
		{
			foreach (T node in _list)
			{
				if (!(node is UnwantedNodeType))
				{
					yield return node;
				}
			}
		}

		public IEnumerable<T> Except<UnwantedNodeType, UnwantedNodeType2>() where UnwantedNodeType : T where UnwantedNodeType2 : T
		{
			foreach (T node in _list)
			{
				if (!(node is UnwantedNodeType) && !(node is UnwantedNodeType2))
				{
					yield return node;
				}
			}
		}

		protected IEnumerable InternalPopRange(int begin)
		{
			OnChanged();
			return _list.PopRange(begin);
		}

		public bool Contains(T node)
		{
			foreach (T item in _list)
			{
				if (item == node)
				{
					return true;
				}
			}
			return false;
		}

		[Obsolete("Use Contains(T node) instead.")]
		public bool ContainsNode(T node)
		{
			return Contains(node);
		}

		public bool Contains(Predicate<T> condition)
		{
			return _list.Contains(condition);
		}

		public bool ContainsEntity(IEntity entity)
		{
			foreach (T item in _list)
			{
				T current = item;
				if (entity == current.Entity)
				{
					return true;
				}
			}
			return false;
		}

		public Node RemoveByEntity(IEntity entity)
		{
			if (null == entity)
			{
				throw new ArgumentNullException("entity");
			}
			for (int i = 0; i < _list.Count; i++)
			{
				Node node = _list[i];
				if (entity == node.Entity)
				{
					RemoveAt(i);
					return node;
				}
			}
			return null;
		}

		public virtual object Clone()
		{
			NodeCollection<T> nodeCollection = (NodeCollection<T>)Activator.CreateInstance(GetType());
			List<T> list = nodeCollection._list;
			foreach (T item in _list)
			{
				T current = item;
				list.Add((T)current.CloneNode());
			}
			return nodeCollection;
		}

		public void ClearTypeSystemBindings()
		{
			foreach (T item in _list)
			{
				T current = item;
				current.ClearTypeSystemBindings();
			}
		}

		internal void InitializeParent(Node parent)
		{
			_parent = parent;
			foreach (T item in _list)
			{
				T current = item;
				current.InitializeParent(_parent);
			}
		}

		public void Reject(Predicate<T> condition)
		{
			if (null == condition)
			{
				throw new ArgumentNullException("condition");
			}
			int num = 0;
			T[] array = ToArray();
			foreach (T obj in array)
			{
				if (condition(obj))
				{
					RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			OnChanged();
		}

		public void ExtendWithClones(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				T current = item;
				Add((T)current.CloneNode());
			}
		}

		public void ReplaceAt(int i, T newItem)
		{
			Initialize(newItem);
			_list[i] = newItem;
			OnChanged();
		}

		public void Add(T item)
		{
			Initialize(item);
			_list.Add(item);
			OnChanged();
		}

		[Obsolete("Use AddRange")]
		public void Extend(IEnumerable<T> items)
		{
			AddRange(items);
		}

		public void AddRange(IEnumerable<T> items)
		{
			AssertNotNull("items", items);
			foreach (T item in items)
			{
				Add(item);
			}
		}

		public bool Replace(T existing, T newItem)
		{
			AssertNotNull("existing", existing);
			for (int i = 0; i < _list.Count; i++)
			{
				if (this[i] == existing)
				{
					if (null == newItem)
					{
						RemoveAt(i);
					}
					else
					{
						ReplaceAt(i, newItem);
					}
					return true;
				}
			}
			return false;
		}

		public void Insert(int index, T item)
		{
			Initialize(item);
			_list.Insert(index, item);
			OnChanged();
		}

		public bool Remove(T item)
		{
			if (((ICollection<T>)_list).Remove(item))
			{
				OnChanged();
				return true;
			}
			return false;
		}

		private void OnChanged()
		{
			this.Changed?.Invoke(this, EventArgs.Empty);
		}

		public override int GetHashCode()
		{
			return _list.GetHashCode();
		}

		public override bool Equals(object other)
		{
			return Equals(other as NodeCollection<T>);
		}

		public bool Equals(NodeCollection<T> other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (InnerList.Count != other.Count)
			{
				return false;
			}
			IEnumerator<T> enumerator = other.GetEnumerator();
			using (IEnumerator<T> enumerator2 = GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					T current = enumerator2.Current;
					if (!enumerator.MoveNext())
					{
						return false;
					}
					if (!current.Equals(enumerator.Current))
					{
						return false;
					}
				}
			}
			return true;
		}

		private void Initialize(Node item)
		{
			AssertNotNull("item", item);
			if (null != _parent)
			{
				item.InitializeParent(_parent);
			}
		}

		private static void AssertNotNull(string descrip, object o)
		{
			if (o == null)
			{
				throw new ArgumentException($"null reference for: {descrip}");
			}
		}

		public NodeCollection<TNew> Cast<TNew>() where TNew : Node
		{
			return Cast<TNew>(0);
		}

		public NodeCollection<TNew> Cast<TNew>(int index) where TNew : Node
		{
			if (index < 0 || index > Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (0 == Count - index)
			{
				return Empty<TNew>(ParentNode);
			}
			NodeCollection<TNew> nodeCollection = new NodeCollection<TNew>(ParentNode);
			int num = -1;
			foreach (T item in _list)
			{
				T current = item;
				num++;
				if (num < index)
				{
					continue;
				}
				TNew val = current as TNew;
				if (null == val)
				{
					ExpressionStatement expressionStatement = current as ExpressionStatement;
					if (null != expressionStatement)
					{
						val = expressionStatement.Expression as TNew;
					}
				}
				if (null == val)
				{
					throw new InvalidCastException($"Cannot cast item #{num + 1} from `{current.GetType()}` to `{typeof(TNew)}`");
				}
				nodeCollection.Add(val);
			}
			return nodeCollection;
		}

		public static NodeCollection<TNode> Empty<TNode>(Node parent) where TNode : Node
		{
			return new NodeCollection<TNode>(parent);
		}

		public bool StartsWith<TNode>() where TNode : T
		{
			return !IsEmpty && First is TNode;
		}

		public bool EndsWith<TNode>() where TNode : T
		{
			return !IsEmpty && Last is TNode;
		}
	}
}
