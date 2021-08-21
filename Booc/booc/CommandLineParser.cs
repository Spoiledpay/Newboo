#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Boo.Lang;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast.Visitors;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.Resources;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;
using Boo.Lang.Runtime;

namespace booc
{
	public class CommandLineParser
	{
		private class StepDebugger
		{
			private string _last;

			private Stopwatch _stopWatch;

			public void BeforeStep(object sender, CompilerStepEventArgs args)
			{
				_stopWatch = Stopwatch.StartNew();
			}

			public void AfterStep(object sender, CompilerStepEventArgs args)
			{
				_stopWatch.Stop();
				Console.WriteLine("********* {0} - {1} *********", args.Step, _stopWatch.Elapsed);
				StringWriter stringWriter = new StringWriter();
				args.Context.CompileUnit.Accept(new BooPrinterVisitor(stringWriter, BooPrinterVisitor.PrintOptions.PrintLocals));
				string text = stringWriter.ToString();
				if (text != _last)
				{
					Console.WriteLine(text);
				}
				else
				{
					Console.WriteLine("no changes");
				}
				_last = text;
			}
		}

		private readonly CompilerParameters _options;

		private readonly Set<string> _processedResponseFiles = new Set<string>();

		private readonly System.Collections.Generic.List<string> _references = new System.Collections.Generic.List<string>();

		private readonly System.Collections.Generic.List<string> _packages = new System.Collections.Generic.List<string>();

		private bool _noConfig;

		private string _pipelineName;

		private bool _debugSteps;

		private Stopwatch stepStopwatch;

		public static void ParseInto(CompilerParameters options, params string[] commandLine)
		{
			new CommandLineParser(commandLine, options);
		}

		private CommandLineParser(IEnumerable<string> args, CompilerParameters options)
		{
			_options = options;
			_options.GenerateInMemory = false;
			string[] enumerable = _options.LibPaths.ToArray();
			_options.LibPaths.Clear();
			Parse(args);
			_options.LibPaths.Extend(enumerable);
			if (_options.StdLib)
			{
				_options.LoadDefaultReferences();
			}
			else if (!_noConfig)
			{
				_references.Insert(0, "mscorlib");
			}
			LoadReferences();
			ConfigurePipeline();
			if (_options.TraceInfo)
			{
				_options.Pipeline.BeforeStep += OnBeforeStep;
				_options.Pipeline.AfterStep += OnAfterStep;
			}
		}

		private void Parse(IEnumerable<string> commandLine)
		{
			bool flag = false;
			System.Collections.Generic.List<string> list = ExpandResponseFiles(commandLine.Select((string s) => StripQuotes(s)));
			AddDefaultResponseFile(list);
			string attribute;
			foreach (string item3 in list)
			{
				if ("-" == item3)
				{
					_options.Input.Add(new StringInput("<stdin>", Consume(Console.In)));
				}
				else if (!IsFlag(item3))
				{
					_options.Input.Add(new FileInput(StripQuotes(item3)));
				}
				else
				{
					if ("-utf8" == item3)
					{
						continue;
					}
					switch (item3[1])
					{
					case 'h':
						if (item3 == "-help" || item3 == "-h")
						{
							Help();
						}
						break;
					case 'w':
						if (item3 == "-wsa")
						{
							_options.WhiteSpaceAgnostic = true;
						}
						else if (item3 == "-warnaserror")
						{
							_options.WarnAsError = true;
						}
						else if (item3.StartsWith("-warnaserror:"))
						{
							string text = ValueOf(item3);
							string[] array = text.Split(',');
							foreach (string code in array)
							{
								_options.EnableWarningAsError(code);
							}
						}
						else if (item3.StartsWith("-warn:"))
						{
							string text = ValueOf(item3);
							string[] array = text.Split(',');
							foreach (string code in array)
							{
								_options.EnableWarning(code);
							}
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					case 'v':
						_options.TraceLevel = TraceLevel.Warning;
						if (item3.Length > 2)
						{
							switch (item3.Substring(1))
							{
							case "vv":
								_options.TraceLevel = TraceLevel.Info;
								MonitorAppDomain();
								break;
							case "vvv":
								_options.TraceLevel = TraceLevel.Verbose;
								break;
							}
						}
						else
						{
							_options.TraceLevel = TraceLevel.Warning;
						}
						break;
					case 'r':
					{
						if (item3.IndexOf(":") > 2 && item3.Substring(1, 9) != "reference")
						{
							string text2 = item3.Substring(1, 8);
							if (text2 != null && text2 == "resource")
							{
								AddResource(ValueOf(item3));
							}
							else
							{
								InvalidOption(item3);
							}
							break;
						}
						string text4 = ValueOf(item3);
						string[] array = text4.Split(',');
						foreach (string item2 in array)
						{
							_references.Add(item2);
						}
						break;
					}
					case 'l':
					{
						string text2 = item3.Substring(1, 3);
						if (text2 != null && text2 == "lib")
						{
							ParseLib(item3);
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					}
					case 'n':
						switch (item3)
						{
						case "-nologo":
							flag = true;
							break;
						case "-noconfig":
							_noConfig = true;
							break;
						case "-nostdlib":
							_options.StdLib = false;
							break;
						case "-nowarn":
							_options.NoWarn = true;
							break;
						default:
							if (item3.StartsWith("-nowarn:"))
							{
								string text = ValueOf(item3);
								string[] array = text.Split(',');
								foreach (string code in array)
								{
									_options.DisableWarning(code);
								}
							}
							else
							{
								InvalidOption(item3);
							}
							break;
						}
						break;
					case 'o':
						_options.OutputAssembly = ValueOf(item3);
						break;
					case 't':
						switch (ValueOf(item3))
						{
						case "library":
							_options.OutputType = CompilerOutputType.Library;
							break;
						case "exe":
							_options.OutputType = CompilerOutputType.ConsoleApplication;
							break;
						case "winexe":
							_options.OutputType = CompilerOutputType.WindowsApplication;
							break;
						default:
							InvalidOption(item3);
							break;
						}
						break;
					case 'p':
						if (item3.StartsWith("-pkg:"))
						{
							string item = ValueOf(item3);
							_packages.Add(item);
						}
						else if (item3.StartsWith("-platform:"))
						{
							switch (ValueOf(item3).ToLowerInvariant())
							{
							case "x86":
								_options.Platform = "x86";
								break;
							case "x64":
								_options.Platform = "x64";
								break;
							case "itanium":
								_options.Platform = "itanium";
								break;
							default:
								InvalidOption(item3, "Valid platform types are: `anycpu', `x86', `x64' or `itanium'.");
								break;
							case "anycpu":
								break;
							}
						}
						else if (item3.StartsWith("-p:"))
						{
							_pipelineName = StripQuotes(item3.Substring(3));
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					case 'c':
						switch (item3.Substring(1))
						{
						case "checked":
						case "checked+":
							_options.Checked = true;
							break;
						case "checked-":
							_options.Checked = false;
							break;
						default:
						{
							string name = item3.Substring(3);
							Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(name);
							break;
						}
						}
						break;
					case 's':
						switch (item3.Substring(1, 6))
						{
						case "srcdir":
						{
							string path = StripQuotes(item3.Substring(8));
							AddFilesForPath(path, _options);
							break;
						}
						case "strict":
						case "strict+":
							_options.Strict = true;
							break;
						default:
							InvalidOption(item3);
							break;
						case "strict-":
							break;
						}
						break;
					case 'k':
						if (item3.Substring(1, 7) == "keyfile")
						{
							_options.KeyFile = StripQuotes(item3.Substring(9));
						}
						else if (item3.Substring(1, 12) == "keycontainer")
						{
							_options.KeyContainer = StripQuotes(item3.Substring(14));
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					case 'd':
						switch (item3.Substring(1))
						{
						case "debug":
						case "debug+":
							_options.Debug = true;
							break;
						case "debug-":
							_options.Debug = false;
							break;
						case "ducky":
							_options.Ducky = true;
							break;
						case "debug-steps":
							_debugSteps = true;
							break;
						case "delaysign":
							_options.DelaySign = true;
							break;
						default:
							if (item3.StartsWith("-d:") || item3.StartsWith("-define:"))
							{
								string[] array2 = ValueOf(item3).Split(",".ToCharArray());
								string[] array = array2;
								foreach (string text3 in array)
								{
									string[] array3 = text3.Split("=".ToCharArray(), 2);
									if (array3[0].Length >= 1)
									{
										if (_options.Defines.ContainsKey(array3[0]))
										{
											_options.Defines[array3[0]] = ((array3.Length > 1) ? array3[1] : null);
											TraceInfo("REPLACED DEFINE '" + array3[0] + "' WITH VALUE '" + ((array3.Length > 1) ? array3[1] : string.Empty) + "'");
										}
										else
										{
											_options.Defines.Add(array3[0], (array3.Length > 1) ? array3[1] : null);
											TraceInfo("ADDED DEFINE '" + array3[0] + "' WITH VALUE '" + ((array3.Length > 1) ? array3[1] : string.Empty) + "'");
										}
									}
								}
							}
							else
							{
								InvalidOption(item3);
							}
							break;
						}
						break;
					case 'e':
					{
						string text2 = item3.Substring(1, 8);
						if (text2 != null && text2 == "embedres")
						{
							EmbedResource(ValueOf(item3));
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					}
					case 'u':
						if (item3 == "-unsafe")
						{
							_options.Unsafe = true;
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					case 'x':
						if (item3.Substring(1).StartsWith("x-type-inference-rule-attribute"))
						{
							attribute = ValueOf(item3);
							_options.Environment = new DeferredEnvironment { 
							{
								typeof(TypeInferenceRuleProvider),
								() => new CustomTypeInferenceRuleProvider(attribute)
							} };
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					default:
						if (item3 == "--help")
						{
							Help();
						}
						else
						{
							InvalidOption(item3);
						}
						break;
					}
				}
			}
			if (!flag)
			{
				DoLogo();
			}
		}

		private static string ValueOf(string arg)
		{
			return StripQuotes(arg.Substring(arg.IndexOf(":") + 1));
		}

		private void ParseLib(string arg)
		{
			string text = TrimAdditionalQuote(ValueOf(arg));
			if (string.IsNullOrEmpty(text))
			{
				Console.Error.WriteLine($"Not a valid directory for -lib argument: '{arg}'");
				return;
			}
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (Directory.Exists(text2))
				{
					_options.LibPaths.Add(text2);
				}
				else
				{
					Console.Error.WriteLine($"Not a valid directory for -lib argument: '{text2}'");
				}
			}
		}

		private static void DoLogo()
		{
			Console.WriteLine("Boo Compiler version {0} ({1})", Builtins.BooVersion, RuntimeServices.RuntimeDisplayName);
		}

		private static void Help()
		{
			Console.WriteLine("Usage: booc [options] file1 ...\nOptions:\n -c:CULTURE           Sets the UI culture to be CULTURE\n -checked[+|-]        Turns on or off checked operations (default: +)\n -debug[+|-]          Generate debugging information (default: +)\n -define:S1[,Sn]      Defines symbols S1..Sn with optional values (=val) (-d:)\n -delaysign           Delays assembly signing\n -ducky               Turns on duck typing by default\n -embedres:FILE[,ID]  Embeds FILE with the optional ID\n -keycontainer:NAME   The key pair container used to strongname the assembly\n -keyfile:FILE        The strongname key file used to strongname the assembly\n -lib:DIRS            Adds the comma-separated DIRS to the assembly search path\n -noconfig            Does not load the standard configuration\n -nologo              Does not display the compiler logo\n -nostdlib            Does not reference any of the default libraries\n -nowarn[:W1,Wn]      Suppress all or a list of compiler warnings\n -o:FILE              Sets the output file name to FILE\n -p:PIPELINE          Sets the pipeline to PIPELINE\n -pkg:P1[,Pn]         References packages P1..Pn (on supported platforms)\n -platform:ARCH       Specifies target platform (anycpu, x86, x64 or itanium)\n -reference:A1[,An]   References assemblies (-r:)\n -resource:FILE[,ID]  Embeds FILE as a resource\n -srcdir:DIR          Adds DIR as a directory where sources can be found\n -strict              Turns on strict mode.\n -target:TYPE         Sets the target type (exe, library or winexe) (-t:)\n -unsafe              Allows to compile unsafe code.\n -utf8                Source file(s) are in utf8 format\n -v, -vv, -vvv        Sets verbosity level from warnings to very detailed\n -warn:W1[,Wn]        Enables a list of optional warnings.\n -warnaserror[:W1,Wn] Treats all or a list of warnings as errors\n -wsa                 Enables white-space-agnostic build\n");
		}

		private void EmbedResource(string resourceFile)
		{
			int num = resourceFile.LastIndexOf(',');
			if (num >= 0)
			{
				string rname = resourceFile.Substring(num + 1);
				resourceFile = resourceFile.Substring(0, num);
				_options.Resources.Add(new NamedEmbeddedFileResource(resourceFile, rname));
			}
			else
			{
				_options.Resources.Add(new EmbeddedFileResource(resourceFile));
			}
		}

		private void AddResource(string resourceFile)
		{
			int num = resourceFile.LastIndexOf(',');
			if (num >= 0)
			{
				string rname = resourceFile.Substring(num + 1);
				resourceFile = resourceFile.Substring(0, num);
				_options.Resources.Add(new NamedFileResource(resourceFile, rname));
			}
			else
			{
				_options.Resources.Add(new FileResource(resourceFile));
			}
		}

		private void ConfigurePipeline()
		{
			CompilerPipeline compilerPipeline = ((_pipelineName != null) ? CompilerPipeline.GetPipeline(_pipelineName) : new CompileToFile());
			_options.Pipeline = compilerPipeline;
			if (_debugSteps)
			{
				StepDebugger @object = new StepDebugger();
				compilerPipeline.BeforeStep += @object.BeforeStep;
				compilerPipeline.AfterStep += @object.AfterStep;
			}
		}

		private static string StripQuotes(string s)
		{
			if (s.Length > 1 && (IsDelimitedBy(s, "\"") || IsDelimitedBy(s, "'")))
			{
				return s.Substring(1, s.Length - 2);
			}
			return s;
		}

		private static bool IsDelimitedBy(string s, string delimiter)
		{
			return s.StartsWith(delimiter) && s.EndsWith(delimiter);
		}

		private static string TrimAdditionalQuote(string s)
		{
			return s.EndsWith("\"") ? s.Substring(0, s.Length - 1) : s;
		}

		private System.Collections.Generic.List<string> LoadResponseFile(string file)
		{
			file = Path.GetFullPath(file);
			if (_processedResponseFiles.Contains(file))
			{
				throw new ApplicationException($"Response file '{file}' listed more than once.");
			}
			_processedResponseFiles.Add(file);
			if (!File.Exists(file))
			{
				throw new ApplicationException($"Response file '{file}' could not be found.");
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			try
			{
				using StreamReader streamReader = new StreamReader(file);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					if (text.Length > 0 && text[0] != '#')
					{
						if (text.StartsWith("@") && text.Length > 2)
						{
							list.AddRange(LoadResponseFile(text.Substring(1)));
						}
						else
						{
							list.Add(StripQuotes(text));
						}
					}
				}
			}
			catch (ApplicationException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new ApplicationException($"An error occurred while loading response file '{file}'.", innerException);
			}
			return list;
		}

		private System.Collections.Generic.List<string> ExpandResponseFiles(IEnumerable<string> args)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			foreach (string arg in args)
			{
				if (arg.StartsWith("@") && arg.Length > 2)
				{
					list.AddRange(LoadResponseFile(arg.Substring(1)));
				}
				else
				{
					list.Add(arg);
				}
			}
			return list;
		}

		private void AddDefaultResponseFile(System.Collections.Generic.List<string> args)
		{
			if (!args.Contains("-noconfig"))
			{
				string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "booc.rsp");
				if (File.Exists(text))
				{
					args.InsertRange(0, LoadResponseFile(text));
				}
			}
		}

		private void OnBeforeStep(object sender, CompilerStepEventArgs args)
		{
			args.Context.TraceEnter("Entering {0}", args.Step);
			stepStopwatch = Stopwatch.StartNew();
		}

		private void OnAfterStep(object sender, CompilerStepEventArgs args)
		{
			stepStopwatch.Stop();
			args.Context.TraceLeave("Leaving {0} ({1}ms)", args.Step, stepStopwatch.ElapsedMilliseconds);
		}

		private void InvalidOption(string arg)
		{
			InvalidOption(arg, null);
		}

		private void InvalidOption(string arg, string message)
		{
			Console.Error.WriteLine("Invalid option: {0}. {1}", arg, message);
		}

		private static bool IsFlag(string arg)
		{
			return arg[0] == '-';
		}

		private static void AddFilesForPath(string path, CompilerParameters options)
		{
			string[] files = Directory.GetFiles(path, "*.boo");
			foreach (string text in files)
			{
				if (text.EndsWith(".boo"))
				{
					options.Input.Add(new FileInput(Path.GetFullPath(text)));
				}
			}
			files = Directory.GetDirectories(path);
			foreach (string path2 in files)
			{
				AddFilesForPath(path2, options);
			}
		}

		private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			TraceInfo("ASSEMBLY LOADED: " + GetAssemblyLocation(args.LoadedAssembly));
		}

		private static string GetAssemblyLocation(Assembly a)
		{
			try
			{
				return a.Location;
			}
			catch (Exception)
			{
				return "<dynamic>" + a.FullName;
			}
		}

		private void MonitorAppDomain()
		{
			if (_options.TraceInfo)
			{
				AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly a in assemblies)
				{
					TraceInfo("ASSEMBLY AT STARTUP: " + GetAssemblyLocation(a));
				}
			}
		}

		private void TraceInfo(string s)
		{
			if (_options.TraceInfo)
			{
				Console.Error.WriteLine(s);
			}
		}

		private static string Consume(TextReader reader)
		{
			StringWriter stringWriter = new StringWriter();
			string text = reader.ReadLine();
			while (null != text)
			{
				stringWriter.WriteLine(text);
				text = reader.ReadLine();
			}
			return stringWriter.ToString();
		}

		private void LoadReferences()
		{
			foreach (string reference in _references)
			{
				_options.References.Add(_options.LoadAssembly(reference, throwOnError: true));
			}
			foreach (string package in _packages)
			{
				_options.LoadReferencesFromPackage(package);
			}
		}
	}
}
