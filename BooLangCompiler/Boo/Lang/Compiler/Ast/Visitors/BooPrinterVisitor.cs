using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Boo.Lang.Compiler.Ast.Visitors
{
	public class BooPrinterVisitor : TextEmitter
	{
		[Flags]
		public enum PrintOptions
		{
			None = 0x0,
			PrintLocals = 0x1,
			WSA = 0x2
		}

		public PrintOptions Options = PrintOptions.None;

		public bool IsWhiteSpaceAgnostic => IsOptionSet(PrintOptions.WSA);

		public BooPrinterVisitor(TextWriter writer)
			: base(writer)
		{
		}

		public BooPrinterVisitor(TextWriter writer, PrintOptions options)
			: this(writer)
		{
			Options = options;
		}

		public bool IsOptionSet(PrintOptions option)
		{
			return (option & Options) == option;
		}

		public void Print(CompileUnit ast)
		{
			OnCompileUnit(ast);
		}

		public virtual void WriteKeyword(string text)
		{
			Write(text);
		}

		public virtual void WriteOperator(string text)
		{
			Write(text);
		}

		public override void OnModule(Module m)
		{
			Visit(m.Namespace);
			if (m.Imports.Count > 0)
			{
				Visit(m.Imports);
				WriteLine();
			}
			foreach (TypeMember member in m.Members)
			{
				Visit(member);
				WriteLine();
			}
			if (m.Globals != null)
			{
				Visit(m.Globals.Statements);
			}
			foreach (Attribute attribute in m.Attributes)
			{
				WriteModuleAttribute(attribute);
			}
			foreach (Attribute assemblyAttribute in m.AssemblyAttributes)
			{
				WriteAssemblyAttribute(assemblyAttribute);
			}
		}

		private void WriteModuleAttribute(Attribute attribute)
		{
			WriteAttribute(attribute, "module: ");
			WriteLine();
		}

		private void WriteAssemblyAttribute(Attribute attribute)
		{
			WriteAttribute(attribute, "assembly: ");
			WriteLine();
		}

		public override void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
			WriteKeyword("namespace");
			WriteLine(" {0}", node.Name);
			WriteLine();
		}

		private static bool IsExtendedRE(string s)
		{
			return s.IndexOfAny(new char[2] { ' ', '\t' }) > -1;
		}

		private static bool CanBeRepresentedAsQualifiedName(string s)
		{
			foreach (char c in s)
			{
				if (!char.IsLetterOrDigit(c) && c != '_' && c != '.')
				{
					return false;
				}
			}
			return true;
		}

		public override void OnImport(Import p)
		{
			WriteKeyword("import");
			Write(" {0}", p.Namespace);
			if (null != p.AssemblyReference)
			{
				WriteKeyword(" from ");
				string name = p.AssemblyReference.Name;
				if (CanBeRepresentedAsQualifiedName(name))
				{
					Write(name);
				}
				else
				{
					WriteStringLiteral(name);
				}
			}
			if (p.Expression.NodeType == NodeType.MethodInvocationExpression)
			{
				MethodInvocationExpression methodInvocationExpression = (MethodInvocationExpression)p.Expression;
				Write("(");
				WriteCommaSeparatedList(methodInvocationExpression.Arguments);
				Write(")");
			}
			if (null != p.Alias)
			{
				WriteKeyword(" as ");
				Write(p.Alias.Name);
			}
			WriteLine();
		}

		private void WritePass()
		{
			if (!IsWhiteSpaceAgnostic)
			{
				WriteIndented();
				WriteKeyword("pass");
				WriteLine();
			}
		}

		private void WriteBlockStatements(Block b)
		{
			if (b.IsEmpty)
			{
				WritePass();
			}
			else
			{
				Visit(b.Statements);
			}
		}

		public void WriteBlock(Block b)
		{
			BeginBlock();
			WriteBlockStatements(b);
			EndBlock();
		}

		private void BeginBlock()
		{
			Indent();
		}

		private void EndBlock()
		{
			Dedent();
			if (IsWhiteSpaceAgnostic)
			{
				WriteEnd();
			}
		}

		private void WriteEnd()
		{
			WriteIndented();
			WriteKeyword("end");
			WriteLine();
		}

		public override void OnAttribute(Attribute att)
		{
			WriteAttribute(att);
		}

		public override void OnClassDefinition(ClassDefinition c)
		{
			WriteTypeDefinition("class", c);
		}

		public override void OnStructDefinition(StructDefinition node)
		{
			WriteTypeDefinition("struct", node);
		}

		public override void OnInterfaceDefinition(InterfaceDefinition id)
		{
			WriteTypeDefinition("interface", id);
		}

		public override void OnEnumDefinition(EnumDefinition ed)
		{
			WriteTypeDefinition("enum", ed);
		}

		public override void OnEvent(Event node)
		{
			WriteAttributes(node.Attributes, addNewLines: true);
			WriteOptionalModifiers(node);
			WriteKeyword("event ");
			Write(node.Name);
			WriteTypeReference(node.Type);
			WriteLine();
		}

		private static bool IsInterfaceMember(TypeMember node)
		{
			return node.ParentNode != null && node.ParentNode.NodeType == NodeType.InterfaceDefinition;
		}

		public override void OnField(Field f)
		{
			WriteAttributes(f.Attributes, addNewLines: true);
			WriteModifiers(f);
			Write(f.Name);
			WriteTypeReference(f.Type);
			if (null != f.Initializer)
			{
				WriteOperator(" = ");
				Visit(f.Initializer);
			}
			WriteLine();
		}

		public override void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			Visit(node.InterfaceType);
			Write(".");
		}

		public override void OnProperty(Property node)
		{
			bool interfaceMember = IsInterfaceMember(node);
			WriteAttributes(node.Attributes, addNewLines: true);
			WriteOptionalModifiers(node);
			WriteIndented("");
			Visit(node.ExplicitInfo);
			Write(node.Name);
			if (node.Parameters.Count > 0)
			{
				WriteParameterList(node.Parameters, "[", "]");
			}
			WriteTypeReference(node.Type);
			WriteLine(":");
			BeginBlock();
			WritePropertyAccessor(node.Getter, "get", interfaceMember);
			WritePropertyAccessor(node.Setter, "set", interfaceMember);
			EndBlock();
		}

		private void WritePropertyAccessor(Method method, string name, bool interfaceMember)
		{
			if (null != method)
			{
				WriteAttributes(method.Attributes, addNewLines: true);
				if (interfaceMember)
				{
					WriteIndented();
				}
				else
				{
					WriteModifiers(method);
				}
				WriteKeyword(name);
				if (interfaceMember)
				{
					WriteLine();
					return;
				}
				WriteLine(":");
				WriteBlock(method.Body);
			}
		}

		public override void OnEnumMember(EnumMember node)
		{
			WriteAttributes(node.Attributes, addNewLines: true);
			WriteIndented(node.Name);
			if (null != node.Initializer)
			{
				WriteOperator(" = ");
				Visit(node.Initializer);
			}
			WriteLine();
		}

		public override void OnConstructor(Constructor c)
		{
			OnMethod(c);
		}

		public override void OnDestructor(Destructor c)
		{
			OnMethod(c);
		}

		private bool IsSimpleClosure(BlockExpression node)
		{
			return node.Body.Statements.Count switch
			{
				0 => true, 
				1 => node.Body.Statements[0].NodeType switch
				{
					NodeType.IfStatement => false, 
					NodeType.WhileStatement => false, 
					NodeType.ForStatement => false, 
					_ => true, 
				}, 
				_ => false, 
			};
		}

		public override void OnBlockExpression(BlockExpression node)
		{
			if (IsSimpleClosure(node))
			{
				DisableNewLine();
				Write("{ ");
				if (node.Parameters.Count > 0)
				{
					WriteCommaSeparatedList(node.Parameters);
					Write(" | ");
				}
				if (node.Body.IsEmpty)
				{
					Write("return");
				}
				else
				{
					Visit(node.Body.Statements);
				}
				Write(" }");
				EnableNewLine();
			}
			else
			{
				WriteKeyword("def ");
				WriteParameterList(node.Parameters);
				WriteTypeReference(node.ReturnType);
				WriteLine(":");
				WriteBlock(node.Body);
			}
		}

		private void WriteCallableDefinitionHeader(string keyword, CallableDefinition node)
		{
			WriteAttributes(node.Attributes, addNewLines: true);
			WriteOptionalModifiers(node);
			WriteKeyword(keyword);
			IExplicitMember explicitMember = node as IExplicitMember;
			if (null != explicitMember)
			{
				Visit(explicitMember.ExplicitInfo);
			}
			Write(node.Name);
			if (node.GenericParameters.Count > 0)
			{
				WriteGenericParameterList(node.GenericParameters);
			}
			WriteParameterList(node.Parameters);
			if (node.ReturnTypeAttributes.Count > 0)
			{
				Write(" ");
				WriteAttributes(node.ReturnTypeAttributes, addNewLines: false);
			}
			WriteTypeReference(node.ReturnType);
		}

		private void WriteOptionalModifiers(TypeMember node)
		{
			if (IsInterfaceMember(node))
			{
				WriteIndented();
			}
			else
			{
				WriteModifiers(node);
			}
		}

		public override void OnCallableDefinition(CallableDefinition node)
		{
			WriteCallableDefinitionHeader("callable ", node);
		}

		public override void OnMethod(Method m)
		{
			if (m.IsRuntime)
			{
				WriteImplementationComment("runtime");
			}
			WriteCallableDefinitionHeader("def ", m);
			if (IsInterfaceMember(m))
			{
				WriteLine();
				return;
			}
			WriteLine(":");
			WriteLocals(m);
			WriteBlock(m.Body);
		}

		private void WriteImplementationComment(string comment)
		{
			WriteIndented("// {0}", comment);
			WriteLine();
		}

		public override void OnLocal(Local node)
		{
			WriteIndented("// Local {0}, {1}, PrivateScope: {2}", node.Name, node.Entity, node.PrivateScope);
			WriteLine();
		}

		private void WriteLocals(Method m)
		{
			if (IsOptionSet(PrintOptions.PrintLocals))
			{
				Visit(m.Locals);
			}
		}

		private void WriteTypeReference(TypeReference t)
		{
			if (null != t)
			{
				WriteKeyword(" as ");
				Visit(t);
			}
		}

		public override void OnParameterDeclaration(ParameterDeclaration p)
		{
			WriteAttributes(p.Attributes, addNewLines: false);
			if (p.IsByRef)
			{
				WriteKeyword("ref ");
			}
			if (IsCallableTypeReferenceParameter(p))
			{
				if (p.IsParamArray)
				{
					Write("*");
				}
				Visit(p.Type);
			}
			else
			{
				Write(p.Name);
				WriteTypeReference(p.Type);
			}
		}

		private static bool IsCallableTypeReferenceParameter(ParameterDeclaration p)
		{
			Node parentNode = p.ParentNode;
			return parentNode != null && parentNode.NodeType == NodeType.CallableTypeReference;
		}

		public override void OnGenericParameterDeclaration(GenericParameterDeclaration gp)
		{
			Write(gp.Name);
			if (gp.BaseTypes.Count <= 0 && gp.Constraints == GenericParameterConstraints.None)
			{
				return;
			}
			Write("(");
			WriteCommaSeparatedList(gp.BaseTypes);
			if (gp.Constraints != 0)
			{
				if (gp.BaseTypes.Count != 0)
				{
					Write(", ");
				}
				WriteGenericParameterConstraints(gp.Constraints);
			}
			Write(")");
		}

		private void WriteGenericParameterConstraints(GenericParameterConstraints constraints)
		{
			List<string> list = new List<string>();
			if ((constraints & GenericParameterConstraints.ReferenceType) != 0)
			{
				list.Add("class");
			}
			if ((constraints & GenericParameterConstraints.ValueType) != 0)
			{
				list.Add("struct");
			}
			if ((constraints & GenericParameterConstraints.Constructable) != 0)
			{
				list.Add("constructor");
			}
			Write(string.Join(", ", list.ToArray()));
		}

		private KeyValuePair<T, string> CreateTranslation<T>(T value, string translation)
		{
			return new KeyValuePair<T, string>(value, translation);
		}

		public override void OnTypeofExpression(TypeofExpression node)
		{
			Write("typeof(");
			Visit(node.Type);
			Write(")");
		}

		public override void OnSimpleTypeReference(SimpleTypeReference t)
		{
			Write(t.Name);
		}

		public override void OnGenericTypeReference(GenericTypeReference node)
		{
			OnSimpleTypeReference(node);
			WriteGenericArguments(node.GenericArguments);
		}

		public override void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
			OnSimpleTypeReference(node);
			Write("[of *");
			for (int i = 1; i < node.GenericPlaceholders; i++)
			{
				Write(", *");
			}
			Write("]");
		}

		public override void OnGenericReferenceExpression(GenericReferenceExpression node)
		{
			Visit(node.Target);
			WriteGenericArguments(node.GenericArguments);
		}

		private void WriteGenericArguments(TypeReferenceCollection arguments)
		{
			Write("[of ");
			WriteCommaSeparatedList(arguments);
			Write("]");
		}

		public override void OnArrayTypeReference(ArrayTypeReference t)
		{
			Write("(");
			Visit(t.ElementType);
			if (t.Rank != null && t.Rank.Value > 1)
			{
				Write(", ");
				t.Rank.Accept(this);
			}
			Write(")");
		}

		public override void OnCallableTypeReference(CallableTypeReference node)
		{
			Write("callable(");
			WriteCommaSeparatedList(node.Parameters);
			Write(")");
			WriteTypeReference(node.ReturnType);
		}

		public override void OnMemberReferenceExpression(MemberReferenceExpression e)
		{
			Visit(e.Target);
			Write(".");
			Write(e.Name);
		}

		public override void OnTryCastExpression(TryCastExpression e)
		{
			Write("(");
			Visit(e.Target);
			WriteTypeReference(e.Type);
			Write(")");
		}

		public override void OnCastExpression(CastExpression node)
		{
			Write("(");
			Visit(node.Target);
			WriteKeyword(" cast ");
			Visit(node.Type);
			Write(")");
		}

		public override void OnNullLiteralExpression(NullLiteralExpression node)
		{
			WriteKeyword("null");
		}

		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			WriteKeyword("self");
		}

		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			WriteKeyword("super");
		}

		public override void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			WriteTimeSpanLiteral(node.Value, _writer);
		}

		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			if (node.Value)
			{
				WriteKeyword("true");
			}
			else
			{
				WriteKeyword("false");
			}
		}

		public override void OnUnaryExpression(UnaryExpression node)
		{
			bool flag = NeedParensAround(node) && !IsMethodInvocationArg(node);
			if (flag)
			{
				Write("(");
			}
			bool flag2 = AstUtil.IsPostUnaryOperator(node.Operator);
			if (!flag2)
			{
				WriteOperator(GetUnaryOperatorText(node.Operator));
			}
			Visit(node.Operand);
			if (flag2)
			{
				WriteOperator(GetUnaryOperatorText(node.Operator));
			}
			if (flag)
			{
				Write(")");
			}
		}

		private bool IsMethodInvocationArg(UnaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = node.ParentNode as MethodInvocationExpression;
			return methodInvocationExpression != null && node != methodInvocationExpression.Target;
		}

		public override void OnConditionalExpression(ConditionalExpression e)
		{
			Write("(");
			Visit(e.TrueValue);
			WriteKeyword(" if ");
			Visit(e.Condition);
			WriteKeyword(" else ");
			Visit(e.FalseValue);
			Write(")");
		}

		private bool NeedParensAround(Expression e)
		{
			if (e.ParentNode == null)
			{
				return false;
			}
			switch (e.ParentNode.NodeType)
			{
			case NodeType.MacroStatement:
			case NodeType.IfStatement:
			case NodeType.UnlessStatement:
			case NodeType.WhileStatement:
			case NodeType.ExpressionStatement:
				return false;
			default:
				return true;
			}
		}

		public override void OnBinaryExpression(BinaryExpression e)
		{
			bool flag = NeedParensAround(e);
			if (flag)
			{
				Write("(");
			}
			Visit(e.Left);
			Write(" ");
			WriteOperator(GetBinaryOperatorText(e.Operator));
			Write(" ");
			if (e.Operator == BinaryOperatorType.TypeTest)
			{
				Visit(((TypeofExpression)e.Right).Type);
			}
			else
			{
				Visit(e.Right);
			}
			if (flag)
			{
				Write(")");
			}
		}

		public override void OnRaiseStatement(RaiseStatement rs)
		{
			WriteIndented();
			WriteKeyword("raise ");
			Visit(rs.Exception);
			Visit(rs.Modifier);
			WriteLine();
		}

		public override void OnMethodInvocationExpression(MethodInvocationExpression e)
		{
			Visit(e.Target);
			Write("(");
			WriteCommaSeparatedList(e.Arguments);
			if (e.NamedArguments.Count > 0)
			{
				if (e.Arguments.Count > 0)
				{
					Write(", ");
				}
				WriteCommaSeparatedList(e.NamedArguments);
			}
			Write(")");
		}

		public override void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			WriteArray(node.Items, node.Type);
		}

		public override void OnListLiteralExpression(ListLiteralExpression node)
		{
			WriteDelimitedCommaSeparatedList("[", node.Items, "]");
		}

		private void WriteDelimitedCommaSeparatedList(string opening, IEnumerable<Expression> list, string closing)
		{
			Write(opening);
			WriteCommaSeparatedList(list);
			Write(closing);
		}

		public override void OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			Visit(node.Collection);
			Write(" ");
			if (node.Initializer is ListLiteralExpression)
			{
				WriteDelimitedCommaSeparatedList("{ ", ((ListLiteralExpression)node.Initializer).Items, " }");
			}
			else
			{
				Visit(node.Initializer);
			}
		}

		public override void OnGeneratorExpression(GeneratorExpression node)
		{
			Write("(");
			Visit(node.Expression);
			WriteGeneratorExpressionBody(node);
			Write(")");
		}

		private void WriteGeneratorExpressionBody(GeneratorExpression node)
		{
			WriteKeyword(" for ");
			WriteCommaSeparatedList(node.Declarations);
			WriteKeyword(" in ");
			Visit(node.Iterator);
			Visit(node.Filter);
		}

		public override void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			Write("(");
			Visit(node.Items[0].Expression);
			for (int i = 0; i < node.Items.Count; i++)
			{
				WriteGeneratorExpressionBody(node.Items[i]);
			}
			Write(")");
		}

		public override void OnSlice(Slice node)
		{
			Visit(node.Begin);
			if (node.End != null || WasOmitted(node.Begin))
			{
				Write(":");
			}
			Visit(node.End);
			if (null != node.Step)
			{
				Write(":");
				Visit(node.Step);
			}
		}

		public override void OnSlicingExpression(SlicingExpression node)
		{
			Visit(node.Target);
			Write("[");
			WriteCommaSeparatedList(node.Indices);
			Write("]");
		}

		public override void OnHashLiteralExpression(HashLiteralExpression node)
		{
			Write("{");
			if (node.Items.Count > 0)
			{
				Write(" ");
				WriteCommaSeparatedList(node.Items);
				Write(" ");
			}
			Write("}");
		}

		public override void OnExpressionPair(ExpressionPair pair)
		{
			Visit(pair.First);
			Write(": ");
			Visit(pair.Second);
		}

		public override void OnRELiteralExpression(RELiteralExpression e)
		{
			if (IsExtendedRE(e.Value))
			{
				Write("@");
			}
			Write(e.Value);
		}

		public override void OnSpliceExpression(SpliceExpression e)
		{
			WriteSplicedExpression(e.Expression);
		}

		private void WriteSplicedExpression(Expression expression)
		{
			WriteOperator("$(");
			Visit(expression);
			WriteOperator(")");
		}

		public override void OnStatementTypeMember(StatementTypeMember node)
		{
			WriteModifiers(node);
			Visit(node.Statement);
		}

		public override void OnSpliceTypeMember(SpliceTypeMember node)
		{
			WriteIndented();
			Visit(node.TypeMember);
			WriteLine();
		}

		public override void OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			WriteIndented();
			WriteSplicedExpression(node.Expression);
			WriteLine();
		}

		public override void OnSpliceTypeReference(SpliceTypeReference node)
		{
			WriteSplicedExpression(node.Expression);
		}

		private void WriteIndentedOperator(string op)
		{
			WriteIndented();
			WriteOperator(op);
		}

		public override void OnQuasiquoteExpression(QuasiquoteExpression e)
		{
			WriteIndentedOperator("[|");
			if (e.Node is Expression)
			{
				Write(" ");
				Visit(e.Node);
				Write(" ");
				WriteIndentedOperator("|]");
			}
			else
			{
				WriteLine();
				Indent();
				Visit(e.Node);
				Dedent();
				WriteIndentedOperator("|]");
				WriteLine();
			}
		}

		public override void OnStringLiteralExpression(StringLiteralExpression e)
		{
			if (e != null && e.Value != null)
			{
				WriteStringLiteral(e.Value);
			}
			else
			{
				WriteKeyword("null");
			}
		}

		public override void OnCharLiteralExpression(CharLiteralExpression e)
		{
			WriteKeyword("char");
			Write("(");
			WriteStringLiteral(e.Value);
			Write(")");
		}

		public override void OnIntegerLiteralExpression(IntegerLiteralExpression e)
		{
			Write(e.Value.ToString());
			if (e.IsLong)
			{
				Write("L");
			}
		}

		public override void OnDoubleLiteralExpression(DoubleLiteralExpression e)
		{
			Write(e.Value.ToString("########0.0##########", CultureInfo.InvariantCulture));
			if (e.IsSingle)
			{
				Write("F");
			}
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			Write(node.Name);
		}

		public override void OnExpressionStatement(ExpressionStatement node)
		{
			WriteIndented();
			Visit(node.Expression);
			Visit(node.Modifier);
			WriteLine();
		}

		public override void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			Write("\"");
			foreach (Expression expression in node.Expressions)
			{
				switch (expression.NodeType)
				{
				case NodeType.StringLiteralExpression:
					WriteStringLiteralContents(((StringLiteralExpression)expression).Value, _writer, single: false);
					break;
				case NodeType.BinaryExpression:
				case NodeType.ReferenceExpression:
					Write("$");
					Visit(expression);
					break;
				default:
					Write("$(");
					Visit(expression);
					Write(")");
					break;
				}
			}
			Write("\"");
		}

		public override void OnStatementModifier(StatementModifier sm)
		{
			Write(" ");
			WriteKeyword(sm.Type.ToString().ToLower());
			Write(" ");
			Visit(sm.Condition);
		}

		public override void OnLabelStatement(LabelStatement node)
		{
			WriteIndented(":");
			WriteLine(node.Name);
		}

		public override void OnGotoStatement(GotoStatement node)
		{
			WriteIndented();
			WriteKeyword("goto ");
			Visit(node.Label);
			Visit(node.Modifier);
			WriteLine();
		}

		public override void OnMacroStatement(MacroStatement node)
		{
			WriteIndented(node.Name);
			Write(" ");
			WriteCommaSeparatedList(node.Arguments);
			if (!node.Body.IsEmpty)
			{
				WriteLine(":");
				WriteBlock(node.Body);
			}
			else
			{
				Visit(node.Modifier);
				WriteLine();
			}
		}

		public override void OnForStatement(ForStatement fs)
		{
			WriteIndented();
			WriteKeyword("for ");
			for (int i = 0; i < fs.Declarations.Count; i++)
			{
				if (i > 0)
				{
					Write(", ");
				}
				Visit(fs.Declarations[i]);
			}
			WriteKeyword(" in ");
			Visit(fs.Iterator);
			WriteLine(":");
			WriteBlock(fs.Block);
			if (fs.OrBlock != null)
			{
				WriteIndented();
				WriteKeyword("or:");
				WriteLine();
				WriteBlock(fs.OrBlock);
			}
			if (fs.ThenBlock != null)
			{
				WriteIndented();
				WriteKeyword("then:");
				WriteLine();
				WriteBlock(fs.ThenBlock);
			}
		}

		public override void OnTryStatement(TryStatement node)
		{
			WriteIndented();
			WriteKeyword("try:");
			WriteLine();
			Indent();
			WriteBlockStatements(node.ProtectedBlock);
			Dedent();
			Visit(node.ExceptionHandlers);
			if (null != node.FailureBlock)
			{
				WriteIndented();
				WriteKeyword("failure:");
				WriteLine();
				Indent();
				WriteBlockStatements(node.FailureBlock);
				Dedent();
			}
			if (null != node.EnsureBlock)
			{
				WriteIndented();
				WriteKeyword("ensure:");
				WriteLine();
				Indent();
				WriteBlockStatements(node.EnsureBlock);
				Dedent();
			}
			if (IsWhiteSpaceAgnostic)
			{
				WriteEnd();
			}
		}

		public override void OnExceptionHandler(ExceptionHandler node)
		{
			WriteIndented();
			WriteKeyword("except");
			if ((node.Flags & ExceptionHandlerFlags.Untyped) == 0)
			{
				if ((node.Flags & ExceptionHandlerFlags.Anonymous) == 0)
				{
					Write(" ");
					Visit(node.Declaration);
				}
				else
				{
					WriteTypeReference(node.Declaration.Type);
				}
			}
			else if ((node.Flags & ExceptionHandlerFlags.Anonymous) == 0)
			{
				Write(" ");
				Write(node.Declaration.Name);
			}
			if ((node.Flags & ExceptionHandlerFlags.Filter) == ExceptionHandlerFlags.Filter)
			{
				UnaryExpression unaryExpression = node.FilterCondition as UnaryExpression;
				if (unaryExpression != null && unaryExpression.Operator == UnaryOperatorType.LogicalNot)
				{
					WriteKeyword(" unless ");
					Visit(unaryExpression.Operand);
				}
				else
				{
					WriteKeyword(" if ");
					Visit(node.FilterCondition);
				}
			}
			WriteLine(":");
			Indent();
			WriteBlockStatements(node.Block);
			Dedent();
		}

		public override void OnUnlessStatement(UnlessStatement node)
		{
			WriteConditionalBlock("unless", node.Condition, node.Block);
		}

		public override void OnBreakStatement(BreakStatement node)
		{
			WriteIndented();
			WriteKeyword("break ");
			Visit(node.Modifier);
			WriteLine();
		}

		public override void OnContinueStatement(ContinueStatement node)
		{
			WriteIndented();
			WriteKeyword("continue ");
			Visit(node.Modifier);
			WriteLine();
		}

		public override void OnYieldStatement(YieldStatement node)
		{
			WriteIndented();
			WriteKeyword("yield ");
			Visit(node.Expression);
			Visit(node.Modifier);
			WriteLine();
		}

		public override void OnWhileStatement(WhileStatement node)
		{
			WriteConditionalBlock("while", node.Condition, node.Block);
			if (node.OrBlock != null)
			{
				WriteIndented();
				WriteKeyword("or:");
				WriteLine();
				WriteBlock(node.OrBlock);
			}
			if (node.ThenBlock != null)
			{
				WriteIndented();
				WriteKeyword("then:");
				WriteLine();
				WriteBlock(node.ThenBlock);
			}
		}

		public override void OnIfStatement(IfStatement node)
		{
			WriteIfBlock("if ", node);
			Block block = WriteElifs(node);
			if (null != block)
			{
				WriteIndented();
				WriteKeyword("else:");
				WriteLine();
				WriteBlock(block);
			}
			else if (IsWhiteSpaceAgnostic)
			{
				WriteEnd();
			}
		}

		private Block WriteElifs(IfStatement node)
		{
			Block falseBlock = node.FalseBlock;
			while (IsElif(falseBlock))
			{
				IfStatement ifStatement = (IfStatement)falseBlock.Statements[0];
				WriteIfBlock("elif ", ifStatement);
				falseBlock = ifStatement.FalseBlock;
			}
			return falseBlock;
		}

		private void WriteIfBlock(string keyword, IfStatement ifs)
		{
			WriteIndented();
			WriteKeyword(keyword);
			Visit(ifs.Condition);
			WriteLine(":");
			Indent();
			WriteBlockStatements(ifs.TrueBlock);
			Dedent();
		}

		private static bool IsElif(Block block)
		{
			if (block == null)
			{
				return false;
			}
			if (block.Statements.Count != 1)
			{
				return false;
			}
			return block.Statements[0] is IfStatement;
		}

		public override void OnDeclarationStatement(DeclarationStatement d)
		{
			WriteIndented();
			Visit(d.Declaration);
			if (null != d.Initializer)
			{
				WriteOperator(" = ");
				Visit(d.Initializer);
			}
			WriteLine();
		}

		public override void OnDeclaration(Declaration d)
		{
			Write(d.Name);
			WriteTypeReference(d.Type);
		}

		public override void OnReturnStatement(ReturnStatement r)
		{
			WriteIndented();
			WriteKeyword("return");
			if (r.Expression != null || r.Modifier != null)
			{
				Write(" ");
			}
			Visit(r.Expression);
			Visit(r.Modifier);
			WriteLine();
		}

		public override void OnUnpackStatement(UnpackStatement us)
		{
			WriteIndented();
			for (int i = 0; i < us.Declarations.Count; i++)
			{
				if (i > 0)
				{
					Write(", ");
				}
				Visit(us.Declarations[i]);
			}
			WriteOperator(" = ");
			Visit(us.Expression);
			Visit(us.Modifier);
			WriteLine();
		}

		public static string GetUnaryOperatorText(UnaryOperatorType op)
		{
			switch (op)
			{
			case UnaryOperatorType.Explode:
				return "*";
			case UnaryOperatorType.Increment:
			case UnaryOperatorType.PostIncrement:
				return "++";
			case UnaryOperatorType.Decrement:
			case UnaryOperatorType.PostDecrement:
				return "--";
			case UnaryOperatorType.UnaryNegation:
				return "-";
			case UnaryOperatorType.LogicalNot:
				return "not ";
			case UnaryOperatorType.OnesComplement:
				return "~";
			case UnaryOperatorType.AddressOf:
				return "&";
			case UnaryOperatorType.Indirection:
				return "*";
			default:
				throw new ArgumentException("op");
			}
		}

		public static string GetBinaryOperatorText(BinaryOperatorType op)
		{
			return op switch
			{
				BinaryOperatorType.Assign => "=", 
				BinaryOperatorType.Match => "=~", 
				BinaryOperatorType.NotMatch => "!~", 
				BinaryOperatorType.Equality => "==", 
				BinaryOperatorType.Inequality => "!=", 
				BinaryOperatorType.Addition => "+", 
				BinaryOperatorType.Exponentiation => "**", 
				BinaryOperatorType.InPlaceAddition => "+=", 
				BinaryOperatorType.InPlaceBitwiseAnd => "&=", 
				BinaryOperatorType.InPlaceBitwiseOr => "|=", 
				BinaryOperatorType.InPlaceSubtraction => "-=", 
				BinaryOperatorType.InPlaceMultiply => "*=", 
				BinaryOperatorType.InPlaceModulus => "%=", 
				BinaryOperatorType.InPlaceExclusiveOr => "^=", 
				BinaryOperatorType.InPlaceDivision => "/=", 
				BinaryOperatorType.Subtraction => "-", 
				BinaryOperatorType.Multiply => "*", 
				BinaryOperatorType.Division => "/", 
				BinaryOperatorType.GreaterThan => ">", 
				BinaryOperatorType.GreaterThanOrEqual => ">=", 
				BinaryOperatorType.LessThan => "<", 
				BinaryOperatorType.LessThanOrEqual => "<=", 
				BinaryOperatorType.Modulus => "%", 
				BinaryOperatorType.Member => "in", 
				BinaryOperatorType.NotMember => "not in", 
				BinaryOperatorType.ReferenceEquality => "is", 
				BinaryOperatorType.ReferenceInequality => "is not", 
				BinaryOperatorType.TypeTest => "isa", 
				BinaryOperatorType.Or => "or", 
				BinaryOperatorType.And => "and", 
				BinaryOperatorType.BitwiseOr => "|", 
				BinaryOperatorType.BitwiseAnd => "&", 
				BinaryOperatorType.ExclusiveOr => "^", 
				BinaryOperatorType.ShiftLeft => "<<", 
				BinaryOperatorType.ShiftRight => ">>", 
				BinaryOperatorType.InPlaceShiftLeft => "<<=", 
				BinaryOperatorType.InPlaceShiftRight => ">>=", 
				_ => throw new NotImplementedException(op.ToString()), 
			};
		}

		public virtual void WriteStringLiteral(string text)
		{
			WriteStringLiteral(text, _writer);
		}

		public static void WriteTimeSpanLiteral(TimeSpan value, TextWriter writer)
		{
			double totalDays = value.TotalDays;
			if (totalDays >= 1.0)
			{
				writer.Write(totalDays.ToString(CultureInfo.InvariantCulture) + "d");
				return;
			}
			double totalHours = value.TotalHours;
			if (totalHours >= 1.0)
			{
				writer.Write(totalHours.ToString(CultureInfo.InvariantCulture) + "h");
				return;
			}
			double totalMinutes = value.TotalMinutes;
			if (totalMinutes >= 1.0)
			{
				writer.Write(totalMinutes.ToString(CultureInfo.InvariantCulture) + "m");
				return;
			}
			double totalSeconds = value.TotalSeconds;
			if (totalSeconds >= 1.0)
			{
				writer.Write(totalSeconds.ToString(CultureInfo.InvariantCulture) + "s");
			}
			else
			{
				writer.Write(value.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms");
			}
		}

		public static void WriteStringLiteral(string text, TextWriter writer)
		{
			writer.Write("'");
			WriteStringLiteralContents(text, writer);
			writer.Write("'");
		}

		public static void WriteStringLiteralContents(string text, TextWriter writer)
		{
			WriteStringLiteralContents(text, writer, single: true);
		}

		public static void WriteStringLiteralContents(string text, TextWriter writer, bool single)
		{
			foreach (char c in text)
			{
				switch (c)
				{
				case '\r':
					writer.Write("\\r");
					break;
				case '\n':
					writer.Write("\\n");
					break;
				case '\t':
					writer.Write("\\t");
					break;
				case '\\':
					writer.Write("\\\\");
					break;
				case '\a':
					writer.Write("\\a");
					break;
				case '\b':
					writer.Write("\\b");
					break;
				case '\f':
					writer.Write("\\f");
					break;
				case '\0':
					writer.Write("\\0");
					break;
				case '\'':
					if (single)
					{
						writer.Write("\\'");
					}
					else
					{
						writer.Write(c);
					}
					break;
				case '"':
					if (!single)
					{
						writer.Write("\\\"");
					}
					else
					{
						writer.Write(c);
					}
					break;
				default:
					writer.Write(c);
					break;
				}
			}
		}

		private void WriteConditionalBlock(string keyword, Expression condition, Block block)
		{
			WriteIndented();
			WriteKeyword(keyword + " ");
			Visit(condition);
			WriteLine(":");
			WriteBlock(block);
		}

		private void WriteParameterList(ParameterDeclarationCollection items)
		{
			WriteParameterList(items, "(", ")");
		}

		private void WriteParameterList(ParameterDeclarationCollection items, string st, string ed)
		{
			Write(st);
			int num = 0;
			foreach (ParameterDeclaration item in items)
			{
				if (num > 0)
				{
					Write(", ");
				}
				if (item.IsParamArray)
				{
					Write("*");
				}
				Visit(item);
				num++;
			}
			Write(ed);
		}

		private void WriteGenericParameterList(GenericParameterDeclarationCollection items)
		{
			Write("[of ");
			WriteCommaSeparatedList(items);
			Write("]");
		}

		private void WriteAttribute(Attribute attribute)
		{
			WriteAttribute(attribute, null);
		}

		private void WriteAttribute(Attribute attribute, string prefix)
		{
			WriteIndented("[");
			if (null != prefix)
			{
				Write(prefix);
			}
			Write(attribute.Name);
			if (attribute.Arguments.Count > 0 || attribute.NamedArguments.Count > 0)
			{
				Write("(");
				WriteCommaSeparatedList(attribute.Arguments);
				if (attribute.NamedArguments.Count > 0)
				{
					if (attribute.Arguments.Count > 0)
					{
						Write(", ");
					}
					WriteCommaSeparatedList(attribute.NamedArguments);
				}
				Write(")");
			}
			Write("]");
		}

		private void WriteAttributes(AttributeCollection attributes, bool addNewLines)
		{
			foreach (Attribute attribute in attributes)
			{
				Visit(attribute);
				if (addNewLines)
				{
					WriteLine();
				}
				else
				{
					Write(" ");
				}
			}
		}

		private void WriteModifiers(TypeMember member)
		{
			WriteIndented();
			if (member.IsPartial)
			{
				WriteKeyword("partial ");
			}
			if (member.IsPublic)
			{
				WriteKeyword("public ");
			}
			else if (member.IsProtected)
			{
				WriteKeyword("protected ");
			}
			else if (member.IsPrivate)
			{
				WriteKeyword("private ");
			}
			else if (member.IsInternal)
			{
				WriteKeyword("internal ");
			}
			if (member.IsStatic)
			{
				WriteKeyword("static ");
			}
			else if (member.IsOverride)
			{
				WriteKeyword("override ");
			}
			else if (member.IsModifierSet(TypeMemberModifiers.Virtual))
			{
				WriteKeyword("virtual ");
			}
			else if (member.IsModifierSet(TypeMemberModifiers.Abstract))
			{
				WriteKeyword("abstract ");
			}
			if (member.IsFinal)
			{
				WriteKeyword("final ");
			}
			if (member.IsNew)
			{
				WriteKeyword("new ");
			}
			if (member.HasTransientModifier)
			{
				WriteKeyword("transient ");
			}
		}

		protected virtual void WriteTypeDefinition(string keyword, TypeDefinition td)
		{
			WriteAttributes(td.Attributes, addNewLines: true);
			WriteModifiers(td);
			WriteIndented();
			WriteKeyword(keyword);
			Write(" ");
			SpliceTypeMember spliceTypeMember = td.ParentNode as SpliceTypeMember;
			if (spliceTypeMember != null)
			{
				WriteSplicedExpression(spliceTypeMember.NameExpression);
			}
			else
			{
				Write(td.Name);
			}
			if (td.GenericParameters.Count != 0)
			{
				WriteGenericParameterList(td.GenericParameters);
			}
			if (td.BaseTypes.Count > 0)
			{
				Write("(");
				WriteCommaSeparatedList(td.BaseTypes);
				Write(")");
			}
			WriteLine(":");
			BeginBlock();
			if (td.Members.Count > 0)
			{
				foreach (TypeMember member in td.Members)
				{
					WriteLine();
					Visit(member);
				}
			}
			else
			{
				WritePass();
			}
			EndBlock();
		}

		private bool WasOmitted(Expression node)
		{
			return node != null && NodeType.OmittedExpression == node.NodeType;
		}
	}
}
