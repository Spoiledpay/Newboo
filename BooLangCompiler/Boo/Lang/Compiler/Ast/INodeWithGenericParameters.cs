namespace Boo.Lang.Compiler.Ast
{
	public interface INodeWithGenericParameters
	{
		GenericParameterDeclarationCollection GenericParameters { get; }
	}
}
