using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class SelfEntity : ITypedEntity, IEntity
	{
		private string _name;

		private IType _type;

		public string Name => _name;

		public string FullName => _name;

		public EntityType EntityType => EntityType.Unknown;

		public IType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public SelfEntity(string name, IType type)
		{
			_name = name;
			_type = type;
		}
	}
}
