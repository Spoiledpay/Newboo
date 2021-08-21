using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalConstructedMethodInfo : IConstructedMethodInfo, IGenericArgumentsProvider
	{
		private ExternalMethod _method;

		private IReflectionTypeSystemProvider _tss;

		private IType[] _arguments = null;

		public IMethod GenericDefinition => _tss.Map(((MethodInfo)_method.MethodInfo).GetGenericMethodDefinition());

		public IType[] GenericArguments
		{
			get
			{
				if (_arguments == null)
				{
					_arguments = Array.ConvertAll(_method.MethodInfo.GetGenericArguments(), _tss.Map);
				}
				return _arguments;
			}
		}

		public bool FullyConstructed => !_method.MethodInfo.ContainsGenericParameters;

		public ExternalConstructedMethodInfo(IReflectionTypeSystemProvider tss, ExternalMethod method)
		{
			_method = method;
			_tss = tss;
		}
	}
}
