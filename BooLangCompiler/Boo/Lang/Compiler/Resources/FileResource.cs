using System;
using System.Collections;
using System.IO;
using System.Resources;

namespace Boo.Lang.Compiler.Resources
{
	public class FileResource : ICompilerResource
	{
		protected string _fname;

		public string FileName => _fname;

		public virtual string Name => Path.GetFileName(_fname);

		public virtual string Description => null;

		public FileResource(string fname)
		{
			if (null == fname)
			{
				throw new ArgumentNullException("fname");
			}
			_fname = fname;
		}

		public void WriteResource(IResourceService service)
		{
			using ResourceReader resourceReader = new ResourceReader(FileName);
			IResourceWriter resourceWriter = service.DefineResource(Name, Description);
			IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
			while (enumerator.MoveNext())
			{
				resourceWriter.AddResource((string)enumerator.Key, enumerator.Value);
			}
		}
	}
}
