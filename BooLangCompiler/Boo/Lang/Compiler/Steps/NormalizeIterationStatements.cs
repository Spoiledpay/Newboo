using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.Steps
{
	public class NormalizeIterationStatements : AbstractTransformerCompilerStep
	{
		internal class OrBlockNormalizer : DepthFirstTransformer
		{
			private readonly CompilerContext _context;

			private Method _currentMethod;

			private EnvironmentProvision<TypeSystemServices> _typeSystemServices;

			public OrBlockNormalizer(CompilerContext context)
			{
				_context = context;
			}

			public override bool EnterMethod(Method node)
			{
				_currentMethod = node;
				return base.EnterMethod(node);
			}

			public override void OnWhileStatement(WhileStatement node)
			{
				if (node.OrBlock != null)
				{
					InternalLocal internalLocal = CodeBuilder().DeclareTempLocal(_currentMethod, BoolType());
					IfStatement ifStatement = new IfStatement(node.OrBlock.LexicalInfo, CodeBuilder().CreateNotExpression(CodeBuilder().CreateReference(internalLocal)), node.OrBlock, null);
					node.OrBlock = ifStatement.ToBlock();
					node.Block.Insert(0, CodeBuilder().CreateAssignment(CreateReference(internalLocal), CreateTrueLiteral()));
				}
			}

			private BoolLiteralExpression CreateTrueLiteral()
			{
				return CodeBuilder().CreateBoolLiteral(value: true);
			}

			private ReferenceExpression CreateReference(InternalLocal enteredLoop)
			{
				return CodeBuilder().CreateReference(enteredLoop);
			}

			private IType BoolType()
			{
				return _typeSystemServices.Instance.BoolType;
			}

			private BooCodeBuilder CodeBuilder()
			{
				return _context.CodeBuilder;
			}
		}

		private static MethodInfo RuntimeServices_MoveNext = Methods.Of<IEnumerator, object>(RuntimeServices.MoveNext);

		private static MethodInfo RuntimeServices_GetEnumerable = Methods.Of<object, IEnumerable>(RuntimeServices.GetEnumerable);

		private static MethodInfo IEnumerable_GetEnumerator = Methods.InstanceFunctionOf<IEnumerable, IEnumerator>((IEnumerable e) => e.GetEnumerator);

		private static MethodInfo IDisposable_Dispose = Methods.InstanceActionOf((IDisposable d) => d.Dispose);

		private Method _current;

		private Node _iteratorNode;

		private IType _enumeratorType;

		private IType _enumeratorItemType;

		private IType _bestEnumeratorType;

		private IMethod _bestGetEnumerator;

		private IMethod _bestMoveNext;

		private IMethod _bestGetCurrent;

		private System.Collections.Generic.List<IEntity> _candidates = new System.Collections.Generic.List<IEntity>();

		private IType CurrentEnumeratorType
		{
			get
			{
				return _enumeratorType;
			}
			set
			{
				if (_enumeratorType != value)
				{
					_enumeratorItemType = null;
					_bestEnumeratorType = null;
					_bestGetEnumerator = null;
					_bestMoveNext = null;
					_bestGetCurrent = null;
					_enumeratorType = value;
				}
			}
		}

		private IType CurrentEnumeratorItemType
		{
			get
			{
				if (null == _enumeratorItemType)
				{
					_enumeratorItemType = base.TypeSystemServices.GetEnumeratorItemType(CurrentEnumeratorType);
				}
				return _enumeratorItemType;
			}
		}

		private IType CurrentBestEnumeratorType
		{
			get
			{
				if (null == _bestEnumeratorType)
				{
					_bestEnumeratorType = FindBestEnumeratorType();
				}
				return _bestEnumeratorType;
			}
		}

		private IMethod CurrentBestGetEnumerator
		{
			get
			{
				if (null == _bestGetEnumerator)
				{
					_bestGetEnumerator = FindBestEnumeratorMethod("GetEnumerator");
				}
				return _bestGetEnumerator;
			}
		}

		private IMethod CurrentBestMoveNext
		{
			get
			{
				if (null == _bestMoveNext)
				{
					_bestMoveNext = FindBestEnumeratorMethod("MoveNext");
				}
				return _bestMoveNext;
			}
		}

		private IMethod CurrentBestGetCurrent
		{
			get
			{
				if (null == _bestGetCurrent)
				{
					_bestGetCurrent = FindBestEnumeratorMethod("Current");
				}
				return _bestGetCurrent;
			}
		}

		public override void Run()
		{
			Visit(base.CompileUnit);
			base.CompileUnit.Accept(new OrBlockNormalizer(base.Context));
		}

		public override void OnMethod(Method node)
		{
			_current = node;
			Visit(node.Body);
		}

		public override void OnConstructor(Constructor node)
		{
			OnMethod(node);
		}

		public override void OnDestructor(Destructor node)
		{
			OnMethod(node);
		}

		public override void OnBlockExpression(BlockExpression node)
		{
		}

		public override void LeaveUnpackStatement(UnpackStatement node)
		{
			Block block = new Block(node.LexicalInfo);
			UnpackExpression(block, node.Expression, node.Declarations);
			ReplaceCurrentNode(block);
		}

		public override void LeaveForStatement(ForStatement node)
		{
			_iteratorNode = node.Iterator;
			CurrentEnumeratorType = GetExpressionType(node.Iterator);
			if (null == CurrentBestEnumeratorType)
			{
				return;
			}
			DeclarationCollection declarations = node.Declarations;
			Block block = new Block(node.LexicalInfo);
			InternalLocal local = base.CodeBuilder.DeclareLocal(_current, base.Context.GetUniqueName("iterator"), CurrentBestEnumeratorType);
			if (CurrentBestEnumeratorType == CurrentEnumeratorType)
			{
				block.Add(base.CodeBuilder.CreateAssignment(node.LexicalInfo, base.CodeBuilder.CreateReference(local), node.Iterator));
			}
			else
			{
				block.Add(base.CodeBuilder.CreateAssignment(node.LexicalInfo, base.CodeBuilder.CreateReference(local), base.CodeBuilder.CreateMethodInvocation(node.Iterator, CurrentBestGetEnumerator)));
			}
			if (null == CurrentBestMoveNext)
			{
				return;
			}
			WhileStatement whileStatement = new WhileStatement(node.LexicalInfo);
			whileStatement.Condition = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(local), CurrentBestMoveNext);
			if (null != CurrentBestGetCurrent)
			{
				Expression expression = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(local), CurrentBestGetCurrent);
				if (1 == declarations.Count)
				{
					whileStatement.Block.Add(base.CodeBuilder.CreateAssignment(node.LexicalInfo, base.CodeBuilder.CreateReference((InternalLocal)declarations[0].Entity), expression));
				}
				else
				{
					UnpackExpression(whileStatement.Block, base.CodeBuilder.CreateCast(CurrentEnumeratorItemType, expression), node.Declarations);
				}
				whileStatement.Block.Add(node.Block);
				whileStatement.OrBlock = node.OrBlock;
				whileStatement.ThenBlock = node.ThenBlock;
				if (IsAssignableFrom(base.TypeSystemServices.IDisposableType, CurrentBestEnumeratorType))
				{
					TryStatement tryStatement = new TryStatement();
					tryStatement.ProtectedBlock.Add(whileStatement);
					tryStatement.EnsureBlock = new Block();
					CastExpression castExpression = new CastExpression();
					castExpression.Type = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.IDisposableType);
					castExpression.Target = base.CodeBuilder.CreateReference(local);
					castExpression.ExpressionType = base.TypeSystemServices.IDisposableType;
					tryStatement.EnsureBlock.Add(base.CodeBuilder.CreateMethodInvocation(castExpression, IDisposable_Dispose));
					block.Add(tryStatement);
				}
				else
				{
					block.Add(whileStatement);
				}
				ReplaceCurrentNode(block);
			}
		}

		private bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		private void UnpackExpression(Block block, Expression expression, DeclarationCollection declarations)
		{
			UnpackExpression(base.CodeBuilder, _current, block, expression, declarations);
		}

		public static void UnpackExpression(BooCodeBuilder codeBuilder, Method method, Block block, Expression expression, DeclarationCollection declarations)
		{
			if (expression.ExpressionType.IsArray)
			{
				UnpackArray(codeBuilder, method, block, expression, declarations);
			}
			else
			{
				UnpackEnumerable(codeBuilder, method, block, expression, declarations);
			}
		}

		public static void UnpackEnumerable(BooCodeBuilder codeBuilder, Method method, Block block, Expression expression, DeclarationCollection declarations)
		{
			TypeSystemServices typeSystemServices = codeBuilder.TypeSystemServices;
			InternalLocal local = codeBuilder.DeclareTempLocal(method, typeSystemServices.IEnumeratorType);
			IType expressionType = expression.ExpressionType;
			if (expressionType.IsSubclassOf(codeBuilder.TypeSystemServices.IEnumeratorType))
			{
				block.Add(codeBuilder.CreateAssignment(codeBuilder.CreateReference(local), expression));
			}
			else
			{
				if (!expressionType.IsSubclassOf(codeBuilder.TypeSystemServices.IEnumerableType))
				{
					expression = codeBuilder.CreateMethodInvocation(RuntimeServices_GetEnumerable, expression);
				}
				block.Add(codeBuilder.CreateAssignment(block.LexicalInfo, codeBuilder.CreateReference(local), codeBuilder.CreateMethodInvocation(expression, IEnumerable_GetEnumerator)));
			}
			for (int i = 0; i < declarations.Count; i++)
			{
				Declaration declaration = declarations[i];
				block.Add(codeBuilder.CreateAssignment(codeBuilder.CreateReference(declaration.Entity), codeBuilder.CreateMethodInvocation(RuntimeServices_MoveNext, codeBuilder.CreateReference(local))));
			}
		}

		public static void UnpackArray(BooCodeBuilder codeBuilder, Method method, Block block, Expression expression, DeclarationCollection declarations)
		{
			ILocalEntity localEntity = expression.Entity as ILocalEntity;
			if (null == localEntity)
			{
				localEntity = codeBuilder.DeclareTempLocal(method, expression.ExpressionType);
				block.Add(codeBuilder.CreateAssignment(codeBuilder.CreateReference(localEntity), expression));
			}
			for (int i = 0; i < declarations.Count; i++)
			{
				Declaration declaration = declarations[i];
				block.Add(codeBuilder.CreateAssignment(codeBuilder.CreateReference(declaration.Entity), codeBuilder.CreateSlicing(codeBuilder.CreateReference(localEntity), i)));
			}
		}

		private IType FindBestEnumeratorType()
		{
			if (IsAssignableFrom(base.TypeSystemServices.IEnumeratorType, CurrentEnumeratorType))
			{
				return CurrentEnumeratorType;
			}
			IType type = null;
			_candidates.Clear();
			CurrentEnumeratorType.Resolve(_candidates, "GetEnumerator", EntityType.Method);
			foreach (IEntity candidate in _candidates)
			{
				IMethod method = (IMethod)candidate;
				if (method.GenericInfo != null || method.GetParameters().Length != 0 || !method.IsPublic || (!IsAssignableFrom(base.TypeSystemServices.IEnumeratorGenericType, method.ReturnType) && !IsAssignableFrom(base.TypeSystemServices.IEnumeratorType, method.ReturnType)))
				{
					continue;
				}
				type = method.ReturnType;
				_bestGetEnumerator = method;
				break;
			}
			if (null == type && IsAssignableFrom(base.TypeSystemServices.IEnumerableGenericType, CurrentEnumeratorType))
			{
				type = base.TypeSystemServices.IEnumeratorGenericType;
				_bestGetEnumerator = base.TypeSystemServices.Map(Types.IEnumerableGeneric.GetMethod("GetEnumerator"));
			}
			if (null == type && IsAssignableFrom(base.TypeSystemServices.IEnumerableType, CurrentEnumeratorType))
			{
				type = base.TypeSystemServices.IEnumeratorType;
				_bestGetEnumerator = base.TypeSystemServices.Map(Types.IEnumerable.GetMethod("GetEnumerator"));
			}
			if (null == type)
			{
				base.Errors.Add(CompilerErrorFactory.InvalidIteratorType(_iteratorNode, CurrentEnumeratorType));
			}
			return type;
		}

		private IMethod FindBestEnumeratorMethod(string name)
		{
			_candidates.Clear();
			CurrentBestEnumeratorType.Resolve(_candidates, name, EntityType.Method | EntityType.Property);
			foreach (IEntity candidate in _candidates)
			{
				if (candidate is IMethod)
				{
					IMethod method = (IMethod)candidate;
					if (method.GenericInfo != null || 0 != method.GetParameters().Length)
					{
						continue;
					}
					return method;
				}
				IProperty property = (IProperty)candidate;
				return property.GetGetMethod();
			}
			base.Errors.Add(CompilerErrorFactory.InvalidIteratorType(_iteratorNode, CurrentEnumeratorType));
			return null;
		}
	}
}
