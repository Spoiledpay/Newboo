using System;
using System.Globalization;
using antlr;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Parser
{
	public class PrimitiveParser
	{
		public static TimeSpan ParseTimeSpan(IToken token, string text)
		{
			try
			{
				return TryParseTimeSpan(token, text);
			}
			catch (OverflowException x)
			{
				LexicalInfo sourceLocation = ToLexicalInfo(token);
				GenericParserError(sourceLocation, x);
				return TimeSpan.Zero;
			}
		}

		private static TimeSpan TryParseTimeSpan(IToken token, string text)
		{
			if (text.EndsWith("ms"))
			{
				return TimeSpan.FromMilliseconds(ParseDouble(token, text.Substring(0, text.Length - 2)));
			}
			char c = text[text.Length - 1];
			double value = ParseDouble(token, text.Substring(0, text.Length - 1));
			return c switch
			{
				's' => TimeSpan.FromSeconds(value), 
				'h' => TimeSpan.FromHours(value), 
				'm' => TimeSpan.FromMinutes(value), 
				'd' => TimeSpan.FromDays(value), 
				_ => throw new ArgumentException(text, "text"), 
			};
		}

		public static double ParseDouble(IToken token, string s)
		{
			return ParseDouble(token, s, isSingle: false);
		}

		public static double ParseDouble(IToken token, string s, bool isSingle)
		{
			try
			{
				return TryParseDouble(isSingle, s);
			}
			catch (Exception x)
			{
				LexicalInfo sourceLocation = ToLexicalInfo(token);
				GenericParserError(sourceLocation, x);
				return double.NaN;
			}
		}

		private static double TryParseDouble(bool isSingle, string s)
		{
			if (isSingle)
			{
				return float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
			}
			return double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
		}

		public static IntegerLiteralExpression ParseIntegerLiteralExpression(IToken token, string text, bool asLong)
		{
			try
			{
				return TryParseIntegerLiteralExpression(token, text, asLong);
			}
			catch (OverflowException x)
			{
				LexicalInfo lexicalInfo = ToLexicalInfo(token);
				GenericParserError(lexicalInfo, x);
				return new IntegerLiteralExpression(lexicalInfo);
			}
		}

		private static void GenericParserError(LexicalInfo sourceLocation, Exception x)
		{
			My<CompilerErrorCollection>.Instance.Add(CompilerErrorFactory.GenericParserError(sourceLocation, x));
		}

		private static IntegerLiteralExpression TryParseIntegerLiteralExpression(IToken token, string text, bool asLong)
		{
			NumberStyles style = NumberStyles.Integer | NumberStyles.AllowExponent;
			int num = text.IndexOf("0x");
			bool flag = false;
			if (num >= 0)
			{
				if (text.StartsWith("-"))
				{
					flag = true;
				}
				text = text.Substring(num + "0x".Length);
				style = NumberStyles.HexNumber;
			}
			long num2 = long.Parse(RemoveLongSuffix(text), style, CultureInfo.InvariantCulture);
			if (flag)
			{
				num2 *= -1;
			}
			return new IntegerLiteralExpression(ToLexicalInfo(token), num2, asLong || num2 > int.MaxValue || num2 < int.MinValue);
		}

		private static string RemoveLongSuffix(string s)
		{
			if (s.EndsWith("l") || s.EndsWith("L"))
			{
				return s.Substring(0, s.Length - 1);
			}
			return s;
		}

		private static LexicalInfo ToLexicalInfo(IToken token)
		{
			return SourceLocationFactory.ToLexicalInfo(token);
		}

		public static int ParseInt(IToken token)
		{
			return (int)ParseIntegerLiteralExpression(token, token.getText(), asLong: false).Value;
		}
	}
}
