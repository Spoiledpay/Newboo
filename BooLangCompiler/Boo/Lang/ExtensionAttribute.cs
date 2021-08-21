using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Services;
using Boo.Lang.Environments;

namespace Boo.Lang
{
	public class ExtensionAttribute : AbstractAstAttribute
	{
		public override void Apply(Node targetNode)
		{
			TypeMember typeMember = targetNode as TypeMember;
			if (typeMember == null)
			{
				base.Errors.Add(CompilerErrorFactory.InvalidExtensionDefinition(targetNode));
			}
			else
			{
				My<ExtensionTagger>.Instance.TagAsExtension(typeMember);
			}
		}
	}
}
