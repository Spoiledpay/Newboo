#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class PEVerify : AbstractCompilerStep
	{
		public override void Run()
		{
			if (base.Errors.Count > 0)
			{
				return;
			}
			string text = null;
			string empty = string.Empty;
			int platform = (int)Environment.OSVersion.Platform;
			if (platform == 4 || platform == 128)
			{
				text = "pedump";
				empty = "--verify all \"" + base.Context.GeneratedAssemblyFileName + "\"";
			}
			else
			{
				text = "peverify.exe";
				empty = "\"" + base.Context.GeneratedAssemblyFileName + "\"";
			}
			try
			{
				Process process = StartProcess(Path.GetDirectoryName(base.Parameters.OutputAssembly), text, empty);
				process.WaitForExit();
				if (0 != process.ExitCode)
				{
					base.Errors.Add(new CompilerError(LexicalInfo.Empty, process.StandardOutput.ReadToEnd()));
				}
			}
			catch (Exception ex)
			{
				base.Warnings.Add(new CompilerWarning("Could not start " + text));
				base.Context.TraceWarning("Could not start " + text + " : " + ex.Message);
			}
		}

		public Process StartProcess(string workingdir, string filename, string arguments)
		{
			Process process = new Process();
			process.StartInfo.Arguments = arguments;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = filename;
			Process process2 = process;
			if (Type.GetType("Mono.Runtime") != null)
			{
				process2.StartInfo.EnvironmentVariables["MONO_PATH"] = workingdir;
				process2.StartInfo.StandardOutputEncoding = Encoding.UTF8;
				process2.StartInfo.StandardErrorEncoding = Encoding.UTF8;
			}
			process2.Start();
			return process2;
		}
	}
}
