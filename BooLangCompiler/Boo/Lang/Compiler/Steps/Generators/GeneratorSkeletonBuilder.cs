using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.Generators
{
	public class GeneratorSkeletonBuilder : AbstractCompilerComponent
	{
		private EnvironmentProvision<GeneratorItemTypeInferrer> _generatorItemTypeInferrer;

		public GeneratorSkeleton SkeletonFor(InternalMethod generator)
		{
			Method method = generator.Method;
			return CreateGeneratorSkeleton(method, method, GeneratorItemTypeFor(generator));
		}

		public GeneratorSkeleton SkeletonFor(GeneratorExpression generator, Method enclosingMethod)
		{
			return CreateGeneratorSkeleton(generator, enclosingMethod, base.TypeSystemServices.GetConcreteExpressionType(generator.Expression));
		}

		protected virtual IType GeneratorItemTypeFor(InternalMethod generator)
		{
			return _generatorItemTypeInferrer.Instance.GeneratorItemTypeFor(generator);
		}

		private GeneratorSkeleton CreateGeneratorSkeleton(Node sourceNode, Method enclosingMethod, IType generatorItemType)
		{
			BooClassBuilder booClassBuilder = SetUpEnumerableClassBuilder(sourceNode, enclosingMethod, generatorItemType);
			BooMethodBuilder getEnumeratorBuilder = SetUpGetEnumeratorMethodBuilder(sourceNode, booClassBuilder, generatorItemType);
			enclosingMethod.DeclaringType.Members.Add(booClassBuilder.ClassDefinition);
			return new GeneratorSkeleton(booClassBuilder, getEnumeratorBuilder, generatorItemType);
		}

		private BooMethodBuilder SetUpGetEnumeratorMethodBuilder(Node sourceNode, BooClassBuilder builder, IType generatorItemType)
		{
			BooMethodBuilder booMethodBuilder = builder.AddVirtualMethod("GetEnumerator", base.TypeSystemServices.IEnumeratorGenericType.GenericInfo.ConstructType(generatorItemType));
			booMethodBuilder.Method.LexicalInfo = sourceNode.LexicalInfo;
			return booMethodBuilder;
		}

		private BooClassBuilder SetUpEnumerableClassBuilder(Node sourceNode, Method enclosingMethod, IType generatorItemType)
		{
			BooClassBuilder booClassBuilder = base.CodeBuilder.CreateClass(base.Context.GetUniqueName(enclosingMethod.Name), TypeMemberModifiers.Internal | TypeMemberModifiers.Final);
			if (enclosingMethod.DeclaringType.IsTransient)
			{
				booClassBuilder.Modifiers |= TypeMemberModifiers.Transient;
			}
			booClassBuilder.LexicalInfo = new LexicalInfo(sourceNode.LexicalInfo);
			booClassBuilder.AddBaseType(base.TypeSystemServices.Map(typeof(GenericGenerator<>)).GenericInfo.ConstructType(generatorItemType));
			booClassBuilder.AddAttribute(base.CodeBuilder.CreateAttribute(typeof(CompilerGeneratedAttribute)));
			return booClassBuilder;
		}
	}
}
