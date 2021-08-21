using System;

namespace Boo.Lang.Compiler.TypeSystem
{
	[Flags]
	public enum EntityType
	{
		CompileUnit = 0x0,
		Type = 0x2,
		Method = 0x8,
		Constructor = 0x10,
		Destructor = 0x20,
		Field = 0x40,
		Property = 0x80,
		Event = 0x100,
		Local = 0x200,
		Parameter = 0x400,
		Assembly = 0x800,
		Namespace = 0x1000,
		Ambiguous = 0x2000,
		Array = 0x4000,
		BuiltinFunction = 0x8000,
		Label = 0x10000,
		GenericParameter = 0x20000,
		Custom = 0x20001,
		Unknown = 0x20002,
		Null = 0x20003,
		Error = 0x20004,
		Any = 0xFFFFFF
	}
}
