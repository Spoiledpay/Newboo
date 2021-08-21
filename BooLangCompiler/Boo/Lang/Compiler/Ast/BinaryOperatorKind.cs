using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public enum BinaryOperatorKind
	{
		Arithmetic = 0xF,
		Comparison = 4080,
		TypeComparison = 3840,
		Assignment = 1044480,
		InPlaceAssignment = 983040,
		Logical = 15728640,
		Bitwise = 251658240
	}
}
