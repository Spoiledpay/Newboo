using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[Flags]
	public enum GenericParameterConstraints
	{
		None = 0x0,
		ValueType = 0x1,
		ReferenceType = 0x2,
		Constructable = 0x4,
		Covariant = 0x8,
		Contravariant = 0x10
	}
}
