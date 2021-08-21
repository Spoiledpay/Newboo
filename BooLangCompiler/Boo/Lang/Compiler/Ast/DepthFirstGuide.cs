using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	public class DepthFirstGuide : IAstVisitor
	{
		public event NodeEvent<CompileUnit> OnCompileUnit;

		public event NodeEvent<TypeMemberStatement> OnTypeMemberStatement;

		public event NodeEvent<ExplicitMemberInfo> OnExplicitMemberInfo;

		public event NodeEvent<SimpleTypeReference> OnSimpleTypeReference;

		public event NodeEvent<ArrayTypeReference> OnArrayTypeReference;

		public event NodeEvent<CallableTypeReference> OnCallableTypeReference;

		public event NodeEvent<GenericTypeReference> OnGenericTypeReference;

		public event NodeEvent<GenericTypeDefinitionReference> OnGenericTypeDefinitionReference;

		public event NodeEvent<CallableDefinition> OnCallableDefinition;

		public event NodeEvent<NamespaceDeclaration> OnNamespaceDeclaration;

		public event NodeEvent<Import> OnImport;

		public event NodeEvent<Module> OnModule;

		public event NodeEvent<ClassDefinition> OnClassDefinition;

		public event NodeEvent<StructDefinition> OnStructDefinition;

		public event NodeEvent<InterfaceDefinition> OnInterfaceDefinition;

		public event NodeEvent<EnumDefinition> OnEnumDefinition;

		public event NodeEvent<EnumMember> OnEnumMember;

		public event NodeEvent<Field> OnField;

		public event NodeEvent<Property> OnProperty;

		public event NodeEvent<Event> OnEvent;

		public event NodeEvent<Local> OnLocal;

		public event NodeEvent<BlockExpression> OnBlockExpression;

		public event NodeEvent<Method> OnMethod;

		public event NodeEvent<Constructor> OnConstructor;

		public event NodeEvent<Destructor> OnDestructor;

		public event NodeEvent<ParameterDeclaration> OnParameterDeclaration;

		public event NodeEvent<GenericParameterDeclaration> OnGenericParameterDeclaration;

		public event NodeEvent<Declaration> OnDeclaration;

		public event NodeEvent<Attribute> OnAttribute;

		public event NodeEvent<StatementModifier> OnStatementModifier;

		public event NodeEvent<GotoStatement> OnGotoStatement;

		public event NodeEvent<LabelStatement> OnLabelStatement;

		public event NodeEvent<Block> OnBlock;

		public event NodeEvent<DeclarationStatement> OnDeclarationStatement;

		public event NodeEvent<MacroStatement> OnMacroStatement;

		public event NodeEvent<TryStatement> OnTryStatement;

		public event NodeEvent<ExceptionHandler> OnExceptionHandler;

		public event NodeEvent<IfStatement> OnIfStatement;

		public event NodeEvent<UnlessStatement> OnUnlessStatement;

		public event NodeEvent<ForStatement> OnForStatement;

		public event NodeEvent<WhileStatement> OnWhileStatement;

		public event NodeEvent<BreakStatement> OnBreakStatement;

		public event NodeEvent<ContinueStatement> OnContinueStatement;

		public event NodeEvent<ReturnStatement> OnReturnStatement;

		public event NodeEvent<YieldStatement> OnYieldStatement;

		public event NodeEvent<RaiseStatement> OnRaiseStatement;

		public event NodeEvent<UnpackStatement> OnUnpackStatement;

		public event NodeEvent<ExpressionStatement> OnExpressionStatement;

		public event NodeEvent<OmittedExpression> OnOmittedExpression;

		public event NodeEvent<ExpressionPair> OnExpressionPair;

		public event NodeEvent<MethodInvocationExpression> OnMethodInvocationExpression;

		public event NodeEvent<UnaryExpression> OnUnaryExpression;

		public event NodeEvent<BinaryExpression> OnBinaryExpression;

		public event NodeEvent<ConditionalExpression> OnConditionalExpression;

		public event NodeEvent<ReferenceExpression> OnReferenceExpression;

		public event NodeEvent<MemberReferenceExpression> OnMemberReferenceExpression;

		public event NodeEvent<GenericReferenceExpression> OnGenericReferenceExpression;

		public event NodeEvent<QuasiquoteExpression> OnQuasiquoteExpression;

		public event NodeEvent<StringLiteralExpression> OnStringLiteralExpression;

		public event NodeEvent<CharLiteralExpression> OnCharLiteralExpression;

		public event NodeEvent<TimeSpanLiteralExpression> OnTimeSpanLiteralExpression;

		public event NodeEvent<IntegerLiteralExpression> OnIntegerLiteralExpression;

		public event NodeEvent<DoubleLiteralExpression> OnDoubleLiteralExpression;

		public event NodeEvent<NullLiteralExpression> OnNullLiteralExpression;

		public event NodeEvent<SelfLiteralExpression> OnSelfLiteralExpression;

		public event NodeEvent<SuperLiteralExpression> OnSuperLiteralExpression;

		public event NodeEvent<BoolLiteralExpression> OnBoolLiteralExpression;

		public event NodeEvent<RELiteralExpression> OnRELiteralExpression;

		public event NodeEvent<SpliceExpression> OnSpliceExpression;

		public event NodeEvent<SpliceTypeReference> OnSpliceTypeReference;

		public event NodeEvent<SpliceMemberReferenceExpression> OnSpliceMemberReferenceExpression;

		public event NodeEvent<SpliceTypeMember> OnSpliceTypeMember;

		public event NodeEvent<SpliceTypeDefinitionBody> OnSpliceTypeDefinitionBody;

		public event NodeEvent<SpliceParameterDeclaration> OnSpliceParameterDeclaration;

		public event NodeEvent<ExpressionInterpolationExpression> OnExpressionInterpolationExpression;

		public event NodeEvent<HashLiteralExpression> OnHashLiteralExpression;

		public event NodeEvent<ListLiteralExpression> OnListLiteralExpression;

		public event NodeEvent<CollectionInitializationExpression> OnCollectionInitializationExpression;

		public event NodeEvent<ArrayLiteralExpression> OnArrayLiteralExpression;

		public event NodeEvent<GeneratorExpression> OnGeneratorExpression;

		public event NodeEvent<ExtendedGeneratorExpression> OnExtendedGeneratorExpression;

		public event NodeEvent<Slice> OnSlice;

		public event NodeEvent<SlicingExpression> OnSlicingExpression;

		public event NodeEvent<TryCastExpression> OnTryCastExpression;

		public event NodeEvent<CastExpression> OnCastExpression;

		public event NodeEvent<TypeofExpression> OnTypeofExpression;

		public event NodeEvent<CustomStatement> OnCustomStatement;

		public event NodeEvent<CustomExpression> OnCustomExpression;

		public event NodeEvent<StatementTypeMember> OnStatementTypeMember;

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCompileUnit(CompileUnit node)
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
			this.OnCompileUnit?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnTypeMemberStatement(TypeMemberStatement node)
		{
			node.Modifier?.Accept(this);
			node.TypeMember?.Accept(this);
			this.OnTypeMemberStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			node.InterfaceType?.Accept(this);
			this.OnExplicitMemberInfo?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSimpleTypeReference(SimpleTypeReference node)
		{
			this.OnSimpleTypeReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnArrayTypeReference(ArrayTypeReference node)
		{
			node.ElementType?.Accept(this);
			node.Rank?.Accept(this);
			this.OnArrayTypeReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCallableTypeReference(CallableTypeReference node)
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
			this.OnCallableTypeReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGenericTypeReference(GenericTypeReference node)
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
			this.OnGenericTypeReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
			this.OnGenericTypeDefinitionReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCallableDefinition(CallableDefinition node)
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
			this.OnCallableDefinition?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnNamespaceDeclaration(NamespaceDeclaration node)
		{
			this.OnNamespaceDeclaration?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnImport(Import node)
		{
			node.Expression?.Accept(this);
			node.AssemblyReference?.Accept(this);
			node.Alias?.Accept(this);
			this.OnImport?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnModule(Module node)
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
			this.OnModule?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnClassDefinition(ClassDefinition node)
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
			this.OnClassDefinition?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnStructDefinition(StructDefinition node)
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
			this.OnStructDefinition?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnInterfaceDefinition(InterfaceDefinition node)
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
			this.OnInterfaceDefinition?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnEnumDefinition(EnumDefinition node)
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
			this.OnEnumDefinition?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnEnumMember(EnumMember node)
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
			this.OnEnumMember?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnField(Field node)
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
			this.OnField?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnProperty(Property node)
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
			this.OnProperty?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnEvent(Event node)
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
			this.OnEvent?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnLocal(Local node)
		{
			this.OnLocal?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnBlockExpression(BlockExpression node)
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
			this.OnBlockExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnMethod(Method node)
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
			this.OnMethod?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnConstructor(Constructor node)
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
			this.OnConstructor?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnDestructor(Destructor node)
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
			this.OnDestructor?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnParameterDeclaration(ParameterDeclaration node)
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
			this.OnParameterDeclaration?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGenericParameterDeclaration(GenericParameterDeclaration node)
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
			this.OnGenericParameterDeclaration?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnDeclaration(Declaration node)
		{
			node.Type?.Accept(this);
			this.OnDeclaration?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnAttribute(Attribute node)
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
			this.OnAttribute?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnStatementModifier(StatementModifier node)
		{
			node.Condition?.Accept(this);
			this.OnStatementModifier?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGotoStatement(GotoStatement node)
		{
			node.Modifier?.Accept(this);
			node.Label?.Accept(this);
			this.OnGotoStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnLabelStatement(LabelStatement node)
		{
			node.Modifier?.Accept(this);
			this.OnLabelStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnBlock(Block node)
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
			this.OnBlock?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnDeclarationStatement(DeclarationStatement node)
		{
			node.Modifier?.Accept(this);
			node.Declaration?.Accept(this);
			node.Initializer?.Accept(this);
			this.OnDeclarationStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnMacroStatement(MacroStatement node)
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
			this.OnMacroStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnTryStatement(TryStatement node)
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
			this.OnTryStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExceptionHandler(ExceptionHandler node)
		{
			node.Declaration?.Accept(this);
			node.FilterCondition?.Accept(this);
			node.Block?.Accept(this);
			this.OnExceptionHandler?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnIfStatement(IfStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.TrueBlock?.Accept(this);
			node.FalseBlock?.Accept(this);
			this.OnIfStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnUnlessStatement(UnlessStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.Block?.Accept(this);
			this.OnUnlessStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnForStatement(ForStatement node)
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
			this.OnForStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnWhileStatement(WhileStatement node)
		{
			node.Modifier?.Accept(this);
			node.Condition?.Accept(this);
			node.Block?.Accept(this);
			node.OrBlock?.Accept(this);
			node.ThenBlock?.Accept(this);
			this.OnWhileStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnBreakStatement(BreakStatement node)
		{
			node.Modifier?.Accept(this);
			this.OnBreakStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnContinueStatement(ContinueStatement node)
		{
			node.Modifier?.Accept(this);
			this.OnContinueStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnReturnStatement(ReturnStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
			this.OnReturnStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnYieldStatement(YieldStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
			this.OnYieldStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnRaiseStatement(RaiseStatement node)
		{
			node.Modifier?.Accept(this);
			node.Exception?.Accept(this);
			this.OnRaiseStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnUnpackStatement(UnpackStatement node)
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
			this.OnUnpackStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExpressionStatement(ExpressionStatement node)
		{
			node.Modifier?.Accept(this);
			node.Expression?.Accept(this);
			this.OnExpressionStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnOmittedExpression(OmittedExpression node)
		{
			this.OnOmittedExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExpressionPair(ExpressionPair node)
		{
			node.First?.Accept(this);
			node.Second?.Accept(this);
			this.OnExpressionPair?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnMethodInvocationExpression(MethodInvocationExpression node)
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
			this.OnMethodInvocationExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnUnaryExpression(UnaryExpression node)
		{
			node.Operand?.Accept(this);
			this.OnUnaryExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnBinaryExpression(BinaryExpression node)
		{
			node.Left?.Accept(this);
			node.Right?.Accept(this);
			this.OnBinaryExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnConditionalExpression(ConditionalExpression node)
		{
			node.Condition?.Accept(this);
			node.TrueValue?.Accept(this);
			node.FalseValue?.Accept(this);
			this.OnConditionalExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnReferenceExpression(ReferenceExpression node)
		{
			this.OnReferenceExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			node.Target?.Accept(this);
			this.OnMemberReferenceExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGenericReferenceExpression(GenericReferenceExpression node)
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
			this.OnGenericReferenceExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnQuasiquoteExpression(QuasiquoteExpression node)
		{
			this.OnQuasiquoteExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnStringLiteralExpression(StringLiteralExpression node)
		{
			this.OnStringLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCharLiteralExpression(CharLiteralExpression node)
		{
			this.OnCharLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			this.OnTimeSpanLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
			this.OnIntegerLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnDoubleLiteralExpression(DoubleLiteralExpression node)
		{
			this.OnDoubleLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnNullLiteralExpression(NullLiteralExpression node)
		{
			this.OnNullLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			this.OnSelfLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			this.OnSuperLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			this.OnBoolLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnRELiteralExpression(RELiteralExpression node)
		{
			this.OnRELiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceExpression(SpliceExpression node)
		{
			node.Expression?.Accept(this);
			this.OnSpliceExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceTypeReference(SpliceTypeReference node)
		{
			node.Expression?.Accept(this);
			this.OnSpliceTypeReference?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			node.Target?.Accept(this);
			node.NameExpression?.Accept(this);
			this.OnSpliceMemberReferenceExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceTypeMember(SpliceTypeMember node)
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
			this.OnSpliceTypeMember?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
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
			this.OnSpliceTypeDefinitionBody?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSpliceParameterDeclaration(SpliceParameterDeclaration node)
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
			this.OnSpliceParameterDeclaration?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
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
			this.OnExpressionInterpolationExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnHashLiteralExpression(HashLiteralExpression node)
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
			this.OnHashLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnListLiteralExpression(ListLiteralExpression node)
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
			this.OnListLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			node.Collection?.Accept(this);
			node.Initializer?.Accept(this);
			this.OnCollectionInitializationExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnArrayLiteralExpression(ArrayLiteralExpression node)
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
			this.OnArrayLiteralExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnGeneratorExpression(GeneratorExpression node)
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
			this.OnGeneratorExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
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
			this.OnExtendedGeneratorExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSlice(Slice node)
		{
			node.Begin?.Accept(this);
			node.End?.Accept(this);
			node.Step?.Accept(this);
			this.OnSlice?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnSlicingExpression(SlicingExpression node)
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
			this.OnSlicingExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnTryCastExpression(TryCastExpression node)
		{
			node.Target?.Accept(this);
			node.Type?.Accept(this);
			this.OnTryCastExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCastExpression(CastExpression node)
		{
			node.Target?.Accept(this);
			node.Type?.Accept(this);
			this.OnCastExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnTypeofExpression(TypeofExpression node)
		{
			node.Type?.Accept(this);
			this.OnTypeofExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCustomStatement(CustomStatement node)
		{
			node.Modifier?.Accept(this);
			this.OnCustomStatement?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnCustomExpression(CustomExpression node)
		{
			this.OnCustomExpression?.Invoke(node);
		}

		[GeneratedCode("astgen.boo", "1")]
		void IAstVisitor.OnStatementTypeMember(StatementTypeMember node)
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
			this.OnStatementTypeMember?.Invoke(node);
		}
	}
}
