using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalGenericParameter : ExternalType, IGenericParameter, IType, ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		private IMethod _declaringMethod = null;

		public int GenericParameterPosition => base.ActualType.GenericParameterPosition;

		public override string FullName => $"{DeclaringEntity.FullName}.{Name}";

		public override IEntity DeclaringEntity
		{
			get
			{
				object result;
				if (_declaringMethod == null)
				{
					IEntity declaringType = base.DeclaringType;
					result = declaringType;
				}
				else
				{
					result = _declaringMethod;
				}
				return (IEntity)result;
			}
		}

		public Variance Variance => (base.ActualType.GenericParameterAttributes & GenericParameterAttributes.VarianceMask) switch
		{
			GenericParameterAttributes.None => Variance.Invariant, 
			GenericParameterAttributes.Covariant => Variance.Covariant, 
			GenericParameterAttributes.Contravariant => Variance.Contravariant, 
			_ => Variance.Invariant, 
		};

		public bool MustHaveDefaultConstructor => (base.ActualType.GenericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint;

		public override bool IsClass => (base.ActualType.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint;

		public override bool IsValueType => (base.ActualType.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint;

		public ExternalGenericParameter(IReflectionTypeSystemProvider provider, Type type)
			: base(provider, type)
		{
			if (type.DeclaringMethod != null)
			{
				_declaringMethod = (IMethod)provider.Map(type.DeclaringMethod);
			}
		}

		public IType[] GetTypeConstraints()
		{
			return Array.ConvertAll(base.ActualType.GetGenericParameterConstraints(), _provider.Map);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
