using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace Boo.Lang.Compiler.Steps
{
	public class SaveAssembly : AbstractCompilerStep
	{
		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				AssemblyBuilder assemblyBuilder = ContextAnnotations.GetAssemblyBuilder(base.Context);
				string fileName = Path.GetFileName(base.Context.GeneratedAssemblyFileName);
				Save(assemblyBuilder, fileName);
			}
		}

		private void Save(AssemblyBuilder builder, string filename)
		{
			switch (base.Parameters.Platform)
			{
			case "x86":
				builder.Save(filename, PortableExecutableKinds.Required32Bit, ImageFileMachine.I386);
				break;
			case "x64":
				builder.Save(filename, PortableExecutableKinds.PE32Plus, ImageFileMachine.AMD64);
				break;
			case "itanium":
				builder.Save(filename, PortableExecutableKinds.PE32Plus, ImageFileMachine.IA64);
				break;
			default:
				builder.Save(filename);
				break;
			}
		}
	}
}
