using System;
using System.Text;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	[Serializable]
	public class CompilerWarning
	{
		private readonly string _code;

		private readonly string _message;

		private readonly LexicalInfo _lexicalInfo;

		public string Message => _message;

		public LexicalInfo LexicalInfo => _lexicalInfo;

		public string Code => _code;

		public CompilerWarning(LexicalInfo lexicalInfo, string message)
			: this("BCW0000", lexicalInfo, message)
		{
		}

		public CompilerWarning(LexicalInfo lexicalInfo, string message, string code)
		{
			if (null == message)
			{
				throw new ArgumentNullException("message");
			}
			if (null == code)
			{
				throw new ArgumentNullException("code");
			}
			_lexicalInfo = lexicalInfo;
			_message = message;
			_code = code;
		}

		public CompilerWarning(string message)
			: this(LexicalInfo.Empty, message)
		{
		}

		public CompilerWarning(string code, LexicalInfo lexicalInfo, params object[] args)
			: this(lexicalInfo, ResourceManager.Format(code, args), code)
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
			stringBuilder.Append(_message);
			return stringBuilder.ToString();
		}
	}
}
