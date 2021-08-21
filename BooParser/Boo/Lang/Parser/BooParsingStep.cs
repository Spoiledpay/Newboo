using System;
using System.IO;
using antlr;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Parser
{
	public class BooParsingStep : ICompilerStep, ICompilerComponent, IDisposable
	{
		private CompilerContext _context;

		protected CompilerContext Context => _context;

		protected int TabSize => My<ParserSettings>.Instance.TabSize;

		public void Initialize(CompilerContext context)
		{
			_context = context;
		}

		public void Dispose()
		{
			_context = null;
		}

		public void Run()
		{
			foreach (ICompilerInput item2 in _context.Parameters.Input)
			{
				try
				{
					using TextReader reader = item2.Open();
					ParseModule(item2.Name, reader, OnParserError);
				}
				catch (CompilerError item)
				{
					_context.Errors.Add(item);
				}
				catch (TokenStreamRecognitionException ex)
				{
					OnParserError(ex.recog);
				}
				catch (Exception error)
				{
					_context.Errors.Add(CompilerErrorFactory.InputError(item2.Name, error));
				}
			}
		}

		protected virtual void ParseModule(string inputName, TextReader reader, ParserErrorHandler errorHandler)
		{
			BooParser.ParseModule(TabSize, _context.CompileUnit, inputName, reader, errorHandler);
		}

		private void OnParserError(RecognitionException error)
		{
			LexicalInfo data = new LexicalInfo(error.getFilename(), error.getLine(), error.getColumn());
			NoViableAltException ex = error as NoViableAltException;
			if (null != ex)
			{
				ParserError(data, ex);
			}
			else
			{
				GenericParserError(data, error);
			}
		}

		private void GenericParserError(LexicalInfo data, RecognitionException error)
		{
			_context.Errors.Add(CompilerErrorFactory.GenericParserError(data, error));
		}

		private void ParserError(LexicalInfo data, NoViableAltException error)
		{
			_context.Errors.Add(CompilerErrorFactory.UnexpectedToken(data, error, error.token.getText()));
		}
	}
}
