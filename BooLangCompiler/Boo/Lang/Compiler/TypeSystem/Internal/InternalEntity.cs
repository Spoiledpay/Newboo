using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public abstract class InternalEntity<T> : IInternalEntity, IEntity, IEntityWithAttributes where T : TypeMember
	{
		protected readonly T _node;

		private bool? _isExtension;

		public Node Node => _node;

		public string Name
		{
			get
			{
				T node = _node;
				return node.Name;
			}
		}

		public virtual string FullName
		{
			get
			{
				T node = _node;
				return node.FullName;
			}
		}

		public IType DeclaringType
		{
			get
			{
				T node = _node;
				return (IType)EntityFor(node.DeclaringType);
			}
		}

		public virtual bool IsStatic
		{
			get
			{
				T node = _node;
				return node.IsStatic;
			}
		}

		public virtual bool IsPublic
		{
			get
			{
				T node = _node;
				return node.IsPublic;
			}
		}

		public virtual bool IsProtected
		{
			get
			{
				T node = _node;
				return node.IsProtected;
			}
		}

		public virtual bool IsPrivate
		{
			get
			{
				T node = _node;
				return node.IsPrivate;
			}
		}

		public virtual bool IsInternal
		{
			get
			{
				T node = _node;
				return node.IsInternal;
			}
		}

		public abstract EntityType EntityType { get; }

		public bool IsExtension
		{
			get
			{
				if (!_isExtension.HasValue)
				{
					_isExtension = IsClrExtension;
				}
				return _isExtension.Value;
			}
		}

		private bool IsClrExtension => IsAttributeDefined(Types.ClrExtensionAttribute);

		protected InternalEntity(T node)
		{
			_node = node;
		}

		private static IEntity EntityFor(TypeMember member)
		{
			return My<InternalTypeSystemProvider>.Instance.EntityFor(member);
		}

		public bool IsDefined(IType type)
		{
			return MetadataUtil.IsAttributeDefined(_node, type);
		}

		protected bool IsAttributeDefined(Type attributeType)
		{
			return IsDefined(My<TypeSystemServices>.Instance.Map(attributeType));
		}
	}
}
