using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessClosures : AbstractTransformerCompilerStep
	{
		public override void Run()
		{
			Visit(base.CompileUnit);
		}

		public override void LeaveBlockExpression(BlockExpression node)
		{
			InternalMethod internalMethod = GetEntity(node) as InternalMethod;
			if (internalMethod != null)
			{
				ForeignReferenceCollector foreignReferenceCollector = new ForeignReferenceCollector();
				foreignReferenceCollector.CurrentMethod = internalMethod.Method;
				foreignReferenceCollector.CurrentType = internalMethod.DeclaringType;
				internalMethod.Method.Body.Accept(foreignReferenceCollector);
				if (foreignReferenceCollector.ContainsForeignLocalReferences)
				{
					BooClassBuilder booClassBuilder = CreateClosureClass(foreignReferenceCollector, internalMethod);
					booClassBuilder.ClassDefinition.LexicalInfo = node.LexicalInfo;
					foreignReferenceCollector.AdjustReferences();
					ReplaceCurrentNode(base.CodeBuilder.CreateMemberReference(foreignReferenceCollector.CreateConstructorInvocationWithReferencedEntities(booClassBuilder.Entity), internalMethod));
				}
				else
				{
					Expression expression = base.CodeBuilder.CreateMemberReference(internalMethod);
					expression.LexicalInfo = node.LexicalInfo;
					base.TypeSystemServices.GetConcreteExpressionType(expression);
					ReplaceCurrentNode(expression);
				}
			}
		}

		private BooClassBuilder CreateClosureClass(ForeignReferenceCollector collector, InternalMethod closure)
		{
			Method method = closure.Method;
			TypeDefinition declaringType = method.DeclaringType;
			declaringType.Members.Remove(method);
			BooClassBuilder booClassBuilder = collector.CreateSkeletonClass(closure.Name, method.LexicalInfo);
			declaringType.Members.Add(booClassBuilder.ClassDefinition);
			booClassBuilder.ClassDefinition.Members.Add(method);
			method.Name = "Invoke";
			if (method.IsStatic)
			{
				foreach (ParameterDeclaration parameter in method.Parameters)
				{
					((InternalParameter)parameter.Entity).Index++;
				}
			}
			method.Modifiers = TypeMemberModifiers.Public;
			return booClassBuilder;
		}
	}
}
