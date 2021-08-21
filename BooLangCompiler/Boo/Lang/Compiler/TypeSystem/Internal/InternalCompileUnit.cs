using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalCompileUnit : ICompileUnit, IEntity
	{
		private INamespace _rootNamespace;

		private CompileUnit _unit;

		public EntityType EntityType => EntityType.CompileUnit;

		public string Name => "<CompileUnit>";

		public string FullName => Name;

		public INamespace RootNamespace
		{
			get
			{
				if (_rootNamespace != null)
				{
					return _rootNamespace;
				}
				return _rootNamespace = new CompileUnitNamespace(_unit);
			}
		}

		public InternalCompileUnit(CompileUnit unit)
		{
			_unit = unit;
		}
	}
}
