namespace Boo.Lang.Compiler.Ast
{
	public interface ITypedAnnotations
	{
		T Get<T>() where T : class;

		void Set<T>(T annotation) where T : class;
	}
}
