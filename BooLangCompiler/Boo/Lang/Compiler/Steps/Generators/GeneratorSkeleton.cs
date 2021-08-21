using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;

namespace Boo.Lang.Compiler.Steps.Generators
{
	public class GeneratorSkeleton
	{
		public readonly BooClassBuilder GeneratorClassBuilder;

		public readonly IType GeneratorItemType;

		public readonly BooMethodBuilder GetEnumeratorBuilder;

		public GeneratorSkeleton(BooClassBuilder generatorBuilder, BooMethodBuilder getEnumeratorBuilder, IType generatorItemType)
		{
			GeneratorClassBuilder = generatorBuilder;
			GeneratorItemType = generatorItemType;
			GetEnumeratorBuilder = getEnumeratorBuilder;
		}
	}
}
