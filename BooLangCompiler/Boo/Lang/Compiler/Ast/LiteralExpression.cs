using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	[XmlInclude(typeof(RELiteralExpression))]
	[XmlInclude(typeof(DoubleLiteralExpression))]
	[XmlInclude(typeof(NullLiteralExpression))]
	[XmlInclude(typeof(SelfLiteralExpression))]
	[XmlInclude(typeof(ListLiteralExpression))]
	[XmlInclude(typeof(TimeSpanLiteralExpression))]
	[XmlInclude(typeof(IntegerLiteralExpression))]
	[XmlInclude(typeof(SuperLiteralExpression))]
	[XmlInclude(typeof(StringLiteralExpression))]
	[XmlInclude(typeof(BoolLiteralExpression))]
	[XmlInclude(typeof(HashLiteralExpression))]
	public abstract class LiteralExpression : Expression
	{
		public virtual object ValueObject
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new LiteralExpression CloneNode()
		{
			return (LiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new LiteralExpression CleanClone()
		{
			return (LiteralExpression)base.CleanClone();
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
			LiteralExpression literalExpression = (LiteralExpression)node;
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
			throw new InvalidOperationException("Cannot clone abstract class: LiteralExpression");
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public LiteralExpression()
		{
		}

		public LiteralExpression(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
