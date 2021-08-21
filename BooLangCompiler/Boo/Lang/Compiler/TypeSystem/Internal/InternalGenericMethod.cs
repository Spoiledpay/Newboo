using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class InternalGenericMethod : InternalMethod, IGenericMethodInfo, IGenericParametersProvider
	{
		private IGenericParameter[] _genericParameters;

		private Dictionary<IType[], IMethod> _constructedMethods = new Dictionary<IType[], IMethod>(ArrayEqualityComparer<IType>.Default);

		public override IGenericMethodInfo GenericInfo => this;

		public IGenericParameter[] GenericParameters => _genericParameters ?? (_genericParameters = Array.ConvertAll(base.Method.GenericParameters.ToArray(), (GenericParameterDeclaration gpd) => (IGenericParameter)gpd.Entity));

		public override string FullName => _node.FullName;

		public InternalGenericMethod(InternalTypeSystemProvider provider, Method method)
			: base(provider, method)
		{
		}

		public IMethod ConstructMethod(IType[] arguments)
		{
			if (!_constructedMethods.TryGetValue(arguments, out var value))
			{
				value = new GenericConstructedMethod(this, arguments);
				_constructedMethods.Add(arguments, value);
			}
			return value;
		}

		public override bool Resolve(ICollection<IEntity> resultingSet, string name, EntityType typesToConsider)
		{
			if (Entities.IsFlagSet(typesToConsider, EntityType.Type))
			{
				foreach (GenericParameterDeclaration genericParameter in base.Method.GenericParameters)
				{
					if (genericParameter.Name == name)
					{
						resultingSet.Add(genericParameter.Entity);
						return true;
					}
				}
			}
			return base.Resolve(resultingSet, name, typesToConsider);
		}
	}
}
