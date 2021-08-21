using System;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericConstructedMethod : IMethod, IMethodBase, IAccessibleMember, IMember, ITypedEntity, IEntityWithAttributes, IExtensionEnabled, IEntityWithParameters, IEntity, IOverridableMember, IConstructedMethodInfo, IGenericArgumentsProvider
	{
		private IType[] _genericArguments;

		private IMethod _definition;

		private GenericMapping _genericMapping;

		private bool _fullyConstructed;

		private string _fullName;

		private IParameter[] _parameters;

		protected GenericMapping GenericMapping => _genericMapping;

		public IType ReturnType => GenericMapping.MapType(_definition.ReturnType);

		public bool IsAbstract => _definition.IsAbstract;

		public bool IsVirtual => _definition.IsVirtual;

		public bool IsFinal => _definition.IsFinal;

		public bool IsSpecialName => _definition.IsSpecialName;

		public bool IsPInvoke => _definition.IsPInvoke;

		public IConstructedMethodInfo ConstructedInfo => this;

		public IGenericMethodInfo GenericInfo => null;

		public ICallableType CallableType => My<TypeSystemServices>.Instance.GetCallableType(this);

		public bool IsExtension => _definition.IsExtension;

		public bool IsProtected => _definition.IsProtected;

		public bool IsInternal => _definition.IsInternal;

		public bool IsPrivate => _definition.IsPrivate;

		public bool AcceptVarArgs => _definition.AcceptVarArgs;

		public bool IsDuckTyped => _definition.IsDuckTyped;

		public IType DeclaringType => _definition.DeclaringType;

		public bool IsStatic => _definition.IsStatic;

		public bool IsPublic => _definition.IsPublic;

		public IType Type => CallableType;

		public string Name => _definition.Name;

		public string FullName => _fullName ?? (_fullName = BuildFullName());

		public EntityType EntityType => EntityType.Method;

		public IMethod GenericDefinition => _definition;

		public IType[] GenericArguments => _genericArguments;

		public bool FullyConstructed => _fullyConstructed;

		public GenericConstructedMethod(IMethod definition, IType[] arguments)
		{
			_definition = definition;
			_genericArguments = arguments;
			_genericMapping = new InternalGenericMapping(this, arguments);
			_fullyConstructed = IsFullyConstructed();
		}

		private bool IsFullyConstructed()
		{
			IType[] genericArguments = GenericArguments;
			foreach (IType type in genericArguments)
			{
				if (GenericsServices.IsOpenGenericType(type))
				{
					return false;
				}
			}
			return true;
		}

		private string BuildFullName()
		{
			string[] value = Array.ConvertAll(GenericArguments, (IType t) => t.FullName);
			return string.Format("{0}.{1}[of {2}]", DeclaringType.FullName, Name, string.Join(", ", value));
		}

		public IParameter[] GetParameters()
		{
			return _parameters ?? (_parameters = GenericMapping.MapParameters(_definition.GetParameters()));
		}

		public bool IsDefined(IType attributeType)
		{
			return _definition.IsDefined(attributeType);
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
