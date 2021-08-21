using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class BindTypeMembers : BindMethods
	{
		private const string PrivateMemberNeverUsed = "PrivateMemberNeverUsed";

		private List _parameters = new List();

		private List _events = new List();

		private IMethod _delegate_Combine;

		private IMethod _delegate_Remove;

		private IMethod Delegate_Combine
		{
			get
			{
				InitializeDelegateMethods();
				return _delegate_Combine;
			}
		}

		private IMethod Delegate_Remove
		{
			get
			{
				InitializeDelegateMethods();
				return _delegate_Remove;
			}
		}

		public override void OnMethod(Method node)
		{
			_parameters.Add(node);
			base.OnMethod(node);
		}

		private void BindAllParameters()
		{
			Method entryPoint = ContextAnnotations.GetEntryPoint(base.Context);
			foreach (INodeWithParameters parameter in _parameters)
			{
				TypeMember typeMember = (TypeMember)parameter;
				if (typeMember.ContainsAnnotation("PrivateMemberNeverUsed"))
				{
					continue;
				}
				base.NameResolutionService.EnterNamespace((INamespace)TypeSystemServices.GetEntity(typeMember.DeclaringType));
				base.CodeBuilder.BindParameterDeclarations(typeMember.IsStatic, parameter);
				if (!typeMember.IsVisible && !typeMember.IsSynthetic)
				{
					IExplicitMember explicitMember = typeMember as IExplicitMember;
					if ((explicitMember == null || null == explicitMember.ExplicitInfo) && typeMember != entryPoint)
					{
						typeMember.Annotate("PrivateMemberNeverUsed", null);
					}
				}
			}
		}

		public override void OnConstructor(Constructor node)
		{
			_parameters.Add(node);
			base.OnConstructor(node);
		}

		public override void OnField(Field node)
		{
			if (null == node.Entity)
			{
				node.Entity = new InternalField(node);
				if (!node.IsVisible && !node.IsSynthetic)
				{
					node.Annotate("PrivateMemberNeverUsed", null);
				}
			}
		}

		public override void OnProperty(Property node)
		{
			EnsureEntityFor(node);
			_parameters.Add(node);
			Visit(node.Getter);
			Visit(node.Setter);
			Visit(node.ExplicitInfo);
		}

		public override void OnEvent(Event node)
		{
			_events.Add(node);
		}

		private void BindAllEvents()
		{
			foreach (Event @event in _events)
			{
				BindEvent(@event);
			}
		}

		private void BindEvent(Event node)
		{
			EnsureEntityFor(node);
			IType type = GetType(node.Type);
			IType type2 = GetType(node.DeclaringType);
			bool flag = type is ICallableType;
			if (!flag)
			{
				base.Errors.Add(CompilerErrorFactory.EventTypeIsNotCallable(node.Type, type));
			}
			if (type2.IsInterface)
			{
				BindInterfaceEvent(node);
			}
			else
			{
				BindClassEvent(node, type, flag);
			}
		}

		private void BindInterfaceEvent(Event node)
		{
			if (null == node.Add)
			{
				node.Add = CreateInterfaceEventAddMethod(node);
			}
			if (null == node.Remove)
			{
				node.Remove = CreateInterfaceEventRemoveMethod(node);
			}
		}

		private void BindClassEvent(Event node, IType type, bool typeIsCallable)
		{
			Field field = base.CodeBuilder.CreateField("$event$" + node.Name, type);
			field.IsSynthetic = true;
			field.Modifiers = TypeMemberModifiers.Private;
			if (node.HasTransientModifier)
			{
				field.Modifiers |= TypeMemberModifiers.Transient;
			}
			if (node.IsStatic)
			{
				field.Modifiers |= TypeMemberModifiers.Static;
			}
			node.DeclaringType.Members.Add(field);
			((InternalEvent)node.Entity).BackingField = (InternalField)field.Entity;
			if (null == node.Add)
			{
				node.Add = CreateEventAddMethod(node, field);
			}
			else
			{
				Visit(node.Add);
			}
			if (null == node.Remove)
			{
				node.Remove = CreateEventRemoveMethod(node, field);
			}
			else
			{
				Visit(node.Remove);
			}
			if (null == node.Raise)
			{
				if (typeIsCallable)
				{
					node.Raise = CreateEventRaiseMethod(node, field);
				}
			}
			else
			{
				Visit(node.Raise);
			}
		}

		public override void Run()
		{
			base.Run();
			BindAll();
		}

		public override TypeMember Reify(TypeMember node)
		{
			base.Reify(node);
			BindAll();
			return node;
		}

		private void BindAll()
		{
			BindAllParameters();
			BindAllEvents();
			Reset();
		}

		private void Reset()
		{
			_parameters.Clear();
			_events.Clear();
		}

		private void InitializeDelegateMethods()
		{
			if (null == _delegate_Combine)
			{
				_delegate_Combine = base.TypeSystemServices.Map(Methods.Of<Delegate, Delegate, Delegate>(Delegate.Combine));
				_delegate_Remove = base.TypeSystemServices.Map(Methods.Of<Delegate, Delegate, Delegate>(Delegate.Remove));
			}
		}

		private Method CreateInterfaceEventMethod(Event node, string prefix)
		{
			Method method = base.CodeBuilder.CreateMethod(prefix + node.Name, base.TypeSystemServices.VoidType, TypeMemberModifiers.Public | TypeMemberModifiers.Virtual | TypeMemberModifiers.Abstract);
			method.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(base.CodeBuilder.GetFirstParameterIndex(node), "handler", GetType(node.Type)));
			return method;
		}

		private Method CreateInterfaceEventAddMethod(Event node)
		{
			return CreateInterfaceEventMethod(node, "add_");
		}

		private Method CreateInterfaceEventRemoveMethod(Event node)
		{
			return CreateInterfaceEventMethod(node, "remove_");
		}

		private Method CreateEventMethod(Event node, string prefix)
		{
			Method method = base.CodeBuilder.CreateMethod(prefix + node.Name, base.TypeSystemServices.VoidType, node.Modifiers);
			method.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(base.CodeBuilder.GetFirstParameterIndex(node), "handler", GetType(node.Type)));
			return method;
		}

		private Method CreateEventAddMethod(Event node, Field backingField)
		{
			Method method = CreateEventMethod(node, "add_");
			method.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(backingField), base.CodeBuilder.CreateMethodInvocation(Delegate_Combine, base.CodeBuilder.CreateReference(backingField), base.CodeBuilder.CreateReference(method.Parameters[0]))));
			return method;
		}

		private Method CreateEventRemoveMethod(Event node, Field backingField)
		{
			Method method = CreateEventMethod(node, "remove_");
			method.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(backingField), base.CodeBuilder.CreateMethodInvocation(Delegate_Remove, base.CodeBuilder.CreateReference(backingField), base.CodeBuilder.CreateReference(method.Parameters[0]))));
			return method;
		}

		private TypeMemberModifiers RemoveAccessiblityModifiers(TypeMemberModifiers modifiers)
		{
			TypeMemberModifiers typeMemberModifiers = TypeMemberModifiers.VisibilityMask;
			return modifiers & ~typeMemberModifiers;
		}

		private Method CreateEventRaiseMethod(Event node, Field backingField)
		{
			TypeMemberModifiers typeMemberModifiers = RemoveAccessiblityModifiers(node.Modifiers);
			typeMemberModifiers = ((!node.IsPrivate) ? (typeMemberModifiers | (TypeMemberModifiers.Internal | TypeMemberModifiers.Protected)) : (typeMemberModifiers | TypeMemberModifiers.Private));
			Method method = base.CodeBuilder.CreateMethod("raise_" + node.Name, base.TypeSystemServices.VoidType, typeMemberModifiers);
			ICallableType callableType = GetEntity(node.Type) as ICallableType;
			if (null != callableType)
			{
				int num = base.CodeBuilder.GetFirstParameterIndex(node);
				IParameter[] parameters = callableType.GetSignature().Parameters;
				foreach (IParameter parameter in parameters)
				{
					method.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(num, parameter.Name, parameter.Type, parameter.IsByRef));
					num++;
				}
			}
			InternalLocal local = base.CodeBuilder.DeclareTempLocal(method, GetType(backingField.Type));
			BinaryExpression expression = new BinaryExpression(BinaryOperatorType.Assign, base.CodeBuilder.CreateReference(local), base.CodeBuilder.CreateReference(backingField));
			method.Body.Add(expression);
			MethodInvocationExpression methodInvocationExpression = base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateReference(local), base.NameResolutionService.ResolveMethod(GetType(backingField.Type), "Invoke"));
			foreach (ParameterDeclaration parameter2 in method.Parameters)
			{
				methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(parameter2));
			}
			IfStatement ifStatement = new IfStatement(node.LexicalInfo);
			ifStatement.Condition = base.CodeBuilder.CreateReference(local);
			ifStatement.TrueBlock = new Block();
			ifStatement.TrueBlock.Add(methodInvocationExpression);
			method.Body.Add(ifStatement);
			return method;
		}
	}
}
