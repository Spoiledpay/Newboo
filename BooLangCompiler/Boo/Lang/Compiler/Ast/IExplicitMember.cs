namespace Boo.Lang.Compiler.Ast
{
	public interface IExplicitMember
	{
		ExplicitMemberInfo ExplicitInfo { get; set; }

		string Name { get; set; }
	}
}
