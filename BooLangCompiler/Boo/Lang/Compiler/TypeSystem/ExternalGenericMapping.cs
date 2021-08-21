using System.Reflection;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalGenericMapping : GenericMapping
	{
		private ExternalType _constructedType;

		public ExternalGenericMapping(ExternalType constructedType, IType[] arguments)
			: base(constructedType, arguments)
		{
			_constructedType = constructedType;
		}

		protected override IMember CreateMappedMember(IMember source)
		{
			ExternalType constructedType = _constructedType;
			return FindByMetadataToken(source, _constructedType);
		}

		public override IMember UnMap(IMember mapped)
		{
			IMember member = base.UnMap(mapped);
			if (member != null)
			{
				return member;
			}
			return FindByMetadataToken(mapped, _constructedType.ConstructedInfo.GenericDefinition as ExternalType);
		}

		private IMember FindByMetadataToken(IMember source, ExternalType targetType)
		{
			MemberInfo memberInfo = ((IExternalEntity)source).MemberInfo;
			MemberFilter filter = (MemberInfo candidate, object metadataToken) => candidate.MetadataToken.Equals(metadataToken);
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic;
			bindingFlags |= (source.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
			MemberInfo[] array = targetType.ActualType.FindMembers(memberInfo.MemberType, bindingFlags, filter, memberInfo.MetadataToken);
			return (IMember)base.TypeSystemServices.Map(array[0]);
		}
	}
}
