using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[Flags]
	public enum ExceptionHandlerFlags
	{
		None = 0x0,
		Anonymous = 0x1,
		Untyped = 0x2,
		Filter = 0x4
	}
}
