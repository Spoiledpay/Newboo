using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalParameter : IParameter, ITypedEntity, IEntity
	{
		private IReflectionTypeSystemProvider _provider;

		protected ParameterInfo _parameter;

		public string Name => _parameter.Name;

		public string FullName => _parameter.Name;

		public EntityType EntityType => EntityType.Parameter;

		public virtual IType Type => _provider.Map(_parameter.ParameterType);

		public bool IsByRef => Type.IsByRef;

		public ExternalParameter(IReflectionTypeSystemProvider provider, ParameterInfo parameter)
		{
			_provider = provider;
			_parameter = parameter;
		}
	}
}
