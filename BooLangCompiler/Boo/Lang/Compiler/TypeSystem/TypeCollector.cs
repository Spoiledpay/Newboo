using System;
using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class TypeCollector : TypeVisitor
	{
		private Predicate<IType> _predicate;

		private List<IType> _matches = new List<IType>();

		public IEnumerable<IType> Matches => _matches;

		public TypeCollector(Predicate<IType> predicate)
		{
			_predicate = predicate;
		}

		public override void Visit(IType type)
		{
			if (_predicate(type))
			{
				_matches.AddUnique(type);
			}
			base.Visit(type);
		}
	}
}
