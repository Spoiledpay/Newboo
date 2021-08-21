using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IInternalEntity : IEntity
	{
		Node Node { get; }
	}
}
