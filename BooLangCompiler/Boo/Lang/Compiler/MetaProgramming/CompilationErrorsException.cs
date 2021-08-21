using System;

namespace Boo.Lang.Compiler.MetaProgramming
{
	public class CompilationErrorsException : Exception
	{
		private CompilerErrorCollection _errors;

		public CompilerErrorCollection Errors => _errors;

		public CompilationErrorsException(CompilerErrorCollection errors)
			: base(errors.ToString())
		{
			_errors = errors;
		}
	}
}
