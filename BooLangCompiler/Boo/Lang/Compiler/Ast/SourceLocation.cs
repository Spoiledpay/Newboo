using System;

namespace Boo.Lang.Compiler.Ast
{
	public class SourceLocation : IComparable<SourceLocation>, IEquatable<SourceLocation>
	{
		private readonly int _line;

		private readonly int _column;

		public int Line => _line;

		public int Column => _column;

		public virtual bool IsValid => _line > 0 && _column > 0;

		public SourceLocation(int line, int column)
		{
			_line = line;
			_column = column;
		}

		public override string ToString()
		{
			return $"({_line},{_column})";
		}

		public int CompareTo(SourceLocation other)
		{
			int num = _line.CompareTo(other._line);
			if (num != 0)
			{
				return num;
			}
			return _column.CompareTo(other._column);
		}

		public bool Equals(SourceLocation other)
		{
			return CompareTo(other) == 0;
		}
	}
}
