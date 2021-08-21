using System.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IExternalEntity : IEntity
	{
		MemberInfo MemberInfo { get; }
	}
}
