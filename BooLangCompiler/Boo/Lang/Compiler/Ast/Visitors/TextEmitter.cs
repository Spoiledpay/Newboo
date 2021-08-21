using System;
using System.Collections.Generic;
using System.IO;

namespace Boo.Lang.Compiler.Ast.Visitors
{
	public class TextEmitter : FastDepthFirstVisitor
	{
		protected TextWriter _writer;

		protected int _indent = 0;

		protected string _indentText = "\t";

		protected bool _needsIndenting = true;

		protected int _disableNewLine = 0;

		public string IndentText
		{
			get
			{
				return _indentText;
			}
			set
			{
				_indentText = value;
			}
		}

		public TextWriter Writer => _writer;

		public TextEmitter(TextWriter writer)
		{
			if (null == writer)
			{
				throw new ArgumentNullException("writer");
			}
			_writer = writer;
		}

		public void Indent()
		{
			_indent++;
		}

		public void Dedent()
		{
			_indent--;
		}

		public virtual void WriteIndented()
		{
			if (_needsIndenting)
			{
				for (int i = 0; i < _indent; i++)
				{
					_writer.Write(_indentText);
				}
				_needsIndenting = false;
			}
		}

		public virtual void WriteLine()
		{
			if (0 == _disableNewLine)
			{
				_writer.WriteLine();
				_needsIndenting = true;
			}
		}

		protected void DisableNewLine()
		{
			_disableNewLine++;
		}

		protected void EnableNewLine()
		{
			if (0 == _disableNewLine)
			{
				throw new InvalidOperationException();
			}
			_disableNewLine--;
		}

		public virtual void Write(string s)
		{
			_writer.Write(s);
		}

		public void WriteIndented(string format, params object[] args)
		{
			WriteIndented();
			Write(format, args);
		}

		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void WriteLine(string s)
		{
			WriteIndented(s);
			WriteLine();
		}

		public void WriteLine(string format, params object[] args)
		{
			WriteIndented(format, args);
			WriteLine();
		}

		protected void WriteCommaSeparatedList<T>(IEnumerable<T> items) where T : Node
		{
			int num = 0;
			foreach (T item in items)
			{
				if (num++ > 0)
				{
					Write(", ");
				}
				Visit(item);
			}
		}

		protected void WriteArray<T>(NodeCollection<T> items) where T : Node
		{
			WriteArray(items, null);
		}

		protected void WriteArray<T>(NodeCollection<T> items, ArrayTypeReference type) where T : Node
		{
			Write("(");
			if (null != type)
			{
				Write("of ");
				type.ElementType.Accept(this);
				Write(": ");
			}
			if (items.Count > 1)
			{
				for (int i = 0; i < items.Count; i++)
				{
					if (i > 0)
					{
						Write(", ");
					}
					Visit(items[i]);
				}
			}
			else
			{
				if (items.Count > 0)
				{
					Visit(items[0]);
				}
				if (items.Count == 0 || null == type)
				{
					Write(",");
				}
			}
			Write(")");
		}
	}
}
