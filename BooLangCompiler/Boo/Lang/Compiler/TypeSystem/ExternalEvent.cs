using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalEvent : ExternalEntity<EventInfo>, IEvent, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		private IMethod _add;

		private IMethod _remove;

		public virtual IType DeclaringType => _provider.Map(_memberInfo.DeclaringType);

		public EventInfo EventInfo => _memberInfo;

		public bool IsPublic => GetAddMethod().IsPublic;

		public override EntityType EntityType => EntityType.Event;

		public virtual IType Type => _provider.Map(_memberInfo.EventHandlerType);

		public bool IsStatic => GetAddMethod().IsStatic;

		public bool IsAbstract => GetAddMethod().IsAbstract;

		public bool IsVirtual => GetAddMethod().IsVirtual;

		protected override Type MemberType => _memberInfo.EventHandlerType;

		public ExternalEvent(IReflectionTypeSystemProvider typeSystemServices, EventInfo event_)
			: base(typeSystemServices, event_)
		{
		}

		public virtual IMethod GetAddMethod()
		{
			if (null != _add)
			{
				return _add;
			}
			return _add = FindAddMethod();
		}

		private IMethod FindAddMethod()
		{
			return _provider.Map(_memberInfo.GetAddMethod(nonPublic: true));
		}

		public virtual IMethod GetRemoveMethod()
		{
			if (null != _remove)
			{
				return _remove;
			}
			return _remove = FindRemoveMethod();
		}

		private IMethod FindRemoveMethod()
		{
			return _provider.Map(_memberInfo.GetRemoveMethod(nonPublic: true));
		}

		public virtual IMethod GetRaiseMethod()
		{
			return _provider.Map(_memberInfo.GetRaiseMethod(nonPublic: true));
		}
	}
}
