namespace Boo.Lang.Compiler.Ast
{
	public interface IAstVisitor
	{
		void OnCompileUnit(CompileUnit node);

		void OnTypeMemberStatement(TypeMemberStatement node);

		void OnExplicitMemberInfo(ExplicitMemberInfo node);

		void OnSimpleTypeReference(SimpleTypeReference node);

		void OnArrayTypeReference(ArrayTypeReference node);

		void OnCallableTypeReference(CallableTypeReference node);

		void OnGenericTypeReference(GenericTypeReference node);

		void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node);

		void OnCallableDefinition(CallableDefinition node);

		void OnNamespaceDeclaration(NamespaceDeclaration node);

		void OnImport(Import node);

		void OnModule(Module node);

		void OnClassDefinition(ClassDefinition node);

		void OnStructDefinition(StructDefinition node);

		void OnInterfaceDefinition(InterfaceDefinition node);

		void OnEnumDefinition(EnumDefinition node);

		void OnEnumMember(EnumMember node);

		void OnField(Field node);

		void OnProperty(Property node);

		void OnEvent(Event node);

		void OnLocal(Local node);

		void OnBlockExpression(BlockExpression node);

		void OnMethod(Method node);

		void OnConstructor(Constructor node);

		void OnDestructor(Destructor node);

		void OnParameterDeclaration(ParameterDeclaration node);

		void OnGenericParameterDeclaration(GenericParameterDeclaration node);

		void OnDeclaration(Declaration node);

		void OnAttribute(Attribute node);

		void OnStatementModifier(StatementModifier node);

		void OnGotoStatement(GotoStatement node);

		void OnLabelStatement(LabelStatement node);

		void OnBlock(Block node);

		void OnDeclarationStatement(DeclarationStatement node);

		void OnMacroStatement(MacroStatement node);

		void OnTryStatement(TryStatement node);

		void OnExceptionHandler(ExceptionHandler node);

		void OnIfStatement(IfStatement node);

		void OnUnlessStatement(UnlessStatement node);

		void OnForStatement(ForStatement node);

		void OnWhileStatement(WhileStatement node);

		void OnBreakStatement(BreakStatement node);

		void OnContinueStatement(ContinueStatement node);

		void OnReturnStatement(ReturnStatement node);

		void OnYieldStatement(YieldStatement node);

		void OnRaiseStatement(RaiseStatement node);

		void OnUnpackStatement(UnpackStatement node);

		void OnExpressionStatement(ExpressionStatement node);

		void OnOmittedExpression(OmittedExpression node);

		void OnExpressionPair(ExpressionPair node);

		void OnMethodInvocationExpression(MethodInvocationExpression node);

		void OnUnaryExpression(UnaryExpression node);

		void OnBinaryExpression(BinaryExpression node);

		void OnConditionalExpression(ConditionalExpression node);

		void OnReferenceExpression(ReferenceExpression node);

		void OnMemberReferenceExpression(MemberReferenceExpression node);

		void OnGenericReferenceExpression(GenericReferenceExpression node);

		void OnQuasiquoteExpression(QuasiquoteExpression node);

		void OnStringLiteralExpression(StringLiteralExpression node);

		void OnCharLiteralExpression(CharLiteralExpression node);

		void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node);

		void OnIntegerLiteralExpression(IntegerLiteralExpression node);

		void OnDoubleLiteralExpression(DoubleLiteralExpression node);

		void OnNullLiteralExpression(NullLiteralExpression node);

		void OnSelfLiteralExpression(SelfLiteralExpression node);

		void OnSuperLiteralExpression(SuperLiteralExpression node);

		void OnBoolLiteralExpression(BoolLiteralExpression node);

		void OnRELiteralExpression(RELiteralExpression node);

		void OnSpliceExpression(SpliceExpression node);

		void OnSpliceTypeReference(SpliceTypeReference node);

		void OnSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node);

		void OnSpliceTypeMember(SpliceTypeMember node);

		void OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node);

		void OnSpliceParameterDeclaration(SpliceParameterDeclaration node);

		void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node);

		void OnHashLiteralExpression(HashLiteralExpression node);

		void OnListLiteralExpression(ListLiteralExpression node);

		void OnCollectionInitializationExpression(CollectionInitializationExpression node);

		void OnArrayLiteralExpression(ArrayLiteralExpression node);

		void OnGeneratorExpression(GeneratorExpression node);

		void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node);

		void OnSlice(Slice node);

		void OnSlicingExpression(SlicingExpression node);

		void OnTryCastExpression(TryCastExpression node);

		void OnCastExpression(CastExpression node);

		void OnTypeofExpression(TypeofExpression node);

		void OnCustomStatement(CustomStatement node);

		void OnCustomExpression(CustomExpression node);

		void OnStatementTypeMember(StatementTypeMember node);
	}
}
