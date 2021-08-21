using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalParameter : AbstractLocalEntity, IParameter, ILocalEntity, ITypedEntity, IInternalEntity, IEntity
	{
		private ParameterDeclaration _parameter;

		public Node Node => _parameter;

		public string Name => _parameter.Name;

		public string FullName => _parameter.Name;

		public EntityType EntityType => EntityType.Parameter;

		public ParameterDeclaration Parameter => _parameter;

		public IType Type => (IType)TypeSystemServices.GetEntity(_parameter.Type);

		public int Index { get; set; }

		public bool IsPrivateScope => false;

		public bool IsDuckTyped => false;

		public bool IsByRef => _parameter.IsByRef;

		public InternalParameter(ParameterDeclaration parameter, int index)
		{
			_parameter = parameter;
			Index = index;
		}
	}
}
