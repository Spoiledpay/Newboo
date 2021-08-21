using System;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class OptimizeIterationStatements : AbstractTransformerCompilerStep
	{
		private sealed class EntityPredicate
		{
			private IEntity _entity;

			public EntityPredicate(IEntity entity)
			{
				_entity = entity;
			}

			public bool Matches(Node node)
			{
				return _entity == node.Entity;
			}
		}

		private static readonly MethodInfo Array_get_Length = Methods.GetterOf((Array a) => a.Length);

		private static readonly MethodInfo System_Math_Ceiling = Methods.Of<double, double>(Math.Ceiling);

		private static readonly ConstructorInfo System_ArgumentOutOfRangeException_ctor = Methods.ConstructorOf(() => new ArgumentOutOfRangeException(string.Empty));

		private IMethod _range_End;

		private IMethod _range_Begin_End;

		private IMethod _range_Begin_End_Step;

		private Method _currentMethod;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			Type typeFromHandle = typeof(Builtins);
			_range_End = Map(typeFromHandle.GetMethod("range", new Type[1] { Types.Int }));
			_range_Begin_End = Map(typeFromHandle.GetMethod("range", new Type[2]
			{
				Types.Int,
				Types.Int
			}));
			_range_Begin_End_Step = Map(typeFromHandle.GetMethod("range", new Type[3]
			{
				Types.Int,
				Types.Int,
				Types.Int
			}));
		}

		private IMethod Map(MethodInfo method)
		{
			return base.TypeSystemServices.Map(method);
		}

		public override void Run()
		{
			Visit(base.CompileUnit);
		}

		public override void OnMethod(Method node)
		{
			_currentMethod = node;
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

		public override void LeaveForStatement(ForStatement node)
		{
			if (node.Declarations.Count != 1 || null == AstUtil.GetLocalByName(_currentMethod, node.Declarations[0].Name))
			{
				CheckForItemInRangeLoop(node);
				CheckForItemInArrayLoop(node);
			}
		}

		private bool IsRangeInvocation(MethodInvocationExpression mi)
		{
			IEntity entity = mi.Target.Entity;
			return entity == _range_End || entity == _range_Begin_End || entity == _range_Begin_End_Step;
		}

		private void CheckForItemInRangeLoop(ForStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = node.Iterator as MethodInvocationExpression;
			if (null == methodInvocationExpression || !IsRangeInvocation(methodInvocationExpression))
			{
				return;
			}
			DeclarationCollection declarations = node.Declarations;
			if (declarations.Count != 1)
			{
				return;
			}
			ExpressionCollection arguments = methodInvocationExpression.Arguments;
			Block block = new Block(node.LexicalInfo);
			IntegerLiteralExpression integerLiteralExpression;
			Expression expression;
			Expression expression2;
			IntegerLiteralExpression integerLiteralExpression2;
			IntegerLiteralExpression integerLiteralExpression3;
			Expression expression3;
			if (arguments.Count == 1)
			{
				integerLiteralExpression = base.CodeBuilder.CreateIntegerLiteral(0);
				expression = integerLiteralExpression;
				expression2 = arguments[0];
				integerLiteralExpression2 = expression2 as IntegerLiteralExpression;
				integerLiteralExpression3 = base.CodeBuilder.CreateIntegerLiteral(1);
				expression3 = integerLiteralExpression3;
			}
			else if (arguments.Count == 2)
			{
				expression = arguments[0];
				integerLiteralExpression = expression as IntegerLiteralExpression;
				expression2 = arguments[1];
				integerLiteralExpression2 = expression2 as IntegerLiteralExpression;
				integerLiteralExpression3 = base.CodeBuilder.CreateIntegerLiteral(1);
				expression3 = integerLiteralExpression3;
			}
			else
			{
				expression = arguments[0];
				integerLiteralExpression = expression as IntegerLiteralExpression;
				expression2 = arguments[1];
				integerLiteralExpression2 = expression2 as IntegerLiteralExpression;
				expression3 = arguments[2];
				integerLiteralExpression3 = expression3 as IntegerLiteralExpression;
			}
			InternalLocal local = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
			Expression expression4 = base.CodeBuilder.CreateReference(local);
			block.Add(base.CodeBuilder.CreateAssignment(expression4, expression));
			Expression expression5;
			if (null != integerLiteralExpression2)
			{
				expression5 = expression2;
			}
			else
			{
				InternalLocal local2 = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
				expression5 = base.CodeBuilder.CreateReference(local2);
				block.Add(base.CodeBuilder.CreateAssignment(expression5, expression2));
			}
			if (arguments.Count == 1)
			{
				if (null != integerLiteralExpression2)
				{
					if (integerLiteralExpression2.Value < 0)
					{
						Statement stmt = base.CodeBuilder.RaiseException(block.LexicalInfo, base.TypeSystemServices.Map(System_ArgumentOutOfRangeException_ctor), base.CodeBuilder.CreateStringLiteral("max"));
						block.Add(stmt);
					}
				}
				else
				{
					IfStatement ifStatement = new IfStatement(block.LexicalInfo);
					ifStatement.TrueBlock = new Block();
					Statement stmt = base.CodeBuilder.RaiseException(block.LexicalInfo, base.TypeSystemServices.Map(System_ArgumentOutOfRangeException_ctor), base.CodeBuilder.CreateStringLiteral("max"));
					ifStatement.Condition = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression5, base.CodeBuilder.CreateIntegerLiteral(0));
					ifStatement.TrueBlock.Add(stmt);
					block.Add(ifStatement);
				}
			}
			Expression expression6;
			switch (arguments.Count)
			{
			case 1:
				expression6 = base.CodeBuilder.CreateIntegerLiteral(1);
				break;
			case 2:
			{
				if (integerLiteralExpression != null && null != integerLiteralExpression2)
				{
					expression6 = ((integerLiteralExpression2.Value >= integerLiteralExpression.Value) ? base.CodeBuilder.CreateIntegerLiteral(1) : base.CodeBuilder.CreateIntegerLiteral(-1));
					break;
				}
				InternalLocal local3 = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
				expression6 = base.CodeBuilder.CreateReference(local3);
				block.Add(base.CodeBuilder.CreateAssignment(expression6, base.CodeBuilder.CreateIntegerLiteral(1)));
				IfStatement ifStatement = new IfStatement(node.LexicalInfo);
				ifStatement.Condition = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression5, expression4);
				ifStatement.TrueBlock = new Block();
				ifStatement.TrueBlock.Add(base.CodeBuilder.CreateAssignment(expression6, base.CodeBuilder.CreateIntegerLiteral(-1)));
				block.Add(ifStatement);
				break;
			}
			default:
			{
				if (null != integerLiteralExpression3)
				{
					expression6 = expression3;
					break;
				}
				InternalLocal local3 = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
				expression6 = base.CodeBuilder.CreateReference(local3);
				block.Add(base.CodeBuilder.CreateAssignment(expression6, expression3));
				break;
			}
			}
			if (arguments.Count == 3)
			{
				Expression expression7 = null;
				bool flag = false;
				if (null == integerLiteralExpression3)
				{
					expression7 = ((integerLiteralExpression2 == null || null == integerLiteralExpression) ? base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.Or, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.And, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression6, base.CodeBuilder.CreateIntegerLiteral(0)), base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.GreaterThan, expression5, expression4)), base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.And, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.GreaterThan, expression6, base.CodeBuilder.CreateIntegerLiteral(0)), base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression5, expression4))) : ((integerLiteralExpression2.Value >= integerLiteralExpression.Value) ? base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression6, base.CodeBuilder.CreateIntegerLiteral(0)) : base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.GreaterThan, expression6, base.CodeBuilder.CreateIntegerLiteral(0))));
				}
				else if (integerLiteralExpression3.Value < 0)
				{
					if (integerLiteralExpression2 != null && null != integerLiteralExpression)
					{
						flag = integerLiteralExpression2.Value > integerLiteralExpression.Value;
					}
					else
					{
						expression7 = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.GreaterThan, expression5, expression4);
					}
				}
				else if (integerLiteralExpression2 != null && null != integerLiteralExpression)
				{
					flag = integerLiteralExpression2.Value < integerLiteralExpression.Value;
				}
				else
				{
					expression7 = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression5, expression4);
				}
				Statement stmt = base.CodeBuilder.RaiseException(block.LexicalInfo, base.TypeSystemServices.Map(System_ArgumentOutOfRangeException_ctor), base.CodeBuilder.CreateStringLiteral("step"));
				if (expression7 != null)
				{
					IfStatement ifStatement = new IfStatement(block.LexicalInfo);
					ifStatement.TrueBlock = new Block();
					ifStatement.Condition = expression7;
					ifStatement.TrueBlock.Add(stmt);
					block.Add(ifStatement);
				}
				else if (flag)
				{
					block.Add(stmt);
				}
				if (integerLiteralExpression3 != null && integerLiteralExpression2 != null && null != integerLiteralExpression)
				{
					int num = (int)integerLiteralExpression3.Value;
					int num2 = (int)integerLiteralExpression2.Value;
					int num3 = (int)integerLiteralExpression.Value;
					expression5 = base.CodeBuilder.CreateIntegerLiteral(num3 + num * (int)Math.Ceiling((double)(num2 - num3) / (double)num));
				}
				else
				{
					Expression lhs = expression5;
					if (null != integerLiteralExpression2)
					{
						InternalLocal local2 = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
						expression5 = base.CodeBuilder.CreateReference(local2);
					}
					block.Add(base.CodeBuilder.CreateAssignment(expression5, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.IntType, BinaryOperatorType.Addition, expression4, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.IntType, BinaryOperatorType.Multiply, expression6, base.CodeBuilder.CreateCast(base.TypeSystemServices.IntType, base.CodeBuilder.CreateMethodInvocation(base.TypeSystemServices.Map(System_Math_Ceiling), base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.DoubleType, BinaryOperatorType.Division, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.IntType, BinaryOperatorType.Subtraction, lhs, expression4), base.CodeBuilder.CreateCast(base.TypeSystemServices.DoubleType, expression6))))))));
				}
			}
			WhileStatement whileStatement = new WhileStatement(node.LexicalInfo);
			BinaryOperatorType op = BinaryOperatorType.Inequality;
			if (expression6.NodeType == NodeType.IntegerLiteralExpression)
			{
				op = ((((IntegerLiteralExpression)expression6).Value <= 0) ? BinaryOperatorType.GreaterThan : BinaryOperatorType.LessThan);
			}
			whileStatement.Condition = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, op, expression4, expression5);
			whileStatement.Condition.LexicalInfo = node.LexicalInfo;
			whileStatement.Block.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference((InternalLocal)declarations[0].Entity), expression4));
			Block block2 = new Block();
			block2["checked"] = false;
			block2.Add(base.CodeBuilder.CreateAssignment(expression4, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.IntType, BinaryOperatorType.Addition, expression4, expression6)));
			whileStatement.Block.Add((Statement)block2);
			whileStatement.Block.Add(node.Block);
			whileStatement.OrBlock = node.OrBlock;
			whileStatement.ThenBlock = node.ThenBlock;
			block.Add(whileStatement);
			ReplaceCurrentNode(block);
		}

		private void CheckForItemInArrayLoop(ForStatement node)
		{
			IArrayType arrayType = GetExpressionType(node.Iterator) as IArrayType;
			if (arrayType == null || arrayType.Rank > 1)
			{
				return;
			}
			IType elementType = arrayType.ElementType;
			if (!(elementType is InternalCallableType))
			{
				Block block = new Block(node.LexicalInfo);
				InternalLocal local = DeclareTempLocal(base.TypeSystemServices.IntType);
				Expression expression = base.CodeBuilder.CreateReference(local);
				block.Add(base.CodeBuilder.CreateAssignment(expression, base.CodeBuilder.CreateIntegerLiteral(0)));
				InternalLocal local2 = DeclareTempLocal(node.Iterator.ExpressionType);
				ReferenceExpression referenceExpression = base.CodeBuilder.CreateReference(local2);
				block.Add(base.CodeBuilder.CreateAssignment(referenceExpression, node.Iterator));
				InternalLocal local3 = base.CodeBuilder.DeclareTempLocal(_currentMethod, base.TypeSystemServices.IntType);
				ReferenceExpression referenceExpression2 = base.CodeBuilder.CreateReference(local3);
				block.Add(base.CodeBuilder.CreateAssignment(node.Iterator.LexicalInfo, referenceExpression2, base.CodeBuilder.CreateMethodInvocation(referenceExpression, Array_get_Length)));
				WhileStatement whileStatement = new WhileStatement(node.LexicalInfo);
				whileStatement.Condition = base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.BoolType, BinaryOperatorType.LessThan, expression, referenceExpression2);
				if (1 == node.Declarations.Count)
				{
					ILocalEntity localEntity = (ILocalEntity)node.Declarations[0].Entity;
					node.Block.ReplaceNodes(new EntityPredicate(localEntity).Matches, CreateRawArraySlicing(referenceExpression, expression, elementType, localEntity.Type));
				}
				else
				{
					UnpackExpression(whileStatement.Block, CreateRawArraySlicing(referenceExpression, expression, elementType), node.Declarations);
				}
				whileStatement.Block.Add(node.Block);
				FixContinueStatements(node, whileStatement);
				BinaryExpression binaryExpression = base.CodeBuilder.CreateAssignment(expression, base.CodeBuilder.CreateBoundBinaryExpression(base.TypeSystemServices.IntType, BinaryOperatorType.Addition, expression, base.CodeBuilder.CreateIntegerLiteral(1)));
				AstAnnotations.MarkUnchecked(binaryExpression);
				whileStatement.Block.Add(binaryExpression);
				whileStatement.OrBlock = node.OrBlock;
				whileStatement.ThenBlock = node.ThenBlock;
				block.Add(whileStatement);
				ReplaceCurrentNode(block);
			}
		}

		private Expression CreateRawArraySlicing(ReferenceExpression arrayRef, Expression indexReference, IType elementType, IType expectedType)
		{
			Expression expression = CreateRawArraySlicing(arrayRef, indexReference, elementType);
			if (elementType != expectedType)
			{
				return base.CodeBuilder.CreateCast(expectedType, expression);
			}
			return expression;
		}

		private void FixContinueStatements(ForStatement node, WhileStatement ws)
		{
			LabelStatement labelStatement = CreateUpdateLabel(node);
			GotoOnTopLevelContinue gotoOnTopLevelContinue = new GotoOnTopLevelContinue(labelStatement);
			node.Block.Accept(gotoOnTopLevelContinue);
			if (gotoOnTopLevelContinue.UsageCount > 0)
			{
				ws.Block.Add(labelStatement);
			}
		}

		private LabelStatement CreateUpdateLabel(ForStatement node)
		{
			return new LabelStatement(LexicalInfo.Empty, base.Context.GetUniqueName("label"));
		}

		private static SlicingExpression CreateRawArraySlicing(ReferenceExpression arrayRef, Expression numRef, IType elementType)
		{
			SlicingExpression slicingExpression = new SlicingExpression(arrayRef.CloneNode(), numRef.CloneNode());
			slicingExpression.ExpressionType = elementType;
			AstAnnotations.MarkRawArrayIndexing(slicingExpression);
			return slicingExpression;
		}

		private InternalLocal DeclareTempLocal(IType type)
		{
			return base.CodeBuilder.DeclareTempLocal(_currentMethod, type);
		}

		private void UnpackExpression(Block block, Expression expression, DeclarationCollection declarations)
		{
			NormalizeIterationStatements.UnpackExpression(base.CodeBuilder, _currentMethod, block, expression, declarations);
		}
	}
}
