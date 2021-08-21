using System;
using System.Collections;
using System.Text;
using antlr;

namespace Boo.Lang.Parser.Util
{
	public class IndentTokenStreamFilter : TokenStream
	{
		private static readonly char[] NewLineCharArray = new char[2] { '\r', '\n' };

		protected TokenStream _istream;

		protected int _wsTokenType;

		protected int _indentTokenType;

		protected int _dedentTokenType;

		protected int _eosTokenType;

		protected Stack _indentStack;

		protected Queue _pendingTokens;

		protected IToken _lastNonWsToken;

		protected string _expectedIndent;

		private StringBuilder _buffer = new StringBuilder();

		public TokenStream InnerStream => _istream;

		protected int CurrentIndentLevel => (int)_indentStack.Peek();

		public IndentTokenStreamFilter(TokenStream istream, int wsTokenType, int indentTokenType, int dedentTokenType, int eosTokenType)
		{
			if (null == istream)
			{
				throw new ArgumentNullException("istream");
			}
			_istream = istream;
			_wsTokenType = wsTokenType;
			_indentTokenType = indentTokenType;
			_dedentTokenType = dedentTokenType;
			_eosTokenType = eosTokenType;
			_indentStack = new Stack();
			_pendingTokens = new Queue();
			_indentStack.Push(0);
		}

		public IToken nextToken()
		{
			if (_pendingTokens.Count == 0)
			{
				ProcessNextTokens();
			}
			return (IToken)_pendingTokens.Dequeue();
		}

		private void ResetBuffer()
		{
			_buffer.Length = 0;
		}

		private IToken BufferUntilNextNonWhiteSpaceToken()
		{
			IToken token = null;
			while (true)
			{
				bool flag = true;
				token = _istream.nextToken();
				int type = token.Type;
				if (Token.SKIP != type)
				{
					if (_wsTokenType != type)
					{
						break;
					}
					_buffer.Append(token.getText());
				}
			}
			return token;
		}

		private void FlushBuffer(IToken token)
		{
			if (0 == _buffer.Length)
			{
				return;
			}
			string text = _buffer.ToString();
			string[] array = text.Split(NewLineCharArray);
			if (array.Length <= 1)
			{
				return;
			}
			string text2 = array[array.Length - 1];
			if (string.Empty != text2)
			{
				if (null == _expectedIndent)
				{
					_expectedIndent = text2.Substring(0, 1);
				}
				if (string.Empty != text2.Replace(_expectedIndent, string.Empty))
				{
					string text3 = ((_expectedIndent == "\t") ? "tabs" : ((_expectedIndent == "\f") ? "form feeds" : "spaces"));
					throw new TokenStreamRecognitionException(new RecognitionException("Mixed indentation, expected the use of " + text3, token.getFilename(), token.getLine(), text2.Length - text2.TrimStart(_expectedIndent[0]).Length + 1));
				}
			}
			if (text2.Length > CurrentIndentLevel)
			{
				EnqueueIndent(token);
				_indentStack.Push(text2.Length);
			}
			else if (text2.Length < CurrentIndentLevel)
			{
				EnqueueEOS(token);
				do
				{
					EnqueueDedent();
					_indentStack.Pop();
				}
				while (text2.Length < CurrentIndentLevel);
			}
			else
			{
				EnqueueEOS(token);
			}
		}

		private void CheckForEOF(IToken token)
		{
			if (1 == token.Type)
			{
				EnqueueEOS(token);
				while (CurrentIndentLevel > 0)
				{
					EnqueueDedent();
					_indentStack.Pop();
				}
			}
		}

		private void ProcessNextNonWhiteSpaceToken(IToken token)
		{
			_lastNonWsToken = token;
			Enqueue(token);
		}

		private void ProcessNextTokens()
		{
			ResetBuffer();
			IToken token = BufferUntilNextNonWhiteSpaceToken();
			FlushBuffer(token);
			CheckForEOF(token);
			ProcessNextNonWhiteSpaceToken(token);
		}

		private void Enqueue(IToken token)
		{
			_pendingTokens.Enqueue(token);
		}

		private void EnqueueIndent(IToken prototype)
		{
			_pendingTokens.Enqueue(CreateToken(prototype, _indentTokenType, "<INDENT>"));
		}

		private void EnqueueDedent()
		{
			_pendingTokens.Enqueue(CreateToken(_lastNonWsToken, _dedentTokenType, "<DEDENT>"));
		}

		private void EnqueueEOS(IToken prototype)
		{
			_pendingTokens.Enqueue(CreateToken(prototype, _eosTokenType, "<EOL>"));
		}

		private IToken CreateToken(IToken prototype, int newTokenType, string newTokenText)
		{
			return new BooToken(newTokenType, newTokenText, prototype.getFilename(), prototype.getLine(), prototype.getColumn() + SafeGetLength(prototype.getText()));
		}

		private int SafeGetLength(string s)
		{
			return s?.Length ?? 0;
		}
	}
}
