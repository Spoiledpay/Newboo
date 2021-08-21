using System;
using System.Text;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	[Serializable]
	public class CompilerError : ApplicationException
	{
		private readonly LexicalInfo _lexicalInfo;

		private readonly string _code;

		[ThreadStatic]
		private static bool _verbose;

		public string Code => _code;

		public LexicalInfo LexicalInfo => _lexicalInfo;

		public CompilerError(string code, LexicalInfo lexicalInfo, Exception cause, params object[] args)
			: base(ResourceManager.Format(code, args), cause)
		{
			if (null == lexicalInfo)
			{
				throw new ArgumentNullException("lexicalInfo");
			}
			_code = code;
			_lexicalInfo = lexicalInfo;
		}

		public CompilerError(string code, Exception cause, params object[] args)
			: this(code, LexicalInfo.Empty, cause, args)
		{
		}

		public CompilerError(string code, LexicalInfo lexicalInfo, params object[] args)
			: base(ResourceManager.Format(code, args))
		{
			if (null == lexicalInfo)
			{
				throw new ArgumentNullException("lexicalInfo");
			}
			_code = code;
			_lexicalInfo = lexicalInfo;
		}

		public CompilerError(string code, LexicalInfo lexicalInfo, string message, Exception cause)
			: base(message, cause)
		{
			if (null == lexicalInfo)
			{
				throw new ArgumentNullException("lexicalInfo");
			}
			_code = code;
			_lexicalInfo = lexicalInfo;
		}

		public CompilerError(LexicalInfo lexicalInfo, string message, Exception cause)
			: this("BCE0000", lexicalInfo, message, cause)
		{
		}

		public CompilerError(Node node, string message, Exception cause)
			: this(node.LexicalInfo, message, cause)
		{
		}

		public CompilerError(Node node, string message)
			: this(node, message, null)
		{
		}

		public CompilerError(LexicalInfo data, string message)
			: this(data, message, null)
		{
		}

		public CompilerError(LexicalInfo data, Exception cause)
			: this(data, cause.Message, cause)
		{
		}

		public CompilerError(string message)
			: this(LexicalInfo.Empty, message, null)
		{
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (_lexicalInfo.Line > 0)
			{
				stringBuilder.Append(_lexicalInfo);
				stringBuilder.Append(": ");
			}
			stringBuilder.Append(_code);
			stringBuilder.Append(": ");
			stringBuilder.Append(_verbose ? base.ToString() : Message);
			return stringBuilder.ToString();
		}

		public string ToString(bool verbose)
		{
			bool verbose2 = _verbose;
			try
			{
				_verbose = _verbose || verbose;
				return ToString();
			}
			finally
			{
				_verbose = verbose2;
			}
		}
	}
}
