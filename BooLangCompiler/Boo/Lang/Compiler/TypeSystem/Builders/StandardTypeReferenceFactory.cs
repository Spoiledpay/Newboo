using System.Linq;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Builders
{
	public class StandardTypeReferenceFactory : ITypeReferenceFactory
	{
		private readonly ICodeBuilder _codeBuilder;

		public StandardTypeReferenceFactory(ICodeBuilder codeBuilder)
		{
			_codeBuilder = codeBuilder;
		}

		public TypeReference TypeReferenceFor(IType type)
		{
			return CreateTypeReferenceFor(type).WithEntity(type);
		}

		private TypeReference CreateTypeReferenceFor(IType type)
		{
			if (type.IsArray)
			{
				IArrayType arrayType = (IArrayType)type;
				return new ArrayTypeReference(TypeReferenceFor(arrayType.ElementType), CreateIntegerLiteral(arrayType.Rank));
			}
			IConstructedTypeInfo constructedInfo = type.ConstructedInfo;
			if (constructedInfo != null)
			{
				return new GenericTypeReference(constructedInfo.GenericDefinition.QualifiedName(), constructedInfo.GenericArguments.Select((IType a) => TypeReferenceFor(a)).ToArray());
			}
			return new SimpleTypeReference(type.QualifiedName());
		}

		private IntegerLiteralExpression CreateIntegerLiteral(int value)
		{
			return _codeBuilder.CreateIntegerLiteral(value);
		}
	}
}
