using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps
{
	[Serializable]
	public class BindEnumMembers : AbstractTransformerCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		public override void Run()
		{
			Visit(base.CompileUnit.Modules);
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			long num = 0L;
			foreach (EnumMember member in node.Members)
			{
				if (null == member.Initializer)
				{
					member.Initializer = new IntegerLiteralExpression(num);
				}
				else if (member.Initializer.NodeType == NodeType.IntegerLiteralExpression)
				{
					num = ((IntegerLiteralExpression)member.Initializer).Value;
				}
				num++;
			}
		}

		public TypeMember Reify(TypeMember node)
		{
			Visit(node);
			return node;
		}
	}
}
