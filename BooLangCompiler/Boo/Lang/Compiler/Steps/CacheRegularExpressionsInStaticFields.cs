using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class CacheRegularExpressionsInStaticFields : AbstractTransformerCompilerStep
	{
		private ClassDefinition _currentType;

		public override bool EnterClassDefinition(ClassDefinition node)
		{
			_currentType = node;
			return base.EnterClassDefinition(node);
		}

		public override void OnRELiteralExpression(RELiteralExpression node)
		{
			if (!AstUtil.IsRhsOfAssignment(node))
			{
				Field field = CreateRegexFieldFor(node);
				AddFieldInitializerToStaticConstructor(field, node);
				ReplaceCurrentNode(base.CodeBuilder.CreateReference(field));
			}
		}

		private Field CreateRegexFieldFor(RELiteralExpression node)
		{
			Field field = base.CodeBuilder.CreateField(base.Context.GetUniqueName("re"), base.TypeSystemServices.RegexType);
			field.Modifiers = TypeMemberModifiers.Internal | TypeMemberModifiers.Static;
			field.LexicalInfo = node.LexicalInfo;
			_currentType.Members.Add(field);
			return field;
		}

		private void AddFieldInitializerToStaticConstructor(Field node, Expression initializer)
		{
			Constructor orCreateStaticConstructorFor = base.CodeBuilder.GetOrCreateStaticConstructorFor(_currentType);
			orCreateStaticConstructorFor.Body.Statements.Insert(0, base.CodeBuilder.CreateFieldAssignment(node, initializer));
		}
	}
}
