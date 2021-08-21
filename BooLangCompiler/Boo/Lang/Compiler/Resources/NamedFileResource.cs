using System;

namespace Boo.Lang.Compiler.Resources
{
	public class NamedFileResource : FileResource
	{
		private string _rname;

		public override string Name => _rname;

		public NamedFileResource(string fname, string rname)
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
