#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.Steps;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler
{
	public class CompilerPipeline : IEnumerable<ICompilerStep>, IEnumerable
	{
		protected List<ICompilerStep> _items;

		protected bool _breakOnErrors;

		public bool BreakOnErrors
		{
			get
			{
				return _breakOnErrors;
			}
			set
			{
				_breakOnErrors = value;
			}
		}

		public int Count => _items.Count;

		public ICompilerStep CurrentStep { get; private set; }

		public ICompilerStep this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("value");
				}
				_items[index] = value;
			}
		}

		public event EventHandler<CompilerPipelineEventArgs> Before;

		public event EventHandler<CompilerPipelineEventArgs> After;

		public event CompilerStepEventHandler BeforeStep;

		public event CompilerStepEventHandler AfterStep;

		public static CompilerPipeline GetPipeline(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException("name");
			}
			switch (name)
			{
			case "parse":
				return new Parse();
			case "compile":
				return new Compile();
			case "run":
				return new Run();
			case "default":
				return new CompileToFile();
			case "verify":
				return new CompileToFileAndVerify();
			case "roundtrip":
				return new ParseAndPrint();
			case "boo":
				return new CompileToBoo();
			case "ast":
				return new ParseAndPrintAst();
			case "xml":
				return new ParseAndPrintXml();
			case "checkforerrors":
				return new CheckForErrors();
			case "dumpreferences":
			{
				CompilerPipeline compilerPipeline = new CompileToBoo();
				compilerPipeline.Add(new DumpReferences());
				return compilerPipeline;
			}
			default:
				return LoadCustomPipeline(name);
			}
		}

		private static CompilerPipeline LoadCustomPipeline(string typeName)
		{
			if (typeName.IndexOf(',') < 0)
			{
				throw new ArgumentException($"'{typeName}' is neither a built-in pipeline nor a valid custom pipeline name.");
			}
			return (CompilerPipeline)Activator.CreateInstance(FindPipelineType(typeName));
		}

		private static Type FindPipelineType(string typeName)
		{
			Assembly assembly = FindLoadedAssembly(AssemblySimpleNameFromFullTypeName(typeName));
			if (null != assembly)
			{
				return assembly.GetType(SimpleTypeNameFromFullTypeName(typeName));
			}
			return Type.GetType(typeName, throwOnError: true);
		}

		private static string SimpleTypeNameFromFullTypeName(string name)
		{
			return name.Split(',')[0].Trim();
		}

		private static string AssemblySimpleNameFromFullTypeName(string name)
		{
			return name.Split(',')[1].Trim();
		}

		private static Assembly FindLoadedAssembly(string assemblyName)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.GetName().Name == assemblyName)
				{
					return assembly;
				}
			}
			return null;
		}

		public CompilerPipeline()
		{
			_items = new List<ICompilerStep>();
			_breakOnErrors = true;
		}

		public CompilerPipeline Add(ICompilerStep step)
		{
			if (null == step)
			{
				throw new ArgumentNullException("step");
			}
			_items.Add(step);
			return this;
		}

		public CompilerPipeline RemoveAt(int index)
		{
			_items.RemoveAt(index);
			return this;
		}

		public CompilerPipeline Remove(Type stepExactType)
		{
			return RemoveAt(Find(stepExactType));
		}

		public CompilerPipeline Insert(int index, ICompilerStep step)
		{
			if (null == step)
			{
				throw new ArgumentNullException("step");
			}
			_items.Insert(index, step);
			return this;
		}

		public CompilerPipeline InsertAfter(Type stepExactType, ICompilerStep step)
		{
			return Insert(Find(stepExactType) + 1, step);
		}

		public CompilerPipeline InsertBefore(Type stepExactType, ICompilerStep step)
		{
			return Insert(Find(stepExactType) - 1, step);
		}

		public CompilerPipeline Replace(Type stepExactType, ICompilerStep step)
		{
			if (null == step)
			{
				throw new ArgumentNullException("step");
			}
			int num = Find(stepExactType);
			if (-1 == num)
			{
				throw new ArgumentException("stepExactType");
			}
			_items[num] = step;
			return this;
		}

		public int Find(Type stepExactType)
		{
			if (null == stepExactType)
			{
				throw new ArgumentNullException("stepExactType");
			}
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i].GetType() == stepExactType)
				{
					return i;
				}
			}
			return -1;
		}

		public ICompilerStep Get(Type stepExactType)
		{
			int num = Find(stepExactType);
			if (-1 == num)
			{
				return null;
			}
			return _items[num];
		}

		public virtual void Clear()
		{
			_items.Clear();
		}

		protected virtual void OnBefore(CompilerContext context)
		{
			EventHandler<CompilerPipelineEventArgs> before = this.Before;
			if (null != before)
			{
				before(this, new CompilerPipelineEventArgs(context));
			}
		}

		protected virtual void OnAfter(CompilerContext context)
		{
			EventHandler<CompilerPipelineEventArgs> after = this.After;
			if (null != after)
			{
				after(this, new CompilerPipelineEventArgs(context));
			}
		}

		protected virtual void OnBeforeStep(CompilerContext context, ICompilerStep step)
		{
			CompilerStepEventHandler beforeStep = this.BeforeStep;
			if (null != beforeStep)
			{
				beforeStep(this, new CompilerStepEventArgs(context, step));
			}
		}

		protected virtual void OnAfterStep(CompilerContext context, ICompilerStep step)
		{
			CompilerStepEventHandler afterStep = this.AfterStep;
			if (null != afterStep)
			{
				afterStep(this, new CompilerStepEventArgs(context, step));
			}
		}

		protected virtual void Prepare(CompilerContext context)
		{
		}

		public virtual void Run(CompilerContext context)
		{
			ActiveEnvironment.With(context.Environment, delegate
			{
				OnBefore(context);
				try
				{
					Prepare(context);
					RunSteps(context);
				}
				finally
				{
					try
					{
						DisposeSteps();
					}
					finally
					{
						OnAfter(context);
					}
				}
			});
		}

		private void TracingErrors(Action action)
		{
			try
			{
				action();
			}
			catch (Exception x)
			{
				My<CompilerContext>.Instance.TraceError(x);
			}
		}

		private void DisposeSteps()
		{
			foreach (IDisposable disposableStep in _items.OfType<IDisposable>())
			{
				TracingErrors(delegate
				{
					disposableStep.Dispose();
				});
			}
		}

		private void RunSteps(CompilerContext context)
		{
			foreach (ICompilerStep item in _items)
			{
				RunStep(context, item);
				if (_breakOnErrors && context.Errors.Count > 0)
				{
					break;
				}
			}
		}

		protected void RunStep(CompilerContext context, ICompilerStep step)
		{
			CurrentStep = step;
			try
			{
				OnBeforeStep(context, step);
				step.Initialize(context);
				try
				{
					step.Run();
				}
				catch (CompilerError item)
				{
					context.Errors.Add(item);
				}
				catch (Exception error)
				{
					context.Errors.Add(CompilerErrorFactory.StepExecutionError(error, step));
				}
				finally
				{
					OnAfterStep(context, step);
				}
			}
			finally
			{
				CurrentStep = null;
			}
		}

		public IEnumerator<ICompilerStep> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ICompilerStep>)this).GetEnumerator();
		}
	}
}
