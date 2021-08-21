using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericConstructedCallableType : GenericConstructedType, ICallableType, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private CallableSignature _signature;

		public bool IsAnonymous => false;

		public GenericConstructedCallableType(ICallableType definition, IType[] arguments)
			: base(definition, arguments)
		{
		}

		public CallableSignature GetSignature()
		{
			if (_signature == null)
			{
				CallableSignature signature = ((ICallableType)_definition).GetSignature();
				IParameter[] parameters = base.GenericMapping.MapParameters(signature.Parameters);
				IType returnType = base.GenericMapping.MapType(signature.ReturnType);
				_signature = new CallableSignature(parameters, returnType);
			}
			return _signature;
		}

		public override bool IsAssignableFrom(IType other)
		{
			return My<TypeSystemServices>.Instance.IsCallableTypeAssignableFrom(this, other);
		}
	}
}
