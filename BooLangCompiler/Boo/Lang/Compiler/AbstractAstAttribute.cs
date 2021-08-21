using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public abstract class AbstractAstAttribute : AbstractCompilerComponent, IAstAttribute, ICompilerComponent
	{
		public Attribute Attribute { get; set; }

		public LexicalInfo LexicalInfo => Attribute.LexicalInfo;

		protected void InvalidNodeForAttribute(string expectedNodeTypes)
		{
			base.Errors.Add(CompilerErrorFactory.InvalidNodeForAttribute(Attribute, GetType().FullName, expectedNodeTypes));
		}

		public abstract void Apply(Node targetNode);
	}
}
