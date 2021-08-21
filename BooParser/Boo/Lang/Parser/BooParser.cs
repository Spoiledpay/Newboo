using System;
using System.IO;
using antlr;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;
using Boo.Lang.Parser.Util;

namespace Boo.Lang.Parser
{
	public class BooParser : BooParserBase
	{
		protected ParserErrorHandler Error;

		public BooParser(TokenStream lexer)
			: base(lexer)
		{
		}

		public static Expression ParseExpression(int tabSize, string name, string text, ParserErrorHandler errorHandler)
		{
			return CreateParser(tabSize, name, new StringReader(text), errorHandler).expression();
		}

		public static Expression ParseExpression(string name, string text)
		{
			return ParseExpression(1, name, text, null);
		}

		public static CompileUnit ParseFile(string fname)
		{
			return ParseFile(4, fname);
		}

		public static CompileUnit ParseFile(int tabSize, string fname)
		{
			if (null == fname)
			{
				throw new ArgumentNullException("fname");
			}
			using StreamReader reader = File.OpenText(fname);
			return ParseReader(tabSize, fname, reader);
		}

		public static CompileUnit ParseString(string name, string text)
		{
			return ParseReader(name, new StringReader(text));
		}

		public static CompileUnit ParseReader(string readerName, TextReader reader)
		{
			return ParseReader(4, readerName, reader);
		}

		public static CompileUnit ParseReader(int tabSize, string readerName, TextReader reader)
		{
			CompileUnit compileUnit = new CompileUnit();
			ParseModule(tabSize, compileUnit, readerName, reader, null);
			return compileUnit;
		}

		public static Module ParseModule(int tabSize, CompileUnit cu, string readerName, TextReader reader, ParserErrorHandler errorHandler)
		{
			if (Readers.IsEmpty(reader))
			{
				Module module = new Module(new LexicalInfo(readerName), CodeFactory.ModuleNameFrom(readerName));
				cu.Modules.Add(module);
				return module;
			}
			Module module2 = CreateParser(tabSize, readerName, reader, errorHandler).start(cu);
			module2.Name = CodeFactory.ModuleNameFrom(readerName);
			return module2;
		}

		public static BooParser CreateParser(int tabSize, string readerName, TextReader reader, ParserErrorHandler errorHandler)
		{
			BooParser booParser = new BooParser(CreateBooLexer(tabSize, readerName, reader));
			booParser.setFilename(readerName);
			booParser.Error = (ParserErrorHandler)Delegate.Combine(booParser.Error, errorHandler);
			return booParser;
		}

		public static TokenStream CreateBooLexer(int tabSize, string readerName, TextReader reader)
		{
			TokenStreamSelector tokenStreamSelector = new TokenStreamSelector();
			BooLexer booLexer = new BooLexer(reader);
			booLexer.setFilename(readerName);
			booLexer.Initialize(tokenStreamSelector, tabSize, BooToken.TokenCreator);
			IndentTokenStreamFilter stream = new IndentTokenStreamFilter(booLexer, 128, 4, 5, 9);
			tokenStreamSelector.select(stream);
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

		protected override void EmitIndexedPropertyDeprecationWarning(Property deprecated)
		{
			if (!OutsideCompilationEnvironment())
			{
				EmitWarning(CompilerWarningFactory.ObsoleteSyntax(deprecated, FormatPropertyWithDelimiters(deprecated, "(", ")"), FormatPropertyWithDelimiters(deprecated, "[", "]")));
			}
		}

		protected override void EmitTransientKeywordDeprecationWarning(LexicalInfo location)
		{
			if (!OutsideCompilationEnvironment())
			{
				EmitWarning(CompilerWarningFactory.ObsoleteSyntax(location, "transient keyword", "[Transient] attribute"));
			}
		}

		private void EmitWarning(CompilerWarning warning)
		{
			My<CompilerWarningCollection>.Instance.Add(warning);
		}

		private bool OutsideCompilationEnvironment()
		{
			return ActiveEnvironment.Instance == null;
		}

		private string FormatPropertyWithDelimiters(Property deprecated, string leftDelimiter, string rightDelimiter)
		{
			return deprecated.Name + leftDelimiter + Builtins.join(deprecated.Parameters, ", ") + rightDelimiter;
		}
	}
}
