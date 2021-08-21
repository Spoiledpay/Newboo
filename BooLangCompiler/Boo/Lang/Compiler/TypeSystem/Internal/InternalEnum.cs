using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalEnum : AbstractInternalType
	{
		private Type _underlyingType;

		public EnumDefinition EnumDefinition => (EnumDefinition)_node;

		public Type UnderlyingType
		{
			get
			{
				if (null != _underlyingType)
				{
					return _underlyingType;
				}
				return _underlyingType = (RequiresLongRepresentation() ? Types.Long : Types.Int);
			}
		}

		public override bool IsFinal => true;

		public override bool IsValueType => true;

		public override IType BaseType => My<TypeSystemServices>.Instance.EnumType;

		internal InternalEnum(InternalTypeSystemProvider provider, EnumDefinition enumDefinition)
			: this(provider, enumDefinition, isByRef: false)
		{
		}

		internal InternalEnum(InternalTypeSystemProvider provider, TypeDefinition enumDefinition, bool isByRef)
			: base(provider, enumDefinition)
		{
			base.IsByRef = isByRef;
		}

		private bool RequiresLongRepresentation()
		{
			return (from EnumMember member in EnumDefinition.Members
				select member.Initializer as IntegerLiteralExpression).Any((IntegerLiteralExpression il) => il?.IsLong ?? false);
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			return base.Resolve(resultingSet, name, typesToConsider) || BaseType.Resolve(resultingSet, name, typesToConsider);
		}

		public override bool IsSubclassOf(IType type)
		{
			IType baseType = BaseType;
			return type == baseType || baseType.IsSubclassOf(type);
		}

		protected override IType CreateElementType()
		{
			return new InternalEnum(_provider, _node, isByRef: true);
		}
	}
}
