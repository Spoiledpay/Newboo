using System;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	internal class GenericMappedTypeParameter : AbstractGenericParameter
	{
		private IGenericParameter _source;

		private GenericMapping _mapping;

		public IGenericParameter Source => _source;

		public GenericMapping GenericMapping => _mapping;

		public override int GenericParameterPosition => Source.GenericParameterPosition;

		public override IEntity DeclaringEntity => _mapping.Map(Source.DeclaringEntity);

		public override string Name => Source.Name;

		public override bool MustHaveDefaultConstructor => Source.MustHaveDefaultConstructor;

		public override Variance Variance => Source.Variance;

		public override bool IsClass => Source.IsClass;

		public override bool IsValueType => Source.IsValueType;

		public GenericMappedTypeParameter(TypeSystemServices tss, IGenericParameter source, GenericMapping mapping)
			: base(tss)
		{
			_source = source;
			_mapping = mapping;
		}

		public override IType[] GetTypeConstraints()
		{
			return Array.ConvertAll(Source.GetTypeConstraints(), _mapping.MapType);
		}
	}
}
