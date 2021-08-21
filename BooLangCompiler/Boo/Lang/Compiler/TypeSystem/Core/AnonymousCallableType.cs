using System;

namespace Boo.Lang.Compiler.TypeSystem.Core
{
	public class AnonymousCallableType : AbstractType, ICallableType, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private readonly TypeSystemServices _typeSystemServices;

		private readonly CallableSignature _signature;

		private IType _concreteType;

		public IType ConcreteType
		{
			get
			{
				return _concreteType;
			}
			set
			{
				if (value == null || _concreteType != null)
				{
					throw new InvalidOperationException();
				}
				_concreteType = value;
			}
		}

		public override IType BaseType => _typeSystemServices.MulticastDelegateType;

		public bool IsAnonymous => true;

		public override string Name => _signature.ToString();

		public override EntityType EntityType => EntityType.Type;

		internal AnonymousCallableType(TypeSystemServices services, CallableSignature signature)
		{
			_typeSystemServices = services;
			_signature = signature;
		}

		public override bool IsSubclassOf(IType other)
		{
			return BaseType.IsSubclassOf(other) || other == BaseType || other == _typeSystemServices.ICallableType;
		}

		public override bool IsAssignableFrom(IType other)
		{
			return _typeSystemServices.IsCallableTypeAssignableFrom(this, other);
		}

		public CallableSignature GetSignature()
		{
			return _signature;
		}

		public override int GetTypeDepth()
		{
			return 3;
		}
	}
}
