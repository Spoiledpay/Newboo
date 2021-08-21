namespace Boo.Lang.Compiler.TypeSystem
{
	public interface IField : IAccessibleMember, IMember, ITypedEntity, IEntity, IEntityWithAttributes
	{
		bool IsInitOnly { get; }

		bool IsLiteral { get; }

		object StaticValue { get; }

		bool IsVolatile { get; }
	}
}
