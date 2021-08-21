using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class MethodTrackingVisitorCompilerStep : AbstractFastVisitorCompilerStep
	{
		private Method _currentMethod;

		public Method CurrentMethod => _currentMethod;

		public override void OnMethod(Method node)
		{
			_currentMethod = node;
			base.OnMethod(node);
		}

		public override void OnConstructor(Constructor node)
		{
			_currentMethod = node;
			base.OnConstructor(node);
		}
	}
}
