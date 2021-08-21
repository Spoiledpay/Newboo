using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Generics;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalGenericParameter : AbstractGenericParameter, IInternalEntity, IEntity
	{
		private int _position = -1;

		private readonly GenericParameterDeclaration _declaration;

		private IType[] _baseTypes;

		public override int GenericParameterPosition
		{
			get
			{
				if (_position == -1)
				{
					IGenericParameter[] array = ((base.DeclaringMethod != null) ? base.DeclaringMethod.GenericInfo.GenericParameters : base.DeclaringType.GenericInfo.GenericParameters);
					_position = Array.IndexOf(array, this);
				}
				return _position;
			}
		}

		public override IEntity DeclaringEntity => TypeSystemServices.GetEntity(_declaration.ParentNode);

		public override string Name => _declaration.Name;

		public Node Node => _declaration;

		public override bool IsValueType => HasConstraint(GenericParameterConstraints.ValueType);

		public override bool IsClass => HasConstraint(GenericParameterConstraints.ReferenceType);

		public override bool MustHaveDefaultConstructor => HasConstraint(GenericParameterConstraints.Constructable);

		public override Variance Variance
		{
			get
			{
				if (HasConstraint(GenericParameterConstraints.Covariant))
				{
					return Variance.Covariant;
				}
				if (HasConstraint(GenericParameterConstraints.Contravariant))
				{
					return Variance.Contravariant;
				}
				return Variance.Invariant;
			}
		}

		public InternalGenericParameter(TypeSystemServices tss, GenericParameterDeclaration declaration)
			: base(tss)
		{
			_declaration = declaration;
		}

		public InternalGenericParameter(TypeSystemServices tss, GenericParameterDeclaration declaration, int position)
			: this(tss, declaration)
		{
			_position = position;
		}

		public override IType[] GetTypeConstraints()
		{
			if (_baseTypes == null)
			{
				List<IType> list = new List<IType>();
				foreach (TypeReference baseType in _declaration.BaseTypes)
				{
					IType type = (IType)baseType.Entity;
					if (type != null)
					{
						list.Add(type);
					}
					else if (IsDeclaringTypeReference(baseType))
					{
						list.Add(base.DeclaringType);
					}
				}
				_baseTypes = list.ToArray();
			}
			return _baseTypes;
		}

		private bool HasConstraint(GenericParameterConstraints flag)
		{
			return (_declaration.Constraints & flag) == flag;
		}

		private bool IsDeclaringTypeReference(TypeReference reference)
		{
			if (!(reference is GenericTypeReference) || !(base.DeclaringType is InternalClass))
			{
				return false;
			}
			return Node.ParentNode == ((InternalClass)base.DeclaringType).Node;
		}
	}
}
