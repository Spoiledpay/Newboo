#define DEBUG
#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Runtime;

namespace Boo.Lang.Compiler.Steps
{
	public class EmitAssembly : AbstractFastVisitorCompilerStep
	{
		private sealed class AttributeEmitVisitor : FastDepthFirstVisitor
		{
			private EmitAssembly _emitter;

			public AttributeEmitVisitor(EmitAssembly emitter)
			{
				_emitter = emitter;
			}

			public override void OnField(Field node)
			{
				_emitter.EmitFieldAttributes(node);
			}

			public override void OnEnumMember(EnumMember node)
			{
				_emitter.EmitFieldAttributes(node);
			}

			public override void OnEvent(Event node)
			{
				_emitter.EmitEventAttributes(node);
			}

			public override void OnProperty(Property node)
			{
				Visit(node.Getter);
				Visit(node.Setter);
				_emitter.EmitPropertyAttributes(node);
			}

			public override void OnConstructor(Constructor node)
			{
				Visit(node.Parameters);
				_emitter.EmitConstructorAttributes(node);
			}

			public override void OnMethod(Method node)
			{
				Visit(node.Parameters);
				_emitter.EmitMethodAttributes(node);
			}

			public override void OnParameterDeclaration(ParameterDeclaration node)
			{
				_emitter.EmitParameterAttributes(node);
			}

			public override void OnClassDefinition(ClassDefinition node)
			{
				base.OnClassDefinition(node);
				_emitter.EmitTypeAttributes(node);
			}

			public override void OnInterfaceDefinition(InterfaceDefinition node)
			{
				base.OnInterfaceDefinition(node);
				_emitter.EmitTypeAttributes(node);
			}

			public override void OnEnumDefinition(EnumDefinition node)
			{
				base.OnEnumDefinition(node);
				_emitter.EmitTypeAttributes(node);
			}
		}

		private delegate void CustomAttributeSetter(CustomAttributeBuilder attribute);

		private sealed class TypeCreator
		{
			private EmitAssembly _emitter;

			private Set<TypeDefinition> _created;

			private List<TypeDefinition> _types;

			private TypeDefinition _current;

			public TypeCreator(EmitAssembly emitter, List<TypeDefinition> types)
			{
				_emitter = emitter;
				_types = types;
				_created = new Set<TypeDefinition>();
			}

			public void Run()
			{
				AppDomain domain = Thread.GetDomain();
				try
				{
					domain.TypeResolve += OnTypeResolve;
					CreateTypes();
				}
				finally
				{
					domain.TypeResolve -= OnTypeResolve;
				}
			}

			private Assembly OnTypeResolve(object sender, ResolveEventArgs args)
			{
				Trace("OnTypeResolve('{0}') during '{1}' creation.", args.Name, _current);
				EnsureInternalFieldDependencies(_current);
				return _emitter._asmBuilder;
			}

			private void CreateTypes()
			{
				foreach (TypeDefinition type in _types)
				{
					CreateType(type);
				}
			}

			private void CreateType(TypeDefinition type)
			{
				if (!_created.Contains(type))
				{
					_created.Add(type);
					TypeDefinition current = _current;
					_current = type;
					try
					{
						HandleTypeCreation(type);
					}
					catch (Exception cause)
					{
						throw CompilerErrorFactory.InternalError(type, $"Failed to create '{type}' type.", cause);
					}
					_current = current;
				}
			}

			private void HandleTypeCreation(TypeDefinition type)
			{
				Trace("creating type '{0}'", type);
				if (IsNestedType(type))
				{
					CreateOuterTypeOf(type);
				}
				CreateRelatedTypes(type);
				TypeBuilder typeBuilder = (TypeBuilder)_emitter.GetBuilder(type);
				typeBuilder.CreateType();
				Trace("type '{0}' successfully created", type);
			}

			private void CreateOuterTypeOf(TypeMember type)
			{
				CreateType(type.DeclaringType);
			}

			private void CreateRelatedTypes(TypeDefinition typedef)
			{
				CreateRelatedTypes(typedef.BaseTypes);
				foreach (GenericParameterDeclaration genericParameter in typedef.GenericParameters)
				{
					CreateRelatedTypes(genericParameter.BaseTypes);
				}
			}

			private void EnsureInternalFieldDependencies(TypeDefinition typedef)
			{
				foreach (Field item in typedef.Members.OfType<Field>())
				{
					EnsureInternalDependencies((IType)item.Type.Entity);
				}
			}

			private void CreateRelatedTypes(IEnumerable<TypeReference> typerefs)
			{
				foreach (TypeReference typeref in typerefs)
				{
					IType type = _emitter.GetType(typeref);
					EnsureInternalDependencies(type);
				}
			}

			private void EnsureInternalDependencies(IType type)
			{
				AbstractInternalType abstractInternalType = type as AbstractInternalType;
				if (null != abstractInternalType)
				{
					CreateType(abstractInternalType.TypeDefinition);
				}
				else if (type.ConstructedInfo != null)
				{
					EnsureInternalDependencies(type.ConstructedInfo.GenericDefinition);
					IType[] genericArguments = type.ConstructedInfo.GenericArguments;
					foreach (IType type2 in genericArguments)
					{
						EnsureInternalDependencies(type2);
					}
				}
			}

			private static bool IsNestedType(TypeMember type)
			{
				switch (type.ParentNode.NodeType)
				{
				case NodeType.ClassDefinition:
				case NodeType.InterfaceDefinition:
					return true;
				default:
					return false;
				}
			}

			private void Trace(string format, params object[] args)
			{
				_emitter.Context.TraceVerbose(format, args);
			}
		}

		private delegate ParameterBuilder ParameterFactory(int index, ParameterAttributes attributes, string name);

		private sealed class SREResourceService : IResourceService
		{
			private AssemblyBuilder _asmBuilder;

			private ModuleBuilder _moduleBuilder;

			public SREResourceService(AssemblyBuilder asmBuilder, ModuleBuilder modBuilder)
			{
				_asmBuilder = asmBuilder;
				_moduleBuilder = modBuilder;
			}

			public bool EmbedFile(string resourceName, string fname)
			{
				_moduleBuilder.DefineManifestResource(resourceName, File.OpenRead(fname), ResourceAttributes.Public);
				return true;
			}

			public IResourceWriter DefineResource(string resourceName, string resourceDescription)
			{
				return _moduleBuilder.DefineResource(resourceName, resourceDescription);
			}
		}

		private const int _DBG_SYMBOLS_QUEUE_CAPACITY = 5;

		private const int InlineArrayItemCountLimit = 3;

		private static ConstructorInfo DebuggableAttribute_Constructor = Methods.ConstructorOf(() => new DebuggableAttribute(DebuggableAttribute.DebuggingModes.Default));

		private static ConstructorInfo RuntimeCompatibilityAttribute_Constructor = Methods.ConstructorOf(() => new RuntimeCompatibilityAttribute());

		private static ConstructorInfo SerializableAttribute_Constructor = Methods.ConstructorOf(() => new SerializableAttribute());

		private static PropertyInfo[] RuntimeCompatibilityAttribute_Property = new PropertyInfo[1] { Properties.Of((RuntimeCompatibilityAttribute a) => a.WrapNonExceptionThrows) };

		private static ConstructorInfo DuckTypedAttribute_Constructor = Methods.ConstructorOf(() => new DuckTypedAttribute());

		private static ConstructorInfo ParamArrayAttribute_Constructor = Methods.ConstructorOf(() => new ParamArrayAttribute());

		private static MethodInfo RuntimeServices_NormalizeArrayIndex = Methods.Of<Array, int, int>(RuntimeServices.NormalizeArrayIndex);

		private static MethodInfo RuntimeServices_ToBool_Object = Types.RuntimeServices.GetMethod("ToBool", new Type[1] { Types.Object });

		private static MethodInfo RuntimeServices_ToBool_Decimal = Types.RuntimeServices.GetMethod("ToBool", new Type[1] { Types.Decimal });

		private static MethodInfo Builtins_ArrayTypedConstructor = Types.Builtins.GetMethod("array", new Type[2]
		{
			Types.Type,
			Types.Int
		});

		private static MethodInfo Builtins_ArrayGenericConstructor = Types.Builtins.GetMethod("array", new Type[1] { Types.Int });

		private static MethodInfo Builtins_ArrayTypedCollectionConstructor = Types.Builtins.GetMethod("array", new Type[2]
		{
			Types.Type,
			Types.ICollection
		});

		private static MethodInfo Array_get_Length = Methods.GetterOf((Array a) => a.Length);

		private static MethodInfo Math_Pow = Methods.Of<double, double, double>(Math.Pow);

		private static ConstructorInfo List_EmptyConstructor = Types.List.GetConstructor(Type.EmptyTypes);

		private static ConstructorInfo List_ArrayBoolConstructor = Types.List.GetConstructor(new Type[2]
		{
			Types.ObjectArray,
			Types.Bool
		});

		private static ConstructorInfo Hash_Constructor = Types.Hash.GetConstructor(Type.EmptyTypes);

		private static ConstructorInfo Regex_Constructor = typeof(Regex).GetConstructor(new Type[1] { Types.String });

		private static ConstructorInfo Regex_Constructor_Options = typeof(Regex).GetConstructor(new Type[2]
		{
			Types.String,
			typeof(RegexOptions)
		});

		private static MethodInfo Hash_Add = Types.Hash.GetMethod("Add", new Type[2]
		{
			typeof(object),
			typeof(object)
		});

		private static ConstructorInfo TimeSpan_LongConstructor = Methods.ConstructorOf(() => new TimeSpan(0L));

		private static MethodInfo Type_GetTypeFromHandle = Methods.Of<RuntimeTypeHandle, Type>(Type.GetTypeFromHandle);

		private static MethodInfo String_IsNullOrEmpty = Methods.Of<string, bool>(string.IsNullOrEmpty);

		private static MethodInfo RuntimeHelpers_InitializeArray = Methods.Of<Array, RuntimeFieldHandle>(RuntimeHelpers.InitializeArray);

		private AssemblyBuilder _asmBuilder;

		private ModuleBuilder _moduleBuilder;

		private Hashtable _symbolDocWriters = new Hashtable();

		private ILGenerator _il;

		private Method _method;

		private int _returnStatements;

		private bool _hasLeaveWithStoredValue;

		private bool _returnImplicit;

		private Label _returnLabel;

		private Label _implicitLabel;

		private Label _leaveLabel;

		private IType _returnType;

		private int _tryBlock;

		private bool _checked = true;

		private bool _rawArrayIndexing = false;

		private bool _perModuleRawArrayIndexing = false;

		private Dictionary<IType, Type> _typeCache = new Dictionary<IType, Type>();

		private readonly Stack<IType> _types = new Stack<IType>();

		private readonly Stack<LoopInfo> _loopInfoStack = new Stack<LoopInfo>();

		private readonly AttributeCollection _assemblyAttributes = new AttributeCollection();

		private LoopInfo _currentLoopInfo;

		private IType _byAddress = null;

		private Dictionary<IType, LocalBuilder> _defaultValueHolders = new Dictionary<IType, LocalBuilder>();

		private LocalBuilder _currentLocal = null;

		private MethodInfo _Builtins_TypedMatrixConstructor;

		private static readonly string[] NoSymbols = new string[0];

		private Queue<LexicalInfo> _dbgSymbols = new Queue<LexicalInfo>(5);

		private Dictionary<byte[], FieldBuilder> _packedArrays = new Dictionary<byte[], FieldBuilder>(ValueTypeArrayEqualityComparer<byte>.Default);

		private MethodInfo _RuntimeServices_Coerce;

		private Hashtable _builders = new Hashtable();

		private static readonly Type IsVolatileType = typeof(IsVolatile);

		private SREResourceService _sreResourceService;

		private static MethodInfo stringFormat;

		private static Dictionary<Type, MethodInfo> _Nullable_HasValue = new Dictionary<Type, MethodInfo>();

		private MethodInfo Array_CreateInstance
		{
			get
			{
				if (_Builtins_TypedMatrixConstructor != null)
				{
					return _Builtins_TypedMatrixConstructor;
				}
				return _Builtins_TypedMatrixConstructor = Types.Array.GetMethod("CreateInstance", new Type[2]
				{
					Types.Type,
					typeof(int[])
				});
			}
		}

		private MethodInfo RuntimeServices_Coerce
		{
			get
			{
				if (_RuntimeServices_Coerce != null)
				{
					return _RuntimeServices_Coerce;
				}
				return _RuntimeServices_Coerce = Types.RuntimeServices.GetMethod("Coerce", new Type[2]
				{
					Types.Object,
					Types.Type
				});
			}
		}

		private static MethodInfo StringFormat
		{
			get
			{
				if (null != stringFormat)
				{
					return stringFormat;
				}
				stringFormat = Methods.Of<string, object, string>(string.Format);
				return stringFormat;
			}
		}

		private void EnterLoop(Label breakLabel, Label continueLabel)
		{
			_loopInfoStack.Push(_currentLoopInfo);
			_currentLoopInfo = new LoopInfo(breakLabel, continueLabel, _tryBlock);
		}

		private bool InTryInLoop()
		{
			return _tryBlock > _currentLoopInfo.TryBlockDepth;
		}

		private void LeaveLoop()
		{
			_currentLoopInfo = _loopInfoStack.Pop();
		}

		private void PushType(IType type)
		{
			_types.Push(type);
		}

		private void PushBool()
		{
			PushType(base.TypeSystemServices.BoolType);
		}

		private void PushVoid()
		{
			PushType(base.TypeSystemServices.VoidType);
		}

		private IType PopType()
		{
			return _types.Pop();
		}

		private IType PeekTypeOnStack()
		{
			return (_types.Count != 0) ? _types.Peek() : null;
		}

		public override void Run()
		{
			if (base.Errors.Count <= 0)
			{
				GatherAssemblyAttributes();
				SetUpAssembly();
				DefineTypes();
				DefineResources();
				DefineAssemblyAttributes();
				DefineEntryPoint();
				_moduleBuilder.CreateGlobalFunctions();
			}
		}

		private void GatherAssemblyAttributes()
		{
			foreach (Boo.Lang.Compiler.Ast.Module module in base.CompileUnit.Modules)
			{
				foreach (Boo.Lang.Compiler.Ast.Attribute assemblyAttribute in module.AssemblyAttributes)
				{
					_assemblyAttributes.Add(assemblyAttribute);
				}
			}
		}

		private void DefineTypes()
		{
			if (base.CompileUnit.Modules.Count == 0)
			{
				return;
			}
			List<TypeDefinition> list = CollectTypes();
			foreach (TypeDefinition item in list)
			{
				DefineType(item);
			}
			foreach (TypeDefinition item2 in list)
			{
				DefineGenericParameters(item2);
				DefineTypeMembers(item2);
			}
			foreach (Boo.Lang.Compiler.Ast.Module module in base.CompileUnit.Modules)
			{
				OnModule(module);
			}
			EmitAttributes();
			CreateTypes(list);
		}

		private void EmitAttributes(INodeWithAttributes node, CustomAttributeSetter setCustomAttribute)
		{
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in node.Attributes)
			{
				setCustomAttribute(GetCustomAttributeBuilder(attribute));
			}
		}

		private void EmitPropertyAttributes(Property node)
		{
			PropertyBuilder propertyBuilder = GetPropertyBuilder(node);
			EmitAttributes(node, propertyBuilder.SetCustomAttribute);
		}

		private void EmitParameterAttributes(ParameterDeclaration node)
		{
			ParameterBuilder @object = (ParameterBuilder)GetBuilder(node);
			EmitAttributes(node, @object.SetCustomAttribute);
		}

		private void EmitEventAttributes(Event node)
		{
			EventBuilder @object = (EventBuilder)GetBuilder(node);
			EmitAttributes(node, @object.SetCustomAttribute);
		}

		private void EmitConstructorAttributes(Constructor node)
		{
			ConstructorBuilder @object = (ConstructorBuilder)GetBuilder(node);
			EmitAttributes(node, @object.SetCustomAttribute);
		}

		private void EmitMethodAttributes(Method node)
		{
			MethodBuilder methodBuilder = GetMethodBuilder(node);
			EmitAttributes(node, methodBuilder.SetCustomAttribute);
		}

		private void EmitTypeAttributes(TypeDefinition node)
		{
			TypeBuilder typeBuilder = GetTypeBuilder(node);
			EmitAttributes(node, typeBuilder.SetCustomAttribute);
		}

		private void EmitFieldAttributes(TypeMember node)
		{
			FieldBuilder fieldBuilder = GetFieldBuilder(node);
			EmitAttributes(node, fieldBuilder.SetCustomAttribute);
		}

		private void EmitAttributes()
		{
			AttributeEmitVisitor visitor = new AttributeEmitVisitor(this);
			foreach (Boo.Lang.Compiler.Ast.Module module in base.CompileUnit.Modules)
			{
				module.Accept(visitor);
			}
		}

		private void CreateTypes(List<TypeDefinition> types)
		{
			new TypeCreator(this, types).Run();
		}

		private List<TypeDefinition> CollectTypes()
		{
			List<TypeDefinition> list = new List<TypeDefinition>();
			foreach (Boo.Lang.Compiler.Ast.Module module in base.CompileUnit.Modules)
			{
				CollectTypes(list, module.Members);
			}
			return list;
		}

		private void CollectTypes(List<TypeDefinition> types, TypeMemberCollection members)
		{
			foreach (TypeMember member in members)
			{
				switch (member.NodeType)
				{
				case NodeType.ClassDefinition:
				case NodeType.InterfaceDefinition:
				{
					TypeDefinition typeDefinition = (TypeDefinition)member;
					types.Add(typeDefinition);
					CollectTypes(types, typeDefinition.Members);
					break;
				}
				case NodeType.EnumDefinition:
					types.Add((TypeDefinition)member);
					break;
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			_asmBuilder = null;
			_moduleBuilder = null;
			_symbolDocWriters.Clear();
			_il = null;
			_returnStatements = 0;
			_hasLeaveWithStoredValue = false;
			_returnImplicit = false;
			_returnType = null;
			_tryBlock = 0;
			_checked = true;
			_rawArrayIndexing = false;
			_types.Clear();
			_typeCache.Clear();
			_builders.Clear();
			_assemblyAttributes.Clear();
			_defaultValueHolders.Clear();
			_packedArrays.Clear();
		}

		public override void OnAttribute(Boo.Lang.Compiler.Ast.Attribute node)
		{
		}

		public override void OnModule(Boo.Lang.Compiler.Ast.Module module)
		{
			_perModuleRawArrayIndexing = AstAnnotations.IsRawIndexing(module);
			_checked = AstAnnotations.IsChecked(module, base.Parameters.Checked);
			Visit(module.Members);
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			TypeBuilder typeBuilder = GetTypeBuilder(node);
			foreach (EnumMember member in node.Members)
			{
				FieldBuilder fieldBuilder = typeBuilder.DefineField(member.Name, typeBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
				fieldBuilder.SetConstant(InitializerValueOf(member, node));
				SetBuilder(member, fieldBuilder);
			}
		}

		private object InitializerValueOf(EnumMember enumMember, EnumDefinition enumType)
		{
			return Convert.ChangeType(((IntegerLiteralExpression)enumMember.Initializer).Value, GetEnumUnderlyingType(enumType));
		}

		public override void OnArrayTypeReference(ArrayTypeReference node)
		{
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			EmitTypeDefinition(node);
		}

		public override void OnField(Field node)
		{
			FieldBuilder fieldBuilder = GetFieldBuilder(node);
			if (fieldBuilder.IsLiteral)
			{
				fieldBuilder.SetConstant(GetInternalFieldStaticValue((InternalField)node.Entity));
			}
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			TypeBuilder typeBuilder = GetTypeBuilder(node);
			foreach (TypeReference baseType in node.BaseTypes)
			{
				typeBuilder.AddInterfaceImplementation(GetSystemType(baseType));
			}
		}

		public override void OnMacroStatement(MacroStatement node)
		{
			NotImplemented(node, "Unexpected macro: " + node.ToCodeString());
		}

		public override void OnCallableDefinition(CallableDefinition node)
		{
			NotImplemented(node, "Unexpected callable definition!");
		}

		private void EmitTypeDefinition(TypeDefinition node)
		{
			TypeBuilder typeBuilder = GetTypeBuilder(node);
			EmitBaseTypesAndAttributes(node, typeBuilder);
			Visit(node.Members);
		}

		public override void OnMethod(Method method)
		{
			if (!method.IsRuntime && !IsPInvoke(method))
			{
				MethodBuilder methodBuilder = GetMethodBuilder(method);
				DefineExplicitImplementationInfo(method);
				EmitMethod(method, methodBuilder.GetILGenerator());
			}
		}

		private void DefineExplicitImplementationInfo(Method method)
		{
			if (null != method.ExplicitInfo)
			{
				IMethod entity = (IMethod)method.ExplicitInfo.Entity;
				MethodInfo methodInfo = GetMethodInfo(entity);
				MethodInfo methodInfo2 = GetMethodInfo((IMethod)method.Entity);
				TypeBuilder typeBuilder = GetTypeBuilder(method.DeclaringType);
				typeBuilder.DefineMethodOverride(methodInfo2, methodInfo);
			}
		}

		private void EmitMethod(Method method, ILGenerator generator)
		{
			_il = generator;
			_method = method;
			DefineLabels(method);
			Visit(method.Locals);
			BeginMethodBody(GetEntity(method).ReturnType);
			Visit(method.Body);
			EndMethodBody(method);
		}

		private void BeginMethodBody(IType returnType)
		{
			_defaultValueHolders.Clear();
			_returnType = returnType;
			_returnStatements = 0;
			_returnImplicit = IsVoid(returnType);
			_hasLeaveWithStoredValue = false;
			_returnLabel = _il.DefineLabel();
			_leaveLabel = _il.DefineLabel();
			_implicitLabel = _il.DefineLabel();
		}

		private void EndMethodBody(Method method)
		{
			if (!_returnImplicit)
			{
				_returnImplicit = !AstUtil.AllCodePathsReturnOrRaise(method.Body);
			}
			bool flag = _returnImplicit && !IsVoid(_returnType);
			if (flag)
			{
				if (_returnStatements == -1)
				{
					_il.Emit(OpCodes.Br_S, _returnLabel);
				}
				_il.MarkLabel(_implicitLabel);
				EmitDefaultValue(_returnType);
				PopType();
			}
			if (_hasLeaveWithStoredValue)
			{
				if (flag || _returnStatements == -1)
				{
					_il.Emit(OpCodes.Br_S, _returnLabel);
				}
				_il.MarkLabel(_leaveLabel);
				_il.Emit(OpCodes.Ldloc, GetDefaultValueHolder(_returnType));
			}
			if (_returnImplicit || _returnStatements != 0)
			{
				_il.MarkLabel(_returnLabel);
				_il.Emit(OpCodes.Ret);
			}
		}

		private bool IsPInvoke(Method method)
		{
			return GetEntity(method).IsPInvoke;
		}

		public override void OnBlock(Block block)
		{
			bool @checked = _checked;
			_checked = AstAnnotations.IsChecked(block, @checked);
			bool rawArrayIndexing = _rawArrayIndexing;
			_rawArrayIndexing = _perModuleRawArrayIndexing || AstAnnotations.IsRawIndexing(block);
			Visit(block.Statements);
			_rawArrayIndexing = rawArrayIndexing;
			_checked = @checked;
		}

		private void DefineLabels(Method method)
		{
			InternalLabel[] array = LabelsOn(method);
			foreach (InternalLabel internalLabel in array)
			{
				internalLabel.Label = _il.DefineLabel();
			}
		}

		private InternalLabel[] LabelsOn(Method method)
		{
			return ((InternalMethod)method.Entity).Labels;
		}

		public override void OnConstructor(Constructor constructor)
		{
			if (!constructor.IsRuntime)
			{
				ConstructorBuilder constructorBuilder = GetConstructorBuilder(constructor);
				EmitMethod(constructor, constructorBuilder.GetILGenerator());
			}
		}

		public override void OnLocal(Local local)
		{
			InternalLocal internalLocal = GetInternalLocal(local);
			internalLocal.LocalBuilder = _il.DeclareLocal(GetSystemType(local), internalLocal.Type.IsPointer);
			if (base.Parameters.Debug)
			{
				internalLocal.LocalBuilder.SetLocalSymInfo(local.Name);
			}
		}

		public override void OnForStatement(ForStatement node)
		{
			NotImplemented("ForStatement");
		}

		public override void OnReturnStatement(ReturnStatement node)
		{
			EmitDebugInfo(node);
			OpCode opCode = ((_tryBlock > 0) ? OpCodes.Leave : OpCodes.Br);
			Label label = _returnLabel;
			Expression expression = node.Expression;
			if (expression != null)
			{
				_returnStatements++;
				LoadExpressionWithType(_returnType, expression);
				if (opCode == OpCodes.Leave)
				{
					LocalBuilder defaultValueHolder = GetDefaultValueHolder(_returnType);
					_il.Emit(OpCodes.Stloc, defaultValueHolder);
					label = _leaveLabel;
					_hasLeaveWithStoredValue = true;
				}
			}
			else if (_returnType != base.TypeSystemServices.VoidType)
			{
				_returnImplicit = true;
				label = _implicitLabel;
			}
			if (_method.Body.LastStatement != node)
			{
				_il.Emit(opCode, label);
			}
			else if (null != expression)
			{
				_returnStatements = -1;
			}
		}

		private void LoadExpressionWithType(IType expectedType, Expression expression)
		{
			Visit(expression);
			EmitCastIfNeeded(expectedType, PopType());
		}

		public override void OnRaiseStatement(RaiseStatement node)
		{
			EmitDebugInfo(node);
			if (node.Exception == null)
			{
				_il.Emit(OpCodes.Rethrow);
				return;
			}
			Visit(node.Exception);
			PopType();
			_il.Emit(OpCodes.Throw);
		}

		public override void OnTryStatement(TryStatement node)
		{
			_tryBlock++;
			Label label = _il.BeginExceptionBlock();
			if (node.FailureBlock != null && null != node.EnsureBlock)
			{
				_tryBlock++;
				_il.BeginExceptionBlock();
			}
			if (node.FailureBlock != null && node.ExceptionHandlers.Count > 0)
			{
				_tryBlock++;
				_il.BeginExceptionBlock();
			}
			Visit(node.ProtectedBlock);
			Visit(node.ExceptionHandlers);
			if (null != node.FailureBlock)
			{
				if (node.ExceptionHandlers.Count > 0)
				{
					_il.EndExceptionBlock();
					_tryBlock--;
				}
				_il.BeginFaultBlock();
				Visit(node.FailureBlock);
				if (null != node.EnsureBlock)
				{
					_il.EndExceptionBlock();
					_tryBlock--;
				}
			}
			if (null != node.EnsureBlock)
			{
				_il.BeginFinallyBlock();
				Visit(node.EnsureBlock);
			}
			_il.EndExceptionBlock();
			_tryBlock--;
		}

		public override void OnExceptionHandler(Boo.Lang.Compiler.Ast.ExceptionHandler node)
		{
			if ((node.Flags & ExceptionHandlerFlags.Filter) == ExceptionHandlerFlags.Filter)
			{
				_il.BeginExceptFilterBlock();
				Label label = _il.DefineLabel();
				if ((node.Flags & ExceptionHandlerFlags.Untyped) == 0)
				{
					Label label2 = _il.DefineLabel();
					_il.Emit(OpCodes.Isinst, GetSystemType(node.Declaration.Type));
					Dup();
					_il.Emit(OpCodes.Brtrue_S, label2);
					EmitStoreOrPopException(node);
					_il.Emit(OpCodes.Ldc_I4_0);
					_il.Emit(OpCodes.Br, label);
					_il.MarkLabel(label2);
				}
				else if ((node.Flags & ExceptionHandlerFlags.Anonymous) == 0)
				{
					_il.Emit(OpCodes.Isinst, GetSystemType(node.Declaration.Type));
				}
				EmitStoreOrPopException(node);
				node.FilterCondition.Accept(this);
				PopType();
				EmitToBoolIfNeeded(node.FilterCondition);
				_il.MarkLabel(label);
				_il.Emit(OpCodes.Ldc_I4_0);
				_il.Emit(OpCodes.Cgt_Un);
				_il.BeginCatchBlock(null);
			}
			else
			{
				_il.BeginCatchBlock(GetSystemType(node.Declaration.Type));
				EmitStoreOrPopException(node);
			}
			Visit(node.Block);
		}

		private void EmitStoreOrPopException(Boo.Lang.Compiler.Ast.ExceptionHandler node)
		{
			if ((node.Flags & ExceptionHandlerFlags.Anonymous) == 0)
			{
				_il.Emit(OpCodes.Stloc, GetLocalBuilder(node.Declaration));
			}
			else
			{
				_il.Emit(OpCodes.Pop);
			}
		}

		public override void OnUnpackStatement(UnpackStatement node)
		{
			NotImplemented("Unpacking");
		}

		public override void OnExpressionStatement(ExpressionStatement node)
		{
			EmitDebugInfo(node);
			base.OnExpressionStatement(node);
			DiscardValueOnStack();
		}

		private void DiscardValueOnStack()
		{
			if (!IsVoid(PopType()))
			{
				_il.Emit(OpCodes.Pop);
			}
		}

		private bool IsVoid(IType type)
		{
			return type == base.TypeSystemServices.VoidType;
		}

		public override void OnUnlessStatement(UnlessStatement node)
		{
			Label label = _il.DefineLabel();
			EmitDebugInfo(node);
			EmitBranchTrue(node.Condition, label);
			node.Block.Accept(this);
			_il.MarkLabel(label);
		}

		private void OnSwitch(MethodInvocationExpression node)
		{
			ExpressionCollection arguments = node.Arguments;
			LoadExpressionWithType(base.TypeSystemServices.IntType, arguments[0]);
			_il.Emit(OpCodes.Switch, (from e in arguments.Skip(1)
				select LabelFor(e)).ToArray());
			PushVoid();
		}

		private static Label LabelFor(Expression expression)
		{
			return ((InternalLabel)expression.Entity).Label;
		}

		public override void OnGotoStatement(GotoStatement node)
		{
			EmitDebugInfo(node);
			InternalLabel internalLabel = (InternalLabel)GetEntity(node.Label);
			int tryBlockDepth = AstAnnotations.GetTryBlockDepth(node);
			int tryBlockDepth2 = AstAnnotations.GetTryBlockDepth(internalLabel.LabelStatement);
			if (tryBlockDepth2 == tryBlockDepth)
			{
				_il.Emit(OpCodes.Br, internalLabel.Label);
			}
			else
			{
				_il.Emit(OpCodes.Leave, internalLabel.Label);
			}
		}

		public override void OnLabelStatement(LabelStatement node)
		{
			EmitDebugInfo(node);
			_il.MarkLabel(((InternalLabel)node.Entity).Label);
		}

		public override void OnConditionalExpression(ConditionalExpression node)
		{
			IType expressionType = GetExpressionType(node);
			Label label = _il.DefineLabel();
			EmitBranchFalse(node.Condition, label);
			LoadExpressionWithType(expressionType, node.TrueValue);
			Label label2 = _il.DefineLabel();
			_il.Emit(OpCodes.Br, label2);
			_il.MarkLabel(label);
			label = label2;
			LoadExpressionWithType(expressionType, node.FalseValue);
			_il.MarkLabel(label);
			PushType(expressionType);
		}

		public override void OnIfStatement(IfStatement node)
		{
			Label label = _il.DefineLabel();
			EmitDebugInfo(node);
			EmitBranchFalse(node.Condition, label);
			node.TrueBlock.Accept(this);
			if (null != node.FalseBlock)
			{
				Label label2 = _il.DefineLabel();
				if (!node.TrueBlock.EndsWith<ReturnStatement>() && !node.TrueBlock.EndsWith<RaiseStatement>())
				{
					_il.Emit(OpCodes.Br, label2);
				}
				_il.MarkLabel(label);
				label = label2;
				node.FalseBlock.Accept(this);
			}
			_il.MarkLabel(label);
		}

		private void EmitBranchTrue(Expression expression, Label label)
		{
			EmitBranch(branchOnTrue: true, expression, label);
		}

		private void EmitBranchFalse(Expression expression, Label label)
		{
			EmitBranch(branchOnTrue: false, expression, label);
		}

		private void EmitBranch(bool branchOnTrue, BinaryExpression expression, Label label)
		{
			switch (expression.Operator)
			{
			case BinaryOperatorType.TypeTest:
				EmitTypeTest(expression);
				_il.Emit(branchOnTrue ? OpCodes.Brtrue : OpCodes.Brfalse, label);
				break;
			case BinaryOperatorType.Or:
			{
				if (branchOnTrue)
				{
					EmitBranch(branchOnTrue: true, expression.Left, label);
					EmitBranch(branchOnTrue: true, expression.Right, label);
					break;
				}
				Label label2 = _il.DefineLabel();
				EmitBranch(branchOnTrue: true, expression.Left, label2);
				EmitBranch(branchOnTrue: false, expression.Right, label);
				_il.MarkLabel(label2);
				break;
			}
			case BinaryOperatorType.And:
				if (branchOnTrue)
				{
					Label label2 = _il.DefineLabel();
					EmitBranch(branchOnTrue: false, expression.Left, label2);
					EmitBranch(branchOnTrue: true, expression.Right, label);
					_il.MarkLabel(label2);
				}
				else
				{
					EmitBranch(branchOnTrue: false, expression.Left, label);
					EmitBranch(branchOnTrue: false, expression.Right, label);
				}
				break;
			case BinaryOperatorType.Equality:
				if (IsZeroEquivalent(expression.Left))
				{
					EmitBranch(!branchOnTrue, expression.Right, label);
					break;
				}
				if (IsZeroEquivalent(expression.Right))
				{
					EmitBranch(!branchOnTrue, expression.Left, label);
					break;
				}
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Beq : OpCodes.Bne_Un, label);
				break;
			case BinaryOperatorType.Inequality:
				if (IsZeroEquivalent(expression.Left))
				{
					EmitBranch(branchOnTrue, expression.Right, label);
					break;
				}
				if (IsZeroEquivalent(expression.Right))
				{
					EmitBranch(branchOnTrue, expression.Left, label);
					break;
				}
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Bne_Un : OpCodes.Beq, label);
				break;
			case BinaryOperatorType.ReferenceEquality:
				if (IsNull(expression.Left))
				{
					EmitRawBranch(!branchOnTrue, expression.Right, label);
					break;
				}
				if (IsNull(expression.Right))
				{
					EmitRawBranch(!branchOnTrue, expression.Left, label);
					break;
				}
				Visit(expression.Left);
				PopType();
				Visit(expression.Right);
				PopType();
				_il.Emit(branchOnTrue ? OpCodes.Beq : OpCodes.Bne_Un, label);
				break;
			case BinaryOperatorType.ReferenceInequality:
				if (IsNull(expression.Left))
				{
					EmitRawBranch(branchOnTrue, expression.Right, label);
					break;
				}
				if (IsNull(expression.Right))
				{
					EmitRawBranch(branchOnTrue, expression.Left, label);
					break;
				}
				Visit(expression.Left);
				PopType();
				Visit(expression.Right);
				PopType();
				_il.Emit(branchOnTrue ? OpCodes.Bne_Un : OpCodes.Beq, label);
				break;
			case BinaryOperatorType.GreaterThan:
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Bgt : OpCodes.Ble, label);
				break;
			case BinaryOperatorType.GreaterThanOrEqual:
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Bge : OpCodes.Blt, label);
				break;
			case BinaryOperatorType.LessThan:
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Blt : OpCodes.Bge, label);
				break;
			case BinaryOperatorType.LessThanOrEqual:
				LoadCmpOperands(expression);
				_il.Emit(branchOnTrue ? OpCodes.Ble : OpCodes.Bgt, label);
				break;
			default:
				EmitDefaultBranch(branchOnTrue, expression, label);
				break;
			}
		}

		private void EmitBranch(bool branchOnTrue, UnaryExpression expression, Label label)
		{
			if (UnaryOperatorType.LogicalNot == expression.Operator)
			{
				EmitBranch(!branchOnTrue, expression.Operand, label);
			}
			else
			{
				EmitDefaultBranch(branchOnTrue, expression, label);
			}
		}

		private void EmitBranch(bool branchOnTrue, Expression expression, Label label)
		{
			switch (expression.NodeType)
			{
			case NodeType.BinaryExpression:
				EmitBranch(branchOnTrue, (BinaryExpression)expression, label);
				break;
			case NodeType.UnaryExpression:
				EmitBranch(branchOnTrue, (UnaryExpression)expression, label);
				break;
			default:
				EmitDefaultBranch(branchOnTrue, expression, label);
				break;
			}
		}

		private void EmitRawBranch(bool branch, Expression condition, Label label)
		{
			condition.Accept(this);
			PopType();
			_il.Emit(branch ? OpCodes.Brtrue : OpCodes.Brfalse, label);
		}

		private void EmitDefaultBranch(bool branch, Expression condition, Label label)
		{
			if (branch && IsOneEquivalent(condition))
			{
				_il.Emit(OpCodes.Br, label);
				return;
			}
			if (!branch && IsZeroEquivalent(condition))
			{
				_il.Emit(OpCodes.Br, label);
				return;
			}
			condition.Accept(this);
			IType type = PopType();
			if (base.TypeSystemServices.IsFloatingPointNumber(type))
			{
				EmitDefaultValue(type);
				_il.Emit(branch ? OpCodes.Bne_Un : OpCodes.Beq, label);
			}
			else
			{
				EmitToBoolIfNeeded(condition);
				_il.Emit(branch ? OpCodes.Brtrue : OpCodes.Brfalse, label);
			}
		}

		private static bool IsZeroEquivalent(Expression expression)
		{
			return IsNull(expression) || IsZero(expression) || IsFalse(expression);
		}

		private static bool IsOneEquivalent(Expression expression)
		{
			return IsBooleanLiteral(expression, value: true) || IsNumberLiteral(expression, 1);
		}

		private static bool IsNull(Expression expression)
		{
			return NodeType.NullLiteralExpression == expression.NodeType;
		}

		private static bool IsFalse(Expression expression)
		{
			return IsBooleanLiteral(expression, value: false);
		}

		private static bool IsBooleanLiteral(Expression expression, bool value)
		{
			return NodeType.BoolLiteralExpression == expression.NodeType && value == ((BoolLiteralExpression)expression).Value;
		}

		private static bool IsZero(Expression expression)
		{
			return IsNumberLiteral(expression, 0);
		}

		private static bool IsNumberLiteral(Expression expression, int value)
		{
			return (NodeType.IntegerLiteralExpression == expression.NodeType && value == ((IntegerLiteralExpression)expression).Value) || (NodeType.DoubleLiteralExpression == expression.NodeType && (double)value == ((DoubleLiteralExpression)expression).Value);
		}

		public override void OnBreakStatement(BreakStatement node)
		{
			EmitGoTo(_currentLoopInfo.BreakLabel, node);
		}

		private void EmitGoTo(Label label, Node debugInfo)
		{
			EmitDebugInfo(debugInfo);
			_il.Emit(InTryInLoop() ? OpCodes.Leave : OpCodes.Br, label);
		}

		public override void OnContinueStatement(ContinueStatement node)
		{
			EmitGoTo(_currentLoopInfo.ContinueLabel, node);
		}

		public override void OnWhileStatement(WhileStatement node)
		{
			Label label = _il.DefineLabel();
			Label label2 = _il.DefineLabel();
			Label label3 = _il.DefineLabel();
			_il.Emit(OpCodes.Br, label3);
			_il.MarkLabel(label2);
			EnterLoop(label, label3);
			node.Block.Accept(this);
			LeaveLoop();
			_il.MarkLabel(label3);
			EmitDebugInfo(node);
			EmitBranchTrue(node.Condition, label2);
			Visit(node.OrBlock);
			Visit(node.ThenBlock);
			_il.MarkLabel(label);
		}

		private void EmitIntNot()
		{
			_il.Emit(OpCodes.Ldc_I4_0);
			_il.Emit(OpCodes.Ceq);
		}

		private void EmitGenericNot()
		{
			Label label = _il.DefineLabel();
			Label label2 = _il.DefineLabel();
			_il.Emit(OpCodes.Brfalse_S, label2);
			_il.Emit(OpCodes.Ldc_I4_0);
			_il.Emit(OpCodes.Br_S, label);
			_il.MarkLabel(label2);
			_il.Emit(OpCodes.Ldc_I4_1);
			_il.MarkLabel(label);
		}

		public override void OnUnaryExpression(UnaryExpression node)
		{
			switch (node.Operator)
			{
			case UnaryOperatorType.LogicalNot:
				EmitLogicalNot(node);
				break;
			case UnaryOperatorType.UnaryNegation:
				EmitUnaryNegation(node);
				break;
			case UnaryOperatorType.OnesComplement:
				EmitOnesComplement(node);
				break;
			case UnaryOperatorType.AddressOf:
				EmitAddressOf(node);
				break;
			case UnaryOperatorType.Indirection:
				EmitIndirection(node);
				break;
			default:
				NotImplemented(node, "unary operator not supported");
				break;
			}
		}

		private bool IsByAddress(IType type)
		{
			return _byAddress == type;
		}

		private void EmitDefaultValue(IType type)
		{
			bool flag = GenericsServices.IsGenericParameter(type);
			if (!type.IsValueType && !flag)
			{
				_il.Emit(OpCodes.Ldnull);
			}
			else if (type == base.TypeSystemServices.BoolType)
			{
				_il.Emit(OpCodes.Ldc_I4_0);
			}
			else if (base.TypeSystemServices.IsFloatingPointNumber(type))
			{
				EmitLoadLiteral(type, 0.0);
			}
			else if (base.TypeSystemServices.IsPrimitiveNumber(type) || type == base.TypeSystemServices.CharType)
			{
				EmitLoadLiteral(type, 0L);
			}
			else if (flag && TypeSystemServices.IsReferenceType(type))
			{
				_il.Emit(OpCodes.Ldnull);
				_il.Emit(OpCodes.Unbox_Any, GetSystemType(type));
			}
			else
			{
				_il.Emit(OpCodes.Ldloc, GetDefaultValueHolder(type));
			}
			PushType(type);
		}

		private LocalBuilder GetDefaultValueHolder(IType type)
		{
			if (_defaultValueHolders.TryGetValue(type, out var value))
			{
				return value;
			}
			value = _il.DeclareLocal(GetSystemType(type));
			_defaultValueHolders.Add(type, value);
			return value;
		}

		private void EmitOnesComplement(UnaryExpression node)
		{
			node.Operand.Accept(this);
			_il.Emit(OpCodes.Not);
		}

		private void EmitLogicalNot(UnaryExpression node)
		{
			Expression operand = node.Operand;
			operand.Accept(this);
			IType type = PopType();
			bool notContext = true;
			if (IsBoolOrInt(type))
			{
				EmitIntNot();
			}
			else if (EmitToBoolIfNeeded(operand, ref notContext))
			{
				if (!notContext)
				{
					EmitIntNot();
				}
			}
			else
			{
				EmitGenericNot();
			}
			PushBool();
		}

		private void EmitUnaryNegation(UnaryExpression node)
		{
			IType expressionType = GetExpressionType(node.Operand);
			if (IsCheckedIntegerOperand(expressionType))
			{
				_il.Emit(OpCodes.Ldc_I4_0);
				if (IsLong(expressionType) || expressionType == base.TypeSystemServices.ULongType)
				{
					_il.Emit(OpCodes.Conv_I8);
				}
				node.Operand.Accept(this);
				_il.Emit(base.TypeSystemServices.IsSignedNumber(expressionType) ? OpCodes.Sub_Ovf : OpCodes.Sub_Ovf_Un);
				if (!IsLong(expressionType) && expressionType != base.TypeSystemServices.ULongType)
				{
					EmitCastIfNeeded(expressionType, base.TypeSystemServices.IntType);
				}
			}
			else
			{
				node.Operand.Accept(this);
				_il.Emit(OpCodes.Neg);
			}
		}

		private bool IsCheckedIntegerOperand(IType operandType)
		{
			return _checked && IsInteger(operandType);
		}

		private void EmitAddressOf(UnaryExpression node)
		{
			_byAddress = GetExpressionType(node.Operand);
			node.Operand.Accept(this);
			PushType(PopType().MakePointerType());
			_byAddress = null;
		}

		private void EmitIndirection(UnaryExpression node)
		{
			node.Operand.Accept(this);
			if (node.Operand.NodeType != NodeType.ReferenceExpression && node.ParentNode.NodeType != NodeType.MemberReferenceExpression)
			{
				IType elementType = PeekTypeOnStack().ElementType;
				OpCode loadRefParamCode = GetLoadRefParamCode(elementType);
				if (loadRefParamCode == OpCodes.Ldobj)
				{
					_il.Emit(loadRefParamCode, GetSystemType(elementType));
				}
				else
				{
					_il.Emit(loadRefParamCode);
				}
				PopType();
				PushType(elementType);
			}
		}

		private static bool ShouldLeaveValueOnStack(Expression node)
		{
			return node.ParentNode.NodeType != NodeType.ExpressionStatement;
		}

		private void OnReferenceComparison(BinaryExpression node)
		{
			node.Left.Accept(this);
			PopType();
			node.Right.Accept(this);
			PopType();
			_il.Emit(OpCodes.Ceq);
			if (BinaryOperatorType.ReferenceInequality == node.Operator)
			{
				EmitIntNot();
			}
			PushBool();
		}

		private void OnAssignmentToSlice(BinaryExpression node)
		{
			SlicingExpression slicingExpression = (SlicingExpression)node.Left;
			Visit(slicingExpression.Target);
			IArrayType arrayType = (IArrayType)PopType();
			if (arrayType.Rank == 1)
			{
				EmitAssignmentToSingleDimensionalArrayElement(arrayType, slicingExpression, node);
			}
			else
			{
				EmitAssignmentToMultiDimensionalArrayElement(arrayType, slicingExpression, node);
			}
		}

		private void EmitAssignmentToMultiDimensionalArrayElement(IArrayType arrayType, SlicingExpression slice, BinaryExpression node)
		{
			IType elementType = arrayType.ElementType;
			LoadArrayIndices(slice);
			LocalBuilder temp = LoadAssignmentOperand(elementType, node);
			CallArrayMethod(arrayType, "Set", typeof(void), ParameterTypesForArraySet(arrayType));
			FlushAssignmentOperand(elementType, temp);
		}

		private void EmitAssignmentToSingleDimensionalArrayElement(IArrayType arrayType, SlicingExpression slice, BinaryExpression node)
		{
			IType elementType = arrayType.ElementType;
			Slice slice2 = slice.Indices[0];
			EmitNormalizedArrayIndex(slice, slice2.Begin);
			OpCode storeEntityOpCode = GetStoreEntityOpCode(elementType);
			bool flag = IsStobj(storeEntityOpCode);
			if (flag)
			{
				_il.Emit(OpCodes.Ldelema, GetSystemType(elementType));
			}
			LocalBuilder temp = LoadAssignmentOperand(elementType, node);
			if (flag)
			{
				_il.Emit(storeEntityOpCode, GetSystemType(elementType));
			}
			else
			{
				_il.Emit(storeEntityOpCode);
			}
			FlushAssignmentOperand(elementType, temp);
		}

		private void FlushAssignmentOperand(IType elementType, LocalBuilder temp)
		{
			if (temp != null)
			{
				LoadLocal(temp, elementType);
			}
			else
			{
				PushVoid();
			}
		}

		private LocalBuilder LoadAssignmentOperand(IType elementType, BinaryExpression node)
		{
			LoadExpressionWithType(elementType, node.Right);
			bool flag = ShouldLeaveValueOnStack(node);
			LocalBuilder result = null;
			if (flag)
			{
				Dup();
				result = StoreTempLocal(elementType);
			}
			return result;
		}

		private void LoadLocal(LocalBuilder local, IType localType)
		{
			_il.Emit(OpCodes.Ldloc, local);
			PushType(localType);
			_currentLocal = local;
		}

		private void LoadLocal(InternalLocal local)
		{
			LoadLocal(local, byAddress: false);
		}

		private void LoadLocal(InternalLocal local, bool byAddress)
		{
			_il.Emit(IsByAddress(local.Type) ? OpCodes.Ldloca : OpCodes.Ldloc, local.LocalBuilder);
			PushType(local.Type);
			_currentLocal = local.LocalBuilder;
		}

		private void LoadIndirectLocal(InternalLocal local)
		{
			LoadLocal(local);
			IType elementType = local.Type.ElementType;
			PopType();
			PushType(elementType);
			OpCode loadRefParamCode = GetLoadRefParamCode(elementType);
			if (loadRefParamCode == OpCodes.Ldobj)
			{
				_il.Emit(loadRefParamCode, GetSystemType(elementType));
			}
			else
			{
				_il.Emit(loadRefParamCode);
			}
		}

		private LocalBuilder StoreTempLocal(IType elementType)
		{
			LocalBuilder localBuilder = _il.DeclareLocal(GetSystemType(elementType));
			_il.Emit(OpCodes.Stloc, localBuilder);
			return localBuilder;
		}

		private void OnAssignment(BinaryExpression node)
		{
			if (NodeType.SlicingExpression == node.Left.NodeType)
			{
				OnAssignmentToSlice(node);
				return;
			}
			bool flag = ShouldLeaveValueOnStack(node);
			IEntity entity = TypeSystemServices.GetEntity(node.Left);
			switch (entity.EntityType)
			{
			case EntityType.Local:
				SetLocal(node, (InternalLocal)entity, flag);
				break;
			case EntityType.Parameter:
			{
				InternalParameter internalParameter = (InternalParameter)entity;
				if (internalParameter.Parameter.IsByRef)
				{
					SetByRefParam(internalParameter, node.Right, flag);
					break;
				}
				LoadExpressionWithType(internalParameter.Type, node.Right);
				if (flag)
				{
					Dup();
					PushType(internalParameter.Type);
				}
				_il.Emit(OpCodes.Starg, internalParameter.Index);
				break;
			}
			case EntityType.Field:
			{
				IField field = (IField)entity;
				SetField(node, field, node.Left, node.Right, flag);
				break;
			}
			case EntityType.Property:
				SetProperty((IProperty)entity, node.Left, node.Right, flag);
				break;
			case EntityType.Event:
			{
				InternalEvent internalEvent = (InternalEvent)entity;
				OpCode opcode = (internalEvent.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld);
				_il.Emit(OpCodes.Ldnull);
				_il.Emit(opcode, GetFieldBuilder(internalEvent.BackingField.Field));
				break;
			}
			default:
				NotImplemented(node, entity.ToString());
				break;
			}
			if (!flag)
			{
				PushVoid();
			}
		}

		private void SetByRefParam(InternalParameter param, Expression right, bool leaveValueOnStack)
		{
			LocalBuilder localBuilder = null;
			IType type = null;
			if (leaveValueOnStack)
			{
				Visit(right);
				type = PopType();
				localBuilder = StoreTempLocal(type);
			}
			LoadParam(param);
			if (localBuilder != null)
			{
				LoadLocal(localBuilder, type);
				PopType();
			}
			else
			{
				LoadExpressionWithType(param.Type, right);
			}
			OpCode storeRefParamCode = GetStoreRefParamCode(param.Type);
			if (IsStobj(storeRefParamCode))
			{
				_il.Emit(storeRefParamCode, GetSystemType(param.Type));
			}
			else
			{
				_il.Emit(storeRefParamCode);
			}
			if (null != localBuilder)
			{
				LoadLocal(localBuilder, type);
			}
		}

		private void EmitTypeTest(BinaryExpression node)
		{
			Visit(node.Left);
			IType actualType = PopType();
			EmitBoxIfNeeded(base.TypeSystemServices.ObjectType, actualType);
			Type cls = ((NodeType.TypeofExpression == node.Right.NodeType) ? GetSystemType(((TypeofExpression)node.Right).Type) : GetSystemType(node.Right));
			_il.Emit(OpCodes.Isinst, cls);
		}

		private void OnTypeTest(BinaryExpression node)
		{
			EmitTypeTest(node);
			_il.Emit(OpCodes.Ldnull);
			_il.Emit(OpCodes.Cgt_Un);
			PushBool();
		}

		private void LoadCmpOperands(BinaryExpression node)
		{
			IType expressionType = node.Left.ExpressionType;
			IType expressionType2 = node.Right.ExpressionType;
			if (expressionType != expressionType2)
			{
				IType promotedNumberType = base.TypeSystemServices.GetPromotedNumberType(expressionType, expressionType2);
				LoadExpressionWithType(promotedNumberType, node.Left);
				LoadExpressionWithType(promotedNumberType, node.Right);
			}
			else
			{
				Visit(node.Left);
				PopType();
				Visit(node.Right);
				PopType();
			}
		}

		private void OnEquality(BinaryExpression node)
		{
			LoadCmpOperands(node);
			_il.Emit(OpCodes.Ceq);
			PushBool();
		}

		private void OnInequality(BinaryExpression node)
		{
			LoadCmpOperands(node);
			_il.Emit(OpCodes.Ceq);
			EmitIntNot();
			PushBool();
		}

		private void OnGreaterThan(BinaryExpression node)
		{
			LoadCmpOperands(node);
			_il.Emit(OpCodes.Cgt);
			PushBool();
		}

		private void OnGreaterThanOrEqual(BinaryExpression node)
		{
			OnLessThan(node);
			EmitIntNot();
		}

		private void OnLessThan(BinaryExpression node)
		{
			LoadCmpOperands(node);
			_il.Emit(OpCodes.Clt);
			PushBool();
		}

		private void OnLessThanOrEqual(BinaryExpression node)
		{
			OnGreaterThan(node);
			EmitIntNot();
		}

		private void OnExponentiation(BinaryExpression node)
		{
			IType doubleType = base.TypeSystemServices.DoubleType;
			LoadOperandsWithType(doubleType, node);
			Call(Math_Pow);
			PushType(doubleType);
		}

		private void LoadOperandsWithType(IType type, BinaryExpression node)
		{
			LoadExpressionWithType(type, node.Left);
			LoadExpressionWithType(type, node.Right);
		}

		private void OnArithmeticOperator(BinaryExpression node)
		{
			IType expressionType = node.ExpressionType;
			LoadOperandsWithType(expressionType, node);
			_il.Emit(GetArithmeticOpCode(expressionType, node.Operator));
			PushType(expressionType);
		}

		private bool EmitToBoolIfNeeded(Expression expression)
		{
			bool notContext = false;
			return EmitToBoolIfNeeded(expression, ref notContext);
		}

		private bool EmitToBoolIfNeeded(Expression expression, ref bool notContext)
		{
			bool flag = notContext;
			notContext = false;
			IType expressionType = GetExpressionType(expression);
			if (base.TypeSystemServices.ObjectType == expressionType || base.TypeSystemServices.DuckType == expressionType)
			{
				Call(RuntimeServices_ToBool_Object);
				return true;
			}
			if (TypeSystemServices.IsNullable(expressionType))
			{
				_il.Emit(OpCodes.Ldloca, _currentLocal);
				Type systemType = GetSystemType(base.TypeSystemServices.GetNullableUnderlyingType(expressionType));
				Call(GetNullableHasValue(systemType));
				LocalBuilder local = StoreTempLocal(base.TypeSystemServices.BoolType);
				_il.Emit(OpCodes.Pop);
				_il.Emit(OpCodes.Ldloc, local);
				return true;
			}
			if (base.TypeSystemServices.StringType == expressionType)
			{
				Call(String_IsNullOrEmpty);
				if (!flag)
				{
					EmitIntNot();
				}
				else
				{
					notContext = true;
				}
				return true;
			}
			if (IsInteger(expressionType))
			{
				if (IsLong(expressionType) || base.TypeSystemServices.ULongType == expressionType)
				{
					_il.Emit(OpCodes.Conv_I4);
				}
				return true;
			}
			if (base.TypeSystemServices.SingleType == expressionType)
			{
				EmitDefaultValue(base.TypeSystemServices.SingleType);
				_il.Emit(OpCodes.Ceq);
				if (!flag)
				{
					EmitIntNot();
				}
				else
				{
					notContext = true;
				}
				return true;
			}
			if (base.TypeSystemServices.DoubleType == expressionType)
			{
				EmitDefaultValue(base.TypeSystemServices.DoubleType);
				_il.Emit(OpCodes.Ceq);
				if (!flag)
				{
					EmitIntNot();
				}
				else
				{
					notContext = true;
				}
				return true;
			}
			if (base.TypeSystemServices.DecimalType == expressionType)
			{
				Call(RuntimeServices_ToBool_Decimal);
				return true;
			}
			if (!expressionType.IsValueType)
			{
				if (expression.GetAncestor<BinaryExpression>() == null && null != expression.GetAncestor<IfStatement>())
				{
					return true;
				}
				_il.Emit(OpCodes.Ldnull);
				if (!flag)
				{
					_il.Emit(OpCodes.Cgt_Un);
				}
				else
				{
					_il.Emit(OpCodes.Ceq);
					notContext = true;
				}
				return true;
			}
			return false;
		}

		private void EmitAnd(BinaryExpression node)
		{
			EmitLogicalOperator(node, OpCodes.Brtrue, OpCodes.Brfalse);
		}

		private void EmitOr(BinaryExpression node)
		{
			EmitLogicalOperator(node, OpCodes.Brfalse, OpCodes.Brtrue);
		}

		private void EmitLogicalOperator(BinaryExpression node, OpCode brForValueType, OpCode brForRefType)
		{
			IType expressionType = GetExpressionType(node);
			Visit(node.Left);
			IType type = PopType();
			if (type != null && type.IsValueType && !expressionType.IsValueType)
			{
				Label label = _il.DefineLabel();
				Label label2 = _il.DefineLabel();
				Dup();
				EmitToBoolIfNeeded(node.Left);
				_il.Emit(brForValueType, label);
				EmitCastIfNeeded(expressionType, type);
				_il.Emit(OpCodes.Br_S, label2);
				_il.MarkLabel(label);
				_il.Emit(OpCodes.Pop);
				LoadExpressionWithType(expressionType, node.Right);
				_il.MarkLabel(label2);
			}
			else
			{
				Label label2 = _il.DefineLabel();
				EmitCastIfNeeded(expressionType, type);
				Dup();
				EmitToBoolIfNeeded(node.Left);
				_il.Emit(brForRefType, label2);
				_il.Emit(OpCodes.Pop);
				LoadExpressionWithType(expressionType, node.Right);
				_il.MarkLabel(label2);
			}
			PushType(expressionType);
		}

		private IType GetExpectedTypeForBitwiseRightOperand(BinaryExpression node)
		{
			switch (node.Operator)
			{
			case BinaryOperatorType.ShiftLeft:
			case BinaryOperatorType.ShiftRight:
				return base.TypeSystemServices.IntType;
			default:
				return GetExpressionType(node);
			}
		}

		private void EmitBitwiseOperator(BinaryExpression node)
		{
			IType expressionType = node.ExpressionType;
			LoadExpressionWithType(expressionType, node.Left);
			LoadExpressionWithType(GetExpectedTypeForBitwiseRightOperand(node), node.Right);
			switch (node.Operator)
			{
			case BinaryOperatorType.BitwiseOr:
				_il.Emit(OpCodes.Or);
				break;
			case BinaryOperatorType.BitwiseAnd:
				_il.Emit(OpCodes.And);
				break;
			case BinaryOperatorType.ExclusiveOr:
				_il.Emit(OpCodes.Xor);
				break;
			case BinaryOperatorType.ShiftLeft:
				_il.Emit(OpCodes.Shl);
				break;
			case BinaryOperatorType.ShiftRight:
				_il.Emit(base.TypeSystemServices.IsSignedNumber(expressionType) ? OpCodes.Shr : OpCodes.Shr_Un);
				break;
			}
			PushType(expressionType);
		}

		public override void OnBinaryExpression(BinaryExpression node)
		{
			switch (node.Operator)
			{
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
			case BinaryOperatorType.ShiftLeft:
			case BinaryOperatorType.ShiftRight:
				EmitBitwiseOperator(node);
				break;
			case BinaryOperatorType.Or:
				EmitOr(node);
				break;
			case BinaryOperatorType.And:
				EmitAnd(node);
				break;
			case BinaryOperatorType.Addition:
			case BinaryOperatorType.Subtraction:
			case BinaryOperatorType.Multiply:
			case BinaryOperatorType.Division:
			case BinaryOperatorType.Modulus:
				OnArithmeticOperator(node);
				break;
			case BinaryOperatorType.Exponentiation:
				OnExponentiation(node);
				break;
			case BinaryOperatorType.Assign:
				OnAssignment(node);
				break;
			case BinaryOperatorType.Equality:
				OnEquality(node);
				break;
			case BinaryOperatorType.Inequality:
				OnInequality(node);
				break;
			case BinaryOperatorType.GreaterThan:
				OnGreaterThan(node);
				break;
			case BinaryOperatorType.LessThan:
				OnLessThan(node);
				break;
			case BinaryOperatorType.GreaterThanOrEqual:
				OnGreaterThanOrEqual(node);
				break;
			case BinaryOperatorType.LessThanOrEqual:
				OnLessThanOrEqual(node);
				break;
			case BinaryOperatorType.ReferenceInequality:
				OnReferenceComparison(node);
				break;
			case BinaryOperatorType.ReferenceEquality:
				OnReferenceComparison(node);
				break;
			case BinaryOperatorType.TypeTest:
				OnTypeTest(node);
				break;
			default:
				OperatorNotImplemented(node);
				break;
			}
		}

		private void OperatorNotImplemented(BinaryExpression node)
		{
			NotImplemented(node, node.Operator.ToString());
		}

		public override void OnTypeofExpression(TypeofExpression node)
		{
			EmitGetTypeFromHandle(GetSystemType(node.Type));
		}

		public override void OnCastExpression(CastExpression node)
		{
			IType type = GetType(node.Type);
			LoadExpressionWithType(type, node.Target);
			PushType(type);
		}

		public override void OnTryCastExpression(TryCastExpression node)
		{
			Type systemType = GetSystemType(node.Type);
			node.Target.Accept(this);
			PopType();
			Isinst(systemType);
			PushType(node.ExpressionType);
		}

		private void Isinst(Type type)
		{
			_il.Emit(OpCodes.Isinst, type);
		}

		private void InvokeMethod(IMethod method, MethodInvocationExpression node)
		{
			MethodInfo methodInfo = GetMethodInfo(method);
			if (!InvokeOptimizedMethod(method, methodInfo, node))
			{
				InvokeRegularMethod(method, methodInfo, node);
			}
		}

		private bool InvokeOptimizedMethod(IMethod method, MethodInfo mi, MethodInvocationExpression node)
		{
			if (Array_get_Length == mi)
			{
				if (!GetType(node.Target).IsArray)
				{
					return false;
				}
				Visit(node.Target);
				PopType();
				_il.Emit(OpCodes.Ldlen);
				PushType(base.TypeSystemServices.IntType);
				return true;
			}
			if (mi.DeclaringType != Builtins_ArrayTypedConstructor.DeclaringType)
			{
				return false;
			}
			if (mi.IsGenericMethod)
			{
				if (Builtins_ArrayGenericConstructor == mi.GetGenericMethodDefinition())
				{
					IType type = method.ConstructedInfo.GenericArguments[0];
					EmitNewArray(type, node.Arguments[0]);
					return true;
				}
				if (mi.Name == "matrix")
				{
					EmitNewMatrix(node);
					return true;
				}
				return false;
			}
			if (Builtins_ArrayTypedConstructor == mi)
			{
				IType type = TypeSystemServices.GetReferencedType(node.Arguments[0]);
				if (null != type)
				{
					EmitNewArray(type, node.Arguments[1]);
					return true;
				}
			}
			else if (Builtins_ArrayTypedCollectionConstructor == mi)
			{
				IType type = TypeSystemServices.GetReferencedType(node.Arguments[0]);
				if (null != type)
				{
					ListLiteralExpression listLiteralExpression = node.Arguments[1] as ListLiteralExpression;
					if (null != listLiteralExpression)
					{
						EmitArray(type, listLiteralExpression.Items);
						PushType(type.MakeArrayType(1));
						return true;
					}
				}
			}
			return false;
		}

		private void EmitNewMatrix(MethodInvocationExpression node)
		{
			IType expressionType = GetExpressionType(node);
			Type systemType = GetSystemType(expressionType);
			EmitGetTypeFromHandle(systemType.GetElementType());
			PopType();
			EmitArray(base.TypeSystemServices.IntType, node.Arguments);
			Call(Array_CreateInstance);
			Castclass(systemType);
			PushType(expressionType);
		}

		private void EmitNewArray(IType type, Expression length)
		{
			LoadIntExpression(length);
			_il.Emit(OpCodes.Newarr, GetSystemType(type));
			PushType(type.MakeArrayType(1));
		}

		private void InvokeRegularMethod(IMethod method, MethodInfo mi, MethodInvocationExpression node)
		{
			if (!CheckConditionalAttributes(method))
			{
				EmitNop();
				PushType(method.ReturnType);
				return;
			}
			IType type = null;
			Expression expression = null;
			if (!mi.IsStatic)
			{
				expression = GetTargetObject(node);
				type = expression.ExpressionType;
				PushTargetObjectFor(mi, expression, type);
			}
			PushArguments(method, node.Arguments);
			if (type != null && type is IGenericParameter)
			{
				_il.Emit(OpCodes.Constrained, GetSystemType(type));
			}
			_il.EmitCall(GetCallOpCode(expression, method), mi, null);
			PushType(method.ReturnType);
		}

		private bool CheckConditionalAttributes(IMethod method)
		{
			foreach (string conditionalSymbol in GetConditionalSymbols(method))
			{
				if (!base.Parameters.Defines.ContainsKey(conditionalSymbol))
				{
					base.Context.TraceInfo("call to method '{0}' not emitted because the symbol '{1}' is not defined.", method, conditionalSymbol);
					return false;
				}
			}
			return true;
		}

		private IEnumerable<string> GetConditionalSymbols(IMethod method)
		{
			GenericMappedMethod genericMappedMethod = method as GenericMappedMethod;
			if (genericMappedMethod != null)
			{
				return GetConditionalSymbols(genericMappedMethod.SourceMember);
			}
			GenericConstructedMethod genericConstructedMethod = method as GenericConstructedMethod;
			if (genericConstructedMethod != null)
			{
				return GetConditionalSymbols(genericConstructedMethod.GenericDefinition);
			}
			ExternalMethod externalMethod = method as ExternalMethod;
			if (externalMethod != null)
			{
				return GetConditionalSymbols(externalMethod);
			}
			InternalMethod internalMethod = method as InternalMethod;
			if (internalMethod != null)
			{
				return GetConditionalSymbols(internalMethod);
			}
			return NoSymbols;
		}

		private IEnumerable<string> GetConditionalSymbols(ExternalMethod method)
		{
			try
			{
				object[] customAttributes = method.MethodInfo.GetCustomAttributes(typeof(ConditionalAttribute), inherit: false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					ConditionalAttribute attr = (ConditionalAttribute)customAttributes[i];
					yield return attr.ConditionString;
				}
			}
			finally
			{
			}
		}

		private IEnumerable<string> GetConditionalSymbols(InternalMethod method)
		{
			foreach (Boo.Lang.Compiler.Ast.Attribute attr in MetadataUtil.GetCustomAttributes(method.Method, base.TypeSystemServices.ConditionalAttribute))
			{
				if (1 == attr.Arguments.Count)
				{
					StringLiteralExpression conditionString = attr.Arguments[0] as StringLiteralExpression;
					if (conditionString != null)
					{
						yield return conditionString.Value;
					}
				}
			}
		}

		private void PushTargetObjectFor(MethodInfo methodToBeInvoked, Expression target, IType targetType)
		{
			if (targetType is IGenericParameter)
			{
				LoadAddress(target);
			}
			else if (targetType.IsValueType)
			{
				if (methodToBeInvoked.DeclaringType.IsValueType)
				{
					LoadAddress(target);
					return;
				}
				Visit(target);
				EmitBox(PopType());
			}
			else
			{
				Visit(target);
				PopType();
			}
		}

		private static Expression GetTargetObject(MethodInvocationExpression node)
		{
			Expression target = node.Target;
			GenericReferenceExpression genericReferenceExpression = target as GenericReferenceExpression;
			if (genericReferenceExpression != null)
			{
				target = genericReferenceExpression.Target;
			}
			return (target as MemberReferenceExpression)?.Target;
		}

		private OpCode GetCallOpCode(Expression target, IMethod method)
		{
			if (method.IsStatic)
			{
				return OpCodes.Call;
			}
			if (NodeType.SuperLiteralExpression == target.NodeType)
			{
				return OpCodes.Call;
			}
			if (IsValueTypeMethodCall(target, method))
			{
				return OpCodes.Call;
			}
			return OpCodes.Callvirt;
		}

		private bool IsValueTypeMethodCall(Expression target, IMethod method)
		{
			IType expressionType = target.ExpressionType;
			return expressionType.IsValueType && method.DeclaringType == expressionType;
		}

		private void InvokeSuperMethod(IMethod method, MethodInvocationExpression node)
		{
			IMethod method2 = (IMethod)GetEntity(node.Target);
			MethodInfo methodInfo = GetMethodInfo(method2);
			if (method.DeclaringType.IsValueType)
			{
				_il.Emit(OpCodes.Ldarga_S, 0);
			}
			else
			{
				_il.Emit(OpCodes.Ldarg_0);
			}
			PushArguments(method2, node.Arguments);
			Call(methodInfo);
			PushType(method2.ReturnType);
		}

		private void EmitGetTypeFromHandle(Type type)
		{
			_il.Emit(OpCodes.Ldtoken, type);
			Call(Type_GetTypeFromHandle);
			PushType(base.TypeSystemServices.TypeType);
		}

		private void OnEval(MethodInvocationExpression node)
		{
			int num = node.Arguments.Count - 1;
			for (int i = 0; i < num; i++)
			{
				Visit(node.Arguments[i]);
				DiscardValueOnStack();
			}
			Visit(node.Arguments[-1]);
		}

		private void OnAddressOf(MethodInvocationExpression node)
		{
			MemberReferenceExpression node2 = (MemberReferenceExpression)node.Arguments[0];
			MethodInfo methodInfo = GetMethodInfo((IMethod)GetEntity(node2));
			if (methodInfo.IsVirtual)
			{
				Dup();
				_il.Emit(OpCodes.Ldvirtftn, methodInfo);
			}
			else
			{
				_il.Emit(OpCodes.Ldftn, methodInfo);
			}
			PushType(base.TypeSystemServices.IntPtrType);
		}

		private void OnBuiltinFunction(BuiltinFunction function, MethodInvocationExpression node)
		{
			switch (function.FunctionType)
			{
			case BuiltinFunctionType.Switch:
				OnSwitch(node);
				break;
			case BuiltinFunctionType.AddressOf:
				OnAddressOf(node);
				break;
			case BuiltinFunctionType.Eval:
				OnEval(node);
				break;
			case BuiltinFunctionType.InitValueType:
				OnInitValueType(node);
				break;
			default:
				NotImplemented(node, "BuiltinFunction: " + function.FunctionType);
				break;
			}
		}

		private void OnInitValueType(MethodInvocationExpression node)
		{
			Debug.Assert(1 == node.Arguments.Count);
			Expression expression = node.Arguments[0];
			LoadAddressForInitObj(expression);
			IType expressionType = GetExpressionType(expression);
			Type systemType = GetSystemType(expressionType);
			Debug.Assert(systemType.IsValueType || (systemType.IsGenericParameter && expressionType.IsValueType));
			_il.Emit(OpCodes.Initobj, systemType);
			PushVoid();
		}

		private void LoadAddressForInitObj(Expression argument)
		{
			IEntity entity = argument.Entity;
			switch (entity.EntityType)
			{
			case EntityType.Local:
			{
				InternalLocal internalLocal = (InternalLocal)entity;
				LocalBuilder localBuilder = internalLocal.LocalBuilder;
				_il.Emit(OpCodes.Ldloca, localBuilder);
				break;
			}
			case EntityType.Field:
				EmitLoadFieldAddress(argument, (IField)entity);
				break;
			default:
				NotImplemented(argument, "__initobj__");
				break;
			}
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			IEntity entity = TypeSystemServices.GetEntity(node.Target);
			switch (entity.EntityType)
			{
			case EntityType.BuiltinFunction:
				OnBuiltinFunction((BuiltinFunction)entity, node);
				break;
			case EntityType.Method:
			{
				IMethod method = (IMethod)entity;
				if (node.Target.NodeType == NodeType.SuperLiteralExpression)
				{
					InvokeSuperMethod(method, node);
				}
				else
				{
					InvokeMethod(method, node);
				}
				break;
			}
			case EntityType.Constructor:
			{
				IConstructor constructor = (IConstructor)entity;
				ConstructorInfo constructorInfo = GetConstructorInfo(constructor);
				if (NodeType.SuperLiteralExpression == node.Target.NodeType || node.Target.NodeType == NodeType.SelfLiteralExpression)
				{
					_il.Emit(OpCodes.Ldarg_0);
					PushArguments(constructor, node.Arguments);
					_il.Emit(OpCodes.Call, constructorInfo);
					PushVoid();
				}
				else
				{
					PushArguments(constructor, node.Arguments);
					_il.Emit(OpCodes.Newobj, constructorInfo);
					PushType(constructor.DeclaringType);
				}
				break;
			}
			default:
				NotImplemented(node, entity.ToString());
				break;
			}
		}

		public override void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			EmitLoadLiteral(node.Value.Ticks);
			_il.Emit(OpCodes.Newobj, TimeSpan_LongConstructor);
			PushType(base.TypeSystemServices.TimeSpanType);
		}

		public override void OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
			IType type = node.ExpressionType ?? base.TypeSystemServices.IntType;
			EmitLoadLiteral(type, node.Value);
			PushType(type);
		}

		public override void OnDoubleLiteralExpression(DoubleLiteralExpression node)
		{
			IType type = node.ExpressionType ?? base.TypeSystemServices.DoubleType;
			EmitLoadLiteral(type, node.Value);
			PushType(type);
		}

		private void EmitLoadLiteral(int i)
		{
			EmitLoadLiteral(base.TypeSystemServices.IntType, i);
		}

		private void EmitLoadLiteral(long l)
		{
			EmitLoadLiteral(base.TypeSystemServices.LongType, l);
		}

		private void EmitLoadLiteral(IType type, double d)
		{
			if (type == base.TypeSystemServices.SingleType)
			{
				if (d != 0.0)
				{
					_il.Emit(OpCodes.Ldc_R4, (float)d);
					return;
				}
				_il.Emit(OpCodes.Ldc_I4_0);
				_il.Emit(OpCodes.Conv_R4);
				return;
			}
			if (type == base.TypeSystemServices.DoubleType)
			{
				if (d != 0.0)
				{
					_il.Emit(OpCodes.Ldc_R8, d);
					return;
				}
				_il.Emit(OpCodes.Ldc_I4_0);
				_il.Emit(OpCodes.Conv_R8);
				return;
			}
			throw new InvalidOperationException($"`{type}' is not a literal");
		}

		private void EmitLoadLiteral(IType type, long l)
		{
			if (type.IsEnum)
			{
				type = base.TypeSystemServices.Map(GetEnumUnderlyingType(type));
			}
			if (!IsInteger(type) && type != base.TypeSystemServices.CharType)
			{
				throw new InvalidOperationException();
			}
			bool flag = true;
			if (l > 8 || l < -1)
			{
				goto IL_01a8;
			}
			switch (l - -1)
			{
			case 0L:
				break;
			case 1L:
				goto IL_00e2;
			case 2L:
				goto IL_00f8;
			case 3L:
				goto IL_010e;
			case 4L:
				goto IL_0124;
			case 5L:
				goto IL_013a;
			case 6L:
				goto IL_0150;
			case 7L:
				goto IL_0166;
			case 8L:
				goto IL_017c;
			case 9L:
				goto IL_0192;
			default:
				goto IL_01a8;
			}
			if (IsLong(type) || type == base.TypeSystemServices.ULongType)
			{
				_il.Emit(OpCodes.Ldc_I8, -1L);
				flag = false;
			}
			else
			{
				_il.Emit(OpCodes.Ldc_I4_M1);
			}
			goto IL_0259;
			IL_0150:
			_il.Emit(OpCodes.Ldc_I4_5);
			goto IL_0259;
			IL_0166:
			_il.Emit(OpCodes.Ldc_I4_6);
			goto IL_0259;
			IL_0124:
			_il.Emit(OpCodes.Ldc_I4_3);
			goto IL_0259;
			IL_010e:
			_il.Emit(OpCodes.Ldc_I4_2);
			goto IL_0259;
			IL_013a:
			_il.Emit(OpCodes.Ldc_I4_4);
			goto IL_0259;
			IL_00f8:
			_il.Emit(OpCodes.Ldc_I4_1);
			goto IL_0259;
			IL_00e2:
			_il.Emit(OpCodes.Ldc_I4_0);
			goto IL_0259;
			IL_0259:
			if (flag && IsLong(type))
			{
				_il.Emit(OpCodes.Conv_I8);
			}
			else if (type == base.TypeSystemServices.ULongType)
			{
				_il.Emit(OpCodes.Conv_U8);
			}
			return;
			IL_01a8:
			if (IsLong(type))
			{
				_il.Emit(OpCodes.Ldc_I8, l);
				return;
			}
			if (l == (sbyte)l)
			{
				_il.Emit(OpCodes.Ldc_I4_S, (sbyte)l);
			}
			else if (l == (int)l || l == (uint)l)
			{
				if ((int)l == -1)
				{
					_il.Emit(OpCodes.Ldc_I4_M1);
				}
				else
				{
					_il.Emit(OpCodes.Ldc_I4, (int)l);
				}
			}
			else
			{
				_il.Emit(OpCodes.Ldc_I8, l);
				flag = false;
			}
			goto IL_0259;
			IL_0192:
			_il.Emit(OpCodes.Ldc_I4_8);
			goto IL_0259;
			IL_017c:
			_il.Emit(OpCodes.Ldc_I4_7);
			goto IL_0259;
		}

		private bool IsLong(IType type)
		{
			return type == base.TypeSystemServices.LongType;
		}

		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			if (node.Value)
			{
				_il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				_il.Emit(OpCodes.Ldc_I4_0);
			}
			PushBool();
		}

		public override void OnHashLiteralExpression(HashLiteralExpression node)
		{
			_il.Emit(OpCodes.Newobj, Hash_Constructor);
			IType objectType = base.TypeSystemServices.ObjectType;
			foreach (ExpressionPair item in node.Items)
			{
				Dup();
				LoadExpressionWithType(objectType, item.First);
				LoadExpressionWithType(objectType, item.Second);
				_il.EmitCall(OpCodes.Callvirt, Hash_Add, null);
			}
			PushType(base.TypeSystemServices.HashType);
		}

		public override void OnGeneratorExpression(GeneratorExpression node)
		{
			NotImplemented(node, node.ToString());
		}

		public override void OnListLiteralExpression(ListLiteralExpression node)
		{
			if (node.Items.Count > 0)
			{
				EmitObjectArray(node.Items);
				_il.Emit(OpCodes.Ldc_I4_1);
				_il.Emit(OpCodes.Newobj, List_ArrayBoolConstructor);
			}
			else
			{
				_il.Emit(OpCodes.Newobj, List_EmptyConstructor);
			}
			PushType(base.TypeSystemServices.ListType);
		}

		public override void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			IArrayType arrayType = (IArrayType)node.ExpressionType;
			EmitArray(arrayType.ElementType, node.Items);
			PushType(arrayType);
		}

		public override void OnRELiteralExpression(RELiteralExpression node)
		{
			RegexOptions regexOptions = AstUtil.GetRegexOptions(node);
			_il.Emit(OpCodes.Ldstr, node.Pattern);
			if (regexOptions == RegexOptions.None)
			{
				_il.Emit(OpCodes.Newobj, Regex_Constructor);
			}
			else
			{
				EmitLoadLiteral((int)regexOptions);
				_il.Emit(OpCodes.Newobj, Regex_Constructor_Options);
			}
			PushType(node.ExpressionType);
		}

		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			if (null == node.Value)
			{
				_il.Emit(OpCodes.Ldnull);
			}
			else if (0 != node.Value.Length)
			{
				_il.Emit(OpCodes.Ldstr, node.Value);
			}
			else
			{
				_il.Emit(OpCodes.Ldsfld, typeof(string).GetField("Empty"));
			}
			PushType(base.TypeSystemServices.StringType);
		}

		public override void OnCharLiteralExpression(CharLiteralExpression node)
		{
			EmitLoadLiteral(node.Value[0]);
			PushType(base.TypeSystemServices.CharType);
		}

		public override void OnSlicingExpression(SlicingExpression node)
		{
			if (!node.IsTargetOfAssignment())
			{
				Visit(node.Target);
				IArrayType arrayType = (IArrayType)PopType();
				if (arrayType.Rank == 1)
				{
					LoadSingleDimensionalArrayElement(node, arrayType);
				}
				else
				{
					LoadMultiDimensionalArrayElement(node, arrayType);
				}
				PushType(arrayType.ElementType);
			}
		}

		private void LoadMultiDimensionalArrayElement(SlicingExpression node, IArrayType arrayType)
		{
			LoadArrayIndices(node);
			CallArrayMethod(arrayType, "Get", GetSystemType(arrayType.ElementType), ParameterTypesForArrayGet(arrayType));
		}

		private static Type[] ParameterTypesForArrayGet(IArrayType arrayType)
		{
			return (from _ in Enumerable.Range(0, arrayType.Rank)
				select typeof(int)).ToArray();
		}

		private Type[] ParameterTypesForArraySet(IArrayType arrayType)
		{
			Type[] array = new Type[arrayType.Rank + 1];
			for (int i = 0; i < arrayType.Rank; i++)
			{
				array[i] = typeof(int);
			}
			array[arrayType.Rank] = GetSystemType(arrayType.ElementType);
			return array;
		}

		private void CallArrayMethod(IType arrayType, string methodName, Type returnType, Type[] parameterTypes)
		{
			MethodInfo arrayMethod = _moduleBuilder.GetArrayMethod(GetSystemType(arrayType), methodName, CallingConventions.HasThis, returnType, parameterTypes);
			Call(arrayMethod);
		}

		private void LoadArrayIndices(SlicingExpression node)
		{
			foreach (Expression item in node.Indices.Select((Slice index) => index.Begin))
			{
				LoadIntExpression(item);
			}
		}

		private void LoadSingleDimensionalArrayElement(SlicingExpression node, IType arrayType)
		{
			EmitNormalizedArrayIndex(node, node.Indices[0].Begin);
			IType elementType = arrayType.ElementType;
			OpCode loadEntityOpCode = GetLoadEntityOpCode(elementType);
			if (OpCodes.Ldelema.Value == loadEntityOpCode.Value)
			{
				Type systemType = GetSystemType(elementType);
				_il.Emit(loadEntityOpCode, systemType);
				if (!IsByAddress(elementType))
				{
					_il.Emit(OpCodes.Ldobj, systemType);
				}
			}
			else if (OpCodes.Ldelem.Value == loadEntityOpCode.Value)
			{
				_il.Emit(loadEntityOpCode, GetSystemType(elementType));
			}
			else
			{
				_il.Emit(loadEntityOpCode);
			}
		}

		private void EmitNormalizedArrayIndex(SlicingExpression sourceNode, Expression index)
		{
			bool isNegative = false;
			if (CanBeNegative(index, ref isNegative) && !_rawArrayIndexing && !AstAnnotations.IsRawIndexing(sourceNode))
			{
				if (isNegative)
				{
					Dup();
					_il.Emit(OpCodes.Ldlen);
					LoadIntExpression(index);
					_il.Emit(OpCodes.Add);
				}
				else
				{
					Dup();
					LoadIntExpression(index);
					Call(RuntimeServices_NormalizeArrayIndex);
				}
			}
			else
			{
				LoadIntExpression(index);
			}
		}

		private bool CanBeNegative(Expression expression, ref bool isNegative)
		{
			IntegerLiteralExpression integerLiteralExpression = expression as IntegerLiteralExpression;
			if (integerLiteralExpression != null)
			{
				if (integerLiteralExpression.Value >= 0)
				{
					return false;
				}
				isNegative = true;
			}
			return true;
		}

		private void LoadIntExpression(Expression expression)
		{
			LoadExpressionWithType(base.TypeSystemServices.IntType, expression);
		}

		public override void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			Type typeFromHandle = typeof(StringBuilder);
			ConstructorInfo constructor = typeFromHandle.GetConstructor(Type.EmptyTypes);
			ConstructorInfo constructor2 = typeFromHandle.GetConstructor(new Type[1] { typeof(string) });
			MethodInfo method = Methods.InstanceFunctionOf<StringBuilder, object, StringBuilder>((StringBuilder sb) => sb.Append);
			MethodInfo method2 = Methods.InstanceFunctionOf<StringBuilder, string, StringBuilder>((StringBuilder sb) => sb.Append);
			Expression expression = node.Expressions[0];
			IType expressionType = expression.ExpressionType;
			if ((typeof(StringLiteralExpression) == expression.GetType() && ((StringLiteralExpression)expression).Value.Length > 0) || (typeof(StringLiteralExpression) != expression.GetType() && base.TypeSystemServices.StringType == expressionType))
			{
				Visit(expression);
				PopType();
				_il.Emit(OpCodes.Newobj, constructor2);
			}
			else
			{
				_il.Emit(OpCodes.Newobj, constructor);
				expression = null;
			}
			foreach (Expression expression2 in node.Expressions)
			{
				if ((!(typeof(StringLiteralExpression) == expression2.GetType()) || ((StringLiteralExpression)expression2).Value.Length != 0) && expression2 != expression)
				{
					string text = expression2["formatString"] as string;
					if (!string.IsNullOrEmpty(text))
					{
						_il.Emit(OpCodes.Ldstr, $"{{0:{text}}}");
					}
					Visit(expression2);
					expressionType = PopType();
					if (!string.IsNullOrEmpty(text))
					{
						EmitCastIfNeeded(base.TypeSystemServices.ObjectType, expressionType);
						Call(StringFormat);
					}
					if (base.TypeSystemServices.StringType == expressionType || !string.IsNullOrEmpty(text))
					{
						Call(method2);
						continue;
					}
					EmitCastIfNeeded(base.TypeSystemServices.ObjectType, expressionType);
					Call(method);
				}
			}
			Call(typeFromHandle.GetMethod("ToString", Type.EmptyTypes));
			PushType(base.TypeSystemServices.StringType);
		}

		private void LoadMemberTarget(Expression self, IMember member)
		{
			if (member.DeclaringType.IsValueType)
			{
				LoadAddress(self);
				return;
			}
			Visit(self);
			PopType();
		}

		private void EmitLoadFieldAddress(Expression expression, IField field)
		{
			if (field.IsStatic)
			{
				_il.Emit(OpCodes.Ldsflda, GetFieldInfo(field));
				return;
			}
			LoadMemberTarget(((MemberReferenceExpression)expression).Target, field);
			_il.Emit(OpCodes.Ldflda, GetFieldInfo(field));
		}

		private void EmitLoadField(Expression self, IField fieldInfo)
		{
			if (fieldInfo.IsStatic)
			{
				if (fieldInfo.IsLiteral)
				{
					EmitLoadLiteralField(self, fieldInfo);
				}
				else
				{
					if (fieldInfo.IsVolatile)
					{
						_il.Emit(OpCodes.Volatile);
					}
					_il.Emit(IsByAddress(fieldInfo.Type) ? OpCodes.Ldsflda : OpCodes.Ldsfld, GetFieldInfo(fieldInfo));
				}
			}
			else
			{
				LoadMemberTarget(self, fieldInfo);
				if (fieldInfo.IsVolatile)
				{
					_il.Emit(OpCodes.Volatile);
				}
				_il.Emit(IsByAddress(fieldInfo.Type) ? OpCodes.Ldflda : OpCodes.Ldfld, GetFieldInfo(fieldInfo));
			}
			PushType(fieldInfo.Type);
		}

		private object GetStaticValue(IField field)
		{
			InternalField internalField = field as InternalField;
			if (null != internalField)
			{
				return GetInternalFieldStaticValue(internalField);
			}
			return field.StaticValue;
		}

		private object GetInternalFieldStaticValue(InternalField field)
		{
			return GetValue(field.Type, (Expression)field.StaticValue);
		}

		private void EmitLoadLiteralField(Node node, IField fieldInfo)
		{
			object obj = GetStaticValue(fieldInfo);
			IType type = fieldInfo.Type;
			if (type.IsEnum)
			{
				Type enumUnderlyingType = GetEnumUnderlyingType(type);
				type = base.TypeSystemServices.Map(enumUnderlyingType);
				obj = Convert.ChangeType(obj, enumUnderlyingType);
			}
			if (null == obj)
			{
				_il.Emit(OpCodes.Ldnull);
			}
			else if (type == base.TypeSystemServices.BoolType)
			{
				_il.Emit(((bool)obj) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
			}
			else if (type == base.TypeSystemServices.StringType)
			{
				_il.Emit(OpCodes.Ldstr, (string)obj);
			}
			else if (type == base.TypeSystemServices.CharType)
			{
				EmitLoadLiteral(type, (char)obj);
			}
			else if (type == base.TypeSystemServices.IntType)
			{
				EmitLoadLiteral(type, (int)obj);
			}
			else if (type == base.TypeSystemServices.UIntType)
			{
				EmitLoadLiteral(type, (uint)obj);
			}
			else if (IsLong(type))
			{
				EmitLoadLiteral(type, (long)obj);
			}
			else if (type == base.TypeSystemServices.ULongType)
			{
				EmitLoadLiteral(type, (long)(ulong)obj);
			}
			else if (type == base.TypeSystemServices.SingleType)
			{
				EmitLoadLiteral(type, (float)obj);
			}
			else if (type == base.TypeSystemServices.DoubleType)
			{
				EmitLoadLiteral(type, (double)obj);
			}
			else if (type == base.TypeSystemServices.SByteType)
			{
				EmitLoadLiteral(type, (sbyte)obj);
			}
			else if (type == base.TypeSystemServices.ByteType)
			{
				EmitLoadLiteral(type, (byte)obj);
			}
			else if (type == base.TypeSystemServices.ShortType)
			{
				EmitLoadLiteral(type, (short)obj);
			}
			else if (type == base.TypeSystemServices.UShortType)
			{
				EmitLoadLiteral(type, (ushort)obj);
			}
			else
			{
				NotImplemented(node, "Literal field type: " + type.ToString());
			}
		}

		public override void OnGenericReferenceExpression(GenericReferenceExpression node)
		{
			IEntity entity = TypeSystemServices.GetEntity(node);
			switch (entity.EntityType)
			{
			case EntityType.Type:
				EmitGetTypeFromHandle(GetSystemType(node));
				break;
			case EntityType.Method:
				node.Target.Accept(this);
				break;
			default:
				NotImplemented(node, entity.ToString());
				break;
			}
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			IEntity entity = TypeSystemServices.GetEntity(node);
			switch (entity.EntityType)
			{
			case EntityType.Method:
			case EntityType.Ambiguous:
				node.Target.Accept(this);
				break;
			case EntityType.Field:
				EmitLoadField(node.Target, (IField)entity);
				break;
			case EntityType.Type:
				EmitGetTypeFromHandle(GetSystemType(node));
				break;
			default:
				NotImplemented(node, entity.ToString());
				break;
			}
		}

		private void LoadAddress(Expression expression)
		{
			if (expression.NodeType == NodeType.SelfLiteralExpression && expression.ExpressionType.IsValueType)
			{
				_il.Emit(OpCodes.Ldarg_0);
				return;
			}
			IEntity entity = expression.Entity;
			if (entity != null)
			{
				switch (entity.EntityType)
				{
				case EntityType.Local:
				{
					InternalLocal internalLocal = (InternalLocal)entity;
					_il.Emit((!internalLocal.Type.IsPointer) ? OpCodes.Ldloca : OpCodes.Ldloc, internalLocal.LocalBuilder);
					return;
				}
				case EntityType.Parameter:
				{
					InternalParameter internalParameter = (InternalParameter)entity;
					if (internalParameter.Parameter.IsByRef)
					{
						LoadParam(internalParameter);
					}
					else
					{
						_il.Emit(OpCodes.Ldarga, internalParameter.Index);
					}
					return;
				}
				case EntityType.Field:
				{
					IField field = (IField)entity;
					if (!field.IsLiteral)
					{
						EmitLoadFieldAddress(expression, field);
						return;
					}
					break;
				}
				}
			}
			if (IsValueTypeArraySlicing(expression))
			{
				LoadArrayElementAddress((SlicingExpression)expression);
				return;
			}
			Visit(expression);
			if (!AstUtil.IsIndirection(expression))
			{
				LocalBuilder local = _il.DeclareLocal(GetSystemType(PopType()));
				_il.Emit(OpCodes.Stloc, local);
				_il.Emit(OpCodes.Ldloca, local);
			}
		}

		private void LoadArrayElementAddress(SlicingExpression slicing)
		{
			Visit(slicing.Target);
			IArrayType arrayType = (IArrayType)PopType();
			if (arrayType.Rank == 1)
			{
				LoadSingleDimensionalArrayElementAddress(slicing, arrayType);
			}
			else
			{
				LoadMultiDimensionalArrayElementAddress(slicing, arrayType);
			}
		}

		private void LoadMultiDimensionalArrayElementAddress(SlicingExpression slicing, IArrayType arrayType)
		{
			LoadArrayIndices(slicing);
			CallArrayMethod(arrayType, "Address", GetSystemType(arrayType.ElementType).MakeByRefType(), ParameterTypesForArrayGet(arrayType));
		}

		private void LoadSingleDimensionalArrayElementAddress(SlicingExpression slicing, IArrayType arrayType)
		{
			EmitNormalizedArrayIndex(slicing, slicing.Indices[0].Begin);
			_il.Emit(OpCodes.Ldelema, GetSystemType(arrayType.ElementType));
		}

		private bool IsValueTypeArraySlicing(Expression expression)
		{
			SlicingExpression slicingExpression = expression as SlicingExpression;
			if (slicingExpression != null)
			{
				IArrayType arrayType = (IArrayType)slicingExpression.Target.ExpressionType;
				return arrayType.ElementType.IsValueType;
			}
			return false;
		}

		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			LoadSelf(node);
		}

		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			LoadSelf(node);
		}

		private void LoadSelf(Expression node)
		{
			_il.Emit(OpCodes.Ldarg_0);
			if (node.ExpressionType.IsValueType)
			{
				_il.Emit(OpCodes.Ldobj, GetSystemType(node.ExpressionType));
			}
			PushType(node.ExpressionType);
		}

		public override void OnNullLiteralExpression(NullLiteralExpression node)
		{
			_il.Emit(OpCodes.Ldnull);
			PushType(null);
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			IEntity entity = TypeSystemServices.GetEntity(node);
			switch (entity.EntityType)
			{
			case EntityType.Local:
				if (!AstUtil.IsIndirection(node.ParentNode))
				{
					LoadLocal((InternalLocal)entity);
				}
				else
				{
					LoadIndirectLocal((InternalLocal)entity);
				}
				break;
			case EntityType.Parameter:
			{
				InternalParameter internalParameter = (InternalParameter)entity;
				LoadParam(internalParameter);
				if (internalParameter.Parameter.IsByRef)
				{
					OpCode loadRefParamCode = GetLoadRefParamCode(internalParameter.Type);
					if (loadRefParamCode.Value == OpCodes.Ldobj.Value)
					{
						_il.Emit(loadRefParamCode, GetSystemType(internalParameter.Type));
					}
					else
					{
						_il.Emit(loadRefParamCode);
					}
				}
				PushType(internalParameter.Type);
				break;
			}
			case EntityType.Type:
			case EntityType.Array:
				EmitGetTypeFromHandle(GetSystemType(node));
				break;
			default:
				NotImplemented(node, entity.ToString());
				break;
			}
		}

		private void LoadParam(InternalParameter param)
		{
			int index = param.Index;
			switch (index)
			{
			case 0:
				_il.Emit(OpCodes.Ldarg_0);
				return;
			case 1:
				_il.Emit(OpCodes.Ldarg_1);
				return;
			case 2:
				_il.Emit(OpCodes.Ldarg_2);
				return;
			case 3:
				_il.Emit(OpCodes.Ldarg_3);
				return;
			}
			if (index < 256)
			{
				_il.Emit(OpCodes.Ldarg_S, index);
			}
			else
			{
				_il.Emit(OpCodes.Ldarg, index);
			}
		}

		private void SetLocal(BinaryExpression node, InternalLocal tag, bool leaveValueOnStack)
		{
			if (AstUtil.IsIndirection(node.Left))
			{
				_il.Emit(OpCodes.Ldloc, tag.LocalBuilder);
			}
			node.Right.Accept(this);
			IType type = null;
			if (leaveValueOnStack)
			{
				type = PeekTypeOnStack();
				Dup();
			}
			else
			{
				type = PopType();
			}
			if (!AstUtil.IsIndirection(node.Left))
			{
				EmitAssignment(tag, type);
			}
			else
			{
				EmitIndirectAssignment(tag, type);
			}
		}

		private void EmitAssignment(InternalLocal tag, IType typeOnStack)
		{
			LocalBuilder localBuilder = tag.LocalBuilder;
			EmitCastIfNeeded(tag.Type, typeOnStack);
			_il.Emit(OpCodes.Stloc, localBuilder);
		}

		private void EmitIndirectAssignment(InternalLocal local, IType typeOnStack)
		{
			IType elementType = local.Type.ElementType;
			EmitCastIfNeeded(elementType, typeOnStack);
			OpCode storeRefParamCode = GetStoreRefParamCode(elementType);
			if (storeRefParamCode == OpCodes.Stobj)
			{
				_il.Emit(storeRefParamCode, GetSystemType(elementType));
			}
			else
			{
				_il.Emit(storeRefParamCode);
			}
		}

		private void SetField(Node sourceNode, IField field, Expression reference, Expression value, bool leaveValueOnStack)
		{
			OpCode opcode = OpCodes.Stsfld;
			if (!field.IsStatic)
			{
				opcode = OpCodes.Stfld;
				if (null != reference)
				{
					LoadMemberTarget(((MemberReferenceExpression)reference).Target, field);
				}
			}
			LoadExpressionWithType(field.Type, value);
			LocalBuilder local = null;
			if (leaveValueOnStack)
			{
				Dup();
				local = _il.DeclareLocal(GetSystemType(field.Type));
				_il.Emit(OpCodes.Stloc, local);
			}
			if (field.IsVolatile)
			{
				_il.Emit(OpCodes.Volatile);
			}
			_il.Emit(opcode, GetFieldInfo(field));
			if (leaveValueOnStack)
			{
				_il.Emit(OpCodes.Ldloc, local);
				PushType(field.Type);
			}
		}

		private void SetProperty(IProperty property, Expression reference, Expression value, bool leaveValueOnStack)
		{
			OpCode opcode = OpCodes.Call;
			MethodInfo methodInfo = GetMethodInfo(property.GetSetMethod());
			IType type = null;
			if (null != reference && !methodInfo.IsStatic)
			{
				Expression target = ((MemberReferenceExpression)reference).Target;
				type = target.ExpressionType;
				if (methodInfo.DeclaringType.IsValueType || type is IGenericParameter)
				{
					LoadAddress(target);
				}
				else
				{
					opcode = GetCallOpCode(target, property.GetSetMethod());
					target.Accept(this);
					PopType();
				}
			}
			LoadExpressionWithType(property.Type, value);
			LocalBuilder local = null;
			if (leaveValueOnStack)
			{
				Dup();
				local = _il.DeclareLocal(GetSystemType(property.Type));
				_il.Emit(OpCodes.Stloc, local);
			}
			if (type is IGenericParameter)
			{
				_il.Emit(OpCodes.Constrained, GetSystemType(type));
				opcode = OpCodes.Callvirt;
			}
			_il.EmitCall(opcode, methodInfo, null);
			if (leaveValueOnStack)
			{
				_il.Emit(OpCodes.Ldloc, local);
				PushType(property.Type);
			}
		}

		private bool EmitDebugInfo(Node node)
		{
			if (!base.Parameters.Debug)
			{
				return false;
			}
			return EmitDebugInfo(node, node);
		}

		private bool EmitDebugInfo(Node startNode, Node endNode)
		{
			LexicalInfo lexicalInfo = startNode.LexicalInfo;
			if (!lexicalInfo.IsValid)
			{
				return false;
			}
			ISymbolDocumentWriter documentWriter = GetDocumentWriter(lexicalInfo.FullPath);
			if (null == documentWriter)
			{
				return false;
			}
			if (_dbgSymbols.Contains(lexicalInfo))
			{
				base.Context.TraceInfo("duplicate symbol emit attempt for '{0}' : '{1}'.", lexicalInfo, startNode);
				return false;
			}
			if (_dbgSymbols.Count >= 5)
			{
				_dbgSymbols.Dequeue();
			}
			_dbgSymbols.Enqueue(lexicalInfo);
			try
			{
				_il.MarkSequencePoint(documentWriter, lexicalInfo.Line, 0, lexicalInfo.Line + 1, 0);
			}
			catch (Exception error)
			{
				Error(CompilerErrorFactory.InternalError(startNode, error));
				return false;
			}
			return true;
		}

		private void EmitNop()
		{
			_il.Emit(OpCodes.Nop);
		}

		private ISymbolDocumentWriter GetDocumentWriter(string fname)
		{
			ISymbolDocumentWriter cachedDocumentWriter = GetCachedDocumentWriter(fname);
			if (null != cachedDocumentWriter)
			{
				return cachedDocumentWriter;
			}
			cachedDocumentWriter = _moduleBuilder.DefineDocument(fname, Guid.Empty, Guid.Empty, SymDocumentType.Text);
			_symbolDocWriters.Add(fname, cachedDocumentWriter);
			return cachedDocumentWriter;
		}

		private ISymbolDocumentWriter GetCachedDocumentWriter(string fname)
		{
			return (ISymbolDocumentWriter)_symbolDocWriters[fname];
		}

		private bool IsBoolOrInt(IType type)
		{
			return base.TypeSystemServices.BoolType == type || base.TypeSystemServices.IntType == type;
		}

		private void PushArguments(IMethodBase entity, ExpressionCollection args)
		{
			IParameter[] parameters = entity.GetParameters();
			for (int i = 0; i < args.Count; i++)
			{
				IType type = parameters[i].Type;
				Expression expression = args[i];
				if (parameters[i].IsByRef)
				{
					LoadAddress(expression);
				}
				else
				{
					LoadExpressionWithType(type, expression);
				}
			}
		}

		private void EmitObjectArray(ExpressionCollection items)
		{
			EmitArray(base.TypeSystemServices.ObjectType, items);
		}

		private void EmitArray(IType type, ExpressionCollection items)
		{
			EmitLoadLiteral(items.Count);
			_il.Emit(OpCodes.Newarr, GetSystemType(type));
			if (items.Count == 0)
			{
				return;
			}
			int num = 0;
			if (items.Count > 3 && base.TypeSystemServices.IsPrimitiveNumber(type))
			{
				foreach (Expression item in items)
				{
					if ((item.NodeType != NodeType.IntegerLiteralExpression && item.NodeType != NodeType.DoubleLiteralExpression) || type != item.ExpressionType)
					{
						num = 0;
						break;
					}
					if (!IsZeroEquivalent(item))
					{
						num++;
					}
				}
			}
			if (num <= 3)
			{
				EmitInlineArrayInit(type, items);
			}
			else
			{
				EmitPackedArrayInit(type, items);
			}
		}

		private void EmitInlineArrayInit(IType type, ExpressionCollection items)
		{
			OpCode storeEntityOpCode = GetStoreEntityOpCode(type);
			for (int i = 0; i < items.Count; i++)
			{
				if (!IsNull(items[i]) && (type != items[i].ExpressionType || !IsZeroEquivalent(items[i])))
				{
					StoreEntity(storeEntityOpCode, i, items[i], type);
				}
			}
		}

		private void EmitPackedArrayInit(IType type, ExpressionCollection items)
		{
			byte[] array = CreateByteArrayFromLiteralCollection(type, items);
			if (null == array)
			{
				EmitInlineArrayInit(type, items);
				return;
			}
			if (!_packedArrays.TryGetValue(array, out var value))
			{
				value = _moduleBuilder.DefineInitializedData(base.Context.GetUniqueName("newarr"), array, FieldAttributes.Private);
				_packedArrays.Add(array, value);
			}
			Dup();
			_il.Emit(OpCodes.Ldtoken, value);
			Call(RuntimeHelpers_InitializeArray);
		}

		private byte[] CreateByteArrayFromLiteralCollection(IType type, ExpressionCollection items)
		{
			using MemoryStream memoryStream = new MemoryStream(items.Count * base.TypeSystemServices.SizeOf(type));
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				foreach (Expression item in items)
				{
					if (item.NodeType == NodeType.IntegerLiteralExpression)
					{
						IntegerLiteralExpression integerLiteralExpression = (IntegerLiteralExpression)item;
						if (type == base.TypeSystemServices.IntType)
						{
							binaryWriter.Write(Convert.ToInt32(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.UIntType)
						{
							binaryWriter.Write(Convert.ToUInt32(integerLiteralExpression.Value));
							continue;
						}
						if (IsLong(type))
						{
							binaryWriter.Write(Convert.ToInt64(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ULongType)
						{
							binaryWriter.Write(Convert.ToUInt64(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ShortType)
						{
							binaryWriter.Write(Convert.ToInt16(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.UShortType)
						{
							binaryWriter.Write(Convert.ToUInt16(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ByteType)
						{
							binaryWriter.Write(Convert.ToByte(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.SByteType)
						{
							binaryWriter.Write(Convert.ToSByte(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.SingleType)
						{
							binaryWriter.Write(Convert.ToSingle(integerLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.DoubleType)
						{
							binaryWriter.Write(Convert.ToDouble(integerLiteralExpression.Value));
							continue;
						}
						return null;
					}
					if (item.NodeType == NodeType.DoubleLiteralExpression)
					{
						DoubleLiteralExpression doubleLiteralExpression = (DoubleLiteralExpression)item;
						if (type == base.TypeSystemServices.SingleType)
						{
							binaryWriter.Write(Convert.ToSingle(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.DoubleType)
						{
							binaryWriter.Write(Convert.ToDouble(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.IntType)
						{
							binaryWriter.Write(Convert.ToInt32(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.UIntType)
						{
							binaryWriter.Write(Convert.ToUInt32(doubleLiteralExpression.Value));
							continue;
						}
						if (IsLong(type))
						{
							binaryWriter.Write(Convert.ToInt64(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ULongType)
						{
							binaryWriter.Write(Convert.ToUInt64(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ShortType)
						{
							binaryWriter.Write(Convert.ToInt16(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.UShortType)
						{
							binaryWriter.Write(Convert.ToUInt16(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.ByteType)
						{
							binaryWriter.Write(Convert.ToByte(doubleLiteralExpression.Value));
							continue;
						}
						if (type == base.TypeSystemServices.SByteType)
						{
							binaryWriter.Write(Convert.ToSByte(doubleLiteralExpression.Value));
							continue;
						}
						return null;
					}
					return null;
				}
			}
			return memoryStream.ToArray();
		}

		private bool IsInteger(IType type)
		{
			return base.TypeSystemServices.IsIntegerNumber(type);
		}

		private MethodInfo GetToDecimalConversionMethod(IType type)
		{
			MethodInfo method = typeof(decimal).GetMethod("op_Implicit", new Type[1] { GetSystemType(type) });
			if (method == null)
			{
				method = typeof(decimal).GetMethod("op_Explicit", new Type[1] { GetSystemType(type) });
				if (method == null)
				{
					NotImplemented($"Numeric promotion for {type} to decimal not implemented!");
				}
			}
			return method;
		}

		private MethodInfo GetFromDecimalConversionMethod(IType type)
		{
			string name = "To" + type.Name;
			MethodInfo method = typeof(decimal).GetMethod(name, new Type[1] { typeof(decimal) });
			if (method == null)
			{
				NotImplemented($"Numeric promotion for decimal to {type} not implemented!");
			}
			return method;
		}

		private OpCode GetArithmeticOpCode(IType type, BinaryOperatorType op)
		{
			if (IsCheckedIntegerOperand(type))
			{
				switch (op)
				{
				case BinaryOperatorType.Addition:
					return OpCodes.Add_Ovf;
				case BinaryOperatorType.Subtraction:
					return OpCodes.Sub_Ovf;
				case BinaryOperatorType.Multiply:
					return OpCodes.Mul_Ovf;
				case BinaryOperatorType.Division:
					return OpCodes.Div;
				case BinaryOperatorType.Modulus:
					return OpCodes.Rem;
				}
			}
			else
			{
				switch (op)
				{
				case BinaryOperatorType.Addition:
					return OpCodes.Add;
				case BinaryOperatorType.Subtraction:
					return OpCodes.Sub;
				case BinaryOperatorType.Multiply:
					return OpCodes.Mul;
				case BinaryOperatorType.Division:
					return OpCodes.Div;
				case BinaryOperatorType.Modulus:
					return OpCodes.Rem;
				}
			}
			throw new ArgumentException("op");
		}

		private OpCode GetLoadEntityOpCode(IType type)
		{
			if (IsByAddress(type))
			{
				return OpCodes.Ldelema;
			}
			if (!type.IsValueType)
			{
				return (type is IGenericParameter) ? OpCodes.Ldelem : OpCodes.Ldelem_Ref;
			}
			if (type.IsEnum)
			{
				type = base.TypeSystemServices.Map(GetEnumUnderlyingType(type));
			}
			if (base.TypeSystemServices.IntType == type)
			{
				return OpCodes.Ldelem_I4;
			}
			if (base.TypeSystemServices.UIntType == type)
			{
				return OpCodes.Ldelem_U4;
			}
			if (IsLong(type))
			{
				return OpCodes.Ldelem_I8;
			}
			if (base.TypeSystemServices.SByteType == type)
			{
				return OpCodes.Ldelem_I1;
			}
			if (base.TypeSystemServices.ByteType == type)
			{
				return OpCodes.Ldelem_U1;
			}
			if (base.TypeSystemServices.ShortType == type || base.TypeSystemServices.CharType == type)
			{
				return OpCodes.Ldelem_I2;
			}
			if (base.TypeSystemServices.UShortType == type)
			{
				return OpCodes.Ldelem_U2;
			}
			if (base.TypeSystemServices.SingleType == type)
			{
				return OpCodes.Ldelem_R4;
			}
			if (base.TypeSystemServices.DoubleType == type)
			{
				return OpCodes.Ldelem_R8;
			}
			return OpCodes.Ldelema;
		}

		private OpCode GetStoreEntityOpCode(IType tag)
		{
			if (tag.IsValueType || tag is IGenericParameter)
			{
				if (tag.IsEnum)
				{
					tag = base.TypeSystemServices.Map(GetEnumUnderlyingType(tag));
				}
				if (base.TypeSystemServices.IntType == tag || base.TypeSystemServices.UIntType == tag)
				{
					return OpCodes.Stelem_I4;
				}
				if (IsLong(tag) || base.TypeSystemServices.ULongType == tag)
				{
					return OpCodes.Stelem_I8;
				}
				if (base.TypeSystemServices.ShortType == tag || base.TypeSystemServices.CharType == tag)
				{
					return OpCodes.Stelem_I2;
				}
				if (base.TypeSystemServices.ByteType == tag || base.TypeSystemServices.SByteType == tag)
				{
					return OpCodes.Stelem_I1;
				}
				if (base.TypeSystemServices.SingleType == tag)
				{
					return OpCodes.Stelem_R4;
				}
				if (base.TypeSystemServices.DoubleType == tag)
				{
					return OpCodes.Stelem_R8;
				}
				return OpCodes.Stobj;
			}
			return OpCodes.Stelem_Ref;
		}

		private OpCode GetLoadRefParamCode(IType tag)
		{
			if (tag.IsValueType)
			{
				if (tag.IsEnum)
				{
					tag = base.TypeSystemServices.Map(GetEnumUnderlyingType(tag));
				}
				if (base.TypeSystemServices.IntType == tag)
				{
					return OpCodes.Ldind_I4;
				}
				if (IsLong(tag) || base.TypeSystemServices.ULongType == tag)
				{
					return OpCodes.Ldind_I8;
				}
				if (base.TypeSystemServices.ByteType == tag)
				{
					return OpCodes.Ldind_U1;
				}
				if (base.TypeSystemServices.ShortType == tag || base.TypeSystemServices.CharType == tag)
				{
					return OpCodes.Ldind_I2;
				}
				if (base.TypeSystemServices.SingleType == tag)
				{
					return OpCodes.Ldind_R4;
				}
				if (base.TypeSystemServices.DoubleType == tag)
				{
					return OpCodes.Ldind_R8;
				}
				if (base.TypeSystemServices.UShortType == tag)
				{
					return OpCodes.Ldind_U2;
				}
				if (base.TypeSystemServices.UIntType == tag)
				{
					return OpCodes.Ldind_U4;
				}
				return OpCodes.Ldobj;
			}
			return OpCodes.Ldind_Ref;
		}

		private OpCode GetStoreRefParamCode(IType tag)
		{
			if (tag.IsValueType)
			{
				if (tag.IsEnum)
				{
					tag = base.TypeSystemServices.Map(GetEnumUnderlyingType(tag));
				}
				if (base.TypeSystemServices.IntType == tag || base.TypeSystemServices.UIntType == tag)
				{
					return OpCodes.Stind_I4;
				}
				if (IsLong(tag) || base.TypeSystemServices.ULongType == tag)
				{
					return OpCodes.Stind_I8;
				}
				if (base.TypeSystemServices.ByteType == tag)
				{
					return OpCodes.Stind_I1;
				}
				if (base.TypeSystemServices.ShortType == tag || base.TypeSystemServices.CharType == tag)
				{
					return OpCodes.Stind_I2;
				}
				if (base.TypeSystemServices.SingleType == tag)
				{
					return OpCodes.Stind_R4;
				}
				if (base.TypeSystemServices.DoubleType == tag)
				{
					return OpCodes.Stind_R8;
				}
				return OpCodes.Stobj;
			}
			return OpCodes.Stind_Ref;
		}

		private bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return (IsPtr(expectedType) && IsPtr(actualType)) || TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		private bool IsPtr(IType type)
		{
			return type == base.TypeSystemServices.IntPtrType || type == base.TypeSystemServices.UIntPtrType;
		}

		private void EmitCastIfNeeded(IType expectedType, IType actualType)
		{
			if (actualType == null || expectedType == actualType || expectedType.IsPointer || actualType.IsPointer)
			{
				return;
			}
			if (IsAssignableFrom(expectedType, actualType))
			{
				EmitBoxIfNeeded(expectedType, actualType);
				return;
			}
			IMethod method = base.TypeSystemServices.FindImplicitConversionOperator(actualType, expectedType) ?? base.TypeSystemServices.FindExplicitConversionOperator(actualType, expectedType);
			if (method != null)
			{
				EmitBoxIfNeeded(method.GetParameters()[0].Type, actualType);
				Call(GetMethodInfo(method));
			}
			else if (expectedType is IGenericParameter)
			{
				_il.Emit(OpCodes.Unbox_Any, GetSystemType(expectedType));
			}
			else if (expectedType.IsValueType)
			{
				if (!actualType.IsValueType)
				{
					EmitUnbox(expectedType);
					return;
				}
				if (base.TypeSystemServices.DecimalType == expectedType)
				{
					Call(GetToDecimalConversionMethod(actualType));
					return;
				}
				if (base.TypeSystemServices.DecimalType == actualType)
				{
					Call(GetFromDecimalConversionMethod(expectedType));
					return;
				}
				if (actualType.IsEnum)
				{
					actualType = base.TypeSystemServices.Map(GetEnumUnderlyingType(actualType));
				}
				if (expectedType.IsEnum)
				{
					expectedType = base.TypeSystemServices.Map(GetEnumUnderlyingType(expectedType));
				}
				if (actualType != expectedType)
				{
					_il.Emit(GetNumericPromotionOpCode(expectedType));
				}
			}
			else
			{
				EmitRuntimeCoercionIfNeeded(expectedType, actualType);
			}
		}

		private void EmitRuntimeCoercionIfNeeded(IType expectedType, IType actualType)
		{
			base.Context.TraceInfo("castclass: expected type='{0}', type on stack='{1}'", expectedType, actualType);
			Type systemType = GetSystemType(expectedType);
			if (base.TypeSystemServices.IsSystemObject(actualType))
			{
				Dup();
				Isinst(systemType);
				Label label = _il.DefineLabel();
				_il.Emit(OpCodes.Brtrue, label);
				EmitGetTypeFromHandle(systemType);
				PopType();
				Call(RuntimeServices_Coerce);
				_il.MarkLabel(label);
			}
			Castclass(systemType);
		}

		private void Call(MethodInfo method)
		{
			_il.EmitCall(OpCodes.Call, method, null);
		}

		private void Castclass(Type expectedSystemType)
		{
			_il.Emit(OpCodes.Castclass, expectedSystemType);
		}

		private void EmitBoxIfNeeded(IType expectedType, IType actualType)
		{
			if ((actualType.IsValueType && !expectedType.IsValueType) || (actualType is IGenericParameter && !(expectedType is IGenericParameter)))
			{
				EmitBox(actualType);
			}
		}

		private void EmitBox(IType type)
		{
			_il.Emit(OpCodes.Box, GetSystemType(type));
		}

		private void EmitUnbox(IType expectedType)
		{
			MethodInfo methodInfo = UnboxMethodFor(expectedType);
			if (null != methodInfo)
			{
				Call(methodInfo);
				return;
			}
			Type systemType = GetSystemType(expectedType);
			_il.Emit(OpCodes.Unbox, systemType);
			_il.Emit(OpCodes.Ldobj, systemType);
		}

		private MethodInfo UnboxMethodFor(IType type)
		{
			if (type == base.TypeSystemServices.ByteType)
			{
				return Methods.Of<object, byte>(RuntimeServices.UnboxByte);
			}
			if (type == base.TypeSystemServices.SByteType)
			{
				return Methods.Of<object, sbyte>(RuntimeServices.UnboxSByte);
			}
			if (type == base.TypeSystemServices.ShortType)
			{
				return Methods.Of<object, short>(RuntimeServices.UnboxInt16);
			}
			if (type == base.TypeSystemServices.UShortType)
			{
				return Methods.Of<object, ushort>(RuntimeServices.UnboxUInt16);
			}
			if (type == base.TypeSystemServices.IntType)
			{
				return Methods.Of<object, int>(RuntimeServices.UnboxInt32);
			}
			if (type == base.TypeSystemServices.UIntType)
			{
				return Methods.Of<object, uint>(RuntimeServices.UnboxUInt32);
			}
			if (IsLong(type))
			{
				return Methods.Of<object, long>(RuntimeServices.UnboxInt64);
			}
			if (type == base.TypeSystemServices.ULongType)
			{
				return Methods.Of<object, ulong>(RuntimeServices.UnboxUInt64);
			}
			if (type == base.TypeSystemServices.SingleType)
			{
				return Methods.Of<object, float>(RuntimeServices.UnboxSingle);
			}
			if (type == base.TypeSystemServices.DoubleType)
			{
				return Methods.Of<object, double>(RuntimeServices.UnboxDouble);
			}
			if (type == base.TypeSystemServices.DecimalType)
			{
				return Methods.Of<object, decimal>(RuntimeServices.UnboxDecimal);
			}
			if (type == base.TypeSystemServices.BoolType)
			{
				return Methods.Of<object, bool>(RuntimeServices.UnboxBoolean);
			}
			if (type == base.TypeSystemServices.CharType)
			{
				return Methods.Of<object, char>(RuntimeServices.UnboxChar);
			}
			return null;
		}

		private OpCode GetNumericPromotionOpCode(IType type)
		{
			return NumericPromotionOpcodeFor(TypeCodeFor(type), _checked);
		}

		private static OpCode NumericPromotionOpcodeFor(TypeCode typeCode, bool @checked)
		{
			switch (typeCode)
			{
			case TypeCode.SByte:
				return @checked ? OpCodes.Conv_Ovf_I1 : OpCodes.Conv_I1;
			case TypeCode.Byte:
				return @checked ? OpCodes.Conv_Ovf_U1 : OpCodes.Conv_U1;
			case TypeCode.Int16:
				return @checked ? OpCodes.Conv_Ovf_I2 : OpCodes.Conv_I2;
			case TypeCode.Char:
			case TypeCode.UInt16:
				return @checked ? OpCodes.Conv_Ovf_U2 : OpCodes.Conv_U2;
			case TypeCode.Int32:
				return @checked ? OpCodes.Conv_Ovf_I4 : OpCodes.Conv_I4;
			case TypeCode.UInt32:
				return @checked ? OpCodes.Conv_Ovf_U4 : OpCodes.Conv_U4;
			case TypeCode.Int64:
				return @checked ? OpCodes.Conv_Ovf_I8 : OpCodes.Conv_I8;
			case TypeCode.UInt64:
				return @checked ? OpCodes.Conv_Ovf_U8 : OpCodes.Conv_U8;
			case TypeCode.Single:
				return OpCodes.Conv_R4;
			case TypeCode.Double:
				return OpCodes.Conv_R8;
			default:
				throw new ArgumentException(typeCode.ToString());
			}
		}

		private static TypeCode TypeCodeFor(IType type)
		{
			ExternalType externalType = type as ExternalType;
			if (externalType != null)
			{
				return Type.GetTypeCode(externalType.ActualType);
			}
			throw new NotImplementedException($"TypeCodeFor({type}) not implemented!");
		}

		private void StoreEntity(OpCode opcode, int index, Expression value, IType elementType)
		{
			Dup();
			EmitLoadLiteral(index);
			if (IsStobj(opcode))
			{
				Type systemType = GetSystemType(elementType);
				_il.Emit(OpCodes.Ldelema, systemType);
				LoadExpressionWithType(elementType, value);
				_il.Emit(opcode, systemType);
			}
			else
			{
				LoadExpressionWithType(elementType, value);
				_il.Emit(opcode);
			}
		}

		private void Dup()
		{
			_il.Emit(OpCodes.Dup);
		}

		private bool IsStobj(OpCode code)
		{
			return OpCodes.Stobj.Value == code.Value;
		}

		private void DefineAssemblyAttributes()
		{
			foreach (Boo.Lang.Compiler.Ast.Attribute assemblyAttribute in _assemblyAttributes)
			{
				_asmBuilder.SetCustomAttribute(GetCustomAttributeBuilder(assemblyAttribute));
			}
		}

		private CustomAttributeBuilder CreateDebuggableAttribute()
		{
			return new CustomAttributeBuilder(DebuggableAttribute_Constructor, new object[1] { DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations });
		}

		private CustomAttributeBuilder CreateRuntimeCompatibilityAttribute()
		{
			return new CustomAttributeBuilder(RuntimeCompatibilityAttribute_Constructor, new object[0], RuntimeCompatibilityAttribute_Property, new object[1] { true });
		}

		private CustomAttributeBuilder CreateUnverifiableCodeAttribute()
		{
			return new CustomAttributeBuilder(Methods.ConstructorOf(() => new UnverifiableCodeAttribute()), new object[0]);
		}

		private void DefineEntryPoint()
		{
			if (base.Context.Parameters.GenerateInMemory)
			{
				base.Context.GeneratedAssembly = _asmBuilder;
			}
			if (CompilerOutputType.Library != base.Parameters.OutputType)
			{
				Method entryPoint = ContextAnnotations.GetEntryPoint(base.Context);
				if (null != entryPoint)
				{
					MethodInfo entryMethod = (base.Context.Parameters.GenerateInMemory ? _asmBuilder.GetType(entryPoint.DeclaringType.FullName).GetMethod(entryPoint.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : GetMethodBuilder(entryPoint));
					_asmBuilder.SetEntryPoint(entryMethod, (PEFileKinds)base.Parameters.OutputType);
				}
				else
				{
					base.Errors.Add(CompilerErrorFactory.NoEntryPoint());
				}
			}
		}

		private Type[] GetParameterTypes(ParameterDeclarationCollection parameters)
		{
			Type[] array = new Type[parameters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetSystemType(parameters[i].Type);
				if (parameters[i].IsByRef && !array[i].IsByRef)
				{
					array[i] = array[i].MakeByRefType();
				}
			}
			return array;
		}

		private void SetBuilder(Node node, object builder)
		{
			if (null == builder)
			{
				throw new ArgumentNullException("type");
			}
			_builders[node] = builder;
		}

		private object GetBuilder(Node node)
		{
			return _builders[node];
		}

		internal TypeBuilder GetTypeBuilder(Node node)
		{
			return (TypeBuilder)_builders[node];
		}

		private PropertyBuilder GetPropertyBuilder(Node node)
		{
			return (PropertyBuilder)_builders[node];
		}

		private FieldBuilder GetFieldBuilder(Node node)
		{
			return (FieldBuilder)_builders[node];
		}

		private MethodBuilder GetMethodBuilder(Method method)
		{
			return (MethodBuilder)_builders[method];
		}

		private ConstructorBuilder GetConstructorBuilder(Method method)
		{
			return (ConstructorBuilder)_builders[method];
		}

		private LocalBuilder GetLocalBuilder(Node local)
		{
			return GetInternalLocal(local).LocalBuilder;
		}

		private PropertyInfo GetPropertyInfo(IEntity tag)
		{
			ExternalProperty externalProperty = tag as ExternalProperty;
			if (null != externalProperty)
			{
				return externalProperty.PropertyInfo;
			}
			return GetPropertyBuilder(((InternalProperty)tag).Property);
		}

		private FieldInfo GetFieldInfo(IField tag)
		{
			ExternalField externalField = tag as ExternalField;
			if (null != externalField)
			{
				return externalField.FieldInfo;
			}
			GenericMappedField genericMappedField = tag as GenericMappedField;
			if (genericMappedField != null)
			{
				return GetMappedFieldInfo(genericMappedField.DeclaringType, genericMappedField.SourceMember);
			}
			return GetFieldBuilder(((InternalField)tag).Field);
		}

		private MethodInfo GetMethodInfo(IMethod entity)
		{
			ExternalMethod externalMethod = entity as ExternalMethod;
			if (null != externalMethod)
			{
				return (MethodInfo)externalMethod.MethodInfo;
			}
			if (entity is GenericConstructedMethod)
			{
				return GetConstructedMethodInfo(entity.ConstructedInfo);
			}
			GenericMappedMethod genericMappedMethod = entity as GenericMappedMethod;
			if (genericMappedMethod != null)
			{
				return GetMappedMethodInfo(genericMappedMethod.DeclaringType, genericMappedMethod.SourceMember);
			}
			return GetMethodBuilder(((InternalMethod)entity).Method);
		}

		private ConstructorInfo GetConstructorInfo(IConstructor entity)
		{
			ExternalConstructor externalConstructor = entity as ExternalConstructor;
			if (null != externalConstructor)
			{
				return externalConstructor.ConstructorInfo;
			}
			GenericMappedConstructor genericMappedConstructor = entity as GenericMappedConstructor;
			if (genericMappedConstructor != null)
			{
				return TypeBuilder.GetConstructor(GetSystemType(genericMappedConstructor.DeclaringType), GetConstructorInfo((IConstructor)genericMappedConstructor.SourceMember));
			}
			return GetConstructorBuilder(((InternalMethod)entity).Method);
		}

		private MethodInfo GetConstructedMethodInfo(IConstructedMethodInfo constructedInfo)
		{
			Type[] typeArguments = Array.ConvertAll(constructedInfo.GenericArguments, GetSystemType);
			return GetMethodInfo(constructedInfo.GenericDefinition).MakeGenericMethod(typeArguments);
		}

		private FieldInfo GetMappedFieldInfo(IType targetType, IField source)
		{
			FieldInfo fieldInfo = GetFieldInfo(source);
			if (!fieldInfo.DeclaringType.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = fieldInfo.DeclaringType.GetGenericTypeDefinition();
				fieldInfo = genericTypeDefinition.GetField(fieldInfo.Name);
			}
			return TypeBuilder.GetField(GetSystemType(targetType), fieldInfo);
		}

		private MethodInfo GetMappedMethodInfo(IType targetType, IMethod source)
		{
			MethodInfo mi = GetMethodInfo(source);
			if (mi.DeclaringType.IsGenericTypeDefinition)
			{
				return GetConstructedMethodInfo(targetType, mi);
			}
			Type genericTypeDefinition = mi.DeclaringType.GetGenericTypeDefinition();
			MethodInfo mi2 = Array.Find(genericTypeDefinition.GetMethods(), (MethodInfo it) => it.MetadataToken == mi.MetadataToken);
			return GetConstructedMethodInfo(targetType, mi2);
		}

		private MethodInfo GetConstructedMethodInfo(IType targetType, MethodInfo mi)
		{
			return TypeBuilder.GetMethod(GetSystemType(targetType), mi);
		}

		private ConstructorInfo GetMappedConstructorInfo(IType targetType, IConstructor source)
		{
			ConstructorInfo ci = GetConstructorInfo(source);
			if (!ci.DeclaringType.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = ci.DeclaringType.GetGenericTypeDefinition();
				ci = Array.Find(genericTypeDefinition.GetConstructors(), (ConstructorInfo other) => other.MetadataToken == ci.MetadataToken);
			}
			return TypeBuilder.GetConstructor(GetSystemType(targetType), ci);
		}

		private Type GetSystemType(Node node)
		{
			return GetSystemType(GetType(node));
		}

		private Type GetSystemType(IType entity)
		{
			if (_typeCache.TryGetValue(entity, out var value))
			{
				return value;
			}
			Type type = SystemTypeFrom(entity);
			if (type == null)
			{
				throw new InvalidOperationException($"Could not find a Type for {entity}.");
			}
			_typeCache.Add(entity, type);
			return type;
		}

		private Type SystemTypeFrom(IType entity)
		{
			ExternalType externalType = entity as ExternalType;
			if (null != externalType)
			{
				return externalType.ActualType;
			}
			if (entity.IsArray)
			{
				IArrayType arrayType = (IArrayType)entity;
				Type systemType = GetSystemType(arrayType.ElementType);
				int rank = arrayType.Rank;
				if (rank == 1)
				{
					return systemType.MakeArrayType();
				}
				return systemType.MakeArrayType(rank);
			}
			if (entity.ConstructedInfo != null)
			{
				Type[] typeArguments = Array.ConvertAll(entity.ConstructedInfo.GenericArguments, GetSystemType);
				return GetSystemType(entity.ConstructedInfo.GenericDefinition).MakeGenericType(typeArguments);
			}
			if (entity.IsNull())
			{
				return Types.Object;
			}
			if (entity is InternalGenericParameter)
			{
				return (Type)GetBuilder(((InternalGenericParameter)entity).Node);
			}
			if (entity is AbstractInternalType)
			{
				TypeDefinition typeDefinition = ((AbstractInternalType)entity).TypeDefinition;
				Type type = (Type)GetBuilder(typeDefinition);
				if (entity.GenericInfo != null && !type.IsGenericType)
				{
					DefineGenericParameters(typeDefinition);
				}
				if (entity.IsPointer && null != type)
				{
					return type.MakePointerType();
				}
				return type;
			}
			return null;
		}

		private TypeAttributes GetNestedTypeAttributes(TypeMember type)
		{
			return GetExtendedTypeAttributes(GetNestedTypeAccessibility(type), type);
		}

		private TypeAttributes GetNestedTypeAccessibility(TypeMember type)
		{
			if (type.IsPublic)
			{
				return TypeAttributes.NestedPublic;
			}
			if (type.IsInternal)
			{
				return TypeAttributes.NestedAssembly;
			}
			return TypeAttributes.NestedPrivate;
		}

		private TypeAttributes GetTypeAttributes(TypeMember type)
		{
			return GetExtendedTypeAttributes(GetTypeVisibilityAttributes(type), type);
		}

		private TypeAttributes GetTypeVisibilityAttributes(TypeMember type)
		{
			return type.IsPublic ? TypeAttributes.Public : TypeAttributes.NotPublic;
		}

		private TypeAttributes GetExtendedTypeAttributes(TypeAttributes attributes, TypeMember type)
		{
			switch (type.NodeType)
			{
			case NodeType.ClassDefinition:
				attributes = attributes;
				attributes = attributes;
				if (!((ClassDefinition)type).HasDeclaredStaticConstructor)
				{
					attributes |= TypeAttributes.BeforeFieldInit;
				}
				if (type.IsAbstract)
				{
					attributes |= TypeAttributes.Abstract;
				}
				if (type.IsFinal)
				{
					attributes |= TypeAttributes.Sealed;
				}
				if (type.IsStatic)
				{
					attributes |= TypeAttributes.Abstract | TypeAttributes.Sealed;
				}
				else if (!type.IsTransient)
				{
					attributes |= TypeAttributes.Serializable;
				}
				if (((IType)type.Entity).IsValueType)
				{
					attributes |= TypeAttributes.SequentialLayout;
				}
				break;
			case NodeType.EnumDefinition:
				attributes |= TypeAttributes.Sealed;
				attributes |= TypeAttributes.Serializable;
				break;
			case NodeType.InterfaceDefinition:
				attributes |= TypeAttributes.ClassSemanticsMask | TypeAttributes.Abstract;
				break;
			case NodeType.Module:
				attributes |= TypeAttributes.Sealed;
				break;
			}
			return attributes;
		}

		private PropertyAttributes PropertyAttributesFor(Property property)
		{
			return (property.ExplicitInfo != null) ? (PropertyAttributes.SpecialName | PropertyAttributes.RTSpecialName) : PropertyAttributes.None;
		}

		private MethodAttributes MethodAttributesFor(TypeMember member)
		{
			MethodAttributes methodAttributes = MethodVisibilityAttributesFor(member);
			if (member.IsStatic)
			{
				methodAttributes |= MethodAttributes.Static;
				if (member.Name.StartsWith("op_"))
				{
					methodAttributes |= MethodAttributes.SpecialName;
				}
			}
			else if (member.IsAbstract)
			{
				methodAttributes |= MethodAttributes.Virtual | MethodAttributes.Abstract;
			}
			else if (member.IsVirtual || member.IsOverride)
			{
				methodAttributes |= MethodAttributes.Virtual;
				if (member.IsFinal)
				{
					methodAttributes |= MethodAttributes.Final;
				}
				if (member.IsNew)
				{
					methodAttributes |= MethodAttributes.VtableLayoutMask;
				}
			}
			return methodAttributes;
		}

		private static MethodAttributes MethodVisibilityAttributesFor(TypeMember member)
		{
			if (member.IsPublic)
			{
				return MethodAttributes.Public;
			}
			if (member.IsProtected)
			{
				return member.IsInternal ? MethodAttributes.FamORAssem : MethodAttributes.Family;
			}
			if (member.IsInternal)
			{
				return MethodAttributes.Assembly;
			}
			return MethodAttributes.Private;
		}

		private MethodAttributes PropertyAccessorAttributesFor(TypeMember property)
		{
			return MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributesFor(property);
		}

		private MethodAttributes GetMethodAttributes(Method method)
		{
			MethodAttributes methodAttributes = MethodAttributes.HideBySig;
			if (method.ExplicitInfo != null)
			{
				methodAttributes |= MethodAttributes.VtableLayoutMask;
			}
			if (IsPInvoke(method))
			{
				Debug.Assert(method.IsStatic);
				methodAttributes |= MethodAttributes.PinvokeImpl;
			}
			return methodAttributes | MethodAttributesFor(method);
		}

		private static FieldAttributes FieldAttributesFor(Field field)
		{
			FieldAttributes fieldAttributes = FieldVisibilityAttributeFor(field);
			if (field.IsStatic)
			{
				fieldAttributes |= FieldAttributes.Static;
			}
			if (field.IsTransient)
			{
				fieldAttributes |= FieldAttributes.NotSerialized;
			}
			if (field.IsFinal)
			{
				IField field2 = (IField)field.Entity;
				fieldAttributes = ((!field2.IsLiteral) ? (fieldAttributes | FieldAttributes.InitOnly) : (fieldAttributes | FieldAttributes.Literal));
			}
			return fieldAttributes;
		}

		private static FieldAttributes FieldVisibilityAttributeFor(Field field)
		{
			if (field.IsProtected)
			{
				return FieldAttributes.Family;
			}
			if (field.IsPublic)
			{
				return FieldAttributes.Public;
			}
			if (field.IsInternal)
			{
				return FieldAttributes.Assembly;
			}
			return FieldAttributes.Private;
		}

		private static Type[] GetFieldRequiredCustomModifiers(Field field)
		{
			if (field.IsVolatile)
			{
				return new Type[1] { IsVolatileType };
			}
			return Type.EmptyTypes;
		}

		private ParameterAttributes GetParameterAttributes(ParameterDeclaration param)
		{
			return ParameterAttributes.None;
		}

		private void DefineEvent(TypeBuilder typeBuilder, Event node)
		{
			EventBuilder eventBuilder = typeBuilder.DefineEvent(node.Name, EventAttributes.None, GetSystemType(node.Type));
			eventBuilder.SetAddOnMethod(DefineEventMethod(typeBuilder, node.Add));
			eventBuilder.SetRemoveOnMethod(DefineEventMethod(typeBuilder, node.Remove));
			if (node.Raise != null)
			{
				eventBuilder.SetRaiseMethod(DefineEventMethod(typeBuilder, node.Raise));
			}
			SetBuilder(node, eventBuilder);
		}

		private MethodBuilder DefineEventMethod(TypeBuilder typeBuilder, Method method)
		{
			return DefineMethod(typeBuilder, method, MethodAttributes.SpecialName | GetMethodAttributes(method));
		}

		private void DefineProperty(TypeBuilder typeBuilder, Property property)
		{
			string name = ((property.ExplicitInfo != null) ? (property.ExplicitInfo.InterfaceType.Name + "." + property.Name) : property.Name);
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributesFor(property), GetSystemType(property.Type), GetParameterTypes(property.Parameters));
			Method getter = property.Getter;
			if (getter != null)
			{
				propertyBuilder.SetGetMethod(DefinePropertyAccessor(typeBuilder, property, getter));
			}
			Method setter = property.Setter;
			if (setter != null)
			{
				propertyBuilder.SetSetMethod(DefinePropertyAccessor(typeBuilder, property, setter));
			}
			if (GetEntity(property).IsDuckTyped)
			{
				propertyBuilder.SetCustomAttribute(CreateDuckTypedCustomAttribute());
			}
			SetBuilder(property, propertyBuilder);
		}

		private MethodBuilder DefinePropertyAccessor(TypeBuilder typeBuilder, Property property, Method accessor)
		{
			if (!accessor.IsVisibilitySet)
			{
				accessor.Visibility = property.Visibility;
			}
			return DefineMethod(typeBuilder, accessor, PropertyAccessorAttributesFor(accessor));
		}

		private void DefineField(TypeBuilder typeBuilder, Field field)
		{
			FieldBuilder builder = typeBuilder.DefineField(field.Name, GetSystemType(field), GetFieldRequiredCustomModifiers(field), Type.EmptyTypes, FieldAttributesFor(field));
			SetBuilder(field, builder);
		}

		private void DefineParameters(ConstructorBuilder builder, ParameterDeclarationCollection parameters)
		{
			DefineParameters(parameters, builder.DefineParameter);
		}

		private void DefineParameters(MethodBuilder builder, ParameterDeclarationCollection parameters)
		{
			DefineParameters(parameters, builder.DefineParameter);
		}

		private void DefineParameters(ParameterDeclarationCollection parameters, ParameterFactory defineParameter)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterBuilder parameterBuilder = defineParameter(i + 1, GetParameterAttributes(parameters[i]), parameters[i].Name);
				if (parameters[i].IsParamArray)
				{
					SetParamArrayAttribute(parameterBuilder);
				}
				SetBuilder(parameters[i], parameterBuilder);
			}
		}

		private void SetParamArrayAttribute(ParameterBuilder builder)
		{
			builder.SetCustomAttribute(new CustomAttributeBuilder(ParamArrayAttribute_Constructor, new object[0]));
		}

		private static MethodImplAttributes ImplementationFlagsFor(Method method)
		{
			return method.IsRuntime ? MethodImplAttributes.CodeTypeMask : MethodImplAttributes.IL;
		}

		private MethodBuilder DefineMethod(TypeBuilder typeBuilder, Method method, MethodAttributes attributes)
		{
			ParameterDeclarationCollection parameters = method.Parameters;
			MethodAttributes attributes2 = GetMethodAttributes(method) | attributes;
			string name = ((method.ExplicitInfo == null) ? method.Name : (method.ExplicitInfo.InterfaceType.Name + "." + method.Name));
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(name, attributes2);
			if (method.GenericParameters.Count != 0)
			{
				DefineGenericParameters(methodBuilder, method.GenericParameters.ToArray());
			}
			methodBuilder.SetParameters(GetParameterTypes(parameters));
			IType type = GetEntity(method).ReturnType;
			if (IsPInvoke(method) && TypeSystemServices.IsUnknown(type))
			{
				type = base.TypeSystemServices.VoidType;
			}
			methodBuilder.SetReturnType(GetSystemType(type));
			methodBuilder.SetImplementationFlags(ImplementationFlagsFor(method));
			DefineParameters(methodBuilder, parameters);
			SetBuilder(method, methodBuilder);
			IMethod entity = GetEntity(method);
			if (entity.IsDuckTyped)
			{
				methodBuilder.SetCustomAttribute(CreateDuckTypedCustomAttribute());
			}
			return methodBuilder;
		}

		private void DefineGenericParameters(TypeDefinition typeDefinition)
		{
			if (!(typeDefinition is EnumDefinition))
			{
				TypeBuilder typeBuilder = GetTypeBuilder(typeDefinition);
				if (!typeBuilder.IsGenericType && typeDefinition.GenericParameters.Count > 0)
				{
					DefineGenericParameters(typeBuilder, typeDefinition.GenericParameters.ToArray());
				}
			}
		}

		private void DefineGenericParameters(TypeBuilder builder, GenericParameterDeclaration[] parameters)
		{
			string[] names = Array.ConvertAll(parameters, (GenericParameterDeclaration gpd) => gpd.Name);
			GenericTypeParameterBuilder[] builders = builder.DefineGenericParameters(names);
			DefineGenericParameters(builders, parameters);
		}

		private void DefineGenericParameters(MethodBuilder builder, GenericParameterDeclaration[] parameters)
		{
			string[] names = Array.ConvertAll(parameters, (GenericParameterDeclaration gpd) => gpd.Name);
			GenericTypeParameterBuilder[] builders = builder.DefineGenericParameters(names);
			DefineGenericParameters(builders, parameters);
		}

		private void DefineGenericParameters(GenericTypeParameterBuilder[] builders, GenericParameterDeclaration[] declarations)
		{
			for (int i = 0; i < builders.Length; i++)
			{
				SetBuilder(declarations[i], builders[i]);
				DefineGenericParameter((InternalGenericParameter)declarations[i].Entity, builders[i]);
			}
		}

		private void DefineGenericParameter(InternalGenericParameter parameter, GenericTypeParameterBuilder builder)
		{
			if (parameter.BaseType != base.TypeSystemServices.ObjectType)
			{
				builder.SetBaseTypeConstraint(GetSystemType(parameter.BaseType));
			}
			Type[] interfaceConstraints = Array.ConvertAll(parameter.GetInterfaces(), GetSystemType);
			builder.SetInterfaceConstraints(interfaceConstraints);
			GenericParameterAttributes genericParameterAttributes = GenericParameterAttributes.None;
			if (parameter.IsClass)
			{
				genericParameterAttributes |= GenericParameterAttributes.ReferenceTypeConstraint;
			}
			if (parameter.IsValueType)
			{
				genericParameterAttributes |= GenericParameterAttributes.NotNullableValueTypeConstraint;
			}
			if (parameter.MustHaveDefaultConstructor)
			{
				genericParameterAttributes |= GenericParameterAttributes.DefaultConstructorConstraint;
			}
			builder.SetGenericParameterAttributes(genericParameterAttributes);
		}

		private CustomAttributeBuilder CreateDuckTypedCustomAttribute()
		{
			return new CustomAttributeBuilder(DuckTypedAttribute_Constructor, new object[0]);
		}

		private void DefineConstructor(TypeBuilder typeBuilder, Method constructor)
		{
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(GetMethodAttributes(constructor), CallingConventions.Standard, GetParameterTypes(constructor.Parameters));
			constructorBuilder.SetImplementationFlags(ImplementationFlagsFor(constructor));
			DefineParameters(constructorBuilder, constructor.Parameters);
			SetBuilder(constructor, constructorBuilder);
		}

		private bool IsEnumDefinition(TypeMember type)
		{
			return NodeType.EnumDefinition == type.NodeType;
		}

		private void DefineType(TypeDefinition typeDefinition)
		{
			SetBuilder(typeDefinition, CreateTypeBuilder(typeDefinition));
		}

		private bool IsValueType(TypeMember type)
		{
			return (type.Entity as IType)?.IsValueType ?? false;
		}

		private InternalLocal GetInternalLocal(Node local)
		{
			return (InternalLocal)GetEntity(local);
		}

		private object CreateTypeBuilder(TypeDefinition type)
		{
			Type parent = null;
			if (IsEnumDefinition(type))
			{
				parent = typeof(Enum);
			}
			else if (IsValueType(type))
			{
				parent = Types.ValueType;
			}
			TypeBuilder typeBuilder = null;
			ClassDefinition classDefinition = type.ParentNode as ClassDefinition;
			EnumDefinition node = type as EnumDefinition;
			typeBuilder = ((null != classDefinition) ? GetTypeBuilder(classDefinition).DefineNestedType(AnnotateGenericTypeName(type, type.Name), GetNestedTypeAttributes(type), parent) : _moduleBuilder.DefineType(AnnotateGenericTypeName(type, type.QualifiedName), GetTypeAttributes(type), parent));
			if (IsEnumDefinition(type))
			{
				typeBuilder.DefineField("value__", GetEnumUnderlyingType(node), FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
			}
			return typeBuilder;
		}

		private string AnnotateGenericTypeName(TypeDefinition typeDef, string name)
		{
			if (typeDef.HasGenericParameters)
			{
				return name + "`" + typeDef.GenericParameters.Count;
			}
			return name;
		}

		private void EmitBaseTypesAndAttributes(TypeDefinition typeDefinition, TypeBuilder typeBuilder)
		{
			foreach (TypeReference baseType in typeDefinition.BaseTypes)
			{
				Type systemType = GetSystemType(baseType);
				if ((systemType.IsGenericType && systemType.GetGenericTypeDefinition().IsClass) || systemType.IsClass)
				{
					typeBuilder.SetParent(systemType);
				}
				else
				{
					typeBuilder.AddInterfaceImplementation(systemType);
				}
			}
		}

		private void NotImplemented(string feature)
		{
			throw new NotImplementedException(feature);
		}

		private CustomAttributeBuilder GetCustomAttributeBuilder(Boo.Lang.Compiler.Ast.Attribute node)
		{
			IConstructor constructor = (IConstructor)GetEntity(node);
			ConstructorInfo constructorInfo = GetConstructorInfo(constructor);
			object[] constructorArgs = ArgumentsForAttributeConstructor(constructor, node.Arguments);
			ExpressionPairCollection namedArguments = node.NamedArguments;
			if (namedArguments.Count > 0)
			{
				GetNamedValues(namedArguments, out var outNamedProperties, out var outPropertyValues, out var outNamedFields, out var outFieldValues);
				return new CustomAttributeBuilder(constructorInfo, constructorArgs, outNamedProperties, outPropertyValues, outNamedFields, outFieldValues);
			}
			return new CustomAttributeBuilder(constructorInfo, constructorArgs);
		}

		private void GetNamedValues(ExpressionPairCollection values, out PropertyInfo[] outNamedProperties, out object[] outPropertyValues, out FieldInfo[] outNamedFields, out object[] outFieldValues)
		{
			List list = new List();
			List list2 = new List();
			List list3 = new List();
			List list4 = new List();
			foreach (ExpressionPair value2 in values)
			{
				ITypedEntity typedEntity = (ITypedEntity)GetEntity(value2.First);
				object value = GetValue(typedEntity.Type, value2.Second);
				if (EntityType.Property == typedEntity.EntityType)
				{
					list.Add(GetPropertyInfo(typedEntity));
					list2.Add(value);
				}
				else
				{
					list3.Add(GetFieldInfo((IField)typedEntity));
					list4.Add(value);
				}
			}
			outNamedProperties = (PropertyInfo[])list.ToArray(typeof(PropertyInfo));
			outPropertyValues = list2.ToArray();
			outNamedFields = (FieldInfo[])list3.ToArray(typeof(FieldInfo));
			outFieldValues = list4.ToArray();
		}

		private object[] ArgumentsForAttributeConstructor(IConstructor ctor, ExpressionCollection args)
		{
			bool acceptVarArgs = ctor.AcceptVarArgs;
			IParameter[] parameters = ctor.GetParameters();
			object[] array = new object[parameters.Length];
			int num = parameters.Length - 1;
			IEnumerable<IParameter> enumerable = (acceptVarArgs ? parameters.Take(num) : parameters);
			int num2 = 0;
			foreach (IParameter item in enumerable)
			{
				array[num2] = GetValue(item.Type, args[num2]);
				num2++;
			}
			if (acceptVarArgs)
			{
				IType varArgType = parameters[num].Type.ElementType;
				array[num] = (from e in args.Skip(num)
					select GetValue(varArgType, e)).ToArray();
			}
			return array;
		}

		private object GetValue(IType expectedType, Expression expression)
		{
			return expression.NodeType switch
			{
				NodeType.NullLiteralExpression => null, 
				NodeType.StringLiteralExpression => ((StringLiteralExpression)expression).Value, 
				NodeType.CharLiteralExpression => ((CharLiteralExpression)expression).Value[0], 
				NodeType.BoolLiteralExpression => ((BoolLiteralExpression)expression).Value, 
				NodeType.IntegerLiteralExpression => ConvertValue(expectedType, ((IntegerLiteralExpression)expression).Value), 
				NodeType.DoubleLiteralExpression => ConvertValue(expectedType, ((DoubleLiteralExpression)expression).Value), 
				NodeType.TypeofExpression => GetSystemType(((TypeofExpression)expression).Type), 
				NodeType.CastExpression => GetValue(expectedType, ((CastExpression)expression).Target), 
				_ => GetComplexExpressionValue(expectedType, expression), 
			};
		}

		private object GetComplexExpressionValue(IType expectedType, Expression expression)
		{
			IEntity entity = GetEntity(expression);
			if (EntityType.Type == entity.EntityType)
			{
				return GetSystemType(expression);
			}
			if (EntityType.Field == entity.EntityType)
			{
				IField field = (IField)entity;
				if (field.IsLiteral)
				{
					if (field.StaticValue is Expression)
					{
						return GetValue(expectedType, field.StaticValue as Expression);
					}
					return field.StaticValue;
				}
			}
			NotImplemented(expression, "Expression value: " + expression);
			return null;
		}

		private object ConvertValue(IType expectedType, object value)
		{
			if (expectedType.IsEnum)
			{
				return Convert.ChangeType(value, GetEnumUnderlyingType(expectedType));
			}
			return Convert.ChangeType(value, GetSystemType(expectedType));
		}

		private Type GetEnumUnderlyingType(EnumDefinition node)
		{
			return ((InternalEnum)node.Entity).UnderlyingType;
		}

		private Type GetEnumUnderlyingType(IType enumType)
		{
			return (enumType is IInternalEntity) ? ((InternalEnum)enumType).UnderlyingType : Enum.GetUnderlyingType(GetSystemType(enumType));
		}

		private void DefineTypeMembers(TypeDefinition typeDefinition)
		{
			if (IsEnumDefinition(typeDefinition))
			{
				return;
			}
			TypeBuilder typeBuilder = GetTypeBuilder(typeDefinition);
			TypeMemberCollection members = typeDefinition.Members;
			foreach (TypeMember item in members)
			{
				switch (item.NodeType)
				{
				case NodeType.Method:
					DefineMethod(typeBuilder, (Method)item, MethodAttributes.PrivateScope);
					break;
				case NodeType.Constructor:
					DefineConstructor(typeBuilder, (Constructor)item);
					break;
				case NodeType.Field:
					DefineField(typeBuilder, (Field)item);
					break;
				case NodeType.Property:
					DefineProperty(typeBuilder, (Property)item);
					break;
				case NodeType.Event:
					DefineEvent(typeBuilder, (Event)item);
					break;
				}
			}
		}

		private string GetAssemblySimpleName(string fname)
		{
			return Path.GetFileNameWithoutExtension(fname);
		}

		private string GetTargetDirectory(string fname)
		{
			return Permissions.WithDiscoveryPermission(() => Path.GetDirectoryName(Path.GetFullPath(fname)));
		}

		private string BuildOutputAssemblyName()
		{
			string outputAssembly = base.Parameters.OutputAssembly;
			if (!string.IsNullOrEmpty(outputAssembly))
			{
				return TryToGetFullPath(outputAssembly);
			}
			string text = base.CompileUnit.Modules[0].Name;
			if (!HasDllOrExeExtension(text))
			{
				text = ((CompilerOutputType.Library != base.Parameters.OutputType) ? (text + ".exe") : (text + ".dll"));
			}
			return TryToGetFullPath(text);
		}

		private string TryToGetFullPath(string path)
		{
			return Permissions.WithDiscoveryPermission(() => Path.GetFullPath(path)) ?? path;
		}

		private bool HasDllOrExeExtension(string fname)
		{
			string extension = Path.GetExtension(fname);
			switch (extension.ToLower())
			{
			case ".dll":
			case ".exe":
				return true;
			default:
				return false;
			}
		}

		private void DefineResources()
		{
			foreach (ICompilerResource resource in base.Parameters.Resources)
			{
				resource.WriteResource(_sreResourceService);
			}
		}

		private void SetUpAssembly()
		{
			string text = BuildOutputAssemblyName();
			AssemblyName assemblyName = CreateAssemblyName(text);
			AssemblyBuilderAccess assemblyBuilderAccess = GetAssemblyBuilderAccess();
			string targetDirectory = GetTargetDirectory(text);
			_asmBuilder = (string.IsNullOrEmpty(targetDirectory) ? AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, assemblyBuilderAccess) : AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, assemblyBuilderAccess, targetDirectory));
			if (base.Parameters.Debug)
			{
				_asmBuilder.SetCustomAttribute(CreateDebuggableAttribute());
			}
			_asmBuilder.SetCustomAttribute(CreateRuntimeCompatibilityAttribute());
			_moduleBuilder = _asmBuilder.DefineDynamicModule(assemblyName.Name, Path.GetFileName(text), base.Parameters.Debug);
			if (base.Parameters.Unsafe)
			{
				_moduleBuilder.SetCustomAttribute(CreateUnverifiableCodeAttribute());
			}
			_sreResourceService = new SREResourceService(_asmBuilder, _moduleBuilder);
			ContextAnnotations.SetAssemblyBuilder(base.Context, _asmBuilder);
			base.Context.GeneratedAssemblyFileName = text;
		}

		private AssemblyBuilderAccess GetAssemblyBuilderAccess()
		{
			return base.Parameters.GenerateInMemory ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Save;
		}

		private AssemblyName CreateAssemblyName(string outputFile)
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = GetAssemblySimpleName(outputFile);
			assemblyName.Version = GetAssemblyVersion();
			if (base.Parameters.DelaySign)
			{
				assemblyName.SetPublicKey(GetAssemblyKeyPair(outputFile).PublicKey);
			}
			else
			{
				assemblyName.KeyPair = GetAssemblyKeyPair(outputFile);
			}
			return assemblyName;
		}

		private StrongNameKeyPair GetAssemblyKeyPair(string outputFile)
		{
			Boo.Lang.Compiler.Ast.Attribute assemblyAttribute = GetAssemblyAttribute("System.Reflection.AssemblyKeyNameAttribute");
			if (base.Parameters.KeyContainer != null)
			{
				if (assemblyAttribute != null)
				{
					base.Warnings.Add(CompilerWarningFactory.HaveBothKeyNameAndAttribute(assemblyAttribute));
				}
				if (base.Parameters.KeyContainer.Length != 0)
				{
					return new StrongNameKeyPair(base.Parameters.KeyContainer);
				}
			}
			else if (assemblyAttribute != null)
			{
				string value = ((StringLiteralExpression)assemblyAttribute.Arguments[0]).Value;
				if (value.Length != 0)
				{
					return new StrongNameKeyPair(value);
				}
			}
			string text = null;
			string srcFile = null;
			assemblyAttribute = GetAssemblyAttribute("System.Reflection.AssemblyKeyFileAttribute");
			if (base.Parameters.KeyFile != null)
			{
				text = base.Parameters.KeyFile;
				if (assemblyAttribute != null)
				{
					base.Warnings.Add(CompilerWarningFactory.HaveBothKeyFileAndAttribute(assemblyAttribute));
				}
			}
			else if (assemblyAttribute != null)
			{
				text = ((StringLiteralExpression)assemblyAttribute.Arguments[0]).Value;
				if (assemblyAttribute.LexicalInfo != null)
				{
					srcFile = assemblyAttribute.LexicalInfo.FileName;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (!Path.IsPathRooted(text))
				{
					text = ResolveRelative(outputFile, srcFile, text);
				}
				using FileStream keyPairFile = File.OpenRead(text);
				return new StrongNameKeyPair(keyPairFile);
			}
			return null;
		}

		private string ResolveRelative(string targetFile, string srcFile, string relativeFile)
		{
			string text = Path.GetFullPath(relativeFile);
			if (File.Exists(text))
			{
				return text;
			}
			if (srcFile != null)
			{
				text = ResolveRelativePath(srcFile, relativeFile);
				if (File.Exists(text))
				{
					return text;
				}
			}
			if (targetFile != null)
			{
				return ResolveRelativePath(targetFile, relativeFile);
			}
			return text;
		}

		private string ResolveRelativePath(string srcFile, string relativeFile)
		{
			return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(srcFile), relativeFile));
		}

		private Version GetAssemblyVersion()
		{
			string text = GetAssemblyAttributeValue("System.Reflection.AssemblyVersionAttribute");
			if (text == null)
			{
				return new Version();
			}
			string[] array = text.Split('.');
			if (array.Length > 2)
			{
				DateTime dateTime = new DateTime(2000, 1, 1);
				TimeSpan timeSpan = DateTime.Now - dateTime;
				if (array[2].StartsWith("*"))
				{
					array[2] = Math.Round(timeSpan.TotalDays).ToString();
				}
				if (array.Length > 3 && array[3].StartsWith("*"))
				{
					array[3] = Math.Round(timeSpan.TotalSeconds).ToString();
				}
				text = string.Join(".", array);
			}
			return new Version(text);
		}

		private string GetAssemblyAttributeValue(string name)
		{
			Boo.Lang.Compiler.Ast.Attribute assemblyAttribute = GetAssemblyAttribute(name);
			if (null != assemblyAttribute)
			{
				return ((StringLiteralExpression)assemblyAttribute.Arguments[0]).Value;
			}
			return null;
		}

		private Boo.Lang.Compiler.Ast.Attribute GetAssemblyAttribute(string name)
		{
			return _assemblyAttributes.Get(name).FirstOrDefault();
		}

		protected override IType GetExpressionType(Expression node)
		{
			IType expressionType = base.GetExpressionType(node);
			if (TypeSystemServices.IsUnknown(expressionType))
			{
				throw CompilerErrorFactory.InvalidNode(node);
			}
			return expressionType;
		}

		private static MethodInfo GetNullableHasValue(Type type)
		{
			if (_Nullable_HasValue.TryGetValue(type, out var value))
			{
				return value;
			}
			value = Types.Nullable.MakeGenericType(type).GetProperty("HasValue").GetGetMethod();
			_Nullable_HasValue.Add(type, value);
			return value;
		}
	}
}
