using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class ProcessAssignmentsToSpecialMembers : AbstractTransformerCompilerStep
	{
		public class ChainItem
		{
			public Expression Container;

			public InternalLocal Local;

			public ChainItem(Expression container)
			{
				Container = container;
			}
		}

		private Method _currentMethod;

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit);
			}
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
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

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (IsAssignmentToSpecialMember(node))
			{
				ProcessAssignmentToSpecialMember(node);
			}
		}

		protected bool IsAssignmentToSpecialMember(BinaryExpression node)
		{
			if (BinaryOperatorType.Assign == node.Operator && NodeType.MemberReferenceExpression == node.Left.NodeType)
			{
				MemberReferenceExpression memberReferenceExpression = node.Left as MemberReferenceExpression;
				Expression target = memberReferenceExpression.Target;
				return !IsTerminalReferenceNode(target) && IsSpecialMemberTarget(target);
			}
			return false;
		}

		protected abstract bool IsSpecialMemberTarget(Expression container);

		private void ProcessAssignmentToSpecialMember(BinaryExpression node)
		{
			MemberReferenceExpression memberRef = (MemberReferenceExpression)node.Left;
			List list = WalkMemberChain(memberRef);
			if (list == null || 0 == list.Count)
			{
				return;
			}
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo);
			InternalLocal internalLocal = DeclareTempLocal(GetExpressionType(node.Right));
			methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(internalLocal), node.Right));
			foreach (ChainItem item2 in list)
			{
				item2.Local = DeclareTempLocal(item2.Container.ExpressionType);
				BinaryExpression item = base.CodeBuilder.CreateAssignment(node.LexicalInfo, base.CodeBuilder.CreateReference(item2.Local), item2.Container.CloneNode());
				item2.Container.ParentNode.Replace(item2.Container, base.CodeBuilder.CreateReference(item2.Local));
				methodInvocationExpression.Arguments.Add(item);
			}
			methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateAssignment(node.LexicalInfo, node.Left, base.CodeBuilder.CreateReference(internalLocal)));
			PropagateChanges(methodInvocationExpression, list);
			if (NodeType.ExpressionStatement != node.ParentNode.NodeType)
			{
				methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(internalLocal));
				BindExpressionType(methodInvocationExpression, internalLocal.Type);
			}
			ReplaceCurrentNode(methodInvocationExpression);
		}

		protected virtual void PropagateChanges(MethodInvocationExpression eval, List chain)
		{
			foreach (ChainItem item in chain.Reversed)
			{
				eval.Arguments.Add(base.CodeBuilder.CreateAssignment(item.Container.CloneNode(), base.CodeBuilder.CreateReference(item.Local)));
			}
		}

		protected virtual List WalkMemberChain(MemberReferenceExpression memberRef)
		{
			List list = new List();
			while (true)
			{
				bool flag = true;
				MemberReferenceExpression memberReferenceExpression = memberRef.Target as MemberReferenceExpression;
				if (memberReferenceExpression == null || (IsSpecialMemberTarget(memberReferenceExpression) && IsReadOnlyMember(memberReferenceExpression)))
				{
					base.Warnings.Add(CompilerWarningFactory.AssignmentToTemporary(memberRef));
					return null;
				}
				if (IsSpecialMemberTarget(memberReferenceExpression) && EntityType.Field != memberReferenceExpression.Entity.EntityType)
				{
					list.Insert(0, new ChainItem(memberReferenceExpression));
				}
				if (IsTerminalReferenceNode(memberReferenceExpression.Target))
				{
					break;
				}
				memberRef = memberReferenceExpression;
			}
			return list;
		}

		protected virtual bool IsTerminalReferenceNode(Expression target)
		{
			NodeType nodeType = target.NodeType;
			return NodeType.ReferenceExpression == nodeType || NodeType.SelfLiteralExpression == nodeType || NodeType.SuperLiteralExpression == nodeType || ProcessMethodBodies.IsArraySlicing(target) || !IsSpecialMemberTarget(target);
		}

		protected virtual bool IsReadOnlyMember(MemberReferenceExpression container)
		{
			return container.Entity.EntityType switch
			{
				EntityType.Property => ((IProperty)container.Entity).GetSetMethod() == null, 
				EntityType.Field => TypeSystemServices.IsReadOnlyField((IField)container.Entity), 
				_ => true, 
			};
		}

		private InternalLocal DeclareTempLocal(IType localType)
		{
			return base.CodeBuilder.DeclareTempLocal(_currentMethod, localType);
		}
	}
}
