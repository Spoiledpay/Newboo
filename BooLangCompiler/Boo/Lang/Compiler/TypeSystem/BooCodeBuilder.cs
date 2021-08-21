#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Services;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class BooCodeBuilder : ICodeBuilder
	{
		private readonly EnvironmentProvision<TypeSystemServices> _tss = default(EnvironmentProvision<TypeSystemServices>);

		private readonly EnvironmentProvision<InternalTypeSystemProvider> _internalTypeSystemProvider = default(EnvironmentProvision<InternalTypeSystemProvider>);

		private readonly DynamicVariable<ITypeReferenceFactory> _typeReferenceFactory;

		public TypeSystemServices TypeSystemServices => _tss;

		private InternalTypeSystemProvider InternalTypeSystemProvider => _internalTypeSystemProvider.Instance;

		private ITypeReferenceFactory TypeReferenceFactory => _typeReferenceFactory.Value;

		public BooCodeBuilder()
		{
			_typeReferenceFactory = new DynamicVariable<ITypeReferenceFactory>(new StandardTypeReferenceFactory(this));
		}

		public int GetFirstParameterIndex(TypeMember member)
		{
			return (!member.IsStatic) ? 1 : 0;
		}

		public Statement CreateFieldAssignment(Field node, Expression initializer)
		{
			InternalField fieldEntity = (InternalField)TypeSystemServices.GetEntity(node);
			return CreateFieldAssignment(node.LexicalInfo, fieldEntity, initializer);
		}

		public Statement CreateFieldAssignment(LexicalInfo lexicalInfo, IField fieldEntity, Expression initializer)
		{
			return new ExpressionStatement(lexicalInfo, CreateFieldAssignmentExpression(fieldEntity, initializer));
		}

		public Expression CreateFieldAssignmentExpression(IField fieldEntity, Expression initializer)
		{
			return CreateAssignment(initializer.LexicalInfo, CreateReference(fieldEntity), initializer);
		}

		public Boo.Lang.Compiler.Ast.Attribute CreateAttribute(Type type)
		{
			return CreateAttribute(TypeSystemServices.Map(type));
		}

		public Boo.Lang.Compiler.Ast.Attribute CreateAttribute(IType type)
		{
			return CreateAttribute(DefaultConstructorFor(type));
		}

		public Boo.Lang.Compiler.Ast.Attribute CreateAttribute(IConstructor constructor)
		{
			Boo.Lang.Compiler.Ast.Attribute attribute = new Boo.Lang.Compiler.Ast.Attribute();
			attribute.Name = constructor.DeclaringType.FullName;
			attribute.Entity = constructor;
			return attribute;
		}

		public Boo.Lang.Compiler.Ast.Attribute CreateAttribute(IConstructor constructor, Expression arg)
		{
			Boo.Lang.Compiler.Ast.Attribute attribute = CreateAttribute(constructor);
			attribute.Arguments.Add(arg);
			return attribute;
		}

		public Boo.Lang.Compiler.Ast.Module CreateModule(string moduleName, string nameSpace)
		{
			Boo.Lang.Compiler.Ast.Module module = new Boo.Lang.Compiler.Ast.Module();
			module.Name = moduleName;
			Boo.Lang.Compiler.Ast.Module module2 = module;
			if (!string.IsNullOrEmpty(nameSpace))
			{
				module2.Namespace = new NamespaceDeclaration(nameSpace);
			}
			InternalTypeSystemProvider.EntityFor(module2);
			return module2;
		}

		public BooClassBuilder CreateClass(string name)
		{
			return new BooClassBuilder(name);
		}

		public BooClassBuilder CreateClass(string name, TypeMemberModifiers modifiers)
		{
			BooClassBuilder booClassBuilder = CreateClass(name);
			booClassBuilder.Modifiers = modifiers;
			return booClassBuilder;
		}

		public Expression CreateDefaultInitializer(LexicalInfo li, ReferenceExpression target, IType type)
		{
			return type.IsValueType ? CreateInitValueType(li, target) : CreateAssignment(li, target, CreateNullLiteral());
		}

		public Expression CreateDefaultInitializer(LexicalInfo li, InternalLocal local)
		{
			return CreateDefaultInitializer(li, CreateReference(local), local.Type);
		}

		public Expression CreateInitValueType(LexicalInfo li, ReferenceExpression target)
		{
			MethodInvocationExpression methodInvocationExpression = CreateBuiltinInvocation(li, BuiltinFunction.InitValueType);
			methodInvocationExpression.Arguments.Add(target);
			return methodInvocationExpression;
		}

		public Expression CreateInitValueType(LexicalInfo li, InternalLocal local)
		{
			return CreateInitValueType(li, CreateReference(local));
		}

		public Expression CreateCast(IType type, Expression target)
		{
			if (type == target.ExpressionType)
			{
				return target;
			}
			CastExpression castExpression = new CastExpression(target.LexicalInfo);
			castExpression.Type = CreateTypeReference(type);
			castExpression.Target = target;
			castExpression.ExpressionType = type;
			return castExpression;
		}

		public TypeofExpression CreateTypeofExpression(IType type)
		{
			TypeofExpression typeofExpression = new TypeofExpression();
			typeofExpression.Type = CreateTypeReference(type);
			typeofExpression.ExpressionType = TypeSystemServices.TypeType;
			return typeofExpression;
		}

		public Expression CreateTypeofExpression(Type type)
		{
			return CreateTypeofExpression(TypeSystemServices.Map(type));
		}

		public ReferenceExpression CreateLabelReference(LabelStatement label)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(label.LexicalInfo, label.Name);
			referenceExpression.Entity = label.Entity;
			return referenceExpression;
		}

		public Statement CreateSwitch(LexicalInfo li, Expression offset, IEnumerable<LabelStatement> labels)
		{
			offset.LexicalInfo = li;
			return CreateSwitch(offset, labels);
		}

		public Statement CreateSwitch(Expression offset, IEnumerable<LabelStatement> labels)
		{
			MethodInvocationExpression methodInvocationExpression = CreateBuiltinInvocation(offset.LexicalInfo, BuiltinFunction.Switch);
			methodInvocationExpression.Arguments.Add(offset);
			foreach (LabelStatement label in labels)
			{
				methodInvocationExpression.Arguments.Add(CreateLabelReference(label));
			}
			methodInvocationExpression.ExpressionType = TypeSystemServices.VoidType;
			return new ExpressionStatement(methodInvocationExpression);
		}

		public Expression CreateAddressOfExpression(IMethod method)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression.Target = CreateBuiltinReference(BuiltinFunction.AddressOf);
			methodInvocationExpression.Arguments.Add(CreateMethodReference(method));
			methodInvocationExpression.ExpressionType = TypeSystemServices.IntPtrType;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(Expression target, MethodInfo method)
		{
			return CreateMethodInvocation(target, TypeSystemServices.Map(method));
		}

		public MethodInvocationExpression CreatePropertyGet(Expression target, IProperty property)
		{
			return property.IsExtension ? CreateMethodInvocation(property.GetGetMethod(), target) : CreateMethodInvocation(target, property.GetGetMethod());
		}

		public MethodInvocationExpression CreatePropertySet(Expression target, IProperty property, Expression value)
		{
			return CreateMethodInvocation(target, property.GetSetMethod(), value);
		}

		public MethodInvocationExpression CreateMethodInvocation(MethodInfo staticMethod, Expression arg)
		{
			return CreateMethodInvocation(TypeSystemServices.Map(staticMethod), arg);
		}

		public MethodInvocationExpression CreateMethodInvocation(MethodInfo staticMethod, Expression arg0, Expression arg1)
		{
			return CreateMethodInvocation(TypeSystemServices.Map(staticMethod), arg0, arg1);
		}

		public MethodInvocationExpression CreateMethodInvocation(LexicalInfo li, Expression target, IMethod tag, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(target, tag, arg);
			methodInvocationExpression.LexicalInfo = li;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(Expression target, IMethod tag, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(target, tag);
			methodInvocationExpression.Arguments.Add(arg);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(Expression target, IMethod entity, Expression arg1, Expression arg2)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(target, entity, arg1);
			methodInvocationExpression.Arguments.Add(arg2);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(LexicalInfo li, IMethod staticMethod, Expression arg0, Expression arg1)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(staticMethod, arg0, arg1);
			methodInvocationExpression.LexicalInfo = li;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(IMethod staticMethod, Expression arg0, Expression arg1)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(staticMethod, arg0);
			methodInvocationExpression.Arguments.Add(arg1);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(LexicalInfo li, IMethod staticMethod, Expression arg0, Expression arg1, Expression arg2)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(staticMethod, arg0, arg1, arg2);
			methodInvocationExpression.LexicalInfo = li;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(IMethod staticMethod, Expression arg0, Expression arg1, Expression arg2)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(staticMethod, arg0, arg1);
			methodInvocationExpression.Arguments.Add(arg2);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(IMethod staticMethod, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(staticMethod);
			methodInvocationExpression.LexicalInfo = arg.LexicalInfo;
			methodInvocationExpression.Arguments.Add(arg);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(IMethod method)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression.Target = CreateMemberReference(method);
			methodInvocationExpression.ExpressionType = method.ReturnType;
			return methodInvocationExpression;
		}

		public TypeReference CreateTypeReference(Type type)
		{
			return CreateTypeReference(TypeSystemServices.Map(type));
		}

		public TypeReference CreateTypeReference(LexicalInfo li, Type type)
		{
			return CreateTypeReference(li, TypeSystemServices.Map(type));
		}

		public TypeReference CreateTypeReference(LexicalInfo li, IType type)
		{
			TypeReference typeReference = CreateTypeReference(type);
			typeReference.LexicalInfo = li;
			return typeReference;
		}

		public TypeReference CreateTypeReference(IType type)
		{
			return TypeReferenceFactory.TypeReferenceFor(type);
		}

		public SuperLiteralExpression CreateSuperReference(IType expressionType)
		{
			SuperLiteralExpression superLiteralExpression = new SuperLiteralExpression();
			superLiteralExpression.ExpressionType = expressionType;
			return superLiteralExpression;
		}

		public SelfLiteralExpression CreateSelfReference(LexicalInfo location, IType expressionType)
		{
			SelfLiteralExpression selfLiteralExpression = CreateSelfReference(expressionType);
			selfLiteralExpression.LexicalInfo = location;
			return selfLiteralExpression;
		}

		public SelfLiteralExpression CreateSelfReference(IType expressionType)
		{
			SelfLiteralExpression selfLiteralExpression = new SelfLiteralExpression();
			selfLiteralExpression.ExpressionType = expressionType;
			return selfLiteralExpression;
		}

		public ReferenceExpression CreateLocalReference(string name, InternalLocal entity)
		{
			return CreateTypedReference(name, entity);
		}

		public ReferenceExpression CreateTypedReference(string name, ITypedEntity entity)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(name);
			referenceExpression.Entity = entity;
			referenceExpression.ExpressionType = entity.Type;
			return referenceExpression;
		}

		public ReferenceExpression CreateReference(IEntity entity)
		{
			return entity.EntityType switch
			{
				EntityType.Local => CreateReference((InternalLocal)entity), 
				EntityType.Field => CreateReference((IField)entity), 
				EntityType.Parameter => CreateReference((InternalParameter)entity), 
				EntityType.Property => CreateReference((IProperty)entity), 
				_ => CreateTypedReference(entity.Name, (ITypedEntity)entity), 
			};
		}

		public ReferenceExpression CreateReference(InternalLocal local)
		{
			return CreateLocalReference(local.Name, local);
		}

		public MemberReferenceExpression CreateReference(LexicalInfo li, Field field)
		{
			MemberReferenceExpression memberReferenceExpression = CreateReference(field);
			memberReferenceExpression.LexicalInfo = li;
			return memberReferenceExpression;
		}

		public MemberReferenceExpression CreateReference(Field field)
		{
			return CreateReference((IField)field.Entity);
		}

		public MemberReferenceExpression CreateReference(IField field)
		{
			return CreateMemberReference(field);
		}

		public MemberReferenceExpression CreateReference(Property property)
		{
			return CreateReference((IProperty)property.Entity);
		}

		public MemberReferenceExpression CreateReference(IProperty property)
		{
			return CreateMemberReference(property);
		}

		public MemberReferenceExpression CreateMemberReference(IMember member)
		{
			Expression target = (member.IsStatic ? ((Expression)CreateReference(member.DeclaringType)) : ((Expression)CreateSelfReference(member.DeclaringType)));
			return CreateMemberReference(target, member);
		}

		public MemberReferenceExpression CreateMemberReference(LexicalInfo li, Expression target, IMember member)
		{
			MemberReferenceExpression memberReferenceExpression = CreateMemberReference(target, member);
			memberReferenceExpression.LexicalInfo = li;
			return memberReferenceExpression;
		}

		public MemberReferenceExpression CreateMemberReference(Expression target, IMember member)
		{
			MemberReferenceExpression memberReferenceExpression = MemberReferenceForEntity(target, member);
			memberReferenceExpression.ExpressionType = member.Type;
			return memberReferenceExpression;
		}

		public MemberReferenceExpression MemberReferenceForEntity(Expression target, IEntity entity)
		{
			MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(target.LexicalInfo);
			memberReferenceExpression.Target = target;
			memberReferenceExpression.Name = entity.Name;
			memberReferenceExpression.Entity = entity;
			return memberReferenceExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(LexicalInfo li, Expression target, IMethod entity)
		{
			MethodInvocationExpression methodInvocationExpression = CreateMethodInvocation(target, entity);
			methodInvocationExpression.LexicalInfo = li;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateMethodInvocation(Expression target, IMethod entity)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(target.LexicalInfo);
			methodInvocationExpression.Target = CreateMemberReference(target, entity);
			methodInvocationExpression.ExpressionType = entity.ReturnType;
			return methodInvocationExpression;
		}

		public ReferenceExpression CreateReference(LexicalInfo info, IType type)
		{
			ReferenceExpression referenceExpression = CreateReference(type);
			referenceExpression.LexicalInfo = info;
			return referenceExpression;
		}

		public ReferenceExpression CreateReference(LexicalInfo li, Type type)
		{
			return CreateReference(li, TypeSystemServices.Map(type));
		}

		public ReferenceExpression CreateReference(IType type)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(type.FullName);
			referenceExpression.Entity = type;
			referenceExpression.IsSynthetic = true;
			return referenceExpression;
		}

		public MethodInvocationExpression CreateEvalInvocation(LexicalInfo li)
		{
			return CreateBuiltinInvocation(li, BuiltinFunction.Eval);
		}

		private static MethodInvocationExpression CreateBuiltinInvocation(LexicalInfo li, BuiltinFunction builtin)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(li);
			methodInvocationExpression.Target = CreateBuiltinReference(builtin);
			return methodInvocationExpression;
		}

		public static ReferenceExpression CreateBuiltinReference(BuiltinFunction builtin)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(builtin.Name);
			referenceExpression.Entity = builtin;
			return referenceExpression;
		}

		public MethodInvocationExpression CreateEvalInvocation(LexicalInfo li, Expression arg, Expression value)
		{
			MethodInvocationExpression methodInvocationExpression = CreateEvalInvocation(li);
			methodInvocationExpression.Arguments.Add(arg);
			methodInvocationExpression.Arguments.Add(value);
			methodInvocationExpression.ExpressionType = value.ExpressionType;
			return methodInvocationExpression;
		}

		public UnpackStatement CreateUnpackStatement(DeclarationCollection declarations, Expression expression)
		{
			UnpackStatement unpackStatement = new UnpackStatement(expression.LexicalInfo);
			unpackStatement.Declarations.AddRange(declarations);
			unpackStatement.Expression = expression;
			return unpackStatement;
		}

		public BinaryExpression CreateAssignment(LexicalInfo li, Expression lhs, Expression rhs)
		{
			BinaryExpression binaryExpression = CreateAssignment(lhs, rhs);
			binaryExpression.LexicalInfo = li;
			return binaryExpression;
		}

		public BinaryExpression CreateAssignment(Expression lhs, Expression rhs)
		{
			return CreateBoundBinaryExpression(TypeSystemServices.GetExpressionType(lhs), BinaryOperatorType.Assign, lhs, rhs);
		}

		public Expression CreateMethodReference(IMethod method)
		{
			return CreateMemberReference(method);
		}

		public Expression CreateMethodReference(LexicalInfo lexicalInfo, IMethod method)
		{
			Expression expression = CreateMethodReference(method);
			expression.LexicalInfo = lexicalInfo;
			return expression;
		}

		public BinaryExpression CreateBoundBinaryExpression(IType expressionType, BinaryOperatorType op, Expression lhs, Expression rhs)
		{
			BinaryExpression binaryExpression = new BinaryExpression(op, lhs, rhs);
			binaryExpression.ExpressionType = expressionType;
			binaryExpression.IsSynthetic = true;
			return binaryExpression;
		}

		public BoolLiteralExpression CreateBoolLiteral(bool value)
		{
			BoolLiteralExpression boolLiteralExpression = new BoolLiteralExpression(value);
			boolLiteralExpression.ExpressionType = TypeSystemServices.BoolType;
			return boolLiteralExpression;
		}

		public StringLiteralExpression CreateStringLiteral(string value)
		{
			StringLiteralExpression stringLiteralExpression = new StringLiteralExpression(value);
			stringLiteralExpression.ExpressionType = TypeSystemServices.StringType;
			return stringLiteralExpression;
		}

		public NullLiteralExpression CreateNullLiteral()
		{
			NullLiteralExpression nullLiteralExpression = new NullLiteralExpression();
			nullLiteralExpression.ExpressionType = Null.Default;
			return nullLiteralExpression;
		}

		public ArrayLiteralExpression CreateObjectArray(ExpressionCollection items)
		{
			return CreateArray(TypeSystemServices.ObjectArrayType, items);
		}

		public ArrayLiteralExpression CreateArray(IType arrayType, ExpressionCollection items)
		{
			if (!arrayType.IsArray)
			{
				throw new ArgumentException($"'{arrayType}'  is not an array type!", "arrayType");
			}
			ArrayLiteralExpression arrayLiteralExpression = new ArrayLiteralExpression();
			arrayLiteralExpression.ExpressionType = arrayType;
			arrayLiteralExpression.Items.AddRange(items);
			TypeSystemServices.MapToConcreteExpressionTypes(arrayLiteralExpression.Items);
			return arrayLiteralExpression;
		}

		public IntegerLiteralExpression CreateIntegerLiteral(int value)
		{
			IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression(value);
			integerLiteralExpression.ExpressionType = TypeSystemServices.IntType;
			return integerLiteralExpression;
		}

		public IntegerLiteralExpression CreateIntegerLiteral(long value)
		{
			IntegerLiteralExpression integerLiteralExpression = new IntegerLiteralExpression(value);
			integerLiteralExpression.ExpressionType = TypeSystemServices.LongType;
			return integerLiteralExpression;
		}

		public SlicingExpression CreateSlicing(Expression target, int begin)
		{
			IArrayType arrayType = target.ExpressionType as IArrayType;
			IType expressionType = ((arrayType != null) ? arrayType.ElementType : TypeSystemServices.ObjectType);
			SlicingExpression slicingExpression = new SlicingExpression(target, CreateIntegerLiteral(begin));
			slicingExpression.ExpressionType = expressionType;
			return slicingExpression;
		}

		public ReferenceExpression CreateReference(ParameterDeclaration parameter)
		{
			return CreateReference((InternalParameter)TypeSystemServices.GetEntity(parameter));
		}

		public ReferenceExpression CreateReference(InternalParameter parameter)
		{
			ReferenceExpression referenceExpression = new ReferenceExpression(parameter.Name);
			referenceExpression.Entity = parameter;
			referenceExpression.ExpressionType = parameter.Type;
			return referenceExpression;
		}

		public UnaryExpression CreateNotExpression(Expression node)
		{
			UnaryExpression unaryExpression = new UnaryExpression();
			unaryExpression.LexicalInfo = node.LexicalInfo;
			unaryExpression.Operand = node;
			unaryExpression.Operator = UnaryOperatorType.LogicalNot;
			unaryExpression.ExpressionType = TypeSystemServices.BoolType;
			return unaryExpression;
		}

		public ParameterDeclaration CreateParameterDeclaration(int index, string name, IType type, bool byref)
		{
			ParameterModifiers modifiers = (byref ? ParameterModifiers.Ref : ParameterModifiers.None);
			ParameterDeclaration parameterDeclaration = new ParameterDeclaration(name, CreateTypeReference(type), modifiers);
			parameterDeclaration.Entity = new InternalParameter(parameterDeclaration, index);
			return parameterDeclaration;
		}

		public ParameterDeclaration CreateParameterDeclaration(int index, string name, IType type)
		{
			return CreateParameterDeclaration(index, name, type, byref: false);
		}

		public GenericParameterDeclaration CreateGenericParameterDeclaration(int index, string name)
		{
			GenericParameterDeclaration genericParameterDeclaration = new GenericParameterDeclaration(name);
			genericParameterDeclaration.Entity = new InternalGenericParameter(_tss, genericParameterDeclaration, index);
			return genericParameterDeclaration;
		}

		public Constructor CreateConstructor(TypeMemberModifiers modifiers)
		{
			Constructor constructor = new Constructor();
			constructor.Modifiers = modifiers;
			constructor.IsSynthetic = true;
			EnsureEntityFor(constructor);
			return constructor;
		}

		private void EnsureEntityFor(TypeMember member)
		{
			InternalTypeSystemProvider.EntityFor(member);
		}

		public Constructor CreateStaticConstructor(TypeDefinition type)
		{
			Constructor constructor = new Constructor();
			constructor.IsSynthetic = true;
			constructor.Modifiers = TypeMemberModifiers.Private | TypeMemberModifiers.Static;
			EnsureEntityFor(constructor);
			type.Members.Add(constructor);
			return constructor;
		}

		public MethodInvocationExpression CreateConstructorInvocation(ClassDefinition cd)
		{
			IConstructor constructor = ((IType)cd.Entity).GetConstructors().First();
			return CreateConstructorInvocation(constructor);
		}

		public MethodInvocationExpression CreateConstructorInvocation(IConstructor constructor, Expression arg1, Expression arg2)
		{
			MethodInvocationExpression methodInvocationExpression = CreateConstructorInvocation(constructor, arg1);
			methodInvocationExpression.Arguments.Add(arg2);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateConstructorInvocation(IConstructor constructor, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = CreateConstructorInvocation(constructor);
			methodInvocationExpression.LexicalInfo = arg.LexicalInfo;
			methodInvocationExpression.Arguments.Add(arg);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateConstructorInvocation(LexicalInfo lexicalInfo, IConstructor constructor, params Expression[] args)
		{
			MethodInvocationExpression methodInvocationExpression = CreateConstructorInvocation(constructor);
			methodInvocationExpression.LexicalInfo = lexicalInfo;
			methodInvocationExpression.Arguments.AddRange(args);
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateConstructorInvocation(IConstructor constructor)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression.Target = new ReferenceExpression(constructor.DeclaringType.FullName);
			methodInvocationExpression.Target.Entity = constructor;
			methodInvocationExpression.ExpressionType = constructor.DeclaringType;
			return methodInvocationExpression;
		}

		public ExpressionStatement CreateSuperConstructorInvocation(IType baseType)
		{
			return CreateSuperConstructorInvocation(DefaultConstructorFor(baseType));
		}

		private IConstructor DefaultConstructorFor(IType baseType)
		{
			IConstructor defaultConstructor = TypeSystemServices.GetDefaultConstructor(baseType);
			if (null == defaultConstructor)
			{
				throw new ArgumentException(string.Concat("No default constructor for type '", baseType, "'."));
			}
			return defaultConstructor;
		}

		public ExpressionStatement CreateSuperConstructorInvocation(IConstructor defaultConstructor)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(new SuperLiteralExpression());
			methodInvocationExpression.Target.Entity = defaultConstructor;
			methodInvocationExpression.ExpressionType = TypeSystemServices.VoidType;
			return new ExpressionStatement(methodInvocationExpression);
		}

		public MethodInvocationExpression CreateSuperMethodInvocation(IMethod superMethod)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(CreateMemberReference(CreateSuperReference(superMethod.DeclaringType), superMethod));
			methodInvocationExpression.ExpressionType = superMethod.ReturnType;
			return methodInvocationExpression;
		}

		public MethodInvocationExpression CreateSuperMethodInvocation(IMethod superMethod, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = CreateSuperMethodInvocation(superMethod);
			methodInvocationExpression.Arguments.Add(arg);
			return methodInvocationExpression;
		}

		public Method CreateVirtualMethod(string name, IType returnType)
		{
			return CreateVirtualMethod(name, CreateTypeReference(returnType));
		}

		public Method CreateVirtualMethod(string name, TypeReference returnType)
		{
			return CreateMethod(name, returnType, TypeMemberModifiers.Public | TypeMemberModifiers.Virtual);
		}

		public Method CreateMethod(string name, IType returnType, TypeMemberModifiers modifiers)
		{
			return CreateMethod(name, CreateTypeReference(returnType), modifiers);
		}

		public Method CreateMethod(string name, TypeReference returnType, TypeMemberModifiers modifiers)
		{
			Method method = new Method(name);
			method.Modifiers = modifiers;
			method.ReturnType = returnType;
			method.IsSynthetic = true;
			EnsureEntityFor(method);
			return method;
		}

		public Property CreateProperty(string name, IType type)
		{
			Property property = new Property(name);
			property.Modifiers = TypeMemberModifiers.Public;
			property.Type = CreateTypeReference(type);
			property.IsSynthetic = true;
			EnsureEntityFor(property);
			return property;
		}

		public Field CreateField(string name, IType type)
		{
			Field field = new Field();
			field.Modifiers = TypeMemberModifiers.Protected;
			field.Name = name;
			field.Type = CreateTypeReference(type);
			field.Entity = new InternalField(field);
			field.IsSynthetic = true;
			return field;
		}

		public Method CreateRuntimeMethod(string name, TypeReference returnType)
		{
			Method method = CreateVirtualMethod(name, returnType);
			method.ImplementationFlags = MethodImplementationFlags.Runtime;
			return method;
		}

		public Method CreateRuntimeMethod(string name, IType returnType)
		{
			return CreateRuntimeMethod(name, CreateTypeReference(returnType));
		}

		public Method CreateRuntimeMethod(string name, IType returnType, IParameter[] parameters, bool hasParamArray)
		{
			Method method = CreateRuntimeMethod(name, returnType);
			DeclareParameters(method, parameters);
			method.Parameters.HasParamArray = hasParamArray;
			return method;
		}

		public void DeclareParameters(INodeWithParameters method, IParameter[] parameters)
		{
			DeclareParameters(method, parameters, 0);
		}

		public void DeclareParameters(INodeWithParameters method, IParameter[] parameters, int parameterIndexDelta)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				IParameter parameter = parameters[i];
				int num = parameterIndexDelta + i;
				method.Parameters.Add(CreateParameterDeclaration(num, string.IsNullOrEmpty(parameter.Name) ? ("arg" + num) : parameter.Name, parameter.Type, parameter.IsByRef));
			}
		}

		public void DeclareGenericParameters(INodeWithGenericParameters node, IGenericParameter[] parameters)
		{
			DeclareGenericParameters(node, parameters, 0);
		}

		public void DeclareGenericParameters(INodeWithGenericParameters node, IGenericParameter[] parameters, int parameterIndexDelta)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				IGenericParameter genericParameter = parameters[i];
				GenericParameterDeclaration item = CreateGenericParameterDeclaration(parameterIndexDelta + i, genericParameter.Name);
				node.GenericParameters.Add(item);
			}
		}

		public Method CreateAbstractMethod(LexicalInfo lexicalInfo, IMethod baseMethod)
		{
			TypeMemberModifiers typeMemberModifiers = VisibilityFrom(baseMethod);
			return CreateMethodFromPrototype(lexicalInfo, baseMethod, typeMemberModifiers | TypeMemberModifiers.Abstract);
		}

		private TypeMemberModifiers VisibilityFrom(IMethod baseMethod)
		{
			if (baseMethod.IsPublic)
			{
				return TypeMemberModifiers.Public;
			}
			if (baseMethod.IsInternal)
			{
				return TypeMemberModifiers.Internal;
			}
			return TypeMemberModifiers.Protected;
		}

		public Method CreateMethodFromPrototype(IMethod baseMethod, TypeMemberModifiers newModifiers)
		{
			return CreateMethodFromPrototype(LexicalInfo.Empty, baseMethod, newModifiers);
		}

		public Method CreateMethodFromPrototype(LexicalInfo lexicalInfo, IMethod baseMethod, TypeMemberModifiers newModifiers)
		{
			return CreateMethodFromPrototype(lexicalInfo, baseMethod, newModifiers, baseMethod.Name);
		}

		public Method CreateMethodFromPrototype(LexicalInfo location, IMethod baseMethod, TypeMemberModifiers newModifiers, string newMethodName)
		{
			Method method = new Method(location);
			method.Name = newMethodName;
			method.Modifiers = newModifiers;
			method.IsSynthetic = true;
			IDictionary<IType, IType> dictionary = DeclareGenericParametersFromPrototype(method, baseMethod);
			ITypeReferenceFactory value = ((dictionary != null) ? new MappedTypeReferenceFactory(TypeReferenceFactory, dictionary) : TypeReferenceFactory);
			_typeReferenceFactory.With(value, (Procedure)delegate
			{
				DeclareParameters(method, baseMethod.GetParameters(), (!baseMethod.IsStatic) ? 1 : 0);
				method.ReturnType = CreateTypeReference(baseMethod.ReturnType);
			});
			EnsureEntityFor(method);
			return method;
		}

		private IDictionary<IType, IType> DeclareGenericParametersFromPrototype(Method method, IMethod baseMethod)
		{
			IGenericMethodInfo genericInfo = baseMethod.GenericInfo;
			if (genericInfo == null)
			{
				return null;
			}
			IGenericParameter[] genericParameters = genericInfo.GenericParameters;
			DeclareGenericParameters(method, genericParameters);
			IGenericParameter[] to = method.GenericParameters.ToArray((GenericParameterDeclaration p) => (IGenericParameter)p.Entity);
			return CreateDictionaryMapping(genericParameters, to);
		}

		private static IDictionary<IType, IType> CreateDictionaryMapping(IGenericParameter[] from, IGenericParameter[] to)
		{
			Dictionary<IType, IType> dictionary = new Dictionary<IType, IType>(from.Length);
			for (int i = 0; i < from.Length; i++)
			{
				dictionary.Add(from[i], to[i]);
			}
			return dictionary;
		}

		public Event CreateAbstractEvent(LexicalInfo lexicalInfo, IEvent baseEvent)
		{
			Event @event = new Event(lexicalInfo);
			@event.Name = baseEvent.Name;
			@event.Type = CreateTypeReference(baseEvent.Type);
			@event.Add = CreateAbstractMethod(lexicalInfo, baseEvent.GetAddMethod());
			@event.Remove = CreateAbstractMethod(lexicalInfo, baseEvent.GetRemoveMethod());
			EnsureEntityFor(@event);
			return @event;
		}

		public Expression CreateNotNullTest(Expression target)
		{
			BinaryExpression binaryExpression = new BinaryExpression(target.LexicalInfo, BinaryOperatorType.ReferenceInequality, target, CreateNullLiteral());
			binaryExpression.ExpressionType = TypeSystemServices.BoolType;
			return binaryExpression;
		}

		public RaiseStatement RaiseException(LexicalInfo lexicalInfo, IConstructor exceptionConstructor, params Expression[] args)
		{
			Debug.Assert(TypeSystemServices.IsValidException(exceptionConstructor.DeclaringType));
			return new RaiseStatement(lexicalInfo, CreateConstructorInvocation(lexicalInfo, exceptionConstructor, args));
		}

		public InternalLocal DeclareTempLocal(Method node, IType type)
		{
			InternalLocal internalLocal = DeclareLocal(node, My<UniqueNameProvider>.Instance.GetUniqueName(), type);
			internalLocal.IsPrivateScope = true;
			return internalLocal;
		}

		public InternalLocal DeclareLocal(Method node, string name, IType type)
		{
			Local local = new Local(node.LexicalInfo, name);
			InternalLocal result = (InternalLocal)(local.Entity = new InternalLocal(local, type));
			node.Locals.Add(local);
			return result;
		}

		public void BindParameterDeclarations(bool isStatic, INodeWithParameters node)
		{
			int num = ((!isStatic) ? 1 : 0);
			ParameterDeclarationCollection parameters = node.Parameters;
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterDeclaration parameterDeclaration = parameters[i];
				if (null == parameterDeclaration.Type)
				{
					if (parameterDeclaration.IsParamArray)
					{
						parameterDeclaration.Type = CreateTypeReference(TypeSystemServices.ObjectArrayType);
					}
					else
					{
						parameterDeclaration.Type = CreateTypeReference(TypeSystemServices.ObjectType);
					}
				}
				parameterDeclaration.Entity = new InternalParameter(parameterDeclaration, i + num);
			}
		}

		public InternalLabel CreateLabel(Node sourceNode, string name)
		{
			return new InternalLabel(new LabelStatement(sourceNode.LexicalInfo, name));
		}

		public TypeMember CreateStub(ClassDefinition node, IMember member)
		{
			IMethod method = member as IMethod;
			if (null != method)
			{
				return CreateMethodStub(method);
			}
			IProperty property = member as IProperty;
			if (null != property)
			{
				return CreatePropertyStub(node, property);
			}
			return null;
		}

		private Method CreateMethodStub(IMethod baseMethod)
		{
			Method method = CreateMethodFromPrototype(baseMethod, TypeSystemServices.GetAccess(baseMethod) | TypeMemberModifiers.Virtual);
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression();
			methodInvocationExpression.Target = new MemberReferenceExpression(new ReferenceExpression("System"), "NotImplementedException");
			MethodInvocationExpression exception = methodInvocationExpression;
			method.Body.Statements.Add(new RaiseStatement(exception)
			{
				LexicalInfo = LexicalInfo.Empty
			});
			return method;
		}

		private Property CreatePropertyStub(ClassDefinition node, IProperty baseProperty)
		{
			Property property = node.Members[baseProperty.Name] as Property;
			if (null == property)
			{
				property = new Property(LexicalInfo.Empty);
				property.Name = baseProperty.Name;
				property.Modifiers = TypeSystemServices.GetAccess(baseProperty) | TypeMemberModifiers.Virtual;
				property.IsSynthetic = true;
				DeclareParameters(property, baseProperty.GetParameters(), (!baseProperty.IsStatic) ? 1 : 0);
				property.Type = CreateTypeReference(baseProperty.Type);
			}
			if (property.Getter == null && null != baseProperty.GetGetMethod())
			{
				property.Getter = CreateMethodStub(baseProperty.GetGetMethod());
			}
			if (property.Setter == null && null != baseProperty.GetSetMethod())
			{
				property.Setter = CreateMethodStub(baseProperty.GetSetMethod());
			}
			EnsureEntityFor(property);
			return property;
		}

		public Constructor GetOrCreateStaticConstructorFor(TypeDefinition type)
		{
			return type.GetStaticConstructor() ?? CreateStaticConstructor(type);
		}
	}
}
