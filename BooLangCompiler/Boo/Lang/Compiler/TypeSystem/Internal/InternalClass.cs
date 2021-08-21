using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalClass : AbstractInternalType
	{
		private int _typeDepth = -1;

		private bool _isPointer;

		public override bool IsValueType => _provider.ValueTypeType == BaseType;

		public override bool IsPointer => _isPointer;

		public override IType BaseType => FindBaseType();

		internal InternalClass(InternalTypeSystemProvider provider, TypeDefinition typeDefinition)
			: this(provider, typeDefinition, isByRef: false)
		{
		}

		internal InternalClass(InternalTypeSystemProvider provider, TypeDefinition typeDefinition, bool isByRef)
			: base(provider, typeDefinition)
		{
			base.IsByRef = isByRef;
		}

		private IType FindBaseType()
		{
			foreach (TypeReference baseType in _node.BaseTypes)
			{
				IType type = (IType)baseType.Entity;
				if (!(type?.IsInterface ?? true))
				{
					return type;
				}
			}
			return null;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			bool flag = base.Resolve(resultingSet, name, typesToConsider);
			IType baseType = BaseType;
			if (null != baseType)
			{
				flag |= baseType.Resolve(resultingSet, name, typesToConsider);
			}
			return flag;
		}

		public override int GetTypeDepth()
		{
			if (-1 == _typeDepth)
			{
				_typeDepth = 1 + BaseType.GetTypeDepth();
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
			return _provider.IsSystemObject(type);
		}

		protected override IType CreateElementType()
		{
			return new InternalClass(_provider, _node, isByRef: true);
		}

		public override IType MakePointerType()
		{
			InternalClass internalClass = new InternalClass(_provider, _node);
			internalClass._isPointer = true;
			internalClass._elementType = this;
			return internalClass;
		}
	}
}
