using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public abstract class TypeMapper
	{
		protected IDictionary<IEntity, IEntity> _cache = new Dictionary<IEntity, IEntity>();

		protected TypeSystemServices TypeSystemServices => My<TypeSystemServices>.Instance;

		protected TEntity Cache<TEntity>(TEntity source, TEntity mapped) where TEntity : IEntity
		{
			_cache[source] = mapped;
			return mapped;
		}

		public virtual IType MapType(IType sourceType)
		{
			if (sourceType == null)
			{
				return null;
			}
			if (_cache.ContainsKey(sourceType))
			{
				return (IType)_cache[sourceType];
			}
			if (sourceType.IsByRef)
			{
				return MapByRefType(sourceType);
			}
			if (sourceType.ConstructedInfo != null)
			{
				return MapConstructedType(sourceType);
			}
			IArrayType arrayType = sourceType as IArrayType;
			if (arrayType != null)
			{
				return MapArrayType(arrayType);
			}
			AnonymousCallableType anonymousCallableType = sourceType as AnonymousCallableType;
			return (anonymousCallableType != null) ? MapCallableType(anonymousCallableType) : sourceType;
		}

		public virtual IType MapByRefType(IType sourceType)
		{
			return MapType(sourceType.ElementType);
		}

		public virtual IType MapArrayType(IArrayType sourceType)
		{
			IType type = MapType(sourceType.ElementType);
			return type.MakeArrayType(sourceType.Rank);
		}

		public virtual IType MapCallableType(AnonymousCallableType sourceType)
		{
			CallableSignature signature = sourceType.GetSignature();
			IType returnType = MapType(signature.ReturnType);
			IParameter[] parameters = MapParameters(signature.Parameters);
			CallableSignature signature2 = new CallableSignature(parameters, returnType, signature.AcceptVarArgs);
			return TypeSystemServices.GetCallableType(signature2);
		}

		public virtual IType MapConstructedType(IType sourceType)
		{
			IType type = MapType(sourceType.ConstructedInfo.GenericDefinition);
			IType[] arguments = Array.ConvertAll(sourceType.ConstructedInfo.GenericArguments, MapType);
			return type.GenericInfo.ConstructType(arguments);
		}

		internal IParameter[] MapParameters(IParameter[] parameters)
		{
			return Array.ConvertAll(parameters, MapParameter);
		}

		internal IParameter MapParameter(IParameter parameter)
		{
			if (_cache.ContainsKey(parameter))
			{
				return _cache[parameter] as IParameter;
			}
			return Cache(parameter, new MappedParameter(parameter, MapType(parameter.Type)));
		}
	}
}
