#define TRACE
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps
{
	public class ProcessInheritedAbstractMembers : AbstractVisitorCompilerStep, ITypeMemberReifier, INodeReifier<TypeMember>
	{
		private sealed class ExplicitMembersFirstComparer<T> : IComparer<T> where T : IExplicitMember
		{
			public int Compare(T lhs, T rhs)
			{
				if (lhs.ExplicitInfo != null && rhs.ExplicitInfo == null)
				{
					return -1;
				}
				if (lhs.ExplicitInfo == null && rhs.ExplicitInfo != null)
				{
					return 1;
				}
				return 0;
			}
		}

		private List<TypeDefinition> _newAbstractClasses;

		private int _depth;

		private Set<IEntity> _explicitMembers;

		public override void Run()
		{
			_newAbstractClasses = new List<TypeDefinition>();
			Visit(base.CompileUnit.Modules);
			ProcessNewAbstractClasses();
		}

		public override void Dispose()
		{
			_newAbstractClasses = null;
			base.Dispose();
		}

		public override void OnProperty(Property node)
		{
			if (node.IsAbstract && null == node.Type)
			{
				node.Type = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.ObjectType);
			}
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				Visit(explicitInfo);
				if (null != explicitInfo.Entity)
				{
					ProcessPropertyImplementation(node, (IProperty)explicitInfo.Entity);
				}
			}
		}

		public override void OnMethod(Method node)
		{
			if (node.IsAbstract && null == node.ReturnType)
			{
				node.ReturnType = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.VoidType);
			}
			ExplicitMemberInfo explicitInfo = node.ExplicitInfo;
			if (null != explicitInfo)
			{
				Visit(explicitInfo);
				if (null != explicitInfo.Entity)
				{
					ProcessMethodImplementation(node, (IMethod)explicitInfo.Entity);
				}
			}
		}

		public override void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			TypeMember typeMember = (TypeMember)node.ParentNode;
			if (CheckExplicitMemberValidity((IExplicitMember)typeMember))
			{
				IType entity = GetEntity(node.InterfaceType);
				IMember member = FindBaseMemberOf((IMember)typeMember.Entity, entity);
				if (null == member)
				{
					Error(CompilerErrorFactory.NotAMemberOfExplicitInterface(typeMember, entity));
					return;
				}
				TraceImplements(typeMember, member);
				node.Entity = member;
			}
		}

		private bool CheckExplicitMemberValidity(IExplicitMember member)
		{
			Node node = (Node)member;
			IMember member2 = (IMember)GetEntity(node);
			IType declaringType = member2.DeclaringType;
			if (!declaringType.IsClass)
			{
				Error(CompilerErrorFactory.InvalidTypeForExplicitMember(node, declaringType));
				return false;
			}
			IType type = GetType(member.ExplicitInfo.InterfaceType);
			if (!type.IsInterface)
			{
				Error(CompilerErrorFactory.InvalidInterfaceForInterfaceMember(node, member.ExplicitInfo.InterfaceType.Name));
				return false;
			}
			if (!declaringType.IsSubclassOf(type))
			{
				Error(CompilerErrorFactory.InterfaceImplForInvalidInterface(node, type.Name, ((TypeMember)node).Name));
				return false;
			}
			return true;
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			if (!WasVisited(node))
			{
				MarkVisited(node);
				base.OnInterfaceDefinition(node);
			}
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			if (!WasVisited(node))
			{
				MarkVisited(node);
				base.OnClassDefinition(node);
				ProcessBaseTypes(node, GetType(node), null);
			}
		}

		private void ProcessBaseTypes(ClassDefinition originalNode, IType currentType, TypeReference rootBaseType)
		{
			if (rootBaseType == null)
			{
				_explicitMembers = null;
				foreach (TypeReference baseType in originalNode.BaseTypes)
				{
					IType type = GetType(baseType);
					EnsureRelatedNodeWasVisited(originalNode, type);
					if (type.IsInterface)
					{
						if (_explicitMembers == null)
						{
							_explicitMembers = ExplicitlyImplementedMembersOn(originalNode);
						}
						ResolveInterfaceMembers(originalNode, type, baseType);
					}
					else if (!IsAbstract(GetType(originalNode)) && IsAbstract(type))
					{
						ResolveAbstractMembers(originalNode, type, baseType);
					}
				}
			}
			else if (currentType.BaseType != null && IsAbstract(currentType.BaseType))
			{
				ResolveAbstractMembers(originalNode, currentType.BaseType, rootBaseType);
			}
		}

		private Set<IEntity> ExplicitlyImplementedMembersOn(ClassDefinition definition)
		{
			Set<IEntity> set = new Set<IEntity>();
			foreach (TypeMember member in definition.Members)
			{
				IExplicitMember explicitMember = member as IExplicitMember;
				if (null != explicitMember)
				{
					ExplicitMemberInfo explicitInfo = explicitMember.ExplicitInfo;
					if (explicitInfo != null && null != explicitInfo.Entity)
					{
						set.Add(explicitInfo.Entity);
					}
				}
			}
			return set;
		}

		private bool CheckInheritsImplementation(ClassDefinition node, IMember abstractMember)
		{
			foreach (TypeReference baseType in node.BaseTypes)
			{
				IType type = GetType(baseType);
				if (!type.IsInterface)
				{
					IMember member = FindBaseMemberOf(abstractMember, type);
					if (member != null && !IsAbstract(member))
					{
						return true;
					}
				}
			}
			return false;
		}

		private IMember FindBaseMemberOf(IMember member, IType inType)
		{
			if (member.DeclaringType == inType)
			{
				return null;
			}
			foreach (IMember item in ImplementationCandidatesFor(member, inType))
			{
				if (item == member || !IsValidImplementationFor(member, item))
				{
					continue;
				}
				return item;
			}
			return null;
		}

		private bool IsValidImplementationFor(IEntity member, IEntity candidate)
		{
			switch (member.EntityType)
			{
			case EntityType.Method:
				if (CheckInheritedMethodImpl(candidate as IMethod, member as IMethod))
				{
					return true;
				}
				break;
			case EntityType.Event:
				if (CheckInheritedEventImpl(candidate as IEvent, member as IEvent))
				{
					return true;
				}
				break;
			case EntityType.Property:
				if (CheckInheritedPropertyImpl(candidate as IProperty, member as IProperty))
				{
					return true;
				}
				break;
			}
			return false;
		}

		private IEnumerable<IMember> ImplementationCandidatesFor(IMember abstractMember, IType inBaseType)
		{
			while (inBaseType != null)
			{
				foreach (IEntity candidate in inBaseType.GetMembers())
				{
					if (candidate.EntityType == abstractMember.EntityType && candidate.EntityType != EntityType.Field)
					{
						string candidateName = (abstractMember.DeclaringType.IsInterface ? SimpleNameOf(candidate) : candidate.Name);
						if (candidateName == abstractMember.Name)
						{
							yield return (IMember)candidate;
						}
					}
				}
				inBaseType = inBaseType.BaseType;
			}
		}

		private string SimpleNameOf(IEntity candidate)
		{
			string[] array = candidate.FullName.Split('.');
			return array[array.Length - 1];
		}

		private bool CheckInheritedMethodImpl(IMethod impl, IMethod baseMethod)
		{
			return TypeSystemServices.CheckOverrideSignature(impl, baseMethod);
		}

		private bool CheckInheritedEventImpl(IEvent impl, IEvent target)
		{
			return impl.Type == target.Type;
		}

		private bool CheckInheritedPropertyImpl(IProperty impl, IProperty target)
		{
			if (!TypeSystemServices.CheckOverrideSignature(impl.GetParameters(), target.GetParameters()))
			{
				return false;
			}
			if (HasGetter(target) && !HasGetter(impl))
			{
				return false;
			}
			if (HasSetter(target) && !HasSetter(impl))
			{
				return false;
			}
			return true;
		}

		private static bool HasGetter(IProperty property)
		{
			return property.GetGetMethod() != null;
		}

		private static bool HasSetter(IProperty property)
		{
			return property.GetSetMethod() != null;
		}

		private bool IsAbstract(IType type)
		{
			if (type.IsAbstract)
			{
				return true;
			}
			AbstractInternalType abstractInternalType = type as AbstractInternalType;
			if (null != abstractInternalType)
			{
				return _newAbstractClasses.Contains(abstractInternalType.TypeDefinition);
			}
			return false;
		}

		private void ResolveAbstractProperty(ClassDefinition node, IProperty baseProperty, TypeReference rootBaseType)
		{
			foreach (Property abstractPropertyImplementationCandidate in GetAbstractPropertyImplementationCandidates(node, baseProperty))
			{
				if (!ResolveAsImplementationOf(baseProperty, abstractPropertyImplementationCandidate) || (HasGetter(baseProperty) && (!HasGetter(baseProperty) || null == abstractPropertyImplementationCandidate.Getter)) || (HasSetter(baseProperty) && (!HasSetter(baseProperty) || null == abstractPropertyImplementationCandidate.Setter)))
				{
					continue;
				}
				return;
			}
			if (!CheckInheritsImplementation(node, baseProperty))
			{
				AbstractMemberNotImplemented(node, rootBaseType, baseProperty);
			}
		}

		private ClassDefinition ClassDefinitionFor(TypeReference parent)
		{
			InternalClass internalClass = GetType(parent) as InternalClass;
			return (internalClass != null) ? ((ClassDefinition)internalClass.TypeDefinition) : null;
		}

		private bool ResolveAsImplementationOf(IProperty baseProperty, Property property)
		{
			if (!TypeSystemServices.CheckOverrideSignature(GetEntity(property).GetParameters(), baseProperty.GetParameters()))
			{
				return false;
			}
			ProcessPropertyImplementation(property, baseProperty);
			AssertValidPropertyImplementation(property, baseProperty);
			return true;
		}

		private void AssertValidPropertyImplementation(Property p, IProperty baseProperty)
		{
			if (baseProperty.Type != p.Type.Entity)
			{
				Error(CompilerErrorFactory.ConflictWithInheritedMember(p, GetEntity(p), baseProperty));
			}
			AssertValidInterfaceImplementation(p, baseProperty);
		}

		private void ProcessPropertyImplementation(Property p, IProperty baseProperty)
		{
			if (p.Type == null)
			{
				p.Type = base.CodeBuilder.CreateTypeReference(baseProperty.Type);
			}
			ProcessPropertyAccessor(p, p.Getter, baseProperty.GetGetMethod());
			ProcessPropertyAccessor(p, p.Setter, baseProperty.GetSetMethod());
		}

		private static void ProcessPropertyAccessor(Property p, Method accessor, IMethod method)
		{
			if (accessor != null)
			{
				accessor.Modifiers |= TypeMemberModifiers.Virtual;
				if (p.ExplicitInfo != null)
				{
					accessor.ExplicitInfo = p.ExplicitInfo.CloneNode();
					accessor.ExplicitInfo.Entity = method;
					accessor.Visibility = TypeMemberModifiers.Private;
				}
			}
		}

		private void ResolveAbstractEvent(ClassDefinition node, TypeReference baseTypeRef, IEvent baseEvent)
		{
			Event @event = node.Members[baseEvent.Name] as Event;
			if (@event != null)
			{
				ProcessEventImplementation(@event, baseEvent);
			}
			else if (!CheckInheritsImplementation(node, baseEvent))
			{
				TypeMember node2;
				if (null != (node2 = node.Members[baseEvent.Name]))
				{
					Error(CompilerErrorFactory.ConflictWithInheritedMember(node2, (IMember)GetEntity(node2), baseEvent));
					return;
				}
				AddStub(node, base.CodeBuilder.CreateAbstractEvent(baseTypeRef.LexicalInfo, baseEvent));
				AbstractMemberNotImplemented(node, baseTypeRef, baseEvent);
			}
		}

		private void ProcessEventImplementation(Event ev, IEvent baseEvent)
		{
			MakeVirtualFinal(ev.Add);
			MakeVirtualFinal(ev.Remove);
			MakeVirtualFinal(ev.Raise);
			AssertValidInterfaceImplementation(ev, baseEvent);
			base.Context.TraceInfo("{0}: Event {1} implements {2}", ev.LexicalInfo, ev, baseEvent);
		}

		private static void MakeVirtualFinal(Method method)
		{
			if (method != null)
			{
				method.Modifiers |= TypeMemberModifiers.Final | TypeMemberModifiers.Virtual;
			}
		}

		private void ResolveAbstractMethod(ClassDefinition node, IMethod baseAbstractMethod, TypeReference rootBaseType)
		{
			if (baseAbstractMethod.IsSpecialName)
			{
				return;
			}
			foreach (Method abstractMethodImplementationCandidate in GetAbstractMethodImplementationCandidates(node, baseAbstractMethod))
			{
				if (ResolveAsImplementationOf(baseAbstractMethod, abstractMethodImplementationCandidate))
				{
					return;
				}
			}
			if (!CheckInheritsImplementation(node, baseAbstractMethod) && !AbstractMemberNotImplemented(node, rootBaseType, baseAbstractMethod))
			{
				AddStub(node, base.CodeBuilder.CreateAbstractMethod(rootBaseType.LexicalInfo, baseAbstractMethod));
			}
		}

		private bool ResolveAsImplementationOf(IMethod baseMethod, Method method)
		{
			if (!TypeSystemServices.CheckOverrideSignature(GetEntity(method), baseMethod))
			{
				return false;
			}
			ProcessMethodImplementation(method, baseMethod);
			if (!method.IsOverride && !method.IsVirtual)
			{
				method.Modifiers |= TypeMemberModifiers.Virtual;
			}
			AssertValidInterfaceImplementation(method, baseMethod);
			TraceImplements(method, baseMethod);
			return true;
		}

		private void ProcessMethodImplementation(Method method, IMethod baseMethod)
		{
			IMethod entity = GetEntity(method);
			CallableSignature overriddenSignature = TypeSystemServices.GetOverriddenSignature(baseMethod, entity);
			if (IsUnknown(entity.ReturnType))
			{
				method.ReturnType = base.CodeBuilder.CreateTypeReference(overriddenSignature.ReturnType);
			}
			else if (overriddenSignature.ReturnType != entity.ReturnType)
			{
				Error(CompilerErrorFactory.ConflictWithInheritedMember(method, entity, baseMethod));
			}
		}

		private void TraceImplements(TypeMember member, IEntity baseMember)
		{
			base.Context.TraceInfo("{0}: Member {1} implements {2}", member.LexicalInfo, member, baseMember);
		}

		private static bool IsUnknown(IType type)
		{
			return TypeSystemServices.IsUnknown(type);
		}

		private IEnumerable<Method> GetAbstractMethodImplementationCandidates(TypeDefinition node, IMethod baseMethod)
		{
			return GetAbstractMemberImplementationCandidates<Method>(node, baseMethod);
		}

		private IEnumerable<Property> GetAbstractPropertyImplementationCandidates(TypeDefinition node, IProperty baseProperty)
		{
			return GetAbstractMemberImplementationCandidates<Property>(node, baseProperty);
		}

		private IEnumerable<TMember> GetAbstractMemberImplementationCandidates<TMember>(TypeDefinition node, IMember baseEntity) where TMember : TypeMember, IExplicitMember
		{
			List<TMember> list = new List<TMember>();
			foreach (TypeMember member in node.Members)
			{
				TMember val = member as TMember;
				if (val != null && IsCandidateMemberImplementationFor(baseEntity, member))
				{
					list.Add(val);
				}
			}
			list.Sort(new ExplicitMembersFirstComparer<TMember>());
			return list;
		}

		private bool IsCandidateMemberImplementationFor(IMember baseMember, TypeMember candidate)
		{
			return candidate.Name == baseMember.Name && IsCorrectExplicitMemberImplOrNoExplicitMemberAtAll(candidate, baseMember);
		}

		private bool IsCorrectExplicitMemberImplOrNoExplicitMemberAtAll(TypeMember member, IMember entity)
		{
			ExplicitMemberInfo explicitInfo = ((IExplicitMember)member).ExplicitInfo;
			if (explicitInfo == null)
			{
				return true;
			}
			if (explicitInfo.Entity != null)
			{
				return false;
			}
			return entity.DeclaringType == GetType(explicitInfo.InterfaceType);
		}

		private bool AbstractMemberNotImplemented(ClassDefinition node, TypeReference baseTypeRef, IMember member)
		{
			if (IsValueType(node))
			{
				Error(CompilerErrorFactory.ValueTypeCantHaveAbstractMember(baseTypeRef, GetType(node), member));
				return false;
			}
			if (!node.IsAbstract)
			{
				TypeMember typeMember = base.CodeBuilder.CreateStub(node, member);
				CompilerWarning compilerWarning = null;
				if (null != typeMember)
				{
					compilerWarning = CompilerWarningFactory.AbstractMemberNotImplementedStubCreated(baseTypeRef, GetType(node), member);
					if (typeMember.NodeType != NodeType.Property || null == node.Members[typeMember.Name])
					{
						AddStub(node, typeMember);
					}
				}
				else
				{
					compilerWarning = CompilerWarningFactory.AbstractMemberNotImplemented(baseTypeRef, GetType(node), member);
					_newAbstractClasses.AddUnique(node);
				}
				base.Warnings.Add(compilerWarning);
				return null != typeMember;
			}
			return false;
		}

		private static bool IsValueType(ClassDefinition node)
		{
			return ((IType)node.Entity).IsValueType;
		}

		private void ResolveInterfaceMembers(ClassDefinition node, IType baseType, TypeReference rootBaseType)
		{
			IType[] interfaces = baseType.GetInterfaces();
			foreach (IType baseType2 in interfaces)
			{
				ResolveInterfaceMembers(node, baseType2, rootBaseType);
			}
			foreach (IMember member in baseType.GetMembers())
			{
				if (!_explicitMembers.Contains(member))
				{
					ResolveAbstractMember(node, member, rootBaseType);
				}
			}
		}

		private void ResolveAbstractMembers(ClassDefinition node, IType baseType, TypeReference rootBaseType)
		{
			foreach (IEntity member in baseType.GetMembers())
			{
				switch (member.EntityType)
				{
				case EntityType.Method:
				{
					IMethod method = (IMethod)member;
					if (method.IsAbstract)
					{
						ResolveAbstractMethod(node, method, rootBaseType);
					}
					break;
				}
				case EntityType.Property:
				{
					IProperty property = (IProperty)member;
					if (IsAbstract(property))
					{
						ResolveAbstractProperty(node, property, rootBaseType);
					}
					break;
				}
				case EntityType.Event:
				{
					IEvent @event = (IEvent)member;
					if (@event.IsAbstract)
					{
						ResolveAbstractEvent(node, rootBaseType, @event);
					}
					break;
				}
				}
			}
			ProcessBaseTypes(node, baseType, rootBaseType);
		}

		private static bool IsAbstract(IProperty property)
		{
			return IsAbstractAccessor(property.GetGetMethod()) || IsAbstractAccessor(property.GetSetMethod());
		}

		private static bool IsAbstractAccessor(IMethod accessor)
		{
			return accessor?.IsAbstract ?? false;
		}

		private void ResolveAbstractMember(ClassDefinition node, IMember member, TypeReference rootBaseType)
		{
			switch (member.EntityType)
			{
			case EntityType.Method:
				ResolveAbstractMethod(node, (IMethod)member, rootBaseType);
				break;
			case EntityType.Property:
				ResolveAbstractProperty(node, (IProperty)member, rootBaseType);
				break;
			case EntityType.Event:
				ResolveAbstractEvent(node, rootBaseType, (IEvent)member);
				break;
			default:
				NotImplemented(rootBaseType, "abstract member: " + member);
				break;
			}
		}

		private void ProcessNewAbstractClasses()
		{
			foreach (ClassDefinition newAbstractClass in _newAbstractClasses)
			{
				newAbstractClass.Modifiers |= TypeMemberModifiers.Abstract;
			}
		}

		private void AddStub(TypeDefinition node, TypeMember stub)
		{
			node.Members.Add(stub);
		}

		private void AssertValidInterfaceImplementation(TypeMember node, IMember baseMember)
		{
			if (baseMember.DeclaringType.IsInterface)
			{
				IExplicitMember explicitMember = node as IExplicitMember;
				if ((explicitMember == null || null == explicitMember.ExplicitInfo) && node.Visibility != TypeMemberModifiers.Public)
				{
					base.Errors.Add(CompilerErrorFactory.InterfaceImplementationMustBePublicOrExplicit(node, baseMember));
				}
			}
		}

		public TypeMember Reify(TypeMember node)
		{
			Visit(node);
			Method method = node as Method;
			if (method != null)
			{
				ReifyMethod(method);
				return node;
			}
			Event @event = node as Event;
			if (@event != null)
			{
				ReifyEvent(@event);
				return node;
			}
			Property property = node as Property;
			if (property != null)
			{
				ReifyProperty(property);
			}
			return node;
		}

		private void ReifyProperty(Property property)
		{
			foreach (IProperty item in InheritedAbstractMembersOf(property.DeclaringType).OfType<IProperty>())
			{
				if (IsCandidateMemberImplementationFor(item, property) && ResolveAsImplementationOf(item, property))
				{
					break;
				}
			}
		}

		private void ReifyEvent(Event @event)
		{
			foreach (IEvent item in InheritedAbstractMembersOf(@event.DeclaringType).OfType<IEvent>())
			{
				if (item.Name == @event.Name)
				{
					ProcessEventImplementation(@event, item);
					break;
				}
			}
		}

		private void ReifyMethod(Method method)
		{
			foreach (IMethod item in InheritedAbstractMembersOf(method.DeclaringType).OfType<IMethod>())
			{
				if (IsCandidateMemberImplementationFor(item, method) && ResolveAsImplementationOf(item, method))
				{
					break;
				}
			}
		}

		private IEnumerable<IMember> InheritedAbstractMembersOf(TypeDefinition typeDefinition)
		{
			IType type = GetType(typeDefinition);
			try
			{
				IType[] interfaces = type.GetInterfaces();
				foreach (IType baseType in interfaces)
				{
					foreach (IMember member2 in baseType.GetMembers())
					{
						if (IsAbstract(member2))
						{
							yield return member2;
						}
					}
				}
			}
			finally
			{
			}
			foreach (IMember member in type.BaseType.GetMembers())
			{
				if (IsAbstract(member))
				{
					yield return member;
				}
			}
		}

		private static bool IsAbstract(IMember member)
		{
			return member.EntityType switch
			{
				EntityType.Method => ((IMethod)member).IsAbstract, 
				EntityType.Property => IsAbstract((IProperty)member), 
				EntityType.Event => ((IEvent)member).IsAbstract, 
				_ => false, 
			};
		}
	}
}
