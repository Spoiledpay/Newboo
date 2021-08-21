using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class PreErrorChecking : AbstractVisitorCompilerStep
	{
		public override void LeaveField(Field node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedAbstract(node);
			CantBeMarkedPartial(node);
		}

		public override void LeaveProperty(Property node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedTransient(node);
			CantBeMarkedPartial(node);
			CheckExplicitImpl(node);
			CheckModifierCombination(node);
		}

		public override void LeaveConstructor(Constructor node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedTransient(node);
			CantBeMarkedPartial(node);
			CantBeMarkedFinal(node);
			CannotReturnValue(node);
			ConstructorCannotBePolymorphic(node);
		}

		public override void LeaveMethod(Method node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedTransient(node);
			CantBeMarkedPartial(node);
			CheckExplicitImpl(node);
			CheckModifierCombination(node);
		}

		public override void LeaveEvent(Event node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedPartial(node);
			CheckModifierCombination(node);
		}

		public override void LeaveInterfaceDefinition(InterfaceDefinition node)
		{
			CantBeMarkedAbstract(node);
			CantBeMarkedTransient(node);
			CantBeMarkedPartialIfNested(node);
			CantBeMarkedFinal(node);
			CantBeMarkedStatic(node);
		}

		public override void LeaveCallableDefinition(CallableDefinition node)
		{
			MakeStaticIfNeeded(node);
			CantBeMarkedAbstract(node);
			CantBeMarkedTransient(node);
			CantBeMarkedPartial(node);
		}

		public override void LeaveStructDefinition(StructDefinition node)
		{
			CantBeMarkedAbstract(node);
			CantBeMarkedFinal(node);
			CantBeMarkedStatic(node);
			CantBeMarkedPartial(node);
		}

		public override void LeaveEnumDefinition(EnumDefinition node)
		{
			CantBeMarkedAbstract(node);
			CantBeMarkedPartialIfNested(node);
			CantBeMarkedFinal(node);
			CantBeMarkedStatic(node);
		}

		public override void LeaveClassDefinition(ClassDefinition node)
		{
			CheckModifierCombination(node);
			CantBeMarkedPartialIfNested(node);
			if (node.IsStatic)
			{
				node.Modifiers |= TypeMemberModifiers.Final | TypeMemberModifiers.Abstract;
			}
		}

		public override void LeaveTryStatement(TryStatement node)
		{
			if (node.EnsureBlock == null && node.FailureBlock == null && node.ExceptionHandlers.Count == 0)
			{
				Error(CompilerErrorFactory.InvalidTryStatement(node));
			}
		}

		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			if (BinaryOperatorType.Assign == node.Operator && node.Right.NodeType != NodeType.TryCastExpression && IsTopLevelOfConditional(node))
			{
				base.Warnings.Add(CompilerWarningFactory.EqualsInsteadOfAssign(node));
			}
		}

		private static bool IsTopLevelOfConditional(Node child)
		{
			Node parentNode = child.ParentNode;
			return parentNode.NodeType == NodeType.IfStatement || parentNode.NodeType == NodeType.UnlessStatement || (parentNode.NodeType == NodeType.ConditionalExpression && ((ConditionalExpression)parentNode).Condition == child) || parentNode.NodeType == NodeType.StatementModifier || parentNode.NodeType == NodeType.ReturnStatement || parentNode.NodeType == NodeType.YieldStatement;
		}

		public override void LeaveDestructor(Destructor node)
		{
			if (node.Modifiers != 0)
			{
				Error(CompilerErrorFactory.InvalidDestructorModifier(node));
			}
			if (node.Parameters.Count != 0)
			{
				Error(CompilerErrorFactory.CantHaveDestructorParameters(node));
			}
			CannotReturnValue(node);
		}

		private void ConstructorCannotBePolymorphic(Constructor node)
		{
			if (node.IsAbstract || node.IsOverride || node.IsVirtual)
			{
				Error(CompilerErrorFactory.ConstructorCantBePolymorphic(node, EntityFor(node)));
			}
		}

		private void CannotReturnValue(Method node)
		{
			if (node.ReturnType != null)
			{
				Error(CompilerErrorFactory.CannotReturnValue(node));
			}
		}

		private void CantBeMarkedAbstract(TypeMember member)
		{
			if (member.IsAbstract)
			{
				Error(CompilerErrorFactory.CantBeMarkedAbstract(member));
			}
		}

		private void CantBeMarkedFinal(TypeMember member)
		{
			if (member.IsFinal)
			{
				Error(CompilerErrorFactory.CantBeMarkedFinal(member));
			}
		}

		private void CantBeMarkedTransient(TypeMember member)
		{
			if (member.HasTransientModifier)
			{
				Error(CompilerErrorFactory.CantBeMarkedTransient(member));
			}
		}

		private void MakeStaticIfNeeded(TypeMember node)
		{
			TypeDefinition declaringType = node.DeclaringType;
			if (declaringType != null && declaringType.IsStatic)
			{
				if (node.IsStatic)
				{
					base.Warnings.Add(CompilerWarningFactory.StaticClassMemberRedundantlyMarkedStatic(node, declaringType.Name, node.Name));
				}
				node.Modifiers |= TypeMemberModifiers.Static;
			}
		}

		private void CheckExplicitImpl(IExplicitMember member)
		{
			ExplicitMemberInfo explicitInfo = member.ExplicitInfo;
			if (null != explicitInfo)
			{
				TypeMember typeMember = (TypeMember)member;
				if (TypeMemberModifiers.None != typeMember.Modifiers)
				{
					Error(CompilerErrorFactory.ExplicitImplMustNotHaveModifiers(typeMember, explicitInfo.InterfaceType.Name, typeMember.Name));
				}
			}
		}

		private void CheckModifierCombination(TypeMember member)
		{
			InvalidCombination(member, TypeMemberModifiers.Static, TypeMemberModifiers.Abstract);
			InvalidCombination(member, TypeMemberModifiers.Static, TypeMemberModifiers.Virtual);
			InvalidCombination(member, TypeMemberModifiers.Static, TypeMemberModifiers.Override);
			InvalidCombination(member, TypeMemberModifiers.Abstract, TypeMemberModifiers.Final);
			if (member.NodeType != NodeType.Field)
			{
				InvalidCombination(member, TypeMemberModifiers.Static, TypeMemberModifiers.Final);
			}
		}

		private void InvalidCombination(TypeMember member, TypeMemberModifiers mod1, TypeMemberModifiers mod2)
		{
			if (member.IsModifierSet(mod1) && member.IsModifierSet(mod2))
			{
				Error(CompilerErrorFactory.InvalidCombinationOfModifiers(member, EntityFor(member), $"{mod1.ToString().ToLower()}, {mod2.ToString().ToLower()}"));
			}
		}

		private IEntity EntityFor(TypeMember member)
		{
			return My<InternalTypeSystemProvider>.Instance.EntityFor(member);
		}

		private IMethod EntityFor(Constructor node)
		{
			return (IMethod)EntityFor((TypeMember)node);
		}

		private void CantBeMarkedPartialIfNested(TypeDefinition type)
		{
			if (type.IsNested)
			{
				CantBeMarkedPartial(type);
			}
		}

		private void CantBeMarkedPartial(TypeMember member)
		{
			if (member.IsPartial)
			{
				Error(CompilerErrorFactory.CantBeMarkedPartial(member));
			}
		}

		private void CantBeMarkedStatic(TypeMember member)
		{
			if (member.IsStatic)
			{
				Error(CompilerErrorFactory.CantBeMarkedStatic(member));
			}
		}
	}
}
