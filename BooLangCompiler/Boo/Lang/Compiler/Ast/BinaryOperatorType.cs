using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public enum BinaryOperatorType
	{
		None,
		Addition,
		Subtraction,
		Multiply,
		Division,
		Modulus,
		Exponentiation,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
		Equality,
		Inequality,
		Match,
		NotMatch,
		Assign,
		InPlaceAddition,
		InPlaceSubtraction,
		InPlaceMultiply,
		InPlaceDivision,
		InPlaceModulus,
		InPlaceBitwiseAnd,
		InPlaceBitwiseOr,
		ReferenceEquality,
		ReferenceInequality,
		TypeTest,
		Member,
		NotMember,
		Or,
		And,
		BitwiseOr,
		BitwiseAnd,
		ExclusiveOr,
		InPlaceExclusiveOr,
		ShiftLeft,
		InPlaceShiftLeft,
		ShiftRight,
		InPlaceShiftRight
	}
}
