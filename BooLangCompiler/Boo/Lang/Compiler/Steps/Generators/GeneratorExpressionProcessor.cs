using System;
using System.Collections;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.Generators
{
	internal class GeneratorExpressionProcessor : AbstractCompilerComponent
	{
		private GeneratorExpression _generator;

		private BooClassBuilder _enumerator;

		private Field _current;

		private Field _enumeratorField;

		private ForeignReferenceCollector _collector;

		private IType _sourceItemType;

		private IType _sourceEnumeratorType;

		private IType _sourceEnumerableType;

		private IType _resultEnumeratorType;

		private GeneratorSkeleton _skeleton;

		public GeneratorExpressionProcessor(CompilerContext context, ForeignReferenceCollector collector, GeneratorExpression node)
		{
			_collector = collector;
			_generator = node;
			_skeleton = My<GeneratorSkeletonBuilder>.Instance.SkeletonFor(node, node.GetAncestor<Method>());
			Initialize(context);
		}

		public void Run()
		{
			RemoveReferencedDeclarations();
			CreateAnonymousGeneratorType();
		}

		private void RemoveReferencedDeclarations()
		{
			Hash referencedEntities = _collector.ReferencedEntities;
			foreach (Declaration declaration in _generator.Declarations)
			{
				referencedEntities.Remove(declaration.Entity);
			}
		}

		private void CreateAnonymousGeneratorType()
		{
			_sourceItemType = base.TypeSystemServices.ObjectType;
			_sourceEnumeratorType = base.TypeSystemServices.IEnumeratorType;
			_sourceEnumerableType = base.TypeSystemServices.IEnumerableType;
			_resultEnumeratorType = base.TypeSystemServices.IEnumeratorGenericType.GenericInfo.ConstructType(_skeleton.GeneratorItemType);
			_enumerator = _collector.CreateSkeletonClass("Enumerator", _generator.LexicalInfo);
			_sourceItemType = base.TypeSystemServices.GetGenericEnumerableItemType(_generator.Iterator.ExpressionType);
			if (_sourceItemType != null && _sourceItemType != base.TypeSystemServices.ObjectType)
			{
				_sourceEnumerableType = base.TypeSystemServices.IEnumerableGenericType.GenericInfo.ConstructType(_sourceItemType);
				_sourceEnumeratorType = base.TypeSystemServices.IEnumeratorGenericType.GenericInfo.ConstructType(_sourceItemType);
			}
			else
			{
				_sourceItemType = base.TypeSystemServices.ObjectType;
			}
			_enumerator.AddBaseType(_resultEnumeratorType);
			_enumerator.AddBaseType(base.TypeSystemServices.Map(typeof(ICloneable)));
			_enumerator.AddBaseType(base.TypeSystemServices.IDisposableType);
			_enumeratorField = _enumerator.AddField("$$enumerator", _sourceEnumeratorType);
			_current = _enumerator.AddField("$$current", _skeleton.GeneratorItemType);
			CreateReset();
			CreateCurrent();
			CreateMoveNext();
			CreateClone();
			CreateDispose();
			EnumeratorConstructorMustCallReset();
			_collector.AdjustReferences();
			BooClassBuilder generatorClassBuilder = _skeleton.GeneratorClassBuilder;
			_collector.DeclareFieldsAndConstructor(generatorClassBuilder);
			CreateGetEnumerator();
			generatorClassBuilder.ClassDefinition.Members.Add(_enumerator.ClassDefinition);
		}

		public MethodInvocationExpression CreateEnumerableConstructorInvocation()
		{
			return _collector.CreateConstructorInvocationWithReferencedEntities(_skeleton.GeneratorClassBuilder.Entity);
		}

		private void EnumeratorConstructorMustCallReset()
		{
			Constructor constructor = _enumerator.ClassDefinition.GetConstructor(0);
			constructor.Body.Add(CreateMethodInvocation(_enumerator.ClassDefinition, "Reset"));
		}

		private IMethod GetMemberwiseCloneMethod()
		{
			return base.TypeSystemServices.Map(typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		private MethodInvocationExpression CreateMethodInvocation(ClassDefinition cd, string name)
		{
			IMethod method = (IMethod)((Method)cd.Members[name]).Entity;
			return base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateSelfReference(method.DeclaringType), method);
		}

		private void CreateCurrent()
		{
			Property property = _enumerator.AddReadOnlyProperty("Current", base.TypeSystemServices.ObjectType);
			property.Getter.Modifiers |= TypeMemberModifiers.Virtual;
			property.Getter.Body.Add(new ReturnStatement(base.CodeBuilder.CreateReference(_current)));
			if (_skeleton.GeneratorItemType != base.TypeSystemServices.ObjectType)
			{
				property.ExplicitInfo = new ExplicitMemberInfo();
				property.ExplicitInfo.InterfaceType = (SimpleTypeReference)base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.IEnumeratorType);
				Property property2 = _enumerator.AddReadOnlyProperty("Current", _skeleton.GeneratorItemType);
				property2.Getter.Modifiers |= TypeMemberModifiers.Virtual;
				property2.Getter.Body.Add(new ReturnStatement(base.CodeBuilder.CreateReference(_current)));
			}
		}

		private void CreateGetEnumerator()
		{
			BooMethodBuilder getEnumeratorBuilder = _skeleton.GetEnumeratorBuilder;
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateConstructorInvocation(_enumerator.ClassDefinition);
			foreach (TypeMember member2 in _skeleton.GeneratorClassBuilder.ClassDefinition.Members)
			{
				if (NodeType.Field == member2.NodeType)
				{
					IField member = (IField)member2.Entity;
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateMemberReference(member));
				}
			}
			getEnumeratorBuilder.Body.Add(new ReturnStatement(methodInvocationExpression));
		}

		private void CreateClone()
		{
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("Clone", base.TypeSystemServices.ObjectType);
			booMethodBuilder.Body.Add(new ReturnStatement(base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateSelfReference(_enumerator.Entity), GetMemberwiseCloneMethod())));
		}

		private void CreateReset()
		{
			IMethod entity = (IMethod)GetMember(_sourceEnumerableType, "GetEnumerator", EntityType.Method);
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("Reset", base.TypeSystemServices.VoidType);
			booMethodBuilder.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference((InternalField)_enumeratorField.Entity), base.CodeBuilder.CreateMethodInvocation(_generator.Iterator, entity)));
		}

		private void CreateMoveNext()
		{
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("MoveNext", base.TypeSystemServices.BoolType);
			Expression condition = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference((InternalField)_enumeratorField.Entity), base.TypeSystemServices.Map(Methods.InstanceFunctionOf<IEnumerator, bool>((IEnumerator e) => e.MoveNext)));
			Expression expression = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference((InternalField)_enumeratorField.Entity), ((IProperty)GetMember(_sourceEnumeratorType, "Current", EntityType.Property)).GetGetMethod());
			Statement statement = null;
			Statement statement2 = null;
			Block block = null;
			Block block2 = null;
			if (null == _generator.Filter)
			{
				IfStatement ifStatement = new IfStatement(condition, new Block(), null);
				block = (block2 = ifStatement.TrueBlock);
				statement2 = ifStatement;
			}
			else
			{
				WhileStatement whileStatement = new WhileStatement(condition);
				block = whileStatement.Block;
				if (StatementModifierType.If == _generator.Filter.Type)
				{
					IfStatement ifStatement2 = new IfStatement(_generator.Filter.Condition, new Block(), null);
					block2 = ifStatement2.TrueBlock;
					statement = ifStatement2;
				}
				else
				{
					UnlessStatement unlessStatement = new UnlessStatement(_generator.Filter.Condition);
					block2 = unlessStatement.Block;
					statement = unlessStatement;
				}
				statement2 = whileStatement;
			}
			DeclarationCollection declarations = _generator.Declarations;
			if (declarations.Count > 1)
			{
				NormalizeIterationStatements.UnpackExpression(base.CodeBuilder, booMethodBuilder.Method, block, expression, declarations);
				foreach (Declaration item in declarations)
				{
					booMethodBuilder.Locals.Add(((InternalLocal)item.Entity).Local);
				}
			}
			else
			{
				InternalLocal internalLocal = (InternalLocal)declarations[0].Entity;
				booMethodBuilder.Locals.Add(internalLocal.Local);
				block.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(internalLocal), expression));
			}
			if (null != statement)
			{
				block.Add(statement);
			}
			block2.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference((InternalField)_current.Entity), _generator.Expression));
			block2.Add(new ReturnStatement(new BoolLiteralExpression(value: true)));
			booMethodBuilder.Body.Add(statement2);
			booMethodBuilder.Body.Add(new ReturnStatement(new BoolLiteralExpression(value: false)));
		}

		private void CreateDispose()
		{
			BooMethodBuilder booMethodBuilder = _enumerator.AddVirtualMethod("Dispose", base.TypeSystemServices.VoidType);
			if (TypeCompatibilityRules.IsAssignableFrom(base.TypeSystemServices.IDisposableType, _sourceEnumeratorType))
			{
				booMethodBuilder.Body.Add(base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(_enumeratorField), Methods.InstanceActionOf((IDisposable d) => d.Dispose)));
			}
		}

		private IEntity GetMember(IType type, string name, EntityType entityType)
		{
			return base.NameResolutionService.ResolveMember(type, name, entityType);
		}
	}
}
