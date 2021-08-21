using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	internal class MethodBodyState
	{
		private int _loopDepth;

		private int _protectedBlockDepth;

		private int _exceptionHandlerDepth;

		private Stack<TryStatement> _tryBlocks = new Stack<TryStatement>();

		private List _labelReferences = new List();

		private Hashtable _labels = new Hashtable();

		public List LabelReferences => _labelReferences;

		public int TryBlockDepth => _tryBlocks.Count;

		public IEnumerable<TryStatement> TryBlocks => _tryBlocks;

		public int ProtectedBlockDepth => _protectedBlockDepth;

		public bool InExceptionHandler => _exceptionHandlerDepth > 0;

		public bool InLoop => _loopDepth > 0;

		public void Reset()
		{
			_loopDepth = 0;
			_exceptionHandlerDepth = 0;
			_tryBlocks.Clear();
			_labelReferences.Clear();
			_labels.Clear();
		}

		public void AddLabelReference(ReferenceExpression node)
		{
			_labelReferences.Add(node);
		}

		public void EnterTryBlock(TryStatement tryBlock)
		{
			_tryBlocks.Push(tryBlock);
		}

		public void LeaveTryBlock()
		{
			_tryBlocks.Pop();
		}

		public void EnterProtectedBlock()
		{
			_protectedBlockDepth++;
		}

		public void LeaveProtectedBlock()
		{
			_protectedBlockDepth--;
		}

		public void EnterExceptionHandler()
		{
			_exceptionHandlerDepth++;
		}

		public void LeaveExceptionHandler()
		{
			_exceptionHandlerDepth--;
		}

		public void EnterLoop()
		{
			_loopDepth++;
		}

		public void LeaveLoop()
		{
			_loopDepth--;
		}

		public InternalLabel ResolveLabel(string name)
		{
			return (InternalLabel)_labels[name];
		}

		public void AddLabel(InternalLabel label)
		{
			_labels.Add(label.Name, label);
		}
	}
}
