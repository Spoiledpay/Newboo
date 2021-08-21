using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalGenericMethodInfo : AbstractExternalGenericInfo<IMethod>, IGenericMethodInfo, IGenericParametersProvider
	{
		private ExternalMethod _method;

		public ExternalGenericMethodInfo(IReflectionTypeSystemProvider provider, ExternalMethod method)
			: base(provider)
		{
			_method = method;
		}

		public IMethod ConstructMethod(IType[] arguments)
		{
			return ConstructEntity(arguments);
		}

		protected override Type[] GetActualGenericParameters()
		{
			return _method.MethodInfo.GetGenericArguments();
		}

		protected override IMethod ConstructExternalEntity(Type[] arguments)
		{
			return _provider.Map(((MethodInfo)_method.MethodInfo).MakeGenericMethod(arguments));
		}

		protected override IMethod ConstructInternalEntity(IType[] arguments)
		{
			return new GenericConstructedMethod(_method, arguments);
		}
	}
}
