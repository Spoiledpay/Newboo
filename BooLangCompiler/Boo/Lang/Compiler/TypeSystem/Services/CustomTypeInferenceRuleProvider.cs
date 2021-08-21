using System;
using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class CustomTypeInferenceRuleProvider : TypeInferenceRuleProvider
	{
		private readonly Type _attribute;

		public CustomTypeInferenceRuleProvider(string attribute)
		{
			_attribute = ResolveAttribute(attribute);
		}

		public override string TypeInferenceRuleFor(MethodBase method)
		{
			return base.TypeInferenceRuleFor(method) ?? TypeInferenceRuleFor(method, _attribute);
		}

		private static Type ResolveAttribute(string attribute)
		{
			ExternalType externalType = My<NameResolutionService>.Instance.ResolveQualifiedName(attribute) as ExternalType;
			if (externalType == null)
			{
				throw new InvalidOperationException($"Type '{attribute}' could not be found!");
			}
			return externalType.ActualType;
		}
	}
}
