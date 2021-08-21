using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	public class ReflectionTypeSystemProvider : IReflectionTypeSystemProvider
	{
		private sealed class ObjectTypeImpl : ExternalType
		{
			internal ObjectTypeImpl(IReflectionTypeSystemProvider provider)
				: base(provider, Types.Object)
			{
			}

			public override bool IsAssignableFrom(IType other)
			{
				ExternalType externalType = other as ExternalType;
				return externalType == null || externalType.ActualType != Types.Void;
			}
		}

		private sealed class VoidTypeImpl : ExternalType
		{
			internal VoidTypeImpl(IReflectionTypeSystemProvider provider)
				: base(provider, Types.Void)
			{
			}

			public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
			{
				return false;
			}

			public override bool IsSubclassOf(IType other)
			{
				return false;
			}

			public override bool IsAssignableFrom(IType other)
			{
				return false;
			}
		}

		private readonly MemoizedFunction<Assembly, AssemblyReference> _referenceCache;

		public ReflectionTypeSystemProvider()
		{
			_referenceCache = new MemoizedFunction<Assembly, AssemblyReference>(AssemblyEqualityComparer.Default, CreateReference);
			MapTo(typeof(object), new ObjectTypeImpl(this));
			MapTo(typeof(Builtins.duck), new ObjectTypeImpl(this));
			MapTo(typeof(void), new VoidTypeImpl(this));
		}

		protected void MapTo(Type type, IType entity)
		{
			AssemblyReferenceFor(type.Assembly).MapTo(type, entity);
		}

		private ReflectionTypeSystemProvider(MemoizedFunction<Assembly, AssemblyReference> referenceCache)
		{
			_referenceCache = referenceCache;
		}

		public IAssemblyReference ForAssembly(Assembly assembly)
		{
			return AssemblyReferenceFor(assembly);
		}

		private AssemblyReference AssemblyReferenceFor(Assembly assembly)
		{
			return _referenceCache.Invoke(assembly);
		}

		private AssemblyReference CreateReference(Assembly assembly)
		{
			return new AssemblyReference(this, assembly);
		}

		public IType Map(Type type)
		{
			return AssemblyReferenceFor(type.Assembly).Map(type);
		}

		public IMethod Map(MethodInfo method)
		{
			return (IMethod)MapMember(method);
		}

		public IConstructor Map(ConstructorInfo ctor)
		{
			return (IConstructor)MapMember(ctor);
		}

		public IEntity Map(MemberInfo mi)
		{
			return MapMember(mi);
		}

		public IParameter[] Map(ParameterInfo[] parameters)
		{
			IParameter[] array = new IParameter[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = new ExternalParameter(this, parameters[i]);
			}
			return array;
		}

		private IEntity MapMember(MemberInfo mi)
		{
			return AssemblyReferenceFor(mi.DeclaringType.Assembly).MapMember(mi);
		}

		public virtual IReflectionTypeSystemProvider Clone()
		{
			return new ReflectionTypeSystemProvider(_referenceCache.Clone());
		}

		public virtual IType CreateEntityForRegularType(Type type)
		{
			return new ExternalType(this, type);
		}

		public virtual IType CreateEntityForCallableType(Type type)
		{
			return new ExternalCallableType(this, type);
		}

		public IEntity Map(MemberInfo[] members)
		{
			switch (members.Length)
			{
			case 0:
				return null;
			case 1:
				return Map(members[0]);
			default:
			{
				IEntity[] array = new IEntity[members.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Map(members[i]);
				}
				return new Ambiguous(array);
			}
			}
		}
	}
}
