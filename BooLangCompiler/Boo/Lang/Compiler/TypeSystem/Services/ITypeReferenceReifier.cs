using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public interface ITypeReferenceReifier
	{
		void Reify(TypeReference node);
	}
}
