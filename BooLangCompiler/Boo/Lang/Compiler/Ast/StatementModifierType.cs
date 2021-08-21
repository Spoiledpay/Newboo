using System;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public enum StatementModifierType
	{
		None,
		If,
		Unless,
		While
	}
}
