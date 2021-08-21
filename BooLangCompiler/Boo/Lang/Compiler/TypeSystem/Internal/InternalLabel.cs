using System;
using System.Reflection.Emit;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalLabel : IEntity
	{
		private LabelStatement _labelStatement;

		public LabelStatement LabelStatement => _labelStatement;

		public EntityType EntityType => EntityType.Label;

		public string Name => _labelStatement.Name;

		public string FullName => _labelStatement.Name;

		public Label Label { get; set; }

		public InternalLabel(LabelStatement labelStatement)
		{
			if (null == labelStatement)
			{
				throw new ArgumentNullException("labelStatement");
			}
			_labelStatement = labelStatement;
			_labelStatement.Entity = this;
		}
	}
}
