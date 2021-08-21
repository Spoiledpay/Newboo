using System.Reflection;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	public interface IAssemblyReference : ICompileUnit, IEntity
	{
		Assembly Assembly { get; }
	}
}
