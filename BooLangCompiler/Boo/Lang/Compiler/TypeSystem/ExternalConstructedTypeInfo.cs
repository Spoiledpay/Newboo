using System;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalConstructedTypeInfo : IConstructedTypeInfo, IGenericArgumentsProvider
	{
		private ExternalType _type;

		private IReflectionTypeSystemProvider _tss;

		private IType[] _arguments = null;

		private GenericMapping _mapping = null;

		protected GenericMapping GenericMapping
		{
			get
			{
				if (_mapping == null)
				{
					_mapping = new ExternalGenericMapping(_type, GenericArguments);
				}
				return _mapping;
			}
		}

		public IType GenericDefinition => _tss.Map(_type.ActualType.GetGenericTypeDefinition());

		public IType[] GenericArguments
		{
			get
			{
				if (_arguments == null)
				{
					_arguments = Array.ConvertAll(_type.ActualType.GetGenericArguments(), _tss.Map);
				}
				return _arguments;
			}
		}

		public bool FullyConstructed => !_type.ActualType.ContainsGenericParameters;

		public ExternalConstructedTypeInfo(IReflectionTypeSystemProvider tss, ExternalType type)
		{
			_type = type;
			_tss = tss;
		}

		public IMember UnMap(IMember mapped)
		{
			return GenericMapping.UnMap(mapped);
		}

		public IType Map(IType type)
		{
			if (type == GenericDefinition)
			{
				return _type;
			}
			return GenericMapping.MapType(type);
		}

		public IMember Map(IMember member)
		{
			return (IMember)GenericMapping.Map(member);
		}
	}
}
