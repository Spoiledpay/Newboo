using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler
{
	public class CompilerReferenceCollection : Set<ICompileUnit>
	{
		private readonly IReflectionTypeSystemProvider _provider;

		public IReflectionTypeSystemProvider Provider => _provider;

		public CompilerReferenceCollection(IReflectionTypeSystemProvider provider)
		{
			if (null == provider)
			{
				throw new ArgumentNullException("provider");
			}
			_provider = provider;
		}

		public void Add(Assembly assembly)
		{
			Add(_provider.ForAssembly(assembly));
		}

		public bool Contains(Assembly assembly)
		{
			return Contains(_provider.ForAssembly(assembly));
		}

		public void AddAll(IEnumerable<ICompileUnit> references)
		{
			foreach (ICompileUnit reference in references)
			{
				Add(reference);
			}
		}

		[Obsolete("Use AddAll")]
		public void Extend(IEnumerable assemblies)
		{
			foreach (Assembly assembly in assemblies)
			{
				Add(assembly);
			}
		}

		public ICompileUnit Find(string simpleName)
		{
			using (IEnumerator<ICompileUnit> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ICompileUnit current = enumerator.Current;
					if (current.Name == simpleName)
					{
						return current;
					}
				}
			}
			return null;
		}

		public void Remove(Assembly assembly)
		{
			Remove(_provider.ForAssembly(assembly));
		}
	}
}
