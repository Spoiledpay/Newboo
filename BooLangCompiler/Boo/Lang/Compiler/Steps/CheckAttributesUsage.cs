using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckAttributesUsage : AbstractFastVisitorCompilerStep
	{
		private static Dictionary<Type, AttributeTargets> _nodesUsageTargets;

		public override void OnMemberReferenceExpression(MemberReferenceExpression node)
		{
			base.OnMemberReferenceExpression(node);
			OnReferenceExpression(node);
		}

		public override void OnAttribute(Boo.Lang.Compiler.Ast.Attribute node)
		{
			Type type = AttributeType(node);
			if (!(type == null))
			{
				AttributeUsageAttribute attributeUsageAttribute = AttributeUsageFor(type);
				if (attributeUsageAttribute != null)
				{
					CheckAttributeUsage(node, type, attributeUsageAttribute);
				}
			}
		}

		private void CheckAttributeUsage(Boo.Lang.Compiler.Ast.Attribute node, Type attrType, AttributeUsageAttribute usage)
		{
			CheckAttributeUsageTarget(node, attrType, usage);
			CheckAttributeUsageCardinality(node, attrType, usage);
		}

		private void CheckAttributeUsageTarget(Boo.Lang.Compiler.Ast.Attribute node, Type attrType, AttributeUsageAttribute usage)
		{
			AttributeTargets attributeTargets = ValidAttributeTargetsFor(attrType, usage);
			if (attributeTargets != AttributeTargets.All)
			{
				AttributeTargets? attributeTargets2 = AttributeTargetsFor(node);
				if (attributeTargets2.HasValue && !IsValid(attributeTargets2.Value, attributeTargets))
				{
					base.Errors.Add(CompilerErrorFactory.InvalidAttributeTarget(node, attrType, attributeTargets));
				}
			}
		}

		private static AttributeTargets ValidAttributeTargetsFor(Type attrType, AttributeUsageAttribute usage)
		{
			return IsExtensionAttribute(attrType) ? (usage.ValidOn | AttributeTargets.Property) : usage.ValidOn;
		}

		private void CheckAttributeUsageCardinality(Boo.Lang.Compiler.Ast.Attribute node, Type attrType, AttributeUsageAttribute usage)
		{
			if (!usage.AllowMultiple && HasSiblingAttributesOfSameType(node, attrType))
			{
				MultipleAttributeUsageError(node, attrType);
			}
		}

		private static bool HasSiblingAttributesOfSameType(Boo.Lang.Compiler.Ast.Attribute node, Type attrType)
		{
			return SiblingAttributesOfSameType(node, attrType).Any();
		}

		private static IEnumerable<Boo.Lang.Compiler.Ast.Attribute> SiblingAttributesOfSameType(Boo.Lang.Compiler.Ast.Attribute node, Type attrType)
		{
			INodeWithAttributes nodeWithAttributes = (INodeWithAttributes)node.ParentNode;
			return nodeWithAttributes.Attributes.Where((Boo.Lang.Compiler.Ast.Attribute _) => _ != node && IsAttributeOfType(attrType, _));
		}

		private void MultipleAttributeUsageError(Boo.Lang.Compiler.Ast.Attribute attribute, Type attrType)
		{
			base.Errors.Add(CompilerErrorFactory.MultipleAttributeUsage(attribute, attrType));
		}

		private static bool IsAttributeOfType(Type attrType, Boo.Lang.Compiler.Ast.Attribute attribute)
		{
			IExternalEntity externalEntity = attribute.Entity as IExternalEntity;
			return externalEntity != null && externalEntity.MemberInfo.DeclaringType == attrType;
		}

		private static bool IsExtensionAttribute(Type attrType)
		{
			return attrType == Types.ClrExtensionAttribute;
		}

		private static AttributeUsageAttribute AttributeUsageFor(Type attrType)
		{
			return (AttributeUsageAttribute)System.Attribute.GetCustomAttributes(attrType, typeof(AttributeUsageAttribute)).FirstOrDefault();
		}

		private static AttributeTargets? AttributeTargetsFor(Boo.Lang.Compiler.Ast.Attribute node)
		{
			Method method = node.ParentNode as Method;
			if (method != null)
			{
				AttributeCollection returnTypeAttributes = method.ReturnTypeAttributes;
				if (returnTypeAttributes.Contains(node))
				{
					return AttributeTargets.ReturnValue;
				}
			}
			if (NodeUsageTargets().TryGetValue(node.ParentNode.GetType(), out var value))
			{
				return value;
			}
			return null;
		}

		private static bool IsValid(AttributeTargets target, AttributeTargets validAttributeTargets)
		{
			return target == (validAttributeTargets & target);
		}

		private static Type AttributeType(Boo.Lang.Compiler.Ast.Attribute node)
		{
			return (node.Entity as IExternalEntity)?.MemberInfo.DeclaringType;
		}

		public override void OnReferenceExpression(ReferenceExpression node)
		{
			IMember member = node.Entity as IMember;
			if (member == null)
			{
				return;
			}
			foreach (ObsoleteAttribute item in ObsoleteAttributesIn(member))
			{
				if (item.IsError)
				{
					base.Errors.Add(CompilerErrorFactory.Obsolete(node, member, item.Message));
				}
				else
				{
					base.Warnings.Add(CompilerWarningFactory.Obsolete(node, member, item.Message));
				}
			}
		}

		private static IEnumerable<ObsoleteAttribute> ObsoleteAttributesIn(IMember member)
		{
			IExternalEntity externalEntity = member as IExternalEntity;
			if (externalEntity == null)
			{
				return new ObsoleteAttribute[0];
			}
			return System.Attribute.GetCustomAttributes(externalEntity.MemberInfo, typeof(ObsoleteAttribute)).Cast<ObsoleteAttribute>();
		}

		protected void OnInternalReferenceExpression(ReferenceExpression node)
		{
		}

		private static Dictionary<Type, AttributeTargets> NodeUsageTargets()
		{
			return _nodesUsageTargets ?? (_nodesUsageTargets = NewUsageTargetsDictionary());
		}

		private static Dictionary<Type, AttributeTargets> NewUsageTargetsDictionary()
		{
			Dictionary<Type, AttributeTargets> dictionary = new Dictionary<Type, AttributeTargets>();
			dictionary.Add(typeof(Assembly), AttributeTargets.Assembly);
			dictionary.Add(typeof(Boo.Lang.Compiler.Ast.Module), AttributeTargets.Assembly);
			dictionary.Add(typeof(ClassDefinition), AttributeTargets.Class);
			dictionary.Add(typeof(StructDefinition), AttributeTargets.Struct);
			dictionary.Add(typeof(EnumDefinition), AttributeTargets.Enum);
			dictionary.Add(typeof(Constructor), AttributeTargets.Constructor);
			dictionary.Add(typeof(Method), AttributeTargets.Method);
			dictionary.Add(typeof(Property), AttributeTargets.Property);
			dictionary.Add(typeof(Field), AttributeTargets.Field);
			dictionary.Add(typeof(Event), AttributeTargets.Event);
			dictionary.Add(typeof(InterfaceDefinition), AttributeTargets.Interface);
			dictionary.Add(typeof(ParameterDeclaration), AttributeTargets.Parameter);
			dictionary.Add(typeof(CallableDefinition), AttributeTargets.Delegate);
			dictionary.Add(typeof(GenericParameterDeclaration), AttributeTargets.GenericParameter);
			return dictionary;
		}
	}
}
