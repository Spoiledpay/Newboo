using System;
using System.IO;

namespace Boo.Lang.Compiler.Resources
{
	public class EmbeddedFileResource : ICompilerResource
	{
		protected string _fname;

		public string FileName => _fname;

		public virtual string Name => Path.GetFileName(_fname);

		public EmbeddedFileResource(string fname)
		{
			if (null == fname)
			{
				throw new ArgumentNullException("fname");
			}
			_fname = fname;
		}

		public void WriteResource(IResourceService resourceService)
		{
			resourceService.EmbedFile(Name, FileName);
		}
	}
}
