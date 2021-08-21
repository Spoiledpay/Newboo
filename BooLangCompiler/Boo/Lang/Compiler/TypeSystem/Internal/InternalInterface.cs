using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalInterface : AbstractInternalType
	{
		private int _typeDepth = -1;

		public override IType BaseType => My<TypeSystemServices>.Instance.ObjectType;

		internal InternalInterface(InternalTypeSystemProvider provider, TypeDefinition typeDefinition)
			: this(provider, typeDefinition, isByRef: false)
		{
		}

		internal InternalInterface(InternalTypeSystemProvider provider, TypeDefinition typeDefinition, bool isByRef)
			: base(provider, typeDefinition)
		{
			base.IsByRef = isByRef;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool flag = base.Resolve(resultingSet, name, typesToConsider);
			foreach (TypeReference baseType in _node.BaseTypes)
			{
				if (TypeSystemServices.GetType(baseType).Resolve(resultingSet, name, typesToConsider))
				{
					flag = true;
				}
			}
			if (!flag && BaseType.Resolve(resultingSet, name, typesToConsider))
			{
				flag = true;
			}
			return flag;
		}

		public override int GetTypeDepth()
		{
			if (-1 == _typeDepth)
			{
				_typeDepth = 1 + GetMaxBaseInterfaceDepth();
			}
			return _typeDepth;
		}

		public override bool IsSubclassOf(IType type)
		{
			foreach (TypeReference baseType in _node.BaseTypes)
			{
				IType type2 = TypeSystemServices.GetType(baseType);
				if (type == type2 || type2.IsSubclassOf(type))
				{
					return true;
				}
			}
			return My<TypeSystemServices>.Instance.IsSystemObject(type);
		}

		private int GetMaxBaseInterfaceDepth()
		{
			int num = 0;
			foreach (TypeReference baseType in _node.BaseTypes)
			{
				IType type = TypeSystemServices.GetType(baseType);
				int typeDepth = type.GetTypeDepth();
				if (typeDepth > num)
				{
					num = typeDepth;
				}
			}
			return num;
		}

		protected override IType CreateElementType()
		{
			return new InternalInterface(_provider, _node, isByRef: true);
		}
	}
}
