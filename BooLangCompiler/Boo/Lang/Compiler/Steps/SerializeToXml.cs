using System;
using System.Xml.Serialization;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class SerializeToXml : AbstractCompilerStep
	{
		public override void Run()
		{
			CompileUnit compileUnit = base.Context.CompileUnit;
			new XmlSerializer(compileUnit.GetType()).Serialize(base.OutputWriter, compileUnit);
			Console.WriteLine();
		}
	}
}
