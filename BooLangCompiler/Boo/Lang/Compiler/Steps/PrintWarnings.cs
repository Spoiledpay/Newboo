using System.IO;

namespace Boo.Lang.Compiler.Steps
{
	public class PrintWarnings : AbstractCompilerStep
	{
		public override void Run()
		{
			foreach (CompilerWarning warning in base.Warnings)
			{
				base.OutputWriter.Write(Path.GetFileName(warning.LexicalInfo.FileName));
				base.OutputWriter.Write("({0},{1}): ", warning.LexicalInfo.Line, warning.LexicalInfo.Column);
				base.OutputWriter.Write("{0}: ", warning.Code);
				base.OutputWriter.WriteLine(warning.Message);
			}
		}
	}
}
