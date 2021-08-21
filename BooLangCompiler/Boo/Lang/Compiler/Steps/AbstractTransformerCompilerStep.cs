#define TRACE
using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractTransformerCompilerStep : DepthFirstTransformer, ICompilerStep, ICompilerComponent, IDisposable
	{
		private CompilerContext _context;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService;

		private EnvironmentProvision<TypeSystemServices> _typeSystemServices;

		protected CompilerContext Context => _context;

		protected BooCodeBuilder CodeBuilder => _context.CodeBuilder;

		protected NameResolutionService NameResolutionService => _nameResolutionService;

		protected CompileUnit CompileUnit => _context.CompileUnit;

		protected CompilerParameters Parameters => _context.Parameters;

		protected CompilerErrorCollection Errors => _context.Errors;

		protected CompilerWarningCollection Warnings => _context.Warnings;

		protected TypeSystemServices TypeSystemServices => _typeSystemServices;

		public virtual void Run()
		{
			Visit(CompileUnit);
		}

		public override void OnQuasiquoteExpression(QuasiquoteExpression node)
		{
		}

		protected void Bind(Node node, IEntity tag)
		{
			node.Entity = tag;
		}

		protected void BindExpressionType(Expression node, IType type)
		{
			_context.TraceVerbose("{0}: Type of expression '{1}' bound to '{2}'.", node.LexicalInfo, node, type);
			node.ExpressionType = type;
		}

		protected virtual IType GetExpressionType(Expression node)
		{
			return TypeSystemServices.GetExpressionType(node);
		}

		public IEntity GetEntity(Node node)
		{
			return TypeSystemServices.GetEntity(node);
		}

		protected IType GetType(Node node)
		{
			return TypeSystemServices.GetType(node);
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

		public virtual void Dispose()
		{
			_context = null;
		}
	}
}
