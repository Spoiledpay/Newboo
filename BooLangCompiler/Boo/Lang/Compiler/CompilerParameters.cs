#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler
{
	public class CompilerParameters
	{
		public static IReflectionTypeSystemProvider SharedTypeSystemProvider = new ReflectionTypeSystemProvider();

		private TextWriter _outputWriter;

		private readonly CompilerInputCollection _input;

		private readonly CompilerResourceCollection _resources;

		private CompilerReferenceCollection _compilerReferences;

		private string _outputAssembly;

		private bool _strict;

		private readonly List<string> _libPaths;

		private readonly string _systemDir;

		private Assembly _booAssembly;

		private readonly Dictionary<string, string> _defines = new Dictionary<string, string>(StringComparer.Ordinal);

		private TypeMemberModifiers _defaultTypeVisibility = TypeMemberModifiers.Public;

		private TypeMemberModifiers _defaultMethodVisibility = TypeMemberModifiers.Public;

		private TypeMemberModifiers _defaultPropertyVisibility = TypeMemberModifiers.Public;

		private TypeMemberModifiers _defaultEventVisibility = TypeMemberModifiers.Public;

		private TypeMemberModifiers _defaultFieldVisibility = TypeMemberModifiers.Protected;

		private bool _defaultVisibilitySettingsRead;

		private Set<string> _disabledWarnings = new Set<string>();

		private Set<string> _promotedWarnings = new Set<string>();

		public Assembly BooAssembly
		{
			get
			{
				return _booAssembly;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("value");
				}
				if (value != _booAssembly)
				{
					_compilerReferences.Remove(_booAssembly);
					_booAssembly = value;
					_compilerReferences.Add(value);
				}
			}
		}

		public int MaxExpansionIterations { get; set; }

		public CompilerInputCollection Input => _input;

		public List<string> LibPaths => _libPaths;

		public CompilerResourceCollection Resources => _resources;

		public CompilerReferenceCollection References
		{
			get
			{
				return _compilerReferences;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("References");
				}
				_compilerReferences = value;
			}
		}

		public CompilerPipeline Pipeline { get; set; }

		public string OutputAssembly
		{
			get
			{
				return _outputAssembly;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("OutputAssembly");
				}
				_outputAssembly = value;
			}
		}

		public CompilerOutputType OutputType { get; set; }

		public bool GenerateInMemory { get; set; }

		public bool StdLib { get; set; }

		public TextWriter OutputWriter
		{
			get
			{
				return _outputWriter;
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("OutputWriter");
				}
				_outputWriter = value;
			}
		}

		public bool Debug { get; set; }

		public virtual bool Ducky { get; set; }

		public bool Checked { get; set; }

		public string KeyFile { get; set; }

		public string KeyContainer { get; set; }

		public bool DelaySign { get; set; }

		public bool WhiteSpaceAgnostic { get; set; }

		public Dictionary<string, string> Defines => _defines;

		public TypeMemberModifiers DefaultTypeVisibility
		{
			get
			{
				if (!_defaultVisibilitySettingsRead)
				{
					ReadDefaultVisibilitySettings();
				}
				return _defaultTypeVisibility;
			}
			set
			{
				_defaultTypeVisibility = value & TypeMemberModifiers.VisibilityMask;
			}
		}

		public TypeMemberModifiers DefaultMethodVisibility
		{
			get
			{
				if (!_defaultVisibilitySettingsRead)
				{
					ReadDefaultVisibilitySettings();
				}
				return _defaultMethodVisibility;
			}
			set
			{
				_defaultMethodVisibility = value & TypeMemberModifiers.VisibilityMask;
			}
		}

		public TypeMemberModifiers DefaultPropertyVisibility
		{
			get
			{
				if (!_defaultVisibilitySettingsRead)
				{
					ReadDefaultVisibilitySettings();
				}
				return _defaultPropertyVisibility;
			}
			set
			{
				_defaultPropertyVisibility = value & TypeMemberModifiers.VisibilityMask;
			}
		}

		public TypeMemberModifiers DefaultEventVisibility
		{
			get
			{
				if (!_defaultVisibilitySettingsRead)
				{
					ReadDefaultVisibilitySettings();
				}
				return _defaultEventVisibility;
			}
			set
			{
				_defaultEventVisibility = value & TypeMemberModifiers.VisibilityMask;
			}
		}

		public TypeMemberModifiers DefaultFieldVisibility
		{
			get
			{
				if (!_defaultVisibilitySettingsRead)
				{
					ReadDefaultVisibilitySettings();
				}
				return _defaultFieldVisibility;
			}
			set
			{
				_defaultFieldVisibility = value & TypeMemberModifiers.VisibilityMask;
			}
		}

		public bool TraceInfo => TraceLevel >= TraceLevel.Info;

		public bool TraceWarning => TraceLevel >= TraceLevel.Warning;

		public bool TraceError => TraceLevel >= TraceLevel.Error;

		public bool TraceVerbose => TraceLevel >= TraceLevel.Verbose;

		public TraceLevel TraceLevel { get; set; }

		public bool NoWarn { get; set; }

		public bool WarnAsError { get; set; }

		public ICollection<string> DisabledWarnings => _disabledWarnings;

		public ICollection<string> WarningsAsErrors => _promotedWarnings;

		public bool Strict
		{
			get
			{
				return _strict;
			}
			set
			{
				_strict = value;
				if (_strict)
				{
					OnStrictMode();
				}
				else
				{
					OnNonStrictMode();
				}
			}
		}

		public bool Unsafe { get; set; }

		public string Platform { get; set; }

		public IEnvironment Environment { get; set; }

		public CompilerParameters()
			: this(loadDefaultReferences: true)
		{
		}

		public CompilerParameters(bool loadDefaultReferences)
			: this(SharedTypeSystemProvider, loadDefaultReferences)
		{
		}

		public CompilerParameters(IReflectionTypeSystemProvider reflectionProvider)
			: this(reflectionProvider, loadDefaultReferences: true)
		{
		}

		public CompilerParameters(IReflectionTypeSystemProvider reflectionProvider, bool loadDefaultReferences)
		{
			_libPaths = new List<string>();
			_systemDir = Permissions.WithDiscoveryPermission(() => GetSystemDir());
			if (_systemDir != null)
			{
				_libPaths.Add(_systemDir);
				_libPaths.Add(Directory.GetCurrentDirectory());
			}
			_input = new CompilerInputCollection();
			_resources = new CompilerResourceCollection();
			_compilerReferences = new CompilerReferenceCollection(reflectionProvider);
			MaxExpansionIterations = 12;
			_outputAssembly = string.Empty;
			OutputType = CompilerOutputType.Auto;
			_outputWriter = Console.Out;
			Debug = true;
			Checked = true;
			GenerateInMemory = true;
			StdLib = true;
			DelaySign = false;
			Strict = false;
			TraceLevel = DefaultTraceLevel();
			if (loadDefaultReferences)
			{
				LoadDefaultReferences();
			}
		}

		private static TraceLevel DefaultTraceLevel()
		{
			string value = Permissions.WithEnvironmentPermission(() => System.Environment.GetEnvironmentVariable("BOO_TRACE_LEVEL"));
			return (!string.IsNullOrEmpty(value)) ? ((TraceLevel)Enum.Parse(typeof(TraceLevel), value)) : TraceLevel.Off;
		}

		public void LoadDefaultReferences()
		{
			_booAssembly = typeof(Builtins).Assembly;
			_compilerReferences.Add(_booAssembly);
			IAssemblyReference extensionsAssembly = TryToLoadExtensionsAssembly();
			if (extensionsAssembly != null)
			{
				_compilerReferences.Add(extensionsAssembly);
			}
			_compilerReferences.Add(GetType().Assembly);
			_compilerReferences.Add(LoadAssembly("mscorlib", throwOnError: true));
			_compilerReferences.Add(LoadAssembly("System", throwOnError: true));
			_compilerReferences.Add(LoadAssembly("System.Core", throwOnError: true));
			Permissions.WithDiscoveryPermission((Func<object>)delegate
			{
				WriteTraceInfo("BOO LANG DLL: " + _booAssembly.Location);
				WriteTraceInfo("BOO COMPILER EXTENSIONS DLL: " + ((extensionsAssembly != null) ? extensionsAssembly.ToString() : "NOT FOUND!"));
				return null;
			});
		}

		private IAssemblyReference TryToLoadExtensionsAssembly()
		{
			return Permissions.WithDiscoveryPermission(delegate
			{
				string text = Path.Combine(Path.GetDirectoryName(_booAssembly.Location), "Boo.Lang.Extensions.dll");
				return File.Exists(text) ? AssemblyReferenceFor(Assembly.LoadFrom(text)) : null;
			}) ?? LoadAssembly("Boo.Lang.Extensions.dll", throwOnError: false);
		}

		public ICompileUnit FindAssembly(string name)
		{
			return _compilerReferences.Find(name);
		}

		public void AddAssembly(Assembly asm)
		{
			if (null == asm)
			{
				throw new ArgumentNullException();
			}
			_compilerReferences.Add(asm);
		}

		public IAssemblyReference LoadAssembly(string assembly)
		{
			return LoadAssembly(assembly, throwOnError: true);
		}

		public IAssemblyReference LoadAssembly(string assemblyName, bool throwOnError)
		{
			Assembly assembly = ForName(assemblyName, throwOnError);
			return (assembly != null) ? AssemblyReferenceFor(assembly) : null;
		}

		private IAssemblyReference AssemblyReferenceFor(Assembly assembly)
		{
			return _compilerReferences.Provider.ForAssembly(assembly);
		}

		protected virtual Assembly ForName(string assembly, bool throwOnError)
		{
			Assembly assembly2 = null;
			try
			{
				assembly2 = ((assembly.IndexOfAny(new char[2] { '/', '\\' }) == -1) ? LoadAssemblyFromGac(assembly) : Assembly.LoadFrom(assembly));
			}
			catch (FileNotFoundException)
			{
				return LoadAssemblyFromLibPaths(assembly, throwOnError);
			}
			catch (BadImageFormatException ex2)
			{
				if (throwOnError)
				{
					throw new ApplicationException($"Unable to load assembly (bad file format): {ex2.FusionLog}", ex2);
				}
			}
			catch (FileLoadException ex3)
			{
				if (throwOnError)
				{
					throw new ApplicationException($"Unable to load assembly: {ex3.FusionLog}", ex3);
				}
			}
			catch (ArgumentNullException innerException)
			{
				if (throwOnError)
				{
					throw new ApplicationException("Unable to load assembly (null argument)", innerException);
				}
			}
			return assembly2 ?? LoadAssemblyFromLibPaths(assembly, throwOnError: false);
		}

		private Assembly LoadAssemblyFromLibPaths(string assembly, bool throwOnError)
		{
			Assembly assembly2 = null;
			string text = "";
			foreach (string libPath in _libPaths)
			{
				string text2 = Path.Combine(libPath, assembly);
				FileInfo fileInfo = new FileInfo(text2);
				if (!IsAssemblyExtension(fileInfo.Extension))
				{
					text2 += ".dll";
				}
				try
				{
					assembly2 = Assembly.LoadFrom(text2);
					if (assembly2 != null)
					{
						return assembly2;
					}
				}
				catch (FileNotFoundException ex)
				{
					text += ex.FusionLog;
				}
			}
			if (throwOnError)
			{
				throw new ApplicationException($"Cannot find assembly: '{assembly}'");
			}
			return assembly2;
		}

		private static bool IsAssemblyExtension(string extension)
		{
			switch (extension.ToLower())
			{
			case ".dll":
			case ".exe":
				return true;
			default:
				return false;
			}
		}

		private static Assembly LoadAssemblyFromGac(string assemblyName)
		{
			assemblyName = NormalizeAssemblyName(assemblyName);
			Assembly assembly = Permissions.WithDiscoveryPermission(() => Assembly.LoadWithPartialName(assemblyName));
			return assembly ?? Assembly.Load(assemblyName);
		}

		private static string NormalizeAssemblyName(string assembly)
		{
			string text = Path.GetExtension(assembly).ToLower();
			if (text == ".dll" || text == ".exe")
			{
				return assembly.Substring(0, assembly.Length - 4);
			}
			return assembly;
		}

		public void LoadReferencesFromPackage(string package)
		{
			string[] array = Regex.Split(pkgconfig(package), "\\-r\\:", RegexOptions.CultureInvariant);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim();
				if (text2.Length != 0)
				{
					WriteTraceInfo("LOADING REFERENCE FROM PKGCONFIG '" + package + "' : " + text2);
					References.Add(LoadAssembly(text2));
				}
			}
		}

		[Conditional("TRACE")]
		private void WriteTraceInfo(string message)
		{
			if (TraceInfo)
			{
				Console.Error.WriteLine(message);
			}
		}

		private static string pkgconfig(string package)
		{
			Process process;
			try
			{
				process = Builtins.shellp("pkg-config", $"--libs {package}");
			}
			catch (Exception innerException)
			{
				throw new ApplicationException("Cannot execute pkg-config, is it in your PATH ?", innerException);
			}
			process.WaitForExit();
			if (process.ExitCode != 0)
			{
				throw new ApplicationException($"pkg-config returned errors: {process.StandardError.ReadToEnd()}");
			}
			return process.StandardOutput.ReadToEnd();
		}

		private static string GetSystemDir()
		{
			return Path.GetDirectoryName(typeof(string).Assembly.Location);
		}

		private void ReadDefaultVisibilitySettings()
		{
			if (_defines.TryGetValue("DEFAULT_TYPE_VISIBILITY", out var value))
			{
				DefaultTypeVisibility = ParseVisibility(value);
			}
			if (_defines.TryGetValue("DEFAULT_METHOD_VISIBILITY", out value))
			{
				DefaultMethodVisibility = ParseVisibility(value);
			}
			if (_defines.TryGetValue("DEFAULT_PROPERTY_VISIBILITY", out value))
			{
				DefaultPropertyVisibility = ParseVisibility(value);
			}
			if (_defines.TryGetValue("DEFAULT_EVENT_VISIBILITY", out value))
			{
				DefaultEventVisibility = ParseVisibility(value);
			}
			if (_defines.TryGetValue("DEFAULT_FIELD_VISIBILITY", out value))
			{
				DefaultFieldVisibility = ParseVisibility(value);
			}
			_defaultVisibilitySettingsRead = true;
		}

		private static TypeMemberModifiers ParseVisibility(string visibility)
		{
			if (string.IsNullOrEmpty(visibility))
			{
				throw new ArgumentNullException("visibility");
			}
			visibility = visibility.ToLower();
			return visibility switch
			{
				"public" => TypeMemberModifiers.Public, 
				"protected" => TypeMemberModifiers.Protected, 
				"internal" => TypeMemberModifiers.Internal, 
				"private" => TypeMemberModifiers.Private, 
				_ => throw new ArgumentException("visibility", $"Invalid visibility: '{visibility}'"), 
			};
		}

		public void EnableWarning(string code)
		{
			if (_disabledWarnings.Contains(code))
			{
				_disabledWarnings.Remove(code);
			}
		}

		public void DisableWarning(string code)
		{
			_disabledWarnings.Add(code);
		}

		public void ResetWarnings()
		{
			NoWarn = false;
			_disabledWarnings.Clear();
			Strict = _strict;
		}

		public void EnableWarningAsError(string code)
		{
			_promotedWarnings.Add(code);
		}

		public void DisableWarningAsError(string code)
		{
			if (_promotedWarnings.Contains(code))
			{
				_promotedWarnings.Remove(code);
			}
		}

		public void ResetWarningsAsErrors()
		{
			WarnAsError = false;
			_promotedWarnings.Clear();
		}

		protected virtual void OnNonStrictMode()
		{
			_defaultTypeVisibility = TypeMemberModifiers.Public;
			_defaultMethodVisibility = TypeMemberModifiers.Public;
			_defaultPropertyVisibility = TypeMemberModifiers.Public;
			_defaultEventVisibility = TypeMemberModifiers.Public;
			_defaultFieldVisibility = TypeMemberModifiers.Protected;
			DisableWarning("BCW0023");
			DisableWarning("BCW0024");
			DisableWarning("BCW0028");
		}

		protected virtual void OnStrictMode()
		{
			_defaultTypeVisibility = TypeMemberModifiers.Private;
			_defaultMethodVisibility = TypeMemberModifiers.Private;
			_defaultPropertyVisibility = TypeMemberModifiers.Private;
			_defaultEventVisibility = TypeMemberModifiers.Private;
			_defaultFieldVisibility = TypeMemberModifiers.Private;
			EnableWarning("BCW0023");
			EnableWarning("BCW0024");
			DisableWarning("BCW0028");
		}
	}
}
