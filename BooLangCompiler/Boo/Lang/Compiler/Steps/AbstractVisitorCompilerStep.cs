#define TRACE
using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractVisitorCompilerStep : DepthFirstVisitor, ICompilerStep, ICompilerComponent, IDisposable
	{
		protected CompilerContext _context;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService;

		private EnvironmentProvision<TypeSystemServices> _typeSystemServices;

		private readonly object VisitedAnnotationKey = new object();

		protected CompilerContext Context => _context;

		protected BooCodeBuilder CodeBuilder => _context.CodeBuilder;

		protected CompileUnit CompileUnit => _context.CompileUnit;

		protected NameResolutionService NameResolutionService => _nameResolutionService;

		protected CompilerParameters Parameters => _context.Parameters;

		protected CompilerErrorCollection Errors => _context.Errors;

		protected CompilerWarningCollection Warnings => _context.Warnings;

		protected TypeSystemServices TypeSystemServices => _typeSystemServices;

		public override void OnQuasiquoteExpression(QuasiquoteExpression node)
		{
		}

		protected override void OnError(Node node, Exception error)
		{
			_context.TraceError("{0}: Internal compiler error on node '{2}': {1}", node.LexicalInfo, error, node);
			base.OnError(node, error);
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

		protected void Bind(Node node, IEntity tag)
		{
			_context.TraceVerbose("{0}: Node '{1}' bound to '{2}'.", node.LexicalInfo, node, tag);
			node.Entity = tag;
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

		protected void BindExpressionType(Expression node, IType type)
		{
			_context.TraceVerbose("{0}: Type of expression '{1}' bound to '{2}'.", node.LexicalInfo, node, type);
			node.ExpressionType = type;
		}

		protected IType GetConcreteExpressionType(Expression expression)
		{
			return TypeSystemServices.GetConcreteExpressionType(expression);
		}

		protected virtual IType GetExpressionType(Expression node)
		{
			return TypeSystemServices.GetExpressionType(node);
		}

		public IType GetType(Node node)
		{
			return TypeSystemServices.GetType(node);
		}

		protected void NotImplemented(Node node, string feature)
		{
			throw CompilerErrorFactory.NotImplemented(node, feature);
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

		protected void MarkVisited(Node node)
		{
			node[VisitedAnnotationKey] = VisitedAnnotationKey;
			_context.TraceInfo("{0}: node '{1}' mark visited.", node.LexicalInfo, node);
		}

		protected virtual void EnsureRelatedNodeWasVisited(Node sourceNode, IEntity entity)
		{
			IInternalEntity constructedInternalEntity = GetConstructedInternalEntity(entity);
			if (null != constructedInternalEntity)
			{
				Node node = constructedInternalEntity.Node;
				if (!WasVisited(node))
				{
					Visit(node);
				}
			}
		}

		protected static IInternalEntity GetConstructedInternalEntity(IEntity entity)
		{
			IConstructedMethodInfo constructedMethodInfo = entity as IConstructedMethodInfo;
			if (null != constructedMethodInfo)
			{
				entity = constructedMethodInfo.GenericDefinition;
			}
			IConstructedTypeInfo constructedTypeInfo = entity as IConstructedTypeInfo;
			if (null != constructedTypeInfo)
			{
				entity = constructedTypeInfo.GenericDefinition;
			}
			return entity as IInternalEntity;
		}

		protected bool WasVisited(Node node)
		{
			return node.ContainsAnnotation(VisitedAnnotationKey);
		}

		public virtual void Run()
		{
			Visit(CompileUnit);
		}
	}
}
