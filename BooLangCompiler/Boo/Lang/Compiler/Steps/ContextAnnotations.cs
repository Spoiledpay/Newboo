using System;
using System.Reflection.Emit;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class ContextAnnotations
	{
		private static readonly object EntryPointKey = new object();

		private static readonly object AssemblyBuilderKey = new object();

		public static Method GetEntryPoint(CompilerContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			return (Method)context.Properties[EntryPointKey];
		}

		public static void SetEntryPoint(CompilerContext context, Method method)
		{
			if (null == method)
			{
				throw new ArgumentNullException("method");
			}
			Method entryPoint = GetEntryPoint(context);
			if (null != entryPoint)
			{
				throw CompilerErrorFactory.MoreThanOneEntryPoint(method);
			}
			context.Properties[EntryPointKey] = method;
		}

		public static AssemblyBuilder GetAssemblyBuilder(CompilerContext context)
		{
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)context.Properties[AssemblyBuilderKey];
			if (null == assemblyBuilder)
			{
				throw CompilerErrorFactory.InvalidAssemblySetUp(context.CompileUnit);
			}
			return assemblyBuilder;
		}

		public static void SetAssemblyBuilder(CompilerContext context, AssemblyBuilder builder)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			if (null == builder)
			{
				throw new ArgumentNullException("builder");
			}
			context.Properties[AssemblyBuilderKey] = builder;
		}

		private ContextAnnotations()
		{
		}
	}
}
