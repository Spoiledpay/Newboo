using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class GenericConstraintsValidator
	{
		private readonly CompilerContext _context;

		private readonly Node _node;

		private readonly GenericParameterDeclarationCollection _parameters;

		public GenericConstraintsValidator(CompilerContext ctx, Node node, GenericParameterDeclarationCollection parameters)
		{
			_context = ctx;
			_node = node;
			_parameters = parameters;
		}

		public void Validate()
		{
			foreach (GenericParameterDeclaration parameter in _parameters)
			{
				new GenericConstraintValidator(_context, parameter).Validate();
			}
		}
	}
}
