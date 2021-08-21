using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Boo.Lang.Compiler.TypeSystem
{
	[CompilerGlobalScope]
	public static class TypeSystemExtensions
	{
		public static IEnumerable<IConstructor> GetConstructors(this IType self)
		{
			return from ctor in self.GetMembers().OfType<IConstructor>()
				where !ctor.IsStatic
				select ctor;
		}
	}
}
