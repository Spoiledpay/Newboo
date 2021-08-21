using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ModuleCollection : NodeCollection<Module>
	{
		[GeneratedCode("astgen.boo", "1")]
		public static ModuleCollection FromArray(params Module[] items)
		{
			ModuleCollection moduleCollection = new ModuleCollection();
			moduleCollection.AddRange(items);
			return moduleCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ModuleCollection PopRange(int begin)
		{
			ModuleCollection moduleCollection = new ModuleCollection(base.ParentNode);
			moduleCollection.InnerList.AddRange(InternalPopRange(begin));
			return moduleCollection;
		}

		public ModuleCollection()
		{
		}

		public ModuleCollection(Node parent)
			: base(parent)
		{
		}
	}
}
