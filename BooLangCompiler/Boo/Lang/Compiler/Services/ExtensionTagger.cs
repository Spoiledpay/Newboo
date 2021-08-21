using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Services
{
	public class ExtensionTagger : AbstractCompilerComponent
	{
		private IConstructor _extensionAttributeConstructor;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			CacheExtensionAttributeConstructor();
			TagAssembly();
		}

		public void TagAsExtension(TypeMember member)
		{
			AddAttributeTo(member.Attributes);
			TagDeclaringTypeOf(member);
		}

		private void TagDeclaringTypeOf(TypeMember member)
		{
			TypeDefinition declaringType = member.DeclaringType;
			if (declaringType != null)
			{
				TagAsExtension(declaringType);
			}
		}

		private void TagAssembly()
		{
			AddAttributeTo(base.CompileUnit.Modules.First.AssemblyAttributes);
		}

		private void AddAttributeTo(AttributeCollection attributes)
		{
			if (!attributes.ContainsEntity(_extensionAttributeConstructor))
			{
				attributes.Add(ExtensionAttributeInstance());
			}
		}

		private Attribute ExtensionAttributeInstance()
		{
			return base.CodeBuilder.CreateAttribute(_extensionAttributeConstructor);
		}

		private void CacheExtensionAttributeConstructor()
		{
			_extensionAttributeConstructor = base.TypeSystemServices.Map(Types.ClrExtensionAttribute).GetConstructors().Single();
		}
	}
}
