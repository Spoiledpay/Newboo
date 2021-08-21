namespace Boo.Lang.Compiler.TypeSystem
{
	public class BuiltinFunction : IEntity
	{
		public static BuiltinFunction Quack = new BuiltinFunction("quack", BuiltinFunctionType.Quack);

		public static BuiltinFunction Len = new BuiltinFunction("len", BuiltinFunctionType.Len);

		public static BuiltinFunction AddressOf = new BuiltinFunction("__addressof__", BuiltinFunctionType.AddressOf);

		public static BuiltinFunction Eval = new BuiltinFunction("@", BuiltinFunctionType.Eval);

		public static BuiltinFunction Switch = new BuiltinFunction("__switch__", BuiltinFunctionType.Switch);

		public static BuiltinFunction InitValueType = new BuiltinFunction("__initobj__", BuiltinFunctionType.InitValueType);

		private BuiltinFunctionType _type;

		private string _name;

		public string Name => _name;

		public string FullName => Name;

		public EntityType EntityType => EntityType.BuiltinFunction;

		public BuiltinFunctionType FunctionType => _type;

		public BuiltinFunction(string name, BuiltinFunctionType type)
		{
			_name = name;
			_type = type;
		}

		public override string ToString()
		{
			return $"BuiltinFunction(\"{_name}\", {_type})";
		}
	}
}
