using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	internal sealed class ReflectionNamespaceBuilder
	{
		private Assembly _assembly;

		private ReflectionNamespace _root;

		public ReflectionNamespaceBuilder(IReflectionTypeSystemProvider provider, Assembly assembly)
		{
			_root = new ReflectionNamespace(provider);
			_assembly = assembly;
		}

		public INamespace Build()
		{
			try
			{
				CatalogPublicTypes(_assembly.GetTypes());
			}
			catch (ReflectionTypeLoadException ex)
			{
				My<CompilerWarningCollection>.Instance.Add(CompilerWarningFactory.CustomWarning(string.Concat("Could not load types from '", _assembly, "': ", Builtins.join(ex.LoaderExceptions, "\n"))));
			}
			return _root;
		}

		private void CatalogPublicTypes(IEnumerable<Type> types)
		{
			string text = "!!not a namespace!!";
			ReflectionNamespace reflectionNamespace = null;
			foreach (Type type in types)
			{
				if (type.IsPublic)
				{
					string text2 = type.Namespace ?? string.Empty;
					if (text2 != text)
					{
						text = text2;
						reflectionNamespace = GetNamespace(text2);
						reflectionNamespace.Add(type);
					}
					else
					{
						reflectionNamespace.Add(type);
					}
				}
			}
		}

		public ReflectionNamespace GetNamespace(string ns)
		{
			if (ns.Length == 0)
			{
				return _root;
			}
			string[] array = ns.Split('.');
			ReflectionNamespace reflectionNamespace = _root;
			string[] array2 = array;
			foreach (string name in array2)
			{
				reflectionNamespace = reflectionNamespace.Produce(name);
			}
			return reflectionNamespace;
		}
	}
}
