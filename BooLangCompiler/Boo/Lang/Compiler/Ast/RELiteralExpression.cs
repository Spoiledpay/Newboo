using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class RELiteralExpression : LiteralExpression
	{
		protected string _value;

		private Regex _regex;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.RELiteralExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public string Pattern => Value.Substring(1, Value.LastIndexOf('/') - 1);

		public string Options
		{
			get
			{
				int num = Value.LastIndexOf('/');
				return (num == Value.Length - 1) ? string.Empty : Value.Substring(num + 1);
			}
		}

		public Regex Regex
		{
			get
			{
				if (null == _regex)
				{
					_regex = new Regex(Pattern, AstUtil.GetRegexOptions(this));
				}
				return _regex;
			}
		}

		public override object ValueObject => Regex;

		[GeneratedCode("astgen.boo", "1")]
		public new RELiteralExpression CloneNode()
		{
			return (RELiteralExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new RELiteralExpression CleanClone()
		{
			return (RELiteralExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnRELiteralExpression(this);
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
			RELiteralExpression rELiteralExpression = (RELiteralExpression)node;
			if (_value != rELiteralExpression._value)
			{
				return NoMatch("RELiteralExpression._value");
			}
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
			RELiteralExpression rELiteralExpression = new RELiteralExpression();
			rELiteralExpression._lexicalInfo = _lexicalInfo;
			rELiteralExpression._endSourceLocation = _endSourceLocation;
			rELiteralExpression._documentation = _documentation;
			rELiteralExpression._isSynthetic = _isSynthetic;
			rELiteralExpression._entity = _entity;
			if (_annotations != null)
			{
				rELiteralExpression._annotations = (Hashtable)_annotations.Clone();
			}
			rELiteralExpression._expressionType = _expressionType;
			rELiteralExpression._value = _value;
			return rELiteralExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
		}

		public RELiteralExpression()
		{
		}

		public RELiteralExpression(string value)
		{
			Value = value;
		}

		public RELiteralExpression(LexicalInfo lexicalInfoProvider, string value)
			: base(lexicalInfoProvider)
		{
			Value = value;
		}
	}
}
