using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	internal class GenericParametersNamespaceExtender : AbstractNamespace
	{
		private IType _type;

		private INamespace _parent;

		public override INamespace ParentNamespace => _parent;

		public GenericParametersNamespaceExtender(IType type, INamespace currentNamespace)
		{
			_type = type;
			_parent = currentNamespace;
		}

		public override IEnumerable<IEntity> GetMembers()
		{
			if (_type.GenericInfo != null)
			{
				return _type.GenericInfo.GenericParameters;
			}
			return NullNamespace.EmptyEntityArray;
		}
	}
}
