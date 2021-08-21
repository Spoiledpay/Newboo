using System;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(UnaryExpression))]
	[XmlInclude(typeof(MethodInvocationExpression))]
	[XmlInclude(typeof(ReferenceExpression))]
	[XmlInclude(typeof(SlicingExpression))]
	[XmlInclude(typeof(SpliceExpression))]
	[XmlInclude(typeof(TryCastExpression))]
	[XmlInclude(typeof(TypeofExpression))]
	[XmlInclude(typeof(OmittedExpression))]
	[XmlInclude(typeof(BinaryExpression))]
	[XmlInclude(typeof(BlockExpression))]
	[XmlInclude(typeof(CastExpression))]
	[XmlInclude(typeof(ConditionalExpression))]
	[XmlInclude(typeof(ExpressionInterpolationExpression))]
	[XmlInclude(typeof(ExtendedGeneratorExpression))]
	[XmlInclude(typeof(GeneratorExpression))]
	[XmlInclude(typeof(QuasiquoteExpression))]
	[XmlInclude(typeof(LiteralExpression))]
	public abstract class Expression : Node
	{
		protected IType _expressionType;

		[XmlIgnore]
		public IType ExpressionType
		{
			get
			{
				return _expressionType;
			}
			set
			{
				_expressionType = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Expression CloneNode()
		{
			return (Expression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Expression CleanClone()
		{
			return (Expression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Matches(Node node)
		{
			if (node == null)
			{
				return false;
			}
			if (NodeType != node.NodeType)
			{
				return false;
			}
			Expression expression = (Expression)node;
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			throw new InvalidOperationException("Cannot clone abstract class: Expression");
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
		}

		public static Expression Lift(string s)
		{
			return new StringLiteralExpression(s);
		}

		public static Expression Lift(char b)
		{
			return new CharLiteralExpression(b);
		}

		public static Expression Lift(byte b)
		{
			return new IntegerLiteralExpression(b);
		}

		public static Expression Lift(bool b)
		{
			return new BoolLiteralExpression(b);
		}

		public static Expression Lift(short s)
		{
			return new IntegerLiteralExpression(s);
		}

		public static Expression Lift(int i)
		{
			return new IntegerLiteralExpression(i);
		}

		public static Expression Lift(long l)
		{
			return new IntegerLiteralExpression(l);
		}

		public static Expression Lift(float f)
		{
			return new DoubleLiteralExpression(f, isSingle: true);
		}

		public static Expression Lift(double d)
		{
			return new DoubleLiteralExpression(d);
		}

		public static Expression Lift(Regex regex)
		{
			return new RELiteralExpression($"/{regex}/");
		}

		public static Expression Lift(TimeSpan ts)
		{
			return new TimeSpanLiteralExpression(ts);
		}

		public static Expression Lift(Block block)
		{
			return new BlockExpression(block);
		}

		public static Expression Lift(Expression e)
		{
			return e?.CloneNode();
		}

		public static Expression Lift(ParameterDeclaration p)
		{
			return new ReferenceExpression(p.LexicalInfo, p.Name);
		}

		public static Expression Lift(Field f)
		{
			return new ReferenceExpression(f.LexicalInfo, f.Name);
		}

		public static Expression Lift(TypeDefinition type)
		{
			return new ReferenceExpression(type.LexicalInfo, type.FullName);
		}

		public static Expression Lift(Type type)
		{
			if (type.IsGenericType)
			{
				return LiftGenericType(type);
			}
			return ReferenceExpressionFor(type);
		}

		private static ReferenceExpression ReferenceExpressionFor(Type type)
		{
			return ReferenceExpression.Lift(TypeUtilities.GetFullName(type));
		}

		private static Expression LiftGenericType(Type type)
		{
			GenericReferenceExpression genericReferenceExpression = new GenericReferenceExpression();
			genericReferenceExpression.Target = ReferenceExpressionFor(type);
			GenericReferenceExpression genericReferenceExpression2 = genericReferenceExpression;
			Type[] genericArguments = type.GetGenericArguments();
			foreach (Type type2 in genericArguments)
			{
				genericReferenceExpression2.GenericArguments.Add(TypeReference.Lift(type2));
			}
			return genericReferenceExpression2;
		}

		public Expression()
		{
		}

		public Expression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
