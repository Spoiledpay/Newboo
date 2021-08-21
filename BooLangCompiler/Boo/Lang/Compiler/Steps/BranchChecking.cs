using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public class BranchChecking : AbstractFastVisitorCompilerStep
	{
		private InternalMethod _currentMethod;

		private MethodBodyState _state = new MethodBodyState();

		public override void OnTryStatement(TryStatement node)
		{
			_state.EnterTryBlock(node);
			_state.EnterProtectedBlock();
			Visit(node.ProtectedBlock);
			_state.LeaveProtectedBlock();
			Visit(node.ExceptionHandlers);
			CheckExceptionHandlers(node.ExceptionHandlers);
			Visit(node.FailureBlock);
			Visit(node.EnsureBlock);
			_state.LeaveTryBlock();
		}

		public override void OnExceptionHandler(ExceptionHandler node)
		{
			_state.EnterExceptionHandler();
			Visit(node.Block);
			_state.LeaveExceptionHandler();
		}

		public override void OnRaiseStatement(RaiseStatement node)
		{
			base.OnRaiseStatement(node);
			if (node.Exception == null && !_state.InExceptionHandler)
			{
				Error(CompilerErrorFactory.ReRaiseOutsideExceptionHandler(node));
			}
		}

		public override void OnConstructor(Constructor node)
		{
			OnMethod(node);
		}

		public override void OnDestructor(Destructor node)
		{
			OnMethod(node);
		}

		public override void OnMethod(Method node)
		{
			_currentMethod = (InternalMethod)node.Entity;
			_state.Reset();
			Visit(node.Body);
			ResolveLabelReferences();
		}

		private void ResolveLabelReferences()
		{
			foreach (ReferenceExpression labelReference in _state.LabelReferences)
			{
				InternalLabel internalLabel = _state.ResolveLabel(labelReference.Name);
				if (null == internalLabel)
				{
					Error(labelReference, CompilerErrorFactory.NoSuchLabel(labelReference, labelReference.Name));
				}
				else
				{
					labelReference.Entity = internalLabel;
				}
			}
		}

		public override void OnWhileStatement(WhileStatement node)
		{
			VisitLoop(node.Block);
			Visit(node.OrBlock);
			Visit(node.ThenBlock);
		}

		private void VisitLoop(Block block)
		{
			_state.EnterLoop();
			Visit(block);
			_state.LeaveLoop();
		}

		public override void OnForStatement(ForStatement node)
		{
			VisitLoop(node.Block);
			Visit(node.OrBlock);
			Visit(node.ThenBlock);
		}

		public override void OnLabelStatement(LabelStatement node)
		{
			AstAnnotations.SetTryBlockDepth(node, _state.TryBlockDepth);
			if (_state.ResolveLabel(node.Name) != null)
			{
				Error(CompilerErrorFactory.LabelAlreadyDefined(node, _currentMethod, node.Name));
			}
			else
			{
				_state.AddLabel(new InternalLabel(node));
			}
		}

		public override void OnYieldStatement(YieldStatement node)
		{
			if (_state.TryBlockDepth == _state.ProtectedBlockDepth)
			{
				foreach (TryStatement tryBlock in _state.TryBlocks)
				{
					if (tryBlock.FailureBlock != null || tryBlock.ExceptionHandlers.Count > 0)
					{
						Error(CompilerErrorFactory.YieldInsideTryExceptOrEnsureBlock(node));
						break;
					}
				}
			}
			else
			{
				Error(CompilerErrorFactory.YieldInsideTryExceptOrEnsureBlock(node));
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (BuiltinFunction.Switch == node.Target.Entity)
			{
				for (int i = 1; i < node.Arguments.Count; i++)
				{
					ReferenceExpression node2 = (ReferenceExpression)node.Arguments[i];
					_state.AddLabelReference(node2);
				}
			}
		}

		public override void OnGotoStatement(GotoStatement node)
		{
			AstAnnotations.SetTryBlockDepth(node, _state.TryBlockDepth);
			_state.AddLabelReference(node.Label);
		}

		public override void OnBreakStatement(BreakStatement node)
		{
			CheckInLoop(node);
		}

		public override void OnContinueStatement(ContinueStatement node)
		{
			CheckInLoop(node);
		}

		public override void OnBlockExpression(BlockExpression node)
		{
		}

		private void CheckInLoop(Statement node)
		{
			if (!_state.InLoop)
			{
				Error(CompilerErrorFactory.NoEnclosingLoop(node));
			}
		}

		private void CheckExceptionHandlers(ExceptionHandlerCollection handlers)
		{
			for (int i = 1; i < handlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = handlers[i];
				for (int num = i - 1; num >= 0; num--)
				{
					ExceptionHandler exceptionHandler2 = handlers[num];
					IType type = exceptionHandler.Declaration.Type.Entity as IType;
					IType type2 = exceptionHandler2.Declaration.Type.Entity as IType;
					if (type != null && null != type2 && ((type == type2 && exceptionHandler2.FilterCondition == null) || type.IsSubclassOf(type2)))
					{
						Error(CompilerErrorFactory.ExceptionAlreadyHandled(exceptionHandler, exceptionHandler2));
						break;
					}
				}
			}
		}
	}
}
