using System.Collections;
using System.IO;
using System.Text;

namespace Boo.Lang.Compiler.Steps
{
	public class PrintErrors : AbstractCompilerStep
	{
		public override void Run()
		{
			Print(base.OutputWriter, base.Errors);
		}

		public static void Print(TextWriter writer, CompilerErrorCollection errors)
		{
			Hashtable hashtable = new Hashtable();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompilerError error in errors)
			{
				stringBuilder.Length = 0;
				stringBuilder.Append(Path.GetFileName(error.LexicalInfo.FileName));
				stringBuilder.AppendFormat("({0},{1}): ", error.LexicalInfo.Line, error.LexicalInfo.Column);
				stringBuilder.AppendFormat("{0}: ", error.Code);
				stringBuilder.Append(error.Message);
				string text = stringBuilder.ToString();
				if (!hashtable.ContainsKey(text))
				{
					hashtable.Add(text, text);
					writer.WriteLine(text);
				}
			}
		}
	}
}
