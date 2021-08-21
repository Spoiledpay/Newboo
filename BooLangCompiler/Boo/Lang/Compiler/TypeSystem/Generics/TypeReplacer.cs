using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class TypeReplacer : TypeMapper
	{
		private IDictionary<IType, IType> _map;

		protected IDictionary<IType, IType> TypeMap => _map;

		public TypeReplacer()
		{
			_map = new Dictionary<IType, IType>();
		}

		public void Replace(IType source, IType replacement)
		{
			_map[source] = replacement;
		}

		public override IType MapType(IType sourceType)
		{
			if (TypeMap.ContainsKey(sourceType))
			{
				return TypeMap[sourceType];
			}
			return base.MapType(sourceType);
		}
	}
}
