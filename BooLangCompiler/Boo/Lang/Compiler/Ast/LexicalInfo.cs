using System;
using System.IO;

namespace Boo.Lang.Compiler.Ast
{
	public class LexicalInfo : SourceLocation, IEquatable<LexicalInfo>, IComparable<LexicalInfo>
	{
		public static readonly LexicalInfo Empty = new LexicalInfo(null, -1, -1);

		private readonly string _filename;

		private string _fullPath;

		public override bool IsValid => _filename != null && base.IsValid;

		public string FileName => _filename;

		public string FullPath
		{
			get
			{
				if (null != _fullPath)
				{
					return _fullPath;
				}
				_fullPath = SafeGetFullPath(_filename);
				return _fullPath;
			}
		}

		public LexicalInfo(string filename, int line, int column)
			: base(line, column)
		{
			_filename = filename;
		}

		public LexicalInfo(string filename)
			: this(filename, -1, -1)
		{
		}

		public LexicalInfo(LexicalInfo other)
			: this(other.FileName, other.Line, other.Column)
		{
		}

		public override string ToString()
		{
			return _filename + base.ToString();
		}

		private static string SafeGetFullPath(string fname)
		{
			try
			{
				return Path.GetFullPath(fname);
			}
			catch (Exception)
			{
			}
			return fname;
		}

		public int CompareTo(LexicalInfo other)
		{
			int num = CompareTo((SourceLocation)other);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(_filename, other._filename);
		}

		public bool Equals(LexicalInfo other)
		{
			return CompareTo(other) == 0;
		}
	}
}
