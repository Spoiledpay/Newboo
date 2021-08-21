using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public class BooCompiler
	{
		private readonly CompilerParameters _parameters;

		public CompilerParameters Parameters => _parameters;

		public BooCompiler()
		{
			_parameters = new CompilerParameters();
		}

		public BooCompiler(CompilerParameters parameters)
		{
			if (null == parameters)
			{
				throw new ArgumentNullException("parameters");
			}
			_parameters = parameters;
		}

		public CompilerContext Run(CompileUnit compileUnit)
		{
			if (null == compileUnit)
			{
				throw new ArgumentNullException("compileUnit");
			}
			if (null == _parameters.Pipeline)
			{
				throw new InvalidOperationException("A pipeline must be specified!");
			}
			CompilerContext compilerContext = new CompilerContext(_parameters, compileUnit);
			_parameters.Pipeline.Run(compilerContext);
			return compilerContext;
		}

		public CompilerContext Run()
		{
			return Run(new CompileUnit());
		}
	}
}
