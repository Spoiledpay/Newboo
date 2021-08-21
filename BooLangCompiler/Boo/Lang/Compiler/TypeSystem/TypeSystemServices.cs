using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class TypeSystemServices
	{
		public static readonly IType ErrorEntity = Error.Default;

		public IType ArrayType;

		public IType BoolType;

		public IType BuiltinsType;

		public IType CharType;

		public IType ConditionalAttribute;

		public IType DateTimeType;

		public IType DecimalType;

		public IType DelegateType;

		public IType DoubleType;

		public IType DuckType;

		public IType EnumType;

		public IType HashType;

		public IType IAstGeneratorMacroType;

		public IType IAstMacroType;

		public IType ICallableType;

		public IType ICollectionGenericType;

		public IType ICollectionType;

		public IType IDisposableType;

		public IType IEnumerableGenericType;

		public IType IEnumerableType;

		public IType IListGenericType;

		public IType IListType;

		public IType IEnumeratorGenericType;

		public IType IEnumeratorType;

		public IType IQuackFuType;

		public IType SByteType;

		public IType ShortType;

		public IType IntType;

		public IType IntPtrType;

		public IType LongType;

		public IType ByteType;

		public IType UShortType;

		public IType UIntType;

		public IType UIntPtrType;

		public IType ULongType;

		public IType ListType;

		public IType MulticastDelegateType;

		public IArrayType ObjectArrayType;

		public IType ObjectType;

		public IType RegexType;

		public IType RuntimeServicesType;

		public IType SingleType;

		public IType StringType;

		public IType SystemAttribute;

		public IType TimeSpanType;

		public IType TypeType;

		public IType ValueTypeType;

		public IType VoidType;

		private Boo.Lang.Compiler.Ast.Module _compilerGeneratedTypesModule;

		private readonly Set<string> _literalPrimitives = new Set<string>();

		private readonly Dictionary<string, IEntity> _primitives = new Dictionary<string, IEntity>(StringComparer.Ordinal);

		private DowncastPermissions _downcastPermissions;

		private readonly MemoizedFunction<IType, IType, IMethod> _findImplicitConversionOperator;

		private readonly MemoizedFunction<IType, IType, IMethod> _findExplicitConversionOperator;

		private readonly MemoizedFunction<IType, IType, bool> _canBeReachedByPromotion;

		private readonly AnonymousCallablesManager _anonymousCallablesManager;

		private readonly CompilerContext _context;

		private EnvironmentProvision<NameResolutionService> _nameResolutionService = default(EnvironmentProvision<NameResolutionService>);

		private EnvironmentProvision<IReflectionTypeSystemProvider> _typeSystemProvider;

		public CompilerContext Context => _context;

		public BooCodeBuilder CodeBuilder => _context.CodeBuilder;

		public virtual IType ExceptionType => Map(typeof(Exception));

		protected NameResolutionService NameResolutionService => _nameResolutionService;

		public TypeSystemServices()
			: this(CompilerContext.Current)
		{
		}

		public TypeSystemServices(CompilerContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			_context = context;
			_anonymousCallablesManager = new AnonymousCallablesManager(this);
			_findImplicitConversionOperator = new MemoizedFunction<IType, IType, IMethod>((IType fromType, IType toType) => FindConversionOperator("op_Implicit", fromType, toType));
			_findExplicitConversionOperator = new MemoizedFunction<IType, IType, IMethod>((IType fromType, IType toType) => FindConversionOperator("op_Explicit", fromType, toType));
			My<CurrentScope>.Instance.Changed += delegate
			{
				ClearScopeDependentMemoizedFunctions();
			};
			_canBeReachedByPromotion = new MemoizedFunction<IType, IType, bool>(CanBeReachedByPromotionImpl);
			DuckType = Map(typeof(Builtins.duck));
			IQuackFuType = Map(typeof(IQuackFu));
			VoidType = Map(Types.Void);
			ObjectType = Map(Types.Object);
			RegexType = Map(Types.Regex);
			ValueTypeType = Map(typeof(ValueType));
			EnumType = Map(typeof(Enum));
			ArrayType = Map(Types.Array);
			TypeType = Map(Types.Type);
			StringType = Map(Types.String);
			BoolType = Map(Types.Bool);
			SByteType = Map(Types.SByte);
			CharType = Map(Types.Char);
			ShortType = Map(Types.Short);
			IntType = Map(Types.Int);
			LongType = Map(Types.Long);
			ByteType = Map(Types.Byte);
			UShortType = Map(Types.UShort);
			UIntType = Map(Types.UInt);
			ULongType = Map(Types.ULong);
			SingleType = Map(Types.Single);
			DoubleType = Map(Types.Double);
			DecimalType = Map(Types.Decimal);
			TimeSpanType = Map(Types.TimeSpan);
			DateTimeType = Map(Types.DateTime);
			RuntimeServicesType = Map(Types.RuntimeServices);
			BuiltinsType = Map(Types.Builtins);
			ListType = Map(Types.List);
			HashType = Map(Types.Hash);
			ICallableType = Map(Types.ICallable);
			IEnumerableType = Map(Types.IEnumerable);
			IEnumeratorType = Map(typeof(IEnumerator));
			ICollectionType = Map(Types.ICollection);
			IDisposableType = Map(typeof(IDisposable));
			IntPtrType = Map(Types.IntPtr);
			UIntPtrType = Map(Types.UIntPtr);
			MulticastDelegateType = Map(Types.MulticastDelegate);
			DelegateType = Map(Types.Delegate);
			SystemAttribute = Map(typeof(System.Attribute));
			ConditionalAttribute = Map(typeof(ConditionalAttribute));
			IEnumerableGenericType = Map(Types.IEnumerableGeneric);
			IEnumeratorGenericType = Map(typeof(IEnumerator<>));
			ICollectionGenericType = Map(typeof(ICollection<>));
			IListGenericType = Map(typeof(IList<>));
			IListType = Map(typeof(IList));
			IAstMacroType = Map(typeof(IAstMacro));
			IAstGeneratorMacroType = Map(typeof(IAstGeneratorMacro));
			ObjectArrayType = ObjectType.MakeArrayType(1);
			PreparePrimitives();
			PrepareBuiltinFunctions();
		}

		private void ClearScopeDependentMemoizedFunctions()
		{
			_findImplicitConversionOperator.Clear();
			_findExplicitConversionOperator.Clear();
		}

		public bool IsGenericGeneratorReturnType(IType returnType)
		{
			return returnType.ConstructedInfo != null && (returnType.ConstructedInfo.GenericDefinition == IEnumerableGenericType || returnType.ConstructedInfo.GenericDefinition == IEnumeratorGenericType);
		}

		public IType GetMostGenericType(ExpressionCollection args)
		{
			IType type = GetConcreteExpressionType(args[0]);
			for (int i = 1; i < args.Count; i++)
			{
				IType concreteExpressionType = GetConcreteExpressionType(args[i]);
				if (type != concreteExpressionType)
				{
					type = GetMostGenericType(type, concreteExpressionType);
					if (IsSystemObject(type))
					{
						break;
					}
				}
			}
			return type;
		}

		public IType GetMostGenericType(IType current, IType candidate)
		{
			if (current == null && null == candidate)
			{
				throw new ArgumentNullException("current", "Both 'current' and 'candidate' are null");
			}
			if (null == current)
			{
				return candidate;
			}
			if (null == candidate)
			{
				return current;
			}
			if (IsAssignableFrom(current, candidate))
			{
				return current;
			}
			if (IsAssignableFrom(candidate, current))
			{
				return candidate;
			}
			if (IsNumberOrBool(current) && IsNumberOrBool(candidate))
			{
				return GetPromotedNumberType(current, candidate);
			}
			if (IsCallableType(current) && IsCallableType(candidate))
			{
				return ICallableType;
			}
			if (current.IsClass && candidate.IsClass)
			{
				if (current == ObjectType || candidate == ObjectType)
				{
					return ObjectType;
				}
				if (current.GetTypeDepth() < candidate.GetTypeDepth())
				{
					return GetMostGenericType(current.BaseType, candidate);
				}
				return GetMostGenericType(current, candidate.BaseType);
			}
			return ObjectType;
		}

		public IType GetPromotedNumberType(IType left, IType right)
		{
			if (left == DecimalType || right == DecimalType)
			{
				return DecimalType;
			}
			if (left == DoubleType || right == DoubleType)
			{
				return DoubleType;
			}
			if (left == SingleType || right == SingleType)
			{
				return SingleType;
			}
			if (left == ULongType)
			{
				if (IsSignedInteger(right))
				{
					return LongType;
				}
				return ULongType;
			}
			if (right == ULongType)
			{
				if (IsSignedInteger(left))
				{
					return LongType;
				}
				return ULongType;
			}
			if (left == LongType || right == LongType)
			{
				return LongType;
			}
			if (left == UIntType)
			{
				if (right == SByteType || right == ShortType || right == IntType)
				{
					return LongType;
				}
				return UIntType;
			}
			if (right == UIntType)
			{
				if (left == SByteType || left == ShortType || left == IntType)
				{
					return LongType;
				}
				return UIntType;
			}
			if (left == IntType || right == IntType || left == ShortType || right == ShortType || left == UShortType || right == UShortType || left == ByteType || right == ByteType || left == SByteType || right == SByteType)
			{
				return IntType;
			}
			return left;
		}

		private bool IsSignedInteger(IType right)
		{
			return right == SByteType || right == ShortType || right == IntType || right == LongType;
		}

		public static bool IsReadOnlyField(IField field)
		{
			return field.IsInitOnly || field.IsLiteral;
		}

		public bool IsCallable(IType type)
		{
			return TypeType == type || IsCallableType(type) || IsDuckType(type);
		}

		public virtual bool IsDuckTyped(Expression expression)
		{
			IType expressionType = expression.ExpressionType;
			return expressionType != null && IsDuckType(expressionType);
		}

		public bool IsQuackBuiltin(Expression node)
		{
			return IsQuackBuiltin(node.Entity);
		}

		public bool IsQuackBuiltin(IEntity entity)
		{
			return BuiltinFunction.Quack == entity;
		}

		public bool IsDuckType(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == DuckType)
			{
				return true;
			}
			if (type == ObjectType && _context.Parameters.Ducky)
			{
				return true;
			}
			return KnowsQuackFu(type);
		}

		public bool KnowsQuackFu(IType type)
		{
			return type.IsSubclassOf(IQuackFuType);
		}

		private bool IsCallableType(IType type)
		{
			return IsAssignableFrom(ICallableType, type) || type is ICallableType;
		}

		public ICallableType GetCallableType(IMethodBase method)
		{
			CallableSignature signature = new CallableSignature(method);
			return GetCallableType(signature);
		}

		public ICallableType GetCallableType(CallableSignature signature)
		{
			return _anonymousCallablesManager.GetCallableType(signature);
		}

		public virtual IType GetConcreteCallableType(Node sourceNode, CallableSignature signature)
		{
			return _anonymousCallablesManager.GetConcreteCallableType(sourceNode, signature);
		}

		public virtual IType GetConcreteCallableType(Node sourceNode, AnonymousCallableType anonymousType)
		{
			return _anonymousCallablesManager.GetConcreteCallableType(sourceNode, anonymousType);
		}

		public IType GetEnumeratorItemType(IType iteratorType)
		{
			if (iteratorType.IsArray)
			{
				return iteratorType.ElementType;
			}
			if (StringType == iteratorType)
			{
				return CharType;
			}
			if (iteratorType.IsClass)
			{
				IType enumeratorItemTypeFromAttribute = GetEnumeratorItemTypeFromAttribute(iteratorType);
				if (null != enumeratorItemTypeFromAttribute)
				{
					return enumeratorItemTypeFromAttribute;
				}
			}
			IType genericEnumerableItemType = GetGenericEnumerableItemType(iteratorType);
			if (null != genericEnumerableItemType)
			{
				return genericEnumerableItemType;
			}
			return ObjectType;
		}

		public static IType GetExpressionType(Expression node)
		{
			return node.ExpressionType ?? Error.Default;
		}

		public IType GetConcreteExpressionType(Expression expression)
		{
			IType expressionType = GetExpressionType(expression);
			AnonymousCallableType anonymousCallableType = expressionType as AnonymousCallableType;
			if (anonymousCallableType != null)
			{
				return expression.ExpressionType = GetConcreteCallableType(expression, anonymousCallableType);
			}
			return expressionType;
		}

		public void MapToConcreteExpressionTypes(ExpressionCollection items)
		{
			foreach (Expression item in items)
			{
				GetConcreteExpressionType(item);
			}
		}

		public IEntity GetDefaultMember(IType type)
		{
			for (IType type2 = type; type2 != null; type2 = type2.BaseType)
			{
				IEntity defaultMember = type2.GetDefaultMember();
				if (defaultMember != null)
				{
					return defaultMember;
				}
			}
			Set<IEntity> set = new Set<IEntity>();
			IType[] interfaces = type.GetInterfaces();
			foreach (IType type3 in interfaces)
			{
				IEntity defaultMember = GetDefaultMember(type3);
				if (defaultMember != null)
				{
					set.Add(defaultMember);
				}
			}
			return Entities.EntityFromList(set);
		}

		public void AddCompilerGeneratedType(TypeDefinition type)
		{
			GetCompilerGeneratedTypesModule().Members.Add(type);
		}

		public Boo.Lang.Compiler.Ast.Module GetCompilerGeneratedTypesModule()
		{
			return _compilerGeneratedTypesModule ?? (_compilerGeneratedTypesModule = NewModule("CompilerGenerated"));
		}

		private Boo.Lang.Compiler.Ast.Module NewModule(string nameSpace)
		{
			return NewModule(nameSpace, nameSpace);
		}

		private Boo.Lang.Compiler.Ast.Module NewModule(string nameSpace, string moduleName)
		{
			Boo.Lang.Compiler.Ast.Module module = CodeBuilder.CreateModule(moduleName, nameSpace);
			_context.CompileUnit.Modules.Add(module);
			return module;
		}

		public bool CanBeReachedFrom(IType expectedType, IType actualType)
		{
			bool byDowncast;
			return CanBeReachedFrom(expectedType, actualType, out byDowncast);
		}

		public bool CanBeReachedFrom(IType expectedType, IType actualType, out bool byDowncast)
		{
			bool considerExplicitConversionOperators = !InStrictMode();
			return CanBeReachedFrom(expectedType, actualType, considerExplicitConversionOperators, out byDowncast);
		}

		public bool CanBeReachedFrom(IType expectedType, IType actualType, bool considerExplicitConversionOperators, out bool byDowncast)
		{
			byDowncast = false;
			return IsAssignableFrom(expectedType, actualType) || CanBeReachedByPromotion(expectedType, actualType) || FindImplicitConversionOperator(actualType, expectedType) != null || (considerExplicitConversionOperators && FindExplicitConversionOperator(actualType, expectedType) != null) || (byDowncast = DowncastPermissions().CanBeReachedByDowncast(expectedType, actualType));
		}

		private DowncastPermissions DowncastPermissions()
		{
			return _downcastPermissions ?? (_downcastPermissions = My<Boo.Lang.Compiler.TypeSystem.Services.DowncastPermissions>.Instance);
		}

		private bool InStrictMode()
		{
			return _context.Parameters.Strict;
		}

		public IMethod FindExplicitConversionOperator(IType fromType, IType toType)
		{
			return _findExplicitConversionOperator.Invoke(fromType, toType);
		}

		public IMethod FindImplicitConversionOperator(IType fromType, IType toType)
		{
			return _findImplicitConversionOperator.Invoke(fromType, toType);
		}

		private IMethod FindConversionOperator(string name, IType fromType, IType toType)
		{
			while (fromType != ObjectType)
			{
				IMethod method = FindConversionOperator(name, fromType, toType, fromType.GetMembers());
				if (null != method)
				{
					return method;
				}
				method = FindConversionOperator(name, fromType, toType, toType.GetMembers());
				if (null != method)
				{
					return method;
				}
				method = FindConversionOperator(name, fromType, toType, FindExtension(fromType, name));
				if (null != method)
				{
					return method;
				}
				fromType = fromType.BaseType;
				if (null == fromType)
				{
					break;
				}
			}
			return null;
		}

		private IEntity[] FindExtension(IType fromType, string name)
		{
			IEntity entity = NameResolutionService.ResolveExtension(fromType, name);
			if (null == entity)
			{
				return Ambiguous.NoEntities;
			}
			Ambiguous ambiguous = entity as Ambiguous;
			if (null != ambiguous)
			{
				return ambiguous.Entities;
			}
			return new IEntity[1] { entity };
		}

		private IMethod FindConversionOperator(string name, IType fromType, IType toType, IEnumerable<IEntity> candidates)
		{
			foreach (IMethod item in NameResolutionService.Select<IMethod>(candidates, name, EntityType.Method))
			{
				if (IsConversionOperator(item, fromType, toType))
				{
					return item;
				}
			}
			return null;
		}

		private static bool IsConversionOperator(IMethod method, IType fromType, IType toType)
		{
			if (!method.IsStatic)
			{
				return false;
			}
			if (method.ReturnType != toType)
			{
				return false;
			}
			IParameter[] parameters = method.GetParameters();
			return 1 == parameters.Length && fromType == parameters[0].Type;
		}

		public bool IsCallableTypeAssignableFrom(ICallableType lhs, IType rhs)
		{
			if (lhs == rhs)
			{
				return true;
			}
			if (rhs.IsNull())
			{
				return true;
			}
			ICallableType callableType = rhs as ICallableType;
			if (null == callableType)
			{
				return false;
			}
			CallableSignature signature = lhs.GetSignature();
			CallableSignature signature2 = callableType.GetSignature();
			if (signature == signature2)
			{
				return true;
			}
			IParameter[] parameters = signature.Parameters;
			IParameter[] parameters2 = signature2.Parameters;
			if (parameters.Length < parameters2.Length)
			{
				return false;
			}
			for (int i = 0; i < parameters2.Length; i++)
			{
				if (!CanBeReachedFrom(parameters[i].Type, parameters2[i].Type))
				{
					return false;
				}
			}
			return CompatibleReturnTypes(signature, signature2);
		}

		private bool CompatibleReturnTypes(CallableSignature lvalue, CallableSignature rvalue)
		{
			if (VoidType != lvalue.ReturnType && VoidType != rvalue.ReturnType)
			{
				return CanBeReachedFrom(lvalue.ReturnType, rvalue.ReturnType);
			}
			return true;
		}

		public static bool CheckOverrideSignature(IMethod impl, IMethod baseMethod)
		{
			if (!GenericsServices.AreOfSameGenerity(impl, baseMethod))
			{
				return false;
			}
			CallableSignature overriddenSignature = GetOverriddenSignature(baseMethod, impl);
			return CheckOverrideSignature(impl.GetParameters(), overriddenSignature.Parameters);
		}

		public static bool CheckOverrideSignature(IParameter[] implParameters, IParameter[] baseParameters)
		{
			return CallableSignature.AreSameParameters(implParameters, baseParameters);
		}

		public static CallableSignature GetOverriddenSignature(IMethod baseMethod, IMethod impl)
		{
			if (baseMethod.GenericInfo != null && GenericsServices.AreOfSameGenerity(baseMethod, impl))
			{
				return baseMethod.GenericInfo.ConstructMethod(impl.GenericInfo.GenericParameters).CallableType.GetSignature();
			}
			return baseMethod.CallableType.GetSignature();
		}

		public virtual bool CanBeReachedByDownCastOrPromotion(IType expectedType, IType actualType)
		{
			return DowncastPermissions().CanBeReachedByDowncast(expectedType, actualType) || CanBeReachedByPromotion(expectedType, actualType);
		}

		public virtual bool CanBeReachedByPromotion(IType expectedType, IType actualType)
		{
			return _canBeReachedByPromotion.Invoke(expectedType, actualType);
		}

		private bool CanBeReachedByPromotionImpl(IType expectedType, IType actualType)
		{
			if (IsNullable(expectedType) && actualType.IsNull())
			{
				return true;
			}
			if (IsIntegerNumber(actualType) && CanBeExplicitlyCastToPrimitiveNumber(expectedType))
			{
				return true;
			}
			if (IsIntegerNumber(expectedType) && CanBeExplicitlyCastToPrimitiveNumber(actualType))
			{
				return true;
			}
			return expectedType.IsValueType && IsNumber(expectedType) && IsNumber(actualType);
		}

		public bool CanBeExplicitlyCastToPrimitiveNumber(IType type)
		{
			return type.IsEnum || type == CharType;
		}

		public static bool ContainsMethodsOnly(ICollection<IEntity> members)
		{
			return members.All((IEntity member) => EntityType.Method == member.EntityType);
		}

		public bool IsIntegerNumber(IType type)
		{
			return IsSignedInteger(type) || IsUnsignedInteger(type);
		}

		private bool IsUnsignedInteger(IType type)
		{
			return type == UShortType || type == UIntType || type == ULongType || type == ByteType;
		}

		public bool IsIntegerOrBool(IType type)
		{
			return BoolType == type || IsIntegerNumber(type);
		}

		public bool IsNumberOrBool(IType type)
		{
			return BoolType == type || IsNumber(type);
		}

		public bool IsNumber(IType type)
		{
			return IsPrimitiveNumber(type) || type == DecimalType;
		}

		public bool IsPrimitiveNumber(IType type)
		{
			return IsIntegerNumber(type) || IsFloatingPointNumber(type);
		}

		public bool IsFloatingPointNumber(IType type)
		{
			return type == DoubleType || type == SingleType;
		}

		public bool IsSignedNumber(IType type)
		{
			return IsNumber(type) && !IsUnsignedInteger(type);
		}

		public static bool IsReferenceType(IType type)
		{
			IGenericParameter genericParameter = type as IGenericParameter;
			if (null == genericParameter)
			{
				return !type.IsValueType;
			}
			if (genericParameter.IsClass)
			{
				return true;
			}
			IType[] typeConstraints = genericParameter.GetTypeConstraints();
			foreach (IType type2 in typeConstraints)
			{
				if (!type2.IsValueType && !type2.IsInterface)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsAnyType(IType type)
		{
			IGenericParameter genericParameter = type as IGenericParameter;
			return genericParameter != null && !genericParameter.IsClass && !genericParameter.IsValueType && 0 == genericParameter.GetTypeConstraints().Length;
		}

		public static bool IsNullable(IType type)
		{
			ExternalType externalType = type as ExternalType;
			return externalType != null && externalType.ActualType.IsGenericType && externalType.ActualType.GetGenericTypeDefinition() == Types.Nullable;
		}

		public IType GetNullableUnderlyingType(IType type)
		{
			ExternalType externalType = type as ExternalType;
			return Map(Nullable.GetUnderlyingType(externalType.ActualType));
		}

		public static bool IsUnknown(Expression node)
		{
			IType expressionType = node.ExpressionType;
			return expressionType != null && IsUnknown(expressionType);
		}

		public static bool IsUnknown(IType type)
		{
			return EntityType.Unknown == type.EntityType;
		}

		public static bool IsError(Expression node)
		{
			IType expressionType = node.ExpressionType;
			return expressionType != null && IsError(expressionType);
		}

		public static bool IsErrorAny(ExpressionCollection collection)
		{
			return collection.Any(IsError);
		}

		public bool IsBuiltin(IEntity entity)
		{
			if (EntityType.Method == entity.EntityType)
			{
				return BuiltinsType == ((IMethod)entity).DeclaringType;
			}
			return false;
		}

		public static bool IsError(IEntity entity)
		{
			return EntityType.Error == entity.EntityType;
		}

		public static TypeMemberModifiers GetAccess(IAccessibleMember member)
		{
			if (member.IsPublic)
			{
				return TypeMemberModifiers.Public;
			}
			if (member.IsProtected)
			{
				if (member.IsInternal)
				{
					return TypeMemberModifiers.Internal | TypeMemberModifiers.Protected;
				}
				return TypeMemberModifiers.Protected;
			}
			if (member.IsInternal)
			{
				return TypeMemberModifiers.Internal;
			}
			return TypeMemberModifiers.Private;
		}

		[Obsolete("Use Node.Entity instead.")]
		public static IEntity GetOptionalEntity(Node node)
		{
			return node.Entity;
		}

		public static IEntity GetEntity(Node node)
		{
			IEntity entity = node.Entity;
			if (entity != null)
			{
				return entity;
			}
			if (My<CompilerParameters>.Instance.Pipeline.BreakOnErrors)
			{
				InvalidNode(node);
			}
			return Error.Default;
		}

		public static IType GetReferencedType(Expression typeref)
		{
			switch (typeref.NodeType)
			{
			case NodeType.TypeofExpression:
				return GetType(((TypeofExpression)typeref).Type);
			case NodeType.ReferenceExpression:
			case NodeType.MemberReferenceExpression:
			case NodeType.GenericReferenceExpression:
				return typeref.Entity as IType;
			default:
				return null;
			}
		}

		public bool IsAttribute(IType type)
		{
			return type.IsSubclassOf(SystemAttribute);
		}

		public static IType GetType(Node node)
		{
			return ((ITypedEntity)GetEntity(node)).Type;
		}

		public IType Map(Type type)
		{
			return TypeSystemProvider().Map(type);
		}

		private IReflectionTypeSystemProvider TypeSystemProvider()
		{
			return _typeSystemProvider.Instance;
		}

		public IParameter[] Map(ParameterInfo[] parameters)
		{
			return TypeSystemProvider().Map(parameters);
		}

		public IConstructor Map(ConstructorInfo constructor)
		{
			return (IConstructor)Map((MemberInfo)constructor);
		}

		public IMethod Map(MethodInfo method)
		{
			return (IMethod)Map((MemberInfo)method);
		}

		public IEntity Map(MemberInfo[] members)
		{
			return TypeSystemProvider().Map(members);
		}

		public IEntity Map(MemberInfo mi)
		{
			return TypeSystemProvider().Map(mi);
		}

		public static string GetSignature(IEntityWithParameters method)
		{
			return My<EntityFormatter>.Instance.FormatSignature(method);
		}

		public IEntity ResolvePrimitive(string name)
		{
			if (_primitives.TryGetValue(name, out var value))
			{
				return value;
			}
			return null;
		}

		public bool IsPrimitive(string name)
		{
			return _primitives.ContainsKey(name);
		}

		public bool IsLiteralPrimitive(IType type)
		{
			ExternalType externalType = type as ExternalType;
			return externalType != null && externalType.PrimitiveName != null && _literalPrimitives.Contains(externalType.PrimitiveName);
		}

		public bool IsSystemObject(IType type)
		{
			return type == ObjectType || type == DuckType;
		}

		public bool IsPointerCompatible(IType type)
		{
			return IsPrimitiveNumber(type) || (type.IsValueType && 0 != SizeOf(type));
		}

		protected virtual void PreparePrimitives()
		{
			AddPrimitiveType("duck", DuckType);
			AddPrimitiveType("void", VoidType);
			AddPrimitiveType("object", ObjectType);
			AddPrimitiveType("callable", ICallableType);
			AddPrimitiveType("decimal", DecimalType);
			AddPrimitiveType("date", DateTimeType);
			AddLiteralPrimitiveType("bool", BoolType);
			AddLiteralPrimitiveType("sbyte", SByteType);
			AddLiteralPrimitiveType("byte", ByteType);
			AddLiteralPrimitiveType("short", ShortType);
			AddLiteralPrimitiveType("ushort", UShortType);
			AddLiteralPrimitiveType("int", IntType);
			AddLiteralPrimitiveType("uint", UIntType);
			AddLiteralPrimitiveType("long", LongType);
			AddLiteralPrimitiveType("ulong", ULongType);
			AddLiteralPrimitiveType("single", SingleType);
			AddLiteralPrimitiveType("double", DoubleType);
			AddLiteralPrimitiveType("char", CharType);
			AddLiteralPrimitiveType("string", StringType);
			AddLiteralPrimitiveType("regex", RegexType);
			AddLiteralPrimitiveType("timespan", TimeSpanType);
		}

		protected virtual void PrepareBuiltinFunctions()
		{
			AddBuiltin(BuiltinFunction.Len);
			AddBuiltin(BuiltinFunction.AddressOf);
			AddBuiltin(BuiltinFunction.Eval);
			AddBuiltin(BuiltinFunction.Switch);
		}

		protected void AddPrimitiveType(string name, IType type)
		{
			_primitives[name] = type;
			((ExternalType)type).PrimitiveName = name;
		}

		protected void AddLiteralPrimitiveType(string name, IType type)
		{
			AddPrimitiveType(name, type);
			_literalPrimitives.Add(name);
		}

		protected void AddBuiltin(BuiltinFunction function)
		{
			_primitives[function.Name] = function;
		}

		public IConstructor GetDefaultConstructor(IType type)
		{
			return type.GetConstructors().FirstOrDefault((IConstructor constructor) => 0 == constructor.GetParameters().Length);
		}

		private IType GetExternalEnumeratorItemType(IType iteratorType)
		{
			Type actualType = ((ExternalType)iteratorType).ActualType;
			EnumeratorItemTypeAttribute enumeratorItemTypeAttribute = (EnumeratorItemTypeAttribute)System.Attribute.GetCustomAttribute(actualType, typeof(EnumeratorItemTypeAttribute));
			return (enumeratorItemTypeAttribute != null) ? Map(enumeratorItemTypeAttribute.ItemType) : null;
		}

		private IType GetEnumeratorItemTypeFromAttribute(IType iteratorType)
		{
			if (iteratorType is ExternalType)
			{
				return GetExternalEnumeratorItemType(iteratorType);
			}
			if (iteratorType.ConstructedInfo != null)
			{
				IType enumeratorItemType = GetEnumeratorItemType(iteratorType.ConstructedInfo.GenericDefinition);
				return iteratorType.ConstructedInfo.Map(enumeratorItemType);
			}
			AbstractInternalType abstractInternalType = (AbstractInternalType)iteratorType;
			IType type = Map(typeof(EnumeratorItemTypeAttribute));
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in abstractInternalType.TypeDefinition.Attributes)
			{
				IConstructor constructor = GetEntity(attribute) as IConstructor;
				if (constructor != null && constructor.DeclaringType == type)
				{
					return GetType(attribute.Arguments[0]);
				}
			}
			return null;
		}

		public IType GetGenericEnumerableItemType(IType iteratorType)
		{
			if (iteratorType is ArrayType)
			{
				return iteratorType.ElementType;
			}
			IType type = null;
			foreach (IType item in GenericsServices.FindConstructedTypes(iteratorType, IEnumerableGenericType))
			{
				IType type2 = item.ConstructedInfo.GenericArguments[0];
				type = ((type == null) ? type2 : GetMostGenericType(type, type2));
			}
			return type;
		}

		private static void InvalidNode(Node node)
		{
			throw CompilerErrorFactory.InvalidNode(node);
		}

		public virtual bool IsValidException(IType type)
		{
			return IsAssignableFrom(ExceptionType, type);
		}

		private static bool IsAssignableFrom(IType expectedType, IType actualType)
		{
			return TypeCompatibilityRules.IsAssignableFrom(expectedType, actualType);
		}

		public virtual IConstructor GetStringExceptionConstructor()
		{
			return Map(typeof(Exception).GetConstructor(new Type[1] { typeof(string) }));
		}

		public virtual bool IsMacro(IType type)
		{
			return type.IsSubclassOf(IAstMacroType) || type.IsSubclassOf(IAstGeneratorMacroType);
		}

		public virtual int SizeOf(IType type)
		{
			if (type.IsPointer)
			{
				type = type.ElementType;
			}
			if (!(type?.IsValueType ?? false))
			{
				return 0;
			}
			ExternalType externalType = type as ExternalType;
			if (null != externalType)
			{
				return Marshal.SizeOf(externalType.ActualType);
			}
			int num = 0;
			InternalClass internalClass = type as InternalClass;
			if (null == internalClass)
			{
				return 0;
			}
			foreach (Field item in internalClass.TypeDefinition.Members.OfType<Field>())
			{
				int num2 = SizeOf(item.Type.Entity as IType);
				if (0 == num2)
				{
					return 0;
				}
				num += num2;
			}
			return num;
		}

		public IType MapWildcardType(IType type)
		{
			if (type.IsNull())
			{
				return ObjectType;
			}
			if (EmptyArrayType.Default == type)
			{
				return ObjectArrayType;
			}
			return type;
		}
	}
}
