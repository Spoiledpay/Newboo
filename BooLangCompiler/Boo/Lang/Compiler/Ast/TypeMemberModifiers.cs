using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[Flags]
	public enum TypeMemberModifiers
	{
		None = 0x0,
		Private = 0x1,
		Internal = 0x2,
		Protected = 0x4,
		Public = 0x8,
		Transient = 0x10,
		Static = 0x20,
		Final = 0x40,
		Virtual = 0x80,
		Override = 0x100,
		Abstract = 0x200,
		Partial = 0x400,
		New = 0x800,
		VisibilityMask = 0xF
	}
}
