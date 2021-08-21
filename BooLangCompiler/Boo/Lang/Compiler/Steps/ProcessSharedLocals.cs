using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessSharedLocals : AbstractTransformerCompilerStep
	{
		private Method _currentMethod;

		private ClassDefinition _sharedLocalsClass;

		private Hashtable _mappings = new Hashtable();

		private readonly List<ReferenceExpression> _references = new List<ReferenceExpression>();

		private readonly List<ILocalEntity> _shared = new List<ILocalEntity>();

		private int _closureDepth;

		public override void Dispose()
		{
			_shared.Clear();
			_references.Clear();
			_mappings.Clear();
			base.Dispose();
		}

		public override void OnField(Field node)
		{
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
		}

		public override void OnConstructor(Constructor node)
		{
			OnMethod(node);
		}

		public override void OnMethod(Method node)
		{
			_references.Clear();
			_mappings.Clear();
			_currentMethod = node;
			_sharedLocalsClass = null;
			_closureDepth = 0;
			Visit(node.Body);
			CreateSharedLocalsClass();
			if (null != _sharedLocalsClass)
			{
				node.DeclaringType.Members.Add(_sharedLocalsClass);
				Map();
			}
		}

		public override void OnBlockExpression(BlockExpression node)
		{
			_closureDepth++;
			Visit(node.Body);
			_closureDepth--;
		}

		public override void OnGeneratorExpression(GeneratorExpression node)
		{
			_closureDepth++;
			Visit(node.Iterator);
			Visit(node.Expression);
			Visit(node.Filter);
			_closureDepth--;
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			ILocalEntity localEntity = node.Entity as ILocalEntity;
			if (null != localEntity && !localEntity.IsPrivateScope)
			{
				_references.Add(node);
				if (_closureDepth != 0)
				{
					localEntity.IsShared = _currentMethod.Locals.ContainsEntity(localEntity) || _currentMethod.Parameters.ContainsEntity(localEntity);
				}
			}
		}

		private void Map()
		{
			IType type = (IType)_sharedLocalsClass.Entity;
			InternalLocal internalLocal = base.CodeBuilder.DeclareLocal(_currentMethod, "$locals", type);
			foreach (ReferenceExpression reference in _references)
			{
				IField field = (IField)_mappings[reference.Entity];
				if (null != field)
				{
					reference.ParentNode.Replace(reference, base.CodeBuilder.CreateMemberReference(base.CodeBuilder.CreateReference(internalLocal), field));
				}
			}
			Block block = new Block();
			block.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(internalLocal), base.CodeBuilder.CreateConstructorInvocation(type.GetConstructors().First())));
			InitializeSharedParameters(block, internalLocal);
			_currentMethod.Body.Statements.Insert(0, block);
			foreach (IEntity key in _mappings.Keys)
			{
				_currentMethod.Locals.RemoveByEntity(key);
			}
		}

		private void InitializeSharedParameters(Block block, InternalLocal locals)
		{
			foreach (ParameterDeclaration parameter in _currentMethod.Parameters)
			{
				InternalParameter internalParameter = (InternalParameter)parameter.Entity;
				if (internalParameter.IsShared)
				{
					block.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateMemberReference(base.CodeBuilder.CreateReference(locals), (IField)_mappings[internalParameter]), base.CodeBuilder.CreateReference(internalParameter)));
				}
			}
		}

		private void CreateSharedLocalsClass()
		{
			_shared.Clear();
			CollectSharedLocalEntities(_currentMethod.Locals);
			CollectSharedLocalEntities(_currentMethod.Parameters);
			if (_shared.Count <= 0)
			{
				return;
			}
			BooClassBuilder booClassBuilder = base.CodeBuilder.CreateClass(base.Context.GetUniqueName(_currentMethod.Name, "locals"));
			booClassBuilder.Modifiers |= TypeMemberModifiers.Internal;
			booClassBuilder.AddBaseType(base.TypeSystemServices.ObjectType);
			int num = 0;
			foreach (ILocalEntity item in _shared)
			{
				Field field = booClassBuilder.AddInternalField($"${item.Name}", item.Type);
				num++;
				_mappings[item] = field.Entity;
			}
			booClassBuilder.AddConstructor().Body.Add(base.CodeBuilder.CreateSuperConstructorInvocation(base.TypeSystemServices.ObjectType));
			_sharedLocalsClass = booClassBuilder.ClassDefinition;
		}

		private void CollectSharedLocalEntities<T>(IEnumerable<T> nodes) where T : Node
		{
			foreach (T node in nodes)
			{
				T current = node;
				ILocalEntity localEntity = (ILocalEntity)current.Entity;
				if (localEntity.IsShared)
				{
					_shared.Add(localEntity);
				}
			}
		}
	}
}
