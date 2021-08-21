using System;
using System.IO;
using antlr;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Parser.Util;

namespace Boo.Lang.Parser
{
	public class WSABooParser : WSABooParserBase
	{
		protected ParserErrorHandler Error;

		public WSABooParser(TokenStream lexer)
			: base(lexer)
		{
		}

		public static Module ParseModule(int tabSize, CompileUnit cu, string readerName, TextReader reader, ParserErrorHandler errorHandler)
		{
			if (Readers.IsEmpty(reader))
			{
				Module module = new Module(new LexicalInfo(readerName), ModuleNameFrom(readerName));
				cu.Modules.Add(module);
				return module;
			}
			Module module2 = CreateParser(tabSize, readerName, reader, errorHandler).start(cu);
			module2.Name = ModuleNameFrom(readerName);
			return module2;
		}

		private static string ModuleNameFrom(string readerName)
		{
			return CodeFactory.ModuleNameFrom(readerName);
		}

		public static WSABooParser CreateParser(int tabSize, string readerName, TextReader reader, ParserErrorHandler errorHandler)
		{
			WSABooParser wSABooParser = new WSABooParser(CreateBooLexer(tabSize, readerName, reader));
			wSABooParser.setFilename(readerName);
			wSABooParser.Error = (ParserErrorHandler)Delegate.Combine(wSABooParser.Error, errorHandler);
			return wSABooParser;
		}

		public static TokenStream CreateBooLexer(int tabSize, string readerName, TextReader reader)
		{
			TokenStreamSelector tokenStreamSelector = new TokenStreamSelector();
			WSABooLexer wSABooLexer = new WSABooLexer(reader);
			wSABooLexer.setFilename(readerName);
			wSABooLexer.Initialize(tokenStreamSelector, tabSize, BooToken.TokenCreator);
			tokenStreamSelector.select(wSABooLexer);
			return tokenStreamSelector;
		}

		public override void reportError(RecognitionException x)
		{
			if (null != Error)
			{
				Error(x);
			}
			else
			{
				base.reportError(x);
			}
		}
	}
}
