using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Services;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class ForeignReferenceCollector : FastDepthFirstVisitor
	{
		private IType _currentType;

		private List _references;

		private List _recursiveReferences;

		private Hash _referencedEntities;

		private SelfEntity _selfEntity;

		private EnvironmentProvision<BooCodeBuilder> _codeBuilder = default(EnvironmentProvision<BooCodeBuilder>);

		private EnvironmentProvision<UniqueNameProvider> _uniqueNameProvider;

		public Node SourceNode { get; set; }

		public Method CurrentMethod { get; set; }

		public IType CurrentType
		{
			get
			{
				return _currentType;
			}
			set
			{
				_currentType = value;
				if (null != _selfEntity)
				{
					_selfEntity.Type = value;
				}
			}
		}

		public List References => _references;

		public Hash ReferencedEntities => _referencedEntities;

		public bool ContainsForeignLocalReferences
		{
			get
			{
				foreach (IEntity key in _referencedEntities.Keys)
				{
					EntityType entityType = key.EntityType;
					if (entityType == EntityType.Local || entityType == EntityType.Parameter)
					{
						return true;
					}
				}
				return false;
			}
		}

		protected BooCodeBuilder CodeBuilder => _codeBuilder;

		public ForeignReferenceCollector()
		{
			_references = new List();
			_recursiveReferences = new List();
			_referencedEntities = new Hash();
		}

		protected IEntity GetSelfEntity()
		{
			return _selfEntity ?? (_selfEntity = new SelfEntity("this", CurrentType));
		}

		public BooClassBuilder CreateSkeletonClass(string name, LexicalInfo lexicalInfo)
		{
			BooClassBuilder booClassBuilder = CodeBuilder.CreateClass(name);
			booClassBuilder.Modifiers |= TypeMemberModifiers.Internal;
			booClassBuilder.LexicalInfo = lexicalInfo;
			booClassBuilder.AddBaseType(CodeBuilder.TypeSystemServices.ObjectType);
			DeclareFieldsAndConstructor(booClassBuilder);
			return booClassBuilder;
		}

		public void DeclareFieldsAndConstructor(BooClassBuilder builder)
		{
			object[] array = Builtins.array(_referencedEntities.Keys);
			for (int i = 0; i < array.Length; i++)
			{
				ITypedEntity typedEntity = (ITypedEntity)array[i];
				Field field = builder.AddInternalField(GetUniqueName(typedEntity.Name), typedEntity.Type);
				_referencedEntities[typedEntity] = field.Entity;
			}
			BooMethodBuilder booMethodBuilder = builder.AddConstructor();
			booMethodBuilder.Modifiers = TypeMemberModifiers.Public;
			booMethodBuilder.Body.Add(CodeBuilder.CreateSuperConstructorInvocation(builder.Entity.BaseType));
			foreach (ITypedEntity key in _referencedEntities.Keys)
			{
				InternalField internalField = (InternalField)_referencedEntities[key];
				ParameterDeclaration parameter = booMethodBuilder.AddParameter(internalField.Name, key.Type);
				booMethodBuilder.Body.Add(CodeBuilder.CreateAssignment(CodeBuilder.CreateReference(internalField), CodeBuilder.CreateReference(parameter)));
			}
		}

		private string GetUniqueName(string name)
		{
			return _uniqueNameProvider.Instance.GetUniqueName(name);
		}

		public void AdjustReferences()
		{
			foreach (Expression reference in _references)
			{
				InternalField internalField = (InternalField)_referencedEntities[reference.Entity];
				if (null != internalField)
				{
					reference.ParentNode.Replace(reference, CodeBuilder.CreateReference(internalField));
				}
			}
			foreach (ReferenceExpression recursiveReference in _recursiveReferences)
			{
				recursiveReference.ParentNode.Replace(recursiveReference, CodeBuilder.MemberReferenceForEntity(CodeBuilder.CreateSelfReference((IType)CurrentMethod.DeclaringType.Entity), CurrentMethod.Entity));
			}
		}

		public MethodInvocationExpression CreateConstructorInvocationWithReferencedEntities(IType type)
		{
			MethodInvocationExpression methodInvocationExpression = CodeBuilder.CreateConstructorInvocation(type.GetConstructors().First());
			foreach (ITypedEntity key in _referencedEntities.Keys)
			{
				methodInvocationExpression.Arguments.Add(CreateForeignReference(key));
			}
			return methodInvocationExpression;
		}

		public Expression CreateForeignReference(IEntity entity)
		{
			if (_selfEntity == entity)
			{
				return CodeBuilder.CreateSelfReference(CurrentType);
			}
			return CodeBuilder.CreateReference(entity);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			if (IsRecursiveReference(node))
			{
				_recursiveReferences.Add(node);
			}
			else
			{
				Visit(node.Target);
			}
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			if (IsForeignReference(node))
			{
				_references.Add(node);
				_referencedEntities[node.Entity] = null;
			}
		}

		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			IEntity key = (node.Entity = GetSelfEntity());
			_references.Add(node);
			_referencedEntities[key] = null;
		}

		private bool IsRecursiveReference(Node node)
		{
			return CurrentMethod != null && node.Entity == CurrentMethod.Entity;
		}

		private bool IsForeignReference(ReferenceExpression node)
		{
			IEntity entity = node.Entity;
			if (null != entity)
			{
				switch (entity.EntityType)
				{
				case EntityType.Local:
					return CurrentMethod == null || !CurrentMethod.Locals.ContainsEntity(entity);
				case EntityType.Parameter:
					return CurrentMethod == null || !CurrentMethod.Parameters.ContainsEntity(entity);
				}
			}
			return false;
		}
	}
}
