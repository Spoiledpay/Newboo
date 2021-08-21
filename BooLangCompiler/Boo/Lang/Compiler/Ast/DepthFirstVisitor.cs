using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace Boo.Lang.Compiler.Ast
{
	public class DepthFirstVisitor : IAstVisitor
	{
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
			if (EnterTypeMemberStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.TypeMember);
				LeaveTypeMemberStatement(node);
			}
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
			if (EnterExplicitMemberInfo(node))
			{
				Visit(node.InterfaceType);
				LeaveExplicitMemberInfo(node);
			}
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
			if (EnterArrayTypeReference(node))
			{
				Visit(node.ElementType);
				Visit(node.Rank);
				LeaveArrayTypeReference(node);
			}
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
			if (EnterCallableTypeReference(node))
			{
				Visit(node.Parameters);
				Visit(node.ReturnType);
				LeaveCallableTypeReference(node);
			}
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
			if (EnterCallableDefinition(node))
			{
				Visit(node.Attributes);
				Visit(node.Parameters);
				Visit(node.GenericParameters);
				Visit(node.ReturnType);
				Visit(node.ReturnTypeAttributes);
				LeaveCallableDefinition(node);
			}
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
			if (EnterImport(node))
			{
				Visit(node.Expression);
				Visit(node.AssemblyReference);
				Visit(node.Alias);
				LeaveImport(node);
			}
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
			if (EnterModule(node))
			{
				Visit(node.Attributes);
				Visit(node.Members);
				Visit(node.BaseTypes);
				Visit(node.GenericParameters);
				Visit(node.Namespace);
				Visit(node.Imports);
				Visit(node.Globals);
				Visit(node.AssemblyAttributes);
				LeaveModule(node);
			}
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
			if (EnterEnumMember(node))
			{
				Visit(node.Attributes);
				Visit(node.Initializer);
				LeaveEnumMember(node);
			}
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
			if (EnterField(node))
			{
				Visit(node.Attributes);
				Visit(node.Type);
				Visit(node.Initializer);
				LeaveField(node);
			}
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
			if (EnterProperty(node))
			{
				Visit(node.Attributes);
				Visit(node.Parameters);
				Visit(node.Getter);
				Visit(node.Setter);
				Visit(node.Type);
				Visit(node.ExplicitInfo);
				LeaveProperty(node);
			}
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
			if (EnterEvent(node))
			{
				Visit(node.Attributes);
				Visit(node.Add);
				Visit(node.Remove);
				Visit(node.Raise);
				Visit(node.Type);
				LeaveEvent(node);
			}
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
			if (EnterBlockExpression(node))
			{
				Visit(node.Parameters);
				Visit(node.ReturnType);
				Visit(node.Body);
				LeaveBlockExpression(node);
			}
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
			if (EnterMethod(node))
			{
				Visit(node.Attributes);
				Visit(node.Parameters);
				Visit(node.GenericParameters);
				Visit(node.ReturnType);
				Visit(node.ReturnTypeAttributes);
				Visit(node.Body);
				Visit(node.Locals);
				Visit(node.ExplicitInfo);
				LeaveMethod(node);
			}
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
			if (EnterConstructor(node))
			{
				Visit(node.Attributes);
				Visit(node.Parameters);
				Visit(node.GenericParameters);
				Visit(node.ReturnType);
				Visit(node.ReturnTypeAttributes);
				Visit(node.Body);
				Visit(node.Locals);
				Visit(node.ExplicitInfo);
				LeaveConstructor(node);
			}
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
			if (EnterDestructor(node))
			{
				Visit(node.Attributes);
				Visit(node.Parameters);
				Visit(node.GenericParameters);
				Visit(node.ReturnType);
				Visit(node.ReturnTypeAttributes);
				Visit(node.Body);
				Visit(node.Locals);
				Visit(node.ExplicitInfo);
				LeaveDestructor(node);
			}
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
			if (EnterParameterDeclaration(node))
			{
				Visit(node.Type);
				Visit(node.Attributes);
				LeaveParameterDeclaration(node);
			}
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
			if (EnterDeclaration(node))
			{
				Visit(node.Type);
				LeaveDeclaration(node);
			}
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
			if (EnterStatementModifier(node))
			{
				Visit(node.Condition);
				LeaveStatementModifier(node);
			}
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
			if (EnterGotoStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Label);
				LeaveGotoStatement(node);
			}
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
			if (EnterLabelStatement(node))
			{
				Visit(node.Modifier);
				LeaveLabelStatement(node);
			}
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
			if (EnterBlock(node))
			{
				Visit(node.Modifier);
				Visit(node.Statements);
				LeaveBlock(node);
			}
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
			if (EnterDeclarationStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Declaration);
				Visit(node.Initializer);
				LeaveDeclarationStatement(node);
			}
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
			if (EnterMacroStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Arguments);
				Visit(node.Body);
				LeaveMacroStatement(node);
			}
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
			if (EnterTryStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.ProtectedBlock);
				Visit(node.ExceptionHandlers);
				Visit(node.FailureBlock);
				Visit(node.EnsureBlock);
				LeaveTryStatement(node);
			}
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
			if (EnterExceptionHandler(node))
			{
				Visit(node.Declaration);
				Visit(node.FilterCondition);
				Visit(node.Block);
				LeaveExceptionHandler(node);
			}
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
			if (EnterIfStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Condition);
				Visit(node.TrueBlock);
				Visit(node.FalseBlock);
				LeaveIfStatement(node);
			}
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
			if (EnterUnlessStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Condition);
				Visit(node.Block);
				LeaveUnlessStatement(node);
			}
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
			if (EnterForStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Declarations);
				Visit(node.Iterator);
				Visit(node.Block);
				Visit(node.OrBlock);
				Visit(node.ThenBlock);
				LeaveForStatement(node);
			}
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
			if (EnterWhileStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Condition);
				Visit(node.Block);
				Visit(node.OrBlock);
				Visit(node.ThenBlock);
				LeaveWhileStatement(node);
			}
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
			if (EnterBreakStatement(node))
			{
				Visit(node.Modifier);
				LeaveBreakStatement(node);
			}
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
			if (EnterContinueStatement(node))
			{
				Visit(node.Modifier);
				LeaveContinueStatement(node);
			}
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
			if (EnterReturnStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Expression);
				LeaveReturnStatement(node);
			}
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
			if (EnterYieldStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Expression);
				LeaveYieldStatement(node);
			}
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
			if (EnterRaiseStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Exception);
				LeaveRaiseStatement(node);
			}
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
			if (EnterUnpackStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Declarations);
				Visit(node.Expression);
				LeaveUnpackStatement(node);
			}
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
			if (EnterExpressionStatement(node))
			{
				Visit(node.Modifier);
				Visit(node.Expression);
				LeaveExpressionStatement(node);
			}
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
			if (EnterExpressionPair(node))
			{
				Visit(node.First);
				Visit(node.Second);
				LeaveExpressionPair(node);
			}
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
			if (EnterMethodInvocationExpression(node))
			{
				Visit(node.Target);
				Visit(node.Arguments);
				Visit(node.NamedArguments);
				LeaveMethodInvocationExpression(node);
			}
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
			if (EnterUnaryExpression(node))
			{
				Visit(node.Operand);
				LeaveUnaryExpression(node);
			}
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
			if (EnterBinaryExpression(node))
			{
				Visit(node.Left);
				Visit(node.Right);
				LeaveBinaryExpression(node);
			}
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
			if (EnterConditionalExpression(node))
			{
				Visit(node.Condition);
				Visit(node.TrueValue);
				Visit(node.FalseValue);
				LeaveConditionalExpression(node);
			}
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
			if (EnterMemberReferenceExpression(node))
			{
				Visit(node.Target);
				LeaveMemberReferenceExpression(node);
			}
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
			if (EnterGenericReferenceExpression(node))
			{
				Visit(node.Target);
				Visit(node.GenericArguments);
				LeaveGenericReferenceExpression(node);
			}
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
			if (EnterSpliceExpression(node))
			{
				Visit(node.Expression);
				LeaveSpliceExpression(node);
			}
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
			if (EnterSpliceTypeReference(node))
			{
				Visit(node.Expression);
				LeaveSpliceTypeReference(node);
			}
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
			if (EnterSpliceMemberReferenceExpression(node))
			{
				Visit(node.Target);
				Visit(node.NameExpression);
				LeaveSpliceMemberReferenceExpression(node);
			}
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
			if (EnterSpliceTypeMember(node))
			{
				Visit(node.Attributes);
				Visit(node.TypeMember);
				Visit(node.NameExpression);
				LeaveSpliceTypeMember(node);
			}
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
			if (EnterSpliceTypeDefinitionBody(node))
			{
				Visit(node.Attributes);
				Visit(node.Expression);
				LeaveSpliceTypeDefinitionBody(node);
			}
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
			if (EnterSpliceParameterDeclaration(node))
			{
				Visit(node.Type);
				Visit(node.Attributes);
				Visit(node.ParameterDeclaration);
				Visit(node.NameExpression);
				LeaveSpliceParameterDeclaration(node);
			}
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
			if (EnterCollectionInitializationExpression(node))
			{
				Visit(node.Collection);
				Visit(node.Initializer);
				LeaveCollectionInitializationExpression(node);
			}
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
			if (EnterArrayLiteralExpression(node))
			{
				Visit(node.Items);
				Visit(node.Type);
				LeaveArrayLiteralExpression(node);
			}
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
			if (EnterGeneratorExpression(node))
			{
				Visit(node.Expression);
				Visit(node.Declarations);
				Visit(node.Iterator);
				Visit(node.Filter);
				LeaveGeneratorExpression(node);
			}
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
			if (EnterSlice(node))
			{
				Visit(node.Begin);
				Visit(node.End);
				Visit(node.Step);
				LeaveSlice(node);
			}
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
			if (EnterSlicingExpression(node))
			{
				Visit(node.Target);
				Visit(node.Indices);
				LeaveSlicingExpression(node);
			}
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
			if (EnterTryCastExpression(node))
			{
				Visit(node.Target);
				Visit(node.Type);
				LeaveTryCastExpression(node);
			}
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
			if (EnterCastExpression(node))
			{
				Visit(node.Target);
				Visit(node.Type);
				LeaveCastExpression(node);
			}
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
			if (EnterTypeofExpression(node))
			{
				Visit(node.Type);
				LeaveTypeofExpression(node);
			}
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
			if (EnterCustomStatement(node))
			{
				Visit(node.Modifier);
				LeaveCustomStatement(node);
			}
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
			if (EnterStatementTypeMember(node))
			{
				Visit(node.Attributes);
				Visit(node.Statement);
				LeaveStatementTypeMember(node);
			}
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

		public virtual bool Visit(Node node)
		{
			if (null != node)
			{
				try
				{
					node.Accept(this);
					return true;
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
			return false;
		}

		protected bool VisitAllowingCancellation(Node node)
		{
			try
			{
				Visit(node);
				return true;
			}
			catch (LongJumpException)
			{
			}
			return false;
		}

		protected void Cancel()
		{
			throw CancellationException;
		}

		protected virtual void OnError(Node node, Exception error)
		{
			throw CompilerErrorFactory.InternalError(node, error);
		}

		public void Visit(Node[] array, NodeType nodeType)
		{
			foreach (Node node in array)
			{
				if (node.NodeType == nodeType)
				{
					Visit(node);
				}
			}
		}

		public void VisitCollection(IEnumerable collection)
		{
			foreach (Node item in collection)
			{
				Visit(item);
			}
		}

		public bool Visit<T>(NodeCollection<T> collection, NodeType nodeType) where T : Node
		{
			if (null != collection)
			{
				Visit(collection.ToArray(), nodeType);
				return true;
			}
			return false;
		}

		public void Visit(Node[] array)
		{
			foreach (Node node in array)
			{
				Visit(node);
			}
		}

		public bool Visit<T>(NodeCollection<T> collection) where T : Node
		{
			if (null != collection)
			{
				Visit(collection.ToArray());
				return true;
			}
			return false;
		}
	}
}
