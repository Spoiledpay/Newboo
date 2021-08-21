using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Services;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler
{
	public static class CompilerWarningFactory
	{
		public static class Codes
		{
			public const string ImplicitReturn = "BCW0023";

			public const string VisibleMemberDoesNotDeclareTypeExplicitely = "BCW0024";

			public const string ImplicitDowncast = "BCW0028";
		}

		private static LanguageAmbiance LanguageAmbiance => My<LanguageAmbiance>.Instance;

		public static CompilerWarning CustomWarning(Node node, string msg)
		{
			return CustomWarning(AstUtil.SafeLexicalInfo(node), msg);
		}

		public static CompilerWarning CustomWarning(LexicalInfo lexicalInfo, string msg)
		{
			return new CompilerWarning(lexicalInfo, msg);
		}

		public static CompilerWarning CustomWarning(string msg)
		{
			return new CompilerWarning(msg);
		}

		public static CompilerWarning AbstractMemberNotImplemented(Node node, IType type, IMember member)
		{
			return Instantiate("BCW0001", AstUtil.SafeLexicalInfo(node), type, member);
		}

		public static CompilerWarning ModifiersInLabelsHaveNoEffect(Node node)
		{
			return Instantiate("BCW0002", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning UnusedLocalVariable(Node node, string name)
		{
			return Instantiate("BCW0003", AstUtil.SafeLexicalInfo(node), name);
		}

		public static CompilerWarning IsInsteadOfIsa(Node node)
		{
			return Instantiate("BCW0004", AstUtil.SafeLexicalInfo(node), LanguageAmbiance.IsKeyword, LanguageAmbiance.IsaKeyword);
		}

		public static CompilerWarning InvalidEventUnsubscribe(Node node, IEvent eventName, CallableSignature expected)
		{
			return Instantiate("BCW0005", AstUtil.SafeLexicalInfo(node), eventName, expected);
		}

		public static CompilerWarning AssignmentToTemporary(Node node)
		{
			return Instantiate("BCW0006", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning EqualsInsteadOfAssign(BinaryExpression node)
		{
			return Instantiate("BCW0007", AstUtil.SafeLexicalInfo(node), node.ToCodeString());
		}

		public static CompilerWarning DuplicateNamespace(Import import, string name)
		{
			return Instantiate("BCW0008", AstUtil.SafeLexicalInfo(import.Expression), name);
		}

		public static CompilerWarning HaveBothKeyFileAndAttribute(Node node)
		{
			return Instantiate("BCW0009", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning HaveBothKeyNameAndAttribute(Node node)
		{
			return Instantiate("BCW0010", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning AbstractMemberNotImplementedStubCreated(Node node, IType type, IMember abstractMember)
		{
			return Instantiate("BCW0011", AstUtil.SafeLexicalInfo(node), type, abstractMember);
		}

		public static CompilerWarning Obsolete(Node node, IMember member, string message)
		{
			return Instantiate("BCW0012", AstUtil.SafeLexicalInfo(node), member, message);
		}

		public static CompilerWarning StaticClassMemberRedundantlyMarkedStatic(Node node, string typeName, string memberName)
		{
			return Instantiate("BCW0013", AstUtil.SafeLexicalInfo(node), typeName, memberName);
		}

		public static CompilerWarning PrivateMemberNeverUsed(TypeMember member)
		{
			return Instantiate("BCW0014", AstUtil.SafeLexicalInfo(member), MemberVisibilityString(member), NodeTypeString(member), member.FullName);
		}

		public static CompilerWarning UnreachableCodeDetected(Node node)
		{
			return Instantiate("BCW0015", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning NamespaceNeverUsed(Import node)
		{
			return Instantiate("BCW0016", AstUtil.SafeLexicalInfo(node.Expression), node.Expression.ToCodeString());
		}

		public static CompilerWarning NewProtectedMemberInSealedType(TypeMember member)
		{
			return Instantiate("BCW0017", AstUtil.SafeLexicalInfo(member), NodeTypeString(member), member.Name, member.DeclaringType.Name);
		}

		public static CompilerWarning OverridingFinalizeIsBadPractice(TypeMember member)
		{
			return Instantiate("BCW0018", AstUtil.SafeLexicalInfo(member));
		}

		public static CompilerWarning AmbiguousExceptionName(ExceptionHandler node)
		{
			return Instantiate("BCW0019", AstUtil.SafeLexicalInfo(node), node.Declaration.Name);
		}

		public static CompilerWarning AssignmentToSameVariable(BinaryExpression node)
		{
			return Instantiate("BCW0020", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning ComparisonWithSameVariable(BinaryExpression node)
		{
			return Instantiate("BCW0021", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning ConstantExpression(Expression node)
		{
			return Instantiate("BCW0022", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning ImplicitReturn(Method node)
		{
			return Instantiate("BCW0023", AstUtil.SafeLexicalInfo(node));
		}

		public static CompilerWarning VisibleMemberDoesNotDeclareTypeExplicitely(TypeMember node)
		{
			return VisibleMemberDoesNotDeclareTypeExplicitely(node, null);
		}

		public static CompilerWarning VisibleMemberDoesNotDeclareTypeExplicitely(TypeMember node, string argument)
		{
			string text = ((argument == null) ? "return" : $"'{argument}' argument");
			return Instantiate("BCW0024", AstUtil.SafeLexicalInfo(node), NodeTypeString(node), text);
		}

		public static CompilerWarning AmbiguousVariableName(Local node, string localName, string baseName)
		{
			return Instantiate("BCW0025", AstUtil.SafeLexicalInfo(node), localName, baseName);
		}

		public static CompilerWarning LikelyTypoInTypeMemberName(TypeMember node, string suggestion)
		{
			return Instantiate("BCW0026", AstUtil.SafeLexicalInfo(node), node.Name, suggestion);
		}

		public static CompilerWarning ObsoleteSyntax(Node anchor, string obsoleteSyntax, string newSyntax)
		{
			return ObsoleteSyntax(AstUtil.SafeLexicalInfo(anchor), obsoleteSyntax, newSyntax);
		}

		public static CompilerWarning ObsoleteSyntax(LexicalInfo location, string obsoleteSyntax, string newSyntax)
		{
			return Instantiate("BCW0027", location, obsoleteSyntax, newSyntax);
		}

		public static CompilerWarning ImplicitDowncast(Node node, IType expectedType, IType actualType)
		{
			return Instantiate("BCW0028", AstUtil.SafeLexicalInfo(node), actualType, expectedType);
		}

		public static CompilerWarning MethodHidesInheritedNonVirtual(Node anchor, IMethod hidingMethod, IMethod hiddenMethod)
		{
			return Instantiate("BCW0029", AstUtil.SafeLexicalInfo(anchor), hidingMethod, hiddenMethod);
		}

		private static CompilerWarning Instantiate(string code, LexicalInfo location, params object[] args)
		{
			return new CompilerWarning(code, location, Array.ConvertAll(args, CompilerErrorFactory.DisplayStringFor));
		}

		private static string NodeTypeString(Node node)
		{
			return node.NodeType.ToString().ToLower();
		}

		private static string MemberVisibilityString(TypeMember member)
		{
			return (member.Modifiers & TypeMemberModifiers.VisibilityMask) switch
			{
				TypeMemberModifiers.Private => "Private", 
				TypeMemberModifiers.Internal => "Internal", 
				TypeMemberModifiers.Protected => "Protected", 
				_ => "Public", 
			};
		}
	}
}
