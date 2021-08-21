using System;
using System.CodeDom.Compiler;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ParameterDeclarationCollection : NodeCollection<ParameterDeclaration>
	{
		public bool HasParamArray { get; set; }

		[Obsolete("Use HasParamArray instead.")]
		public bool VariableNumber
		{
			get
			{
				return HasParamArray;
			}
			set
			{
				HasParamArray = value;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public static ParameterDeclarationCollection FromArray(params ParameterDeclaration[] items)
		{
			ParameterDeclarationCollection parameterDeclarationCollection = new ParameterDeclarationCollection();
			parameterDeclarationCollection.AddRange(items);
			return parameterDeclarationCollection;
		}

		[GeneratedCode("astgen.boo", "1")]
		public ParameterDeclarationCollection PopRange(int begin)
		{
			ParameterDeclarationCollection parameterDeclarationCollection = new ParameterDeclarationCollection(base.ParentNode);
			parameterDeclarationCollection.InnerList.AddRange(InternalPopRange(begin));
			return parameterDeclarationCollection;
		}

		public static ParameterDeclarationCollection FromArray(bool hasParamArray, params ParameterDeclaration[] parameters)
		{
			ParameterDeclarationCollection parameterDeclarationCollection = FromArray(parameters);
			parameterDeclarationCollection.HasParamArray = hasParamArray;
			return parameterDeclarationCollection;
		}

		public ParameterDeclarationCollection()
		{
		}

		public ParameterDeclarationCollection(Node parent)
			: base(parent)
		{
		}

		public override object Clone()
		{
			ParameterDeclarationCollection parameterDeclarationCollection = (ParameterDeclarationCollection)base.Clone();
			parameterDeclarationCollection.HasParamArray = HasParamArray;
			return parameterDeclarationCollection;
		}
	}
}
