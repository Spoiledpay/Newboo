using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalMethod : InternalEntity<Method>, IMethod, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IOverridableMember, INamespace, IEntity
	{
		private sealed class LabelCollector : FastDepthFirstVisitor
		{
			private static readonly InternalLabel[] EmptyInternalLabelArray = new InternalLabel[0];

			private List _labels;

			public InternalLabel[] Labels
			{
				get
				{
					if (null == _labels)
					{
						return EmptyInternalLabelArray;
					}
					return (InternalLabel[])_labels.ToArray(new InternalLabel[_labels.Count]);
				}
			}

			public override void OnLabelStatement(LabelStatement node)
			{
				if (null == _labels)
				{
					_labels = new List();
				}
				_labels.Add(node.Entity);
			}
		}

		protected InternalTypeSystemProvider _provider;

		protected IMethod _override;

		protected ExpressionCollection _returnExpressions;

		protected List _yieldStatements;

		public bool IsDuckTyped => My<TypeSystemServices>.Instance.IsDuckType(ReturnType);

		public bool IsPInvoke => IsAttributeDefined(Types.DllImportAttribute);

		public bool IsAbstract => _node.IsAbstract;

		public bool IsVirtual => _node.IsVirtual || _node.IsAbstract || _node.IsOverride;

		public bool IsFinal => _node.IsFinal;

		public bool IsSpecialName => false;

		public bool AcceptVarArgs => _node.Parameters.HasParamArray;

		public override EntityType EntityType => EntityType.Method;

		public ICallableType CallableType => My<TypeSystemServices>.Instance.GetCallableType(this);

		public IType Type => CallableType;

		public Method Method => _node;

		public IMethod Overriden
		{
			get
			{
				return _override;
			}
			set
			{
				_override = value;
			}
		}

		public virtual IType ReturnType
		{
			get
			{
				if (null == _node.ReturnType)
				{
					return (_node.DeclaringType.NodeType == NodeType.ClassDefinition) ? Unknown.Default : _provider.VoidType;
				}
				return TypeSystemServices.GetType(_node.ReturnType);
			}
		}

		public INamespace ParentNamespace => base.DeclaringType;

		public bool IsGenerator => null != _yieldStatements;

		public ExpressionCollection ReturnExpressions => _returnExpressions;

		public ExpressionCollection YieldExpressions
		{
			get
			{
				ExpressionCollection expressionCollection = new ExpressionCollection();
				foreach (YieldStatement yieldStatement in _yieldStatements)
				{
					if (null != yieldStatement.Expression)
					{
						expressionCollection.Add(yieldStatement.Expression);
					}
				}
				return expressionCollection;
			}
		}

		public InternalLabel[] Labels
		{
			get
			{
				LabelCollector labelCollector = new LabelCollector();
				_node.Accept(labelCollector);
				return labelCollector.Labels;
			}
		}

		public virtual IConstructedMethodInfo ConstructedInfo => null;

		public virtual IGenericMethodInfo GenericInfo => null;

		public bool IsNew => _node.IsNew;

		internal InternalMethod(InternalTypeSystemProvider provider, Method method)
			: base(method)
		{
			_provider = provider;
		}

		public IParameter[] GetParameters()
		{
			return _provider.Map(_node.Parameters);
		}

		public void AddYieldStatement(YieldStatement stmt)
		{
			if (null == _yieldStatements)
			{
				_yieldStatements = new List();
			}
			_yieldStatements.Add(stmt);
		}

		public void AddReturnExpression(Expression expression)
		{
			if (null == _returnExpressions)
			{
				_returnExpressions = new ExpressionCollection();
			}
			_returnExpressions.Add(expression);
		}

		public Local ResolveLocal(string name)
		{
			LocalCollection locals = _node.Locals;
			for (int i = 0; i < locals.Count; i++)
			{
				Local local = locals[i];
				if (!local.PrivateScope && string.CompareOrdinal(name, local.Name) == 0)
				{
					return local;
				}
			}
			return null;
		}

		public ParameterDeclaration ResolveParameter(string name)
		{
			foreach (ParameterDeclaration parameter in _node.Parameters)
			{
				if (name == parameter.Name)
				{
					return parameter;
				}
			}
			return null;
		}

		public virtual bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (Entities.IsFlagSet(typesToConsider, EntityType.Local))
			{
				Local local = ResolveLocal(name);
				if (null != local)
				{
					resultingSet.Add(TypeSystemServices.GetEntity(local));
					return true;
				}
			}
			if (Entities.IsFlagSet(typesToConsider, EntityType.Parameter))
			{
				ParameterDeclaration parameterDeclaration = ResolveParameter(name);
				if (null != parameterDeclaration)
				{
					resultingSet.Add(TypeSystemServices.GetEntity(parameterDeclaration));
					return true;
				}
			}
			return false;
		}

		IEnumerable<IEntity> INamespace.GetMembers()
		{
			return NullNamespace.EmptyEntityArray;
		}

		public override string ToString()
		{
			return _node.FullName;
		}
	}
}
