using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Ast
{
	public static class AstUtil
	{
		public static bool IsOverloadableOperator(BinaryOperatorType op)
		{
			switch (op)
			{
			case BinaryOperatorType.Addition:
			case BinaryOperatorType.Subtraction:
			case BinaryOperatorType.Multiply:
			case BinaryOperatorType.Division:
			case BinaryOperatorType.Modulus:
			case BinaryOperatorType.Exponentiation:
			case BinaryOperatorType.LessThan:
			case BinaryOperatorType.LessThanOrEqual:
			case BinaryOperatorType.GreaterThan:
			case BinaryOperatorType.GreaterThanOrEqual:
			case BinaryOperatorType.Match:
			case BinaryOperatorType.NotMatch:
			case BinaryOperatorType.Member:
			case BinaryOperatorType.NotMember:
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
			case BinaryOperatorType.ShiftLeft:
			case BinaryOperatorType.ShiftRight:
				return true;
			default:
				return false;
			}
		}

		public static string GetMethodNameForOperator(BinaryOperatorType op)
		{
			return "op_" + op;
		}

		public static string GetMethodNameForOperator(UnaryOperatorType op)
		{
			return "op_" + op;
		}

		public static Node GetMemberAnchor(Node node)
		{
			MemberReferenceExpression memberReferenceExpression = node as MemberReferenceExpression;
			return (memberReferenceExpression != null) ? memberReferenceExpression.Target : node;
		}

		public static bool IsPostUnaryOperator(UnaryOperatorType op)
		{
			return UnaryOperatorType.PostIncrement == op || UnaryOperatorType.PostDecrement == op;
		}

		public static bool IsIncDec(Node node)
		{
			if (node.NodeType == NodeType.UnaryExpression)
			{
				UnaryOperatorType @operator = ((UnaryExpression)node).Operator;
				return UnaryOperatorType.Increment == @operator || UnaryOperatorType.PostIncrement == @operator || UnaryOperatorType.Decrement == @operator || UnaryOperatorType.PostDecrement == @operator;
			}
			return false;
		}

		public static BinaryOperatorKind GetBinaryOperatorKind(BinaryExpression expression)
		{
			return GetBinaryOperatorKind(expression.Operator, exact: false);
		}

		public static BinaryOperatorKind GetBinaryOperatorKind(BinaryExpression expression, bool exact)
		{
			return GetBinaryOperatorKind(expression.Operator, exact);
		}

		public static BinaryOperatorKind GetBinaryOperatorKind(BinaryOperatorType op)
		{
			return GetBinaryOperatorKind(op, exact: false);
		}

		public static BinaryOperatorKind GetBinaryOperatorKind(BinaryOperatorType op, bool exact)
		{
			switch (op)
			{
			case BinaryOperatorType.Addition:
			case BinaryOperatorType.Subtraction:
			case BinaryOperatorType.Multiply:
			case BinaryOperatorType.Division:
			case BinaryOperatorType.Modulus:
			case BinaryOperatorType.Exponentiation:
				return BinaryOperatorKind.Arithmetic;
			case BinaryOperatorType.LessThan:
			case BinaryOperatorType.LessThanOrEqual:
			case BinaryOperatorType.GreaterThan:
			case BinaryOperatorType.GreaterThanOrEqual:
			case BinaryOperatorType.Equality:
			case BinaryOperatorType.Inequality:
			case BinaryOperatorType.Match:
			case BinaryOperatorType.NotMatch:
			case BinaryOperatorType.ReferenceEquality:
			case BinaryOperatorType.ReferenceInequality:
				return BinaryOperatorKind.Comparison;
			case BinaryOperatorType.TypeTest:
			case BinaryOperatorType.Member:
			case BinaryOperatorType.NotMember:
				return exact ? BinaryOperatorKind.TypeComparison : BinaryOperatorKind.Comparison;
			case BinaryOperatorType.Assign:
				return BinaryOperatorKind.Assignment;
			case BinaryOperatorType.InPlaceAddition:
			case BinaryOperatorType.InPlaceSubtraction:
			case BinaryOperatorType.InPlaceMultiply:
			case BinaryOperatorType.InPlaceDivision:
			case BinaryOperatorType.InPlaceModulus:
			case BinaryOperatorType.InPlaceBitwiseAnd:
			case BinaryOperatorType.InPlaceBitwiseOr:
			case BinaryOperatorType.InPlaceExclusiveOr:
			case BinaryOperatorType.InPlaceShiftLeft:
			case BinaryOperatorType.InPlaceShiftRight:
				return exact ? BinaryOperatorKind.InPlaceAssignment : BinaryOperatorKind.Assignment;
			case BinaryOperatorType.Or:
			case BinaryOperatorType.And:
				return BinaryOperatorKind.Logical;
			case BinaryOperatorType.BitwiseOr:
			case BinaryOperatorType.BitwiseAnd:
			case BinaryOperatorType.ExclusiveOr:
			case BinaryOperatorType.ShiftLeft:
			case BinaryOperatorType.ShiftRight:
				return BinaryOperatorKind.Bitwise;
			default:
				throw new NotSupportedException($"unknown operator: {op}");
			}
		}

		public static bool IsAssignment(Node node)
		{
			if (node.NodeType == NodeType.BinaryExpression)
			{
				return GetBinaryOperatorKind((BinaryExpression)node) == BinaryOperatorKind.Assignment;
			}
			return false;
		}

		public static ClassDefinition GetParentClass(Node node)
		{
			return (ClassDefinition)node.GetAncestor(NodeType.ClassDefinition);
		}

		public static Node GetParentTryExceptEnsure(Node node)
		{
			Node parentNode = node.ParentNode;
			while (null != parentNode)
			{
				switch (parentNode.NodeType)
				{
				case NodeType.TryStatement:
				case NodeType.ExceptionHandler:
					return parentNode;
				case NodeType.Block:
					if (NodeType.TryStatement == parentNode.ParentNode.NodeType && parentNode == ((TryStatement)parentNode.ParentNode).EnsureBlock)
					{
						return parentNode;
					}
					break;
				case NodeType.Method:
					return null;
				}
				parentNode = parentNode.ParentNode;
			}
			return null;
		}

		public static bool IsListGenerator(Node node)
		{
			return NodeType.ListLiteralExpression == node.NodeType && IsListGenerator((ListLiteralExpression)node);
		}

		public static bool IsListGenerator(ListLiteralExpression node)
		{
			if (1 == node.Items.Count)
			{
				NodeType nodeType = node.Items[0].NodeType;
				return NodeType.GeneratorExpression == nodeType;
			}
			return false;
		}

		public static bool IsListMultiGenerator(Node node)
		{
			return NodeType.ListLiteralExpression == node.NodeType && IsListMultiGenerator((ListLiteralExpression)node);
		}

		public static bool IsListMultiGenerator(ListLiteralExpression node)
		{
			if (1 == node.Items.Count)
			{
				NodeType nodeType = node.Items[0].NodeType;
				return NodeType.ExtendedGeneratorExpression == nodeType;
			}
			return false;
		}

		public static bool IsTargetOfMethodInvocation(Expression node)
		{
			return IsTargetOfGenericMethodInvocation(node) || (node.ParentNode.NodeType == NodeType.MethodInvocationExpression && node == ((MethodInvocationExpression)node.ParentNode).Target);
		}

		public static bool IsTargetOfGenericMethodInvocation(Expression node)
		{
			return node.ParentNode.NodeType == NodeType.GenericReferenceExpression && node.ParentNode.ParentNode != null && node.ParentNode.ParentNode.NodeType == NodeType.MethodInvocationExpression && node.ParentNode == ((MethodInvocationExpression)node.ParentNode.ParentNode).Target;
		}

		public static bool IsTargetOfMemberReference(Expression node)
		{
			return node.ParentNode.NodeType == NodeType.MemberReferenceExpression && node == ((MemberReferenceExpression)node.ParentNode).Target;
		}

		public static bool IsTargetOfSlicing(Expression node)
		{
			if (NodeType.SlicingExpression == node.ParentNode.NodeType && node == ((SlicingExpression)node.ParentNode).Target)
			{
				return true;
			}
			return false;
		}

		[Obsolete("Use node.IsTargetOfAssignment()")]
		public static bool IsLhsOfAssignment(Expression node)
		{
			return node.IsTargetOfAssignment();
		}

		public static bool IsRhsOfAssignment(Expression node)
		{
			BinaryExpression binaryExpression = node.ParentNode as BinaryExpression;
			if (binaryExpression == null)
			{
				return false;
			}
			return node == binaryExpression.Right && IsAssignment(binaryExpression);
		}

		public static bool IsLhsOfInPlaceAddSubtract(Expression node)
		{
			if (NodeType.BinaryExpression == node.ParentNode.NodeType)
			{
				BinaryExpression binaryExpression = (BinaryExpression)node.ParentNode;
				if (node == binaryExpression.Left)
				{
					BinaryOperatorType @operator = binaryExpression.Operator;
					return @operator == BinaryOperatorType.InPlaceAddition || @operator == BinaryOperatorType.InPlaceSubtraction;
				}
			}
			return false;
		}

		public static bool IsStandaloneReference(Node node)
		{
			Node parentNode = node.ParentNode;
			if (parentNode is GenericReferenceExpression)
			{
				parentNode = parentNode.ParentNode;
			}
			return parentNode.NodeType != NodeType.MemberReferenceExpression;
		}

		public static Constructor CreateConstructor(Node lexicalInfoProvider, TypeMemberModifiers modifiers)
		{
			Constructor constructor = new Constructor(lexicalInfoProvider.LexicalInfo);
			constructor.Modifiers = modifiers;
			constructor.IsSynthetic = true;
			return constructor;
		}

		public static Constructor CreateDefaultConstructor(TypeDefinition type)
		{
			TypeMemberModifiers modifiers = TypeMemberModifiers.Public;
			if (type.IsAbstract)
			{
				modifiers = TypeMemberModifiers.Protected;
			}
			return CreateConstructor(type, modifiers);
		}

		public static ReferenceExpression CreateReferenceExpression(LexicalInfo li, string fullname)
		{
			ReferenceExpression referenceExpression = CreateReferenceExpression(fullname);
			referenceExpression.LexicalInfo = li;
			return referenceExpression;
		}

		public static ReferenceExpression CreateReferenceExpression(string fullname)
		{
			string[] array = fullname.Split('.');
			ReferenceExpression referenceExpression = new ReferenceExpression(array[0]);
			referenceExpression.IsSynthetic = true;
			ReferenceExpression referenceExpression2 = referenceExpression;
			for (int i = 1; i < array.Length; i++)
			{
				MemberReferenceExpression memberReferenceExpression = new MemberReferenceExpression(referenceExpression2, array[i]);
				memberReferenceExpression.IsSynthetic = true;
				referenceExpression2 = memberReferenceExpression;
			}
			return referenceExpression2;
		}

		public static MethodInvocationExpression CreateMethodInvocationExpression(Expression target, Expression arg)
		{
			return CreateMethodInvocationExpression(arg.LexicalInfo, target, arg);
		}

		public static MethodInvocationExpression CreateMethodInvocationExpression(LexicalInfo li, Expression target, Expression arg)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(li);
			methodInvocationExpression.Target = (Expression)target.Clone();
			methodInvocationExpression.IsSynthetic = true;
			MethodInvocationExpression methodInvocationExpression2 = methodInvocationExpression;
			methodInvocationExpression2.Arguments.Add((Expression)arg.Clone());
			return methodInvocationExpression2;
		}

		public static bool IsExplodeExpression(Node node)
		{
			UnaryExpression unaryExpression = node as UnaryExpression;
			return unaryExpression != null && unaryExpression.Operator == UnaryOperatorType.Explode;
		}

		public static bool IsIndirection(Node node)
		{
			UnaryExpression unaryExpression = node as UnaryExpression;
			return unaryExpression != null && unaryExpression.Operator == UnaryOperatorType.Indirection;
		}

		public static string ToXml(Node node)
		{
			StringWriter stringWriter = new StringWriter();
			new XmlSerializer(node.GetType()).Serialize(stringWriter, node);
			return stringWriter.ToString();
		}

		public static Node FromXml(Type type, string code)
		{
			return (Node)new XmlSerializer(type).Deserialize(new StringReader(code));
		}

		public static void DebugNode(Node node)
		{
			Console.WriteLine("{0}: {1} - {2}", node.LexicalInfo, node.NodeType, SafeToCodeString(node));
		}

		public static string SafeToCodeString(Node node)
		{
			try
			{
				return node.ToCodeString();
			}
			catch (Exception)
			{
				return "<unavailable>";
			}
		}

		public static string BuildUniqueTypeMemberName(TypeDefinition type, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			StringBuilder stringBuilder = new StringBuilder("$");
			stringBuilder.Append(name);
			stringBuilder.Append("__");
			stringBuilder.Append(type.QualifiedName);
			if (type.HasGenericParameters)
			{
				stringBuilder.Append("_");
				foreach (string item in type.GenericParameters.Select((GenericParameterDeclaration gpd) => gpd.Name))
				{
					stringBuilder.Append("_");
					stringBuilder.Append(item);
				}
			}
			stringBuilder.Replace('.', '_');
			stringBuilder.Append("$");
			return stringBuilder.ToString();
		}

		internal static Local GetLocalByName(Method method, string name)
		{
			if (method.Locals.Count == 0)
			{
				return null;
			}
			return method.Locals.FirstOrDefault((Local local) => !local.PrivateScope && local.Name == name);
		}

		public static LexicalInfo SafeLexicalInfo(Node node)
		{
			if (node == null)
			{
				return LexicalInfo.Empty;
			}
			LexicalInfo lexicalInfo = node.LexicalInfo;
			return lexicalInfo.IsValid ? lexicalInfo : SafeLexicalInfo(node.ParentNode);
		}

		public static string SafePositionOnlyLexicalInfo(Node node)
		{
			LexicalInfo lexicalInfo = SafeLexicalInfo(node);
			return $"({lexicalInfo.Line},{lexicalInfo.Column})";
		}

		public static ICollection<TValue> GetValues<TNode, TValue>(NodeCollection<TNode> nodes) where TNode : LiteralExpression
		{
			return nodes.Select((TNode node) => (TValue)Convert.ChangeType(node.ValueObject, typeof(TValue))).ToList();
		}

		internal static bool AllCodePathsReturnOrRaise(Block block)
		{
			if (block == null || block.IsEmpty)
			{
				return false;
			}
			Statement lastStatement = block.LastStatement;
			switch (lastStatement.NodeType)
			{
			case NodeType.ReturnStatement:
			case NodeType.RaiseStatement:
				return true;
			case NodeType.Block:
				return AllCodePathsReturnOrRaise((Block)lastStatement);
			case NodeType.IfStatement:
			{
				IfStatement ifStatement = (IfStatement)lastStatement;
				return AllCodePathsReturnOrRaise(ifStatement.TrueBlock) && AllCodePathsReturnOrRaise(ifStatement.FalseBlock);
			}
			case NodeType.TryStatement:
			{
				TryStatement tryStatement = (TryStatement)lastStatement;
				if (!AllCodePathsReturnOrRaise(tryStatement.ProtectedBlock))
				{
					return false;
				}
				return tryStatement.ExceptionHandlers.Select((ExceptionHandler handler) => handler.Block).All(AllCodePathsReturnOrRaise);
			}
			default:
				return false;
			}
		}

		internal static RegexOptions GetRegexOptions(RELiteralExpression re)
		{
			RegexOptions regexOptions = RegexOptions.None;
			if (string.IsNullOrEmpty(re.Options))
			{
				return regexOptions;
			}
			string options = re.Options;
			foreach (char c in options)
			{
				switch (c)
				{
				case 'i':
					regexOptions |= RegexOptions.IgnoreCase;
					break;
				case 'm':
					regexOptions |= RegexOptions.Multiline;
					break;
				case 's':
					regexOptions |= RegexOptions.Singleline;
					break;
				case 'n':
					regexOptions |= RegexOptions.CultureInvariant;
					break;
				case 'c':
					regexOptions |= RegexOptions.Compiled;
					break;
				case 'e':
					regexOptions |= RegexOptions.ExplicitCapture;
					break;
				default:
					My<CompilerErrorCollection>.Instance.Add(CompilerErrorFactory.InvalidRegexOption(re, c));
					break;
				case 'g':
				case 'l':
				case 'x':
					break;
				}
			}
			return regexOptions;
		}

		public static bool IsTargetOfGenericReferenceExpression(Expression node)
		{
			GenericReferenceExpression genericReferenceExpression = node.ParentNode as GenericReferenceExpression;
			if (genericReferenceExpression == null)
			{
				return false;
			}
			return genericReferenceExpression.Target == node;
		}

		public static bool InvocationEndsWithExplodeExpression(MethodInvocationExpression node)
		{
			return EndsWithExplodeExpression(node.Arguments);
		}

		public static bool EndsWithExplodeExpression(ExpressionCollection expressionCollection)
		{
			return expressionCollection.Count > 0 && IsExplodeExpression(expressionCollection[-1]);
		}

		public static string TypeKeywordFor(TypeDefinition node)
		{
			return node.NodeType switch
			{
				NodeType.ClassDefinition => "class", 
				NodeType.InterfaceDefinition => "interface", 
				NodeType.StructDefinition => "struct", 
				NodeType.EnumDefinition => "enum", 
				_ => throw new ArgumentException("Unsupported type definition kind: " + node.NodeType, "node"), 
			};
		}
	}
}
