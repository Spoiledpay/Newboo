using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public interface INodeReifier<T> where T : Node
	{
		T Reify(T node);
	}
}
