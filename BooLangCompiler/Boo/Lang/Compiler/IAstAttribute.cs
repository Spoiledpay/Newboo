using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	public interface IAstAttribute : ICompilerComponent
	{
		Attribute Attribute { set; }

		void Apply(Node targetNode);
	}
}
