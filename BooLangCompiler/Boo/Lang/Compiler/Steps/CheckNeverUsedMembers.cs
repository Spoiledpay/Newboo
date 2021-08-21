using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckNeverUsedMembers : AbstractTransformerCompilerStep
	{
		private bool? hasInternalsVisibleToAttribute;

		private bool HasInternalsVisibleToAttribute
		{
			get
			{
				if (!hasInternalsVisibleToAttribute.HasValue)
				{
					hasInternalsVisibleToAttribute = FindInternalsVisibleToAttribute();
				}
				return hasInternalsVisibleToAttribute.Value;
			}
		}

		public override void Run()
		{
			Visit(base.CompileUnit.Modules);
		}

		public override void LeaveModule(Module module)
		{
			if (module.ContainsAnnotation("merged-module"))
			{
				return;
			}
			string text = ((module.EnclosingNamespace != null) ? module.EnclosingNamespace.Name : string.Empty);
			foreach (Import import in module.Imports)
			{
				if (import.Entity != Error.Default && !ImportAnnotations.IsUsedImport(import) && !(import.Namespace == text) && !(import.Namespace == "System"))
				{
					base.Warnings.Add(CompilerWarningFactory.NamespaceNeverUsed(import));
				}
			}
		}

		public override bool EnterClassDefinition(ClassDefinition node)
		{
			CheckMembers(node);
			return false;
		}

		public override bool EnterInterfaceDefinition(InterfaceDefinition node)
		{
			return false;
		}

		public override bool EnterStructDefinition(StructDefinition node)
		{
			return false;
		}

		protected void CheckMembers(ClassDefinition node)
		{
			foreach (TypeMember member in node.Members)
			{
				WarnIfPrivateMemberNeverUsed(member);
				WarnIfProtectedMemberInSealedClass(member);
			}
		}

		protected void WarnIfPrivateMemberNeverUsed(TypeMember node)
		{
			if ((NodeType.Constructor != node.NodeType || !node.IsStatic) && !IsVisible(node) && node.ContainsAnnotation("PrivateMemberNeverUsed"))
			{
				base.Warnings.Add(CompilerWarningFactory.PrivateMemberNeverUsed(node));
			}
		}

		protected void WarnIfProtectedMemberInSealedClass(TypeMember member)
		{
			if (member.IsProtected && !member.IsSynthetic && !member.IsOverride && member.DeclaringType.IsFinal)
			{
				base.Warnings.Add(CompilerWarningFactory.NewProtectedMemberInSealedType(member));
			}
		}

		private bool IsVisible(TypeMember member)
		{
			return HasInternalsVisibleToAttribute ? (!member.IsPrivate) : (!member.IsPrivate && !member.IsInternal);
		}

		private bool FindInternalsVisibleToAttribute()
		{
			foreach (Module module in base.CompileUnit.Modules)
			{
				foreach (Attribute assemblyAttribute in module.AssemblyAttributes)
				{
					IType declaringType = ((IMethod)assemblyAttribute.Entity).DeclaringType;
					if (declaringType.FullName == "System.Runtime.CompilerServices.InternalsVisibleToAttribute")
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
