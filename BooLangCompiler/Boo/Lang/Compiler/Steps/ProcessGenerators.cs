using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps.Generators;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessGenerators : AbstractTransformerCompilerStep
	{
		public static readonly ConstructorInfo List_IEnumerableConstructor = Methods.ConstructorOf(() => new List(null));

		private Method _current;

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				Visit(base.CompileUnit.Modules);
			}
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
		}

		public override void OnField(Field node)
		{
		}

		public override void OnConstructor(Constructor method)
		{
			_current = method;
			Visit(_current.Body);
		}

		public override bool EnterMethod(Method method)
		{
			_current = method;
			return true;
		}

		public override void LeaveMethod(Method method)
		{
			InternalMethod internalMethod = (InternalMethod)method.Entity;
			if (internalMethod.IsGenerator)
			{
				GeneratorMethodProcessor generatorMethodProcessor = new GeneratorMethodProcessor(base.Context, internalMethod);
				generatorMethodProcessor.Run();
			}
		}

		public override void OnListLiteralExpression(ListLiteralExpression node)
		{
			bool flag = AstUtil.IsListGenerator(node);
			Visit(node.Items);
			if (flag)
			{
				ReplaceCurrentNode(base.CodeBuilder.CreateConstructorInvocation(base.TypeSystemServices.Map(List_IEnumerableConstructor), node.Items[0]));
			}
		}

		public override void LeaveGeneratorExpression(GeneratorExpression node)
		{
			ForeignReferenceCollector foreignReferenceCollector = new ForeignReferenceCollector();
			foreignReferenceCollector.CurrentType = TypeContaining(node);
			ForeignReferenceCollector foreignReferenceCollector2 = foreignReferenceCollector;
			node.Accept(foreignReferenceCollector2);
			GeneratorExpressionProcessor generatorExpressionProcessor = new GeneratorExpressionProcessor(base.Context, foreignReferenceCollector2, node);
			generatorExpressionProcessor.Run();
			ReplaceCurrentNode(generatorExpressionProcessor.CreateEnumerableConstructorInvocation());
		}

		private IType TypeContaining(GeneratorExpression node)
		{
			return (IType)AstUtil.GetParentClass(node).Entity;
		}
	}
}
