#define TRACE
using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class AbstractFastVisitorCompilerStep : FastDepthFirstVisitor, ICompilerStep, ICompilerComponent, IDisposable
	{
		private CompilerContext _context;

		private EnvironmentProvision<BooCodeBuilder> _codeBuilder;

		private EnvironmentProvision<TypeSystemServices> _typeSystemServices;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService;

		protected CompilerContext Context => _context;

		protected CompilerErrorCollection Errors => _context.Errors;

		protected CompilerWarningCollection Warnings => _context.Warnings;

		protected CompilerParameters Parameters => _context.Parameters;

		protected BooCodeBuilder CodeBuilder => _codeBuilder;

		protected TypeSystemServices TypeSystemServices => _typeSystemServices;

		protected NameResolutionService NameResolutionService => _nameResolutionService;

		protected CompileUnit CompileUnit => _context.CompileUnit;

		public virtual void Run()
		{
			CompileUnit.Accept(this);
		}

		public virtual void Initialize(CompilerContext context)
		{
			_context = context;
			_codeBuilder = default(EnvironmentProvision<BooCodeBuilder>);
			_typeSystemServices = default(EnvironmentProvision<TypeSystemServices>);
			_nameResolutionService = default(EnvironmentProvision<NameResolutionService>);
		}

		public virtual void Dispose()
		{
			_context = null;
		}

		protected IType GetType(Node node)
		{
			return TypeSystemServices.GetType(node);
		}

		protected void Error(Expression node, CompilerError error)
		{
			Error(node);
			Errors.Add(error);
		}

		protected void Error(CompilerError error)
		{
			Errors.Add(error);
		}

		protected void Error(Expression node)
		{
			node.ExpressionType = TypeSystemServices.ErrorEntity;
		}

		protected void NotImplemented(Node node, string feature)
		{
			throw CompilerErrorFactory.NotImplemented(node, feature);
		}

		protected IType GetEntity(TypeReference node)
		{
			return (IType)TypeSystemServices.GetEntity(node);
		}

		protected IEntity GetEntity(Node node)
		{
			return TypeSystemServices.GetEntity(node);
		}

		protected IMethod GetEntity(Method node)
		{
			return (IMethod)TypeSystemServices.GetEntity(node);
		}

		protected IProperty GetEntity(Property node)
		{
			return (IProperty)TypeSystemServices.GetEntity(node);
		}

		protected virtual IType GetExpressionType(Expression node)
		{
			return TypeSystemServices.GetExpressionType(node);
		}

		protected void BindExpressionType(Expression node, IType type)
		{
			_context.TraceVerbose("{0}: Type of expression '{1}' bound to '{2}'.", node.LexicalInfo, node, type);
			node.ExpressionType = type;
		}
	}
}
