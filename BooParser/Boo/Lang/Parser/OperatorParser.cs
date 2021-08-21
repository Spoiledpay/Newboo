using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Parser
{
	public class OperatorParser
	{
		public static BinaryOperatorType ParseComparison(string op)
		{
			return op switch
			{
				"<=" => BinaryOperatorType.LessThanOrEqual, 
				">=" => BinaryOperatorType.GreaterThanOrEqual, 
				"==" => BinaryOperatorType.Equality, 
				"!=" => BinaryOperatorType.Inequality, 
				"=~" => BinaryOperatorType.Match, 
				"!~" => BinaryOperatorType.NotMatch, 
				_ => throw new ArgumentException(op, "op"), 
			};
		}

		public static BinaryOperatorType ParseAssignment(string op)
		{
			return op switch
			{
				"=" => BinaryOperatorType.Assign, 
				"+=" => BinaryOperatorType.InPlaceAddition, 
				"-=" => BinaryOperatorType.InPlaceSubtraction, 
				"/=" => BinaryOperatorType.InPlaceDivision, 
				"*=" => BinaryOperatorType.InPlaceMultiply, 
				"%=" => BinaryOperatorType.InPlaceModulus, 
				_ => throw new ArgumentException(op, "op"), 
			};
		}
	}
}
