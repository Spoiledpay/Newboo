using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class CompilerErrorEmitter
	{
		private static CompilerErrorCollection CompilerErrors => My<CompilerErrorCollection>.Instance;

		public void GenericArgumentsCountMismatch(Node anchor, IType type)
		{
			CompilerErrors.Add(CompilerErrorFactory.GenericDefinitionArgumentCount(anchor, type, type.GenericInfo.GenericParameters.Length));
		}
	}
}
