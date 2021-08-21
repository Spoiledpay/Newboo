using System.Reflection.Emit;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalLocal : AbstractLocalEntity, ILocalEntity, ITypedEntity, IInternalEntity, IEntity
	{
		private Local _local;

		private IType _type;

		private Declaration _originalDeclaration;

		public Node Node => _local;

		public string Name => _local.Name;

		public string FullName => _local.Name;

		public EntityType EntityType => EntityType.Local;

		public bool IsPrivateScope
		{
			get
			{
				return _local.PrivateScope;
			}
			set
			{
				_local.PrivateScope = value;
			}
		}

		public Local Local => _local;

		public IType Type => _type;

		public LocalBuilder LocalBuilder { get; set; }

		public Declaration OriginalDeclaration
		{
			get
			{
				return _originalDeclaration;
			}
			set
			{
				_originalDeclaration = value;
			}
		}

		public bool IsExplicit => null != _originalDeclaration;

		public InternalLocal(Local local, IType type)
		{
			_local = local;
			_type = type;
			base.IsShared = false;
		}

		public override string ToString()
		{
			return $"Local({Name}, {Type})";
		}
	}
}
