using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps.Generators
{
	public class GeneratorItemTypeInferrer : AbstractCompilerComponent
	{
		public virtual IType GeneratorItemTypeFor(InternalMethod generator)
		{
			return base.TypeSystemServices.MapWildcardType(InferGeneratorItemTypeFor(generator));
		}

		private IType InferGeneratorItemTypeFor(InternalMethod generator)
		{
			if (base.TypeSystemServices.IsGenericGeneratorReturnType(generator.ReturnType))
			{
				return generator.ReturnType.ConstructedInfo.GenericArguments[0];
			}
			ExpressionCollection yieldExpressions = generator.YieldExpressions;
			return (yieldExpressions.Count > 0) ? base.TypeSystemServices.GetMostGenericType(yieldExpressions) : base.TypeSystemServices.ObjectType;
		}
	}
}
