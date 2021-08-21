namespace Boo.Lang.Compiler.Services
{
	public class LanguageAmbiance
	{
		public virtual string SelfKeyword => "self";

		public virtual string IsaKeyword => "isa";

		public virtual string IsKeyword => "is";

		public virtual string TryKeyword => "try";

		public virtual string ExceptKeyword => "except";

		public virtual string FailureKeyword => "failure";

		public virtual string EnsureKeyword => "ensure";

		public virtual string RaiseKeyword => "raise";

		public virtual string CallableKeyword => "callable";

		public virtual string DefaultGeneratorTypeFor(string typeName)
		{
			return typeName + "*";
		}
	}
}
