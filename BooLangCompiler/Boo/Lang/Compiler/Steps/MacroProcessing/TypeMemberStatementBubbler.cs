using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	internal sealed class TypeMemberStatementBubbler : DepthFirstTransformer
	{
		private bool _bubbled;

		public static bool BubbleTypeMemberStatementsUp(Node node)
		{
			TypeMemberStatementBubbler typeMemberStatementBubbler = new TypeMemberStatementBubbler();
			typeMemberStatementBubbler.VisitNode(node);
			return typeMemberStatementBubbler._bubbled;
		}

		public override void LeaveTypeMemberStatement(TypeMemberStatement node)
		{
			if (node.TypeMember != null)
			{
				_bubbled = true;
				TypeDefinition ancestor = node.GetAncestor<TypeDefinition>();
				TypeMember insertionPoint = node.InsertionPoint;
				if (insertionPoint != null)
				{
					ancestor.Members.Insert(ancestor.Members.IndexOf(insertionPoint), node.TypeMember);
				}
				else
				{
					ancestor.Members.Add(node.TypeMember);
				}
				node.TypeMember = null;
			}
			RemoveCurrentNode();
		}

		public override void LeaveStatementTypeMember(StatementTypeMember node)
		{
			if (node.Statement == null)
			{
				RemoveCurrentNode();
			}
		}
	}
}
