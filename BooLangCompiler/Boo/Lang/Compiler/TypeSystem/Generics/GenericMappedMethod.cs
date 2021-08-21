using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericMappedMethod : GenericMappedAccessibleMember<IMethod>, IMethod, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity, IOverridableMember, IGenericMethodInfo, IGenericParametersProvider
	{
		private IParameter[] _parameters = null;

		private IGenericParameter[] _typeParameters = null;

		private ICallableType _callableType = null;

		private IDictionary<IType[], IMethod> _constructedMethods = new Dictionary<IType[], IMethod>();

		public bool IsAbstract => base.SourceMember.IsAbstract;

		public bool IsVirtual => base.SourceMember.IsVirtual;

		public bool IsFinal => base.SourceMember.IsFinal;

		public bool IsSpecialName => base.SourceMember.IsSpecialName;

		public bool IsPInvoke => base.SourceMember.IsPInvoke;

		public virtual IConstructedMethodInfo ConstructedInfo => null;

		public IGenericMethodInfo GenericInfo
		{
			get
			{
				if (base.SourceMember.GenericInfo == null)
				{
					return null;
				}
				return this;
			}
		}

		public ICallableType CallableType
		{
			get
			{
				if (null == _callableType)
				{
					_callableType = _tss.GetCallableType(this);
				}
				return _callableType;
			}
		}

		public bool AcceptVarArgs => base.SourceMember.AcceptVarArgs;

		public bool IsExtension => base.SourceMember.IsExtension;

		public IType ReturnType => base.GenericMapping.MapType(base.SourceMember.ReturnType);

		IGenericParameter[] IGenericParametersProvider.GenericParameters
		{
			get
			{
				if (_typeParameters == null)
				{
					_typeParameters = Array.ConvertAll(base.SourceMember.GenericInfo.GenericParameters, base.GenericMapping.MapGenericParameter);
				}
				return _typeParameters;
			}
		}

		public GenericMappedMethod(TypeSystemServices tss, IMethod source, GenericMapping genericMapping)
			: base(tss, source, genericMapping)
		{
		}

		public IParameter[] GetParameters()
		{
			if (_parameters == null)
			{
				_parameters = base.GenericMapping.MapParameters(base.SourceMember.GetParameters());
			}
			return _parameters;
		}

		IMethod IGenericMethodInfo.ConstructMethod(params IType[] arguments)
		{
			IMethod value = null;
			if (!_constructedMethods.TryGetValue(arguments, out value))
			{
				value = new GenericConstructedMethod(this, arguments);
				_constructedMethods.Add(arguments, value);
			}
			return value;
		}

		protected override string BuildFullName()
		{
			StringBuilder stringBuilder = new StringBuilder(base.BuildFullName());
			if (GenericInfo != null)
			{
				stringBuilder.Append("[of ");
				string[] value = Array.ConvertAll(GenericInfo.GenericParameters, (IGenericParameter gp) => gp.Name);
				stringBuilder.Append(string.Join(", ", value));
				stringBuilder.Append("]");
			}
			stringBuilder.Append("(");
			string[] value2 = Array.ConvertAll(GetParameters(), (IParameter p) => p.Type.Name);
			stringBuilder.Append(string.Join(", ", value2));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
