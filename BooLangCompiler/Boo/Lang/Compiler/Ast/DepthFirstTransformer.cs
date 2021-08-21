using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	public class DepthFirstTransformer : IAstVisitor
	{
		protected Node _resultingNode = null;

		private static readonly LongJumpException CancellationException = new LongJumpException();

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCompileUnit(CompileUnit node)
		{
			if (EnterCompileUnit(node))
			{
				Visit(node.Modules);
				LeaveCompileUnit(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCompileUnit(CompileUnit node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCompileUnit(CompileUnit node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTypeMemberStatement(TypeMemberStatement node)
		{
			if (!EnterTypeMemberStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			TypeMember typeMember = node.TypeMember;
			if (null != typeMember)
			{
				TypeMember typeMember2 = (TypeMember)VisitNode(typeMember);
				if (!object.ReferenceEquals(typeMember2, typeMember))
				{
					node.TypeMember = typeMember2;
				}
			}
			LeaveTypeMemberStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterTypeMemberStatement(TypeMemberStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveTypeMemberStatement(TypeMemberStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			if (!EnterExplicitMemberInfo(node))
			{
				return;
			}
			SimpleTypeReference interfaceType = node.InterfaceType;
			if (null != interfaceType)
			{
				SimpleTypeReference simpleTypeReference = (SimpleTypeReference)VisitNode(interfaceType);
				if (!object.ReferenceEquals(simpleTypeReference, interfaceType))
				{
					node.InterfaceType = simpleTypeReference;
				}
			}
			LeaveExplicitMemberInfo(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExplicitMemberInfo(ExplicitMemberInfo node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExplicitMemberInfo(ExplicitMemberInfo node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSimpleTypeReference(SimpleTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnArrayTypeReference(ArrayTypeReference node)
		{
			if (!EnterArrayTypeReference(node))
			{
				return;
			}
			TypeReference elementType = node.ElementType;
			if (null != elementType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(elementType);
				if (!object.ReferenceEquals(typeReference, elementType))
				{
					node.ElementType = typeReference;
				}
			}
			IntegerLiteralExpression rank = node.Rank;
			if (null != rank)
			{
				IntegerLiteralExpression integerLiteralExpression = (IntegerLiteralExpression)VisitNode(rank);
				if (!object.ReferenceEquals(integerLiteralExpression, rank))
				{
					node.Rank = integerLiteralExpression;
				}
			}
			LeaveArrayTypeReference(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterArrayTypeReference(ArrayTypeReference node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveArrayTypeReference(ArrayTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCallableTypeReference(CallableTypeReference node)
		{
			if (!EnterCallableTypeReference(node))
			{
				return;
			}
			Visit(node.Parameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			LeaveCallableTypeReference(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCallableTypeReference(CallableTypeReference node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCallableTypeReference(CallableTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericTypeReference(GenericTypeReference node)
		{
			if (EnterGenericTypeReference(node))
			{
				Visit(node.GenericArguments);
				LeaveGenericTypeReference(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterGenericTypeReference(GenericTypeReference node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveGenericTypeReference(GenericTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCallableDefinition(CallableDefinition node)
		{
			if (!EnterCallableDefinition(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Parameters);
			Visit(node.GenericParameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			Visit(node.ReturnTypeAttributes);
			LeaveCallableDefinition(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCallableDefinition(CallableDefinition node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCallableDefinition(CallableDefinition node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnImport(Import node)
		{
			if (!EnterImport(node))
			{
				return;
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			ReferenceExpression assemblyReference = node.AssemblyReference;
			if (null != assemblyReference)
			{
				ReferenceExpression referenceExpression = (ReferenceExpression)VisitNode(assemblyReference);
				if (!object.ReferenceEquals(referenceExpression, assemblyReference))
				{
					node.AssemblyReference = referenceExpression;
				}
			}
			ReferenceExpression alias = node.Alias;
			if (null != alias)
			{
				ReferenceExpression referenceExpression = (ReferenceExpression)VisitNode(alias);
				if (!object.ReferenceEquals(referenceExpression, alias))
				{
					node.Alias = referenceExpression;
				}
			}
			LeaveImport(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterImport(Import node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveImport(Import node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnModule(Module node)
		{
			if (!EnterModule(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Members);
			Visit(node.BaseTypes);
			Visit(node.GenericParameters);
			NamespaceDeclaration @namespace = node.Namespace;
			if (null != @namespace)
			{
				NamespaceDeclaration namespaceDeclaration = (NamespaceDeclaration)VisitNode(@namespace);
				if (!object.ReferenceEquals(namespaceDeclaration, @namespace))
				{
					node.Namespace = namespaceDeclaration;
				}
			}
			Visit(node.Imports);
			Block globals = node.Globals;
			if (null != globals)
			{
				Block block = (Block)VisitNode(globals);
				if (!object.ReferenceEquals(block, globals))
				{
					node.Globals = block;
				}
			}
			Visit(node.AssemblyAttributes);
			LeaveModule(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterModule(Module node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveModule(Module node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnClassDefinition(ClassDefinition node)
		{
			if (EnterClassDefinition(node))
			{
				Visit(node.Attributes);
				Visit(node.Members);
				Visit(node.BaseTypes);
				Visit(node.GenericParameters);
				LeaveClassDefinition(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterClassDefinition(ClassDefinition node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveClassDefinition(ClassDefinition node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStructDefinition(StructDefinition node)
		{
			if (EnterStructDefinition(node))
			{
				Visit(node.Attributes);
				Visit(node.Members);
				Visit(node.BaseTypes);
				Visit(node.GenericParameters);
				LeaveStructDefinition(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterStructDefinition(StructDefinition node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveStructDefinition(StructDefinition node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnInterfaceDefinition(InterfaceDefinition node)
		{
			if (EnterInterfaceDefinition(node))
			{
				Visit(node.Attributes);
				Visit(node.Members);
				Visit(node.BaseTypes);
				Visit(node.GenericParameters);
				LeaveInterfaceDefinition(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterInterfaceDefinition(InterfaceDefinition node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveInterfaceDefinition(InterfaceDefinition node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEnumDefinition(EnumDefinition node)
		{
			if (EnterEnumDefinition(node))
			{
				Visit(node.Attributes);
				Visit(node.Members);
				Visit(node.BaseTypes);
				Visit(node.GenericParameters);
				LeaveEnumDefinition(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterEnumDefinition(EnumDefinition node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveEnumDefinition(EnumDefinition node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEnumMember(EnumMember node)
		{
			if (!EnterEnumMember(node))
			{
				return;
			}
			Visit(node.Attributes);
			Expression initializer = node.Initializer;
			if (null != initializer)
			{
				Expression expression = (Expression)VisitNode(initializer);
				if (!object.ReferenceEquals(expression, initializer))
				{
					node.Initializer = expression;
				}
			}
			LeaveEnumMember(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterEnumMember(EnumMember node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveEnumMember(EnumMember node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnField(Field node)
		{
			if (!EnterField(node))
			{
				return;
			}
			Visit(node.Attributes);
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			Expression initializer = node.Initializer;
			if (null != initializer)
			{
				Expression expression = (Expression)VisitNode(initializer);
				if (!object.ReferenceEquals(expression, initializer))
				{
					node.Initializer = expression;
				}
			}
			LeaveField(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterField(Field node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveField(Field node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnProperty(Property node)
		{
			if (!EnterProperty(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Parameters);
			Method getter = node.Getter;
			if (null != getter)
			{
				Method method = (Method)VisitNode(getter);
				if (!object.ReferenceEquals(method, getter))
				{
					node.Getter = method;
				}
			}
			Method setter = node.Setter;
			if (null != setter)
			{
				Method method = (Method)VisitNode(setter);
				if (!object.ReferenceEquals(method, setter))
				{
					node.Setter = method;
				}
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				ExplicitMemberInfo explicitMemberInfo = (ExplicitMemberInfo)VisitNode(explicitInfo);
				if (!object.ReferenceEquals(explicitMemberInfo, explicitInfo))
				{
					node.ExplicitInfo = explicitMemberInfo;
				}
			}
			LeaveProperty(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterProperty(Property node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveProperty(Property node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEvent(Event node)
		{
			if (!EnterEvent(node))
			{
				return;
			}
			Visit(node.Attributes);
			Method add = node.Add;
			if (null != add)
			{
				Method method = (Method)VisitNode(add);
				if (!object.ReferenceEquals(method, add))
				{
					node.Add = method;
				}
			}
			Method remove = node.Remove;
			if (null != remove)
			{
				Method method = (Method)VisitNode(remove);
				if (!object.ReferenceEquals(method, remove))
				{
					node.Remove = method;
				}
			}
			Method raise = node.Raise;
			if (null != raise)
			{
				Method method = (Method)VisitNode(raise);
				if (!object.ReferenceEquals(method, raise))
				{
					node.Raise = method;
				}
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			LeaveEvent(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterEvent(Event node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveEvent(Event node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnLocal(Local node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBlockExpression(BlockExpression node)
		{
			if (!EnterBlockExpression(node))
			{
				return;
			}
			Visit(node.Parameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			Block body = node.Body;
			if (null != body)
			{
				Block block = (Block)VisitNode(body);
				if (!object.ReferenceEquals(block, body))
				{
					node.Body = block;
				}
			}
			LeaveBlockExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterBlockExpression(BlockExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveBlockExpression(BlockExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMethod(Method node)
		{
			if (!EnterMethod(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Parameters);
			Visit(node.GenericParameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			Visit(node.ReturnTypeAttributes);
			Block body = node.Body;
			if (null != body)
			{
				Block block = (Block)VisitNode(body);
				if (!object.ReferenceEquals(block, body))
				{
					node.Body = block;
				}
			}
			Visit(node.Locals);
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				ExplicitMemberInfo explicitMemberInfo = (ExplicitMemberInfo)VisitNode(explicitInfo);
				if (!object.ReferenceEquals(explicitMemberInfo, explicitInfo))
				{
					node.ExplicitInfo = explicitMemberInfo;
				}
			}
			LeaveMethod(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterMethod(Method node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveMethod(Method node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnConstructor(Constructor node)
		{
			if (!EnterConstructor(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Parameters);
			Visit(node.GenericParameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			Visit(node.ReturnTypeAttributes);
			Block body = node.Body;
			if (null != body)
			{
				Block block = (Block)VisitNode(body);
				if (!object.ReferenceEquals(block, body))
				{
					node.Body = block;
				}
			}
			Visit(node.Locals);
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				ExplicitMemberInfo explicitMemberInfo = (ExplicitMemberInfo)VisitNode(explicitInfo);
				if (!object.ReferenceEquals(explicitMemberInfo, explicitInfo))
				{
					node.ExplicitInfo = explicitMemberInfo;
				}
			}
			LeaveConstructor(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterConstructor(Constructor node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveConstructor(Constructor node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDestructor(Destructor node)
		{
			if (!EnterDestructor(node))
			{
				return;
			}
			Visit(node.Attributes);
			Visit(node.Parameters);
			Visit(node.GenericParameters);
			TypeReference returnType = node.ReturnType;
			if (null != returnType)
			{
				TypeReference typeReference = (TypeReference)VisitNode(returnType);
				if (!object.ReferenceEquals(typeReference, returnType))
				{
					node.ReturnType = typeReference;
				}
			}
			Visit(node.ReturnTypeAttributes);
			Block body = node.Body;
			if (null != body)
			{
				Block block = (Block)VisitNode(body);
				if (!object.ReferenceEquals(block, body))
				{
					node.Body = block;
				}
			}
			Visit(node.Locals);
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				ExplicitMemberInfo explicitMemberInfo = (ExplicitMemberInfo)VisitNode(explicitInfo);
				if (!object.ReferenceEquals(explicitMemberInfo, explicitInfo))
				{
					node.ExplicitInfo = explicitMemberInfo;
				}
			}
			LeaveDestructor(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterDestructor(Destructor node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveDestructor(Destructor node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnParameterDeclaration(ParameterDeclaration node)
		{
			if (!EnterParameterDeclaration(node))
			{
				return;
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			Visit(node.Attributes);
			LeaveParameterDeclaration(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterParameterDeclaration(ParameterDeclaration node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveParameterDeclaration(ParameterDeclaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericParameterDeclaration(GenericParameterDeclaration node)
		{
			if (EnterGenericParameterDeclaration(node))
			{
				Visit(node.BaseTypes);
				LeaveGenericParameterDeclaration(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterGenericParameterDeclaration(GenericParameterDeclaration node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveGenericParameterDeclaration(GenericParameterDeclaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDeclaration(Declaration node)
		{
			if (!EnterDeclaration(node))
			{
				return;
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			LeaveDeclaration(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterDeclaration(Declaration node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveDeclaration(Declaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnAttribute(Attribute node)
		{
			if (EnterAttribute(node))
			{
				Visit(node.Arguments);
				Visit(node.NamedArguments);
				LeaveAttribute(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterAttribute(Attribute node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveAttribute(Attribute node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStatementModifier(StatementModifier node)
		{
			if (!EnterStatementModifier(node))
			{
				return;
			}
			Expression condition = node.Condition;
			if (null != condition)
			{
				Expression expression = (Expression)VisitNode(condition);
				if (!object.ReferenceEquals(expression, condition))
				{
					node.Condition = expression;
				}
			}
			LeaveStatementModifier(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterStatementModifier(StatementModifier node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveStatementModifier(StatementModifier node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGotoStatement(GotoStatement node)
		{
			if (!EnterGotoStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			ReferenceExpression label = node.Label;
			if (null != label)
			{
				ReferenceExpression referenceExpression = (ReferenceExpression)VisitNode(label);
				if (!object.ReferenceEquals(referenceExpression, label))
				{
					node.Label = referenceExpression;
				}
			}
			LeaveGotoStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterGotoStatement(GotoStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveGotoStatement(GotoStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnLabelStatement(LabelStatement node)
		{
			if (!EnterLabelStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			LeaveLabelStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterLabelStatement(LabelStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveLabelStatement(LabelStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBlock(Block node)
		{
			if (!EnterBlock(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Visit(node.Statements);
			LeaveBlock(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterBlock(Block node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveBlock(Block node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDeclarationStatement(DeclarationStatement node)
		{
			if (!EnterDeclarationStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Declaration declaration = node.Declaration;
			if (null != declaration)
			{
				Declaration declaration2 = (Declaration)VisitNode(declaration);
				if (!object.ReferenceEquals(declaration2, declaration))
				{
					node.Declaration = declaration2;
				}
			}
			Expression initializer = node.Initializer;
			if (null != initializer)
			{
				Expression expression = (Expression)VisitNode(initializer);
				if (!object.ReferenceEquals(expression, initializer))
				{
					node.Initializer = expression;
				}
			}
			LeaveDeclarationStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterDeclarationStatement(DeclarationStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveDeclarationStatement(DeclarationStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMacroStatement(MacroStatement node)
		{
			if (!EnterMacroStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Visit(node.Arguments);
			Block body = node.Body;
			if (null != body)
			{
				Block block = (Block)VisitNode(body);
				if (!object.ReferenceEquals(block, body))
				{
					node.Body = block;
				}
			}
			LeaveMacroStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterMacroStatement(MacroStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveMacroStatement(MacroStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTryStatement(TryStatement node)
		{
			if (!EnterTryStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Block protectedBlock = node.ProtectedBlock;
			if (null != protectedBlock)
			{
				Block block = (Block)VisitNode(protectedBlock);
				if (!object.ReferenceEquals(block, protectedBlock))
				{
					node.ProtectedBlock = block;
				}
			}
			Visit(node.ExceptionHandlers);
			Block failureBlock = node.FailureBlock;
			if (null != failureBlock)
			{
				Block block = (Block)VisitNode(failureBlock);
				if (!object.ReferenceEquals(block, failureBlock))
				{
					node.FailureBlock = block;
				}
			}
			Block ensureBlock = node.EnsureBlock;
			if (null != ensureBlock)
			{
				Block block = (Block)VisitNode(ensureBlock);
				if (!object.ReferenceEquals(block, ensureBlock))
				{
					node.EnsureBlock = block;
				}
			}
			LeaveTryStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterTryStatement(TryStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveTryStatement(TryStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExceptionHandler(ExceptionHandler node)
		{
			if (!EnterExceptionHandler(node))
			{
				return;
			}
			Declaration declaration = node.Declaration;
			if (null != declaration)
			{
				Declaration declaration2 = (Declaration)VisitNode(declaration);
				if (!object.ReferenceEquals(declaration2, declaration))
				{
					node.Declaration = declaration2;
				}
			}
			Expression filterCondition = node.FilterCondition;
			if (null != filterCondition)
			{
				Expression expression = (Expression)VisitNode(filterCondition);
				if (!object.ReferenceEquals(expression, filterCondition))
				{
					node.FilterCondition = expression;
				}
			}
			Block block = node.Block;
			if (null != block)
			{
				Block block2 = (Block)VisitNode(block);
				if (!object.ReferenceEquals(block2, block))
				{
					node.Block = block2;
				}
			}
			LeaveExceptionHandler(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExceptionHandler(ExceptionHandler node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExceptionHandler(ExceptionHandler node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnIfStatement(IfStatement node)
		{
			if (!EnterIfStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression condition = node.Condition;
			if (null != condition)
			{
				Expression expression = (Expression)VisitNode(condition);
				if (!object.ReferenceEquals(expression, condition))
				{
					node.Condition = expression;
				}
			}
			Block trueBlock = node.TrueBlock;
			if (null != trueBlock)
			{
				Block block = (Block)VisitNode(trueBlock);
				if (!object.ReferenceEquals(block, trueBlock))
				{
					node.TrueBlock = block;
				}
			}
			Block falseBlock = node.FalseBlock;
			if (null != falseBlock)
			{
				Block block = (Block)VisitNode(falseBlock);
				if (!object.ReferenceEquals(block, falseBlock))
				{
					node.FalseBlock = block;
				}
			}
			LeaveIfStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterIfStatement(IfStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveIfStatement(IfStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnlessStatement(UnlessStatement node)
		{
			if (!EnterUnlessStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression condition = node.Condition;
			if (null != condition)
			{
				Expression expression = (Expression)VisitNode(condition);
				if (!object.ReferenceEquals(expression, condition))
				{
					node.Condition = expression;
				}
			}
			Block block = node.Block;
			if (null != block)
			{
				Block block2 = (Block)VisitNode(block);
				if (!object.ReferenceEquals(block2, block))
				{
					node.Block = block2;
				}
			}
			LeaveUnlessStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterUnlessStatement(UnlessStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveUnlessStatement(UnlessStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnForStatement(ForStatement node)
		{
			if (!EnterForStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Visit(node.Declarations);
			Expression iterator = node.Iterator;
			if (null != iterator)
			{
				Expression expression = (Expression)VisitNode(iterator);
				if (!object.ReferenceEquals(expression, iterator))
				{
					node.Iterator = expression;
				}
			}
			Block block = node.Block;
			if (null != block)
			{
				Block block2 = (Block)VisitNode(block);
				if (!object.ReferenceEquals(block2, block))
				{
					node.Block = block2;
				}
			}
			Block orBlock = node.OrBlock;
			if (null != orBlock)
			{
				Block block2 = (Block)VisitNode(orBlock);
				if (!object.ReferenceEquals(block2, orBlock))
				{
					node.OrBlock = block2;
				}
			}
			Block thenBlock = node.ThenBlock;
			if (null != thenBlock)
			{
				Block block2 = (Block)VisitNode(thenBlock);
				if (!object.ReferenceEquals(block2, thenBlock))
				{
					node.ThenBlock = block2;
				}
			}
			LeaveForStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterForStatement(ForStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveForStatement(ForStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnWhileStatement(WhileStatement node)
		{
			if (!EnterWhileStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression condition = node.Condition;
			if (null != condition)
			{
				Expression expression = (Expression)VisitNode(condition);
				if (!object.ReferenceEquals(expression, condition))
				{
					node.Condition = expression;
				}
			}
			Block block = node.Block;
			if (null != block)
			{
				Block block2 = (Block)VisitNode(block);
				if (!object.ReferenceEquals(block2, block))
				{
					node.Block = block2;
				}
			}
			Block orBlock = node.OrBlock;
			if (null != orBlock)
			{
				Block block2 = (Block)VisitNode(orBlock);
				if (!object.ReferenceEquals(block2, orBlock))
				{
					node.OrBlock = block2;
				}
			}
			Block thenBlock = node.ThenBlock;
			if (null != thenBlock)
			{
				Block block2 = (Block)VisitNode(thenBlock);
				if (!object.ReferenceEquals(block2, thenBlock))
				{
					node.ThenBlock = block2;
				}
			}
			LeaveWhileStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterWhileStatement(WhileStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveWhileStatement(WhileStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBreakStatement(BreakStatement node)
		{
			if (!EnterBreakStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			LeaveBreakStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterBreakStatement(BreakStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveBreakStatement(BreakStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnContinueStatement(ContinueStatement node)
		{
			if (!EnterContinueStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			LeaveContinueStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterContinueStatement(ContinueStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveContinueStatement(ContinueStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnReturnStatement(ReturnStatement node)
		{
			if (!EnterReturnStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveReturnStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterReturnStatement(ReturnStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveReturnStatement(ReturnStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnYieldStatement(YieldStatement node)
		{
			if (!EnterYieldStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveYieldStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterYieldStatement(YieldStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveYieldStatement(YieldStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnRaiseStatement(RaiseStatement node)
		{
			if (!EnterRaiseStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression exception = node.Exception;
			if (null != exception)
			{
				Expression expression = (Expression)VisitNode(exception);
				if (!object.ReferenceEquals(expression, exception))
				{
					node.Exception = expression;
				}
			}
			LeaveRaiseStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterRaiseStatement(RaiseStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveRaiseStatement(RaiseStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnpackStatement(UnpackStatement node)
		{
			if (!EnterUnpackStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Visit(node.Declarations);
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveUnpackStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterUnpackStatement(UnpackStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveUnpackStatement(UnpackStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionStatement(ExpressionStatement node)
		{
			if (!EnterExpressionStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveExpressionStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExpressionStatement(ExpressionStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExpressionStatement(ExpressionStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnOmittedExpression(OmittedExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionPair(ExpressionPair node)
		{
			if (!EnterExpressionPair(node))
			{
				return;
			}
			Expression first = node.First;
			if (null != first)
			{
				Expression expression = (Expression)VisitNode(first);
				if (!object.ReferenceEquals(expression, first))
				{
					node.First = expression;
				}
			}
			Expression second = node.Second;
			if (null != second)
			{
				Expression expression = (Expression)VisitNode(second);
				if (!object.ReferenceEquals(expression, second))
				{
					node.Second = expression;
				}
			}
			LeaveExpressionPair(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExpressionPair(ExpressionPair node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExpressionPair(ExpressionPair node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			if (!EnterMethodInvocationExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			Visit(node.Arguments);
			Visit(node.NamedArguments);
			LeaveMethodInvocationExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterMethodInvocationExpression(MethodInvocationExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveMethodInvocationExpression(MethodInvocationExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnaryExpression(UnaryExpression node)
		{
			if (!EnterUnaryExpression(node))
			{
				return;
			}
			Expression operand = node.Operand;
			if (null != operand)
			{
				Expression expression = (Expression)VisitNode(operand);
				if (!object.ReferenceEquals(expression, operand))
				{
					node.Operand = expression;
				}
			}
			LeaveUnaryExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterUnaryExpression(UnaryExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveUnaryExpression(UnaryExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBinaryExpression(BinaryExpression node)
		{
			if (!EnterBinaryExpression(node))
			{
				return;
			}
			Expression left = node.Left;
			if (null != left)
			{
				Expression expression = (Expression)VisitNode(left);
				if (!object.ReferenceEquals(expression, left))
				{
					node.Left = expression;
				}
			}
			Expression right = node.Right;
			if (null != right)
			{
				Expression expression = (Expression)VisitNode(right);
				if (!object.ReferenceEquals(expression, right))
				{
					node.Right = expression;
				}
			}
			LeaveBinaryExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterBinaryExpression(BinaryExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveBinaryExpression(BinaryExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnConditionalExpression(ConditionalExpression node)
		{
			if (!EnterConditionalExpression(node))
			{
				return;
			}
			Expression condition = node.Condition;
			if (null != condition)
			{
				Expression expression = (Expression)VisitNode(condition);
				if (!object.ReferenceEquals(expression, condition))
				{
					node.Condition = expression;
				}
			}
			Expression trueValue = node.TrueValue;
			if (null != trueValue)
			{
				Expression expression = (Expression)VisitNode(trueValue);
				if (!object.ReferenceEquals(expression, trueValue))
				{
					node.TrueValue = expression;
				}
			}
			Expression falseValue = node.FalseValue;
			if (null != falseValue)
			{
				Expression expression = (Expression)VisitNode(falseValue);
				if (!object.ReferenceEquals(expression, falseValue))
				{
					node.FalseValue = expression;
				}
			}
			LeaveConditionalExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterConditionalExpression(ConditionalExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveConditionalExpression(ConditionalExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnReferenceExpression(ReferenceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			if (!EnterMemberReferenceExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			LeaveMemberReferenceExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterMemberReferenceExpression(MemberReferenceExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericReferenceExpression(GenericReferenceExpression node)
		{
			if (!EnterGenericReferenceExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			Visit(node.GenericArguments);
			LeaveGenericReferenceExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterGenericReferenceExpression(GenericReferenceExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveGenericReferenceExpression(GenericReferenceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnQuasiquoteExpression(QuasiquoteExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStringLiteralExpression(StringLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCharLiteralExpression(CharLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDoubleLiteralExpression(DoubleLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnNullLiteralExpression(NullLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnRELiteralExpression(RELiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceExpression(SpliceExpression node)
		{
			if (!EnterSpliceExpression(node))
			{
				return;
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveSpliceExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceExpression(SpliceExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceExpression(SpliceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeReference(SpliceTypeReference node)
		{
			if (!EnterSpliceTypeReference(node))
			{
				return;
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveSpliceTypeReference(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceTypeReference(SpliceTypeReference node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceTypeReference(SpliceTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			if (!EnterSpliceMemberReferenceExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			Expression nameExpression = node.NameExpression;
			if (null != nameExpression)
			{
				Expression expression = (Expression)VisitNode(nameExpression);
				if (!object.ReferenceEquals(expression, nameExpression))
				{
					node.NameExpression = expression;
				}
			}
			LeaveSpliceMemberReferenceExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeMember(SpliceTypeMember node)
		{
			if (!EnterSpliceTypeMember(node))
			{
				return;
			}
			Visit(node.Attributes);
			TypeMember typeMember = node.TypeMember;
			if (null != typeMember)
			{
				TypeMember typeMember2 = (TypeMember)VisitNode(typeMember);
				if (!object.ReferenceEquals(typeMember2, typeMember))
				{
					node.TypeMember = typeMember2;
				}
			}
			Expression nameExpression = node.NameExpression;
			if (null != nameExpression)
			{
				Expression expression = (Expression)VisitNode(nameExpression);
				if (!object.ReferenceEquals(expression, nameExpression))
				{
					node.NameExpression = expression;
				}
			}
			LeaveSpliceTypeMember(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceTypeMember(SpliceTypeMember node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceTypeMember(SpliceTypeMember node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			if (!EnterSpliceTypeDefinitionBody(node))
			{
				return;
			}
			Visit(node.Attributes);
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			LeaveSpliceTypeDefinitionBody(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
			if (!EnterSpliceParameterDeclaration(node))
			{
				return;
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			Visit(node.Attributes);
			ParameterDeclaration parameterDeclaration = node.ParameterDeclaration;
			if (null != parameterDeclaration)
			{
				ParameterDeclaration parameterDeclaration2 = (ParameterDeclaration)VisitNode(parameterDeclaration);
				if (!object.ReferenceEquals(parameterDeclaration2, parameterDeclaration))
				{
					node.ParameterDeclaration = parameterDeclaration2;
				}
			}
			Expression nameExpression = node.NameExpression;
			if (null != nameExpression)
			{
				Expression expression = (Expression)VisitNode(nameExpression);
				if (!object.ReferenceEquals(expression, nameExpression))
				{
					node.NameExpression = expression;
				}
			}
			LeaveSpliceParameterDeclaration(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			if (EnterExpressionInterpolationExpression(node))
			{
				Visit(node.Expressions);
				LeaveExpressionInterpolationExpression(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnHashLiteralExpression(HashLiteralExpression node)
		{
			if (EnterHashLiteralExpression(node))
			{
				Visit(node.Items);
				LeaveHashLiteralExpression(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterHashLiteralExpression(HashLiteralExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveHashLiteralExpression(HashLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnListLiteralExpression(ListLiteralExpression node)
		{
			if (EnterListLiteralExpression(node))
			{
				Visit(node.Items);
				LeaveListLiteralExpression(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterListLiteralExpression(ListLiteralExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveListLiteralExpression(ListLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			if (!EnterCollectionInitializationExpression(node))
			{
				return;
			}
			Expression collection = node.Collection;
			if (null != collection)
			{
				Expression expression = (Expression)VisitNode(collection);
				if (!object.ReferenceEquals(expression, collection))
				{
					node.Collection = expression;
				}
			}
			Expression initializer = node.Initializer;
			if (null != initializer)
			{
				Expression expression = (Expression)VisitNode(initializer);
				if (!object.ReferenceEquals(expression, initializer))
				{
					node.Initializer = expression;
				}
			}
			LeaveCollectionInitializationExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCollectionInitializationExpression(CollectionInitializationExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			if (!EnterArrayLiteralExpression(node))
			{
				return;
			}
			Visit(node.Items);
			ArrayTypeReference type = node.Type;
			if (null != type)
			{
				ArrayTypeReference arrayTypeReference = (ArrayTypeReference)VisitNode(type);
				if (!object.ReferenceEquals(arrayTypeReference, type))
				{
					node.Type = arrayTypeReference;
				}
			}
			LeaveArrayLiteralExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterArrayLiteralExpression(ArrayLiteralExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveArrayLiteralExpression(ArrayLiteralExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGeneratorExpression(GeneratorExpression node)
		{
			if (!EnterGeneratorExpression(node))
			{
				return;
			}
			Expression expression = node.Expression;
			if (null != expression)
			{
				Expression expression2 = (Expression)VisitNode(expression);
				if (!object.ReferenceEquals(expression2, expression))
				{
					node.Expression = expression2;
				}
			}
			Visit(node.Declarations);
			Expression iterator = node.Iterator;
			if (null != iterator)
			{
				Expression expression2 = (Expression)VisitNode(iterator);
				if (!object.ReferenceEquals(expression2, iterator))
				{
					node.Iterator = expression2;
				}
			}
			StatementModifier filter = node.Filter;
			if (null != filter)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(filter);
				if (!object.ReferenceEquals(statementModifier, filter))
				{
					node.Filter = statementModifier;
				}
			}
			LeaveGeneratorExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterGeneratorExpression(GeneratorExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveGeneratorExpression(GeneratorExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			if (EnterExtendedGeneratorExpression(node))
			{
				Visit(node.Items);
				LeaveExtendedGeneratorExpression(node);
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSlice(Slice node)
		{
			if (!EnterSlice(node))
			{
				return;
			}
			Expression begin = node.Begin;
			if (null != begin)
			{
				Expression expression = (Expression)VisitNode(begin);
				if (!object.ReferenceEquals(expression, begin))
				{
					node.Begin = expression;
				}
			}
			Expression end = node.End;
			if (null != end)
			{
				Expression expression = (Expression)VisitNode(end);
				if (!object.ReferenceEquals(expression, end))
				{
					node.End = expression;
				}
			}
			Expression step = node.Step;
			if (null != step)
			{
				Expression expression = (Expression)VisitNode(step);
				if (!object.ReferenceEquals(expression, step))
				{
					node.Step = expression;
				}
			}
			LeaveSlice(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSlice(Slice node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSlice(Slice node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSlicingExpression(SlicingExpression node)
		{
			if (!EnterSlicingExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			Visit(node.Indices);
			LeaveSlicingExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterSlicingExpression(SlicingExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveSlicingExpression(SlicingExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTryCastExpression(TryCastExpression node)
		{
			if (!EnterTryCastExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			LeaveTryCastExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterTryCastExpression(TryCastExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveTryCastExpression(TryCastExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCastExpression(CastExpression node)
		{
			if (!EnterCastExpression(node))
			{
				return;
			}
			Expression target = node.Target;
			if (null != target)
			{
				Expression expression = (Expression)VisitNode(target);
				if (!object.ReferenceEquals(expression, target))
				{
					node.Target = expression;
				}
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			LeaveCastExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCastExpression(CastExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCastExpression(CastExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTypeofExpression(TypeofExpression node)
		{
			if (!EnterTypeofExpression(node))
			{
				return;
			}
			TypeReference type = node.Type;
			if (null != type)
			{
				TypeReference typeReference = (TypeReference)VisitNode(type);
				if (!object.ReferenceEquals(typeReference, type))
				{
					node.Type = typeReference;
				}
			}
			LeaveTypeofExpression(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterTypeofExpression(TypeofExpression node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveTypeofExpression(TypeofExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCustomStatement(CustomStatement node)
		{
			if (!EnterCustomStatement(node))
			{
				return;
			}
			StatementModifier modifier = node.Modifier;
			if (null != modifier)
			{
				StatementModifier statementModifier = (StatementModifier)VisitNode(modifier);
				if (!object.ReferenceEquals(statementModifier, modifier))
				{
					node.Modifier = statementModifier;
				}
			}
			LeaveCustomStatement(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterCustomStatement(CustomStatement node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveCustomStatement(CustomStatement node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCustomExpression(CustomExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStatementTypeMember(StatementTypeMember node)
		{
			if (!EnterStatementTypeMember(node))
			{
				return;
			}
			Visit(node.Attributes);
			Statement statement = node.Statement;
			if (null != statement)
			{
				Statement statement2 = (Statement)VisitNode(statement);
				if (!object.ReferenceEquals(statement2, statement))
				{
					node.Statement = statement2;
				}
			}
			LeaveStatementTypeMember(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual bool EnterStatementTypeMember(StatementTypeMember node)
		{
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void LeaveStatementTypeMember(StatementTypeMember node)
		{
		}

		protected virtual void RemoveCurrentNode()
		{
			_resultingNode = null;
		}

		protected virtual void ReplaceCurrentNode(Node replacement)
		{
			_resultingNode = replacement;
		}

		protected virtual void OnNode(Node node)
		{
			node.Accept(this);
		}

		public virtual Node VisitNode(Node node)
		{
			if (null != node)
			{
				try
				{
					Node resultingNode = _resultingNode;
					_resultingNode = node;
					OnNode(node);
					Node resultingNode2 = _resultingNode;
					_resultingNode = resultingNode;
					return resultingNode2;
				}
				catch (LongJumpException)
				{
					throw;
				}
				catch (CompilerError)
				{
					throw;
				}
				catch (Exception error)
				{
					OnError(node, error);
				}
			}
			return null;
		}

		protected bool VisitAllowingCancellation(Node node, out Node resultingNode)
		{
			try
			{
				resultingNode = VisitNode(node);
				return true;
			}
			catch (LongJumpException)
			{
				resultingNode = null;
			}
			return false;
		}

		protected bool VisitAllowingCancellation(Node node)
		{
			Node resultingNode;
			return VisitAllowingCancellation(node, out resultingNode);
		}

		protected void Cancel()
		{
			throw CancellationException;
		}

		protected virtual void OnError(Node node, Exception error)
		{
			throw CompilerErrorFactory.InternalError(node, error);
		}

		public Node Visit(Node node)
		{
			return VisitNode(node);
		}

		public TypeMember Visit(TypeMember node)
		{
			return (TypeMember)VisitNode(node);
		}

		public Expression Visit(Expression node)
		{
			return (Expression)VisitNode(node);
		}

		public Statement Visit(Statement node)
		{
			return (Statement)VisitNode(node);
		}

		public bool Visit<T>(NodeCollection<T> collection) where T : Node
		{
			if (null == collection)
			{
				return false;
			}
			T[] array = collection.ToArray();
			T[] array2 = array;
			foreach (T val in array2)
			{
				T val2 = (T)VisitNode(val);
				if (val != val2)
				{
					collection.Replace(val, val2);
				}
			}
			return true;
		}
	}
}
