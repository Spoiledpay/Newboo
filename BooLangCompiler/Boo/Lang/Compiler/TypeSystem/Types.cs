using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.TypeSystem
{
	public static class Types
	{
		public static readonly Type RuntimeServices = typeof(RuntimeServices);

		public static readonly Type Builtins = typeof(Builtins);

		public static readonly Type List = typeof(List);

		public static readonly Type Hash = typeof(Hash);

		public static readonly Type ICallable = typeof(ICallable);

		public static readonly Type ICollection = typeof(ICollection);

		public static readonly Type IEnumerable = typeof(IEnumerable);

		public static readonly Type IEnumerableGeneric = typeof(IEnumerable<>);

		public static readonly Type Object = typeof(object);

		public static readonly Type Regex = typeof(Regex);

		public static readonly Type ValueType = typeof(ValueType);

		public static readonly Type Array = typeof(Array);

		public static readonly Type ObjectArray = typeof(object[]);

		public static readonly Type Void = typeof(void);

		public static readonly Type String = typeof(string);

		public static readonly Type Byte = typeof(byte);

		public static readonly Type SByte = typeof(sbyte);

		public static readonly Type Char = typeof(char);

		public static readonly Type Short = typeof(short);

		public static readonly Type Int = typeof(int);

		public static readonly Type Long = typeof(long);

		public static readonly Type UShort = typeof(ushort);

		public static readonly Type UInt = typeof(uint);

		public static readonly Type ULong = typeof(ulong);

		public static readonly Type TimeSpan = typeof(TimeSpan);

		public static readonly Type DateTime = typeof(DateTime);

		public static readonly Type Single = typeof(float);

		public static readonly Type Double = typeof(double);

		public static readonly Type Decimal = typeof(decimal);

		public static readonly Type Bool = typeof(bool);

		public static readonly Type IntPtr = typeof(IntPtr);

		public static readonly Type UIntPtr = typeof(UIntPtr);

		public static readonly Type Type = typeof(Type);

		public static readonly Type MulticastDelegate = typeof(MulticastDelegate);

		public static readonly Type Delegate = typeof(Delegate);

		public static readonly Type DuckTypedAttribute = typeof(DuckTypedAttribute);

		public static readonly Type ClrExtensionAttribute = typeof(System.Runtime.CompilerServices.ExtensionAttribute);

		public static readonly Type DllImportAttribute = typeof(DllImportAttribute);

		public static readonly Type ModuleAttribute = typeof(CompilerGlobalScopeAttribute);

		public static readonly Type ParamArrayAttribute = typeof(ParamArrayAttribute);

		public static readonly Type DefaultMemberAttribute = typeof(DefaultMemberAttribute);

		public static readonly Type Nullable = typeof(Nullable<>);

		public static readonly Type CompilerGeneratedAttribute = typeof(CompilerGeneratedAttribute);
	}
}
