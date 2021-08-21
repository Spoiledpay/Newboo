using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalEvent : InternalEntity<Event>, IEvent, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		public Event Event => _node;

		public override EntityType EntityType => EntityType.Event;

		public IType Type => (IType)TypeSystemServices.GetEntity(_node.Type);

		public bool IsVirtual => _node.IsVirtual;

		public bool IsAbstract => _node.IsAbstract;

		public InternalField BackingField { get; set; }

		public bool IsDuckTyped => false;

		public InternalEvent(Event event_)
			: base(event_)
		{
		}

		public IMethod GetAddMethod()
		{
			return (IMethod)TypeSystemServices.GetEntity(_node.Add);
		}

		public IMethod GetRemoveMethod()
		{
			return (IMethod)TypeSystemServices.GetEntity(_node.Remove);
		}

		public IMethod GetRaiseMethod()
		{
			return (IMethod)TypeSystemServices.GetEntity(_node.Raise);
		}
	}
}
