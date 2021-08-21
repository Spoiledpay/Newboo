using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Boo.Lang.Compiler;

namespace booc
{
	public class App
	{
		[STAThread]
		public static int Main(string[] args)
		{
			if (((IList)args).Contains((object)"-utf8"))
			{
				return RunInUtf8Mode(args);
			}
			return AppRun(args);
		}

		private static int AppRun(string[] args)
		{
			return new App().Run(args);
		}

		private static int RunInUtf8Mode(string[] args)
		{
			using StreamWriter streamWriter = new StreamWriter(Console.OpenStandardError(), Encoding.UTF8);
			streamWriter.WriteLine();
			Console.SetError(streamWriter);
			return AppRun(args);
		}

		public int Run(string[] args)
		{
			int result = 127;
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
			CheckBooCompiler();
			CompilerParameters compilerParameters = new CompilerParameters(loadDefaultReferences: false);
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				CommandLineParser.ParseInto(compilerParameters, args);
				if (0 == compilerParameters.Input.Count)
				{
					throw new ApplicationException("No inputs specified");
				}
				BooCompiler booCompiler = new BooCompiler(compilerParameters);
				stopwatch.Stop();
				Stopwatch stopwatch2 = Stopwatch.StartNew();
				CompilerContext compilerContext = booCompiler.Run();
				stopwatch2.Stop();
				if (compilerContext.Warnings.Count > 0)
				{
					Console.Error.WriteLine(compilerContext.Warnings);
					Console.Error.WriteLine("{0} warning(s).", compilerContext.Warnings.Count);
				}
				if (compilerContext.Errors.Count == 0)
				{
					result = 0;
				}
				else
				{
					foreach (CompilerError error in compilerContext.Errors)
					{
						Console.Error.WriteLine(error.ToString(compilerParameters.TraceInfo));
					}
					Console.Error.WriteLine("{0} error(s).", compilerContext.Errors.Count);
				}
				if (compilerParameters.TraceWarning)
				{
					Console.Error.WriteLine("{0} module(s) processed in {1}ms after {2}ms of environment setup.", compilerParameters.Input.Count, stopwatch2.ElapsedMilliseconds, stopwatch.ElapsedMilliseconds);
				}
			}
			catch (Exception ex)
			{
				object arg = (compilerParameters.TraceWarning ? ((object)ex) : ((object)ex.Message));
				Console.Error.WriteLine($"Fatal error: {arg}.");
			}
			return result;
		}

		private static void CheckBooCompiler()
		{
			string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Boo.Lang.Compiler.dll");
			if (!File.Exists(text))
			{
				return;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.FullName.StartsWith("Boo.Lang.Compiler"))
				{
					if (string.Compare(assembly.Location, text, ignoreCase: true) != 0)
					{
						string value = $"WARNING: booc is not using the Boo.Lang.Compiler.dll next to booc.exe.  Using '{assembly.Location}' instead of '{text}'.  You may need to remove boo dlls from the GAC using gacutil or Mscorcfg.";
						Console.Error.WriteLine(value);
					}
					break;
				}
			}
		}

		private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string text = args.Name.Split(',')[0];
			string path = Path.Combine(Environment.CurrentDirectory, text + ".dll");
			if (File.Exists(path))
			{
				return Assembly.LoadFile(path);
			}
			return null;
		}
	}
}
