using System.Resources;

namespace Boo.Lang.Compiler
{
	public interface IResourceService
	{
		IResourceWriter DefineResource(string resourceName, string resourceDescription);

		bool EmbedFile(string resourceName, string path);
	}
}
