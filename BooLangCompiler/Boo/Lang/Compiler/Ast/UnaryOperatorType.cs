using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public enum UnaryOperatorType
	{
		None,
		UnaryNegation,
		Increment,
		Decrement,
		PostIncrement,
		PostDecrement,
		LogicalNot,
		Explode,
		OnesComplement,
		AddressOf,
		Indirection
	}
}
