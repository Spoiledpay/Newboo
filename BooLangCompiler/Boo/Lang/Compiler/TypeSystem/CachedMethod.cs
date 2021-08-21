namespace Boo.Lang.Compiler.TypeSystem
{
	internal sealed class CachedMethod
	{
		public readonly IMethod Value;

		public CachedMethod(IMethod value)
		{
			Value = value;
		}
	}
}
