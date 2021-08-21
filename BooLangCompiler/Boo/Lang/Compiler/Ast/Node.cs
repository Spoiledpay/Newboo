using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Boo.Lang.Compiler.Ast.Visitors;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public abstract class Node : ICloneable, ITypedAnnotations
	{
		private sealed class ReplaceVisitor : DepthFirstTransformer
		{
			private readonly NodePredicate _predicate;

			private readonly Node _template;

			public int MatchCount { get; private set; }

			public ReplaceVisitor(NodePredicate predicate, Node template)
			{
				_predicate = predicate;
				_template = template;
			}

			protected override void OnNode(Node node)
			{
				if (_predicate(node))
				{
					MatchCount++;
					ReplaceCurrentNode(_template.CloneNode());
				}
				else
				{
					base.OnNode(node);
				}
			}
		}

		protected LexicalInfo _lexicalInfo = LexicalInfo.Empty;

		protected SourceLocation _endSourceLocation = LexicalInfo.Empty;

		protected Node _parent;

		protected string _documentation;

		protected Hashtable _annotations;

		protected IEntity _entity;

		protected bool _isSynthetic;

		[DefaultValue(false)]
		[XmlAttribute]
		public bool IsSynthetic
		{
			get
			{
				return _isSynthetic;
			}
			set
			{
				_isSynthetic = value;
			}
		}

		[XmlIgnore]
		public IEntity Entity
		{
			get
			{
				return _entity;
			}
			set
			{
				_entity = value;
			}
		}

		public object this[object key]
		{
			get
			{
				if (_annotations == null)
				{
					return null;
				}
				return _annotations[key];
			}
			set
			{
				if (_annotations == null)
				{
					_annotations = new Hashtable();
				}
				_annotations[key] = value;
			}
		}

		public ITypedAnnotations Tags => this;

		public bool HasAnnotations => _annotations != null && _annotations.Count > 0;

		public Node ParentNode => _parent;

		public string Documentation
		{
			get
			{
				return _documentation;
			}
			set
			{
				_documentation = value;
			}
		}

		[XmlIgnore]
		public LexicalInfo LexicalInfo
		{
			get
			{
				if (_lexicalInfo.Equals(LexicalInfo.Empty) && ParentNode != null && null != ParentNode.LexicalInfo)
				{
					_lexicalInfo = ParentNode.LexicalInfo;
				}
				return _lexicalInfo;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("LexicalInfo");
				}
				_lexicalInfo = value;
			}
		}

		[XmlIgnore]
		public virtual SourceLocation EndSourceLocation
		{
			get
			{
				return _endSourceLocation;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("EndSourceLocation");
				}
				_endSourceLocation = value;
			}
		}

		public abstract NodeType NodeType { get; }

		public static bool Matches<T>(T lhs, T rhs) where T : Node
		{
			return lhs?.Matches(rhs) ?? (rhs == null);
		}

		public static bool Matches(Block lhs, Block rhs)
		{
			return lhs?.Matches(rhs) ?? rhs?.IsEmpty ?? true;
		}

		public static bool AllMatch<T>(IEnumerable<T> lhs, IEnumerable<T> rhs) where T : Node
		{
			if (lhs == null)
			{
				return rhs == null || IsEmpty(rhs);
			}
			if (rhs == null)
			{
				return IsEmpty(lhs);
			}
			IEnumerator<T> enumerator = rhs.GetEnumerator();
			foreach (T lh in lhs)
			{
				if (!enumerator.MoveNext())
				{
					return false;
				}
				if (!Matches(lh, enumerator.Current))
				{
					return false;
				}
			}
			if (enumerator.MoveNext())
			{
				return false;
			}
			return true;
		}

		private static bool IsEmpty<T>(IEnumerable<T> e)
		{
			return !e.GetEnumerator().MoveNext();
		}

		protected Node()
		{
			_lexicalInfo = LexicalInfo.Empty;
		}

		protected Node(LexicalInfo lexicalInfo)
		{
			if (null == lexicalInfo)
			{
				throw new ArgumentNullException("lexicalInfo");
			}
			_lexicalInfo = lexicalInfo;
		}

		protected void InitializeFrom(Node other)
		{
			_lexicalInfo = other.LexicalInfo;
			_isSynthetic = other.IsSynthetic;
		}

		public Node CloneNode()
		{
			return (Node)Clone();
		}

		T ITypedAnnotations.Get<T>()
		{
			return (T)this[typeof(T).TypeHandle];
		}

		void ITypedAnnotations.Set<T>(T annotation)
		{
			Annotate(typeof(T).TypeHandle, annotation);
		}

		public void Annotate(object key)
		{
			Annotate(key, key);
		}

		public void Annotate(object key, object value)
		{
			if (_annotations == null)
			{
				_annotations = new Hashtable();
			}
			_annotations.Add(key, value);
		}

		public bool ContainsAnnotation(object key)
		{
			if (_annotations == null)
			{
				return false;
			}
			return _annotations.ContainsKey(key);
		}

		public void RemoveAnnotation(object key)
		{
			if (_annotations != null)
			{
				_annotations.Remove(key);
			}
		}

		internal virtual void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public virtual bool Replace(Node existing, Node newNode)
		{
			if (null == existing)
			{
				throw new ArgumentNullException("existing");
			}
			return false;
		}

		public int ReplaceNodes(Node pattern, Node template)
		{
			NodePredicate predicate = pattern.Matches;
			return ReplaceNodes(predicate, template);
		}

		public int ReplaceNodes(NodePredicate predicate, Node template)
		{
			ReplaceVisitor replaceVisitor = new ReplaceVisitor(predicate, template);
			Accept(replaceVisitor);
			return replaceVisitor.MatchCount;
		}

		internal void InitializeParent(Node parent)
		{
			_parent = parent;
		}

		public abstract void Accept(IAstVisitor visitor);

		public abstract object Clone();

		public Node CleanClone()
		{
			Node node = (Node)Clone();
			node.ClearTypeSystemBindings();
			return node;
		}

		public abstract bool Matches(Node other);

		protected bool NoMatch(string fieldName)
		{
			return false;
		}

		public override string ToString()
		{
			return ToCodeString();
		}

		public string ToCodeString()
		{
			StringWriter stringWriter = new StringWriter();
			Accept(new BooPrinterVisitor(stringWriter));
			return stringWriter.ToString();
		}

		public Node GetAncestor(NodeType ancestorType)
		{
			return GetAncestor(ancestorType, int.MaxValue);
		}

		public Node GetAncestor(NodeType ancestorType, int limitDepth)
		{
			Node parentNode = ParentNode;
			while (parentNode != null && limitDepth > 0)
			{
				if (ancestorType == parentNode.NodeType)
				{
					return parentNode;
				}
				parentNode = parentNode.ParentNode;
				limitDepth--;
			}
			return null;
		}

		public TAncestor GetAncestor<TAncestor>() where TAncestor : Node
		{
			for (Node parentNode = ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
			{
				TAncestor val = parentNode as TAncestor;
				if (null != val)
				{
					return val;
				}
			}
			return null;
		}

		public TAncestor GetRootAncestor<TAncestor>() where TAncestor : Node
		{
			TAncestor result = null;
			foreach (TAncestor ancestor in GetAncestors<TAncestor>())
			{
				result = ancestor;
			}
			return result;
		}

		public IEnumerable<TAncestor> GetAncestors<TAncestor>() where TAncestor : Node
		{
			for (Node parent = ParentNode; parent != null; parent = parent.ParentNode)
			{
				TAncestor ancestor = parent as TAncestor;
				if (ancestor != null)
				{
					yield return ancestor;
				}
			}
		}
	}
}
