using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Builders
{
	public class MappedTypeReferenceFactory : ITypeReferenceFactory
	{
		private readonly ITypeReferenceFactory _typeReferenceFactory;

		private readonly IDictionary<IType, IType> _typeMappings;

		public MappedTypeReferenceFactory(ITypeReferenceFactory typeReferenceFactory, IDictionary<IType, IType> typeMappings)
		{
			_typeReferenceFactory = typeReferenceFactory;
			_typeMappings = typeMappings;
		}

		public TypeReference TypeReferenceFor(IType type)
		{
			return _typeReferenceFactory.TypeReferenceFor(Map(type));
		}

		private IType Map(IType type)
		{
			if (_typeMappings.TryGetValue(type, out var value))
			{
				return value;
			}
			if (type.IsArray)
			{
				IArrayType arrayType = (IArrayType)type;
				IType type2 = Map(arrayType.ElementType);
				return type2.MakeArrayType(arrayType.Rank);
			}
			IConstructedTypeInfo constructedInfo = type.ConstructedInfo;
			if (constructedInfo != null)
			{
				return constructedInfo.GenericDefinition.GenericInfo.ConstructType(constructedInfo.GenericArguments.Select((IType a) => Map(a)).ToArray());
			}
			return type;
		}
	}
}
