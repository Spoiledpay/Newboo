#define TRACE
using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.MetaProgramming;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	public class MacroCompiler
	{
		private static readonly object CachedTypeAnnotation = new object();

		private EnvironmentProvision<CompilerContext> _context = default(EnvironmentProvision<CompilerContext>);

		protected CompilerContext Context => _context;

		public virtual bool AlreadyCompiled(TypeDefinition node)
		{
			return node.ContainsAnnotation(CachedTypeAnnotation);
		}

		public virtual Type Compile(TypeDefinition node)
		{
			Type type = CachedType(node);
			if (type != null)
			{
				return type;
			}
			if (AlreadyCompiled(node))
			{
				return null;
			}
			Type type2 = (IsNestedMacro(node) ? CompileNestedMacro(node) : CompileRegularMacro(node));
			CacheType(node, type2);
			return type2;
		}

		private static bool IsNestedMacro(TypeDefinition node)
		{
			return node.DeclaringType is ClassDefinition;
		}

		private Type CompileNestedMacro(TypeDefinition node)
		{
			Type type = Compile(node.DeclaringType);
			if (type == null)
			{
				return null;
			}
			return type.GetNestedType(node.Name);
		}

		private Type CompileRegularMacro(TypeDefinition node)
		{
			TraceInfo("Compiling macro '{0}'", node);
			CompilerContext compilerContext = Compilation.compile_(CompileUnitFor(node), Context.Parameters.References.ToArray());
			if (compilerContext.Errors.Count == 0)
			{
				TraceInfo("Macro '{0}' successfully compiled to '{1}'", node, compilerContext.GeneratedAssembly);
				return compilerContext.GeneratedAssembly.GetType(node.FullName);
			}
			Context.Errors.Extend(compilerContext.Errors);
			Context.Warnings.Extend(compilerContext.Warnings);
			return null;
		}

		private void TraceInfo(string format, params object[] args)
		{
			Context.TraceInfo(format, args);
		}

		protected CompileUnit CompileUnitFor(TypeDefinition node)
		{
			CompileUnit compileUnit = new CompileUnit();
			GetModuleFor(compileUnit, node);
			return compileUnit;
		}

		private void GetModuleFor(CompileUnit unit, TypeDefinition node)
		{
			unit.Modules.Add(ModuleFor(node));
			CollectModulesForBaseTypes(unit, node);
		}

		private void CollectModulesForBaseTypes(CompileUnit unit, TypeDefinition node)
		{
			foreach (TypeReference baseType in node.BaseTypes)
			{
				InternalClass internalClass = baseType.Entity as InternalClass;
				if (internalClass != null)
				{
					GetModuleFor(unit, internalClass.TypeDefinition);
				}
			}
		}

		private static Module ModuleFor(TypeDefinition node)
		{
			Module module = new Module();
			module.Namespace = SafeCleanClone(node.EnclosingModule.Namespace);
			module.Name = node.Name;
			Module module2 = module;
			foreach (Import import in node.EnclosingModule.Imports)
			{
				module2.Imports.Add(import.CleanClone());
			}
			module2.Members.Add(node.CleanClone());
			return module2;
		}

		private static T SafeCleanClone<T>(T node) where T : Node
		{
			return (node != null) ? ((T)node.CleanClone()) : null;
		}

		protected static void CacheType(TypeDefinition node, Type type)
		{
			node[CachedTypeAnnotation] = type;
		}

		protected static Type CachedType(TypeDefinition node)
		{
			return node[CachedTypeAnnotation] as Type;
		}
	}
}
