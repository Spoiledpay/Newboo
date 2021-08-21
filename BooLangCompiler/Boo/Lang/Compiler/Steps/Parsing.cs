using System;
using System.IO;
using System.Reflection;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class Parsing : ICompilerStep, ICompilerComponent, IDisposable
	{
		private ICompilerStep _parser;

		private static Assembly _parserAssembly;

		private ICompilerStep Parser => _parser ?? (_parser = NewParserStep());

		private static Type ConfiguredParserType
		{
			get
			{
				string name = (My<CompilerParameters>.Instance.WhiteSpaceAgnostic ? "Boo.Lang.Parser.WSABooParsingStep" : "Boo.Lang.Parser.BooParsingStep");
				return ParserAssembly().GetType(name, throwOnError: true);
			}
		}

		public void Initialize(CompilerContext context)
		{
			Parser.Initialize(context);
		}

		public void Run()
		{
			Parser.Run();
		}

		public void Dispose()
		{
			if (_parser != null)
			{
				_parser.Dispose();
				_parser = null;
			}
		}

		private static ICompilerStep NewParserStep()
		{
			return (ICompilerStep)Activator.CreateInstance(ConfiguredParserType);
		}

		private static Assembly ParserAssembly()
		{
			return _parserAssembly ?? (_parserAssembly = FindParserAssembly());
		}

		private static Assembly FindParserAssembly()
		{
			string text = Permissions.WithDiscoveryPermission(() => ThisAssembly().Location) ?? "";
			if (string.IsNullOrEmpty(text))
			{
				return LoadParserAssemblyByName();
			}
			string text2 = (text.EndsWith("Boo.Lang.Compiler.dll", StringComparison.OrdinalIgnoreCase) ? (text.Substring(0, text.Length - "Boo.Lang.Compiler.dll".Length) + "Boo.Lang.Parser.dll") : "");
			return File.Exists(text2) ? Assembly.LoadFrom(text2) : LoadParserAssemblyByName();
		}

		private static Assembly LoadParserAssemblyByName()
		{
			return Assembly.Load(ThisAssembly().FullName.Replace("Boo.Lang.Compiler", "Boo.Lang.Parser"));
		}

		private static Assembly ThisAssembly()
		{
			return typeof(Parsing).Assembly;
		}
	}
}
