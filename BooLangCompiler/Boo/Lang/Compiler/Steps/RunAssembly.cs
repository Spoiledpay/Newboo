using System;
using System.Reflection;

namespace Boo.Lang.Compiler.Steps
{
	public class RunAssembly : AbstractCompilerStep
	{
		public override void Run()
		{
			if (base.Errors.Count > 0 || CompilerOutputType.Library == base.Parameters.OutputType || base.Context.GeneratedAssembly == null)
			{
				return;
			}
			AppDomain.CurrentDomain.AssemblyResolve += ResolveGeneratedAssembly;
			try
			{
				MethodInfo entryPoint = base.Context.GeneratedAssembly.EntryPoint;
				if (entryPoint.GetParameters().Length == 1)
				{
					entryPoint.Invoke(null, new object[1] { new string[0] });
				}
				else
				{
					entryPoint.Invoke(null, null);
				}
			}
			finally
			{
				AppDomain.CurrentDomain.AssemblyResolve -= ResolveGeneratedAssembly;
			}
		}

		private Assembly ResolveGeneratedAssembly(object sender, ResolveEventArgs args)
		{
			return (args.Name == base.Context.GeneratedAssembly.FullName) ? base.Context.GeneratedAssembly : null;
		}
	}
}
