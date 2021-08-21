using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class BindMethods : AbstractFastVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		private InternalTypeSystemProvider _internalTypeSystemProvider;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_internalTypeSystemProvider = My<InternalTypeSystemProvider>.Instance;
		}

		public override void OnMethod(Method node)
		{
			EnsureEntityFor(node);
			Visit(node.ExplicitInfo);
		}

		protected void EnsureEntityFor(TypeMember node)
		{
			_internalTypeSystemProvider.EntityFor(node);
		}

		public override void OnConstructor(Constructor node)
		{
			EnsureEntityFor(node);
		}

		public override void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			Visit(node.InterfaceType);
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			Visit(node.Members);
		}

		public override void OnModule(Module node)
		{
			Visit(node.Members);
		}

		public virtual TypeMember Reify(TypeMember member)
		{
			member.Accept(this);
			return member;
		}
	}
}
