using System;
using System.Reflection;

namespace Boo.Lang.Compiler.TypeSystem.Reflection
{
	public interface IReflectionTypeSystemProvider
	{
		IAssemblyReference ForAssembly(Assembly assembly);

		IType Map(Type type);

		IMethod Map(MethodInfo method);

		IConstructor Map(ConstructorInfo ctor);

		IEntity Map(MemberInfo[] members);

		IEntity Map(MemberInfo member);

		IParameter[] Map(ParameterInfo[] parameters);

		IReflectionTypeSystemProvider Clone();
	}
}
