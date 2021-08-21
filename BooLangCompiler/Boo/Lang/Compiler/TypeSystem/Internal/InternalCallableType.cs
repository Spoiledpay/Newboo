using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalCallableType : InternalClass, ICallableType, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private CallableSignature _signature;

		public bool IsAnonymous => _node.ContainsAnnotation(AnonymousCallablesManager.AnonymousCallableTypeAnnotation);

		internal InternalCallableType(InternalTypeSystemProvider provider, TypeDefinition typeDefinition)
			: base(provider, typeDefinition)
		{
		}

		public CallableSignature GetSignature()
		{
			if (null == _signature)
			{
				IMethod invokeMethod = GetInvokeMethod();
				if (null == invokeMethod)
				{
					return null;
				}
				_signature = invokeMethod.CallableType.GetSignature();
			}
			return _signature;
		}

		protected override IType CreateConstructedType(IType[] arguments)
		{
			return new GenericConstructedCallableType(this, arguments);
		}

		public IMethod GetInvokeMethod()
		{
			return (IMethod)_node.Members["Invoke"].Entity;
		}

		public IMethod GetEndInvokeMethod()
		{
			return (IMethod)_node.Members["EndInvoke"].Entity;
		}

		public override bool IsAssignableFrom(IType other)
		{
			return _provider.IsCallableTypeAssignableFrom(this, other);
		}
	}
}
