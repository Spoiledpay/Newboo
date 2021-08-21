using System.IO;
using System.Text;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Parser
{
	internal class CodeFactory
	{
		public static TypeReference EnumerableTypeReferenceFor(TypeReference tr)
		{
			GenericTypeReference genericTypeReference = new GenericTypeReference(tr.LexicalInfo, "System.Collections.Generic.IEnumerable");
			genericTypeReference.GenericArguments.Add(tr);
			return genericTypeReference;
		}

		public static Module NewQuasiquoteModule(LexicalInfo li)
		{
			Module module = new Module(li);
			module.Name = ModuleNameFrom(li.FileName) + "$" + li.Line;
			return module;
		}

		public static string ModuleNameFrom(string readerName)
		{
			if (readerName.IndexOfAny(Path.GetInvalidPathChars()) > -1)
			{
				return EncodeModuleName(readerName);
			}
			return Path.GetFileNameWithoutExtension(Path.GetFileName(readerName));
		}

		private static string EncodeModuleName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			foreach (char c in name)
			{
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append("_");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
