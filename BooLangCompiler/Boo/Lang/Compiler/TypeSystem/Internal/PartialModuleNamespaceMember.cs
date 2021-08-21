namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	internal class PartialModuleNamespaceMember : PartialModuleNamespace
	{
		private readonly PartialModuleNamespace _parent;

		private string _simpleName;

		public override string Name => _simpleName;

		public override INamespace ParentNamespace => _parent;

		public PartialModuleNamespaceMember(PartialModuleNamespace parent, string fullyQualifiedName, string simpleName, InternalModule module)
			: base(fullyQualifiedName, module)
		{
			_parent = parent;
			_simpleName = simpleName;
		}
	}
}
