using System;

namespace Boo.Lang.Compiler.Resources
{
	public class NamedEmbeddedFileResource : EmbeddedFileResource
	{
		private string _rname;

		public override string Name => _rname;

		public NamedEmbeddedFileResource(string fname, string rname)
			: base(fname)
		{
			if (null == rname)
			{
				throw new ArgumentNullException("rname");
			}
			_rname = rname;
		}
	}
}
