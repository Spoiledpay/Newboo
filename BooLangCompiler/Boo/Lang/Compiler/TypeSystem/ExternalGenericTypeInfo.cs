using System;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalGenericTypeInfo : AbstractExternalGenericInfo<IType>, IGenericTypeInfo, IGenericParametersProvider
	{
		private ExternalType _type;

		public ExternalGenericTypeInfo(IReflectionTypeSystemProvider provider, ExternalType type)
			: base(provider)
		{
			_type = type;
		}

		public IType ConstructType(IType[] arguments)
		{
			return ConstructEntity(arguments);
		}

		protected override Type[] GetActualGenericParameters()
		{
			return _type.ActualType.GetGenericArguments();
		}

		protected override IType ConstructExternalEntity(Type[] arguments)
		{
			return _provider.Map(_type.ActualType.MakeGenericType(arguments));
		}

		protected override IType ConstructInternalEntity(IType[] arguments)
		{
			ExternalCallableType externalCallableType = _type as ExternalCallableType;
			if (null != externalCallableType)
			{
				return new GenericConstructedCallableType(externalCallableType, arguments);
			}
			return new GenericConstructedType(_type, arguments);
		}
	}
}
