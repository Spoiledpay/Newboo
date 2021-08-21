using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class InitializeNameResolutionService : AbstractCompilerStep
	{
		public override void Run()
		{
			EnsureModulesImportEnclosingNamespace();
			ResolveImportAssemblyReferences();
		}

		private void EnsureModulesImportEnclosingNamespace()
		{
			foreach (Module module in base.CompileUnit.Modules)
			{
				if (module.Namespace != null)
				{
					string moduleNamespace = module.Namespace.Name;
					if (!module.Imports.Contains((Import candidate) => candidate.Namespace == moduleNamespace))
					{
						module.Imports.Add(new Import(module.Namespace.LexicalInfo, moduleNamespace));
					}
				}
			}
		}

		private void ResolveImportAssemblyReferences()
		{
			foreach (Module module in base.CompileUnit.Modules)
			{
				ImportCollection imports = module.Imports;
				ResolveAssemblyReferences(imports);
			}
		}

		private void ResolveAssemblyReferences(ImportCollection imports)
		{
			Import[] array = imports.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Import import = array[i];
				ReferenceExpression assemblyReference = import.AssemblyReference;
				if (null != assemblyReference)
				{
					try
					{
						assemblyReference.Entity = ResolveAssemblyReference(assemblyReference);
					}
					catch (Exception error)
					{
						base.Errors.Add(CompilerErrorFactory.UnableToLoadAssembly(assemblyReference, assemblyReference.Name, error));
						imports.RemoveAt(i);
					}
				}
			}
		}

		private ICompileUnit ResolveAssemblyReference(ReferenceExpression reference)
		{
			ICompileUnit compileUnit = base.Parameters.FindAssembly(reference.Name);
			if (null != compileUnit)
			{
				return compileUnit;
			}
			ICompileUnit compileUnit2 = base.Parameters.LoadAssembly(reference.Name);
			if (null != compileUnit2)
			{
				base.Parameters.References.Add(compileUnit2);
				return compileUnit2;
			}
			return null;
		}
	}
}
