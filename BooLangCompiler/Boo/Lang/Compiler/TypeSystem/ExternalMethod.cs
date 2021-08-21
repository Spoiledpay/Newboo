using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalMethod : ExternalEntity<MethodBase>, IMethod, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity, IOverridableMember, IEquatable<ExternalMethod>
	{
		protected IParameter[] _parameters;

		private bool? _acceptVarArgs;

		private bool? _isPInvoke;

		private bool? _isMeta;

		private ExternalGenericMethodInfo _genericMethodDefinitionInfo;

		private ExternalConstructedMethodInfo _genericMethodInfo;

		public bool IsMeta
		{
			get
			{
				if (!_isMeta.HasValue)
				{
					_isMeta = IsStatic && MetadataUtil.IsAttributeDefined(_memberInfo, typeof(MetaAttribute));
				}
				return _isMeta.Value;
			}
		}

		public bool IsPInvoke
		{
			get
			{
				if (!_isPInvoke.HasValue)
				{
					_isPInvoke = IsStatic && MetadataUtil.IsAttributeDefined(_memberInfo, Types.DllImportAttribute);
				}
				return _isPInvoke.Value;
			}
		}

		public virtual IType DeclaringType => _provider.Map(_memberInfo.DeclaringType);

		public bool IsStatic => _memberInfo.IsStatic;

		public bool IsPublic => _memberInfo.IsPublic;

		public bool IsProtected => _memberInfo.IsFamily || _memberInfo.IsFamilyOrAssembly;

		public bool IsPrivate => _memberInfo.IsPrivate;

		public bool IsAbstract => _memberInfo.IsAbstract;

		public bool IsInternal => _memberInfo.IsAssembly;

		public bool IsVirtual => _memberInfo.IsVirtual;

		public bool IsFinal => _memberInfo.IsFinal;

		public bool IsSpecialName => _memberInfo.IsSpecialName;

		public bool AcceptVarArgs
		{
			get
			{
				if (!_acceptVarArgs.HasValue)
				{
					ParameterInfo[] parameters = _memberInfo.GetParameters();
					_acceptVarArgs = parameters.Length > 0 && IsParamArray(parameters[parameters.Length - 1]);
				}
				return _acceptVarArgs.Value;
			}
		}

		public override EntityType EntityType => EntityType.Method;

		public ICallableType CallableType => My<TypeSystemServices>.Instance.GetCallableType(this);

		public IType Type => CallableType;

		public virtual IType ReturnType
		{
			get
			{
				MethodInfo methodInfo = _memberInfo as MethodInfo;
				if (null != methodInfo)
				{
					return _provider.Map(methodInfo.ReturnType);
				}
				return null;
			}
		}

		public MethodBase MethodInfo => _memberInfo;

		public IGenericMethodInfo GenericInfo
		{
			get
			{
				if (!MethodInfo.IsGenericMethodDefinition)
				{
					return null;
				}
				return _genericMethodDefinitionInfo ?? (_genericMethodDefinitionInfo = new ExternalGenericMethodInfo(_provider, this));
			}
		}

		public virtual IConstructedMethodInfo ConstructedInfo
		{
			get
			{
				if (!MethodInfo.IsGenericMethod)
				{
					return null;
				}
				return _genericMethodInfo ?? (_genericMethodInfo = new ExternalConstructedMethodInfo(_provider, this));
			}
		}

		protected override Type MemberType
		{
			get
			{
				MethodInfo methodInfo = _memberInfo as MethodInfo;
				if (null != methodInfo)
				{
					return methodInfo.ReturnType;
				}
				return null;
			}
		}

		internal ExternalMethod(IReflectionTypeSystemProvider provider, MethodBase mi)
			: base(provider, mi)
		{
		}

		private bool IsParamArray(ParameterInfo parameter)
		{
			return parameter.ParameterType.IsArray && parameter.GetCustomAttributes(Types.ParamArrayAttribute, inherit: false).Length > 0;
		}

		public virtual IParameter[] GetParameters()
		{
			if (null != _parameters)
			{
				return _parameters;
			}
			return _parameters = _provider.Map(_memberInfo.GetParameters());
		}

		public override bool Equals(object other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			ExternalMethod other2 = other as ExternalMethod;
			return Equals(other2);
		}

		public bool Equals(ExternalMethod other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			return _memberInfo.MethodHandle.Value == other._memberInfo.MethodHandle.Value;
		}

		public override int GetHashCode()
		{
			return _memberInfo.MethodHandle.Value.GetHashCode();
		}

		public override string ToString()
		{
			return _memberInfo.ToString();
		}
	}
}
