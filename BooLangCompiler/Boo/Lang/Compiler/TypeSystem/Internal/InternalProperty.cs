using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalProperty : InternalEntity<Property>, IProperty, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity
	{
		private readonly InternalTypeSystemProvider _provider;

		private IParameter[] _parameters;

		public override EntityType EntityType => EntityType.Property;

		public IType Type => (_node.Type != null) ? TypeSystemServices.GetType(_node.Type) : Unknown.Default;

		public bool AcceptVarArgs => false;

		public IProperty Overriden { get; set; }

		public Property Property => _node;

		public bool IsDuckTyped => Type == _provider.DuckType || _node.Attributes.Contains("Boo.Lang.DuckTypedAttribute");

		public InternalProperty(InternalTypeSystemProvider provider, Property property)
			: base(property)
		{
			_provider = provider;
		}

		public IParameter[] GetParameters()
		{
			return _parameters ?? (_parameters = _provider.Map(_node.Parameters));
		}

		public IMethod GetGetMethod()
		{
			if (_node.Getter != null)
			{
				return (IMethod)TypeSystemServices.GetEntity(_node.Getter);
			}
			return (Overriden != null) ? Overriden.GetGetMethod() : null;
		}

		public IMethod GetSetMethod()
		{
			if (_node.Setter != null)
			{
				return (IMethod)TypeSystemServices.GetEntity(_node.Setter);
			}
			return (Overriden != null) ? Overriden.GetSetMethod() : null;
		}

		public override string ToString()
		{
			return $"{base.Name} as {Type}";
		}
	}
}
