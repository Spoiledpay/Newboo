using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class NormalizeExpressions : AbstractTransformerCompilerStep
	{
		private Method _current;

		public override void Run()
		{
			Visit(base.CompileUnit);
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

		public override void OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(node.LexicalInfo, base.Context.GetUniqueName("collection"));
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateEvalInvocation(node.LexicalInfo);
			methodInvocationExpression.Arguments.Add(new BinaryExpression(BinaryOperatorType.Assign, referenceExpression, node.Collection));
			if (node.Initializer is ListLiteralExpression)
			{
				foreach (Expression item in ((ListLiteralExpression)node.Initializer).Items)
				{
					methodInvocationExpression.Arguments.Add(NewAddInvocation(item.LexicalInfo, referenceExpression, item));
				}
			}
			else
			{
				foreach (ExpressionPair item2 in ((HashLiteralExpression)node.Initializer).Items)
				{
					methodInvocationExpression.Arguments.Add(NewAddInvocation(item2.LexicalInfo, referenceExpression, item2.First, item2.Second));
				}
			}
			methodInvocationExpression.Arguments.Add(referenceExpression.CloneNode());
			ReplaceCurrentNode(methodInvocationExpression);
		}

		private static MethodInvocationExpression NewAddInvocation(LexicalInfo location, ReferenceExpression target, params Expression[] args)
		{
			return new MethodInvocationExpression(location, new MemberReferenceExpression(target.CloneNode(), "Add"), args);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			if (node.Target.NodeType == NodeType.OmittedExpression)
			{
				if (_current.IsStatic)
				{
					node.Target = new ReferenceExpression(node.Target.LexicalInfo, _current.DeclaringType.Name);
				}
				else
				{
					node.Target = new SelfLiteralExpression(node.Target.LexicalInfo);
				}
			}
		}
	}
}
