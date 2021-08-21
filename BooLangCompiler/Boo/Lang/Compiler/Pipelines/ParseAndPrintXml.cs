using Boo.Lang.Compiler.Steps;

namespace Boo.Lang.Compiler.Pipelines
{
	public class ParseAndPrintXml : Parse
	{
		public ParseAndPrintXml()
		{
			Add(new SerializeToXml());
		}
	}
}
