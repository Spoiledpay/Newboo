using System;
using System.Reflection;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	internal class AssemblyReference : IAssemblyReference, ICompileUnit, IEntity, IEquatable<AssemblyReference>
	{
		private readonly Assembly _assembly;

		private readonly ReflectionTypeSystemProvider _provider;

		private INamespace _rootNamespace;

		private readonly MemoizedFunction<Type, IType> _typeEntityCache;

		private readonly MemoizedFunction<MemberInfo, IEntity> _memberCache;

		private string _name;

		public string Name => _name ?? (_name = new AssemblyName(_assembly.FullName).Name);

		public string FullName => _assembly.FullName;

		public EntityType EntityType => EntityType.Assembly;

		public Assembly Assembly => _assembly;

		public INamespace RootNamespace
		{
			get
			{
				if (_rootNamespace != null)
				{
					return _rootNamespace;
				}
				return _rootNamespace = CreateRootNamespace();
			}
		}

		internal AssemblyReference(ReflectionTypeSystemProvider provider, Assembly assembly)
		{
			if (null == assembly)
			{
				throw new ArgumentNullException("assembly");
			}
			_provider = provider;
			_assembly = assembly;
			_typeEntityCache = new MemoizedFunction<Type, IType>(NewType);
			_memberCache = new MemoizedFunction<MemberInfo, IEntity>(NewEntityForMember);
		}

		private INamespace CreateRootNamespace()
		{
			return new ReflectionNamespaceBuilder(_provider, _assembly).Build();
		}

		public override bool Equals(object other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			AssemblyReference other2 = other as AssemblyReference;
			return Equals(other2);
		}

		public bool Equals(AssemblyReference other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			return IsReferencing(other._assembly);
		}

		private bool IsReferencing(Assembly assembly)
		{
			return AssemblyEqualityComparer.Default.Equals(_assembly, assembly);
		}

		public override int GetHashCode()
		{
			return AssemblyEqualityComparer.Default.GetHashCode(_assembly);
		}

		public override string ToString()
		{
			return _assembly.FullName;
		}

		public IType Map(Type type)
		{
			AssertAssembly(type);
			return _typeEntityCache.Invoke(type);
		}

		public IEntity MapMember(MemberInfo mi)
		{
			AssertAssembly(mi);
			if (mi.MemberType == MemberTypes.NestedType)
			{
				return Map((Type)mi);
			}
			return _memberCache.Invoke(mi);
		}

		private void AssertAssembly(MemberInfo member)
		{
			if (!IsReferencing(member.Module.Assembly))
			{
				throw new ArgumentException($"{member} doesn't belong to assembly '{_assembly}'.");
			}
		}

		private IEntity NewEntityForMember(MemberInfo mi)
		{
			return mi.MemberType switch
			{
				MemberTypes.Method => new ExternalMethod(_provider, (MethodBase)mi), 
				MemberTypes.Constructor => new ExternalConstructor(_provider, (ConstructorInfo)mi), 
				MemberTypes.Field => new ExternalField(_provider, (FieldInfo)mi), 
				MemberTypes.Property => new ExternalProperty(_provider, (PropertyInfo)mi), 
				MemberTypes.Event => new ExternalEvent(_provider, (EventInfo)mi), 
				_ => throw new NotImplementedException(mi.ToString()), 
			};
		}

		public void MapTo(Type type, IType entity)
		{
			AssertAssembly(type);
			_typeEntityCache.Add(type, entity);
		}

		private IType NewType(Type type)
		{
			return type.IsArray ? Map(type.GetElementType()).MakeArrayType(type.GetArrayRank()) : CreateEntityForType(type);
		}

		private IType CreateEntityForType(Type type)
		{
			if (type.IsGenericParameter)
			{
				return new ExternalGenericParameter(_provider, type);
			}
			if (type.IsSubclassOf(Types.MulticastDelegate))
			{
				return _provider.CreateEntityForCallableType(type);
			}
			return _provider.CreateEntityForRegularType(type);
		}
	}
}
