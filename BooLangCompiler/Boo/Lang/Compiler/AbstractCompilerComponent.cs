using System;
using System.IO;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler
{
	public abstract class AbstractCompilerComponent : ICompilerComponent
	{
		private EnvironmentProvision<TypeSystemServices> _typeSystemServices;

		private CompilerContext _context;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService;

		protected CompilerContext Context => _context;

		protected BooCodeBuilder CodeBuilder => Context.CodeBuilder;

		protected CompileUnit CompileUnit => Context.CompileUnit;

		protected CompilerParameters Parameters => Context.Parameters;

		protected TextWriter OutputWriter => Context.Parameters.OutputWriter;

		protected CompilerErrorCollection Errors => Context.Errors;

		protected CompilerWarningCollection Warnings => Context.Warnings;

		protected TypeSystemServices TypeSystemServices => _typeSystemServices;

		protected NameResolutionService NameResolutionService => _nameResolutionService;

		protected AbstractCompilerComponent()
		{
		}

		protected AbstractCompilerComponent(CompilerContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			_context = context;
		}

		public IEntity GetEntity(Node node)
		{
			if (null == node.Entity)
			{
				throw CompilerErrorFactory.InvalidNode(node);
			}
			return node.Entity;
		}

		public virtual void Initialize(CompilerContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			_context = context;
			_typeSystemServices = default(EnvironmentProvision<TypeSystemServices>);
			_nameResolutionService = default(EnvironmentProvision<NameResolutionService>);
		}
	}
}
