#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Services;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler
{
	public class CompilerContext
	{
		private readonly CompilerParameters _parameters;

		private readonly CompileUnit _unit;

		private readonly CompilerReferenceCollection _references;

		private readonly CompilerErrorCollection _errors;

		private readonly CompilerWarningCollection _warnings;

		private Assembly _generatedAssembly;

		private string _generatedAssemblyFileName;

		private readonly Hash _properties;

		private EnvironmentProvision<BooCodeBuilder> _codeBuilder = default(EnvironmentProvision<BooCodeBuilder>);

		private int _indentation;

		private readonly CachingEnvironment _environment;

		public static CompilerContext Current => (ActiveEnvironment.Instance != null) ? My<CompilerContext>.Instance : null;

		public IEnvironment Environment => _environment;

		public Hash Properties => _properties;

		public string GeneratedAssemblyFileName
		{
			get
			{
				return _generatedAssemblyFileName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				_generatedAssemblyFileName = value;
			}
		}

		public object this[object key]
		{
			get
			{
				return _properties[key];
			}
			set
			{
				_properties[key] = value;
			}
		}

		public CompilerParameters Parameters => _parameters;

		public CompilerReferenceCollection References => _references;

		public CompilerErrorCollection Errors => _errors;

		public CompilerWarningCollection Warnings => _warnings;

		public CompileUnit CompileUnit => _unit;

		public BooCodeBuilder CodeBuilder => _codeBuilder;

		public Assembly GeneratedAssembly
		{
			get
			{
				return _generatedAssembly;
			}
			set
			{
				_generatedAssembly = value;
			}
		}

		private TextWriter TraceWriter => Console.Out;

		public CompilerContext()
			: this(new CompileUnit())
		{
		}

		public CompilerContext(CompileUnit unit)
			: this(new CompilerParameters(), unit)
		{
		}

		public CompilerContext(bool stdlib)
			: this(new CompilerParameters(stdlib), new CompileUnit())
		{
		}

		public CompilerContext(CompilerParameters options)
			: this(options, new CompileUnit())
		{
		}

		public CompilerContext(CompilerParameters options, CompileUnit unit)
		{
			if (null == options)
			{
				throw new ArgumentNullException("options");
			}
			if (null == unit)
			{
				throw new ArgumentNullException("unit");
			}
			_unit = unit;
			_errors = new CompilerErrorCollection();
			_warnings = new CompilerWarningCollection();
			_warnings.Adding += OnCompilerWarning;
			_references = options.References;
			_parameters = options;
			if (_parameters.Debug && !_parameters.Defines.ContainsKey("DEBUG"))
			{
				_parameters.Defines.Add("DEBUG", null);
			}
			_properties = new Hash();
			InstantiatingEnvironment instantiatingEnvironment = new InstantiatingEnvironment();
			_environment = ((_parameters.Environment != null) ? new CachingEnvironment(new EnvironmentChain(_parameters.Environment, instantiatingEnvironment)) : new CachingEnvironment(instantiatingEnvironment));
			_environment.InstanceCached += InitializeService;
			RegisterService(_references.Provider);
			RegisterService(_parameters);
			RegisterService(_errors);
			RegisterService(_warnings);
			RegisterService(_unit);
			RegisterService(this);
		}

		public string GetUniqueName(params string[] components)
		{
			return My<UniqueNameProvider>.Instance.GetUniqueName(components);
		}

		[Conditional("TRACE")]
		public void TraceEnter(string format, params object[] args)
		{
			if (_parameters.TraceInfo)
			{
				TraceLine(format, args);
				IndentTraceOutput();
			}
		}

		[Conditional("TRACE")]
		public void TraceLeave(string format, params object[] args)
		{
			if (_parameters.TraceInfo)
			{
				DedentTraceOutput();
				TraceLine(format, args);
			}
		}

		[Conditional("TRACE")]
		public void TraceInfo(string format, params object[] args)
		{
			if (_parameters.TraceInfo)
			{
				TraceLine(format, args);
			}
		}

		[Conditional("TRACE")]
		public void TraceInfo(string message)
		{
			if (_parameters.TraceInfo)
			{
				TraceLine(message);
			}
		}

		[Conditional("TRACE")]
		public void TraceWarning(string message)
		{
			if (_parameters.TraceWarning)
			{
				TraceLine(message);
			}
		}

		[Conditional("TRACE")]
		public void TraceWarning(string message, params object[] args)
		{
			if (_parameters.TraceWarning)
			{
				TraceLine(message, args);
			}
		}

		[Conditional("TRACE")]
		public void TraceVerbose(string format, params object[] args)
		{
			if (_parameters.TraceVerbose)
			{
				TraceLine(format, args);
			}
		}

		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param1, object param2)
		{
			if (_parameters.TraceVerbose)
			{
				TraceLine(format, param1, param2);
			}
		}

		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param1, object param2, object param3)
		{
			if (_parameters.TraceVerbose)
			{
				TraceLine(format, param1, param2, param3);
			}
		}

		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param)
		{
			if (_parameters.TraceVerbose)
			{
				TraceLine(format, param);
			}
		}

		[Conditional("TRACE")]
		public void TraceVerbose(string message)
		{
			if (_parameters.TraceVerbose)
			{
				TraceLine(message);
			}
		}

		[Conditional("TRACE")]
		public void TraceError(string message, params object[] args)
		{
			if (_parameters.TraceError)
			{
				TraceLine(message, args);
			}
		}

		[Conditional("TRACE")]
		public void TraceError(Exception x)
		{
			if (_parameters.TraceError)
			{
				TraceLine(x);
			}
		}

		private void IndentTraceOutput()
		{
			_indentation++;
		}

		private void DedentTraceOutput()
		{
			_indentation--;
		}

		private void TraceLine(object o)
		{
			WriteIndentation();
			TraceWriter.WriteLine(o);
		}

		private void WriteIndentation()
		{
			for (int i = 0; i < _indentation; i++)
			{
				TraceWriter.Write('\t');
			}
		}

		private void TraceLine(string format, params object[] args)
		{
			WriteIndentation();
			TraceWriter.WriteLine(format, args);
		}

		public void RegisterService<T>(T service) where T : class
		{
			if (null == service)
			{
				throw new ArgumentNullException("service");
			}
			AddService(typeof(T), service);
		}

		private void AddService(Type serviceType, object service)
		{
			_environment.Add(serviceType, service);
		}

		private void InitializeService(object service)
		{
			TraceInfo("Compiler component '{0}' instantiated.", service);
			(service as ICompilerComponent)?.Initialize(this);
		}

		private void OnCompilerWarning(object o, CompilerWarningEventArgs args)
		{
			CompilerWarning warning = args.Warning;
			if (Parameters.NoWarn || Parameters.DisabledWarnings.Contains(warning.Code))
			{
				args.Cancel();
			}
			if (Parameters.WarnAsError || Parameters.WarningsAsErrors.Contains(warning.Code))
			{
				Errors.Add(new CompilerError(warning.Code, warning.LexicalInfo, warning.Message, null));
				args.Cancel();
			}
		}
	}
}
