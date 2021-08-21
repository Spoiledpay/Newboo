using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem
{
	public abstract class AbstractExternalGenericInfo<T> where T : IEntity
	{
		protected IReflectionTypeSystemProvider _provider;

		private IGenericParameter[] _parameters;

		private Dictionary<IType[], T> _instances = new Dictionary<IType[], T>(ArrayEqualityComparer<IType>.Default);

		public IGenericParameter[] GenericParameters
		{
			get
			{
				if (null == _parameters)
				{
					_parameters = Array.ConvertAll(GetActualGenericParameters(), (Type t) => (ExternalGenericParameter)_provider.Map(t));
				}
				return _parameters;
			}
		}

		protected AbstractExternalGenericInfo(IReflectionTypeSystemProvider provider)
		{
			_provider = provider;
		}

		protected T ConstructEntity(IType[] arguments)
		{
			if (Array.TrueForAll(arguments, IsExternal))
			{
				Type[] arguments2 = Array.ConvertAll(arguments, GetSystemType);
				return ConstructExternalEntity(arguments2);
			}
			if (_instances.ContainsKey(arguments))
			{
				return _instances[arguments];
			}
			T val = ConstructInternalEntity(arguments);
			_instances.Add(arguments, val);
			return val;
		}

		protected abstract Type[] GetActualGenericParameters();

		protected abstract T ConstructInternalEntity(IType[] arguments);

		protected abstract T ConstructExternalEntity(Type[] arguments);

		private bool IsExternal(IType type)
		{
			if (type is ExternalType)
			{
				return true;
			}
			if (type is ArrayType)
			{
				return IsExternal(type.ElementType);
			}
			return false;
		}

		private Type GetSystemType(IType type)
		{
			ExternalType externalType = type as ExternalType;
			if (null != externalType)
			{
				return externalType.ActualType;
			}
			ArrayType arrayType = type as ArrayType;
			if (arrayType != null)
			{
				Type systemType = GetSystemType(arrayType.ElementType);
				int rank = arrayType.Rank;
				return (rank == 1) ? systemType.MakeArrayType() : systemType.MakeArrayType(rank);
			}
			return null;
		}
	}
}
