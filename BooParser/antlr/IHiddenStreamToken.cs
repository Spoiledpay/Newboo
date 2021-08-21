namespace antlr
{
	public interface IHiddenStreamToken : IToken
	{
		IHiddenStreamToken getHiddenAfter();

		void setHiddenAfter(IHiddenStreamToken t);

		IHiddenStreamToken getHiddenBefore();

		void setHiddenBefore(IHiddenStreamToken t);
	}
}
