using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.Ast
{
	public class CodeSerializer : FastDepthFirstVisitor
	{
		private readonly bool _preserveLexicalInfo;

		private readonly Stack<Expression> _stack = new Stack<Expression>();

		private int _quasiquoteDepth;

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(TypeMemberModifiers value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(TypeMemberModifiers value)
		{
			return SerializeEnum("TypeMemberModifiers", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(MethodImplementationFlags value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(MethodImplementationFlags value)
		{
			return SerializeEnum("MethodImplementationFlags", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(ParameterModifiers value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(ParameterModifiers value)
		{
			return SerializeEnum("ParameterModifiers", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(ExceptionHandlerFlags value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(ExceptionHandlerFlags value)
		{
			return SerializeEnum("ExceptionHandlerFlags", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(GenericParameterConstraints value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(GenericParameterConstraints value)
		{
			return SerializeEnum("GenericParameterConstraints", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(StatementModifierType value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(StatementModifierType value)
		{
			return SerializeEnum("StatementModifierType", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(BinaryOperatorType value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(BinaryOperatorType value)
		{
			return SerializeEnum("BinaryOperatorType", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(BinaryOperatorKind value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(BinaryOperatorKind value)
		{
			return SerializeEnum("BinaryOperatorKind", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public bool ShouldSerialize(UnaryOperatorType value)
		{
			return (long)value != 0L;
		}

		[GeneratedCode("astgen.boo", "1")]
		public Expression Serialize(UnaryOperatorType value)
		{
			return SerializeEnum("UnaryOperatorType", (long)value);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCompileUnit(CompileUnit node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CompileUnit"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modules))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modules"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ModuleCollection", node.Modules)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnTypeMemberStatement(TypeMemberStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.TypeMemberStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.TypeMember))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "TypeMember"), Serialize(node.TypeMember)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ExplicitMemberInfo"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.InterfaceType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "InterfaceType"), Serialize(node.InterfaceType)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnSimpleTypeReference(SimpleTypeReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SimpleTypeReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnArrayTypeReference(ArrayTypeReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ArrayTypeReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.ElementType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ElementType"), Serialize(node.ElementType)));
			}
			if (ShouldSerialize(node.Rank))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Rank"), Serialize(node.Rank)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCallableTypeReference(CallableTypeReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CallableTypeReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGenericTypeReference(GenericTypeReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GenericTypeReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.GenericArguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericArguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.GenericArguments)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGenericTypeDefinitionReference(GenericTypeDefinitionReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GenericTypeDefinitionReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.GenericPlaceholders))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericPlaceholders"), Serialize(node.GenericPlaceholders)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCallableDefinition(CallableDefinition node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CallableDefinition"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			if (ShouldSerialize(node.ReturnTypeAttributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnTypeAttributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.ReturnTypeAttributes)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.NamespaceDeclaration"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnImport(Import node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Import"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			if (ShouldSerialize(node.AssemblyReference))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "AssemblyReference"), Serialize(node.AssemblyReference)));
			}
			if (ShouldSerialize(node.Alias))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Alias"), Serialize(node.Alias)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnModule(Module node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Module"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Members))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Members"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeMemberCollection", node.Members)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			if (ShouldSerialize(node.Namespace))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Namespace"), Serialize(node.Namespace)));
			}
			if (ShouldSerialize(node.Imports))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Imports"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ImportCollection", node.Imports)));
			}
			if (ShouldSerialize(node.Globals))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Globals"), Serialize(node.Globals)));
			}
			if (ShouldSerialize(node.AssemblyAttributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "AssemblyAttributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.AssemblyAttributes)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnClassDefinition(ClassDefinition node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ClassDefinition"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Members))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Members"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeMemberCollection", node.Members)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnStructDefinition(StructDefinition node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.StructDefinition"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Members))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Members"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeMemberCollection", node.Members)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.InterfaceDefinition"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Members))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Members"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeMemberCollection", node.Members)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnEnumDefinition(EnumDefinition node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.EnumDefinition"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Members))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Members"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeMemberCollection", node.Members)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnEnumMember(EnumMember node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.EnumMember"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Initializer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Initializer"), Serialize(node.Initializer)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnField(Field node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Field"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			if (ShouldSerialize(node.Initializer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Initializer"), Serialize(node.Initializer)));
			}
			if (ShouldSerialize(node.IsVolatile))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsVolatile"), Serialize(node.IsVolatile)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnProperty(Property node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Property"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.Getter))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Getter"), Serialize(node.Getter)));
			}
			if (ShouldSerialize(node.Setter))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Setter"), Serialize(node.Setter)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			if (ShouldSerialize(node.ExplicitInfo))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ExplicitInfo"), Serialize(node.ExplicitInfo)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnEvent(Event node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Event"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Add))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Add"), Serialize(node.Add)));
			}
			if (ShouldSerialize(node.Remove))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Remove"), Serialize(node.Remove)));
			}
			if (ShouldSerialize(node.Raise))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Raise"), Serialize(node.Raise)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnLocal(Local node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Local"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnBlockExpression(BlockExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.BlockExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			if (ShouldSerialize(node.Body))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Body"), Serialize(node.Body)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnMethod(Method node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Method"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			if (ShouldSerialize(node.ReturnTypeAttributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnTypeAttributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.ReturnTypeAttributes)));
			}
			if (ShouldSerialize(node.Body))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Body"), Serialize(node.Body)));
			}
			if (ShouldSerialize(node.Locals))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Locals"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.LocalCollection", node.Locals)));
			}
			if (ShouldSerialize(node.ImplementationFlags))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ImplementationFlags"), Serialize(node.ImplementationFlags)));
			}
			if (ShouldSerialize(node.ExplicitInfo))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ExplicitInfo"), Serialize(node.ExplicitInfo)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnConstructor(Constructor node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Constructor"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			if (ShouldSerialize(node.ReturnTypeAttributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnTypeAttributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.ReturnTypeAttributes)));
			}
			if (ShouldSerialize(node.Body))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Body"), Serialize(node.Body)));
			}
			if (ShouldSerialize(node.Locals))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Locals"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.LocalCollection", node.Locals)));
			}
			if (ShouldSerialize(node.ImplementationFlags))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ImplementationFlags"), Serialize(node.ImplementationFlags)));
			}
			if (ShouldSerialize(node.ExplicitInfo))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ExplicitInfo"), Serialize(node.ExplicitInfo)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnDestructor(Destructor node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Destructor"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Parameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Parameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ParameterDeclarationCollection", node.Parameters)));
			}
			if (ShouldSerialize(node.GenericParameters))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericParameters"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclarationCollection", node.GenericParameters)));
			}
			if (ShouldSerialize(node.ReturnType))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnType"), Serialize(node.ReturnType)));
			}
			if (ShouldSerialize(node.ReturnTypeAttributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ReturnTypeAttributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.ReturnTypeAttributes)));
			}
			if (ShouldSerialize(node.Body))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Body"), Serialize(node.Body)));
			}
			if (ShouldSerialize(node.Locals))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Locals"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.LocalCollection", node.Locals)));
			}
			if (ShouldSerialize(node.ImplementationFlags))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ImplementationFlags"), Serialize(node.ImplementationFlags)));
			}
			if (ShouldSerialize(node.ExplicitInfo))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ExplicitInfo"), Serialize(node.ExplicitInfo)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnParameterDeclaration(ParameterDeclaration node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ParameterDeclaration"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGenericParameterDeclaration(GenericParameterDeclaration node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GenericParameterDeclaration"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.BaseTypes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "BaseTypes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.BaseTypes)));
			}
			if (ShouldSerialize(node.Constraints))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Constraints"), Serialize(node.Constraints)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnDeclaration(Declaration node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Declaration"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnAttribute(Attribute node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Attribute"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Arguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Arguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Arguments)));
			}
			if (ShouldSerialize(node.NamedArguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "NamedArguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionPairCollection", node.NamedArguments)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnStatementModifier(StatementModifier node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.StatementModifier"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			if (ShouldSerialize(node.Condition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Condition"), Serialize(node.Condition)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGotoStatement(GotoStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GotoStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Label))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Label"), Serialize(node.Label)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnLabelStatement(LabelStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.LabelStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnBlock(Block node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Block"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Statements))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Statements"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.StatementCollection", node.Statements)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnDeclarationStatement(DeclarationStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.DeclarationStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Declaration))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Declaration"), Serialize(node.Declaration)));
			}
			if (ShouldSerialize(node.Initializer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Initializer"), Serialize(node.Initializer)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnMacroStatement(MacroStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.MacroStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Arguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Arguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Arguments)));
			}
			if (ShouldSerialize(node.Body))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Body"), Serialize(node.Body)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnTryStatement(TryStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.TryStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.ProtectedBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ProtectedBlock"), Serialize(node.ProtectedBlock)));
			}
			if (ShouldSerialize(node.ExceptionHandlers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ExceptionHandlers"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExceptionHandlerCollection", node.ExceptionHandlers)));
			}
			if (ShouldSerialize(node.FailureBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "FailureBlock"), Serialize(node.FailureBlock)));
			}
			if (ShouldSerialize(node.EnsureBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "EnsureBlock"), Serialize(node.EnsureBlock)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnExceptionHandler(ExceptionHandler node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ExceptionHandler"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Declaration))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Declaration"), Serialize(node.Declaration)));
			}
			if (ShouldSerialize(node.FilterCondition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "FilterCondition"), Serialize(node.FilterCondition)));
			}
			if (ShouldSerialize(node.Flags))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Flags"), Serialize(node.Flags)));
			}
			if (ShouldSerialize(node.Block))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Block"), Serialize(node.Block)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnIfStatement(IfStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.IfStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Condition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Condition"), Serialize(node.Condition)));
			}
			if (ShouldSerialize(node.TrueBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "TrueBlock"), Serialize(node.TrueBlock)));
			}
			if (ShouldSerialize(node.FalseBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "FalseBlock"), Serialize(node.FalseBlock)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnUnlessStatement(UnlessStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.UnlessStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Condition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Condition"), Serialize(node.Condition)));
			}
			if (ShouldSerialize(node.Block))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Block"), Serialize(node.Block)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnForStatement(ForStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ForStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Declarations))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Declarations"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.DeclarationCollection", node.Declarations)));
			}
			if (ShouldSerialize(node.Iterator))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Iterator"), Serialize(node.Iterator)));
			}
			if (ShouldSerialize(node.Block))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Block"), Serialize(node.Block)));
			}
			if (ShouldSerialize(node.OrBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "OrBlock"), Serialize(node.OrBlock)));
			}
			if (ShouldSerialize(node.ThenBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ThenBlock"), Serialize(node.ThenBlock)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnWhileStatement(WhileStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.WhileStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Condition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Condition"), Serialize(node.Condition)));
			}
			if (ShouldSerialize(node.Block))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Block"), Serialize(node.Block)));
			}
			if (ShouldSerialize(node.OrBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "OrBlock"), Serialize(node.OrBlock)));
			}
			if (ShouldSerialize(node.ThenBlock))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ThenBlock"), Serialize(node.ThenBlock)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnBreakStatement(BreakStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.BreakStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnContinueStatement(ContinueStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ContinueStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnReturnStatement(ReturnStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ReturnStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnYieldStatement(YieldStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.YieldStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnRaiseStatement(RaiseStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.RaiseStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Exception))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Exception"), Serialize(node.Exception)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnUnpackStatement(UnpackStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.UnpackStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			if (ShouldSerialize(node.Declarations))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Declarations"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.DeclarationCollection", node.Declarations)));
			}
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnExpressionPair(ExpressionPair node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ExpressionPair"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.First))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "First"), Serialize(node.First)));
			}
			if (ShouldSerialize(node.Second))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Second"), Serialize(node.Second)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.MethodInvocationExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.Arguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Arguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Arguments)));
			}
			if (ShouldSerialize(node.NamedArguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "NamedArguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionPairCollection", node.NamedArguments)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnUnaryExpression(UnaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.UnaryExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Operator))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Operator"), Serialize(node.Operator)));
			}
			if (ShouldSerialize(node.Operand))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Operand"), Serialize(node.Operand)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnBinaryExpression(BinaryExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.BinaryExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Operator))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Operator"), Serialize(node.Operator)));
			}
			if (ShouldSerialize(node.Left))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Left"), Serialize(node.Left)));
			}
			if (ShouldSerialize(node.Right))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Right"), Serialize(node.Right)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnConditionalExpression(ConditionalExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ConditionalExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Condition))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Condition"), Serialize(node.Condition)));
			}
			if (ShouldSerialize(node.TrueValue))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "TrueValue"), Serialize(node.TrueValue)));
			}
			if (ShouldSerialize(node.FalseValue))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "FalseValue"), Serialize(node.FalseValue)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnReferenceExpression(ReferenceExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ReferenceExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.MemberReferenceExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGenericReferenceExpression(GenericReferenceExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GenericReferenceExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.GenericArguments))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "GenericArguments"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.TypeReferenceCollection", node.GenericArguments)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.StringLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCharLiteralExpression(CharLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CharLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.TimeSpanLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.IntegerLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			if (ShouldSerialize(node.IsLong))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsLong"), Serialize(node.IsLong)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnDoubleLiteralExpression(DoubleLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.DoubleLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			if (ShouldSerialize(node.IsSingle))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsSingle"), Serialize(node.IsSingle)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnNullLiteralExpression(NullLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.NullLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SelfLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SuperLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.BoolLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnRELiteralExpression(RELiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.RELiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Value))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Value"), Serialize(node.Value)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceExpression(SpliceExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceTypeReference(SpliceTypeReference node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceTypeReference"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.IsPointer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "IsPointer"), Serialize(node.IsPointer)));
			}
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceMemberReferenceExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.NameExpression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "NameExpression"), Serialize(node.NameExpression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceTypeMember(SpliceTypeMember node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceTypeMember"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.TypeMember))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "TypeMember"), Serialize(node.TypeMember)));
			}
			if (ShouldSerialize(node.NameExpression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "NameExpression"), Serialize(node.NameExpression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceTypeDefinitionBody"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		internal void SerializeSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SpliceParameterDeclaration"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.ParameterDeclaration))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "ParameterDeclaration"), Serialize(node.ParameterDeclaration)));
			}
			if (ShouldSerialize(node.NameExpression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "NameExpression"), Serialize(node.NameExpression)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnExpressionInterpolationExpression(ExpressionInterpolationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ExpressionInterpolationExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Expressions))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expressions"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Expressions)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnHashLiteralExpression(HashLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.HashLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Items))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Items"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionPairCollection", node.Items)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnListLiteralExpression(ListLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ListLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Items))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Items"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Items)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCollectionInitializationExpression(CollectionInitializationExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CollectionInitializationExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Collection))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Collection"), Serialize(node.Collection)));
			}
			if (ShouldSerialize(node.Initializer))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Initializer"), Serialize(node.Initializer)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnArrayLiteralExpression(ArrayLiteralExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ArrayLiteralExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Items))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Items"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.ExpressionCollection", node.Items)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnGeneratorExpression(GeneratorExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.GeneratorExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Expression))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Expression"), Serialize(node.Expression)));
			}
			if (ShouldSerialize(node.Declarations))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Declarations"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.DeclarationCollection", node.Declarations)));
			}
			if (ShouldSerialize(node.Iterator))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Iterator"), Serialize(node.Iterator)));
			}
			if (ShouldSerialize(node.Filter))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Filter"), Serialize(node.Filter)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnExtendedGeneratorExpression(ExtendedGeneratorExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.ExtendedGeneratorExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Items))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Items"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.GeneratorExpressionCollection", node.Items)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnSlice(Slice node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.Slice"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Begin))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Begin"), Serialize(node.Begin)));
			}
			if (ShouldSerialize(node.End))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "End"), Serialize(node.End)));
			}
			if (ShouldSerialize(node.Step))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Step"), Serialize(node.Step)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnSlicingExpression(SlicingExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.SlicingExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.Indices))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Indices"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.SliceCollection", node.Indices)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnTryCastExpression(TryCastExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.TryCastExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCastExpression(CastExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CastExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Target))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Target"), Serialize(node.Target)));
			}
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnTypeofExpression(TypeofExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.TypeofExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Type))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Type"), Serialize(node.Type)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCustomStatement(CustomStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CustomStatement"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifier))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifier"), Serialize(node.Modifier)));
			}
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnCustomExpression(CustomExpression node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.CustomExpression"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			Push(methodInvocationExpression);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void OnStatementTypeMember(StatementTypeMember node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.StatementTypeMember"));
			methodInvocationExpression.Arguments.Add(Serialize(node.LexicalInfo));
			if (ShouldSerialize(node.Modifiers))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Modifiers"), Serialize(node.Modifiers)));
			}
			if (ShouldSerialize(node.Name))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Name"), Serialize(node.Name)));
			}
			if (ShouldSerialize(node.Attributes))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Attributes"), SerializeCollection(node, "Boo.Lang.Compiler.Ast.AttributeCollection", node.Attributes)));
			}
			if (ShouldSerialize(node.Statement))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Statement"), Serialize(node.Statement)));
			}
			Push(methodInvocationExpression);
		}

		public static string LiftName(string value)
		{
			return value;
		}

		public static string LiftName(ReferenceExpression node)
		{
			return node.Name;
		}

		public CodeSerializer()
			: this(preserveLexicalInfo: false)
		{
		}

		public CodeSerializer(bool preserveLexicalInfo)
		{
			_preserveLexicalInfo = preserveLexicalInfo;
		}

		public Expression Serialize(QuasiquoteExpression node)
		{
			_quasiquoteDepth = 0;
			Expression result = Serialize(node.Node);
			if (_stack.Count != 0)
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		public Expression Serialize(Node node)
		{
			if (null == node)
			{
				return new NullLiteralExpression();
			}
			node.Accept(this);
			return _stack.Pop();
		}

		public Expression Serialize(LexicalInfo location)
		{
			if (_preserveLexicalInfo && location.IsValid)
			{
				return SerializeLexicalInfo(location);
			}
			return CreateReference(location, location.GetType().FullName + ".Empty");
		}

		private Expression SerializeLexicalInfo(LexicalInfo location)
		{
			MethodInvocationExpression methodInvocationExpression = CreateInvocation(location, location.GetType().FullName);
			methodInvocationExpression.Arguments.Add(Serialize(location.FileName));
			methodInvocationExpression.Arguments.Add(Serialize(location.Line));
			methodInvocationExpression.Arguments.Add(Serialize(location.Column));
			return methodInvocationExpression;
		}

		public Expression CreateReference(Node sourceNode, string qname)
		{
			return CreateReference(sourceNode.LexicalInfo, qname);
		}

		private Expression CreateReference(LexicalInfo lexicalInfo, string qname)
		{
			return AstUtil.CreateReferenceExpression(lexicalInfo, qname);
		}

		public Expression CreateReference(string qname)
		{
			return AstUtil.CreateReferenceExpression(qname);
		}

		public bool ShouldSerialize<T>(NodeCollection<T> c) where T : Node
		{
			return c.Count > 0;
		}

		public bool ShouldSerialize(object value)
		{
			return value != null;
		}

		public Expression Serialize(string value)
		{
			return new StringLiteralExpression(value);
		}

		public Expression Serialize(bool value)
		{
			return new BoolLiteralExpression(value);
		}

		public Expression Serialize(long value)
		{
			return new IntegerLiteralExpression(value);
		}

		public Expression Serialize(double value)
		{
			return new DoubleLiteralExpression(value);
		}

		public Expression Serialize(TimeSpan value)
		{
			return new TimeSpanLiteralExpression(value);
		}

		private Expression SerializeEnum(string enumType, long value)
		{
			return new CastExpression(Serialize(value), new SimpleTypeReference("Boo.Lang.Compiler.Ast." + enumType));
		}

		protected Expression SerializeCollection(Node sourceNode, string typeName, StatementCollection nodes)
		{
			return SerializeCollectionWith((Node item) => LiftStatement(Serialize(item)), sourceNode, typeName, nodes);
		}

		protected Expression SerializeCollection(Node sourceNode, string typeName, ParameterDeclarationCollection nodes)
		{
			MethodInvocationExpression methodInvocationExpression = SerializeCollectionWith(Serialize, sourceNode, typeName, nodes);
			methodInvocationExpression.Arguments.Insert(0, new BoolLiteralExpression(nodes.HasParamArray));
			return methodInvocationExpression;
		}

		protected Expression SerializeCollection(Node sourceNode, string typeName, IEnumerable nodes)
		{
			return SerializeCollectionWith(Serialize, sourceNode, typeName, nodes);
		}

		private MethodInvocationExpression SerializeCollectionWith(Func<Node, Expression> serialize, Node sourceNode, string typeName, IEnumerable nodes)
		{
			MethodInvocationExpression methodInvocationExpression = CreateFromArrayInvocation(sourceNode, typeName);
			foreach (Node node in nodes)
			{
				methodInvocationExpression.Arguments.Add(serialize(node));
			}
			return methodInvocationExpression;
		}

		private MethodInvocationExpression CreateFromArrayInvocation(Node sourceNode, string typeName)
		{
			return CreateInvocation(sourceNode, typeName + ".FromArray");
		}

		public override void OnExpressionStatement(ExpressionStatement node)
		{
			Visit(node.Expression);
			if (null != node.Modifier)
			{
				Visit(node.Modifier);
				MethodInvocationExpression methodInvocationExpression = CreateInvocation(node, "Boo.Lang.Compiler.Ast.ExpressionStatement");
				methodInvocationExpression.NamedArguments.Add(Pair("Modifier", Pop()));
				methodInvocationExpression.NamedArguments.Add(Pair("Expression", Pop()));
				Push(methodInvocationExpression);
			}
		}

		public override void OnOmittedExpression(OmittedExpression node)
		{
			Push(CreateReference(node, "Boo.Lang.Compiler.Ast.OmittedExpression.Default"));
		}

		public override void OnQuasiquoteExpression(QuasiquoteExpression node)
		{
			_quasiquoteDepth++;
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, CreateReference(node, "Boo.Lang.Compiler.Ast.QuasiquoteExpression"));
			if (ShouldSerialize(node.Node))
			{
				methodInvocationExpression.NamedArguments.Add(new ExpressionPair(CreateReference(node, "Node"), Serialize(node.Node)));
			}
			Push(methodInvocationExpression);
			_quasiquoteDepth--;
		}

		public override void OnSpliceMemberReferenceExpression(SpliceMemberReferenceExpression node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceMemberReferenceExpression(node);
				return;
			}
			MethodInvocationExpression methodInvocationExpression = CreateInvocation(node, "Boo.Lang.Compiler.Ast.MemberReferenceExpression");
			methodInvocationExpression.Arguments.Add(Serialize(node.Target));
			methodInvocationExpression.Arguments.Add(LiftMemberName(node.NameExpression));
			Push(methodInvocationExpression);
		}

		private bool InsideSerializedQuasiquote()
		{
			return _quasiquoteDepth > 0;
		}

		public override void OnSpliceTypeDefinitionBody(SpliceTypeDefinitionBody node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceTypeDefinitionBody(node);
			}
			else if (node.ParentNode is EnumDefinition)
			{
				Push(LiftEnumMember(node.Expression));
			}
			else
			{
				Push(LiftTypeMember(node.Expression));
			}
		}

		public override void OnSpliceTypeMember(SpliceTypeMember node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceTypeMember(node);
				return;
			}
			MethodInvocationExpression methodInvocationExpression = (MethodInvocationExpression)Serialize(node.TypeMember);
			SpliceName(methodInvocationExpression, node.NameExpression);
			Push(methodInvocationExpression);
		}

		private void SpliceName(MethodInvocationExpression ctor, Expression nameExpression)
		{
			ctor.NamedArguments.Add(Pair("Name", LiftMemberName(nameExpression)));
		}

		private ExpressionPair Pair(string name, Expression e)
		{
			return new ExpressionPair(new ReferenceExpression(e.LexicalInfo, name), e);
		}

		public override void OnSpliceParameterDeclaration(SpliceParameterDeclaration node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceParameterDeclaration(node);
				return;
			}
			MethodInvocationExpression methodInvocationExpression = (MethodInvocationExpression)Serialize(node.ParameterDeclaration);
			SpliceName(methodInvocationExpression, node.NameExpression);
			Push(methodInvocationExpression);
		}

		public override void OnSpliceTypeReference(SpliceTypeReference node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceTypeReference(node);
			}
			else
			{
				Push(LiftTypeReference(node.Expression));
			}
		}

		public override void OnSpliceExpression(SpliceExpression node)
		{
			if (InsideSerializedQuasiquote())
			{
				SerializeSpliceExpression(node);
			}
			else if (IsStatementExpression(node))
			{
				Push(LiftStatement(node.Expression));
			}
			else
			{
				Push(LiftExpression(node.Expression));
			}
		}

		private MethodInvocationExpression LiftMemberName(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.CodeSerializer.LiftName", node);
		}

		private MethodInvocationExpression LiftStatement(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.Statement.Lift", node);
		}

		private MethodInvocationExpression LiftExpression(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.Expression.Lift", node);
		}

		private MethodInvocationExpression LiftTypeReference(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.TypeReference.Lift", node);
		}

		private MethodInvocationExpression LiftTypeMember(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.TypeMember.Lift", node);
		}

		private MethodInvocationExpression LiftEnumMember(Expression node)
		{
			return Lift("Boo.Lang.Compiler.Ast.EnumMember.Lift", node);
		}

		private MethodInvocationExpression Lift(string methodName, Expression node)
		{
			MethodInvocationExpression methodInvocationExpression = CreateInvocation(node, methodName);
			methodInvocationExpression.Arguments.Add(new QuasiquoteExpander().Visit(node));
			return methodInvocationExpression;
		}

		private MethodInvocationExpression CreateInvocation(Node sourceNode, string reference)
		{
			return CreateInvocation(sourceNode.LexicalInfo, reference);
		}

		private MethodInvocationExpression CreateInvocation(LexicalInfo lexicalInfo, string reference)
		{
			return new MethodInvocationExpression(lexicalInfo, CreateReference(lexicalInfo, reference));
		}

		private static bool IsStatementExpression(SpliceExpression node)
		{
			if (node.ParentNode == null)
			{
				return false;
			}
			return node.ParentNode.NodeType == NodeType.ExpressionStatement;
		}

		private void Push(Expression node)
		{
			_stack.Push(node);
		}

		private Expression Pop()
		{
			return _stack.Pop();
		}
	}
}
