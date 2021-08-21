using System;
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public sealed class CheckMemberNames : AbstractVisitorCompilerStep
	{
		private Dictionary<string, List<TypeMember>> _members = new Dictionary<string, List<TypeMember>>(StringComparer.Ordinal);

		public override void Dispose()
		{
			_members.Clear();
			base.Dispose();
		}

		public override void LeaveClassDefinition(ClassDefinition node)
		{
			CheckMembers(node);
		}

		public override void LeaveInterfaceDefinition(InterfaceDefinition node)
		{
			CheckMembers(node);
		}

		private void CheckMembers(TypeDefinition node)
		{
			_members.Clear();
			foreach (TypeMember member in node.Members)
			{
				if (member.NodeType != NodeType.StatementTypeMember)
				{
					List<TypeMember> memberList = GetMemberList(member.Name);
					CheckMember(memberList, member);
					memberList.Add(member);
				}
			}
		}

		public override void LeaveEnumDefinition(EnumDefinition node)
		{
			_members.Clear();
			foreach (TypeMember member in node.Members)
			{
				if (_members.ContainsKey(member.Name))
				{
					MemberNameConflict(member);
					continue;
				}
				_members[member.Name] = new List<TypeMember> { member };
			}
		}

		private void CheckMember(List<TypeMember> list, TypeMember member)
		{
			switch (member.NodeType)
			{
			case NodeType.StatementTypeMember:
				break;
			case NodeType.Method:
			case NodeType.Constructor:
				CheckOverloadableMember(list, member);
				CheckLikelyTypoInTypeMemberName(member);
				break;
			case NodeType.Property:
				CheckOverloadableMember(list, member);
				break;
			default:
				CheckNonOverloadableMember(list, member);
				break;
			}
		}

		private void CheckNonOverloadableMember(List<TypeMember> existing, TypeMember member)
		{
			if (existing.Count > 0)
			{
				MemberNameConflict(member);
			}
		}

		private void CheckOverloadableMember(List<TypeMember> existing, TypeMember member)
		{
			NodeType nodeType = member.NodeType;
			foreach (TypeMember item in existing)
			{
				if (nodeType != item.NodeType)
				{
					MemberNameConflict(member);
				}
				else if ((nodeType != NodeType.Constructor || item.IsStatic == member.IsStatic) && IsConflictingOverload(member, item))
				{
					MemberConflict(member, TypeSystemServices.GetSignature((IEntityWithParameters)member.Entity));
				}
			}
		}

		private bool IsConflictingOverload(TypeMember member, TypeMember existingMember)
		{
			return AreParametersTheSame(existingMember, member) && !AreDifferentInterfaceMembers((IExplicitMember)existingMember, (IExplicitMember)member) && !AreDifferentConversionOperators(existingMember, member) && IsGenericityTheSame(existingMember, member);
		}

		private bool AreParametersTheSame(TypeMember lhs, TypeMember rhs)
		{
			IParameter[] parameters = GetParameters(lhs.Entity);
			IParameter[] parameters2 = GetParameters(rhs.Entity);
			return CallableSignature.AreSameParameters(parameters, parameters2);
		}

		private static IParameter[] GetParameters(IEntity entity)
		{
			return ((IEntityWithParameters)entity).GetParameters();
		}

		private bool IsGenericityTheSame(TypeMember lhs, TypeMember rhs)
		{
			IGenericParameter[] genericParameters = GenericsServices.GetGenericParameters(lhs.Entity);
			IGenericParameter[] genericParameters2 = GenericsServices.GetGenericParameters(rhs.Entity);
			return genericParameters == genericParameters2 || (genericParameters != null && genericParameters2 != null && genericParameters.Length == genericParameters2.Length);
		}

		private bool AreDifferentInterfaceMembers(IExplicitMember lhs, IExplicitMember rhs)
		{
			if (lhs.ExplicitInfo == null && rhs.ExplicitInfo == null)
			{
				return false;
			}
			return lhs.ExplicitInfo == null || rhs.ExplicitInfo == null || lhs.ExplicitInfo.InterfaceType.Entity != rhs.ExplicitInfo.InterfaceType.Entity;
		}

		private bool AreDifferentConversionOperators(TypeMember existing, TypeMember actual)
		{
			if ((existing.Name == "op_Implicit" || existing.Name == "op_Explicit") && existing.Name == actual.Name && existing.NodeType == NodeType.Method && existing.IsStatic && actual.IsStatic)
			{
				IMethod method = existing.Entity as IMethod;
				IMethod method2 = actual.Entity as IMethod;
				return method != null && method2 != null && method.ReturnType != method2.ReturnType;
			}
			return false;
		}

		private void MemberNameConflict(TypeMember member)
		{
			MemberConflict(member, member.Name);
		}

		private void MemberConflict(TypeMember member, string memberName)
		{
			Error(CompilerErrorFactory.MemberNameConflict(member, GetType(member.DeclaringType), memberName));
		}

		private List<TypeMember> GetMemberList(string name)
		{
			if (_members.TryGetValue(name, out var value))
			{
				return value;
			}
			value = new List<TypeMember>();
			_members[name] = value;
			return value;
		}

		private void CheckLikelyTypoInTypeMemberName(TypeMember member)
		{
			foreach (string likelyTypoName in GetLikelyTypoNames(member))
			{
				if (likelyTypoName == member.Name)
				{
					break;
				}
				if (Math.Abs(likelyTypoName.Length - member.Name.Length) > 1 || 1 != StringUtilities.GetDistance(likelyTypoName, member.Name))
				{
					continue;
				}
				base.Warnings.Add(CompilerWarningFactory.LikelyTypoInTypeMemberName(member, likelyTypoName));
				break;
			}
		}

		private IEnumerable<string> GetLikelyTypoNames(TypeMember member)
		{
			char first = member.Name[0];
			if (first == 'c' || first == 'C')
			{
				yield return "constructor";
			}
			else if (first == 'd' || first == 'D')
			{
				yield return "destructor";
			}
			if (member.IsStatic && member.Name.StartsWith("op_"))
			{
				yield return "op_Implicit";
				yield return "op_Addition";
				yield return "op_Subtraction";
				yield return "op_Multiply";
				yield return "op_Division";
				yield return "op_Modulus";
				yield return "op_Exponentiation";
				yield return "op_Equality";
				yield return "op_LessThan";
				yield return "op_LessThanOrEqual";
				yield return "op_GreaterThan";
				yield return "op_GreaterThanOrEqual";
				yield return "op_Match";
				yield return "op_NotMatch";
				yield return "op_Member";
				yield return "op_NotMember";
				yield return "op_BitwiseOr";
				yield return "op_BitwiseAnd";
				yield return "op_UnaryNegation";
			}
		}
	}
}
