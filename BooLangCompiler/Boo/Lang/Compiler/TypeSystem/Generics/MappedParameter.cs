namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class MappedParameter : IParameter, ITypedEntity, IEntity
	{
		private IType _type;

		private IParameter _baseParameter;

		public bool IsByRef => _baseParameter.IsByRef;

		public IType Type => _type;

		public string Name => _baseParameter.Name;

		public string FullName => _baseParameter.FullName;

		public EntityType EntityType => EntityType.Parameter;

		public MappedParameter(IParameter baseParameter, IType type)
		{
			_baseParameter = baseParameter;
			_type = type;
		}
	}
}
