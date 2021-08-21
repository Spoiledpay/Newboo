using System.IO;

namespace Boo.Lang.Parser
{
	public class WSABooParsingStep : BooParsingStep
	{
		protected override void ParseModule(string inputName, TextReader reader, ParserErrorHandler errorHandler)
		{
			WSABooParser.ParseModule(base.TabSize, base.Context.CompileUnit, inputName, reader, errorHandler);
		}
	}
}
