using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractNamespaceSensitiveVisitorCompilerStep : AbstractVisitorCompilerStep
	{
		protected INamespace CurrentNamespace => base.NameResolutionService.CurrentNamespace;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			base.NameResolutionService.Reset();
		}

		protected void EnterNamespace(INamespace ns)
		{
			base.NameResolutionService.EnterNamespace(ns);
		}

		protected void LeaveNamespace()
		{
			base.NameResolutionService.LeaveNamespace();
		}

		public override void OnModule(Module module)
		{
			EnterNamespace(InternalModule.ScopeFor(module));
			VisitTypeDefinitionBody(module);
			Visit(module.AssemblyAttributes);
			LeaveNamespace();
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			EnterNamespace((INamespace)GetEntity(node));
			VisitTypeDefinitionBody(node);
			LeaveNamespace();
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			EnterNamespace((INamespace)GetEntity(node));
			VisitTypeDefinitionBody(node);
			LeaveNamespace();
		}

		public override void OnStructDefinition(StructDefinition node)
		{
			EnterNamespace((INamespace)GetEntity(node));
			VisitTypeDefinitionBody(node);
			LeaveNamespace();
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			EnterNamespace((INamespace)GetEntity(node));
			VisitTypeDefinitionBody(node);
			LeaveNamespace();
		}

		private void VisitTypeDefinitionBody(TypeDefinition node)
		{
			Visit(node.Attributes);
			Visit(node.GenericParameters);
			Visit(node.Members);
			LeaveTypeDefinition(node);
		}

		public virtual void LeaveTypeDefinition(TypeDefinition node)
		{
		}
	}
}
