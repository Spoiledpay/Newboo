using System;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public abstract class GenericMapping : TypeMapper
	{
		private IDictionary<IGenericParameter, IType> _map = new Dictionary<IGenericParameter, IType>();

		private IDictionary<IMember, IMember> _memberCache = new Dictionary<IMember, IMember>();

		private IEntity _constructedOwner = null;

		private IEntity _genericSource = null;

		public GenericMapping(IType constructedType, IType[] arguments)
			: this(constructedType.ConstructedInfo.GenericDefinition.GenericInfo.GenericParameters, arguments)
		{
			_constructedOwner = constructedType;
			_genericSource = constructedType.ConstructedInfo.GenericDefinition;
		}

		public GenericMapping(IMethod constructedMethod, IType[] arguments)
			: this(constructedMethod.ConstructedInfo.GenericDefinition.GenericInfo.GenericParameters, arguments)
		{
			_constructedOwner = constructedMethod;
			_genericSource = constructedMethod.ConstructedInfo.GenericDefinition;
		}

		protected GenericMapping(IGenericParameter[] parameters, IType[] arguments)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				_map.Add(parameters[i], arguments[i]);
			}
		}

		public override IType MapType(IType sourceType)
		{
			if (sourceType == _genericSource)
			{
				return _constructedOwner as IType;
			}
			IGenericParameter genericParameter = sourceType as IGenericParameter;
			if (genericParameter != null)
			{
				if (_map.ContainsKey(genericParameter))
				{
					return _map[genericParameter];
				}
				return GenericsServices.GetGenericParameters(Map(genericParameter.DeclaringEntity))[genericParameter.GenericParameterPosition];
			}
			return base.MapType(sourceType);
		}

		public IEntity Map(IEntity source)
		{
			if (source == null)
			{
				return null;
			}
			if (source == _genericSource)
			{
				return _constructedOwner;
			}
			Ambiguous ambiguous = source as Ambiguous;
			if (ambiguous != null)
			{
				return MapAmbiguousEntity(ambiguous);
			}
			IMember member = source as IMember;
			if (member != null)
			{
				return MapMember(member);
			}
			IType type = source as IType;
			if (type != null)
			{
				return MapType(type);
			}
			return source;
		}

		private IMember MapMember(IMember source)
		{
			if (_memberCache.ContainsKey(source))
			{
				return _memberCache[source];
			}
			if (source.DeclaringType == _genericSource)
			{
				return CacheMember(source, CreateMappedMember(source));
			}
			IType declaringType = source.DeclaringType;
			if (declaringType.ConstructedInfo != null)
			{
				source = declaringType.ConstructedInfo.UnMap(source);
			}
			IType type = MapType(declaringType);
			if (type.ConstructedInfo != null)
			{
				return type.ConstructedInfo.Map(source);
			}
			return source;
		}

		protected abstract IMember CreateMappedMember(IMember source);

		public IConstructor Map(IConstructor source)
		{
			return (IConstructor)Map((IEntity)source);
		}

		public IMethod Map(IMethod source)
		{
			return (IMethod)Map((IEntity)source);
		}

		public IField Map(IField source)
		{
			return (IField)Map((IEntity)source);
		}

		public IProperty Map(IProperty source)
		{
			return (IProperty)Map((IEntity)source);
		}

		public IEvent Map(IEvent source)
		{
			return (IEvent)Map((IEntity)source);
		}

		internal IGenericParameter MapGenericParameter(IGenericParameter source)
		{
			return Cache(source, new GenericMappedTypeParameter(base.TypeSystemServices, source, this));
		}

		private IEntity MapAmbiguousEntity(Ambiguous source)
		{
			return new Ambiguous(Array.ConvertAll(source.Entities, Map));
		}

		public virtual IMember UnMap(IMember mapped)
		{
			foreach (KeyValuePair<IMember, IMember> item in _memberCache)
			{
				if (item.Value == mapped)
				{
					return item.Key;
				}
			}
			return null;
		}

		private IMember CacheMember(IMember source, IMember mapped)
		{
			_memberCache[source] = mapped;
			return mapped;
		}
	}
}
