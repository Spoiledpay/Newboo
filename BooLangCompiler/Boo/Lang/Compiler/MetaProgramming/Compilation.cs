using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.MetaProgramming
{
	[CompilerGlobalScope]
	public sealed class Compilation
	{
		public static Type compile(TypeDefinition klass, params Assembly[] references)
		{
			CompilerContext compilerContext = compile_(klass, references);
			AssertNoErrors(compilerContext);
			return compilerContext.GeneratedAssembly.GetType(klass.Name);
		}

		public static CompilerContext compile_(TypeDefinition klass, params Assembly[] references)
		{
			return compile_(CreateCompileUnit(klass), references);
		}

		public static Assembly compile(Boo.Lang.Compiler.Ast.Module module, params Assembly[] references)
		{
			return compile(new CompileUnit(module), references);
		}

		public static CompilerContext compile_(Boo.Lang.Compiler.Ast.Module module, params Assembly[] references)
		{
			return compile_(new CompileUnit(module), references);
		}

		public static Assembly compile(CompileUnit unit, params Assembly[] references)
		{
			CompilerContext compilerContext = compile_(unit, references);
			AssertNoErrors(compilerContext);
			return compilerContext.GeneratedAssembly;
		}

		private static void AssertNoErrors(CompilerContext result)
		{
			if (result.Errors.Count > 0)
			{
				throw new CompilationErrorsException(result.Errors);
			}
		}

		public static CompilerContext compile_(CompileUnit unit, Assembly[] references)
		{
			BooCompiler booCompiler = NewCompiler();
			foreach (Assembly assembly in references)
			{
				booCompiler.Parameters.References.Add(assembly);
			}
			return booCompiler.Run(unit);
		}

		public static CompilerContext compile_(CompileUnit unit, params ICompileUnit[] references)
		{
			return NewCompilerWithReferences(references).Run(unit);
		}

		private static BooCompiler NewCompilerWithReferences(IEnumerable<ICompileUnit> references)
		{
			BooCompiler booCompiler = NewCompiler(loadDefaultReferences: false);
			booCompiler.Parameters.References.AddAll(references);
			return booCompiler;
		}

		private static BooCompiler NewCompiler()
		{
			return NewCompiler(loadDefaultReferences: true);
		}

		private static BooCompiler NewCompiler(bool loadDefaultReferences)
		{
			BooCompiler booCompiler = new BooCompiler(new CompilerParameters(loadDefaultReferences));
			booCompiler.Parameters.OutputType = CompilerOutputType.Auto;
			booCompiler.Parameters.Pipeline = new CompileToMemory();
			return booCompiler;
		}

		private static CompileUnit CreateCompileUnit(TypeDefinition klass)
		{
			return new CompileUnit(CreateModule(klass));
		}

		private static Boo.Lang.Compiler.Ast.Module CreateModule(TypeDefinition klass)
		{
			Boo.Lang.Compiler.Ast.Module module = new Boo.Lang.Compiler.Ast.Module();
			module.Name = klass.Name;
			module.Members.Add(klass);
			return module;
		}
	}
}
