using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class RuntimeMethodCache : AbstractCompilerComponent
	{
		private readonly Dictionary<string, IMethodBase> _methodCache = new Dictionary<string, IMethodBase>(StringComparer.Ordinal);

		public IMethod Activator_CreateInstance => CachedMethod("Activator_CreateInstance", () => Methods.Of<Type, object[], object>(Activator.CreateInstance));

		public IConstructor Exception_StringConstructor => CachedConstructor("Exception_StringConstructor", () => base.TypeSystemServices.GetStringExceptionConstructor());

		public IMethod TextReaderEnumerator_lines => CachedMethod("TextReaderEnumerator_lines", () => Methods.Of<TextReader, IEnumerable<string>>(TextReaderEnumerator.lines));

		public IMethod List_GetRange1 => CachedMethod("List_GetRange1", () => Methods.InstanceFunctionOf<List<object>, int, List<object>>((List<object> l) => l.GetRange));

		public IMethod List_GetRange2 => CachedMethod("List_GetRange2", () => Methods.InstanceFunctionOf<List<object>, int, int, List<object>>((List<object> l) => l.GetRange));

		public IMethod RuntimeServices_GetRange1 => CachedRuntimeServicesMethod("GetRange1", () => Methods.Of<Array, int, Array>(RuntimeServices.GetRange1));

		public IMethod RuntimeServices_GetRange2 => CachedRuntimeServicesMethod("GetRange2", () => Methods.Of<Array, int, int, Array>(RuntimeServices.GetRange2));

		public IMethod RuntimeServices_GetMultiDimensionalRange1 => CachedRuntimeServicesMethod("GetMultiDimensionalRange1", () => Methods.Of<Array, int[], bool[], bool[], Array>(RuntimeServices.GetMultiDimensionalRange1));

		public IMethod RuntimeServices_Len => CachedRuntimeServicesMethod("Len", () => Methods.Of<object, int>(RuntimeServices.Len));

		public IMethod RuntimeServices_Mid => CachedRuntimeServicesMethod("Mid", () => Methods.Of<string, int, int, string>(RuntimeServices.Mid));

		public IMethod RuntimeServices_NormalizeStringIndex => CachedRuntimeServicesMethod("NormalizeStringIndex", () => Methods.Of<string, int, int>(RuntimeServices.NormalizeStringIndex));

		public IMethod RuntimeServices_AddArrays => CachedRuntimeServicesMethod("AddArrays", () => Methods.Of<Type, Array, Array, Array>(RuntimeServices.AddArrays));

		public IMethod RuntimeServices_SetMultiDimensionalRange1 => CachedRuntimeServicesMethod("SetMultiDimensionalRange1", () => Methods.Of<Array, Array, int[], bool[], bool[]>(RuntimeServices.SetMultiDimensionalRange1));

		public IMethod RuntimeServices_GetEnumerable => CachedRuntimeServicesMethod("GetEnumerable", () => Methods.Of<object, IEnumerable>(RuntimeServices.GetEnumerable));

		public IMethod RuntimeServices_EqualityOperator => CachedMethod("RuntimeServices_EqualityOperator", () => Methods.Of<object, object, bool>(RuntimeServices.EqualityOperator));

		public IMethod Array_get_Length => CachedMethod("Array_get_Length", () => Methods.GetterOf((Array a) => a.Length));

		public IMethod Array_GetLength => CachedMethod("Array_GetLength", () => Methods.InstanceFunctionOf<Array, int, int>((Array a) => a.GetLength));

		public IMethod String_get_Length => CachedMethod("String_get_Length", () => Methods.GetterOf((string s) => s.Length));

		public IMethod String_Substring_Int => CachedMethod("String_Substring_Int", () => Methods.InstanceFunctionOf<string, int, string>((string s) => s.Substring));

		public IMethod ICollection_get_Count => CachedMethod("ICollection_get_Count", () => Methods.GetterOf((ICollection c) => c.Count));

		public IMethod ICallable_Call => CachedMethod("ICallable_Call", () => Methods.InstanceFunctionOf<ICallable, object[], object>((ICallable c) => c.Call));

		private IMethod CachedRuntimeServicesMethod(string methodName, Func<MethodInfo> producer)
		{
			return CachedMethod("RuntimeServices_" + methodName, producer);
		}

		private IMethod CachedMethod(string key, Func<MethodInfo> producer)
		{
			return (IMethod)CachedMethodBase(key, () => base.TypeSystemServices.Map(producer()));
		}

		private IConstructor CachedConstructor(string key, Func<IMethodBase> producer)
		{
			return (IConstructor)CachedMethodBase(key, producer);
		}

		private IMethodBase CachedMethodBase(string key, Func<IMethodBase> producer)
		{
			if (!_methodCache.TryGetValue(key, out var value))
			{
				value = producer();
				_methodCache.Add(key, value);
			}
			return value;
		}
	}
}
