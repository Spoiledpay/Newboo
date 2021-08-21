namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IType : ITypedEntity, INamespace, IEntity, IEntityWithAttributes
	{
		bool IsClass { get; }

		bool IsAbstract { get; }

		bool IsInterface { get; }

		bool IsEnum { get; }

		bool IsByRef { get; }

		bool IsValueType { get; }

		bool IsFinal { get; }

		bool IsArray { get; }

		bool IsPointer { get; }

		IEntity DeclaringEntity { get; }

		IType ElementType { get; }

		IType BaseType { get; }

		IGenericTypeInfo GenericInfo { get; }

		IConstructedTypeInfo ConstructedInfo { get; }

		int GetTypeDepth();

		IEntity GetDefaultMember();

		IType[] GetInterfaces();

		bool IsSubclassOf(IType other);

		bool IsAssignableFrom(IType other);

		IArrayType MakeArrayType(int rank);

		IType MakePointerType();
	}
}
