using System;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalCallableType : ExternalType, ICallableType, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private readonly IMethod _invoke;

		public bool IsAnonymous => false;

		public ExternalCallableType(IReflectionTypeSystemProvider provider, Type type)
			: base(provider, type)
		{
			_invoke = provider.Map(type.GetMethod("Invoke"));
		}

		public CallableSignature GetSignature()
		{
			return _invoke.CallableType.GetSignature();
		}

		public override bool IsAssignableFrom(IType other)
		{
			return My<TypeSystemServices>.Instance.IsCallableTypeAssignableFrom(this, other);
		}
	}
}
