using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	public class FastDepthFirstVisitor : IAstVisitor
	{
		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCompileUnit(CompileUnit node)
		{
			ModuleCollection modules = node.Modules;
			if (modules != null)
			{
				List<Module> innerList = modules.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTypeMemberStatement(TypeMemberStatement node)
		{
			node.Modifier?.Accept(this);
			node.TypeMember?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			node.InterfaceType?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSimpleTypeReference(SimpleTypeReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnArrayTypeReference(ArrayTypeReference node)
		{
			node.ElementType?.Accept(this);
			node.Rank?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCallableTypeReference(CallableTypeReference node)
		{
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList = parameters.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericTypeReference(GenericTypeReference node)
		{
			TypeReferenceCollection genericArguments = node.GenericArguments;
			if (genericArguments != null)
			{
				List<TypeReference> innerList = genericArguments.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCallableDefinition(CallableDefinition node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList2 = parameters.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList3 = genericParameters.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
			AttributeCollection returnTypeAttributes = node.ReturnTypeAttributes;
			if (returnTypeAttributes != null)
			{
				List<Attribute> innerList = returnTypeAttributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnImport(Import node)
		{
			node.Expression?.Accept(this);
			node.AssemblyReference?.Accept(this);
			node.Alias?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnModule(Module node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			TypeMemberCollection members = node.Members;
			if (members != null)
			{
				List<TypeMember> innerList2 = members.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList3 = baseTypes.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList4 = genericParameters.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
			node.Namespace?.Accept(this);
			ImportCollection imports = node.Imports;
			if (imports != null)
			{
				List<Import> innerList5 = imports.InnerList;
				int count = innerList5.Count;
				for (int i = 0; i < count; i++)
				{
					innerList5.FastAt(i).Accept(this);
				}
			}
			node.Globals?.Accept(this);
			AttributeCollection assemblyAttributes = node.AssemblyAttributes;
			if (assemblyAttributes != null)
			{
				List<Attribute> innerList = assemblyAttributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnClassDefinition(ClassDefinition node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			TypeMemberCollection members = node.Members;
			if (members != null)
			{
				List<TypeMember> innerList2 = members.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList3 = baseTypes.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList4 = genericParameters.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStructDefinition(StructDefinition node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			TypeMemberCollection members = node.Members;
			if (members != null)
			{
				List<TypeMember> innerList2 = members.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList3 = baseTypes.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList4 = genericParameters.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnInterfaceDefinition(InterfaceDefinition node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			TypeMemberCollection members = node.Members;
			if (members != null)
			{
				List<TypeMember> innerList2 = members.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList3 = baseTypes.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList4 = genericParameters.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEnumDefinition(EnumDefinition node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			TypeMemberCollection members = node.Members;
			if (members != null)
			{
				List<TypeMember> innerList2 = members.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList3 = baseTypes.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList4 = genericParameters.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEnumMember(EnumMember node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Initializer?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnField(Field node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Type?.Accept(this);
			node.Initializer?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnProperty(Property node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList2 = parameters.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			node.Getter?.Accept(this);
			node.Setter?.Accept(this);
			node.Type?.Accept(this);
			node.ExplicitInfo?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnEvent(Event node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Add?.Accept(this);
			node.Remove?.Accept(this);
			node.Raise?.Accept(this);
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnLocal(Local node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBlockExpression(BlockExpression node)
		{
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList = parameters.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
			node.Body?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMethod(Method node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList2 = parameters.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList3 = genericParameters.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
			AttributeCollection returnTypeAttributes = node.ReturnTypeAttributes;
			if (returnTypeAttributes != null)
			{
				List<Attribute> innerList = returnTypeAttributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Body?.Accept(this);
			LocalCollection locals = node.Locals;
			if (locals != null)
			{
				List<Local> innerList4 = locals.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
			node.ExplicitInfo?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnConstructor(Constructor node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList2 = parameters.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList3 = genericParameters.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
			AttributeCollection returnTypeAttributes = node.ReturnTypeAttributes;
			if (returnTypeAttributes != null)
			{
				List<Attribute> innerList = returnTypeAttributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Body?.Accept(this);
			LocalCollection locals = node.Locals;
			if (locals != null)
			{
				List<Local> innerList4 = locals.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
			node.ExplicitInfo?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDestructor(Destructor node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ParameterDeclarationCollection parameters = node.Parameters;
			if (parameters != null)
			{
				List<ParameterDeclaration> innerList2 = parameters.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
			GenericParameterDeclarationCollection genericParameters = node.GenericParameters;
			if (genericParameters != null)
			{
				List<GenericParameterDeclaration> innerList3 = genericParameters.InnerList;
				int count = innerList3.Count;
				for (int i = 0; i < count; i++)
				{
					innerList3.FastAt(i).Accept(this);
				}
			}
			node.ReturnType?.Accept(this);
			AttributeCollection returnTypeAttributes = node.ReturnTypeAttributes;
			if (returnTypeAttributes != null)
			{
				List<Attribute> innerList = returnTypeAttributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Body?.Accept(this);
			LocalCollection locals = node.Locals;
			if (locals != null)
			{
				List<Local> innerList4 = locals.InnerList;
				int count = innerList4.Count;
				for (int i = 0; i < count; i++)
				{
					innerList4.FastAt(i).Accept(this);
				}
			}
			node.ExplicitInfo?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnParameterDeclaration(ParameterDeclaration node)
		{
			node.Type?.Accept(this);
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericParameterDeclaration(GenericParameterDeclaration node)
		{
			TypeReferenceCollection baseTypes = node.BaseTypes;
			if (baseTypes != null)
			{
				List<TypeReference> innerList = baseTypes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDeclaration(Declaration node)
		{
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnAttribute(Attribute node)
		{
			ExpressionCollection arguments = node.Arguments;
			if (arguments != null)
			{
				List<Expression> innerList = arguments.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ExpressionPairCollection namedArguments = node.NamedArguments;
			if (namedArguments != null)
			{
				List<ExpressionPair> innerList2 = namedArguments.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStatementModifier(StatementModifier node)
		{
			node.Condition?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGotoStatement(GotoStatement node)
		{
			node.Modifier?.Accept(this);
			node.Label?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnLabelStatement(LabelStatement node)
		{
			node.Modifier?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBlock(Block node)
		{
			node.Modifier?.Accept(this);
			StatementCollection statements = node.Statements;
			if (statements != null)
			{
				List<Statement> innerList = statements.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnDeclarationStatement(DeclarationStatement node)
		{
			node.Modifier?.Accept(this);
			node.Declaration?.Accept(this);
			node.Initializer?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMacroStatement(MacroStatement node)
		{
			node.Modifier?.Accept(this);
			ExpressionCollection arguments = node.Arguments;
			if (arguments != null)
			{
				List<Expression> innerList = arguments.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Body?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTryStatement(TryStatement node)
		{
			node.Modifier?.Accept(this);
			node.ProtectedBlock?.Accept(this);
			ExceptionHandlerCollection exceptionHandlers = node.ExceptionHandlers;
			if (exceptionHandlers != null)
			{
				List<ExceptionHandler> innerList = exceptionHandlers.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.FailureBlock?.Accept(this);
			node.EnsureBlock?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExceptionHandler(ExceptionHandler node)
		{
			node.Declaration?.Accept(this);
			node.FilterCondition?.Accept(this);
			node.Block?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnIfStatement(IfStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.TrueBlock?.Accept(this);
			node.FalseBlock?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnlessStatement(UnlessStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.Block?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnForStatement(ForStatement node)
		{
			node.Modifier?.Accept(this);
			DeclarationCollection declarations = node.Declarations;
			if (declarations != null)
			{
				List<Declaration> innerList = declarations.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Iterator?.Accept(this);
			node.Block?.Accept(this);
			node.OrBlock?.Accept(this);
			node.ThenBlock?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnWhileStatement(WhileStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.Block?.Accept(this);
			node.OrBlock?.Accept(this);
			node.ThenBlock?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBreakStatement(BreakStatement node)
		{
			node.Modifier?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnContinueStatement(ContinueStatement node)
		{
			node.Modifier?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnReturnStatement(ReturnStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnYieldStatement(YieldStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnRaiseStatement(RaiseStatement node)
		{
			node.Modifier?.Accept(this);
			node.Exception?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnpackStatement(UnpackStatement node)
		{
			node.Modifier?.Accept(this);
			DeclarationCollection declarations = node.Declarations;
			if (declarations != null)
			{
				List<Declaration> innerList = declarations.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionStatement(ExpressionStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnOmittedExpression(OmittedExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionPair(ExpressionPair node)
		{
			node.First?.Accept(this);
			node.Second?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			node.Target?.Accept(this);
			ExpressionCollection arguments = node.Arguments;
			if (arguments != null)
			{
				List<Expression> innerList = arguments.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			ExpressionPairCollection namedArguments = node.NamedArguments;
			if (namedArguments != null)
			{
				List<ExpressionPair> innerList2 = namedArguments.InnerList;
				int count = innerList2.Count;
				for (int i = 0; i < count; i++)
				{
					innerList2.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnUnaryExpression(UnaryExpression node)
		{
			node.Operand?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnBinaryExpression(BinaryExpression node)
		{
			node.Left?.Accept(this);
			node.Right?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnConditionalExpression(ConditionalExpression node)
		{
			node.Condition?.Accept(this);
			node.TrueValue?.Accept(this);
			node.FalseValue?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnReferenceExpression(ReferenceExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			node.Target?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGenericReferenceExpression(GenericReferenceExpression node)
		{
			node.Target?.Accept(this);
			TypeReferenceCollection genericArguments = node.GenericArguments;
			if (genericArguments != null)
			{
				List<TypeReference> innerList = genericArguments.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
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
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeReference(SpliceTypeReference node)
		{
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			node.Target?.Accept(this);
			node.NameExpression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeMember(SpliceTypeMember node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.TypeMember?.Accept(this);
			node.NameExpression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Expression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
			node.Type?.Accept(this);
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.ParameterDeclaration?.Accept(this);
			node.NameExpression?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			ExpressionCollection expressions = node.Expressions;
			if (expressions != null)
			{
				List<Expression> innerList = expressions.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnHashLiteralExpression(HashLiteralExpression node)
		{
			ExpressionPairCollection items = node.Items;
			if (items != null)
			{
				List<ExpressionPair> innerList = items.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnListLiteralExpression(ListLiteralExpression node)
		{
			ExpressionCollection items = node.Items;
			if (items != null)
			{
				List<Expression> innerList = items.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			node.Collection?.Accept(this);
			node.Initializer?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			ExpressionCollection items = node.Items;
			if (items != null)
			{
				List<Expression> innerList = items.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnGeneratorExpression(GeneratorExpression node)
		{
			node.Expression?.Accept(this);
			DeclarationCollection declarations = node.Declarations;
			if (declarations != null)
			{
				List<Declaration> innerList = declarations.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Iterator?.Accept(this);
			node.Filter?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			GeneratorExpressionCollection items = node.Items;
			if (items != null)
			{
				List<GeneratorExpression> innerList = items.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSlice(Slice node)
		{
			node.Begin?.Accept(this);
			node.End?.Accept(this);
			node.Step?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnSlicingExpression(SlicingExpression node)
		{
			node.Target?.Accept(this);
			SliceCollection indices = node.Indices;
			if (indices != null)
			{
				List<Slice> innerList = indices.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTryCastExpression(TryCastExpression node)
		{
			node.Target?.Accept(this);
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCastExpression(CastExpression node)
		{
			node.Target?.Accept(this);
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnTypeofExpression(TypeofExpression node)
		{
			node.Type?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCustomStatement(CustomStatement node)
		{
			node.Modifier?.Accept(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnCustomExpression(CustomExpression node)
		{
		}

		[GeneratedCode("astgen.boo", "1")]
		public virtual void OnStatementTypeMember(StatementTypeMember node)
		{
			AttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				List<Attribute> innerList = attributes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					innerList.FastAt(i).Accept(this);
				}
			}
			node.Statement?.Accept(this);
		}

		protected virtual void Visit(Node node)
		{
			node?.Accept(this);
		}

		protected virtual void Visit<T>(NodeCollection<T> nodes) where T : Node
		{
			if (nodes != null)
			{
				List<T> innerList = nodes.InnerList;
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					T val = innerList.FastAt(i);
					val.Accept(this);
				}
			}
		}
	}
}
