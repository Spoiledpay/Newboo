using System;
using System.Reflection;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class TypeInferenceRuleProvider
	{
		public virtual string TypeInferenceRuleFor(MethodBase method)
		{
			return TypeInferenceRuleFor(method, typeof(TypeInferenceRuleAttribute));
		}

		protected string TypeInferenceRuleFor(MethodBase method, Type attributeType)
		{
			return Attribute.GetCustomAttribute(method, attributeType)?.ToString();
		}
	}
}
